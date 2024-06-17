using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="CostumerDbModel"/>
/// </summary>
internal sealed class SchemaMappingsConfiguration : IEntityTypeConfiguration<SchemaMapping>
{
    public void Configure(EntityTypeBuilder<SchemaMapping> builder)
    {
        builder.ToTable(TableNames.SchemaMappings);
        builder.HasKey(x => new { x.CostumerId, x.TargetDbTableId });
        builder.Property(x => x.CostumerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);
        builder.Property(x => x.TargetDbTableId).ValueGeneratedNever();
    }
}
