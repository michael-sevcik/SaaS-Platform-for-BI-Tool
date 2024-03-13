// TODO: REDO OR add source

export class SearchableDropdown {
    private readonly input: HTMLInputElement;
    private readonly dropdown: HTMLDivElement;
    private readonly dropdownContent: HTMLDivElement;
    private readonly options: HTMLDivElement[] = [];
    private readonly optionsMap = new Map<string, HTMLDivElement>();
    private optionValues: string[] | null = null; 
    private readonly onOptionSelected: (optionIndex: number) => void;
    
    /**
     * Indicates whether the dropdown is currently displayed as incorrect
     */
    private displayingIncorrect = false;

    public constructor(
        private readonly container: HTMLDivElement,
        private readonly optionsProvider: () => string[],
        private readonly placeholder: string,
        onOptionSelected: (optionIndex: number) => void
    ) {
        this.onOptionSelected = onOptionSelected;
        this.input = document.createElement('input', {});
        this.input.classList.add('searchable-dropdown-input');
        this.input.placeholder = placeholder;
        this.input.addEventListener('input', () => this.handleInput());
        this.input.addEventListener('focus', () => this.handleInput());
        // this.input.addEventListener('blur', () => this.handleBlur());    // TODO: is this needed?

        this.dropdown = document.createElement('div');
        this.dropdown.classList.add('searchable-dropdown');
        this.dropdownContent = document.createElement('div');
        this.dropdownContent.classList.add('dropdown-content');
        this.dropdown.appendChild(this.dropdownContent);

        this.container.appendChild(this.input);
        this.container.appendChild(this.dropdown);

    }

    /**
     * Displays as incorrect
     * 
     * Creates a red border around the input
     */
    public displayIncorrect() : void {
        if (this.displayingIncorrect) {
            return;
        }
        
        this.input.classList.add('incorrect-input');
        this.displayingIncorrect = true;
    }

    /**
     * Displays as correct
     * 
     * Removes the red border around the input
     */
    public displayCorrect() : void {
        if (!this.displayingIncorrect) {
            return;
        }
        
        this.input.classList.remove('incorrect-input');
    }

    /**
     * Sets place holder
     * @param placeholder the placeholder 
     */
    public setPlaceHolder(placeholder: string) {
        this.input.placeholder = placeholder;
    }

    private handleInput() {
        const options = this.optionsProvider();
        this.optionValues = options;
        this.dropdownContent.innerHTML = '';
        this.options.length = 0;
        this.optionsMap.clear(); //TODO: IS needed?
        const inputText = this.input.value.toUpperCase();
        // TODO: FILTER OPTIONS
        for (let i = 0; i < options.length; i++) {
            const option = options[i];

            // check if the option contains the input text
            if (option.toUpperCase().indexOf(inputText) === -1) {
                continue;
            }

            const optionDiv = document.createElement('div');
            optionDiv.classList.add('searchable-dropdown-option');
            optionDiv.innerText = option;
            optionDiv.addEventListener('click', () => this.handleOptionClick(i));
            this.options.push(optionDiv);
            this.optionsMap.set(option, optionDiv);
            this.dropdownContent.appendChild(optionDiv);
        }

        this.dropdown.style.display = 'block';
    }

    /**
     * Handles blur
     */
    private hideOptions() {
        this.dropdown.style.display = 'none';
    }

    private handleOptionClick(optionIndex: number) {
        this.displayCorrect();

        const optionValue = this.optionValues[optionIndex];
        this.input.value = optionValue;
        this.onOptionSelected(optionIndex);
        this.hideOptions();
    }
}