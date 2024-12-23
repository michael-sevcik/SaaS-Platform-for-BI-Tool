﻿using BIManagement.Modules.DataIntegration.Domain.DbModelling;
using System.Text.Json;

namespace BIManagement.Modules.DataIntegration.Domain.Mapping;

/// <summary>
/// Contains the model of the target database.
/// </summary>
public static class TargetDbModel
{
    /// <summary>
    /// The model of the target database.
    /// </summary>
    public static readonly DbModel model = JsonSerializer.Deserialize<DbModel>(SerializedModel, SerializationOptions.Default)
        ?? throw new InvalidOperationException("Model must be deserializable.");

    public const string SerializedModel = """
        {
          "name": "TargetDb",
          "tables": [
            {
              "name": "Employees",
              "schema": "dbo",
              "primaryKeys": [
                {
                  "name": "Id",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": false
                  }
                }
              ],
              "columns": [
                {
                  "name": "Id",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": false
                  }
                },
                {
                  "name": "PersonalID",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": false
                  }
                },
                {
                  "name": "ExternalId",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": false
                  }
                },
                {
                  "name": "FirstName",
                  "dataType": {
                    "type": "nVarCharMax",
                    "isNullable": false
                  }
                },
                {
                  "name": "Lastname",
                  "dataType": {
                    "type": "nVarCharMax",
                    "isNullable": false
                  }
                }
              ],
              "description": null
            },
            {
              "name": "Workplaces",
              "schema": "dbo",
              "primaryKeys": [
                {
                  "name": "Id",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": false
                  }
                }
              ],
              "columns": [
                {
                  "name": "Id",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": false
                  }
                },
                {
                  "name": "Label",
                  "dataType": {
                    "type": "nVarCharMax",
                    "isNullable": false
                  }
                },
                {
                  "name": "Name",
                  "dataType": {
                    "type": "nVarCharMax",
                    "isNullable": true
                  }
                }
              ],
              "description": null
            },
            {
              "name": "WorkReports",
              "schema": "dbo",
              "primaryKeys": [
                {
                  "name": "ID",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": false
                  }
                }
              ],
              "columns": [
                {
                  "name": "ID",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": false
                  }
                },
                {
                  "name": "OrderId",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": true
                  }
                },
                {
                  "name": "ProductionOperationId",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": true
                  }
                },
                {
                  "name": "Quantity",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Decimal",
                    "isNullable": false
                  }
                },
                {
                  "name": "ExpectedTime",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Decimal",
                    "isNullable": false
                  }
                },
                {
                  "name": "DateTime",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Datetime",
                    "isNullable": false
                  }
                },
                {
                  "name": "ProductType",
                  "dataType": {
                    "type": "nVarCharMax",
                    "isNullable": true
                  }
                },
                {
                  "name": "WorkerId",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": true
                  }
                },
                {
                  "name": "WorkPlaceId",
                  "dataType": {
                    "type": "simple",
                    "simpleType": "Integer",
                    "isNullable": true
                  }
                }
              ],
              "description": null
            }
          ]
        }
        """;
}
