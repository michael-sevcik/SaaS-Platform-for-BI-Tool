using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDbConnectionConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DataIntegration");

            migrationBuilder.CreateTable(
                name: "DatabaseConnectionConfigurations",
                schema: "DataIntegration",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Provider = table.Column<int>(type: "int", nullable: false),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DatabaseConnectionConfigurations", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DatabaseConnectionConfigurations",
                schema: "DataIntegration");
        }
    }
}
