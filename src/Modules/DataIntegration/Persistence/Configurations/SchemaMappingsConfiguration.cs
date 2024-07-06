using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="CustomerDbModel"/>
/// </summary>
internal sealed class SchemaMappingsConfiguration : IEntityTypeConfiguration<SchemaMapping>
{
    public void Configure(EntityTypeBuilder<SchemaMapping> builder)
    {
        builder.ToTable(TableNames.SchemaMappings);
        builder.HasKey(x => new { x.CustomerId, x.TargetDbTableId });
        builder.Property(x => x.CustomerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);
        builder.Property(x => x.TargetDbTableId).ValueGeneratedNever();
    }
}
