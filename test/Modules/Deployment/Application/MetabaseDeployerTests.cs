using System;
using System.Threading.Tasks;
using k8s;
using k8s.Models;

namespace BIManagement.Test.Modules.Deployment.Application;
public class MetabaseDeployer(IKubernetes kubernetesClient)
{
    // TODO: RETURN Task<Result> instead of void
    // TODO: DEPLOY to a specific namespace - metabase
    public async Task DeployMetabaseInstanceAsync(string instanceName, string urlPath, string image = "metabase/metabase:v0.41.4")
    {
        var deployment = CreateDeployment(instanceName, image, urlPath);
        var service = CreateService(instanceName);
        var ingress = CreateIngress(instanceName, urlPath);

        // TODO: catch exceptions and log them
        await kubernetesClient.AppsV1.CreateNamespacedDeploymentAsync(deployment, "default", cancellationToken: default);
        await kubernetesClient.CoreV1.CreateNamespacedServiceAsync(service, "default");
        await kubernetesClient.NetworkingV1.CreateNamespacedIngressAsync(ingress, "default");
    }

    public async Task DeleteMetabaseInstanceAsync(string instanceName)
    {
        var namespaceName = "default";

        //try
        {
            await kubernetesClient.AppsV1.DeleteNamespacedDeploymentAsync(instanceName, namespaceName);
            await kubernetesClient.CoreV1.DeleteNamespacedServiceAsync(instanceName, namespaceName);
            await kubernetesClient.NetworkingV1.DeleteNamespacedIngressAsync(instanceName, namespaceName);
            // TODO: REPLACE writelines with logging
            Console.WriteLine($"Deleted Metabase instance '{instanceName}' successfully.");
        }
        //catch (Exception ex)
        {
            //Console.WriteLine($"Error deleting Metabase instance '{instanceName}': {ex.Message}");
        }
    }

    private static V1Deployment CreateDeployment(string instanceName, string image, string urlPath) => new()
    {
        //TODO: ADD host
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
                            Env = [new("MB_SITE_URL", $"http://localhost{urlPath}") ] // TODO: REPLACE localhost
                        }
                    ]
                }
            }
        }
    };

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

    private static V1Ingress CreateIngress(string instanceName, string urlPath) => new()
    {
        ApiVersion = "networking.k8s.io/v1",
        Kind = "Ingress",
        Metadata = new V1ObjectMeta { Name = instanceName },
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
                                    Path = urlPath,
                                    PathType = "Prefix",
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
}

public class MetabaseDeployerTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        var kubernetesClient = new Kubernetes(config);

        var deployer = new MetabaseDeployer(kubernetesClient);

        string instanceName = "metabase-instance5";
        string urlPath = "/metabase5";

        await deployer.DeployMetabaseInstanceAsync(instanceName, urlPath);
            
        Console.WriteLine($"Deployed {instanceName} accessible at {urlPath}");
    }

    [Test]
    public async Task DeleteMetabaseInstance_Should_DeleteMetabaseInstance()
    {
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        var kubernetesClient = new Kubernetes(config);

        var deployer = new MetabaseDeployer(kubernetesClient);

        string instanceName = "metabase-instance2";
        string urlPath = "/metabase5";

        await deployer.DeleteMetabaseInstanceAsync(instanceName);

        Console.WriteLine($"Deleted {instanceName} accessible at {urlPath}");
    }
}