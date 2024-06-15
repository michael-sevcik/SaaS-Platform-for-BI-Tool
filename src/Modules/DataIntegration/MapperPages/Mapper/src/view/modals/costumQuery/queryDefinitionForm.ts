export class QueryDefinitionForm {
    private container: HTMLElement;

    constructor(container: HTMLElement) {
        this.container = container;
        this.initializeForm();
        // TODO: Access the query
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
        const queryTextArea = document.createElement('textarea');
        queryTextArea.classList.add('form-control');
        queryTextArea.id = 'sql-query';
        queryTextArea.rows = 5;
        queryTextArea.placeholder = 'Enter your SQL query here...';
        return queryTextArea;
    }

    /**
     * Gets the SQL query from the text area.
     * @returns The SQL query.
     */
    public getSqlQuery(): string {
        const queryTextArea = document.getElementById('sql-query') as HTMLTextAreaElement;
        return queryTextArea.value.trim();
    }
}