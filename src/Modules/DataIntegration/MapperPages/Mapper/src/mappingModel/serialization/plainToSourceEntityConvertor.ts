import { Column } from "../../dbModel/database";
import { ConditionLink } from "../Agregators/Conditions/conditionLink";
import { JoinCondition } from "../Agregators/Conditions/joinCondition";
import { Join } from "../Agregators/join";
import { SourceColumn } from "../sourceColumn";
import { SourceConcreteEntity } from "../sourceConcreteEntity";
import { SourceEntity } from "../sourceEntity";
import { SourceTable } from "../sourceTable";

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

    public convertToColumn(column: any) : SourceColumn {
        if (column == null) {
            throw new Error("plainColumnMapping cannot be null");
        }
        
        return new SourceColumn(new Column (
            column["name"],
            column["type"]));
    }

    public convertToJoinCondition(plainJoinCondition: any) : null | JoinCondition {
        if (plainJoinCondition == null) {
            return null;
        }

        let conditionLink : ConditionLink | null = null; 
        const plainCondtionLink = plainJoinCondition["conditionLink"];
        if (plainCondtionLink != null) {
            conditionLink  = new ConditionLink(
                plainCondtionLink["relation"], // TODO: check the type of the relation
                 this.convertToJoinCondition(plainCondtionLink["leftCondition"]));
        }

        return new JoinCondition(
            plainJoinCondition["relation"],
            this.getSourceColumnByReference(plainJoinCondition["leftColumn"]),
            this.getSourceColumnByReference(plainJoinCondition["rightColumn"]),
            plainCondtionLink);
    }

    public convertToSourceEntity(value: any) : SourceEntity {

        if (value == null) {
            throw new Error("value cannot be null");
        }

        const refValue = value["$ref"];
        if (refValue !== undefined) {
            return this.sourceEntityReferences.get(refValue);
        }

        let result : SourceEntity;
        let id : string = value["$id"];

        // TODO: Check the type names used
        // TODO: deal with type errors - missing properties, arrays not being arrays, etc.
        switch (value["type"]) {
            case "sourceTable": {
                result = new SourceTable(
                    value["name"],
                    this.convertSourceColumns(value.selectedColumns));
                break;
            }
            case "join": {
                const leftSourceEntity = this.convertToSourceEntity(value["leftSourceEntity"]);
                const rightSourceEntity = this.convertToSourceEntity(value["rightSourceEntity"]);

                // const outputColumns = Array<SourceColumn>();
                // value["outputColumns"].forEach(plainColumnMapping => {
                //     outputColumns.push(this.convertToColunmMapping(plainColumnMapping));
                // });

                const plainCondition = value["joinCondition"];
                const joinCondition = this.convertToJoinCondition(plainCondition);
                if (joinCondition == null) {
                    throw new Error("joinCondition cannot be null");
                }

                result = new Join(
                    value["name"],
                    value["joinType"], // TODO: The value should be probalby checked
                    leftSourceEntity,
                    rightSourceEntity as SourceConcreteEntity,
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