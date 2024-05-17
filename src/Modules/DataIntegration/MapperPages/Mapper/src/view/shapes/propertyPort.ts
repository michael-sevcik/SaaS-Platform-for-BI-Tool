import { dia } from "@joint/core";
import { Column } from "../../dbModel/database";

export class PropertyPort implements dia.Element.Port {
    public attrs: dia.Cell.Selectors
    public id?: string;

    constructor(
        public column: Column,
        public group: string) {
        this.attrs = {
            portLabel: { text: `${column.name}: ${column.type}`},
        };

        if (column.description !== null) {
            this.setDescription(column.description);
        }
    }

    /**
     * Tests whether the column of this port is assignable with the column of the given port.
     * @param port The port representing the column to test.
     * @returns true if the column of this port is assignable with the column of the given port, false otherwise.
     */
    public isAssignableWith(port: PropertyPort): boolean {
        return this.column.isAssignableWith(port.column);
    }

    public setDescription(description: string ) {
        this.attrs.rect = { title: description };
    }
}