import { dia, elementTools, layout, linkTools, shapes } from '@joint/core';
import { DirectedGraph } from '@joint/layout-directed-graph';
import { SHAPE_NAMESPACE } from './shapes/shapeNamespace';
import { GRID_SIZE, TARGET_DATABASE_ENTITY_GROUP_NAME, LIGHT_COLOR, contentDiv } from '../constants';
import { PropertyLink } from './shapes/propertyLink';
import { SourceTableShape } from './shapes/sourceTableShape';
import { JoinLink } from './shapes/joinLink';
import { EntityMapping } from '../mappingModel/entityMapping';
import { SourceEntitiesToShapesTransformer } from './sourceEntitiesToShapesTransformer';
import { TargetTableShape } from './shapes/targetTableShape';
import { Database, Table } from '../dbModel/database';
import { Link } from './shapes/link';
import { PropertyPort } from './shapes/propertyPort';
import { SourceTable } from '../mappingModel/sourceTable';
import { SourceColumn } from '../mappingModel/sourceColumn';
import { Join, JoinType } from '../mappingModel/aggregators/join';
import { JoinModal } from './modals/joinModal';
import { SourceTablePickerModal } from './modals/sourceTablePickerModal';
import { MappingVisitor } from '../mappingModel/mappingVisitor';
import { ConditionLink } from '../mappingModel/aggregators/conditions/conditionLink';
import { JoinCondition } from '../mappingModel/aggregators/conditions/joinCondition';
import { BaseEntityShape } from './shapes/baseEntityShape';
import { BaseSourceEntityShape } from './shapes/baseSourceEntityShape';
import { EntityMappingConvertor } from '../mappingModel/converting/entityMappingConvertor';
import { plainToInstance } from 'class-transformer';
import { getElementOrThrow } from '../utils';


/**
 * Helper class for encapsulating objects needed to finish joining process
 * that starts after an addition of a new source table.
 */
class IntermediateSourceTableData {

    /**
     * Initializes a new instance o of {@link IntermediateSourceTableData}.
     * @param tableToAdd The table that is being added and needs to be joined.
     * @param tableModel The model of table that is being added.
     *  Used for construction of {@link SourceTableShape}.
     * @param joinModal the modal responsible for joining the new table.
     * @param unfinishedJoin Entity representing the join between old table and newly added.
     */
    public constructor(
        public readonly tableToAdd: SourceTable,
        public readonly tableModel: Table,
        public readonly joinModal: JoinModal,
        public readonly unfinishedJoin: Join)
        {}
}

/**
 * Helper {@link MappingVisitor} derived class for finding the rightmost 
 * source table in the joining tree.
 */
class SourceTableFinder extends MappingVisitor {
    public desiredSourceEntity : SourceTable | null;

    // Intentionally left not implemented
    public visitSourceColumn(sourceColumn: SourceColumn): void {
        throw new Error('Method not implemented.');
    }

    // Intentionally left not implemented
    public visitConditionLink(conditionLink: ConditionLink): void {
        throw new Error('Method not implemented.');
    }

    // Intentionally left not implemented
    public visitJoinCondition(joinCondition: JoinCondition): void {
        throw new Error('Method not implemented.');
    }

    public visitSourceTable(sourceTable: SourceTable): void {
        this.desiredSourceEntity = sourceTable;
    }
    public visitJoin(join: Join): void {
        join.rightSourceEntity.accept(this);
    }

}

export class MappingEditor {
    public static readonly containerId = 'paper';
//#region static event hadler methods
    private static onPaperElementMouseEnter(elementView: dia.ElementView) {
        const baseEntityShape = elementView.model as BaseEntityShape;

        if (baseEntityShape instanceof BaseSourceEntityShape) {
            const toolsView = new dia.ToolsView({
                tools: [new elementTools.Remove( {
                    action: () => {
                        console.log('Removing an element.')
                        baseEntityShape.handleRemoving();
                    }
                })]
            })
            
            elementView.addTools(toolsView)
        }
    }

    private static onPaperElementMouseLeave(linkView: dia.ElementView) {
        linkView.removeTools();
    }

    private static onPaperLinkMouseEnter(linkView: dia.LinkView) {
        const link = linkView.model as Link;

        // TODO: OPTIMIZE - do not create new tools every time 
        const toolsView = new dia.ToolsView({
            tools: [new linkTools.Remove( { action: () => {
                console.log('Removing a link');
                link.handleRemoving();
            }})]
        });
    
        linkView.addTools(toolsView);
        if (link instanceof PropertyLink) console.log('Property link');
        
        link.highlight();
    }

