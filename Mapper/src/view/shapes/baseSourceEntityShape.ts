import { Column } from "../../dbModel/database";
import { BaseEntityShape } from "./baseEntityShape";

export abstract class BaseSourceEntityShape extends BaseEntityShape {
    public constructor(
        public readonly selectedColumns: Column[],
        ...args: any[]
    ) {
        super(selectedColumns, ...args);
    }

    public abstract handleRemoving(): void;
}