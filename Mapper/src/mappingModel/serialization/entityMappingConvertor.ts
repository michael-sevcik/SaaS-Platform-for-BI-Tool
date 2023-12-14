import { EntityMapping } from "../entityMapping";
import { SourceColumn } from "../sourceColumn";
import { SourceEntity } from "../sourceEntity";
import { MappingConverterVisiter } from "./mappingConverterVisitor";
import { PlainToSourceEntityConvertor } from "./plainToSourceEntityConvertor";

export class EntityMappingConvertor {
    static convertEntityMappingToPlain(entityMapping: EntityMapping) : any {
        entityMapping.createBackwardConnections(); // TODO: is this needed?
        const visitor = new MappingConverterVisiter();
        entityMapping.sourceEntity.accept(visitor);
        const plainSourceEntities = visitor.sortedSourceEntities;
        visitor.getResult();
        entityMapping.sourceEntity.accept(visitor);
        const plainSourceEntity = visitor.getResult();
        const plainColumnMappings = {};
        entityMapping.columnMappings.forEach((sourceColumn, key, map) => {
            let result : any| null;
            console.log(sourceColumn);
            
            if (sourceColumn === null || sourceColumn === undefined) {
                result = null;
            }
            else {
                visitor.visitSourceColumn(sourceColumn);
                result = visitor.getResult();
            }
            
            plainColumnMappings[key] = result
        });

        return {
            name: entityMapping.name,
            sourceEntity: plainSourceEntity,
            sourceEntities: plainSourceEntities,
            columnMappings: plainColumnMappings
        };
    }

    // TODO: catch errors
    static convertPlainToEntityMapping(value: any) : EntityMapping {
        const convertor = new PlainToSourceEntityConvertor();
        const plainSourceEntities = value["sourceEntities"];
        const sourceEntities = Array<SourceEntity>();
        for (const plainSourceEntity of plainSourceEntities) {
            sourceEntities.push(convertor.convertToSourceEntity(plainSourceEntity));
        }
        
        const sourceEntity = convertor.convertToSourceEntity(value["sourceEntity"]);

        const columnMappings = new Map<string, SourceColumn | null>();
        Object.entries(value["columnMappings"]).forEach(([key, value]) => {
            let column : SourceColumn | null = null;
            if (value != null) {
                column = convertor.getSourceColumnByReference(value);
            }

            columnMappings.set(key, column);
        });


        const entityMapping = new EntityMapping(
            value["name"],
            sourceEntity,
            sourceEntities,
            columnMappings);

        entityMapping.createBackwardConnections();
        return entityMapping;
    }
}