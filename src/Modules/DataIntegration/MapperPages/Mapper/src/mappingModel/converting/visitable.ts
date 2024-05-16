import { MappingVisitor } from "../mappingVisitor";

export interface Visitable {
    accept(visitor: MappingVisitor): void;
}