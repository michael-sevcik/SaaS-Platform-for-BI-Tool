using System;
using System.Threading.Tasks;
using BIManagement.Common.Shared.Results;
using BIManagement.Modules.Deployment.Application.MetabaseDeployment;
using BIManagement.Modules.Deployment.Domain;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace BIManagement.Test.Modules.Deployment.Application;

public class MetabaseDeployerTests
{
    private MockRepository mockRepository;

    private Mock<ILogger<MetabaseDeployer>> mockLogger;
    private Mock<IMetabaseDeploymentRepository> mockDeploymentRepository;

    [SetUp]
    public void SetUp()
    {
        this.mockRepository = new MockRepository(MockBehavior.Default);

        this.mockLogger = this.mockRepository.Create<ILogger<MetabaseDeployer>>();
        this.mockLogger.Setup(x => x.Log(
            It.IsAny<LogLevel>(),
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()))
            .Verifiable();

        this.mockDeploymentRepository = this.mockRepository.Create<IMetabaseDeploymentRepository>();
    }

    [Test]
    public async Task Test1()
    {
        var customerId = "Customer2";
        this.mockDeploymentRepository.Setup(x => x.DeleteDeploymentAsync(customerId))
           .ReturnsAsync(Result.Success())
           .Verifiable();

        this.mockDeploymentRepository.Setup(x => x.SaveDeploymentAsync(It.IsAny<MetabaseDeployment>()))
            .ReturnsAsync(Result.Success())
            .Callback<MetabaseDeployment>(deployment => deployment.Id = 15)
            .Verifiable();

        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        var kubernetesClient = new Kubernetes(config);

        var deployer = new MetabaseDeployer(
            mockLogger.Object,
            mockDeploymentRepository.Object,
            kubernetesClient);

        await deployer.DeployMetabaseAsync(customerId);
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
                Image = "metabase/metabase:v0.50.10",
                InstanceName = "metabase-15",
                UrlPath = "/metabase-15"
            }))
            .Verifiable();

        this.mockDeploymentRepository.Setup(x => x.DeleteDeploymentAsync(customerId))
            .ReturnsAsync(Result.Success())
            .Verifiable();

        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile();
        var kubernetesClient = new Kubernetes(config);

        var deployer = new MetabaseDeployer(
            mockLogger.Object,
            mockDeploymentRepository.Object,
            kubernetesClient);

        string instanceName = "metabase-instance5";
        string urlPath = "/metabase5";

        await deployer.DeleteDeploymentAsync(customerId);
        Mock.Verify(mockDeploymentRepository, mockDeploymentRepository);
    }
}