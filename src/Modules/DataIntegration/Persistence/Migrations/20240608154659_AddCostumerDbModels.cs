using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCostumerDbModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CostumerDbModels",
                schema: "dataIntegration",
                columns: table => new
                {
                    CostumerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    DbModel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostumerDbModels", x => x.CostumerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CostumerDbModels",
                schema: "dataIntegration");
        }
    }
}
