import { Column } from "../../../dbModel/database";
import { NVarChar, NVarCharMax, SimpleDataTypes, SimpleType, UnknownDataType, type DataTypeBase } from "../../../dbModel/dataTypes";
export class SelectedColumnAdditionForm {
    private container: HTMLElement;
    private columnMap: Map<string, Column>;

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
        const colNameInput = document.createElement('input');
        colNameInput.type = 'text';
        colNameInput.classList.add('form-control');
        colNameInput.id = 'selected-column';
        colNameInput.placeholder = 'Enter column name';
        colNameDiv.appendChild(colNameLabel);
        colNameDiv.appendChild(colNameInput);
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
        const dataTypeSelect = document.createElement('select');
        dataTypeSelect.classList.add('form-control');
        dataTypeSelect.id = 'data-type';
        dataTypeSelect.onchange = this.handleSelectionChange;

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
            dataTypeSelect.appendChild(option);
        });

        dataTypeDiv.appendChild(dataTypeLabel);
        dataTypeDiv.appendChild(dataTypeSelect);
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
        const lengthInputDiv = document.createElement('div');
        lengthInputDiv.classList.add('form-group', 'col-md-6');
        lengthInputDiv.id = 'length-input';
        lengthInputDiv.style.display = 'none';
        const lengthInputLabel = document.createElement('label');
        lengthInputLabel.htmlFor = 'nvarchar-length';
        lengthInputLabel.innerText = 'Specify Length for nvarchar';
        const lengthInput = document.createElement('input');
        lengthInput.type = 'number';
        lengthInput.classList.add('form-control');
        lengthInput.id = 'nvarchar-length';
        lengthInput.placeholder = 'Enter length';
        lengthInputDiv.appendChild(lengthInputLabel);
        lengthInputDiv.appendChild(lengthInput);
        return lengthInputDiv;
    }

    /**
     * Creates the other type input field for custom data types.
     * @returns {HTMLDivElement} The other type input field div.
     */
    private createOtherTypeDiv(): HTMLDivElement {
        const otherTypeDiv = document.createElement('div');
        otherTypeDiv.classList.add('form-group', 'col-md-6');
        otherTypeDiv.id = 'other-input';
        otherTypeDiv.style.display = 'none';
        const otherTypeLabel = document.createElement('label');
        otherTypeLabel.htmlFor = 'other-type';
        otherTypeLabel.innerText = 'Specify Other Type';
        const otherTypeInput = document.createElement('input');
        otherTypeInput.type = 'text';
        otherTypeInput.classList.add('form-control');
        otherTypeInput.id = 'other-type';
        otherTypeInput.placeholder = 'Enter type name';
        otherTypeDiv.appendChild(otherTypeLabel);
        otherTypeDiv.appendChild(otherTypeInput);
        return otherTypeDiv;
    }

    /**
     * Creates the nullability input field for specifying if a column can be null.
     * @returns {HTMLDivElement} The nullability input field div.
     */
    private createNullabilityDiv(): HTMLDivElement {
        const nullabilityDiv = document.createElement('div');
        nullabilityDiv.classList.add('form-group', 'form-check', 'col-md-6');
        const nullabilityCheckbox = document.createElement('input');
        nullabilityCheckbox.type = 'checkbox';
        nullabilityCheckbox.id = 'column-nullable';
        nullabilityCheckbox.classList.add('form-check-input');
        const nullabilityLabel = document.createElement('label');
        nullabilityLabel.classList.add('form-check-label');
        nullabilityLabel.htmlFor = 'column-nullable';
        nullabilityLabel.innerText = 'Nullable';
        nullabilityDiv.appendChild(nullabilityCheckbox);
        nullabilityDiv.appendChild(nullabilityLabel);
        return nullabilityDiv;
    }

    /**
     * Creates the button to add selected columns to the list.
     * @returns {HTMLButtonElement} The add column button element.
     */
    private createAddColumnButton(): HTMLButtonElement {
        const addColumnBtn = document.createElement('button');
        addColumnBtn.type = 'button';
        addColumnBtn.classList.add('btn', 'btn-primary');
        addColumnBtn.id = 'add-column-btn';
        addColumnBtn.innerText = 'Add Selected Column';
        return addColumnBtn;
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
        const columnList = document.createElement('ul');
        columnList.classList.add('list-group');
        columnList.id = 'column-list';
        return columnList;
    }

    /**
     * Adds event listeners for dynamic form behavior.
     */
    private addEventListeners(): void {
        const addColumnBtn = document.getElementById('add-column-btn') as HTMLButtonElement;
        const columnList = document.getElementById('column-list') as HTMLUListElement;
        const colNameInput = document.getElementById('selected-column') as HTMLInputElement;
        const dataTypeSelect = document.getElementById('data-type') as HTMLSelectElement;
        const nvarcharLengthInput = document.getElementById('nvarchar-length') as HTMLInputElement;
        const otherTypeInput = document.getElementById('other-type') as HTMLInputElement;
        const nullabilityCheckbox = document.getElementById('column-nullable') as HTMLInputElement;

        addColumnBtn.addEventListener('click', () => {
            const columnName = colNameInput.value.trim();
            let dataType: DataTypeBase;
            let additionalInfo = '';
            const isNullable = nullabilityCheckbox.checked;

            if (dataTypeSelect.value === 'nvarchar(length)') {
                const length = parseInt(nvarcharLengthInput.value, 10);
                dataType = new NVarChar(length, isNullable);
                additionalInfo = `(${length})`;
            } else if (dataTypeSelect.value === 'nvarchar(max)') {
                dataType = new NVarCharMax(isNullable);
            } else if (dataTypeSelect.value in SimpleDataTypes) {
                dataType = new SimpleType(SimpleDataTypes[dataTypeSelect.value], isNullable);
            } else {
                dataType = new UnknownDataType(dataTypeSelect.value, isNullable);
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
                    columnList.removeChild(listItem);
                    this.columnMap.delete(itemId);
                });

                listItem.appendChild(removeBtn);
                columnList.appendChild(listItem);

                // Add to the column map
                const column = new Column(columnName, dataType);
                this.columnMap.set(itemId, column);

                // Clear inputs
                colNameInput.value = '';
                nvarcharLengthInput.value = '';
                otherTypeInput.value = '';
                dataTypeSelect.value = '';
                nullabilityCheckbox.checked = false;
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
        const dataTypeSelect = document.getElementById('data-type') as HTMLSelectElement;
        const lengthInputDiv = document.getElementById('length-input') as HTMLDivElement;
        const otherInputDiv = document.getElementById('other-input') as HTMLDivElement;

        const dataType = dataTypeSelect.value;

        if (dataType === 'nvarchar(length)') {
            lengthInputDiv.style.display = 'block';
            otherInputDiv.style.display = 'none';
        } else if (dataType === 'other') {
            lengthInputDiv.style.display = 'none';
            otherInputDiv.style.display = 'block';
        } else {
            lengthInputDiv.style.display = 'none';
            otherInputDiv.style.display = 'none';
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
