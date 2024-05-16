import { plainToInstance } from "class-transformer";
import { MappingModelConvertor } from "../src/mappingModel/converting/mappingModelConverter";
import { DbConnectionConfig } from "../src/mappingModel/dbConnectionConfig";
import { PlainToSourceEntityConvertor } from "../src/mappingModel/converting/deserialization/plainToSourceEntityConvertor";

// TODO: shares data with the serialization tests - consider separating it to a shared file.

/** 
 * RUN TESTS
**/

describe('DbConnectionConfig deserialization', () => {
    it('should deserialize a DbConnectionConfig', () => {
        const jsonText = `
        {
            "databaseProvider" : "SQL Server",
            "server" : "server",
            "initialCatalogue" : "db"
        }
        `;
        
        // deserialize the text and convert the plain entity to a class instance
        const deserialized = plainToInstance(DbConnectionConfig, JSON.parse(jsonText));
        expect(JSON.stringify(deserialized)).toBe('{"databaseProvider":"SQL Server","server":"server","initialCatalogue":"db"}')
    })
});

describe('Join condition deserialization', () => {
    it('should convert a plain entity to JoinCondition instance', () => {
        const jsonText = `
        {
            "leftColumn" : {
                "sourceEntity" : {
                    "$id" : "1",
                    "type" : "sourceTable",
                    "name" : "TabMzdList",
                    "selectedColumns" : [
                        "ZamestnanecId", "OdpracHod", "IdObdobi"
                    ]
                },
                "sourceColumn" : "IdObdobi"
            },
            "rightColumn" : {
                "sourceEntity" : {
                    "$id" : "2",
                    "type" : "sourceTable",
                    "name" : "TabMzdObd",
                    "selectedColumns" : [
                        "MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi"
                    ]
                },
                "sourceColumn" : "IdObdobi"
            },
            "relation" : "equal",
            "linkedCondition" : null
        }
        `;
        
        // deserialize the text and convert the plain entity to a class instance
        const sourceEntityConvertor = new PlainToSourceEntityConvertor();
        const deserialized = sourceEntityConvertor.convertToJoinCondition(JSON.parse(jsonText));

        // asert
        const expectedResult = `
        {
            "relation": "equal",
            "leftColumn": {
               "sourceColumn": "IdObdobi",
               "sourceEntity": {
                  "name": "TabMzdList",
                  "selectedColumns": [
                     "ZamestnanecId",
                     "OdpracHod",
                     "IdObdobi"
                  ]
               }
            },
            "rightColumn": {
               "sourceColumn": "IdObdobi",
               "sourceEntity": {
                  "name": "TabMzdObd",
                  "selectedColumns": [
                     "MzdObd_DatumOd",
                     "MzdObd_DatumDo",
                     "IdObdobi"
                  ]
               }
            }
         }`;

        expect(JSON.stringify(deserialized)).toBe('{"relation":"equal","leftColumn":{"sourceColumn":"IdObdobi","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}},"rightColumn":{"sourceColumn":"IdObdobi","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}}}');
    })
});

describe('Join deserialization', () => {
    it('should convert a plain entity to a Join instance', () => {
        const jsonText = `
        {
            "$id" : "3",
            "type" : "join",
            "name" : "3",
            "joinType" : "inner",
            "leftSourceEntity" : {
                "$id" : "1",
                "type" : "sourceTable",
                "name" : "TabMzdList",
                "selectedColumns" : [
                    "ZamestnanecId", "OdpracHod", "IdObdobi"
                ]
            },
            "rightSourceEntity" : {
                "$id" : "2",
                "type" : "sourceTable",
                "name" : "TabMzdObd",
                "selectedColumns" : [
                    "MzdObd_DatumOd", "MzdObd_DatumDo", "IdObdobi"
                ]
            },
            "joinCondition" : {
                    "leftColumn" : {
                        "sourceEntity" : {
                            "$ref" : "1"
                        },
                        "sourceColumn" : "IdObdobi"
                    },
                    "rightColumn" : {
                        "sourceEntity" : {
                            "$ref" : "2"
                        },
                        "sourceColumn" : "IdObdobi"
                    },
                    "relation" : "="
            },
            "outputColumns" : [
                {
                    "sourceEntity" : {
                        "$ref" : "1"
                    },
                    "sourceColumn" : "ZamestnanecId"
                },
                {
                    "sourceEntity" : {
                        "$ref" : "1"
                    },
                    "sourceColumn" : "OdpracHod"
                },
                {
                    "sourceEntity" : {
                        "$ref" : "2"
                    },
                    "sourceColumn" : "MzdObd_DatumOd"
                },
                {
                    "sourceEntity" : {
                        "$ref" : "2"
                    },
                    "sourceColumn" : "MzdObd_DatumDo"
                }
            ]
        }
        `;
        
        // deserialize the text and convert the plain entity to a class instance
        const sourceEntityConvertor = new PlainToSourceEntityConvertor();
        const deserialized = sourceEntityConvertor.convertToSourceEntity(JSON.parse(jsonText));

        // assert
        expect(JSON.stringify(deserialized)).toBe('{"name":"3","type":"inner","leftSourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]},"rightSourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]},"outputColumns":[{"sourceColumn":"ZamestnanecId","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}},{"sourceColumn":"OdpracHod","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}},{"sourceColumn":"MzdObd_DatumOd","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}},{"sourceColumn":"MzdObd_DatumDo","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}}],"condition":{"relation":"=","leftColumn":{"sourceColumn":"IdObdobi","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}},"rightColumn":{"sourceColumn":"IdObdobi","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}}}}')
    })
});

