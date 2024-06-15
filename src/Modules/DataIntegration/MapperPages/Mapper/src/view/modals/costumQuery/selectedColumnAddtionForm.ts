import { Column } from "../../../dbModel/database";
import { NVarChar, NVarCharMax, SimpleDataTypes, SimpleType, UnknownDataType, type DataTypeBase } from "../../../dbModel/dataTypes";
import { CheckboxInput } from "../../elements/checkboxInput";
import { SelectInput } from "../../elements/selectInput";
import { TextInput } from "../../elements/textInput";
import { ColumnItem } from "./columnItem";
export class SelectedColumnAdditionForm {
    private container: HTMLElement;
    private columnMap: Map<string, Column>;
    private colNameInput: TextInput;
    private dataTypeSelect: SelectInput;
    private nvarcharLengthInput: TextInput;
    private otherTypeInput: TextInput;
    private nullabilityCheckbox: CheckboxInput;
    private columnList: HTMLUListElement;
    private addColumnBtn: HTMLButtonElement;
    private lengthInputDiv: HTMLDivElement;
    private otherInputDiv: HTMLDivElement;

    constructor(container: HTMLElement) {
        this.container = container;
        this.columnMap = new Map();
        this.initializeForm();
        this.addEventListeners();
    }

    /**
     * Initializes the form elements and appends them to the container.
     */
    private initializeForm(): void {
        const form = document.createElement('form');
        form.id = 'column-addition-form';

        form.appendChild(this.createFormRow1());
        form.appendChild(this.createFormRow2());
        form.appendChild(this.createAddColumnButton());
        form.appendChild(this.createColumnsHeading());
        form.appendChild(this.createColumnList());

        this.container.appendChild(form);
    }

    /**
     * Creates the first form row containing column name input and data type selection.
     * @returns {HTMLDivElement} The first form row element.
     */
    private createFormRow1(): HTMLDivElement {
        const formRow1 = document.createElement('div');
        formRow1.classList.add('form-row');

        formRow1.appendChild(this.createColNameDiv());
        formRow1.appendChild(this.createDataTypeDiv());

        return formRow1;
    }

    /**
     * Creates the column name input field.
     * @returns {HTMLDivElement} The column name input field div.
     */
    private createColNameDiv(): HTMLDivElement {
        const colNameDiv = document.createElement('div');
        colNameDiv.classList.add('form-group', 'col-md-6');
        const colNameLabel = document.createElement('label');
        colNameLabel.htmlFor = 'selected-column';
        colNameLabel.innerText = 'Selected Column Name:';
        this.colNameInput = new TextInput('selected-column', 'Enter column name');
        colNameDiv.appendChild(colNameLabel);
        colNameDiv.appendChild(this.colNameInput.input);
        return colNameDiv;
    }

    /**
     * Creates the data type selection field.
     * @returns {HTMLDivElement} The data type selection field div.
     */
    private createDataTypeDiv(): HTMLDivElement {
        const dataTypeDiv = document.createElement('div');
        dataTypeDiv.classList.add('form-group', 'col-md-6');
        const dataTypeLabel = document.createElement('label');
        dataTypeLabel.htmlFor = 'data-type';
        dataTypeLabel.innerText = 'Select Data Type:';
        const dataTypes = [
            'TinyInteger', 'SmallInteger', 'Integer', 'BigInteger', 'Money',
            'Float', 'Decimal', 'Numeric', 'Boolean', 'Date', 'Datetime',
            'Datetime2', 'DatetimeOffset', 'Timestamp', 'Time', 'nvarchar(max)',
            'nvarchar(length)', 'other'
        ];
        this.dataTypeSelect = new SelectInput('data-type', dataTypes);
        this.dataTypeSelect.select.onchange = this.handleSelectionChange.bind(this);
        dataTypeDiv.appendChild(dataTypeLabel);
        dataTypeDiv.appendChild(this.dataTypeSelect.select);
        return dataTypeDiv;
    }

    /**
     * Creates the second form row containing additional inputs for data types and nullability.
     * @returns {HTMLDivElement} The second form row element.
     */
    private createFormRow2(): HTMLDivElement {
        const formRow2 = document.createElement('div');
        formRow2.classList.add('form-row');

        formRow2.appendChild(this.createLengthInputDiv());
        formRow2.appendChild(this.createOtherTypeDiv());
        formRow2.appendChild(this.createNullabilityDiv());

        return formRow2;
    }

    /**
     * Creates the length input field for nvarchar data type.
     * @returns {HTMLDivElement} The length input field div.
     */
    private createLengthInputDiv(): HTMLDivElement {
        this.lengthInputDiv = document.createElement('div');
        this.lengthInputDiv.classList.add('form-group', 'col-md-6');
        this.lengthInputDiv.id = 'length-input';
        this.lengthInputDiv.style.display = 'none';
        const lengthInputLabel = document.createElement('label');
        lengthInputLabel.htmlFor = 'nvarchar-length';
        lengthInputLabel.innerText = 'Specify Length for nvarchar';
        this.nvarcharLengthInput = new TextInput('nvarchar-length', 'Enter length', 'number');
        this.lengthInputDiv.appendChild(lengthInputLabel);
        this.lengthInputDiv.appendChild(this.nvarcharLengthInput.input);
        return this.lengthInputDiv;
    }

