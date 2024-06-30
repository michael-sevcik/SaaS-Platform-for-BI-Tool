import { plainToInstance } from "class-transformer";
import { PlainToSourceEntityConvertor } from "../src/mappingModel/converting/deserialization/plainToSourceEntityConvertor";
import { SourceColumn } from "../src/mappingModel/sourceEntities/sourceColumn";
import { inspect } from "util";

// TODO: shares data with the serialization tests - consider separating it to a shared file.

/** 
 * RUN TESTS
**/

describe('sourceColumnDeserialization', () => {
    it('should deserialize a sourceColumn with simple data type', () => {
        const jsonText = '{"name":"ZamestnanecId","description":null,"dataType":{"isNullable":false,"simpleType":"Integer","type":"simple"}}';
        const sourceColumn = plainToInstance(SourceColumn, JSON.parse(jsonText));
        expect(JSON.stringify(sourceColumn)).toBe('{"name":"ZamestnanecId","description":null,"dataType":{"isNullable":false,"simpleType":"Integer"},"_owner":null,"referenceHolders":{}}');
    });
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