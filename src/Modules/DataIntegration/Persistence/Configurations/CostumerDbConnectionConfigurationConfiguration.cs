using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the <see cref="CostumerDbConnectionConfiguration"/>
/// </summary>
internal sealed class CostumerDbConnectionConfigurationConfiguration : IEntityTypeConfiguration<CostumerDbConnectionConfiguration>
{
    public void Configure(EntityTypeBuilder<CostumerDbConnectionConfiguration> builder)
    {
        builder.ToTable(TableNames.DatabaseConnectionConfigurations);
        builder.HasKey(x => x.CostumerId);
        builder.Property(x => x.CostumerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);
    }
}
