import { dia } from '@joint/core';
import { SHAPE_NAMESPACE } from './shapes/shapeNamespace';
import { GRID_SIZE, TARGET_DATABASE_ENTITY_GROUP_NAME, contentDiv } from '../constants';
import { PropertyLink } from './shapes/propertyLink';
import { TargetTableShape } from './shapes/targetTableShape';
import { EntityMapping } from '../mappingModel/entityMapping';
import { SourceEntitiesToShapesTransformer } from './helpers/sourceEntitiesToShapesTransformer';
import { Database, Table } from '../dbModel/database';
import { handlePointerMove, onPaperElementMouseEnter, onPaperElementMouseLeave, onPaperLinkMouseEnter, onPaperLinkMouseLeave, validateMagnet } from './helpers/eventHandlers';
import { createButton, attachChangeEvent } from './elements/utilityMethods';
import { SourceTablePickerModal } from './modals/sourceTablePickerModal';
import type { BaseEntityShape } from './shapes/baseEntityShape';
import { JoinLink } from './shapes/joinLink';
import { EntityMappingConvertor } from '../mappingModel/converting/entityMappingConvertor';
import { CustomQueryDefinitionModal } from './modals/customQuery/customQueryDefinitionModal';
import { SourceColumn } from '../mappingModel/sourceColumn';
import { plainToInstance } from 'class-transformer';
import { DirectedGraph } from '@joint/layout-directed-graph';
import { SourceTable } from '../mappingModel/sourceEntities/sourceTable';
import { SourceTableShape } from './shapes/sourceTableShape';
import { CustomQueryShape } from './shapes/customQueryShape';
import type { CustomQuery } from '../mappingModel/sourceEntities/customQuery';
import { JoinedSourceEntityData, type SourceEntityShapeFactory } from './helpers/joinedSourceEntityData';
import { Join, JoinType } from '../mappingModel/aggregators/join';
import { JoinModal } from './modals/joinModal';
import { ConcreteSourceEntityFinder } from './helpers/concreteSourceEntityFinder';
import { BaseSourceEntityShape } from './shapes/baseSourceEntityShape';
import type { SourceEntityBase } from '../mappingModel/sourceEntities/sourceEntityBase';
import type { PropertyPort } from './shapes/propertyPort';

/**
 * The `MappingEditor` class represents an editor for mapping entities and tables.
 * It provides functionality for creating, loading, and manipulating entity mapping.
 */
export class MappingEditor {
    public static readonly containerId = 'paper';

    private entityMapping: EntityMapping | null = null;
    private targetTable: Table | null;
    private readonly sourceTablePickerModal: SourceTablePickerModal;
    private readonly toolbar = document.getElementById('toolbar');
    public readonly graph = new dia.Graph({}, { cellNamespace: SHAPE_NAMESPACE });
    public paper: dia.Paper;

    public constructor(private readonly sourceDb: Database) {
        this.initializePaper();
        this.initializeEvents();
        this.initializeToolBox();
        this.sourceTablePickerModal = new SourceTablePickerModal(this.sourceDb.tables);
    }

    private initializePaper() : void {
        this.paper = new dia.Paper({
            el: document.getElementById('paper'),
            width: '100%',
            height: '100%',
            gridSize: GRID_SIZE,
            model: this.graph,
            frozen: true,
            async: true,
            defaultLink: new PropertyLink(),
            sorting: dia.Paper.sorting.APPROX,
            magnetThreshold: 'onleave',
            linkPinning: false,
            snapLinks: true,
            background: { color: '#F3F7F6' },
            defaultRouter: { name: 'metro', args: { step: 2 * GRID_SIZE } },
            defaultConnector: { name: 'jumpover', args: { size: 8 } },
            cellViewNamespace: SHAPE_NAMESPACE,
            validateConnection: this.validateConnection.bind(this),
            validateMagnet: validateMagnet,
        });

        this.paper.el.style.border = `1px solid #e2e2e2`;
    }

