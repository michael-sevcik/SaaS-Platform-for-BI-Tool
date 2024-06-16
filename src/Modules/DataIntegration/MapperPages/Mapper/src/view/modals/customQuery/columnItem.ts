import type { Column } from "../../../dbModel/database";

/**
 * Represents a column item in the custom query modal.
 */
export class ColumnItem {
    public listItem: HTMLLIElement;

    /**
     * Creates a new instance of the ColumnItem class.
     * @param column The column object.
     * @param removeCallback The callback function to remove the column item.
     */
    constructor(column: Column, removeCallback: () => void) {
        this.listItem = document.createElement('li');
        this.listItem.className = 'list-group-item d-flex justify-content-between align-items-center';
        this.listItem.textContent = `${column.name} (${column.dataType.Descriptor})`;

        const removeBtn = document.createElement('button');
        removeBtn.className = 'btn btn-danger btn-sm';
        removeBtn.textContent = 'Remove';
        removeBtn.addEventListener('click', removeCallback);

        this.listItem.appendChild(removeBtn);
    }

    /**
     * Gets the ID of the column item.
     * @returns The ID of the column item.
     */
    getId(): string {
        return this.listItem.id;
    }

    /**
     * Sets the ID of the column item.
     * @param id The ID to set.
     */
    setId(id: string): void {
        this.listItem.id = id;
    }
}
