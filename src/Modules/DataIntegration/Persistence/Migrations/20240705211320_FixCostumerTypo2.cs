using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixCostumerTypo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CostumerDbModels",
                schema: "dataIntegration",
                table: "CostumerDbModels");

            migrationBuilder.RenameTable(
                name: "CostumerDbModels",
                schema: "dataIntegration",
                newName: "CustomerDbModels",
                newSchema: "dataIntegration");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CustomerDbModels",
                schema: "dataIntegration",
                table: "CustomerDbModels",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CustomerDbModels",
                schema: "dataIntegration",
                table: "CustomerDbModels");

            migrationBuilder.RenameTable(
                name: "CustomerDbModels",
                schema: "dataIntegration",
                newName: "CostumerDbModels",
                newSchema: "dataIntegration");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CostumerDbModels",
                schema: "dataIntegration",
                table: "CostumerDbModels",
                column: "CustomerId");
        }
    }
}