    private initializeEvents(): void {
        this.paper.on({
            'element:mouseenter': onPaperElementMouseEnter,
            'element:mouseleave': onPaperElementMouseLeave,
            'element:pointerdblclick': (cellView) => (cellView.model as BaseEntityShape).handleDoubleClick(),
            'link:mouseenter': onPaperLinkMouseEnter,
            'link:mouseleave': onPaperLinkMouseLeave,
            'link:pointerdblclick': (cellView) => { if (cellView.model instanceof JoinLink) (cellView.model as JoinLink).handleDoubleClick(); },
            'link:connect': (linkView) => (linkView.model as PropertyLink).handleConnection(),
            'cell:pointermove': handlePointerMove,
        });
    }

    private initializeToolBox() : void {
        this.toolbar?.appendChild(createButton('Add source table', () => this.openSourceTableSelectionModal()));
        this.toolbar?.appendChild(createButton('Export mapping', () => this.downloadMapping()));

        attachChangeEvent('source-file', (event) => {
            const file = (event.target as HTMLInputElement).files?.[0];
            if (!file) return;
            const reader = new FileReader();
            reader.onload = () => {
                const result = reader.result;
                if (!result) return;
                const entityMapping = EntityMappingConvertor.convertPlainToEntityMapping(JSON.parse(result as string));
                this.loadEntityMapping(entityMapping, this.targetTable!);
            };
            reader.readAsText(file);
        });

        this.toolbar?.appendChild(createButton('Add custom query', () => {
            const modal = new CustomQueryDefinitionModal();
            modal.open(() => {
                this.addCustomQuery(modal.customQuery ?? (() => { throw new Error('Custom query should be initialized by the modal.'); })());
                modal.finalize();
            }, true);
        }));
    }

    /**
     * Checks if the mapping is complete.
     * @returns True if mapper is initialized and all non-nullable columns are mapped, false otherwise.
     */
    public isMappingComplete(): boolean {
        if (this.entityMapping === null || this.targetTable === null) return false;
        for (const column of this.targetTable.columns) {
            if (!column.dataType.isNullable && this.entityMapping.columnMappings.get(column.name) == null) {
                return false;
            }
        }
        return true;
    }

    /**
     * Serializes the current entity mapping to JSON.
     * @returns A JSON string representation of the entity mapping.
     */
    public createSerializedMapping(): string {
        const plainEntityMapping = EntityMappingConvertor.convertEntityMappingToPlain(this.entityMapping ?? (() => { throw new Error('Entity mapping was not yet initialized.'); })());
        return JSON.stringify(plainEntityMapping, null, 2);
    }

