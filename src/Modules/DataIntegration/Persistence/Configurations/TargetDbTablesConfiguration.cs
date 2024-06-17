using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Domain.Mapping;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="TargetDbTable"/>
/// </summary>
internal sealed class TargetDbTablesConfiguration : IEntityTypeConfiguration<TargetDbTable>
{
    /// <inheritdoc/>
    public void Configure(EntityTypeBuilder<TargetDbTable> builder)
    {
        builder.ToTable(TableNames.TargetDbTables);
        builder.HasKey(x => x.Id);

        builder.Property(x => x.TableModel)
            .HasConversion(
                v => JsonSerializer.Serialize(v, SerializationOptions.Default),
                v => JsonSerializer.Deserialize<Table>(v, SerializationOptions.Default) // TODO: HANDLE NULL
            );

        List<TargetDbTable> targetTables = new();
        int id = 1;
        foreach (var table in TargetDbModel.model.Tables)
        {
            targetTables.Add(new TargetDbTable()
            {
                Id = id++,
                TableModel = table,
                TableName = table.Name,
                Schema = table.Schema,
            });
        }

        builder.HasData(targetTables);
    }
}
