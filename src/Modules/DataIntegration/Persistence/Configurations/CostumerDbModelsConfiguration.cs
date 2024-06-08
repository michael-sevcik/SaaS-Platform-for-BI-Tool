﻿using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="CostumerDbModel"/>
/// </summary>
internal sealed class CostumerDbModelsConfiguration : IEntityTypeConfiguration<CostumerDbModel>
{
    private static readonly JsonSerializerOptions jsonSerializerOptions = new ();
    public void Configure(EntityTypeBuilder<CostumerDbModel> builder)
    {
        builder.ToTable(TableNames.CostumerDbModels);
        builder.HasKey(x => x.CostumerId);
        builder.Property(x => x.CostumerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);

        //builder.ComplexProperty(x => x.DbModel);

        builder.Property(x => x.DbModel)
            .HasConversion(
                v => JsonSerializer.Serialize(v, jsonSerializerOptions),
                v => JsonSerializer.Deserialize<DbModel>(v, jsonSerializerOptions) // TODO: HANDLE NULL
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

        // TODO: TEST
    }
}
