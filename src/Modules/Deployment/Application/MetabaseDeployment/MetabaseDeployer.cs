using BIManagement.Common.Application.ServiceLifetimes;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Domain;
using BIManagement.Modules.Deployment.Domain.Configuration;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;

// used for testing
[assembly:InternalsVisibleTo("BIManagement.Test.Modules.Deployment.Application")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7")]
namespace BIManagement.Modules.Deployment.Application.MetabaseDeployment;

/// <summary>
/// Represents a deployer for Metabase.
/// </summary>
/// <param name="logger">The logger instance.</param>
/// <param name="deploymentRepository">The repository for Metabase deployments.</param>
/// <param name="kubernetesClient">The Kubernetes client.</param>
/// <param name="metabaseConfigurator">The configurator for Metabase.</param>
/// <param name="integrationNotifier">The notifier used for sending metabase deployed notifications.</param>
internal class MetabaseDeployer(
    ILogger<MetabaseDeployer> logger,
    IMetabaseDeploymentRepository deploymentRepository,
    IKubernetes kubernetesClient,
    IMetabaseConfigurator metabaseConfigurator,
    IIntegrationNotifier integrationNotifier,
    IOptions<KubernetesPublicUrlOption> kubernetesPublicUrlOption) : IMetabaseDeployer, IScoped
{
    private const string DefaultNamespace = "default";
    private const string Image = "michaelsevcik/preconfigured-metabase:1.0.0";
    private readonly string baseClusterHostUrl = kubernetesPublicUrlOption.Value.PublicUrl;
    private readonly string internalBaseClusterHostUrl = kubernetesPublicUrlOption.Value.InternalUrl;

    /// <inheritdoc/>
    public async Task<Result<string>> DeployMetabaseAsync(string customerId, DefaultAdminSettings defaultAdminSettings)
    {
        // deploy on random secret url and configure it with the client, then change the url to the desired public one
        string instanceName = $"metabase-{customerId}";
        var tempUrlPath = $"/metabase-config-{Guid.NewGuid()}";

        Domain.MetabaseDeployment deployment = new()
        {
            CustomerId = customerId,
            Image = Image,
            UrlPath = baseClusterHostUrl + tempUrlPath,
            InstanceName = instanceName
        };

        // save the record of the deployment to the repository
        var result = await deploymentRepository.SaveDeploymentAsync(deployment);
        if (result.IsFailure)
        {
            return Result.Failure<string>(result.Error);
        }

        var metabaseId = deployment.Id;
        var publicUrlPath = $"/metabase-{metabaseId}";
        var publicAbsoluteMetabaseUrl = baseClusterHostUrl + publicUrlPath; // TODO: use configuration for this
        deployment.UrlPath = publicAbsoluteMetabaseUrl;
        deployment.InstanceName = instanceName;


        var metabaseV1K8sDeployment = CreateDeployment(instanceName, Image, publicAbsoluteMetabaseUrl);
        var service = CreateService(instanceName);
        var ingress = CreateIngress(instanceName, tempUrlPath);
        result = await DeployMetabase(customerId, metabaseV1K8sDeployment, service, ingress)
            .Bind(() => WaitForDeploymentToBeReady(instanceName, DefaultNamespace))
            .Bind(() => metabaseConfigurator.ConfigureMetabase(customerId, internalBaseClusterHostUrl + tempUrlPath, defaultAdminSettings))
            .Bind(() => ChangeIngressPathAsync(instanceName, publicUrlPath));

        if (result.IsFailure)
        {
            logger.LogError("Failed to deploy Metabase for Customer {CustomerId}", customerId);
            return Result.Failure<string>(result.Error);
        }

        deployment.UrlPath = publicAbsoluteMetabaseUrl;
        result = await deploymentRepository.SaveDeploymentAsync(deployment);
        if (result.IsFailure)
        {
            logger.LogError("Failed to update Metabase deployment information; CustomerId: {customerId}", customerId);
            return Result.Failure<string>(new(
                "Deployment.MetabaseDeployer.MetabaseDeploymentFailed",
                "Failed to update Metabase deployment information"));
        }

        logger.LogInformation("Metabase for Customer {CustomerId} deployed on {urlPart}", customerId, publicAbsoluteMetabaseUrl);

        await integrationNotifier.SentMetabaseDeployedNotification(customerId, publicAbsoluteMetabaseUrl);
        return Result.Success(publicAbsoluteMetabaseUrl);
    }

    private async Task<Result> ChangeIngressPathAsync(string instanceName, string newPath)
    {
        try
        {
            var ingress = await kubernetesClient.NetworkingV1.ReadNamespacedIngressAsync(instanceName, DefaultNamespace);
            if (ingress == null)
            {
                logger.LogCritical("Ingress {InstanceName} not found", instanceName);
                return Result.Failure(new("Deployment.MetabaseDeployer.IngressNotFound", $"Ingress {instanceName} not found."));
            }

            // update the path in the ingress rules
            ingress.Spec.Rules[0].Http.Paths[0].Path = $"{newPath}(/|$)(.*)";

            // apply the updated ingress
            await kubernetesClient.NetworkingV1.ReplaceNamespacedIngressAsync(ingress, instanceName, DefaultNamespace);
            logger.LogInformation("Ingress path for instance {InstanceName} updated to {NewPath}", instanceName, newPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating ingress path for instance '{instanceName}': {ex.Message}");
            return Result.Failure(new("IngressUpdateError", $"Error updating ingress path for instance '{instanceName}': {ex.Message}"));
        }

        return Result.Success();
    }


    private async Task<Result> DeployMetabase(string customerId, V1Deployment metabaseV1K8sDeployment, V1Service service, V1Ingress ingress)
    {
        try
        {
            await kubernetesClient.AppsV1.CreateNamespacedDeploymentAsync(metabaseV1K8sDeployment, DefaultNamespace);
            await kubernetesClient.CoreV1.CreateNamespacedServiceAsync(service, DefaultNamespace);
            await kubernetesClient.NetworkingV1.CreateNamespacedIngressAsync(ingress, DefaultNamespace);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to deploy Metabase");
            var result = await deploymentRepository.DeleteDeploymentAsync(customerId);
            if (result.IsFailure)
            {
                logger.LogError("Failed to clean up Metabase deployment; CustomerId: {customerId}", customerId);
            }

            return Result.Failure(new(
                "Deployment.MetabaseDeployer.MetabaseDeploymentFailed",
                "Failed to deploy Metabase"));
        }

        return Result.Success();
    }

    /// <inheritdoc/>
    public async Task<Result> DeleteDeploymentAsync(string customerId)
    {
        var deployment = await deploymentRepository.GetAsync(customerId);
        if (deployment is null)
        {
            return Result.Failure(new(
                "Deployment.MetabaseDeployer.MetabaseDeploymentNotFound",
                "Metabase deployment not found"));
        }

        try
        {
            await kubernetesClient.AppsV1.DeleteNamespacedDeploymentAsync(deployment.InstanceName, DefaultNamespace);
            await kubernetesClient.CoreV1.DeleteNamespacedServiceAsync(deployment.InstanceName, DefaultNamespace);
            await kubernetesClient.NetworkingV1.DeleteNamespacedIngressAsync(deployment.InstanceName, DefaultNamespace);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Metabase instance {InstanceName}", deployment.InstanceName);
            return Result.Failure(new(
                "Deployment.MetabaseDeployer.MetabaseDeploymentDeleteFailed",
                "Failed to delete Metabase deployment"));
        }

        var result = await deploymentRepository.DeleteDeploymentAsync(customerId);
        if (result.IsFailure)
        {
            logger.LogError("Failed to delete Metabase deployment using the repository; CustomerId: {customerId}", customerId);
            return Result.Failure(new(
                "Deployment.MetabaseDeployer.MetabaseDeploymentDeleteFailed",
                "Failed to delete Metabase deployment"));
        }

        logger.LogInformation("Metabase of customer {customerId} deployment deleted successfully", customerId);
        return Result.Success();
    }

    /// <summary>
    /// Creates a Kubernetes deployment for Metabase.
    /// </summary>
    /// <param name="instanceName">The name of the deployment instance.</param>
    /// <param name="image">The image to use for the deployment.</param>
    /// <param name="metabaseSiteUrl">The URL path for metabase site url environment variable.</param>
    /// <returns>The created Kubernetes deployment.</returns>
    private static V1Deployment CreateDeployment(string instanceName, string image, string metabaseSiteUrl) => new()
    {
        ApiVersion = "apps/v1",
        Kind = "Deployment",
        Metadata = new V1ObjectMeta { Name = instanceName },
        Spec = new V1DeploymentSpec
        {
            Replicas = 1,
            Selector = new V1LabelSelector { MatchLabels = new Dictionary<string, string> { { "app", instanceName } } },
            Template = new V1PodTemplateSpec
            {
                Metadata = new V1ObjectMeta { Labels = new Dictionary<string, string> { { "app", instanceName } } },
                Spec = new V1PodSpec
                {
                    Containers =
                [
                    new V1Container
                        {
                            Name = instanceName,
                            Image = image,
                            Ports = [new(3000)],
                            Env = [new("MB_SITE_URL", metabaseSiteUrl) ]
                        }
                ]
                }
            }
        }
    };

    /// <summary>
    /// Creates a Kubernetes service for Metabase.
    /// </summary>
    /// <param name="instanceName">The name of the service instance.</param>
    /// <returns>The created Kubernetes service.</returns>
    private static V1Service CreateService(string instanceName) => new()
    {
        ApiVersion = "v1",
        Kind = "Service",
        Metadata = new V1ObjectMeta { Name = instanceName },
        Spec = new V1ServiceSpec
        {
            Selector = new Dictionary<string, string> { { "app", instanceName } },
            Ports = [new() { Port = 80, TargetPort = 3000 }]
        }
    };

    /// <summary>
    /// Creates a Kubernetes ingress for Metabase.
    /// </summary>
    /// <param name="instanceName">The name of the ingress instance.</param>
    /// <param name="urlPath">The URL path for the ingress.</param>
    /// <returns>The created Kubernetes ingress.</returns>
    private static V1Ingress CreateIngress(string instanceName, string urlPath) => new()
    {
        ApiVersion = "networking.k8s.io/v1",
        Kind = "Ingress",
        Metadata = new V1ObjectMeta { 
            Name = instanceName,
            NamespaceProperty = DefaultNamespace,
            Annotations = new Dictionary<string, string> { { "nginx.ingress.kubernetes.io/rewrite-target", "/$2" } }
        },
        Spec = new V1IngressSpec
        {
            Rules =
                [
                    new V1IngressRule
                    {
                        Http = new V1HTTPIngressRuleValue
                        {
                            Paths =
                            [
                                new() {
                                    Path = $"{urlPath}(/|$)(.*)",
                                    PathType = "ImplementationSpecific",
                                    Backend = new V1IngressBackend
                                    {
                                        Service = new V1IngressServiceBackend
                                        {
                                            Name = instanceName,
                                            Port = new V1ServiceBackendPort { Number = 80 }
                                        }
                                    }
                                }
                            ]
                        }
                    }
                ],
            IngressClassName = "nginx"
        }
    };

    private async Task<Result> WaitForDeploymentToBeReady(string deploymentName, string namespaceName, int timeoutInSeconds = 300, int checkIntervalInSeconds = 5)
    {
        var deadline = DateTime.Now.AddSeconds(timeoutInSeconds);
        while (DateTime.Now < deadline)
        {
            var deployment = await kubernetesClient.AppsV1.ReadNamespacedDeploymentAsync(deploymentName, namespaceName);
            var status = deployment.Status;
            if (status.AvailableReplicas.HasValue && status.AvailableReplicas.Value == 1)
            {
                return Result.Success();
            }

            await Task.Delay(checkIntervalInSeconds * 1000);
        }

        return Result.Failure(new(
            "Deployment.MetabaseDeployer.MetabaseDeploymentTimeout",
            "Timed out waiting for Metabase deployment to be ready"));
    }
}
