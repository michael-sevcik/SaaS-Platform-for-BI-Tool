export function getElementOrThrow(id: string): HTMLElement {
    const element = document.getElementById(id);
    if (element === null) {
        throw new Error(`Cannot find element with id: ${id}`);
    }
    return element;
}