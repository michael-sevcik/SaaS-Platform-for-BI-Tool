import { instanceToPlain } from "class-transformer";
import { EntityMappingConvertor } from "../src/mappingModel/converting/entityMappingConvertor";
import { MappingToPlainConverterVisiter } from "../src/mappingModel/converting/serialization/mappingToPlainConverterVisitor";
import { JoinCondition, Operator } from "../src/mappingModel/sourceEntities/aggregators/conditions/joinCondition";
import { SourceTable } from "../src/mappingModel/sourceEntities/sourceTable";
import { Join, JoinType } from "../src/mappingModel/sourceEntities/aggregators/join";
import { Column, Table } from "../src/dbModel/database";
import { SourceColumn } from "../src/mappingModel/sourceEntities/sourceColumn";
import { NVarChar, SimpleDataTypes, SimpleType } from "../src/dbModel/dataTypes";
import { inspect } from "util";
import { EntityMapping } from "../src/mappingModel/entityMapping";

function SimpleTypeFactory(type: SimpleDataTypes) : SimpleType {
    return new SimpleType(type, false);
}

const table1 = new Table("TabMzdList", [
    new Column("ZamestnanecId", SimpleTypeFactory(SimpleDataTypes.Integer)),
    new Column("OdpracHod", SimpleTypeFactory(SimpleDataTypes.Decimal)),
    new Column("ZamestnanecId", SimpleTypeFactory(SimpleDataTypes.Integer)),
    new Column("IdObdobi", SimpleTypeFactory(SimpleDataTypes.Integer)),
]);

const table2 = new Table("TabMzdObd", [
    new Column("MzdObd_DatumOd", SimpleTypeFactory(SimpleDataTypes.Date)),
    new Column("MzdObd_DatumDo", SimpleTypeFactory(SimpleDataTypes.Date)),
    new Column("IdObdobi", SimpleTypeFactory(SimpleDataTypes.Integer))
]);

const sourceTable1 = new SourceTable(table1.name, table1.schema, table1.columns.map(c => new SourceColumn(c)));
const sourceTable2 = new SourceTable(table2.name, table2.schema, table2.columns.map(c => new SourceColumn(c)));

const joinCondition = new JoinCondition(
    Operator.equals,
    sourceTable1.getSelectedColumn("IdObdobi"),
    sourceTable2.getSelectedColumn("IdObdobi")
);

const join = new Join(
    "join1",
    JoinType.inner,
    sourceTable1,
    sourceTable2,
    joinCondition
);

const targetEntityColumnMapping = new Map<string, SourceColumn | null>([
    ["PersonalId", sourceTable1.getSelectedColumn("ZamestnanecId")],
    ["HoursCount", sourceTable1.getSelectedColumn("OdpracHod")],
    ["DateFrom", sourceTable2.getSelectedColumn("MzdObd_DatumOd")],
    ["DateTo", sourceTable2.getSelectedColumn("MzdObd_DatumDo")],
    ["note", null]
]);

const entityMapping = new EntityMapping(
    "EmployeeHoursWorked",
    "dbo",
    join,
    [ sourceTable1, sourceTable2, join ],
    targetEntityColumnMapping
);

/** 
 * RUN TESTS
**/

describe('sourceColumn serialization', () => {
    it('should serialize a sourceColumn with simple data type', () => {
        const column = sourceTable1.getSelectedColumn("ZamestnanecId");
        const plain = instanceToPlain(column);
        expect(JSON.stringify(plain)).toBe('{"name":"ZamestnanecId","description":null,"dataType":{"isNullable":false,"simpleType":"Integer","type":"simple"}}');
    });
    
    it('should serialize a sourceColumn with nvchar datatype', () => {
        const column = new Column("note", new NVarChar(450, true), "note");
        const sourceColumn = new SourceColumn(column);
        const plain = instanceToPlain(sourceColumn);
        expect(JSON.stringify(plain)).toBe('{"name":"note","description":null,"dataType":{"isNullable":true,"length":450,"type":"nVarChar"}}');
    });
});

