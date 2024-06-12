// Must be in a global place
import 'reflect-metadata';

import { MappingEditor } from './view/mappingEditor';
import { SourceTable } from './mappingModel/sourceTable';
import { Join, JoinType } from './mappingModel/aggregators/join';
import { JoinCondition, Operator } from './mappingModel/aggregators/conditions/joinCondition';
import { EntityMapping } from './mappingModel/entityMapping';
import { Column, Database, Table } from './dbModel/database';
import { SourceColumn } from './mappingModel/sourceColumn';
import { EntityMappingConvertor } from './mappingModel/converting/entityMappingConvertor';
import { NVarCharMax, SimpleDataTypes, SimpleType } from './dbModel/dataTypes';
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
    new Column("ZamestnanecId", new SimpleType(SimpleDataTypes.Integer, false), "Id zamestnance"),
    new Column("OdpracHod", new SimpleType(SimpleDataTypes.Decimal, false), "Odpracované hodiny"),
    new Column("HodSaz", new SimpleType(SimpleDataTypes.Decimal, false)),
    new Column("IdObdobi", new SimpleType(SimpleDataTypes.Integer, false))
]);

const table2 = new Table("TabMzdObd", [
    new Column("MzdObd_DatumOd", new SimpleType(SimpleDataTypes.Date, false)),
    new Column("MzdObd_DatumDo", new SimpleType(SimpleDataTypes.Date, false)),
    new Column("IdObdobi", new SimpleType(SimpleDataTypes.Integer, false))
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
    new Column("PersonalId", new SimpleType(SimpleDataTypes.Integer, false), "Id zamestnance"),
    new Column("HoursCount", new SimpleType(SimpleDataTypes.Decimal, false), "Odpracované hodiny"),
    new Column("DateFrom", new SimpleType(SimpleDataTypes.Date, false), "Začátek období"),
    new Column("DateTo", new SimpleType(SimpleDataTypes.Date, false), "Konec období"),
    new Column("note", new NVarCharMax(true), "Poznámka"),
]

const targetTable = new Table("EmployeeHoursWorked2", targetColumns);

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
    targetTable.schema,
    join,
    [ sourceTable1, sourceTable2, join ],
    targetEntityColumnMapping
);

entityMapping.createBackwardConnections();

const plainEntitiyMapping = EntityMappingConvertor.convertEntityMappingToPlain(entityMapping);
const deserializedEntityMapping = EntityMappingConvertor.convertPlainToEntityMapping(plainEntitiyMapping);

// TODO:
//const mappingEditor = new MappingEditor(sourceDb, targetDb);
//mappingEditor.loadEntityMapping(deserializedEntityMapping);


// We need to get model of source db and entity to map
// We have the model prepared
// We need to change the mapper to accept only current target table.
    
export function getMappingEditor(): MappingEditor {
    const mappingEditor = new MappingEditor(sourceDb);
   return mappingEditor;
}

export function loadDevelopmentView() {

}


export function ahoj(): number {
    const mappingEditor = new MappingEditor(sourceDb);

    mappingEditor.loadEntityMapping(deserializedEntityMapping, targetTable);

    return 800;
}
