import { Table } from "../../dbModel/database";
import { SearchableDropdown } from "../elements/searchableDropdown";
import { BaseModal } from "./baseModal";

export class SourceTablePickerModal extends BaseModal {
    private _tableToAdd: Table | null = null;
    private readonly tablePicker: SearchableDropdown;
    protected name: string = 'Source table picker';

    public get tableToAdd(): Table | null { return this._tableToAdd; }

    public constructor(tables: Table[]) {
        super();

        this.setTitle(this.name);
        const instructions = document.createElement('p');
        instructions.textContent = 'Vyberte zdrojovou tabulku k přidání: \n';
        this.modalContent.appendChild(instructions);

        this.tablePicker = new SearchableDropdown(
            this.modalContent,
            () => tables.map(table => table.name),
            "Tabulka k přidání",
            index => this._tableToAdd = tables[index]);
    }

    protected cancel(): void {
        this.tablePicker.clear();
    }

    protected save(): boolean {
        if (this._tableToAdd === null) {
            this.tablePicker.displayIncorrect();
            alert('Vyberte tabulku k přidání');
            return false;
        }
        
        this.tablePicker.clear();
        return true;
    }
}