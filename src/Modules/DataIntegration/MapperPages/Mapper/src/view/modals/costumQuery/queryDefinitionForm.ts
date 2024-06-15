export class QueryDefinitionForm {
    private container: HTMLElement;
    private queryTextArea: HTMLTextAreaElement;

    constructor(container: HTMLElement) {
        this.container = container;
        this.initializeForm();
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
     * @returns {HTMLTextAreaElement} The SQL query text area element.
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
     * @returns {string} The SQL query.
     */
    public getSqlQuery(): string {
        return this.queryTextArea.value.trim();
    }
}