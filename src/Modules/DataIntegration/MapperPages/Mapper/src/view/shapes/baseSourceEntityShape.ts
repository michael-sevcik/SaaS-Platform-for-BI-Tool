import { Column } from "../../dbModel/database";
import { SourceColumn } from "../../mappingModel/sourceEntities/sourceColumn";
import { BaseEntityShape } from "./baseEntityShape";
import type { Link } from "./link";

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
     * 
     * This method removes all connected links and the this shape.
     * 
     */
    public handleRemoving(): void {
        for (const link of this.graph.getConnectedLinks(this, {inbound: true})) {
            const connectedLink = link as Link;
            connectedLink.handleTargetRemoval();
        }

        for (const link of this.graph.getConnectedLinks(this, { outbound: true })) {
            const connectedLink = link as Link;
            connectedLink.handleRemoving();
        }

        this.remove();
    }

    /**
     * Handles the removal of the source entity shape.
     * 
     * This method removes all connected links and this shape.
     */
    public handleOwnerRemoval(): void {
        for (const link of this.graph.getConnectedLinks(this, { outbound: true })) {
            const connectedLink = link as Link;
            connectedLink.handleRemoving();
        }

        this.remove();
    }

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