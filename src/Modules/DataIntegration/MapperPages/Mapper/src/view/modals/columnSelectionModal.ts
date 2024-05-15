import { SourceTableShape } from "../shapes/sourceTableShape";
import { BaseModal } from "./baseModal";


export class ColumnSelectionModal extends BaseModal {
    protected name: string;

    public constructor(public readonly sourceTableShape : SourceTableShape) {
        super();
    }

    protected cancel(): void {
    }

    protected save(): boolean {
        throw new Error("Method not implemented.");
    }
}