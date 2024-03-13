import { dia } from 'jointjs';
import {
    DATABASE_ENTITY_PROPERTY_WIDTH,
    DATABASE_ENTITY_PROPERTY_HEIGHT,
    DATABASE_ENTITY_PROPERTY_GAP,
    DATABASE_ENTITY_MAX_PORT_COUNT,
    HEADER_HEIGHT,
    PADDING_L,
    headerAttributes,
    propertyPosition,
    propertyAttributes,
    

 } from '../../constants';
import { Column } from '../../dbModel/database';
import { PropertyPort } from './propertyPort';

// Provides a base implementation for database entity shapes.
export abstract class BaseEntityShape extends dia.Element {
    // This entity must be implemented by the extending class for adding ports during initializetion.
    protected abstract groupName : string;
    private _portsByColumns : Map<Column, PropertyPort>;

    defaults() {
        console.log("base entity defaults");
        console.log(this.groupName);
        return {
            ...super.defaults,
            ...headerAttributes,
            type: 'DatabaseEntityShape',
            size: {
                width: DATABASE_ENTITY_PROPERTY_WIDTH,
                height: 0
            },
            ports: {
                groups: {
                    [this.groupName]: {
                        position: propertyPosition,
                        ...propertyAttributes
                    }
                },
                items: []
            },
            label: {
                text: 'ahoj'
            }
        }
    }

    public constructor(columns: Column[], ...args: any[]) {
        super();
        console.log('BaseEntityShape.initialize()');
        console.log(columns);
        this._portsByColumns = new Map<Column, PropertyPort>();
        this.on('change:ports', () => this.resizeToFitPorts());
        console.log(columns);
        this.addPropertyPorts(columns);
        this.resizeToFitPorts();
        super.initialize.call(this, ...args);
    }
    
    public setTitle(title: string): void {
        this.attr('label/text', title);
    }

    public setDescription(title: string): void {
        this.attr('description/text', title);
    }

    private resizeToFitPorts() {
        const { length } = this.getPorts();
        this.prop(['size', 'height'], HEADER_HEIGHT + (DATABASE_ENTITY_PROPERTY_HEIGHT + DATABASE_ENTITY_PROPERTY_GAP) * length + PADDING_L);
    }

    public addPropertyPort(column: Column) : PropertyPort {
        if (!this.canAddPort(this.groupName)) {
            console.log('Cannot add more ports');
            return;
        }

        const port = new PropertyPort(column, this.groupName);
        this.addPort(port);
        this._portsByColumns.set(column, port);
        return port;
    }

    public addPropertyPorts(columns: Column[]) : void{
        for (const column of columns) {
            this.addPropertyPort(column);
        }
    }

    canAddPort(group: string): boolean {
        return Object.keys(this.getGroupPorts(group)).length < DATABASE_ENTITY_MAX_PORT_COUNT;
    }

    public getPortByColumn(column: Column): PropertyPort | undefined {
        return this._portsByColumns.get(column);
    }

    abstract handleDoubleClick(): void;
}
