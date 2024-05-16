
// TODO: Convert mapping entities to plain objects - use sourceEntityConvertorVisitor
// TODO: Convert plain objects to mapping entities

import { instanceToPlain, plainToInstance } from "class-transformer";
import { MappingConfig } from "../mappingConfig";


// TODO: add @isString() decorator to all string properties - https://github.com/typestack/class-transformer#implicit-type-conversion
// TODO: Consider dividing this class into two classes
export class MappingModelConvertor {
    public static convertToMappingModel(plain : any) : MappingConfig {
        const result = plainToInstance(MappingConfig, plain);
        return result;
    }

    public static convertToPlain(mappingConfig: MappingConfig) : any {
        const result = instanceToPlain(mappingConfig);
        return result;
    }
}