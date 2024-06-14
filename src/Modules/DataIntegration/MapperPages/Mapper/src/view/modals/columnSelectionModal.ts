import type { Column } from "../../dbModel/database";
import type { SourceColumn } from "../../mappingModel/sourceColumn";
import { SourceTableShape } from "../shapes/sourceTableShape";
import { BaseModal } from "./baseModal";


export class ColumnSelectionModal extends BaseModal {
    protected name: string = 'Select columns which will be used in the mapping';
    private readonly idPrefix : string;
    private readonly columnCheckboxesByNames: Map<string, HTMLInputElement> = new Map();
    private readonly changes = new Map<string, boolean>();

    public constructor(public readonly sourceTableShape : SourceTableShape) {
        super();
        this.setTitle(this.name);
        // TODO: consider adding deselect all button

        this.idPrefix = `${sourceTableShape.tableModel.schema}-${sourceTableShape.sourceTable.name}-column-selection-modal`;
        const optionDiv = document.createElement('div');
        optionDiv.classList.add('overflow-auto');
        optionDiv.style.height = '40vh';
        this.modalContent.appendChild(optionDiv);

        // TODO: FOR each column in sourceTableShape.tableModel.columns create a checkbox and add it to the modalContent
        this.sourceTableShape.tableModel.columns.sort((a, b) => a.name > b.name ? 1 : -1)
        .forEach((column) => {
            const [div, checkbox] = this.createCheckbox(column);
            this.columnCheckboxesByNames.set(column.name, checkbox);
            optionDiv.appendChild(div);
        });

        // check selected columns
        for (const column of sourceTableShape.sourceTable.selectedColumns) {
            const checkbox = this.columnCheckboxesByNames.get(column.name)
                ?? (() => { throw new Error(`Checkbox for column ${column.name} not found`) })();
            checkbox.checked = true;
        }

        this.disableCheckboxesOfReferencedColumns();
    }

    protected override onOpen(): void {
        super.onOpen();
        this.disableCheckboxesOfReferencedColumns();
    }

    /**
     * Creates a checkbox for the given column.
     * @param column Column for which the checkbox will be created.
     * @returns A tuple containing a div element and an input element.
     */
    private createCheckbox(column : Column): [HTMLDivElement, HTMLInputElement] {
        const div = document.createElement('div');
        div.classList.add('form-check');

        const input = document.createElement('input');
        input.type = 'checkbox';
        input.id = `${this.idPrefix}-${column.name}`;
        input.value = column.name;
        input.classList.add('form-check-input');
        // TODO: check whether the handler is called when the checkbox is set from code.
        input.addEventListener('change', (event) => {
            const target = event.target as HTMLInputElement;
            this.changes.set(target.value, target.checked);
            console.log(`Name: ${target.name}, Value: ${target.value}, Checked: ${target.checked}`);
        });
        div.appendChild(input);

        const label = document.createElement('label');
        label.innerText = `${column.name} (${column.dataType.Descriptor})`;
        label.classList.add('form-check-label');
        label.htmlFor = input.id;
        div.appendChild(label);
        
        return [div, input];
    }

    /**
     * Disables checkboxes of columns which are referenced.
     */
    private disableCheckboxesOfReferencedColumns() {
        for (const sourceColumn of this.sourceTableShape.sourceTable.selectedColumns) {
            const checkbox = this.columnCheckboxesByNames.get(sourceColumn.name)
                ?? (() => { throw new Error(`Checkbox for column ${sourceColumn.name} not found`) })();
            if (sourceColumn.isUsed()) {
                checkbox.checked = true;
                checkbox.disabled = true;
                checkbox.title = 'This column is already used in the mapping, so it cannot be deselected';
            }
            else {
                checkbox.disabled = false;
                checkbox.title = '';
            }
        }
    }

    protected cancel(): void {
    }

    private processChanges(): void {
        if (this.changes.size === 0) return;

        const columnsByName = new Map<string, Column>();
        for (const column of this.sourceTableShape.tableModel.columns) {
            columnsByName.set(column.name, column);
        }
        
        const sourceColumnsByName = new Map<string, SourceColumn>();
        for (const sourceColumn of this.sourceTableShape.sourceTable.selectedColumns) {
            sourceColumnsByName.set(sourceColumn.name, sourceColumn);
        }

        for (const [columnName, checked] of this.changes) {
            if (checked) {
                const column = columnsByName.get(columnName);
                if (column === undefined) {
                    throw new Error(`Column ${columnName} not found`);
                }

                this.sourceTableShape.addPortAndCorrespondingColumn(column)
            }
            else {
                const sourceColumn = sourceColumnsByName.get(columnName);
                if (sourceColumn === undefined) {
                    throw new Error(`Source column ${columnName} not found`);
                }

                this.sourceTableShape.removePortAndCorrespondingColumn(sourceColumn);
            }
        }

        this.changes.clear();
    }

    protected save(): boolean {
        this.processChanges();
        return true;
    }
}