    /**
     * Creates the other type input field for custom data types.
     * @returns {HTMLDivElement} The other type input field div.
     */
    private createOtherTypeDiv(): HTMLDivElement {
        this.otherInputDiv = document.createElement('div');
        this.otherInputDiv.classList.add('form-group', 'col-md-6');
        this.otherInputDiv.id = 'other-input';
        this.otherInputDiv.style.display = 'none';
        const otherTypeLabel = document.createElement('label');
        otherTypeLabel.htmlFor = 'other-type';
        otherTypeLabel.innerText = 'Specify Other Type';
        this.otherTypeInput = new TextInput('other-type', 'Enter type name');
        this.otherInputDiv.appendChild(otherTypeLabel);
        this.otherInputDiv.appendChild(this.otherTypeInput.input);
        return this.otherInputDiv;
    }

    /**
     * Creates the nullability input field for specifying if a column can be null.
     * @returns {HTMLDivElement} The nullability input field div.
     */
    private createNullabilityDiv(): HTMLDivElement {
        const nullabilityDiv = document.createElement('div');
        nullabilityDiv.classList.add('form-group', 'form-check', 'col-md-6');
        this.nullabilityCheckbox = new CheckboxInput('column-nullable', 'Nullable');
        nullabilityDiv.appendChild(this.nullabilityCheckbox.checkbox);
        return nullabilityDiv;
    }

    /**
     * Creates the button to add selected columns to the list.
     * @returns {HTMLButtonElement} The add column button element.
     */
    private createAddColumnButton(): HTMLButtonElement {
        this.addColumnBtn = document.createElement('button');
        this.addColumnBtn.type = 'button';
        this.addColumnBtn.classList.add('btn', 'btn-primary');
        this.addColumnBtn.id = 'add-column-btn';
        this.addColumnBtn.innerText = 'Add Selected Column';
        return this.addColumnBtn;
    }

    /**
     * Creates the heading for the selected columns section.
     * @returns {HTMLHeadingElement} The columns heading element.
     */
    private createColumnsHeading(): HTMLHeadingElement {
        const columnsHeading = document.createElement('h3');
        columnsHeading.classList.add('mt-4');
        columnsHeading.innerText = 'Selected Columns';
        return columnsHeading;
    }

    /**
     * Creates the list to display selected columns.
     * @returns {HTMLUListElement} The column list element.
     */
    private createColumnList(): HTMLUListElement {
        this.columnList = document.createElement('ul');
        this.columnList.classList.add('list-group');
        this.columnList.id = 'column-list';
        return this.columnList;
    }

    /**
     * Adds event listeners for dynamic form behavior.
     */
    private addEventListeners(): void {
        this.addColumnBtn.addEventListener('click', () => {
            const columnName = this.colNameInput.getValue();
            let dataType: DataTypeBase;
            let additionalInfo = '';
            const isNullable = this.nullabilityCheckbox.isChecked();

            if (this.dataTypeSelect.getValue() === 'nvarchar(length)') {
                const length = parseInt(this.nvarcharLengthInput.getValue(), 10);
                dataType = new NVarChar(length, isNullable);
                additionalInfo = `(${length})`;
            } else if (this.dataTypeSelect.getValue() === 'nvarchar(max)') {
                dataType = new NVarCharMax(isNullable);
            } else if (this.dataTypeSelect.getValue() in SimpleDataTypes) {
                dataType = new SimpleType(SimpleDataTypes[this.dataTypeSelect.getValue() as keyof typeof SimpleDataTypes], isNullable);
            } else {
                dataType = new UnknownDataType(this.dataTypeSelect.getValue(), isNullable);
            }

            if (columnName && dataType) {
                const column = new Column(columnName, dataType);
                const columnItem = new ColumnItem(column, () => {
                    this.columnList.removeChild(columnItem.listItem);
                    this.columnMap.delete(columnItem.getId());
                });

                const itemId = `column-${Date.now()}`;
                columnItem.setId(itemId);
                this.columnList.appendChild(columnItem.listItem);

                this.columnMap.set(itemId, column);

                // Clear inputs
                this.colNameInput.clear();
                this.nvarcharLengthInput.clear();
                this.otherTypeInput.clear();
                this.dataTypeSelect.clear();
                this.nullabilityCheckbox.clear();
                this.handleSelectionChange();
            } else {
                alert('Please enter a column name and select a data type.');
            }
        });
    }

    /**
     * Handles the change event for data type selection, showing or hiding additional inputs.
     */
    private handleSelectionChange(): void {
        const dataType = this.dataTypeSelect.getValue();

        if (dataType === 'nvarchar(length)') {
            this.lengthInputDiv.style.display = 'block';
            this.otherInputDiv.style.display = 'none';
        } else if (dataType === 'other') {
            this.lengthInputDiv.style.display = 'none';
            this.otherInputDiv.style.display = 'block';
        } else {
            this.lengthInputDiv.style.display = 'none';
            this.otherInputDiv.style.display = 'none';
        }
    }

    /**
     * Extracts the selected columns from the map.
     * @returns {Column[]} The list of selected columns as Column instances.
     */
    public extractSelectedColumns(): Column[] {
        return Array.from(this.columnMap.values());
    }
}
