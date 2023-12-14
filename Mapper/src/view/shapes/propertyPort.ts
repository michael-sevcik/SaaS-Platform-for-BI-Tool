import { dia } from "jointjs";
import { Column } from "../../dbModel/database";

export class PropertyPort implements dia.Element.Port {
    public attrs: dia.Cell.Selectors
    public id?: string;

    constructor(
        public column: Column,
        public group: string) {
        this.attrs = { portLabel: { text: `${column.name}: ${column.type}`} };
    }

    /**
     * Tests whether the column of this port is assignable with the column of the given port.
     * @param port The port representing the column to test.
     * @returns true if the column of this port is assignable with the column of the given port, false otherwise.
     */
    public isAssignableWith(port: PropertyPort): boolean {
        return this.column.isAssignableWith(port.column);
    }
}