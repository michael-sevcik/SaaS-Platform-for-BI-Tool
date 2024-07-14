using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCostumerTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CostumerId",
                schema: "dataIntegration",
                table: "SchemaMappings",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "CostumerId",
                schema: "dataIntegration",
                table: "DatabaseConnectionConfigurations",
                newName: "CustomerId");

            migrationBuilder.RenameColumn(
                name: "CostumerId",
                schema: "dataIntegration",
                table: "CostumerDbModels",
                newName: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "dataIntegration",
                table: "SchemaMappings",
                newName: "CostumerId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "dataIntegration",
                table: "DatabaseConnectionConfigurations",
                newName: "CostumerId");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                schema: "dataIntegration",
                table: "CostumerDbModels",
                newName: "CostumerId");
        }
    }
}
