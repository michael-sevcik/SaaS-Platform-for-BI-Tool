// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
import { ahoj } from './dist/bundle.js'

export function showPrompt(message) {
    console.log(ahoj());
    console.log(ahoj);

    return "ahojka8"; 
}
