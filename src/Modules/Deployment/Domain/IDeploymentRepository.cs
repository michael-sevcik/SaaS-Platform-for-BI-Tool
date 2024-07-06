using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Deployment.Domain;

internal interface IDeploymentRepository
{
    Task<MetabaseDeployment> GetDeploymentAsync(string customerId);
    Task<MetabaseDeployment> CreateDeploymentAsync(MetabaseDeployment deployment);
    Task DeleteDeploymentAsync(string customerId);
}
