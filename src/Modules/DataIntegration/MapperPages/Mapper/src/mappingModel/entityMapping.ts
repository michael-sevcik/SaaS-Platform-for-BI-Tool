import 'reflect-metadata';

import type { SourceColumn } from './sourceEntities/sourceColumn';
import { Owner } from './owner';
import { Ownable } from './ownable';
import { Column } from '../dbModel/database';
import type { SourceEntity } from './sourceEntities/sourceEntity';

/**
 * Represents a mapping of source entities on a target entities.
 */
export class EntityMapping implements Owner{
    public constructor(
        public readonly name: string,
        public readonly schema: string | null,
        public sourceEntity : SourceEntity | null,
        public sourceEntities : SourceEntity[],
        public columnMappings : Map<string, SourceColumn | null>,
        public readonly description : string | null = null) { }
    removeReferences(): void {
        this.columnMappings.forEach((value, key) => {
            if (value !== null) {
                value.removeReference(this);
            }
        });
    }
    
    public createBackwardConnections(): void {
        if (this.sourceEntity !== null) {
            this.sourceEntity.owner = this;
            this.sourceEntity.createBackwardConnections();
        }

        this.columnMappings.forEach((value, key) => {
            if (value !== null) {
                value.addReference(this);
            }
        });
    }

    /**
     * Removes the column mapping for the specified column.
     * 
     * @param column - The column to remove the mapping for.
     */
    removeColumnMapping(column: Column) {
        const oldColumn = this.columnMappings.get(column.name);
        if (oldColumn !== undefined && oldColumn !== null) {
            oldColumn.removeReference(this);
        }
        
        this.columnMappings.set(column.name, null);
    }

    replaceChild(oldChild: Ownable | null, newChild: SourceEntity | null): void {
        if (oldChild !== this.sourceEntity) {
            throw new Error('Invalid child');
        }
        
        this.sourceEntity = newChild;
        this.createBackwardConnections();
    }
    
    /**
     * Sets the sourceColumn as the mapping of the targetColumn.
     * @param sourceColumn
     * @param targetColumn 
     */
    setColumnMapping(sourceColumn: SourceColumn, targetColumn: Column) {
        const oldColumn = this.columnMappings.get(targetColumn.name);
        if (oldColumn !== undefined && oldColumn !== null) {
            oldColumn.removeReference(this);
        }

        this.columnMappings.set(targetColumn.name, sourceColumn);
        sourceColumn.addReference(this);
    }
}
