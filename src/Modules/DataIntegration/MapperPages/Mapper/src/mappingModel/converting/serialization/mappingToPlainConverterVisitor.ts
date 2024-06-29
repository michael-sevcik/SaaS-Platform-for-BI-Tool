import { ConditionLink } from "../../sourceEntities/aggregators/conditions/conditionLink";
import { JoinCondition } from "../../sourceEntities/aggregators/conditions/joinCondition";
import { Join } from "../../sourceEntities/aggregators/join";
import { SourceColumn } from "../../sourceEntities/sourceColumn";
import { SourceTable } from "../../sourceEntities/sourceTable";
import { MappingVisitor } from "../../mappingVisitor";
import { instanceToPlain } from "class-transformer";
import type { SourceEntity } from "../../sourceEntities/sourceEntity";
import { CustomQuery } from "../../sourceEntities/customQuery";


// TODO: check NESTED JOIN VISITATION

/**
 * This class is used to convert a SourceEntity to a plain JS object
 * with use of the visitor pattern and id / ref / type properties
 */
export class MappingToPlainConverterVisiter extends MappingVisitor {
    public visitCustomQuery(customQuery: CustomQuery): void {
        this.useReferenceOrCreateNew(customQuery, (id : string) => {
            customQuery.selectedColumns.forEach(column => column.accept(this));
            const plainColumns : any[] = []
            for (let i = 0; i < customQuery.selectedColumns.length; i++) {
                plainColumns.push(this.safeIntermediateResultPop())
            }

            return { 
                $id: id,
                type: CustomQuery.typeDescriptor,
                name: customQuery.name,
                query: customQuery.query,
                selectedColumns: plainColumns,
            };
        });
    }
    private id = 0;
    private intermediateResult: any[] = [];
    private plainSourceEntitiesByOriginal: Map<SourceEntity, any> = new Map<SourceEntity, any>();
    public readonly plainSourceColumnsByOriginal: Map<SourceColumn, any> = new Map<SourceColumn, any>();
    /**
     * The source entities sorted in a way without forward references.
     */
    public readonly sortedSourceEntities: SourceEntity[] = [];

    private static getReferenceWithId(id: string): any {
        return {$ref: id};
    }

    /**
     * Checks if the sourceEntity was already visited and if so, uses the reference,
     * otherwise creates a new object using the {@link createFunction}
     * and adds it to the intermediate result.
     * @param sourceEntity the source entity to check
     * @param createFunction the function that should be used to create a new object
     * if needed.
     */
    private useReferenceOrCreateNew(
        sourceEntity : SourceEntity,
        createFunction : (id : string) => any)
    {
        let result;
        let id : string;
        const plainSourceEntity = this.plainSourceEntitiesByOriginal.get(sourceEntity);
        if (plainSourceEntity !== undefined) {
            id = plainSourceEntity.$id;
            result = MappingToPlainConverterVisiter.getReferenceWithId(id);
        }
        else {
            id = (this.id++).toString();
            result = createFunction(id);
            this.sortedSourceEntities.push(result);
            this.plainSourceEntitiesByOriginal.set(sourceEntity, result);
        }

        this.intermediateResult.push(MappingToPlainConverterVisiter.getReferenceWithId(id));
    }

    /**
     * Gets a plain reference to the source column.
     * @param sourceColumn The source column to get the reference for.
     * @returns A plain reference to the source column.
     */
    public getSourceColumnRef(sourceColumn: SourceColumn) : any {
        console.log(this.plainSourceColumnsByOriginal);
        console.log(this.plainSourceColumnsByOriginal.has(sourceColumn));
        
        const plainSourceColumn = this.plainSourceColumnsByOriginal.get(sourceColumn);
        return {$ref: plainSourceColumn.$id};
    }

    /** 
     * Gets the result of the conversion.
     * @returns The result of the conversion. The first element are the sorted source entities. The second element is the root source entity.
     */
    public getResult(): [any[], any] {
        return [this.sortedSourceEntities, this.safeIntermediateResultPop()];
    }

    public visitConditionLink(conditionLink: ConditionLink): void {
        conditionLink.condition.accept(this);
        this.intermediateResult.push({
            relation: conditionLink.relation,
            condition: this.intermediateResult.pop(),
        });
    }

    public visitJoin(join: Join): void {
        this.useReferenceOrCreateNew(join, (id : string) => {
            // Prepare othrer properties. The order is important!
            join.leftSourceEntity.accept(this);
            join.rightSourceEntity.accept(this);
            if (join.condition == null) {
                throw new Error("Join condition is not defined.");
            }
            join.condition.accept(this);

            const plainCondition = this.intermediateResult.pop();
            const plainRightSourceEntity = this.intermediateResult.pop();
            const plainLeftSourceEntity = this.intermediateResult.pop();
            
            console.log("converting columns");
            console.log(this.plainSourceColumnsByOriginal);
            
            // Prepare output columns - convert them to plain objects
            const plainSelectedColumns : any[] = [];
            for (let i = 0; i < join.selectedColumns.length; i++) {
                plainSelectedColumns.push(this.getSourceColumnRef(join.selectedColumns[i]));
            }
            console.log("plainColumns");
            
            console.log(plainSelectedColumns);
            console.log(join.selectedColumns);
            
            

            return { 
                $id: id,
                type: Join.typeDescriptor, // TODO: move this to a map to constatns or something
                name: join.name,
                joinType: join.type,
                leftSourceEntity: plainLeftSourceEntity,
                rightSourceEntity: plainRightSourceEntity,
                joinCondition: plainCondition,
                selectedColumns: plainSelectedColumns
            };
        });
    }

    public visitJoinCondition(joinCondition: JoinCondition): void {
        let linkedCondition = null;
        if (joinCondition.linkedCondition !== null
            && joinCondition.linkedCondition !== undefined)
        {
            joinCondition.linkedCondition.accept(this);
            linkedCondition = this.intermediateResult.pop();
        }

        joinCondition.rightColumn.accept(this);
        joinCondition.leftColumn.accept(this);

        this.intermediateResult.push({
            relation: joinCondition.relation,
            leftColumn: this.intermediateResult.pop(),
            rightColumn: this.intermediateResult.pop(),
            linkedCondition: linkedCondition
        });
    }

    public visitSourceColumn(sourceColumn: SourceColumn): void {
        let result = this.plainSourceColumnsByOriginal.get(sourceColumn);
        if (result === undefined) {
            console.log("undefined column - creating new plain");
            
            result = instanceToPlain(sourceColumn);
            result["$id"] = (this.id++).toString();

            this.plainSourceColumnsByOriginal.set(sourceColumn, result);
        }
        else {
            result = {$ref: result.$id}
        }

        // HACK: to prevent circular references
        this.intermediateResult.push(result);
    }

    public visitSourceTable(sourceTable: SourceTable): void {
        this.useReferenceOrCreateNew(sourceTable, (id : string) => {
            sourceTable.selectedColumns.forEach(column => column.accept(this));
            const plainColumns : any[] = []
            for (let i = 0; i < sourceTable.selectedColumns.length; i++) {
                plainColumns.push(this.safeIntermediateResultPop())
            }

            return { 
                $id: id,
                type: SourceTable.typeDescriptor, // TODO: move this to a map to constants or something
                name: sourceTable.name,
                schema: sourceTable.schema,
                selectedColumns: plainColumns,
            };
        });
    }

    private safeIntermediateResultPop(): any {
        const result = this.intermediateResult.pop();
        if (result === undefined) {
            throw new Error("Intermediate result is empty");
        }

        return result;
    }
}