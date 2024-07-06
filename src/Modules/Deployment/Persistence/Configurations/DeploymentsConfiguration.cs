using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.Deployment.Domain;
using BIManagement.Modules.Deployment.Persistence.Constants;
using Microsoft.EntityFrameworkCore;

namespace BIManagement.Modules.Deployment.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="CostumerDbModel"/>
/// </summary>
internal class DeploymentsConfiguration : IEntityTypeConfiguration<MetabaseDeployment>
{
    /// <inheritdoc/>
    public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MetabaseDeployment> builder)
    {
        builder.ToTable(TableNames.MetabaseDeployments);
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => x.CustomerId);
        builder.HasIndex(x => x.CustomerId);
        builder.Property(x => x.CustomerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);
        builder.Property(x => x.UrlPath).HasMaxLength(PropertyConstants.UrlPrefixMaxLength);
    }
}
