import { instanceToPlain, plainToInstance } from "class-transformer";
import { MappingConfig } from "../mappingConfig";

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