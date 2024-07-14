using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.Deployment.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMetabaseDeployments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "deployment");

            migrationBuilder.CreateTable(
                name: "MetabaseDeployments",
                schema: "deployment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    UrlPath = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstanceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetabaseDeployments", x => x.Id);
                    table.UniqueConstraint("AK_MetabaseDeployments_CustomerId", x => x.CustomerId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MetabaseDeployments_CustomerId",
                schema: "deployment",
                table: "MetabaseDeployments",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetabaseDeployments",
                schema: "deployment");
        }
    }
}
