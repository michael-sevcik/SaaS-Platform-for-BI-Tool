import { mapperElement } from "../../constants";

type CallbackFunction = () => void;
type OptionalCallbackFunction = CallbackFunction | null;
/**
 * Represents a base modal class.
 * Provides common functionality for creating and managing modals.
 */
export abstract class BaseModal {
    private static counter = 0;
    protected static readonly overlay = document.getElementById('overlay')
    ?? (() => { throw new Error('Overlay not found') })();
    
    /**
     * Indicates whether the modal should be finalized when closed without saving.
     */
    private finalizeOnCancel: boolean = false;
    private finalized: boolean = false;
    private readonly cancelAction = () => this.handleCancelClick();
    private saveCallback: OptionalCallbackFunction;
    private readonly title: HTMLHeadingElement;
    protected readonly modal : HTMLDivElement;
    protected readonly modalContent: HTMLDivElement
    protected abstract name: string;

    /**
     * Constructs a new instance of the BaseModal class.
     * @param isSavable - A boolean indicating whether the modal is savable. Default is true.
     * Whether the continue button is enabled or disabled.
     */
    public constructor(isSavable: boolean = true) {
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

        const saveButton = document.createElement('button');
        saveButton.innerText = 'Continue';
        saveButton.disabled = !isSavable;
        saveButton.addEventListener('click', () => this.handleSaveRequest());
        footer.appendChild(saveButton);

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

    private close(): void {
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
        if (this.finalizeOnCancel) {
            this.finalize();
        }
    }

    /**
     * Handles request of saving the changes and closing the modal.
     * 
     * Closes the modal if saving was successful
     */
    private handleSaveRequest(): void {
        if (!this.save()) {
            return;
        }

        this.close();
        const callback = this.saveCallback;
        if (callback !== null) {
            this.saveCallback = null;
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
     * @throws Error if the modal has been already finalized.
     */
    public finalize(): void { 
        this.checkFinalized();
        this.modal.parentNode?.removeChild(this.modal);
        this.finalized = true;
    }

    /**
     * Called when the modal is opened
     */
    protected onOpen(): void {
        // nothing to do here
    }

    private checkFinalized(): void {
        if (this.finalized) {
            throw new Error('Modal has been already finalized');
        }
    }

    /**
     * Opens the modal.
     * 
     * The callback might never be called, the modal might be closed using the cancel button.
     * 
     * @param saveCallback Callback to be called when the modal is closed and the changes are saved.
     * @param finalizeOnCancel  Finalizes the modal on cancelling.
     */
    public open(saveCallback: OptionalCallbackFunction = null, finalizeOnCancel = false) {
        this.checkFinalized();
        this.saveCallback = saveCallback;
        this.finalizeOnCancel = finalizeOnCancel;
        this.onOpen();
        this.modal.classList.add('active');
        BaseModal.overlay.classList.add('active');
        BaseModal.overlay.addEventListener('click', this.cancelAction);
    }
}