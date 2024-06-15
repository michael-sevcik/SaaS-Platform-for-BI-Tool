export class TextInput {
    public input: HTMLInputElement;

    constructor(id: string, placeholder: string, type: string = 'text') {
        this.input = document.createElement('input');
        this.input.type = type;
        this.input.classList.add('form-control');
        this.input.id = id;
        this.input.placeholder = placeholder;
    }

    getValue(): string {
        return this.input.value.trim();
    }

    setValue(value: string): void {
        this.input.value = value;
    }

    clear(): void {
        this.input.value = '';
    }
}