    /**
     * Initiates the download of the current mapping as a JSON file.
     */
    public downloadMapping(): void {
        const jsonString = this.createSerializedMapping();
        const blob = new Blob([jsonString], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        const downloadLink = document.createElement('a');
        downloadLink.href = url;
        downloadLink.download = 'mappings.json';
        downloadLink.click();
        URL.revokeObjectURL(url);
    }

    /**
     * Creates entity mapping for the given table.
     * @param targetTable The target table that should be mapped.
     */
    public createFromTargetTable(targetTable: Table) {
        const columnMappings = new Map<string, SourceColumn | null>();
        for (const column of targetTable.columns) {
            columnMappings.set(column.name, null);
        }

        const entityMapping = new EntityMapping(targetTable.name, targetTable.schema, null, [], columnMappings);
        this.loadEntityMapping(entityMapping, targetTable);
    }

    /**
     * Creates entity mapping for the given serialized table.
     * @param serializedTable The serialized target table that should be mapped.
     */
    public createFromSerializedTargetTable(serializedTable: string) {
        const table = plainToInstance(Table, JSON.parse(serializedTable));
        this.createFromTargetTable(table);
    }

    /**
     * Loads an entity mapping and its corresponding target table into the editor.
     * @param entityMapping The entity mapping to load.
     * @param targetTable The target table to map.
     */
    public loadEntityMapping(entityMapping: EntityMapping, targetTable: Table) {
        this.targetTable = targetTable;
        this.entityMapping = entityMapping;
        this.paper.freeze();
        const cells = this.convertEntityMappingToShapes(entityMapping);
        this.graph.resetCells(cells);
        DirectedGraph.layout(this.graph, { nodeSep: 300, edgeSep: 100, rankDir: "LR" });
        this.paper.unfreeze();
    }

    /**
     * Loads a serialized entity mapping and its corresponding serialized table into the editor.
     * @param serializedEntityMapping The serialized entity mapping to load.
     * @param serializedTable The serialized target table to map.
     */
    public loadSerializedEntityMapping(serializedEntityMapping: string, serializedTable: string) {
        const plainEntityMapping = JSON.parse(serializedEntityMapping);
        const entityMapping = EntityMappingConvertor.convertPlainToEntityMapping(plainEntityMapping);
        const targetTable = plainToInstance(Table, JSON.parse(serializedTable));
        this.loadEntityMapping(entityMapping, targetTable);
    }

    /**
     * Opens the modal for selecting a source table.
     */
    private openSourceTableSelectionModal(): void {
        this.sourceTablePickerModal.open(() => this.addSourceTable(this.sourceTablePickerModal.tableToAdd ?? (() => { throw new Error('Table to add should be initialized by the modal.'); })()));
    }

    /**
     * Adds a source table to the mapping editor.
     * @param table The source table to add.
     */
    private addSourceTable(table: Table) {
        const sourceTable = new SourceTable(table.name, table.schema, table.columns.map(column => new SourceColumn(column)));
        sourceTable.createBackwardConnections();
        this.handleSourceEntityShapeAddition(() => new SourceTableShape(sourceTable, table), sourceTable);
    }

    /**
     * Adds a custom query to the mapping editor.
     * @param customQuery The custom query to add.
     */
    private addCustomQuery(customQuery: CustomQuery) {
        customQuery.createBackwardConnections();
        this.handleSourceEntityShapeAddition(() => new CustomQueryShape(customQuery), customQuery);
    }

    /**
     * Handles the addition of a source entity shape.
     * @param sourceEntityShapeFactory A factory method for creating the source entity shape.
     * @param sourceEntity The source entity to add.
     */
    private handleSourceEntityShapeAddition(sourceEntityShapeFactory: SourceEntityShapeFactory, sourceEntity: SourceEntityBase) {
        const currentSourceEntity = this.entityMapping!.sourceEntity;
        if (currentSourceEntity === null) {
            const sourceTableShape = sourceEntityShapeFactory();
            this.entityMapping!.sourceEntity = sourceEntity;
            this.entityMapping!.createBackwardConnections();
            this.graph.addCell(sourceTableShape);
            return;
        }

        const join = new Join(`join_${currentSourceEntity.name}_${sourceEntity.fullName}`, JoinType.inner, currentSourceEntity, sourceEntity);
        const joinModal = new JoinModal(join);
        const sourceTableData = new JoinedSourceEntityData(sourceEntityShapeFactory, joinModal, join);
        joinModal.open(() => this.finishJoiningSourceTable(sourceTableData), true);
    }

    /**
     * After join definition is finished, this method is called to finish the joining process.
     * Handles the creation of the join link and the right source table shape.
     * @param joinedSourceEntityData Encapsulation of objects needed to finish the joining process.
     */
    private finishJoiningSourceTable(joinedSourceEntityData: JoinedSourceEntityData) {
        const rightSourceEntityShape = joinedSourceEntityData.sourceEntityShapeFactory();
        const sourceTableFinder = new ConcreteSourceEntityFinder();

        if (this.entityMapping!.sourceEntity == null) throw new Error('Source entity should be initialized.');
        this.entityMapping!.sourceEntity.accept(sourceTableFinder);
        const leftEntity = sourceTableFinder.desiredSourceEntity;

        const leftSourceTableShape = this.graph.getElements().find(element => {
            if (element instanceof BaseSourceEntityShape) {
                return element.uniqueName === leftEntity?.fullName;
            }
            return false;
        }) as BaseSourceEntityShape;

        const joinLink = new JoinLink(joinedSourceEntityData.unfinishedJoin, joinedSourceEntityData.joinModal);
        joinLink.source(leftSourceTableShape);
        joinLink.target(rightSourceEntityShape);

        this.entityMapping!.sourceEntity = joinedSourceEntityData.unfinishedJoin;
        this.entityMapping!.createBackwardConnections();

        this.paper.freeze();
        this.graph.addCells([rightSourceEntityShape, joinLink]);
        this.paper.unfreeze();

        rightSourceEntityShape.onPaperPlacement();
    }

    /**
     * Converts the given entity mapping to a set of JointJS shapes.
     * @param entityMapping The entity mapping to convert.
     * @returns An array of JointJS cells.
     */
    private convertEntityMappingToShapes(entityMapping: EntityMapping): dia.Cell[] {
        const transformer = new SourceEntitiesToShapesTransformer(this.sourceDb.tables);
        if (entityMapping.sourceEntity !== null) {
            entityMapping.sourceEntity.accept(transformer);
        }

        const cells = transformer.cells;
        const targetShape = new TargetTableShape(entityMapping, []);
        for (const [targetColumnName, sourceColumn] of entityMapping.columnMappings) {
            const targetColumn = this.targetTable!.columns.find((column) => column.name === targetColumnName);
            if (targetColumn === undefined) throw new Error('Cannot find the corresponding column');

            const targetPort = targetShape.addPropertyPort(targetColumn);
            if (targetPort === undefined) throw new Error('Cannot create the port for the target column');

            if (sourceColumn !== null) {
                const sourceElement = transformer.elementMap.get(sourceColumn.owner) ?? (() => { throw new Error('Source element not found.'); })();
                const sourcePort = sourceElement.getPortByColumn(sourceColumn);
                const link = new PropertyLink();
                link.source({ id: sourceElement.id, magnet: "portBody", port: sourcePort.id });
                link.target({ id: targetShape.id, magnet: "portBody", port: targetPort.id });
                cells.push(link);
            }
        }

        cells.push(targetShape);
        return cells;
    }

    /**
     * Validates the connection between source and target ports.
     * @param sourceView The source element view.
     * @param _sourceMagnet The source magnet.
     * @param targetView The target element view.
     * @param _targetMagnet The target magnet.
     * @returns True if the connection is valid, false otherwise.
     */
    private validateConnection(sourceView: dia.ElementView, _sourceMagnet: SVGAElement, targetView: dia.ElementView, _targetMagnet: SVGAElement) {
        const targetElement = targetView.model as dia.Element;
        const targetPortId = targetView.findAttribute('port', _targetMagnet) ?? (() => { throw new Error('Target port not found.'); })();
        const targetPort = targetElement.getPort(targetPortId) as PropertyPort;

        if (sourceView === targetView) return false;

        if (targetElement instanceof BaseSourceEntityShape) {
            console.debug('paper<validateConnection>: Cannot connect to source database entity.');
            return false;
        }

        if (this.graph.getConnectedLinks(targetElement, { inbound: true }).find((link) => link.target().port === targetPortId)) {
            console.debug('paper<validateConnection>', 'The port has already an inbound link (we allow only one link per port)');
            return false;
        }

        const sourceEntity = sourceView.model as dia.Element;
        const sourcePortId = sourceView.findAttribute('port', _sourceMagnet) ?? (() => { throw new Error('Source port not found.'); })();
        const sourcePort = sourceEntity.getPort(sourcePortId) as PropertyPort;

        if (!targetPort.isAssignableWith(sourcePort)) {
            console.debug('paper<validateConnection>', 'The types of the ports are not compatible.');
            return false;
        }

        return true;
    }
}
