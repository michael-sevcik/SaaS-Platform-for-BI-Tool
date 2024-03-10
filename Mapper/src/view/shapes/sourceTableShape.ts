import { SOURCE_DATABASE_ENTITY_GROUP_NAME, DEFAULT_SOURCE_ENTITY_DESCRIPTION } from '../../constants';
import { EntityMapping } from '../../mappingModel/entityMapping';
import { SourceTable } from '../../mappingModel/sourceTable';
import { ColumnSelectionModal } from '../modals/columnSelectionModal';
import { BaseSourceEntityShape } from './baseSourceEntityShape';
import { PropertyLink } from './propertyLink';


export class SourceTableShape extends BaseSourceEntityShape {
    groupName: string;
    
    defaults() {
        this.groupName = SOURCE_DATABASE_ENTITY_GROUP_NAME;
        return super.defaults();
    }
    
    public handleRemoving(): void {
        for (const link of this.graph.getConnectedLinks(this, { outbound: true })) {
            const propertyLink = link as PropertyLink;
            propertyLink.handleRemoving();
        }
        
        this.sourceTable.owner?.replaceChild(this.sourceTable, null);
        this.remove();
    }

    public handleDoubleClick(): void {
        const columnSelectionModal = new ColumnSelectionModal(this);
        columnSelectionModal.open();
    }
    
    public constructor(public readonly sourceTable: SourceTable, ...args: any[]) {
        super(sourceTable.selectedColumns, ...args);
        this.setTitle(sourceTable.name);
        this.setDescription(sourceTable.description ?? DEFAULT_SOURCE_ENTITY_DESCRIPTION);
    }
}