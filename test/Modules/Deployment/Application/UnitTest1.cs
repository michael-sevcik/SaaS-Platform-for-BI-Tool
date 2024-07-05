using System;
using System.Threading.Tasks;
using k8s;
using k8s.Models;

namespace BIManagement.Test.Modules.Deployment.Application;
public class MetabaseDeployer(IKubernetes kubernetesClient)
{
    public async Task DeployMetabaseInstanceAsync(string instanceName, string urlPath, string image = "metabase/metabase:v0.41.4")
    {
        var deployment = CreateDeployment(instanceName, image, urlPath);
        var service = CreateService(instanceName);
        var ingress = CreateIngress(instanceName, urlPath);

        var deployedMetabase = await kubernetesClient.AppsV1.CreateNamespacedDeploymentAsync(deployment, "default", cancellationToken: default);
        await kubernetesClient.CoreV1.CreateNamespacedServiceAsync(service, "default");
        await kubernetesClient.NetworkingV1.CreateNamespacedIngressAsync(ingress, "default");
    }

    private static V1Deployment CreateDeployment(string instanceName, string image, string urlPath) => new()
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

class Program
{
    static async Task Main(string[] args)
    {
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        var kubernetesClient = new Kubernetes(config);

        var deployer = new MetabaseDeployer(kubernetesClient);

        string instanceName = "metabase-instance";
        string urlPath = "/metabase-instance";

        await deployer.DeployMetabaseInstanceAsync(instanceName, urlPath);

        Console.WriteLine($"Deployed {instanceName} accessible at {urlPath}");
    }
}



public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task Test1()
    {
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        var kubernetesClient = new   Kubernetes(config);

        var deployer = new MetabaseDeployer(kubernetesClient);

        string instanceName = "metabase-instance5";
        string urlPath = "/metabase5";

        await deployer.DeployMetabaseInstanceAsync(instanceName, urlPath);

        Console.WriteLine($"Deployed {instanceName} accessible at {urlPath}");
    }
}