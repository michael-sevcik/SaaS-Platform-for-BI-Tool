import { TARGET_DATABASE_ENTITY_GROUP_NAME, DEFAULT_TARGET_ENTITY_DESCRIPTION } from '../../constants';
import { Column } from '../../dbModel/database';
import { EntityMapping } from '../../mappingModel/entityMapping';
import { SourceColumn } from '../../mappingModel/sourceEntities/sourceColumn';
import { BaseEntityShape } from './baseEntityShape';
import { PropertyPort } from './propertyPort';
import { TargetElementShape } from './targetElementShape';

export class TargetTableShape extends BaseEntityShape implements TargetElementShape {
    groupName: string;

    public constructor(public readonly entityMapping: EntityMapping, columns: Column[], ...args: any[]) {
        super(columns, ...args);
        this.setTitle(entityMapping.name);
        this.setDescription(entityMapping.description ?? DEFAULT_TARGET_ENTITY_DESCRIPTION);
    }

    defaults() {
        this.groupName = TARGET_DATABASE_ENTITY_GROUP_NAME;
        return super.defaults();
    }

    handleDoubleClick(): void {
        // no use for this event yet
    }

    /** @inheritdoc */
    public removeColumnMapping(targetColumn: Column) {
        this.entityMapping.removeColumnMapping(targetColumn);
        console.log(this.entityMapping);
        
    }

    /** @inheritdoc */
    public setColumnMapping(sourceColumn: SourceColumn, targetColumn: Column) {
        this.entityMapping.setColumnMapping(sourceColumn, targetColumn);
        console.log(this.entityMapping);
    }
}