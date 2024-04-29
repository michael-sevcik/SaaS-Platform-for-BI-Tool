import { shapes } from '@joint/core';

import { SourceTableShape } from './sourceTableShape';
import { TargetTableShape } from './targetTableShape';
import { PropertyLink } from './propertyLink';

export const SHAPE_NAMESPACE = {
    ...shapes,
    SourceTableShape: SourceTableShape,
    TargetTableShape: TargetTableShape,
    propertyLink: PropertyLink
};