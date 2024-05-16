import { Column } from "../dbModel/database";
import { Ownable } from "./ownable";
import { Owner } from "./owner";
import { MappingVisitor } from "./mappingVisitor";
import { Visitable } from "./converting/visitable";
import { SourceColumn } from "./sourceColumn";

export abstract class SourceEntity implements Visitable, Owner, Ownable {
  public _owner : Owner | null = null;

  /**
   * Sets the owner of this entity for propagating changes.
   */
  public get owner() : Owner | null {
    return this._owner;
  }

  public abstract get selectedColumns() : SourceColumn[];
  public constructor(public name: string) { }

  public abstract accept(visitor: MappingVisitor): void;
  public abstract createBackwardConnections(): void;
  abstract replaceChild(oldChild: any, newChild: any): void;
  /**
   * Sets the owner of this entity for propagating changes.
   */
  public set owner(owner : Owner) {
    this._owner = owner;
  }
}