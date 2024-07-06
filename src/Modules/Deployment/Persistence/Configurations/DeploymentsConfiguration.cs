using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.Deployment.Domain;
using BIManagement.Modules.Deployment.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.Deployment.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="CostumerDbModel"/>
/// </summary>
internal class DeploymentsConfiguration : IEntityTypeConfiguration<MetabaseDeployment>
{
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MetabaseDeployment> builder)
    {
        builder.ToTable(TableNames.Deployments);
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CustomerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);
        builder.Property(builder => builder.UrlPath).HasMaxLength(255);
        builder.HasAlternateKey(x => x.CustomerId);
    }
}
