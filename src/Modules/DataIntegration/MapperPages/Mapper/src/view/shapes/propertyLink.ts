import { dia, shapes, util } from "@joint/core";
import { HIGHLIGHTED_OUTLINE_COLOR, LIGHT_COLOR, SECONDARY_DARK_COLOR } from "../../constants";
import { Link } from "./link";
import { TargetElementShape } from "./targetElementShape";
import { PropertyPort } from "./propertyPort";

export class PropertyLink extends Link {
    private clickMethod() {
        console.log('clickMethod');
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

    handleConnection() {
        const targetElement = this.accessTargetElement();
        
        // HACK: this is a workaround for a for getting TargetElementShape
        const targetEntity = targetElement as unknown as TargetElementShape;
        if (targetEntity === undefined) {
            throw new Error('Cannot find the target element.');
        }

        const targetPort = targetElement.getPort(this.accessTargetPort()) as PropertyPort;
        if (targetPort === undefined) {
            throw new Error('Cannot find the target port.');
        }

        const sourceEntity = this.accessSourceElement();
        const sourcePort = sourceEntity.getPort(this.accessSourcePort()) as PropertyPort;

        targetEntity.setColumnMapping(sourcePort, targetPort);
    }
    public handleRemoving(opt?: dia.Cell.DisconnectableOptions): this {
        const element = this.accessTargetElement();
        
        // HACK: this is a workaround for a for getting TargetElementShape
        const targetElement = element as unknown as TargetElementShape;
        if (targetElement === undefined) {
            throw new Error('Cannot find the target element.');
        }

        const targetPort = element.getPort(this.accessTargetPort()) as PropertyPort;
        if (targetPort === undefined) {
            throw new Error('Cannot find the target port.');
        }

        targetElement.removeColumnMapping(targetPort);

        // TODO: update references source column references
        return this.remove(opt);
    }

    private accessTargetElement() : dia.Element {
        const element = this.getTargetElement();
        if (element == null) {
            throw new Error('Cannot find the target element.');
        }

        return element
    }

    private accessSourceElement(): dia.Element {
        const element = this.getSourceElement();
        if (element == null) {
            throw new Error('Cannot find the target element.');
        }

        return element
    }

    private accessTargetPort() : string {
        const targetPort = this.target().port;
        if (targetPort === undefined) {
            throw new Error('Target port is undefined.');
        }
        return targetPort;
    }

    private accessSourcePort(): string {
        const sourcePort = this.source().port;
        if (sourcePort === undefined) {
            throw new Error('Source port is undefined.');
        }
        return sourcePort;
    }
}