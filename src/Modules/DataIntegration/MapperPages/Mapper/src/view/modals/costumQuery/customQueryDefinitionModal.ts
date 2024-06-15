import type { Column } from "../../../dbModel/database";
import { BaseModal } from "../baseModal";
import { QueryDefinitionForm } from "./QueryDefinitionForm";
import { SelectedColumnAdditionForm } from "./selectedColumnAddtionForm";

export class CustomQueryDefinitionModal extends BaseModal {
    /** @inheritdoc */
    protected name: string = 'Define a custom query for the source table';

    private columnForm: SelectedColumnAdditionForm;
    private queryForm: QueryDefinitionForm;

    public constructor() {
        super();
        this.setTitle(this.name);

        // Make modal content scrollable
        this.modalContent.style.overflowY = 'auto';
        this.modalContent.style.maxHeight = '80vh'; // Set a max height for the modal content

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
        const columns = this.extractSelectedColumns();
        const sqlQuery = this.getSqlQuery();
        console.log('Save button clicked. SQL Query:', sqlQuery, 'Selected Columns:', columns);
        return true;
    }
}
