export abstract class BaseModal {
    private static counter = 0;
    private readonly title: HTMLHeadingElement;
    private readonly cancelAction = () => this.handleCancelClick();
    private saveCallback: () => void | null = null;

    protected static readonly overlay = document.getElementById('overlay');
    protected readonly modal : HTMLDivElement;
    protected abstract name: string;
    protected readonly body: HTMLDivElement

    public constructor() {
        console.log('base modal constructor');
        this.title = document.createElement('h1');
        this.title.classList.add('modal-title');

        const closeButton = document.createElement('button');
        closeButton.classList.add('close-button');
        closeButton.innerHTML = '&times;';
        closeButton.addEventListener('click', () => this.handleCancelClick());

        const modalHeader = document.createElement('div');
        modalHeader.classList.add('modal-header');
        modalHeader.appendChild(this.title);
        modalHeader.appendChild(closeButton);

        this.body = document.createElement('div');
        this.body.classList.add('modal-body');

        const footer = document.createElement('div');
        footer.classList.add('modal-footer');

        const cancelButton = document.createElement('button');
        cancelButton.innerText = 'Continue';
        cancelButton.addEventListener('click', () => this.handleSaveRequest());
        footer.appendChild(cancelButton);

        const modal = document.createElement('div');
        modal.id = 'Modal-' + BaseModal.counter++;
        modal.classList.add('modal');
        modal.appendChild(modalHeader);
        modal.appendChild(this.body);
        modal.appendChild(footer);
        this.modal = modal;

        document.body.appendChild(modal);
    }

    /**
     * Saves base modal
     * @returns true if saving was successful, false otherwise - invalid data, etc. 
     */
    protected abstract save(): boolean;

    /**
     * Handles the action of canceling the modal.
     * 
     * Cancels the changes
     */
    protected abstract cancel(): void;


    /**
     * Handles click on the cancel button.
     */
    private handleCancelClick(): void{
        this.cancel();
        this.close();
    }

    /**
     * Handles request of saving the changes and closing the modal.
     * 
     * Closes the modal if saving was successful
     */
    private handleSaveRequest() {
        if (!this.save()) {
            return;
        }

        this.close();
        const callback = this.saveCallback;
        if (callback !== null) {
            this.saveCallback = null; // TODO: is this necessary?
            callback();
        }
    }

    /**
     * Opens the modal
     * 
     * The callback might never be called if the modal is closed by the cancel button
     * 
     * @param saveCallback callback to be called when the modal is closed and the changes are saved
     */
    public open(saveCallback: () => void | null = null) {
        this.saveCallback = saveCallback;
        this.modal.classList.add('active');
        BaseModal.overlay.classList.add('active');
        BaseModal.overlay.addEventListener('click', this.cancelAction);
    }

    /**
     * Frees the resources used by the modal
     */
    public finalize() { 
        this.modal.parentNode.removeChild(this.modal);
    }

    private close() {
        this.modal.classList.remove('active');
        BaseModal.overlay.classList.remove('active');
        BaseModal.overlay.removeEventListener('click', this.cancelAction);
    }

    protected setTitle(title: string) {
        this.title.textContent = title;
    }
}