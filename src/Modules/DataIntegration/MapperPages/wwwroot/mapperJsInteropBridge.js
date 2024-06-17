// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.
import { getMappingEditor as gme} from './dist/bundle.js'
// TODO: REFACTOR

export function getMappingEditor(serializedSourceDb) {
    return gme(serializedSourceDb);
}