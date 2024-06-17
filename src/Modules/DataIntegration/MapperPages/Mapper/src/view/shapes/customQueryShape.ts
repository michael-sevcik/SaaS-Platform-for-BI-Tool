import { SOURCE_DATABASE_ENTITY_GROUP_NAME, DEFAULT_SOURCE_ENTITY_DESCRIPTION, DEFAULT_CUSTOM_QUERY_DESCRIPTION } from '../../constants';
import type { Column, Table } from '../../dbModel/database';
import { EntityMapping } from '../../mappingModel/entityMapping';
import type { CustomQuery } from '../../mappingModel/sourceEntities/customQuery';
import { SourceTable } from '../../mappingModel/sourceEntities/sourceTable';
import { ColumnSelectionModal } from '../modals/columnSelectionModal';
import { QueryPreviewModal } from '../modals/customQuery/queryPreviewModal';
import { BaseSourceEntityShape } from './baseSourceEntityShape';
import { PropertyLink } from './propertyLink';


/**
 * Represents a shape for a {@link CustomQuery} in the mapper view.
 */
export class CustomQueryShape extends BaseSourceEntityShape {
    public onPaperPlacement(): void {
        // nothing to do here.
    }
    public get uniqueName(): string {
        return this.customQuery.fullName;
    }
    
    groupName: string;

    /**
     * Creates an instance of SourceTableShape.
     * @param customQuery The source table associated with the shape.
     * @param tableModel The table model associated with the shape.
     * @param args Additional arguments.
     */
    public constructor(
        public readonly customQuery: CustomQuery,
        ...args: any[]
    ) {
        super(customQuery.selectedColumns, ...args);
        this.setTitle(customQuery.name);
        this.setDescription(customQuery.description ?? DEFAULT_CUSTOM_QUERY_DESCRIPTION);
    }

    defaults() {
        this.groupName = SOURCE_DATABASE_ENTITY_GROUP_NAME;
        return super.defaults();
    }

    /** @inheritdoc */
    public handleDoubleClick(): void {
        const modal = new QueryPreviewModal(this.customQuery.query);
        modal.open(null, true);
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

        this.customQuery.owner?.replaceChild(this.customQuery, null);        
        this.remove();
    }
}