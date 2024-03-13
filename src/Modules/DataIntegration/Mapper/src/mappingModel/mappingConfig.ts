import { Transform, Type,  } from "class-transformer";

import { DbConnectionConfig } from "./dbConnectionConfig";
import { EntityMapping } from "./entityMapping";
import { EntityMappingConvertor } from "./serialization/entityMappingConvertor";

export class MappingConfig {
    constructor(sourceConnection : DbConnectionConfig, targetMappings : EntityMapping[]) {
        this.sourceConnection = sourceConnection;
        this.targetMappings = targetMappings;
    }
    @Type(() => DbConnectionConfig)
    public sourceConnection : DbConnectionConfig;

    @Type(() => EntityMapping)
    @Transform(({ value, key, obj, type }) => value.map(EntityMappingConvertor.convertEntityMappingToPlain), { toPlainOnly: true })
    @Transform(({ value, key, obj, type }) => value.map(EntityMappingConvertor.convertPlainToEntityMapping), { toClassOnly: true })
    public targetMappings : EntityMapping[];
}