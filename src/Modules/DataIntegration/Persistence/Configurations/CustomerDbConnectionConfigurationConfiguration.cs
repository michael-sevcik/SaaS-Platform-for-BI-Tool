using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="CustomerDbConnectionConfiguration"/>
/// </summary>
internal sealed class CustomerDbConnectionConfigurationConfiguration : IEntityTypeConfiguration<CustomerDbConnectionConfiguration>
{
    public void Configure(EntityTypeBuilder<CustomerDbConnectionConfiguration> builder)
    {
        builder.ToTable(TableNames.DatabaseConnectionConfigurations);
        builder.HasKey(x => x.CustomerId);
        builder.Property(x => x.CustomerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);
    }
}