    private static onPaperLinkMouseLeave(linkView: dia.LinkView) {
        linkView.removeTools();
        const link = linkView.model as Link;
        link.unhighlight();
    }
//#endregion 

    private entityMapping : EntityMapping | null = null;
    private targetTable : Table | null;
    private readonly sourceTablePickerModal: SourceTablePickerModal;
    private readonly toolbar = document.getElementById('toolbar');
    public readonly graph = new dia.Graph({}, { cellNamespace: SHAPE_NAMESPACE });
    public readonly paper : dia.Paper;

    // TODO: consider creating map of target tables with their names as keys
    public constructor(private readonly sourceDb: Database) {
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
            background: {
                color: '#F3F7F6'
            },
            defaultRouter: { name: 'manhattan', args: { step: 2*GRID_SIZE }},
            // defaultConnector: { name: 'jumpover', args: { size: 10 }}, // TODO: change the connecter and router
            cellViewNamespace: SHAPE_NAMESPACE,
            validateConnection: (sourceView, _sourceMagnet, targetView, _targetMagnet) => {
                // TODO: use the types of columns to validate connections
                const targetElement = targetView.model as dia.Element;
                const targetPortId = targetView.findAttribute('port', _targetMagnet)
                    ?? (() => { throw new Error('Target port not found.'); })();
                const targetPort = targetElement.getPort(targetPortId) as PropertyPort; 

                if (sourceView === targetView) return false;
                // TODO: Create an abstract class for source elements
                if (targetElement instanceof BaseSourceEntityShape) {
                    console.log('paper<validateConnection>', 'Cannot connect to source database entity.');
                    return false;
                };
        
                // FIXME: A lot of logs is being printed here, fix it
                if (
                    this.graph
                        .getConnectedLinks(targetElement, { inbound: true })
                        .find((link) => link.target().port === targetPortId)
                ) {
                    console.log('paper<validateConnection>', 'The port has already an inbound link (we allow only one link per port)');
                    return false;
                }

                const sourceEntity = sourceView.model as dia.Element;
                const sourcePortId = sourceView.findAttribute('port', _sourceMagnet)
                    ?? (() => { throw new Error('Source port not found.'); })();
                const sourcePort = sourceEntity.getPort(sourcePortId) as PropertyPort;
        
                if (!targetPort.isAssignableWith(sourcePort)) {
                    console.log('paper<validateConnection>', 'The types of the ports are not compatible.');
                    return false;
                }
                
                return true;
            },
            validateMagnet: (sourceView, sourceMagnet) => {
                const sourceGroup = sourceView.findAttribute("port-group", sourceMagnet);
                const sourcePort = sourceView.findAttribute("port", sourceMagnet);
                
                const source = sourceView.model;
            
                console.log(sourceMagnet);
                console.log(sourceView);
                console.log(sourceGroup);
                
                if (sourceGroup === TARGET_DATABASE_ENTITY_GROUP_NAME) {
                    console.log(
                    "paper<validateMagnet>",
                    "It's not possible to create a link from an inbound port.");
                    return false;
                }
            
                if (
                    this.graph
                        .getConnectedLinks(source, { outbound: true })
                        .find((link) => link.source().port === sourcePort)
                ) {
                        console.log(
                        "paper<validateMagnet>",
                        "The port has already an outbound link (we allow only one link per port)"
                    );
                    return false;
                }
            
                return true;
              },
        });
        
        this.paper.el.style.border = `1px solid #e2e2e2`;
        
        // set up the event handlers 
        this.paper.on({
            'element:mouseenter': MappingEditor.onPaperElementMouseEnter,
            'element:mouseleave': MappingEditor.onPaperElementMouseLeave,
            'element:pointerdblclick': (cellView) => {
                const baseEntityShape = cellView.model as BaseEntityShape;
                baseEntityShape.handleDoubleClick();
            },
            'link:mouseenter': MappingEditor.onPaperLinkMouseEnter,
            'link:mouseleave': MappingEditor.onPaperLinkMouseLeave,
            'link:pointerdblclick': (cellView) => { // TODO: REMOVE and use this for the joinLink to open the modal 
                if (cellView.model instanceof JoinLink) {
                    const customLink = cellView.model as JoinLink;
                    customLink.handleDoubleClick();
                }
            },
            'link:connect': (linkView, evt, elementViewConnected) => { 
                const propertyLink = linkView.model as PropertyLink;
                propertyLink.handleConnection();
            },
            'cell:pointermove': function (cellView, evt, x, y) {
                // Check if the cell being dragged is an element
                if (cellView.model.isElement()) {
                    const element = cellView.model;
                    const boundingBox = element.getBBox();

                    const containerRect = contentDiv.getBoundingClientRect();

                    // Define boundaries of the container
                    // bounding box is relative to the paper
                    const containerMinX = 0;
                    const containerMinY = 0;
                    const containerMaxX = containerRect.width;
                    const containerMaxY = containerRect.height;

                    // Check if the element is trying to move beyond the container boundaries
                    if (boundingBox.x < containerMinX
                        || boundingBox.x + boundingBox.width > containerMaxX
                        || boundingBox.y < containerMinY
                        || boundingBox.y + boundingBox.height > containerMaxY) {
                        // Adjust the position of the element to keep it within container boundaries
                        var newX = Math.min(Math.max(boundingBox.x, containerMinX), containerMaxX - boundingBox.width);
                        var newY = Math.min(Math.max(boundingBox.y, containerMinY), containerMaxY - boundingBox.height);
                        element.position(newX, newY);
                    }
                }
            }
        });

        const addSourceTableButton = document.createElement('button');
        addSourceTableButton.innerText = 'Přidat zdrojovou tabulku';
        addSourceTableButton.addEventListener('click', () => this.openSourceTableSelectionModal());
        this.toolbar?.appendChild(addSourceTableButton);

        const exportMappingButton = document.createElement('button');
        exportMappingButton.innerText = 'Exportovat mapování';
        exportMappingButton.addEventListener('click', () => this.downloadMapping());
        this.toolbar?.appendChild(exportMappingButton);

        this.sourceTablePickerModal = new SourceTablePickerModal(this.sourceDb.tables);

        // TODO: DELETE for production
        const importFile = getElementOrThrow('source-file');
        importFile.addEventListener('change', (event) => {
            const file = (event.target as HTMLInputElement).files?.[0];
            if (file === undefined) return;

            const reader = new FileReader();
            reader.onload = () => {
                const result = reader.result;
                if (result === null) return;

                const entityMapping = EntityMappingConvertor.convertPlainToEntityMapping(JSON.parse(result as string));
                this.loadEntityMapping(entityMapping, this.targetTable!);
            }

            reader.readAsText(file);
        });

    }
 
    /**
     * Checks if the mapping is complete.
     * @returns True if mapper is initialized and all non-nullable columns are mapped, false otherwise.
     */
    public isMappingComplete() : boolean {
        if (this.entityMapping === null || this.targetTable === null) return false;
        for (const column of this.targetTable!.columns) {
            if (!column.dataType.isNullable && this.entityMapping.columnMappings.get(column.name) == null){
                return false;
            }
        }
        return true;
    }

    public createSerializedMapping() {
        console.debug(this.entityMapping);

        const plainEntityMapping = EntityMappingConvertor.convertEntityMappingToPlain(this.entityMapping
            ?? (() => { throw new Error('Entity mapping was not yet initialized.'); })()
        );

        console.debug(plainEntityMapping);

        // Convert the mappings array to a JSON string
        return JSON.stringify(plainEntityMapping, null, 2); // Use 2 spaces for indentation
    }

    public downloadMapping() {
        // Convert the mappings array to a JSON string
        const jsonString = this.createSerializedMapping();

        // Create a Blob with the JSON data
        const blob = new Blob([jsonString], { type: 'application/json' });

        // Create a URL from the Blob
        const url = URL.createObjectURL(blob);


        // Create a link element
        const downloadLink = document.createElement('a');
        downloadLink.href = url;
        downloadLink.download = 'mappings.json'; // Specify the filename for the downloaded file
        downloadLink.click();

        // Clean up the URL object after the download is initiated
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

        const entityMapping = new EntityMapping(
            targetTable.name,
            targetTable.schema,
            null,
            [],
            columnMappings);

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

    public loadEntityMapping(entityMapping : EntityMapping, targetTable: Table) {
        this.targetTable = targetTable;
        this.entityMapping = entityMapping;
        this.paper.freeze();
        const cells = this.convertEntityMappingToShapes(entityMapping);
        
        // Add the cells to the graph
        this.graph.resetCells(cells);

        // layout the cells - source https://www.jointjs.com/demos/directed-graph-layout
        DirectedGraph.layout(this.graph, {
            nodeSep: 200,
            edgeSep: 80,
            rankDir: "LR"
        });

        this.paper.unfreeze();
    } 

    public loadSerializedEntityMapping(serializedEntityMapping: string, serializedTable: string) {
        const plainEntityMapping = JSON.parse(serializedEntityMapping);
        const entityMapping = EntityMappingConvertor.convertPlainToEntityMapping(plainEntityMapping);

        // TODO: deserialize the serializedTable and map it
        // this.loadEntityMapping(entityMapping);
    }

    private openSourceTableSelectionModal() {
        this.sourceTablePickerModal.open(() => this.addSourceTable(this.sourceTablePickerModal.tableToAdd
             ?? (() => { throw new Error('Table to add should be initialized by the modal.'); })()));
    }

    private addSourceTable(table: Table) {
        // first select all columns - they might be used in join 
        const sourceTable = new SourceTable(table.name, table.schema, table.columns.map(column => new SourceColumn(column)));
        sourceTable.createBackwardConnections();

        const sourceEntity = this.entityMapping!.sourceEntity;
        console.debug(this.entityMapping);
        // if there is no source entity, add the source table to the graph
        if (sourceEntity === null) {
            const sourceTableShape = new SourceTableShape(sourceTable, table);
            this.entityMapping!.sourceEntity = sourceTable;
            this.entityMapping!.createBackwardConnections();
            this.graph.addCell(sourceTableShape);
            sourceTableShape.columnSelectionModal.open();
            return;
        }

        // otherwise, create a join between the source entity and the new source table
        const join = new Join(`join_${sourceEntity.name}_${sourceTable.name}`, JoinType.inner, sourceEntity, sourceTable);

        const joinModal = new JoinModal(join);
        const sourceTableData = new IntermediateSourceTableData(sourceTable, table, joinModal, join);
        joinModal.open(() => this.finishJoiningSourceTable(sourceTableData));
    }
    
    /**
     * After join definition is finished, this method is called to finish the joining process.
     * Handles the creation of the join link and the right source table shape.
     * @param sourceTableData The intermediate that encapsulates the objects needed to finish the joining process.
     */
    private finishJoiningSourceTable(sourceTableData: IntermediateSourceTableData) : void {
        // TODO: handle first add

        const rightSourceTableShape = new SourceTableShape(sourceTableData.tableToAdd, sourceTableData.tableModel);
        const sourceTableFinder = new SourceTableFinder();
        
        if (this.entityMapping!.sourceEntity == null) throw new Error('Source entity should be initialized.');
        this.entityMapping!.sourceEntity.accept(sourceTableFinder);
        const leftSourceTable = sourceTableFinder.desiredSourceEntity; 

        const leftSourceTableShape = this.graph.getElements().find(element => { 
            if (element instanceof SourceTableShape) {
                const sourceTableShape = element as SourceTableShape;
                if (sourceTableShape.sourceTable === leftSourceTable) {
                    return true;
                }
            }
            return false;
        }) as SourceTableShape;

        const joinLink = new JoinLink(sourceTableData.unfinishedJoin, sourceTableData.joinModal);
        joinLink.source(leftSourceTableShape);
        joinLink.target(rightSourceTableShape);

        this.entityMapping!.sourceEntity = sourceTableData.unfinishedJoin;
        this.entityMapping!.createBackwardConnections();
        
        this.paper.freeze();
        this.graph.addCells([rightSourceTableShape, joinLink]);
        this.paper.unfreeze();
        
        rightSourceTableShape.columnSelectionModal.open();
    }
    private convertEntityMappingToShapes(entityMapping: EntityMapping): dia.Cell[] {
        // initialize the transformer            
        const transformer = new SourceEntitiesToShapesTransformer(this.sourceDb.tables);
        
        // transform the source entities to shapes
        if (entityMapping.sourceEntity !== null) {
            entityMapping.sourceEntity.accept(transformer);
        }
        
        // Create the target entity shape and links between the source and target entities
        const cells = transformer.cells;
        const targetShape = new TargetTableShape(entityMapping, []);
        for (const [targetColumnName, sourceColumn] of entityMapping.columnMappings) {
            // Add port to the target entity
            // note: target table must be set by public methods
            const targetColumn = this.targetTable!.columns.find((column) => column.name === targetColumnName);
            if (targetColumn === undefined) throw new Error('Cannot find the corresponding column');

            const targetPort = targetShape.addPropertyPort(targetColumn);
            if (targetPort === undefined) {
                throw new Error('Cannot create the port for the target column');
            }

            // if it has mapping, add a link
            if (sourceColumn !== null) {
                const sourceElement = transformer.elementMap.get(sourceColumn.owner)
                    ?? (() => { throw new Error('Source element not found.'); })();
                const sourcePort = sourceElement.getPortByColumn(sourceColumn);                
                const link = new PropertyLink();
                link.source({
                    id: sourceElement.id,
                    magnet: "portBody",
                    port: sourcePort.id
                });
                link.target({ 
                    id: targetShape.id,
                    magnet: "portBody",
                    port: targetPort.id
                });
                console.debug('Adding a link');
                console.debug(link);
                cells.push(link);
            }
        }

        cells.push(targetShape);
        return cells;
    }
}