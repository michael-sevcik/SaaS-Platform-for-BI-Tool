import { SourceTableShape } from "../shapes/sourceTableShape";
import { BaseModal } from "./baseModal";


export class ColumnSelectionModal extends BaseModal {
    protected name: string;
    protected save(): boolean {
        throw new Error("Method not implemented.");
    }
    protected cancel(): void {
    }

    public constructor(public readonly sourceTableShape : SourceTableShape) {
        super();
    }


}