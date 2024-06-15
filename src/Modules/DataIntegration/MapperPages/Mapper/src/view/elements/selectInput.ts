/**
 * Represents a select input element.
 */
export class SelectInput {
    public select: HTMLSelectElement;

    constructor(id: string, options: string[]) {
        this.select = document.createElement('select');
        this.select.classList.add('form-control');
        this.select.id = id;

        options.forEach(optionText => {
            const option = document.createElement('option');
            option.value = optionText;
            option.text = optionText;
            this.select.appendChild(option);
        });
    }

    /**
     * Gets the selected value of the select input.
     * @returns The selected value.
     */
    getValue(): string {
        return this.select.value;
    }

    /**
     * Sets the value of the select input.
     * @param value - The value to set.
     */
    setValue(value: string): void {
        this.select.value = value;
    }

    /**
     * Clears the select input by setting its value to an empty string.
     */
    clear(): void {
        this.select.value = '';
    }
}
