import { dia, shapes, util } from "jointjs";
import { HIGHLIGHTED_OUTLINE_COLOR, LIGHT_COLOR, SECONDARY_DARK_COLOR } from "../../constants";
import { Link } from "./link";
import { TargetElementShape } from "./targetElementShape";
import { PropertyPort } from "./propertyPort";

export class PropertyLink extends Link {
    handleConnection() {
        const targetElement = this.getTargetElement();
        
        // HACK: this is a workaround for a for getting TargetElementShape
        const targetEntity = targetElement as unknown as TargetElementShape;
        if (targetEntity === undefined) {
            throw new Error('Cannot find the target element.');
        }

        const targetPort = targetElement.getPort(this.target().port) as PropertyPort;
        if (targetPort === undefined) {
            throw new Error('Cannot find the target port.');
        }

        const sourceEntity = this.getSourceElement();
        const sourcePort = sourceEntity.getPort(this.source().port) as PropertyPort;

        targetEntity.setColumnMapping(sourcePort, targetPort);
    }
    public handleRemoving(opt?: dia.Cell.DisconnectableOptions): this {
        const element = this.getTargetElement();
        
        // HACK: this is a workaround for a for getting TargetElementShape
        const targetElement = element as unknown as TargetElementShape;
        if (targetElement === undefined) {
            throw new Error('Cannot find the target element.');
        }

        const targetPort = element.getPort(this.target().port) as PropertyPort;
        if (targetPort === undefined) {
            throw new Error('Cannot find the target port.');
        }

        targetElement.removeColumnMapping(targetPort);

        // TODO: update references source column references
        return this.remove(opt);
    }
    
    protected getHighlightedAttrs() {
        return {
            line: {
                stroke: SECONDARY_DARK_COLOR,
            },
            outline: {
                stroke: HIGHLIGHTED_OUTLINE_COLOR
            }
        }
    }

    protected getUnhighlightedAttrs() {
        return {
            line: {
                stroke: LIGHT_COLOR,
            },
            outline: {
                stroke: SECONDARY_DARK_COLOR
            }
        
        }
    }

    defaults() {
        return util.defaultsDeep({
            type: 'ListLink',
            z: -1,
            attrs: {
                line: {
                    stroke: LIGHT_COLOR,
                    targetMarker: {
                        stroke: SECONDARY_DARK_COLOR
                    }
                },
                outline: {
                    stroke: SECONDARY_DARK_COLOR
                }
            }
        }, super.defaults);
    }

    clickMethod() {
        console.log('clickMethod');
    }
}