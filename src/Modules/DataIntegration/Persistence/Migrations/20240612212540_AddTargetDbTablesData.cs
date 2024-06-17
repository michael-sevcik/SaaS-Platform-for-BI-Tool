using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTargetDbTablesData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                columns: new[] { "Id", "Schema", "TableModel", "TableName" },
                values: new object[,]
                {
                    { 1, "dbo", "{\"name\":\"Employees\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"PersonalID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"ExternalId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"FirstName\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":false}},{\"name\":\"Lastname\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":false}}],\"description\":null}", "Employees" },
                    { 2, "dbo", "{\"name\":\"Workplaces\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"Label\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":false}},{\"name\":\"Name\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":true}}],\"description\":null}", "Workplaces" },
                    { 3, "dbo", "{\"name\":\"WorkReports\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"ID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"ID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"OrderId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"ProductionOperationId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"Quantity\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Decimal\",\"isNullable\":false}},{\"name\":\"ExpectedTime\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Decimal\",\"isNullable\":false}},{\"name\":\"DateTime\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Datetime\",\"isNullable\":false}},{\"name\":\"Valid\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Boolean\",\"isNullable\":false}},{\"name\":\"ProductTypeID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"WorkerId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"WorkPlaceId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}}],\"description\":null}", "WorkReports" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
