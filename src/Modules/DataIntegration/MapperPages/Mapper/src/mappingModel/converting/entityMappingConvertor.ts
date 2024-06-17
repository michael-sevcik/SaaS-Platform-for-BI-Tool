import { EntityMapping } from "../entityMapping";
import { SourceColumn } from "../sourceColumn";
import { MappingToPlainConverterVisiter } from "./serialization/mappingToPlainConverterVisitor";
import { PlainToSourceEntityConvertor } from "./deserialization/plainToSourceEntityConvertor";
import type { SourceEntity } from "../sourceEntities/sourceEntity";

export class EntityMappingConvertor {
    static convertEntityMappingToPlain(entityMapping: EntityMapping) : any {
        let plainSourceEntity : any | null = null;
        let plainSourceEntities : SourceEntity[] = [];
        const visitor = new MappingToPlainConverterVisiter();
        entityMapping.createBackwardConnections();
        if (entityMapping.sourceEntity !== null) {
            entityMapping.sourceEntity.accept(visitor);
            plainSourceEntity = visitor.popResult();
            plainSourceEntities = visitor.sortedSourceEntities;
        }
        
        const plainColumnMappings = {};
        entityMapping.columnMappings.forEach((sourceColumn, key, map) => {
            let result : any| null;
            console.log(sourceColumn);
            
            if (sourceColumn === null || sourceColumn === undefined) {
                result = null;
            }
            else {
                visitor.visitSourceColumn(sourceColumn);
                result = visitor.popResult();
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

    static convertPlainToEntityMapping(value: any) : EntityMapping {
        const convertor = new PlainToSourceEntityConvertor();
        const plainSourceEntities = value["sourceEntities"];
        const sourceEntities = Array<SourceEntity>();
        for (const plainSourceEntity of plainSourceEntities) {
            sourceEntities.push(convertor.convertToSourceEntity(plainSourceEntity));
        }

        let sourceEntity : SourceEntity | null = null; 
        if (value["sourceEntity"] != null) {
            sourceEntity = convertor.convertToSourceEntity(value["sourceEntity"]);
        }

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
            value["schema"],
            sourceEntity,
            sourceEntities,
            columnMappings);

        entityMapping.createBackwardConnections();
        return entityMapping;
    }
}