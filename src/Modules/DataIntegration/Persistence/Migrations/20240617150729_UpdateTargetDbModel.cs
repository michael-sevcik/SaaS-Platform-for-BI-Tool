using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BIManagement.Modules.DataIntegration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTargetDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 3,
                column: "TableModel",
                value: "{\"name\":\"WorkReports\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"ID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"ID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"OrderId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"ProductionOperationId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"Quantity\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Decimal\",\"isNullable\":false}},{\"name\":\"ExpectedTime\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Decimal\",\"isNullable\":false}},{\"name\":\"DateTime\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Datetime\",\"isNullable\":false}},{\"name\":\"ProductType\",\"dataType\":{\"$type\":\"nVarCharMax\",\"isNullable\":true}},{\"name\":\"WorkerId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"WorkPlaceId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}}],\"description\":null}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "dataIntegration",
                table: "TargetDbTables",
                keyColumn: "Id",
                keyValue: 3,
                column: "TableModel",
                value: "{\"name\":\"WorkReports\",\"schema\":\"dbo\",\"primaryKeys\":[{\"name\":\"ID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}}],\"columns\":[{\"name\":\"ID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"OrderId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"ProductionOperationId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"Quantity\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Decimal\",\"isNullable\":false}},{\"name\":\"ExpectedTime\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Decimal\",\"isNullable\":false}},{\"name\":\"DateTime\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Datetime\",\"isNullable\":false}},{\"name\":\"Valid\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Boolean\",\"isNullable\":false}},{\"name\":\"ProductTypeID\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":false}},{\"name\":\"WorkerId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}},{\"name\":\"WorkPlaceId\",\"dataType\":{\"$type\":\"simple\",\"type\":\"Integer\",\"isNullable\":true}}],\"description\":null}");
        }
    }
}
