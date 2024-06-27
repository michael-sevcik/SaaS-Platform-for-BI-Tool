import { Ownable } from "../ownable";
import { Owner } from "../owner";
import { MappingVisitable } from "../converting/mappingVisitable";
import { SourceColumn } from "./sourceColumn";
import type { VisitableSourceEntity } from "../visitable";
import type { SourceEntityVisitor } from "../sourceEntityVisitor";

export abstract class SourceEntity implements MappingVisitable, VisitableSourceEntity, Owner, Ownable {
  public _owner : Owner | null = null;

  /**
   * Sets the owner of this entity for propagating changes.
   */
  public get owner() : Owner {
    return this._owner ?? (() => { throw new Error('Owner not set.'); })();
  }

  public abstract get selectedColumns() : SourceColumn[];
  public constructor(public name: string) { }

  /** @inheritdoc */
  public abstract removeReferences(): void;

  public abstract accept(visitor: SourceEntityVisitor): void;
  public abstract createBackwardConnections(): void;
  abstract replaceChild(oldChild: any, newChild: any): void;
  /**
   * Sets the owner of this entity for propagating changes.
   */
  public set owner(owner : Owner) {
    this._owner = owner;
  }
}