import { SOURCE_DATABASE_ENTITY_GROUP_NAME, DEFAULT_SOURCE_ENTITY_DESCRIPTION } from '../../constants';
import type { Column, Table } from '../../dbModel/database';
import { EntityMapping } from '../../mappingModel/entityMapping';
import { SourceTable } from '../../mappingModel/sourceTable';
import { ColumnSelectionModal } from '../modals/columnSelectionModal';
import { BaseSourceEntityShape } from './baseSourceEntityShape';
import { PropertyLink } from './propertyLink';


/**
 * Represents a shape for a {@link SourceTable} in the mapper view.
 */
export class SourceTableShape extends BaseSourceEntityShape {
    groupName: string;
    public readonly columnSelectionModal: ColumnSelectionModal;

    /**
     * Creates an instance of SourceTableShape.
     * @param sourceTable The source table associated with the shape.
     * @param tableModel The table model associated with the shape.
     * @param args Additional arguments.
     */
    public constructor(
        public readonly sourceTable: SourceTable,
        public readonly tableModel: Table,
        ...args: any[]
    ) {
        super(sourceTable.selectedColumns, ...args);
        this.setTitle(sourceTable.name);
        this.columnSelectionModal = new ColumnSelectionModal(this);
        this.setDescription(sourceTable.description ?? DEFAULT_SOURCE_ENTITY_DESCRIPTION);
    }

    defaults() {
        this.groupName = SOURCE_DATABASE_ENTITY_GROUP_NAME;
        return super.defaults();
    }

    /** @inheritdoc */
    public handleDoubleClick(): void {
        this.columnSelectionModal.open();
    }

    /**
     * Removes the port and corresponding column from the source table shape.
     * @param column The column to be removed.
     */
    public removePortAndCorrespondingColumn(column: Column): void {
        const port = this.getPortByColumn(column);
        this.sourceTable.removeSelectedColumn(column);
        this.removePort(port);
    }

    /**
     * Adds a port and corresponding column to the source table shape.
     * @param column The column to be added.
     */
    public addPortAndCorrespondingColumn(column: Column): void{
        const sourceColumn = this.sourceTable.addSelectedColumn(column);
        this.addPropertyPort(sourceColumn)
    }

    /**
     * Handles the removing of the source table shape.
     * This method is responsible for removing connected links, replacing the source table with null,
     * finalizing the column selection modal, and removing the shape itself.
     */
    public handleRemoving(): void {
        for (const link of this.graph.getConnectedLinks(this, { outbound: true })) {
            const propertyLink = link as PropertyLink;
            propertyLink.handleRemoving();
        }

        this.sourceTable.owner?.replaceChild(this.sourceTable, null);
        this.columnSelectionModal.finalize();
        this.remove();
    }
}