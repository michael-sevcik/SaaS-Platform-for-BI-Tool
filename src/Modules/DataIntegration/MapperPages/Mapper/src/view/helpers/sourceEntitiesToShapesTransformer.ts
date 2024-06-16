import { Table } from "../../dbModel/database";
import { Join } from "../../mappingModel/aggregators/join";
import { SourceTable } from "../../mappingModel/sourceEntities/sourceTable";
import { BaseEntityShape } from "../shapes/baseEntityShape";
import { JoinLink } from "../shapes/joinLink";
import { JoinModal } from "../modals/joinModal";
import { SourceTableShape } from "../shapes/sourceTableShape";

import { dia } from "@joint/core";
import type { SourceEntity } from "../../mappingModel/sourceEntities/sourceEntity";
import { CustomQuery } from "../../mappingModel/sourceEntities/customQuery";
import { CustomQueryShape } from "../shapes/customQueryShape";
import { SourceEntityVisitor } from "../../mappingModel/sourceEntityVisitor";


export class SourceEntitiesToShapesTransformer extends SourceEntityVisitor {
    /**
     * Element stack of source entity shapes
     */
    private readonly cellStack = new Array<dia.Cell>();
    private readonly sourceTablesByName = new Map<string, Table>();

    public readonly cells = new Array<dia.Cell>();
    public readonly elementMap = new Map<SourceEntity, BaseEntityShape>();

    public constructor(sourceTables: Table[]) {
        super();
        // TODO: Consider using also the schema name as a key
        sourceTables.forEach((table) =>
            this.sourceTablesByName.set(table.schema + table.name, table)
        );
    }

    public visitCustomQuery(customQuery: CustomQuery): void {
        const shape = new CustomQueryShape(customQuery);
        this.elementMap.set(customQuery, shape);
        this.cells.push(shape);
        this.cellStack.push(shape);
    }

    public visitJoin(join: Join): void {
        const joinModal = new JoinModal(join);
        
        join.leftSourceEntity.accept(this);
        let leftSourceEntityShape = this.safeStackPop();
        if (leftSourceEntityShape.isLink()) {
            const link = leftSourceEntityShape as JoinLink;
            const joinTargetEntity = link.join.rightSourceEntity;
            leftSourceEntityShape = this.elementMap.get(joinTargetEntity)
                ?? (() => { throw new Error('Source entity shape not found.'); })();
        }
        
        join.rightSourceEntity.accept(this);
        const rightSourceEntityShape = this.safeStackPop();
        
        const joinLink = new JoinLink(join, joinModal);
        joinLink.source(leftSourceEntityShape);
        joinLink.target(rightSourceEntityShape);
        joinLink.source()

        this.cells.push(joinLink);
        console.log('Join link');
        console.log(joinLink);
        this.cellStack.push(joinLink);
    }

    public visitSourceTable(sourceTable: SourceTable): void {
        const shape = new SourceTableShape(sourceTable, this.getCorrespondingTable(sourceTable));
        this.elementMap.set(sourceTable, shape);
        this.cells.push(shape);
        this.cellStack.push(shape);
    }

    private safeStackPop(): dia.Cell {
        const value = this.cellStack.pop();
        if (value === undefined) {
            throw new Error('Cell stack is empty');
        }

        return value;
    }

    private getCorrespondingTable(sourceTable: SourceTable): Table {
        const table = this.sourceTablesByName.get(sourceTable.schema + sourceTable.name);
        if (table === undefined) {
            throw new Error('Table not found');
        }

        return table;
    }
}