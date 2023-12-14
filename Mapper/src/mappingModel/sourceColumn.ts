import { Column } from "../dbModel/database";
import { ReferenceHolder } from "./referenceHolder";
import { Ownable } from "./ownable";
import { MappingVisitor } from "./serialization/mappingVisitor";
import { Visitable } from "./serialization/visitable";
import { SourceEntity } from "./sourceEntity";

/**
 * Represents a column in the source entity of a mapping.
 */
export class SourceColumn extends Column implements Ownable, Visitable {
    private _owner: SourceEntity | null = null;
    
    /**
     * Holders of references to this column.
     */
    public referenceHolders = new Set<ReferenceHolder>();

    /**
     * Sets the owner of the source column.
     * @param owner The owner entity.
     */
    set owner(owner: SourceEntity) {
        this._owner = owner;
    }

    /**
     * Gets the owner of the source column.
     * @returns The owner entity or null if not set.
     */
    get owner(): SourceEntity | null {
        return this._owner;
    }

    constructor(column: Column) { 
        super(column.name, column.type);
    }

    /**
     * Accepts a mapping visitor and calls the appropriate visit method.
     * @param visitor The mapping visitor.
     */
    accept(visitor: MappingVisitor): void {
        visitor.visitSourceColumn(this);
    }

    /**
     * Checks if the source column is used in the mapping.
     * @returns True if the source column is used in the mapping.
     */
    public isUsed(): boolean {
        return this.referenceHolders.size > 0;
    }

    /**
     * Adds a reference holder to the source column.
     * @param referenceHolder The reference holder.
     */
    public addReference(referenceHolder: ReferenceHolder): void {
        this.referenceHolders.add(referenceHolder);
    }

    /**
     * Removes a reference holder from the source column.
     * @param referenceHolder The reference holder.
     */
    public removeReference(referenceHolder: any): void {
        this.referenceHolders.delete(referenceHolder);
    }
}