describe('Join serialization', () => { 
    it('should serialize a join', () => {
        // Serialize
        const visitor = new MappingToPlainConverterVisiter();
        join.accept(visitor);
        const [sourceEntities, _] = visitor.getResult();
        const stringifiedResult = JSON.stringify(sourceEntities, null, 4);

        const parsedExpectedResult = JSON.parse(`
        [
            {
                "$id": "1",
                "type": "sourceTable",
                "name": "TabMzdList",
                "schema": null,
                "selectedColumns": [
                    {
                        "name": "IdObdobi",
                        "description": null,
                        "dataType": {
                            "isNullable": false,
                            "simpleType": "Integer",
                            "type": "simple"
                        },
                        "$id": "5"
                    },
                    {
                        "name": "ZamestnanecId",
                        "description": null,
                        "dataType": {
                            "isNullable": false,
                            "simpleType": "Integer",
                            "type": "simple"
                        },
                        "$id": "4"
                    },
                    {
                        "name": "OdpracHod",
                        "description": null,
                        "dataType": {
                            "isNullable": false,
                            "simpleType": "Decimal",
                            "type": "simple"
                        },
                        "$id": "3"
                    },
                    {
                        "name": "ZamestnanecId",
                        "description": null,
                        "dataType": {
                            "isNullable": false,
                            "simpleType": "Integer",
                            "type": "simple"
                        },
                        "$id": "2"
                    }
                ]
            },
            {
                "$id": "6",
                "type": "sourceTable",
                "name": "TabMzdObd",
                "schema": null,
                "selectedColumns": [
                    {
                        "name": "IdObdobi",
                        "description": null,
                        "dataType": {
                            "isNullable": false,
                            "simpleType": "Integer",
                            "type": "simple"
                        },
                        "$id": "9"
                    },
                    {
                        "name": "MzdObd_DatumDo",
                        "description": null,
                        "dataType": {
                            "isNullable": false,
                            "simpleType": "Date",
                            "type": "simple"
                        },
                        "$id": "8"
                    },
                    {
                        "name": "MzdObd_DatumOd",
                        "description": null,
                        "dataType": {
                            "isNullable": false,
                            "simpleType": "Date",
                            "type": "simple"
                        },
                        "$id": "7"
                    }
                ]
            },
            {
                "$id": "0",
                "type": "join",
                "name": "join1",
                "joinType": "inner",
                "leftSourceEntity": {
                    "$ref": "1"
                },
                "rightSourceEntity": {
                    "$ref": "6"
                },
                "joinCondition": {
                    "relation": "equals",
                    "leftColumn": {
                        "$ref": "5"
                    },
                    "rightColumn": {
                        "$ref": "9"
                    },
                    "linkedCondition": null
                },
                "selectedColumns": [
                    {
                        "$ref": "2"
                    },
                    {
                        "$ref": "3"
                    },
                    {
                        "$ref": "4"
                    },
                    {
                        "$ref": "5"
                    },
                    {
                        "$ref": "7"
                    },
                    {
                        "$ref": "8"
                    },
                    {
                        "$ref": "9"
                    }
                ]
            }
        ]
        `);

        const stringifiedExpected = JSON.stringify(parsedExpectedResult, null, 4);

        // Assert
        expect(stringifiedResult).toBe(stringifiedExpected);
    });
});

describe('Entity mapping serialization', () => {
    it('should serialize an entity mapping', () => {
        // Convert to plain
        const plain = EntityMappingConvertor.convertEntityMappingToPlain(entityMapping);
        const stringifiedResult = JSON.stringify(plain);
        console.log(stringifiedResult);
        
        expect(stringifiedResult).toBe('{"name":"EmployeeHoursWorked","schema":"dbo","mappingData":[[{"$id":"1","type":"sourceTable","name":"TabMzdList","schema":null,"selectedColumns":[{"name":"IdObdobi","description":null,"dataType":{"isNullable":false,"simpleType":"Integer","type":"simple"},"$id":"5"},{"name":"ZamestnanecId","description":null,"dataType":{"isNullable":false,"simpleType":"Integer","type":"simple"},"$id":"4"},{"name":"OdpracHod","description":null,"dataType":{"isNullable":false,"simpleType":"Decimal","type":"simple"},"$id":"3"},{"name":"ZamestnanecId","description":null,"dataType":{"isNullable":false,"simpleType":"Integer","type":"simple"},"$id":"2"}]},{"$id":"6","type":"sourceTable","name":"TabMzdObd","schema":null,"selectedColumns":[{"name":"IdObdobi","description":null,"dataType":{"isNullable":false,"simpleType":"Integer","type":"simple"},"$id":"9"},{"name":"MzdObd_DatumDo","description":null,"dataType":{"isNullable":false,"simpleType":"Date","type":"simple"},"$id":"8"},{"name":"MzdObd_DatumOd","description":null,"dataType":{"isNullable":false,"simpleType":"Date","type":"simple"},"$id":"7"}]},{"$id":"0","type":"join","name":"join1","joinType":"inner","leftSourceEntity":{"$ref":"1"},"rightSourceEntity":{"$ref":"6"},"joinCondition":{"relation":"equals","leftColumn":{"$ref":"5"},"rightColumn":{"$ref":"9"},"linkedCondition":null},"selectedColumns":[{"$ref":"2"},{"$ref":"3"},{"$ref":"4"},{"$ref":"5"},{"$ref":"7"},{"$ref":"8"},{"$ref":"9"}]}],{"$ref":"0"},{"PersonalId":{"$ref":"2"},"HoursCount":{"$ref":"3"},"DateFrom":{"$ref":"7"},"DateTo":{"$ref":"8"},"note":null}]}');
    });
});