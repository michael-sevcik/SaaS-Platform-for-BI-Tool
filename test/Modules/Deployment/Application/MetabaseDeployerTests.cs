using System;
using System.Text;
using System.Threading.Tasks;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Application.MetabaseDeployment;
using BIManagement.Modules.Deployment.Domain;
using BIManagement.Modules.Deployment.Domain.Configuration;
using DotNet.Testcontainers.Builders;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Testcontainers.K3s;

namespace BIManagement.Test.Modules.Deployment.Application;

public class MetabaseDeployerTests
{
    private MockRepository mockRepository;

    private Mock<ILogger<MetabaseDeployer>> mockLogger;
    private Mock<IMetabaseDeploymentRepository> mockDeploymentRepository;
    private Mock<IMetabaseConfigurator> mockMetabaseConfigurator;
    private K3sContainer k3sContainer;
    private IKubernetes kubernetesClient;

    [SetUp]
    public async Task SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Default);

        k3sContainer = new K3sBuilder()
          .WithImage("ingress-nginx-k3s:latest")
          .WithWaitStrategy(Wait.ForUnixContainer()
            .UntilMessageIsLogged("Ingress NGINX is running successfully!"))
          //.WithPortBinding(80, true)
          //.WithPortBinding(443, true)
          .Build();


        await k3sContainer.StartAsync();

        var config = await k3sContainer.GetKubeconfigAsync();

        var byteArray = Encoding.UTF8.GetBytes(config);
        using var memoryStream = new MemoryStream(byteArray);

        var k8sConfig = await KubernetesClientConfiguration.LoadKubeConfigAsync(memoryStream);
        var k8sClientConfig = KubernetesClientConfiguration.BuildConfigFromConfigObject(k8sConfig);
        kubernetesClient = new Kubernetes(k8sClientConfig);

        this.mockLogger = this.mockRepository.Create<ILogger<MetabaseDeployer>>();
        this.mockLogger.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()))
            .Verifiable();

        this.mockDeploymentRepository = this.mockRepository.Create<IMetabaseDeploymentRepository>();
        this.mockMetabaseConfigurator = this.mockRepository.Create<IMetabaseConfigurator>();
    }

    [TearDown]
    public async Task TearDown()
    {
        await k3sContainer.StopAsync();
        await k3sContainer.DisposeAsync();
        kubernetesClient.Dispose();
    }

    [Test]
    public async Task DeployMetabaseInstance_Should_DeployMetabase()
    {
        var customerId = "customer5";
        this.mockDeploymentRepository.Setup(x => x.DeleteDeploymentAsync(customerId))
           .ReturnsAsync(Result.Success())
           .Verifiable();

        this.mockDeploymentRepository.Setup(x => x.SaveDeploymentAsync(It.IsAny<MetabaseDeployment>()))
            .ReturnsAsync(Result.Success())
            .Callback<MetabaseDeployment>(deployment => deployment.Id = 11)
            .Verifiable();

        DefaultAdminSettings defaultAdminSettings = new("admin@admin.cz", "Heski2*");

        this.mockMetabaseConfigurator.Setup(x => x.ConfigureMetabase(customerId, It.IsAny<string>(), defaultAdminSettings))
            .ReturnsAsync(Result.Success())
            .Verifiable();

        var deployer = new MetabaseDeployer(
            mockLogger.Object,
            mockDeploymentRepository.Object,
            kubernetesClient,
            mockMetabaseConfigurator.Object);


        var result = await deployer.DeployMetabaseAsync(customerId, defaultAdminSettings);

        Assert.That(result.IsSuccess, result.Error.Message);
        this.mockMetabaseConfigurator.Verify();
        var pods = await this.kubernetesClient.CoreV1.ListNamespacedPodAsync("default");
        Assert.That(pods.Items.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteMetabaseInstance_Should_DeleteMetabaseInstance()
    {
        var customerId = "Customer2";
        this.mockDeploymentRepository.Setup(x => x.GetAsync(customerId))
            .Returns(Task.FromResult<MetabaseDeployment?>(new MetabaseDeployment() 
            { 
                CustomerId = customerId,
                Id = 15,
                InstanceName = "metabase-customer3",
            }))
            .Verifiable();

        this.mockDeploymentRepository.Setup(x => x.DeleteDeploymentAsync(customerId))
            .ReturnsAsync(Result.Success())
            .Verifiable();

        var deployer = new MetabaseDeployer(
            mockLogger.Object,
            mockDeploymentRepository.Object,
            kubernetesClient,
            mockMetabaseConfigurator.Object);

        var result = await deployer.DeleteDeploymentAsync(customerId);

        Assert.That(result.IsSuccess, result.Error.Message);
        Mock.Verify(mockDeploymentRepository, mockDeploymentRepository);
    }
}