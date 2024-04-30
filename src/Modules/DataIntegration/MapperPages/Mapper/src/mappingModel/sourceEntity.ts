import { Column } from "../dbModel/database";
import { Ownable } from "./ownable";
import { Owner } from "./owner";
import { MappingVisitor } from "./serialization/mappingVisitor";
import { Visitable } from "./serialization/visitable";
import { SourceColumn } from "./sourceColumn";

export abstract class SourceEntity implements Visitable, Owner, Ownable {
  public _owner : Owner | null = null;
  public abstract get selectedColumns() : SourceColumn[];
  public constructor(public name: string) { }
  abstract replaceChild(oldChild: any, newChild: any): void;
  public abstract accept(visitor: MappingVisitor): void;

  public abstract createBackwardConnections(): void;

  /**
   * Sets the owner of this entity for propagating changes.
   */
  public set owner(owner : Owner) {
    this._owner = owner;
  }

  /**
   * Sets the owner of this entity for propagating changes.
   */
  public get owner() : Owner | null {
    return this._owner;
  }
}