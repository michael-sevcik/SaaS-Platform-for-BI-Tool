import { Column } from "../../../dbModel/database";
import { ConditionLink } from "../../aggregators/conditions/conditionLink";
import { JoinCondition } from "../../aggregators/conditions/joinCondition";
import { Join } from "../../aggregators/join";
import { SourceColumn } from "../../sourceColumn";
import { SourceEntityBase } from "../../sourceEntityBase";
import { SourceEntity } from "../../sourceEntity";
import { SourceTable } from "../../sourceTable";
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
                plainConditionLink["relation"], // TODO: check the type of the relation
                this.convertToJoinCondition(plainConditionLink["leftCondition"])
                    ?? (() => { throw new Error("Chained condition cannot be null.") })())
        }

        return new JoinCondition(
            plainJoinCondition["relation"],
            this.getSourceColumnByReference(plainJoinCondition["leftColumn"]),
            this.getSourceColumnByReference(plainJoinCondition["rightColumn"]),
            plainConditionLink);
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

        // TODO: Check the type names used
        // TODO: deal with type errors - missing properties, arrays not being arrays, etc.
        switch (value["type"]) {
            case "sourceTable": {
                result = new SourceTable(
                    value["name"],
                    value["schema"] ? value["schema"] : null,
                    this.convertSourceColumns(value.selectedColumns));
                break;
            }
            case "join": {
                const leftSourceEntity = this.convertToSourceEntity(value["leftSourceEntity"]);
                const rightSourceEntity = this.convertToSourceEntity(value["rightSourceEntity"]);

                // const outputColumns = Array<SourceColumn>();
                // value["outputColumns"].forEach(plainColumnMapping => {
                //     outputColumns.push(this.convertToColumnMapping(plainColumnMapping));
                // });

                const plainCondition = value["joinCondition"];
                const joinCondition = this.convertToJoinCondition(plainCondition);
                if (joinCondition == null) {
                    throw new Error("joinCondition cannot be null");
                }

                result = new Join(
                    value["name"],
                    value["joinType"], // TODO: The value should be probably checked
                    leftSourceEntity,
                    rightSourceEntity as SourceEntityBase,
                    joinCondition);

                    break;
            }
            default:{
                throw new Error("type property is missing");
            }
        }

        this.sourceEntityReferences.set(id, result);
        return result;
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