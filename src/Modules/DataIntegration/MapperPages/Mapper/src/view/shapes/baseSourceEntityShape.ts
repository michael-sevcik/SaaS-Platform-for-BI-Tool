import { Column } from "../../dbModel/database";
import { SourceColumn } from "../../mappingModel/sourceColumn";
import { BaseEntityShape } from "./baseEntityShape";

/**
 * Base abstract class for source entity shapes.
 */
/**
 * Represents the base class for source entity shapes.
 */
export abstract class BaseSourceEntityShape extends BaseEntityShape {
    /**
     * Creates a new instance of the BaseSourceEntityShape class.
     * @param selectedColumns The selected columns for the source entity.
     * @param args Additional arguments.
     */
    public constructor(
        public readonly selectedColumns: Column[],
        ...args: any[]
    ) {
        super(selectedColumns, ...args);
    }

    /**
     * Handles the removal of the source entity shape.
     */
    public abstract handleRemoving(): void;

    /**
     * Gets the unique name of the source entity shape.
     * @returns The unique name of the source entity shape.
     */
    public abstract get uniqueName(): string;

    /**
     * Handles the placement of the source entity shape on the paper.
     */
    public abstract onPaperPlacement(): void;

    /**
     * Gets the source column by its port ID.
     * @param portId The port ID of the source column.
     * @returns The source column with the specified port ID.
     * @throws Error if the column is not of type SourceColumn.
     */
    public getSourceColumnByPortId(portId: string): SourceColumn {
        const column = this.getColumnByPortId(portId);
        if (!(column instanceof SourceColumn)) {
            throw new Error('Columns of source entities must be of type SourceColumn.');
        }

        return column as SourceColumn;
    }
}