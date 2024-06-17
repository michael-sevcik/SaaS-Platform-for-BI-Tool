using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetDbTablesAndSchemaMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SchemaMappings",
                schema: "dataIntegration",
                columns: table => new
                {
                    CostumerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    TargetDbTableId = table.Column<int>(type: "int", nullable: false),
                    Mapping = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchemaMappings", x => new { x.CostumerId, x.TargetDbTableId });
                });

            migrationBuilder.CreateTable(
                name: "TargetDbTables",
                schema: "dataIntegration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Schema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TableModel = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetDbTables", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SchemaMappings",
                schema: "dataIntegration");

            migrationBuilder.DropTable(
                name: "TargetDbTables",
                schema: "dataIntegration");
        }
    }
}
