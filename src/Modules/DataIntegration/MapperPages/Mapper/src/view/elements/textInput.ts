/**
 * Represents a text input element.
 */
export class TextInput {
    public input: HTMLInputElement;
    private allowWhiteSpaces: boolean;

    /**
     * Creates a new instance of the TextInput class.
     * @param id - The ID of the input element.
     * @param placeholder - The placeholder text for the input element.
     * @param type - The type of the input element (default: 'text').
     * @param allowWhiteSpaces - Whether white spaces are allowed (default: true).
     */
    constructor(id: string, placeholder: string, type: string = 'text', allowWhiteSpaces: boolean = true) {
        this.input = document.createElement('input');
        this.input.type = type;
        this.input.classList.add('form-control');
        this.input.id = id;
        this.input.placeholder = placeholder;
        this.allowWhiteSpaces = allowWhiteSpaces;

        if (!this.allowWhiteSpaces) {
            this.input.addEventListener('input', this.removeWhiteSpaces.bind(this));
        }
    }

    /**
     * Removes white spaces from the input value.
     */
    private removeWhiteSpaces(): void {
        this.input.value = this.input.value.replace(/\s/g, '');
    }

    /**
     * Gets the value of the input element.
     * @returns The value of the input element.
     */
    getValue(): string {
        return this.input.value.trim();
    }

    /**
     * Sets the value of the input element.
     * @param value - The value to set.
     */
    setValue(value: string): void {
        this.input.value = value;
        if (!this.allowWhiteSpaces) {
            this.removeWhiteSpaces();
        }
    }

    /**
     * Clears the value of the input element.
     */
    clear(): void {
        this.input.value = '';
    }
}