import { Column } from "../../../dbModel/database";
import { ConditionLink, LinkRelation } from "../../sourceEntities/aggregators/conditions/conditionLink";
import { JoinCondition } from "../../sourceEntities/aggregators/conditions/joinCondition";
import { Join, JoinType } from "../../sourceEntities/aggregators/join";
import { SourceColumn } from "../../sourceEntities/sourceColumn";
import { CustomQuery } from "../../sourceEntities/customQuery";
import type { SourceEntity } from "../../sourceEntities/sourceEntity";
import { SourceEntityBase } from "../../sourceEntities/sourceEntityBase";
import { SourceTable } from "../../sourceEntities/sourceTable";
import { plainToInstance } from "class-transformer";

export class PlainToSourceEntityConvertor {
    private columnReferences = new Map<string, SourceColumn>();
    private sourceEntityReferences = new Map<string, SourceEntity>();

    public convertSourceColumns(plainSourceColumns: any) : SourceColumn[] { 
        if (plainSourceColumns.length == 0) {
            throw new Error("plainSourceColumns cannot be empty");
        }

        const result = Array<SourceColumn>();

        for (const plainColumn of plainSourceColumns) {
            const column = this.convertToColumn(plainColumn);
            this.columnReferences.set(plainColumn["$id"], column);
            result.push(column);
        }

        return result;
    }

    public convertToColumn(plainColumn: any) : SourceColumn {
        if (plainColumn == null) {
            throw new Error("plainColumnMapping cannot be null or undefined");
        }
        
        return plainToInstance(SourceColumn, plainColumn);
    }

    public convertToJoinCondition(plainJoinCondition: any) : null | JoinCondition {
        if (plainJoinCondition == null) {
            return null;
        }

        let conditionLink : ConditionLink | null = null; 
        const plainConditionLink = plainJoinCondition["conditionLink"];
        if (plainConditionLink != null) {
            conditionLink  = new ConditionLink(
                PlainToSourceEntityConvertor.safeGetLinkRelation(plainConditionLink["relation"]),
                this.convertToJoinCondition(plainConditionLink["leftCondition"])
                    ?? (() => { throw new Error("Chained condition cannot be null.") })())
        }

        return new JoinCondition(
            plainJoinCondition["relation"],
            this.getSourceColumnByReference(plainJoinCondition["leftColumn"]),
            this.getSourceColumnByReference(plainJoinCondition["rightColumn"]),
            conditionLink);
    }

    private static safeGetLinkRelation(value: any) : LinkRelation {
        if (value == null) {
            throw new Error("value cannot be null");
        }

        const result = value["relation"];
        if (result === undefined) {
            throw new Error("relation property is missing");
        }

        if (!(result in Object.values(LinkRelation))) {
            throw new Error("relation property is not a valid LinkRelation");
        }

        return result as LinkRelation;
    }

    public convertToSourceEntity(value: any) : SourceEntity {

        if (value == null) {
            throw new Error("value cannot be null");
        }

        const refValue = value["$ref"];
        if (refValue !== undefined) {
            return this.sourceEntityReferences.get(refValue)
                ?? (() => { throw new Error("Source entity with id ${refValue} does not exist") })();
        }

        let result : SourceEntity;
        let id : string = value["$id"];

        switch (value["type"]) {
            case SourceTable.typeDescriptor: {
                result = new SourceTable(
                    PlainToSourceEntityConvertor.safeGetProperty(value, "name"),
                    value["schema"] !== undefined ? value["schema"] : null,
                    this.convertSourceColumns(value.selectedColumns));
                break;
            }
            case Join.typeDescriptor: {
                const leftSourceEntity = this.convertToSourceEntity(value["leftSourceEntity"]);
                const rightSourceEntity = this.convertToSourceEntity(value["rightSourceEntity"]);

                const plainCondition = value["joinCondition"];
                const joinCondition = this.convertToJoinCondition(plainCondition);
                if (joinCondition == null) {
                    throw new Error("joinCondition cannot be null");
                }

                result = new Join(
                    PlainToSourceEntityConvertor.safeGetProperty(value, "name"),
                    PlainToSourceEntityConvertor.parseJoinType(value),
                    leftSourceEntity,
                    rightSourceEntity as SourceEntityBase,
                    joinCondition);

                    break;
            }
            case CustomQuery.typeDescriptor: {
                result = new CustomQuery(
                    PlainToSourceEntityConvertor.safeGetProperty(value, "name"),
                    PlainToSourceEntityConvertor.safeGetProperty(value, "query"),
                    this.convertSourceColumns(value.selectedColumns));

                break;
            }
            default:{
                throw new Error("type property is missing");
            }
        }

        this.sourceEntityReferences.set(id, result);
        return result;
    }

    private static safeGetProperty(value: any, propertyName: string) : any {
        const result = value[propertyName];
        if (result === undefined) {
            throw new Error(`Property ${propertyName} is missing`);
        }

        return result;
    }

    private static parseJoinType(value: any) : JoinType {
        if (value == null) {
            throw new Error("value cannot be null");
        }

        const result = value["joinType"];
        if (result === undefined) {
            throw new Error("joinType property is missing");
        }

        const values = Object.values(JoinType);

        if (!values.includes(result)) {
            throw new Error("joinType property is not a valid JoinType");
        }

        return result as JoinType;
    }

    public getSourceColumnByReference(reference: any) {
        if (reference == null) {
            throw new Error("reference cannot be null");
        }

        const id = reference["$ref"];
        if (id === undefined) {
            throw new Error("reference must have a $ref property");
        }
        
        const column = this.columnReferences.get(id);
        if (column === undefined) {
            throw new Error(`Column with id ${id} does not exist`);
        }

        return column;
    }
}