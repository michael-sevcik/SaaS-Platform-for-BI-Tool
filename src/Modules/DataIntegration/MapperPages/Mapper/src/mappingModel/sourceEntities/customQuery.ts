import { MappingVisitor } from "../mappingVisitor";
import { SourceEntityBase } from "./sourceEntityBase";
import { SourceColumn } from "./sourceColumn";


/**
 * Represents a custom query source entity.
 */
export class CustomQuery extends SourceEntityBase {
    public static readonly typeDescriptor = 'customQuery';
    
    /** @inheritdoc */
    public get fullName(): string {
        return this.name;
    }

    /** @inheritdoc */
    public removeReferences(): void {
        // no references to remove
    }

    /** @inheritdoc */
    replaceChild(oldChild: any, newChild: any): void {
    }
    
    /** @inheritdoc */
    public createBackwardConnections(): void {
        for (const column of this._selectedColumns) {
            column.owner = this;
        }
    }

    /**
     * Creates a new instance of the CustomQuery class.
     * @param name The name of the custom query.
     * @param query The query string.
     * @param selectedColumns The selected columns.
     * @param description The description of the custom query.
     */
    public constructor(
        name: string,
        public readonly query: string,
        selectedColumns: SourceColumn[] = [],
        public readonly description: string | null = null
    ) {
        super(name);
        this._selectedColumns = selectedColumns;
        this.createBackwardConnections();
    }

    /** @inheritdoc */
    public accept(visitor: MappingVisitor): void {
        visitor.visitCustomQuery(this);  
    }
}
