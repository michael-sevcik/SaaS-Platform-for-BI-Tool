import { mapperElement } from "../../constants";

type CallbackFunction = () => void;
type OptionalCallbackFunction = CallbackFunction | null;
export abstract class BaseModal {
    private static counter = 0;
    protected static readonly overlay = document.getElementById('overlay')
        ?? (() => { throw new Error('Overlay not found') })();
        
    private readonly cancelAction = () => this.handleCancelClick();
    private saveCallback: OptionalCallbackFunction;
    private readonly title: HTMLHeadingElement;
    protected readonly modal : HTMLDivElement;
    protected readonly modalContent: HTMLDivElement
    protected abstract name: string;
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

        this.modalContent = document.createElement('div');
        this.modalContent.classList.add('modal-body');

        const footer = document.createElement('div');
        footer.classList.add('modal-footer');

        const cancelButton = document.createElement('button');
        cancelButton.innerText = 'Continue';
        cancelButton.addEventListener('click', () => this.handleSaveRequest());
        footer.appendChild(cancelButton);

        const modal = document.createElement('div');
        modal.id = 'Modal-' + BaseModal.counter++;
        modal.classList.add('mapper-modal');
        modal.appendChild(modalHeader);
        modal.appendChild(this.modalContent);
        modal.appendChild(footer);
        this.modal = modal;

        // document.body.appendChild(modal);
        mapperElement.appendChild(modal); 
    }

    private close() {
        this.modal.classList.remove('active');
        BaseModal.overlay.classList.remove('active');
        BaseModal.overlay.removeEventListener('click', this.cancelAction);
    }

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
     * Handles the action of canceling the modal.
     * 
     * Cancels the changes
     */
    protected abstract cancel(): void;
    /**
     * Saves base modal
     * @returns true if saving was successful, false otherwise - invalid data, etc. 
     */
    protected abstract save(): boolean;

    protected setTitle(title: string) {
        this.title.textContent = title;
    }

    /**
     * Frees the resources used by the modal
     */
    public finalize() { 
        this.modal.parentNode?.removeChild(this.modal);
    }

    /**
     * Opens the modal
     * 
     * The callback might never be called if the modal is closed by the cancel button
     * 
     * @param saveCallback callback to be called when the modal is closed and the changes are saved
     */
    public open(saveCallback: OptionalCallbackFunction = null) {
        this.saveCallback = saveCallback;
        this.modal.classList.add('active');
        BaseModal.overlay.classList.add('active');
        BaseModal.overlay.addEventListener('click', this.cancelAction);
    }
}