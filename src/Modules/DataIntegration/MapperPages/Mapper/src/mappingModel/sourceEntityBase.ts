import { MappingVisitor } from "./mappingVisitor";
import { SourceColumn } from "./sourceColumn";
import { SourceEntity } from "./sourceEntity";

/** Represents a source entity that directly provides, or generates data.
 * @note These entities are represented as elements, and not as links between elements.
 */
export abstract class SourceEntityBase extends SourceEntity {
    protected _selectedColumns: SourceColumn[];
    constructor(name: string) {
        super(name);
    }
}