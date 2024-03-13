import { Table } from "../../dbModel/database";
import { SearchableDropdown } from "../elements/searchableDropdown";
import { BaseModal } from "./baseModal";

export class SourceTablePickerModal extends BaseModal {
    protected name: string = 'Source table picker';
    protected save(): boolean {
        if (this._tableToAdd === null) {
            this.tablePicker.displayIncorrect();
            alert('Vyberte tabulku k přidání');
            return false;
        }
        
        return true;
    }
    protected cancel(): void {
        
    }

    private readonly tablePicker: SearchableDropdown;
    private _tableToAdd: Table | null = null;
    public get tableToAdd(): Table | null { return this._tableToAdd; }

    public constructor(tables: Table[]) {
        super();

        this.setTitle(this.name);
        const instructions = document.createElement('p');
        instructions.textContent = 'Vyberte zdrojovou tabulku k přidání: \n';
        this.body.appendChild(instructions);

        this.tablePicker = new SearchableDropdown(
            this.body,
            () => tables.map(table => table.name),
            "Tabulka k přidání",
            index => this._tableToAdd = tables[index]);
    }

}