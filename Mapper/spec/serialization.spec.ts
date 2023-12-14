import { instanceToPlain } from "class-transformer";
import { EntityMappingConvertor } from "../src/mappingModel/serialization/entityMappingConvertor";
import { MappingConverterVisiter } from "../src/mappingModel/serialization/mappingConverterVisitor";
import { MappingModelConvertor } from "../src/mappingModel/serialization/mappingModelConverter";
import { JoinCondition, Operator } from "../src/mappingModel/Agregators/Conditions/joinCondition";
import { SourceTable } from "../src/mappingModel/sourceTable";
import { Join, JoinType } from "../src/mappingModel/Agregators/join";
import { EntityMapping } from "../src/mappingModel/entityMapping";
import { DbConnectionConfig } from "../src/mappingModel/dbConnectionConfig";
import { MappingConfig } from "../src/mappingModel/mappingConfig";
import { Column, ColumnType, Table } from "../src/dbModel/database";
import { SourceColumn } from "../src/mappingModel/sourceColumn";

const table1 = new Table("TabMzdList", [
    new Column("ZamestnanecId", ColumnType.int),
    new Column("OdpracHod", ColumnType.decimal),
    new Column("ZamestnanecId", ColumnType.int),
    new Column("IdObdobi", ColumnType.int)
]);

const table2 = new Table("TabMzdObd", [
    new Column("MzdObd_DatumOd", ColumnType.date),
    new Column("MzdObd_DatumDo", ColumnType.date),
    new Column("IdObdobi", ColumnType.int)
]);

const sourceTable1 = new SourceTable(table1.name, table1.columns.map(c => new SourceColumn(c)));
const sourceTable2 = new SourceTable(table2.name, table2.columns.map(c => new SourceColumn(c)));

const joinCondition = new JoinCondition(
    Operator.equals,
    sourceTable1.getSelectedColumn("IdObdobi"),
    sourceTable2.getSelectedColumn("IdObdobi")
)

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
    join,
    [ sourceTable1, sourceTable2, join ],
    targetEntityColumnMapping
);

const dbConnectionConfig = new DbConnectionConfig(
    "SQL Server",
    "server",
    "db");

const mappingConfig = new MappingConfig(dbConnectionConfig, [entityMapping]);

/** 
 * RUN TESTS
**/

// TODO: Consider removing because the serialization is implicitly expecting that the columns are defined in sourceTables before the join condition references them
// describe('Join condition serializetion', () => {
//     it('should serialize a join condition', () => {
        
//         const visitor = new MappingConverterVisiter();
//         joinCondition.accept(visitor);
        
//         const stringifiedResult = JSON.stringify(visitor.getResult());
//         // todo: 
//         expect(stringifiedResult).toBe('{"relation":"equals","leftColumn":{"sourceEntity":{"$id":"1","type":"sourceTable","name":"table1","selectedColumns":["col1","col2"]},"sourceColumn":"col1"},"rightColumn":{"sourceEntity":{"$id":"0","type":"sourceTable","name":"table1","selectedColumns":["col1","col2"]},"sourceColumn":"col1"},"linkedCondition":null}');
//     });
// });

fdescribe('Join serialization', () => {
    it('should serialize a join', () => {
        // Serialize
        const visitor = new MappingConverterVisiter();
        join.accept(visitor);
        const stringifiedResult = JSON.stringify(visitor.getResult(), null, 4);

        const parsed = JSON.parse(`
        {
            "$id": "0",
            "type": "join",
            "name": "join1",
            "joinType": "inner",
            "leftSourceEntity": {
                "$id": "1",
                "type": "sourceTable",
                "name": "TabMzdList",
                "selectedColumns": [
                    "ZamestnanecId",
                    "OdpracHod",
                    "IdObdobi"
                ]
            },
            "rightSourceEntity": {
                "$id": "2",
                "type": "sourceTable",
                "name": "TabMzdObd",
                "selectedColumns": [
                    "MzdObd_DatumOd",
                    "MzdObd_DatumDo",
                    "IdObdobi"
                ]
            },
            "joinCondition": {
                "relation": "equals",
                "leftColumn": {
                    "sourceEntity": {
                        "$ref": "1"
                    },
                    "sourceColumn": "IdObdobi"
                },
                "rightColumn": {
                    "sourceEntity": {
                        "$ref": "2"
                    },
                    "sourceColumn": "IdObdobi"
                },
                "linkedCondition": null
            },
            "outputColumns": [
                {
                    "sourceEntity": {
                        "$ref": "2"
                    },
                    "sourceColumn": "MzdObd_DatumDo"
                },
                {
                    "sourceEntity": {
                        "$ref": "2"
                    },
                    "sourceColumn": "MzdObd_DatumOd"
                },
                {
                    "sourceEntity": {
                        "$ref": "1"
                    },
                    "sourceColumn": "OdpracHod"
                },
                {
                    "sourceEntity": {
                        "$ref": "1"
                    },
                    "sourceColumn": "ZamestnanecId"
                }
            ]
        }
        `);

        const stringifiedExpected = JSON.stringify(parsed, null, 4);

        // Assert
        expect(stringifiedResult).toBe(stringifiedExpected);
    });
});

