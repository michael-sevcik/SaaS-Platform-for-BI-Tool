export class CheckboxInput {
    public checkbox: HTMLInputElement;

    constructor(id: string, label: string) {
        const checkboxDiv = document.createElement('div');
        checkboxDiv.classList.add('form-check');

        this.checkbox = document.createElement('input');
        this.checkbox.type = 'checkbox';
        this.checkbox.id = id;
        this.checkbox.classList.add('form-check-input');

        const checkboxLabel = document.createElement('label');
        checkboxLabel.classList.add('form-check-label');
        checkboxLabel.htmlFor = id;
        checkboxLabel.innerText = label;

        checkboxDiv.appendChild(this.checkbox);
        checkboxDiv.appendChild(checkboxLabel);
    }

    isChecked(): boolean {
        return this.checkbox.checked;
    }

    setChecked(value: boolean): void {
        this.checkbox.checked = value;
    }

    clear(): void {
        this.checkbox.checked = false;
    }
}
