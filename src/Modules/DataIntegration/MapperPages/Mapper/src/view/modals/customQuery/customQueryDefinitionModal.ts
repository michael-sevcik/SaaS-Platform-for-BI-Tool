import type { Column } from "../../../dbModel/database";
import { SourceColumn } from "../../../mappingModel/sourceEntities/sourceColumn";
import { CustomQuery } from "../../../mappingModel/sourceEntities/customQuery";
import { TextInput } from "../../elements/textInput";
import { BaseModal } from "../baseModal";
import { QueryDefinitionForm } from "./queryDefinitionForm";
import { SelectedColumnAdditionForm } from "./selectedColumnAddtionForm";

export class CustomQueryDefinitionModal extends BaseModal {
    /** @inheritdoc */
    protected name: string = 'Define a custom query for the source table';

    private columnForm: SelectedColumnAdditionForm;
    private queryForm: QueryDefinitionForm;
    private nameInput: TextInput;
    private _customQuery: CustomQuery | null = null;

    public get customQuery(): CustomQuery | null {
        return this._customQuery;
    }

    public constructor() {
        super();
        this.setTitle(this.name);

        // Make modal content scrollable
        this.modalContent.style.overflowY = 'auto';
        this.modalContent.style.maxHeight = '80vh'; // Set a max height for the modal content

        
        this.nameInput = new TextInput('Custom-query-def-modal-name-input', 'Unique name', undefined, false);
        this.modalContent.appendChild(this.nameInput.input);

        this.queryForm = new QueryDefinitionForm(this.modalContent);
        this.columnForm = new SelectedColumnAdditionForm(this.modalContent);
    }

    /**
     * Extracts the selected columns from the form.
     * @returns The list of selected columns as Column instances.
     */
    private extractSelectedColumns(): Column[] {
        return this.columnForm.extractSelectedColumns();
    }

    /**
     * Gets the SQL query from the form.
     * @returns The SQL query.
     */
    private getSqlQuery(): string {
        return this.queryForm.getSqlQuery();
    }

    /** @inheritdoc */
    protected cancel(): void {
        // Handle cancel action
        console.log('Cancel button clicked.');
    }

    /** @inheritdoc */
    protected save(): boolean {
        // Handle save action
        if (this.nameInput.getValue() === '') {
            alert('Please enter a unique name for the custom query.');
            return false;
        }

        const columns = this.extractSelectedColumns();
        const sqlQuery = this.getSqlQuery();
        this._customQuery = new CustomQuery(this.nameInput.getValue(), sqlQuery, columns.map(c => new SourceColumn(c)));
        console.debug('Save button clicked. SQL Query:', sqlQuery, 'Selected Columns:', columns);
        return true;
    }
}
