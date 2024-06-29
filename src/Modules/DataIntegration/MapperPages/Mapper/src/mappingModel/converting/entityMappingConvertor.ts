import { EntityMapping } from "../entityMapping";
import { SourceColumn } from "../sourceEntities/sourceColumn";
import { MappingToPlainConverterVisiter } from "./serialization/mappingToPlainConverterVisitor";
import { PlainToSourceEntityConvertor } from "./deserialization/plainToSourceEntityConvertor";
import type { SourceEntity } from "../sourceEntities/sourceEntity";


/**
 * Converts EntityMapping objects to plain JavaScript objects and vice versa.
 */
export class EntityMappingConvertor {
    /**
     * Converts an EntityMapping object to a plain JavaScript object.
     * @param entityMapping - The EntityMapping object to convert.
     * @returns The converted plain JavaScript object.
     */
    static convertEntityMappingToPlain(entityMapping: EntityMapping): any {
        let plainRootSourceEntity: any | null = null;
        let plainSourceEntities: SourceEntity[] = [];
        const visitor = new MappingToPlainConverterVisiter();
        entityMapping.createBackwardConnections();
        if (entityMapping.sourceEntity !== null) {
            entityMapping.sourceEntity.accept(visitor);
            [plainSourceEntities, plainRootSourceEntity] = visitor.getResult();
        }
        
        const plainColumnMappings = {};
        entityMapping.columnMappings.forEach((sourceColumn, key, map) => {
            let result: any | null;
            console.log(sourceColumn);
            
            if (sourceColumn === null || sourceColumn === undefined) {
                result = null;
            }
            else {
                result = visitor.getSourceColumnRef(sourceColumn);
            }
            
            plainColumnMappings[key] = result
        });

        return {
            name: entityMapping.name,
            schema: entityMapping.schema,
            mappingData: [
                plainSourceEntities,
                plainRootSourceEntity,
                plainColumnMappings
            ]
        };
    }

    /**
     * Converts a plain JavaScript object to an EntityMapping object.
     * @param value - The plain JavaScript object to convert.
     * @returns The converted EntityMapping object.
     */
    static convertPlainToEntityMapping(value: any): EntityMapping {
        const convertor = new PlainToSourceEntityConvertor();
        const mappingData = value["mappingData"];
        const plainSourceEntities = mappingData[0];
        const plainRootSourceEntity = mappingData[1];
        const plainColumnMappings = mappingData[2];
        const sourceEntities = Array<SourceEntity>();
        for (const plainSourceEntity of plainSourceEntities) {
            sourceEntities.push(convertor.convertToSourceEntity(plainSourceEntity));
        }

        let sourceEntity: SourceEntity | null = null; 
        if (plainRootSourceEntity != null) {
            sourceEntity = convertor.convertToSourceEntity(plainRootSourceEntity);
        }

        const columnMappings = new Map<string, SourceColumn | null>();
        Object.entries(plainColumnMappings).forEach(([key, value]) => {
            let column: SourceColumn | null = null;
            if (value != null) {
                column = convertor.getSourceColumnByReference(value);
            }

            columnMappings.set(key, column);
        });

        const entityMapping = new EntityMapping(
            value["name"],
            value["schema"] == null ? null : value["schema"],
            sourceEntity,
            sourceEntities,
            columnMappings);

        entityMapping.createBackwardConnections();
        return entityMapping;
    }
}