describe('Entity mapping serialization', () => {
    it('should serialize an entity mapping', () => {
        // Convert to plain
        const plain = EntityMappingConvertor.convertEntityMappingToPlain(entityMapping);
        const stringifiedResult = JSON.stringify(plain);

        expect(stringifiedResult).toBe('{"name":"EmployeeHoursWorked","sourceEntity":{"$ref":"2"},"sourceEntities":[{"$id":"0","type":"sourceTable","name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]},{"$id":"1","type":"sourceTable","name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]},{"$id":"2","type":"join","name":"join1","joinType":"inner","leftSourceEntity":{"$ref":"0"},"rightSourceEntity":{"$ref":"1"},"joinCondition":{"relation":"equals","leftColumn":{"sourceEntity":{"$ref":"0"},"sourceColumn":"IdObdobi"},"rightColumn":{"sourceEntity":{"$ref":"1"},"sourceColumn":"IdObdobi"},"linkedCondition":null},"outputColumns":[{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumDo"},{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumOd"},{"sourceEntity":{"$ref":"0"},"sourceColumn":"OdpracHod"},{"sourceEntity":{"$ref":"0"},"sourceColumn":"ZamestnanecId"}]}],"columnMappings":{"PersonalId":{"sourceEntity":{"$ref":"0"},"sourceColumn":"ZamestnanecId"},"HoursCount":{"sourceEntity":{"$ref":"0"},"sourceColumn":"OdpracHod"},"DateFrom":{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumOd"},"DateTo":{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumDo"}}}');
    });
});

describe('DbConnectionConfig serialization', () => {
    it('should serialize a DbConnectionConfig', () => {
        // Convert to plain
        const plain = instanceToPlain(dbConnectionConfig);
        expect(JSON.stringify(plain)).toBe('{"databaseProvider":"SQL Server","server":"server","initialCatalogue":"db"}');
    })
});

// TODO: The order of properties - either change the format and add definitions property or sort the properties to have the sourceEntites first
describe('Mapping config serialization', () => {
    it('should serialize a mapping configuration', () => {
        // Convert to plain
        const plain = MappingModelConvertor.convertToPlain(mappingConfig);
        const serialized = JSON.stringify(plain);

        expect(serialized).toBe('{"sourceConnection":{"databaseProvider":"SQL Server","server":"server","initialCatalogue":"db"},"targetMappings":[{"name":"EmployeeHoursWorked","sourceEntity":{"$ref":"2"},"sourceEntities":[{"$id":"0","type":"sourceTable","name":"TabMzdList","selectedColumns":["ZamestnanecId","OdpracHod","IdObdobi"]},{"$id":"1","type":"sourceTable","name":"TabMzdObd","selectedColumns":["MzdObd_DatumOd","MzdObd_DatumDo","IdObdobi"]},{"$id":"2","type":"join","name":"join1","joinType":"inner","leftSourceEntity":{"$ref":"0"},"rightSourceEntity":{"$ref":"1"},"joinCondition":{"relation":"equals","leftColumn":{"sourceEntity":{"$ref":"0"},"sourceColumn":"IdObdobi"},"rightColumn":{"sourceEntity":{"$ref":"1"},"sourceColumn":"IdObdobi"},"linkedCondition":null},"outputColumns":[{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumDo"},{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumOd"},{"sourceEntity":{"$ref":"0"},"sourceColumn":"OdpracHod"},{"sourceEntity":{"$ref":"0"},"sourceColumn":"ZamestnanecId"}]}],"columnMappings":{"PersonalId":{"sourceEntity":{"$ref":"0"},"sourceColumn":"ZamestnanecId"},"HoursCount":{"sourceEntity":{"$ref":"0"},"sourceColumn":"OdpracHod"},"DateFrom":{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumOd"},"DateTo":{"sourceEntity":{"$ref":"1"},"sourceColumn":"MzdObd_DatumDo"}}}]}')
    })
});