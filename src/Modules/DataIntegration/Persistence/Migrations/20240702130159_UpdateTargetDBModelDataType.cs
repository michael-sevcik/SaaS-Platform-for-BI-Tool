using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTargetDBModelDataType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 1,
                column: "TableModel",
                value: "{\"name\":\"Employees\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"Id\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"PersonalID\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"ExternalId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"FirstName\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":false}},{\"name\":\"Lastname\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":false}}],\"description\":null}");

            migrationBuilder.UpdateData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 2,
                column: "TableModel",
                value: "{\"name\":\"Workplaces\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"Id\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"Label\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":false}},{\"name\":\"Name\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":true}}],\"description\":null}");

            migrationBuilder.UpdateData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 3,
                column: "TableModel",
                value: "{\"name\":\"WorkReports\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"ID\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"ID\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":false}},{\"name\":\"OrderId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":true}},{\"name\":\"ProductionOperationId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":true}},{\"name\":\"Quantity\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Decimal\",\"isNullable\":false}},{\"name\":\"ExpectedTime\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Decimal\",\"isNullable\":false}},{\"name\":\"DateTime\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Datetime\",\"isNullable\":false}},{\"name\":\"ProductType\",\"dataType\":{\"type\":\"nVarCharMax\",\"isNullable\":true}},{\"name\":\"WorkerId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":true}},{\"name\":\"WorkPlaceId\",\"dataType\":{\"type\":\"simple\",\"simpleType\":\"Integer\",\"isNullable\":true}}],\"description\":null}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 1,
                column: "TableModel",
                value: "{\"name\":\"Employees\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"PersonalID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"ExternalId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"FirstName\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":false}},{\"name\":\"Lastname\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":false}}],\"description\":null}");

            migrationBuilder.UpdateData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 2,
                column: "TableModel",
                value: "{\"name\":\"Workplaces\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"Id\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"Label\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":false}},{\"name\":\"Name\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":true}}],\"description\":null}");

            migrationBuilder.UpdateData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 3,
                column: "TableModel",
                value: "{\"name\":\"WorkReports\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"ID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"ID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"OrderId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"ProductionOperationId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"Quantity\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Decimal\",\"isNullable\":false}},{\"name\":\"ExpectedTime\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Decimal\",\"isNullable\":false}},{\"name\":\"DateTime\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Datetime\",\"isNullable\":false}},{\"name\":\"ProductType\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":true}},{\"name\":\"WorkerId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"WorkPlaceId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}}],\"description\":null}");
        }
    }
}
