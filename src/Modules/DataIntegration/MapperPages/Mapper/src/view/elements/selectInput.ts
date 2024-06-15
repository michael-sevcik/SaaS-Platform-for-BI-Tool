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

    getValue(): string {
        return this.select.value;
    }

    setValue(value: string): void {
        this.select.value = value;
    }

    clear(): void {
        this.select.value = '';
    }
}
