using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BIManagement.Common.Persistence.Constants;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the <see cref="DbConnectionConfiguration"/>
/// </summary>
internal sealed class DbConnectionConfigurationConfiguration : IEntityTypeConfiguration<DbConnectionConfiguration>
{
    public void Configure(EntityTypeBuilder<DbConnectionConfiguration> builder)
    {
        builder.ToTable(TableNames.DatabaseConnectionConfigurations);
        builder.HasKey(x => x.CostumerId);
        builder.Property(x => x.CostumerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);
    }
}
