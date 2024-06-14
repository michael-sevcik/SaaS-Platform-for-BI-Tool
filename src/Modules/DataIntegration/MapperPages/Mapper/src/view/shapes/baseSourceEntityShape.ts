import { Column } from "../../dbModel/database";
import { SourceColumn } from "../../mappingModel/sourceColumn";
import { BaseEntityShape } from "./baseEntityShape";

/**
 * Base abstract class for source entity shapes.
 */
export abstract class BaseSourceEntityShape extends BaseEntityShape {
    public constructor(
        public readonly selectedColumns: Column[],
        ...args: any[]
    ) {
        super(selectedColumns, ...args);
    }

    public abstract handleRemoving(): void;

    public getSourceColumnByPortId(portId: string): SourceColumn {
        const column = this.getColumnByPortId(portId);
        if (!(column instanceof SourceColumn)) {
            throw new Error('Columns of source entities must be of type SourceColumn.');
        }

        return column as SourceColumn;
    }
}