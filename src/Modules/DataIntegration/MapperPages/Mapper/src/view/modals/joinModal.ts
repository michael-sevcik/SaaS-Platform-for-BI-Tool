import { JoinCondition, Operator } from "../../mappingModel/aggregators/conditions/joinCondition";
import { Join } from "../../mappingModel/aggregators/join";
import { SourceColumn } from "../../mappingModel/sourceColumn";
import { SearchableDropdown } from "../elements/searchableDropdown";
import { BaseModal } from "./baseModal";

export class JoinModal extends BaseModal{
    private static getColumnDescription(column: SourceColumn) : string {
        return `${column.owner?.name}_${column.name}: ${column.dataType.Descriptor}`;
    }

    private leftColumn: SourceColumn | null = null;
    private readonly leftColumnPicker: SearchableDropdown;
    private operator: Operator | null = null;
    private readonly operatorPicker: SearchableDropdown;
    private rightColumn: SourceColumn | null = null;
    private readonly rightColumnPicker: SearchableDropdown;
    protected name = 'JoinModal';

    public constructor(public join : Join) {
        super();
        
        this.setTitle(this.name);
        const instructions = document.createElement('p');
        instructions.textContent = 'Vyberte sloupce pro podmínku Join: \n';
        this.modalContent.appendChild(instructions);

        const joinCondition = this.join.condition;
        const leftColumns = this.join.leftSourceEntity.selectedColumns;
        const rightColumns = this.join.rightSourceEntity.selectedColumns;

        const pickColumnsDiv = document.createElement('div');
        pickColumnsDiv.classList.add('dropdown');
        
        this.leftColumnPicker = new SearchableDropdown(
            pickColumnsDiv,
            () => leftColumns.map(JoinModal.getColumnDescription),
            'Left column',
            (optionIndex) => this.leftColumn = leftColumns[optionIndex]);

        const operatorOptions = Object.values(Operator);
        this.operatorPicker = new SearchableDropdown(
            pickColumnsDiv,
            () => operatorOptions,
            'Operator',
            (optionIndex) => this.operator = operatorOptions[optionIndex]);

        this.rightColumnPicker = new SearchableDropdown(
            pickColumnsDiv,
            () => rightColumns.map(JoinModal.getColumnDescription),
            'Left column',
            (optionIndex) => this.rightColumn = rightColumns[optionIndex]);
    
        this.modalContent.appendChild(pickColumnsDiv);
        this.setDefaultValues();
    }

    private setDefaultValues() : void {
        const joinCondition = this.join.condition;
        if (joinCondition !== null) {
            this.leftColumnPicker.setPlaceHolder(JoinModal.getColumnDescription(joinCondition.leftColumn));
            this.leftColumn = joinCondition.leftColumn;	

            this.operator = joinCondition.relation;
            this.operatorPicker.setPlaceHolder(joinCondition.relation);

            this.rightColumn = joinCondition.rightColumn;
            this.rightColumnPicker.setPlaceHolder(JoinModal.getColumnDescription(joinCondition.rightColumn));
        }
        
        this.leftColumnPicker.displayCorrect();
        this.operatorPicker.displayCorrect();
        this.rightColumnPicker.displayCorrect();
    }

    protected cancel(): void {
        this.setDefaultValues();
    }

    protected save(): boolean {
        if (this.leftColumn === null) {
            this.leftColumnPicker.displayIncorrect();
            alert('Vyberte sloupec pro podmínku Join');
            return false;
        }
        if (this.operator === null) {
            this.operatorPicker.displayIncorrect();
            alert('Vyberte vztah pro podmínku Join');
            return false;
        }
        if (this.rightColumn === null) {
            this.rightColumnPicker.displayIncorrect();
            alert('Vyberte sloupec pro podmínku Join');
            return false;
        }

        // TODO: handle different types of joins
        if (!this.leftColumn.isAssignableWith(this.rightColumn)) {
            this.leftColumnPicker.displayIncorrect();
            this.rightColumnPicker.displayIncorrect();
            alert('Sloupce musí být porovnatelného typu.');
            return false;
        }

        if (this.join.condition === null) {
            this.join.condition = new JoinCondition(this.operator, this.leftColumn, this.rightColumn);
        }
        else {
            this.join.condition.relation = this.operator;
            this.join.condition.leftColumn = this.leftColumn;
            this.join.condition.rightColumn = this.rightColumn;
        }

        this.setDefaultValues();
        return true;
    }
}