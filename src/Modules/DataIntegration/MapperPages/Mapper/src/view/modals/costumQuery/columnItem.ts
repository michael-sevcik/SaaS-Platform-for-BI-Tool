import type { Column } from "../../../dbModel/database";

export class ColumnItem {
    public listItem: HTMLLIElement;

    constructor(column: Column, removeCallback: () => void) {
        this.listItem = document.createElement('li');
        this.listItem.className = 'list-group-item d-flex justify-content-between align-items-center';
        this.listItem.textContent = `${column.name} (${column.dataType.Descriptor}) ${column.dataType.isNullable ? 'NULL' : 'NOT NULL'}`;

        const removeBtn = document.createElement('button');
        removeBtn.className = 'btn btn-danger btn-sm';
        removeBtn.textContent = 'Remove';
        removeBtn.addEventListener('click', removeCallback);

        this.listItem.appendChild(removeBtn);
    }

    getId(): string {
        return this.listItem.id;
    }

    setId(id: string): void {
        this.listItem.id = id;
    }
}
