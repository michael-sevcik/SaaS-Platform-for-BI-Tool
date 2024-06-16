import type { Column } from "../../../dbModel/database";
import { BaseModal } from "../baseModal";
import { QueryDefinitionForm } from "./queryDefinitionForm";
import { SelectedColumnAdditionForm } from "./selectedColumnAddtionForm";

/**
 * Represents a modal for displaying a query preview.
 */
export class QueryPreviewModal extends BaseModal {
    /** @inheritdoc */
    protected name: string = 'Query Preview';

    /**
     * Creates a new instance of QueryPreviewModal.
     * @param query - The query to be displayed in the modal.
     */
    public constructor(query: string) {
        super();
        this.setTitle(this.name);

        // Make modal content scrollable
        this.modalContent.style.overflowY = 'auto';
        this.modalContent.style.maxHeight = '80vh'; // Set a max height for the modal content

        new QueryDefinitionForm(this.modalContent, query, false);
    }

    /** @inheritdoc */
    protected cancel(): void {
        // Handle cancel action
        console.debug('Cancel button clicked.');
    }

    /** @inheritdoc */
    protected save(): boolean {
        return true;
    }
}
