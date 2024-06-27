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
                type: CustomQuery.typeDescriptor, // TODO: move this to a map to constatns or something
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

    /**
     * Checks if the sourceEntity was already visited and if so, uses the reference,
     * otherwise creates a new object using the createFunctionand
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
        const plainSourceEntity = this.plainSourceEntitiesByOriginal.get(sourceEntity);
        if (plainSourceEntity !== undefined) {
            result = {
                $ref: plainSourceEntity.$id,
            }
        }
        else {
            result = createFunction((this.id++).toString());
            this.sortedSourceEntities.push(result);
            this.plainSourceEntitiesByOriginal.set(sourceEntity, result);
        }

        this.intermediateResult.push(result);
    }

    public getSourceColumnRef(sourceColumn: SourceColumn) : any {
        console.log(this.plainSourceColumnsByOriginal);
        console.log(this.plainSourceColumnsByOriginal.has(sourceColumn));
        
        const plainSourceColumn = this.plainSourceColumnsByOriginal.get(sourceColumn);
        return {$ref: plainSourceColumn.$id};
    }

    public popResult(): any {
        const result = this.intermediateResult.pop();
        if (this.intermediateResult.length !== 0) {
            throw new Error("Intermediate result is not empty");
        }

        return result;
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