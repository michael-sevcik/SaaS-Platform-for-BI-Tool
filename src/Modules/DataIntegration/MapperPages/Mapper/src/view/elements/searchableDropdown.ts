export class SearchableDropdown {
    private displayingIncorrect = false;
    private readonly dropdown: HTMLDivElement;
    private readonly dropdownContent: HTMLDivElement;
    private readonly input: HTMLInputElement;
    private readonly options: HTMLDivElement[] = [];
    private readonly optionsMap = new Map<string, HTMLDivElement>();
    private optionValues: string[] | null = null;

    public constructor(
        private readonly container: HTMLDivElement,
        private readonly optionsProvider: () => string[],
        private readonly placeholder: string,
        private readonly onOptionSelected: (optionIndex: number) => void
    ) {
        this.input = this.createInput();
        this.dropdown = this.createDropdown();
        this.dropdownContent = this.createDropdownContent();

        this.dropdown.appendChild(this.dropdownContent);
        this.container.appendChild(this.input);
        this.container.appendChild(this.dropdown);
    }

    /**
     * Creates and returns the input element.
     */
    private createInput(): HTMLInputElement {
        const input = document.createElement('input');
        input.classList.add('form-control', 'searchable-dropdown-input');
        input.placeholder = this.placeholder;
        input.setAttribute('aria-label', 'Search');
        input.addEventListener('input', () => this.handleInput());
        input.addEventListener('focus', () => this.handleInput());
        input.addEventListener('click', () => this.selectAllText());
        return input;
    }

    /**
     * Creates and returns the dropdown element.
     */
    private createDropdown(): HTMLDivElement {
        const dropdown = document.createElement('div');
        dropdown.classList.add('dropdown', 'w-100');
        return dropdown;
    }

    /**
     * Creates and returns the dropdown content element.
     */
    private createDropdownContent(): HTMLDivElement {
        const dropdownContent = document.createElement('div');
        dropdownContent.classList.add('dropdown-menu', 'w-100', 'p-2');
        dropdownContent.style.maxHeight = '200px'; // Set max height for the dropdown
        dropdownContent.style.overflowY = 'auto'; // Make the dropdown scrollable
        dropdownContent.style.overflowX = 'hidden'; // Hide horizontal scrollbar
        return dropdownContent;
    }

    /**
     * Handles input events to filter and display dropdown options.
     */
    private handleInput() {
        const options = this.optionsProvider();
        this.optionValues = options;
        this.dropdownContent.innerHTML = '';
        this.options.length = 0;
        this.optionsMap.clear();

        const inputText = this.input.value.toUpperCase();
        this.filterAndDisplayOptions(options, inputText);

        this.showDropdown();
    }

    /**
     * Filters and displays options based on input text.
     */
    private filterAndDisplayOptions(options: string[], inputText: string) {
        options.forEach((option, index) => {
            if (option.toUpperCase().includes(inputText)) {
                const optionDiv = this.createOptionDiv(option, index);
                this.options.push(optionDiv);
                this.optionsMap.set(option, optionDiv);
                this.dropdownContent.appendChild(optionDiv);
            }
        });
    }

    /**
     * Creates and returns an option div element.
     */
    private createOptionDiv(option: string, index: number): HTMLDivElement {
        const optionDiv = document.createElement('div');
        optionDiv.classList.add('dropdown-item', 'searchable-dropdown-option');
        optionDiv.innerText = option;
        optionDiv.style.whiteSpace = 'normal'; // Make the option text wrap to the next line
        optionDiv.style.wordWrap = 'break-word'; // Ensure long words are broken to fit within the width
        optionDiv.addEventListener('click', () => this.handleOptionClick(index));
        return optionDiv;
    }

    /**
     * Handles option click events.
     */
    private handleOptionClick(optionIndex: number) {
        if (!this.optionValues) {
            throw new Error('Options are not initialized. handleInput must run before this method');
        }

        const optionValue = this.optionValues[optionIndex];
        this.input.value = optionValue;
        this.onOptionSelected(optionIndex);
        this.hideDropdown();
        this.displayCorrect();
    }

    /**
     * Shows the dropdown.
     */
    private showDropdown() {
        this.dropdownContent.classList.add('show');
    }

    /**
     * Hides the dropdown.
     */
    private hideDropdown() {
        this.dropdownContent.classList.remove('show');
    }

    /**
     * Selects all text in the input field.
     */
    private selectAllText(): void {
        this.input.select();
    }

    /**
     * Displays the input as correct.
     */
    public displayCorrect(): void {
        if (this.displayingIncorrect) {
            this.input.classList.remove('is-invalid');
            this.displayingIncorrect = false;
        }
    }

    /**
     * Displays the input as incorrect.
     */
    public displayIncorrect(): void {
        if (!this.displayingIncorrect) {
            this.input.classList.add('is-invalid');
            this.displayingIncorrect = true;
        }
    }

    /**
     * Clears the input.
     */
    public clear(): void {
        this.input.value = '';
    }

    /**
     * Sets the input placeholder.
     */
    public setPlaceholder(placeholder: string): void {
        this.input.placeholder = placeholder;
    }
}