describe('Mapping config deserialization', () => {
    it('should deserialize a mapping configuration', () => {
        // data
        const jsonText = '{"sourceConnection":{"databaseProvider":"SQL Server","server":"server","initialCatalogue":"db"},"targetMappings":[{"name":"EmployeeHoursWorked","sourceEntity":{"$ref":"2"},"sourceEntities":[{"$id":"0","type":"sourceTable","name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]},{"$id":"1","type":"sourceTable","name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]},{"$id":"2","type":"join","name":"join1","joinType":"inner","leftSourceEntity":{"$ref":"0"},"rightSourceEntity":{"$ref":"1"},"joinCondition":{"relation":"equals","leftColumn":{"sourceEntity":{"$ref":"0"},"sourceColumn":"IdObdobi"},"rightColumn":{"sourceEntity":{"$ref":"1"},"sourceColumn":"IdObdobi"},"linkedCondition":null},"outputColumns":[{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumDo"},{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumOd"},{"sourceEntity":{"$ref":"0"},"sourceColumn":"OdpracHod"},{"sourceEntity":{"$ref":"0"},"sourceColumn":"ZamestnanecId"}]}],"columnMappings":{"PersonalId":{"sourceEntity":{"$ref":"0"},"sourceColumn":"ZamestnanecId"},"HoursCount":{"sourceEntity":{"$ref":"0"},"sourceColumn":"OdpracHod"},"DateFrom":{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumOd"},"DateTo":{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumDo"}}}]}';
        const deserialized = JSON.parse(jsonText);

        // deserialize
        const instance = MappingModelConvertor.convertToMappingModel(deserialized);
        
        // Assert
        const serialized = JSON.stringify(instance);
        expect(serialized).toBe('{"sourceConnection":{"databaseProvider":"SQL Server","server":"server","initialCatalogue":"db"},"targetMappings":[{"name":"EmployeeHoursWorked","columnMappings":{},"sourceEntity":{"name":"join1","type":"inner","leftSourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]},"rightSourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]},"outputColumns":[{"sourceColumn":"MzdObd_DatumDo","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}},{"sourceColumn":"MzdObd_DatumOd","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}},{"sourceColumn":"OdpracHod","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}},{"sourceColumn":"ZamestnanecId","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}}],"condition":{"relation":"equals","leftColumn":{"sourceColumn":"IdObdobi","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}},"rightColumn":{"sourceColumn":"IdObdobi","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}}}},"sourceEntities":[{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]},{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]},{"name":"join1","type":"inner","leftSourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]},"rightSourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]},"outputColumns":[{"sourceColumn":"MzdObd_DatumDo","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}},{"sourceColumn":"MzdObd_DatumOd","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}},{"sourceColumn":"OdpracHod","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}},{"sourceColumn":"ZamestnanecId","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}}],"condition":{"relation":"equals","leftColumn":{"sourceColumn":"IdObdobi","sourceEntity":{"name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]}},"rightColumn":{"sourceColumn":"IdObdobi","sourceEntity":{"name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]}}}}]}]}');
    })
});