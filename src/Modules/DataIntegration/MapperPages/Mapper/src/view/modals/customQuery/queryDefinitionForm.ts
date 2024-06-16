/**
 * Represents a form for defining a query.
 */
export class QueryDefinitionForm {
    private container: HTMLElement;
    private queryTextArea: HTMLTextAreaElement;

    /**
     * Initializes an instance of QueryDefinitionForm.
     * @constructor
     * @param container - The container element for the form.
     * @param {string} [defaultText=''] - The default text for the query text area.
     * @param {boolean} [editable=true] - Indicates whether the query text area is editable.
     */
    constructor(container: HTMLElement, defaultText: string = '', editable: boolean = true) {
        this.container = container;
        this.initializeForm();

        this.queryTextArea.value = defaultText;
        this.queryTextArea.readOnly = !editable;
    }

    /**
     * Initializes the form elements and appends them to the container.
     */
    private initializeForm(): void {
        const form = document.createElement('form');
        form.id = 'query-definition-form';

        form.appendChild(this.createQueryTextArea());

        this.container.appendChild(form);
    }



    /**
     * Creates the SQL query text area.
     * @returns The SQL query text area element.
     */
    private createQueryTextArea(): HTMLTextAreaElement {
        this.queryTextArea = document.createElement('textarea');
        this.queryTextArea.classList.add('form-control');
        this.queryTextArea.id = 'sql-query';
        this.queryTextArea.rows = 5;
        this.queryTextArea.placeholder = 'Enter your SQL query here...';
        return this.queryTextArea;
    }

    /**
     * Gets the SQL query from the text area.
     * @returns The SQL query.
     */
    public getSqlQuery(): string {
        return this.queryTextArea.value.trim();
    }
}