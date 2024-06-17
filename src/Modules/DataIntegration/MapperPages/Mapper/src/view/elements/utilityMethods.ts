import { getElementOrThrow } from "../../utils";

export function createButton(text: string, onClick: () => void): HTMLButtonElement {
    const button = document.createElement('button');
    button.innerText = text;
    button.addEventListener('click', onClick);
    return button;
}

export function attachChangeEvent(elementId: string, onChange: (event: Event) => void) {
    const element = getElementOrThrow(elementId);
    element.addEventListener('change', onChange);
}
