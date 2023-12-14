import { TARGET_DATABASE_ENTITY_GROUP_NAME } from '../../constants';
import { Column } from '../../dbModel/database';
import { EntityMapping } from '../../mappingModel/entityMapping';
import { SourceColumn } from '../../mappingModel/sourceColumn';
import { BaseEntityShape } from './baseEntityShape';
import { PropertyPort } from './propertyPort';
import { TargetElementShape } from './targetElementShape';

export class TargetTableShape extends BaseEntityShape implements TargetElementShape {
    groupName: string;

    defaults() {
        this.groupName = TARGET_DATABASE_ENTITY_GROUP_NAME;
        return super.defaults();
    }

    public constructor(private readonly entityMapping: EntityMapping, columns: Column[], ...args: any[]) {
        super(columns, ...args);
        this.setTitle(entityMapping.name);
        // TODO: add description
    }
    
    public removeColumnMapping(targetPort: PropertyPort) {
        this.entityMapping.removeColumnMapping(targetPort.column);
        console.log(this.entityMapping);
        
    }

    public setColumnMapping(sourcePort: PropertyPort, targetPort: PropertyPort) {
        this.entityMapping.setColumnMapping(sourcePort.column as SourceColumn, targetPort.column);
        console.log(this.entityMapping);
        
    }
}