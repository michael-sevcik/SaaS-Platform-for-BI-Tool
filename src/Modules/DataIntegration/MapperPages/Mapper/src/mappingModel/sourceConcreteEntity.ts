import { MappingVisitor } from "./serialization/mappingVisitor";
import { SourceColumn } from "./sourceColumn";
import { SourceEntity } from "./sourceEntity";

/** Represents a source entity that is directly provides, or generates data.
 * @note These entities are represented as elements, and not as links between elements.
 */
export abstract class SourceConcreteEntity extends SourceEntity {
    protected _selectedColumns: SourceColumn[];
    constructor(name: string) {
        super(name);
    }
}