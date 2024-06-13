import { Column } from "../dbModel/database";
import { ReferenceHolder } from "./referenceHolder";
import { Ownable } from "./ownable";
import { MappingVisitor } from "./mappingVisitor";
import { Visitable } from "./converting/visitable";
import { SourceEntity } from "./sourceEntity";

/**
 * Represents a column in the source entity of a mapping.
 */
export class SourceColumn extends Column implements Ownable, Visitable {
    private _owner: SourceEntity | null = null;

    /**
     * Gets the owner of the source column.
     * @returns The owner entity or throws error.
     */
    get owner(): SourceEntity {
        return this._owner ?? (() => { throw new Error('Owner not set.'); })();
    }

    /**
     * Holders of references to this column.
     */
    public referenceHolders = new Set<ReferenceHolder>();

    constructor(column: Column) { 
        super(column.name, column.dataType);
    }

    /**
     * Accepts a mapping visitor and calls the appropriate visit method.
     * @param visitor The mapping visitor.
     */
    accept(visitor: MappingVisitor): void {
        visitor.visitSourceColumn(this);
    }

    /**
     * Adds a reference holder to the source column.
     * @param referenceHolder The reference holder.
     */
    public addReference(referenceHolder: ReferenceHolder): void {
        this.referenceHolders.add(referenceHolder);
    }

    /**
     * Checks if the source column is used in the mapping.
     * @returns True if the source column is used in the mapping.
     */
    public isUsed(): boolean {
        return this.referenceHolders.size > 0;
    }

    /**
     * Removes a reference holder from the source column.
     * @param referenceHolder The reference holder.
     */
    public removeReference(referenceHolder: any): void {
        this.referenceHolders.delete(referenceHolder);
    }

    /**
     * Sets the owner of the source column.
     * @param owner The owner entity.
     */
    set owner(owner: SourceEntity) {
        this._owner = owner;
    }
}
