/**
 * Represents a checkbox input element.
 */
export class CheckboxInput {
    private checkbox: HTMLInputElement;
    public container: HTMLDivElement;

    /**
     * Creates a new instance of CheckboxInput.
     * @param id - The ID of the checkbox input.
     * @param label - The label text for the checkbox input.
     */
    constructor(id: string, label: string) {
        this.container = document.createElement('div');
        this.container.classList.add('form-group', 'form-check');

        this.checkbox = document.createElement('input');
        this.checkbox.type = 'checkbox';
        this.checkbox.id = id;
        this.checkbox.classList.add('form-check-input');

        const checkboxLabel = document.createElement('label');
        checkboxLabel.classList.add('form-check-label');
        checkboxLabel.htmlFor = id;
        checkboxLabel.innerText = label;

        this.container.appendChild(this.checkbox);
        this.container.appendChild(checkboxLabel);
    }

    /**
     * Checks if the checkbox is currently checked.
     * @returns A boolean indicating whether the checkbox is checked.
     */
    isChecked(): boolean {
        return this.checkbox.checked;
    }

    /**
     * Sets the checked state of the checkbox.
     * @param value - A boolean indicating whether the checkbox should be checked.
     */
    setChecked(value: boolean): void {
        this.checkbox.checked = value;
    }

    /**
     * Clears the checkbox by unchecking it.
     */
    clear(): void {
        this.checkbox.checked = false;
    }
}
