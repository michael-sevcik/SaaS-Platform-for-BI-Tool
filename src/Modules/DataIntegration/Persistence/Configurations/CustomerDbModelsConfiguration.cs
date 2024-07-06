using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.DataIntegration.Domain;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Text.Json;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="CustomerDbModel"/>
/// </summary>
internal sealed class CustomerDbModelsConfiguration : IEntityTypeConfiguration<CustomerDbModel>
{
    public void Configure(EntityTypeBuilder<CustomerDbModel> builder)
    {
        builder.ToTable(TableNames.CustomerDbModels);
        builder.HasKey(x => x.CustomerId);
        builder.Property(x => x.CustomerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);

        //builder.ComplexProperty(x => x.DbModel);

        builder.Property(x => x.DbModel)
            .HasConversion(
                v => JsonSerializer.Serialize(v, SerializationOptions.Default),
                v => JsonSerializer.Deserialize<DbModel>(v, SerializationOptions.Default) // TODO: HANDLE NULL
            );

        //builder.OwnsOne(x => x.DbModel, ownedDbModelBuilder =>
        //{
        //    ownedDbModelBuilder.ToJson();
        //    ownedDbModelBuilder.OwnsMany(x => x.Tables, ownedTablesBuilder =>
        //    {
        //        ownedDbModelBuilder.ToJson();
        //        ownedTablesBuilder.OwnsMany(x => x.Columns, ownedColumnsBuilder =>
        //        {
        //            ownedColumnsBuilder.ToJson();
        //        });

        //        ownedTablesBuilder.OwnsMany(x => x.PrimaryKeys, ownedPrimaryKeysBuilder =>
        //        {
        //            ownedPrimaryKeysBuilder.ToJson();
        //        });
        //    });
        //});
    }
}
