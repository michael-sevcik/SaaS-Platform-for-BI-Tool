import { Column } from "../../../dbModel/database";
import { NVarChar, NVarCharMax, SimpleDataTypes, SimpleType, UnknownDataType, type DataTypeBase } from "../../../dbModel/dataTypes";
export class SelectedColumnAdditionForm {
    private container: HTMLElement;
    private columnMap: Map<string, Column>;
    private colNameInput: HTMLInputElement;
    private dataTypeSelect: HTMLSelectElement;
    private nvarcharLengthInput: HTMLInputElement;
    private otherTypeInput: HTMLInputElement;
    private nullabilityCheckbox: HTMLInputElement;
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
        this.colNameInput = document.createElement('input');
        this.colNameInput.type = 'text';
        this.colNameInput.classList.add('form-control');
        this.colNameInput.id = 'selected-column';
        this.colNameInput.placeholder = 'Enter column name';
        colNameDiv.appendChild(colNameLabel);
        colNameDiv.appendChild(this.colNameInput);
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
        this.dataTypeSelect = document.createElement('select');
        this.dataTypeSelect.classList.add('form-control');
        this.dataTypeSelect.id = 'data-type';
        this.dataTypeSelect.onchange = this.handleSelectionChange.bind(this);

        const dataTypes = [
            'TinyInteger', 'SmallInteger', 'Integer', 'BigInteger', 'Money',
            'Float', 'Decimal', 'Numeric', 'Boolean', 'Date', 'Datetime',
            'Datetime2', 'DatetimeOffset', 'Timestamp', 'Time', 'nvarchar(max)',
            'nvarchar(length)', 'other'
        ];
        dataTypes.forEach(type => {
            const option = document.createElement('option');
            option.value = type;
            option.text = type;
            this.dataTypeSelect.appendChild(option);
        });

        dataTypeDiv.appendChild(dataTypeLabel);
        dataTypeDiv.appendChild(this.dataTypeSelect);
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
        this.nvarcharLengthInput = document.createElement('input');
        this.nvarcharLengthInput.type = 'number';
        this.nvarcharLengthInput.classList.add('form-control');
        this.nvarcharLengthInput.id = 'nvarchar-length';
        this.nvarcharLengthInput.placeholder = 'Enter length';
        this.lengthInputDiv.appendChild(lengthInputLabel);
        this.lengthInputDiv.appendChild(this.nvarcharLengthInput);
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
        this.otherTypeInput = document.createElement('input');
        this.otherTypeInput.type = 'text';
        this.otherTypeInput.classList.add('form-control');
        this.otherTypeInput.id = 'other-type';
        this.otherTypeInput.placeholder = 'Enter type name';
        this.otherInputDiv.appendChild(otherTypeLabel);
        this.otherInputDiv.appendChild(this.otherTypeInput);
        return this.otherInputDiv;
    }

    /**
     * Creates the nullability input field for specifying if a column can be null.
     * @returns {HTMLDivElement} The nullability input field div.
     */
    private createNullabilityDiv(): HTMLDivElement {
        const nullabilityDiv = document.createElement('div');
        nullabilityDiv.classList.add('form-group', 'form-check', 'col-md-6');
        this.nullabilityCheckbox = document.createElement('input');
        this.nullabilityCheckbox.type = 'checkbox';
        this.nullabilityCheckbox.id = 'column-nullable';
        this.nullabilityCheckbox.classList.add('form-check-input');
        const nullabilityLabel = document.createElement('label');
        nullabilityLabel.classList.add('form-check-label');
        nullabilityLabel.htmlFor = 'column-nullable';
        nullabilityLabel.innerText = 'Nullable';
        nullabilityDiv.appendChild(this.nullabilityCheckbox);
        nullabilityDiv.appendChild(nullabilityLabel);
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
            const columnName = this.colNameInput.value.trim();
            let dataType: DataTypeBase;
            let additionalInfo = '';
            const isNullable = this.nullabilityCheckbox.checked;

            if (this.dataTypeSelect.value === 'nvarchar(length)') {
                const length = parseInt(this.nvarcharLengthInput.value, 10);
                dataType = new NVarChar(length, isNullable);
                additionalInfo = `(${length})`;
            } else if (this.dataTypeSelect.value === 'nvarchar(max)') {
                dataType = new NVarCharMax(isNullable);
            } else if (this.dataTypeSelect.value in SimpleDataTypes) {
                dataType = new SimpleType(SimpleDataTypes[this.dataTypeSelect.value], isNullable);
            } else {
                dataType = new UnknownDataType(this.dataTypeSelect.value, isNullable);
            }

            if (columnName && dataType) {
                const listItem = document.createElement('li');
                const itemId = `column-${Date.now()}`;
                listItem.id = itemId;
                listItem.className = 'list-group-item d-flex justify-content-between align-items-center';
                listItem.textContent = `${columnName} (${dataType.Descriptor}${additionalInfo}) ${isNullable ? 'NULL' : 'NOT NULL'}`;

                const removeBtn = document.createElement('button');
                removeBtn.className = 'btn btn-danger btn-sm';
                removeBtn.textContent = 'Remove';
                removeBtn.addEventListener('click', () => {
                    this.columnList.removeChild(listItem);
                    this.columnMap.delete(itemId);
                });

                listItem.appendChild(removeBtn);
                this.columnList.appendChild(listItem);

                // Add to the column map
                const column = new Column(columnName, dataType);
                this.columnMap.set(itemId, column);

                // Clear inputs
                this.colNameInput.value = '';
                this.nvarcharLengthInput.value = '';
                this.otherTypeInput.value = '';
                this.dataTypeSelect.value = '';
                this.nullabilityCheckbox.checked = false;
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
        const dataType = this.dataTypeSelect.value;

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
