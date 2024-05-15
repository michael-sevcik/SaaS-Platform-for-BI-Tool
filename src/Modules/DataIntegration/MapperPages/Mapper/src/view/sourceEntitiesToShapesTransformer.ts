import { Column, Database, Table } from "../dbModel/database";
import { ConditionLink } from "../mappingModel/Agregators/Conditions/conditionLink";
import { JoinCondition } from "../mappingModel/Agregators/Conditions/joinCondition";
import { Join } from "../mappingModel/Agregators/join";
import { MappingVisitor } from "../mappingModel/serialization/mappingVisitor";
import { SourceColumn } from "../mappingModel/sourceColumn";
import { SourceEntity } from "../mappingModel/sourceEntity";
import { SourceTable } from "../mappingModel/sourceTable";
import { BaseEntityShape } from "./shapes/baseEntityShape";
import { JoinLink } from "./shapes/joinLink";
import { JoinModal } from "./modals/joinModal";
import { SourceTableShape } from "./shapes/sourceTableShape";

import { dia } from "@joint/core";


export class SourceEntitiesToShapesTransformer extends MappingVisitor{
    /**
     * Element stack of source entity shapes
     */
    private readonly cellStack = new Array<dia.Cell>();
    private readonly sourceTablesByName = new Map<string, Table>();

    public readonly cells = new Array<dia.Cell>();
    public readonly elementMap = new Map<SourceEntity, BaseEntityShape>();

    public constructor(sourceTables: Table[]) {
        super();
        sourceTables.forEach((table) =>
            this.sourceTablesByName.set(table.name, table)
        );
    }

    public visitConditionLink(conditionLink: ConditionLink): void {
        throw new Error("Method not implemented.");
    }

    public visitJoin(join: Join): void {
        const joinModal = new JoinModal(join);
        
        join.leftSourceEntity.accept(this);
        let leftSourceEntityShape = this.cellStack.pop();
        if (leftSourceEntityShape.isLink()) {
            const link = leftSourceEntityShape as JoinLink;
            const joinTargetEntity = link.join.rightSourceEntity;
            leftSourceEntityShape = this.elementMap.get(joinTargetEntity);
        }
        
        join.rightSourceEntity.accept(this);
        const rightSourceEntityShape = this.cellStack.pop();
        
        const joinLink = new JoinLink(join, joinModal);
        joinLink.source(leftSourceEntityShape);
        joinLink.target(rightSourceEntityShape);
        joinLink.source()

        this.cells.push(joinLink);
        console.log('Join link');
        console.log(joinLink);
    }

    public visitJoinCondition(joinCondition: JoinCondition): void {
        throw new Error("Method not implemented.");
    }

    public visitSourceColumn(sourceColumn: SourceColumn): void {
        throw new Error("Method not implemented.");
    }

    public visitSourceTable(sourceTable: SourceTable): void {
        const shape = new SourceTableShape(sourceTable);
        this.elementMap.set(sourceTable, shape);
        this.cells.push(shape);
        this.cellStack.push(shape);
    }
}