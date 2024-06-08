using BIManagement.Common.Persistence.Constants;
using BIManagement.Modules.DataIntegration.Domain.DatabaseConnection;
using BIManagement.Modules.DataIntegration.Domain.DbModeling;
using BIManagement.Modules.DataIntegration.Persistence.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIManagement.Modules.DataIntegration.Persistence.Configurations;

/// <summary>
/// Represents the EF core configuration for <see cref="CostumerDbModel"/>
/// </summary>
internal sealed class CostumerDbModelsConfiguration : IEntityTypeConfiguration<CostumerDbModel>
{
    public void Configure(EntityTypeBuilder<CostumerDbModel> builder)
    {
        builder.ToTable(TableNames.CostumerDbModels);
        builder.HasKey(x => x.CostumerId);
        builder.Property(x => x.CostumerId)
            .ValueGeneratedNever()
            .HasMaxLength(PropertyConstants.UserIdMaxLength);

        builder.Property(x => x.DbModel).HasJsonPropertyName("DbModel");
        builder.OwnsOne(x => x.DbModel, ownedNavigationBuilder =>
        {
            ownedNavigationBuilder.ToJson();
        });

        // TODO: TEST
    }
}
