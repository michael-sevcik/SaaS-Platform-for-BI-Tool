// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
import { ahoj, getMappingEditor as gme} from './dist/bundle.js'
// TODO: REFACTOR
export function showPrompt(message) {
    console.log(ahoj());
    console.log(ahoj);

    return "ahojka14"; 
}

export function getMappingEditor(serializedSourceDb) {
    return gme(serializedSourceDb);
}