using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ChangeDataIntegrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dataIntegration");

            migrationBuilder.RenameTable(
                name: "DatabaseConnectionConfigurations",
                schema: "DataIntegration",
                newName: "DatabaseConnectionConfigurations",
                newSchema: "dataIntegration");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DataIntegration");

            migrationBuilder.RenameTable(
                name: "DatabaseConnectionConfigurations",
                schema: "dataIntegration",
                newName: "DatabaseConnectionConfigurations",
                newSchema: "DataIntegration");
        }
    }
}
