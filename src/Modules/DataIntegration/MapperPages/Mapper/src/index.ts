// Must be in a global place
import 'reflect-metadata';

import { MappingEditor } from './view/mappingEditor';
import { SourceTable } from './mappingModel/sourceTable';
import { Join, JoinType } from './mappingModel/aggregators/join';
import { JoinCondition, Operator } from './mappingModel/aggregators/conditions/joinCondition';
import { EntityMapping } from './mappingModel/entityMapping';
import { Column, ColumnType, Database, Table } from './dbModel/database';
import { SourceColumn } from './mappingModel/sourceColumn';
import { EntityMappingConvertor } from './mappingModel/converting/entityMappingConvertor';
//export { getMappingEditor };


 

// Example Diagram

const articleMovementRowsProperties = [
    'Quantity: decimal',
    'RowNumber: int',
    'TotalPrice: decimal',
    'ArticleGroupId: string',
    'ArticleRegistrationNumber: string',
    'ArticleName: string',
    'ArticleMovementExternalId: int'
];

const tabPohybyZboziProperties = [
    'Id: int',
    'IdzboSklad: int',
    'Iddoklad: int',
    'SkupZbo: string',
    'RegCis: string',
    'Nazev1: string',
    'Nazev2: string',
    'Mnozstvi: decimal',
    'MnozstviReal: decimal',
    'Poradi: int',
    'VstupniCena: byte',
    'JcbezDaniKc: decimal',
    'CcbezDaniKc: decimal',
    'CcbezDaniVal: decimal',
    'Autor: string',
];

const table1 = new Table("TabMzdList", [
    new Column("ZamestnanecId", ColumnType.int, "Id zamestnance"),
    new Column("OdpracHod", ColumnType.decimal, "Odpracované hodiny"),
    new Column("HodSaz", ColumnType.decimal),
    new Column("IdObdobi", ColumnType.int)
]);

const table2 = new Table("TabMzdObd", [
    new Column("MzdObd_DatumOd", ColumnType.date),
    new Column("MzdObd_DatumDo", ColumnType.date),
    new Column("IdObdobi", ColumnType.int)
]);

const sourceDb = new Database("SourceDatabase", [table1, table2]);

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

const targetColumns = [
    new Column("PersonalId", ColumnType.int, "Id zamestnance"),
    new Column("HoursCount", ColumnType.decimal, "Odpracované hodiny"),
    new Column("DateFrom", ColumnType.date, "Začátek období"),
    new Column("DateTo", ColumnType.date, "Konec období"),
    new Column("note", ColumnType.string, "Poznámka"),
]

const targetTable = new Table("EmployeeHoursWorked", targetColumns);

const targetEntityColumnMapping = new Map<string, SourceColumn | null>([
    [targetColumns[0].name, sourceTable1.getSelectedColumn("ZamestnanecId")],
    [targetColumns[1].name, sourceTable1.getSelectedColumn("OdpracHod")],
    [targetColumns[2].name, sourceTable2.getSelectedColumn("MzdObd_DatumOd")],
    [targetColumns[3].name, null],
    [targetColumns[4].name, null]
]);

const targetDb = new Database("TargetDatabase", [targetTable]);

const entityMapping = new EntityMapping(
    targetTable.name,
    join,
    [ sourceTable1, sourceTable2, join ],
    targetEntityColumnMapping
);

entityMapping.createBackwardConnections();

const plainEntitiyMapping = EntityMappingConvertor.convertEntityMappingToPlain(entityMapping);
const deserializedEntityMapping = EntityMappingConvertor.convertPlainToEntityMapping(plainEntitiyMapping);

const mappingEditor = new MappingEditor(sourceDb, targetDb);
// TODO:
//const mappingEditor = new MappingEditor(sourceDb, targetDb);
//mappingEditor.loadEntityMapping(deserializedEntityMapping);


// function getMappingEditor(): MappingEditor {
//    return M;
// }




export function ahoj(): number {
    mappingEditor.loadEntityMapping(deserializedEntityMapping);

    return 800;
}
