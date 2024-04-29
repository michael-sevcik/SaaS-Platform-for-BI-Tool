import { dia, util } from "jointjs";
import { HIGHLIGHTED_OUTLINE_COLOR, LIGHT_COLOR, SECONDARY_DARK_COLOR } from "../../constants";
import { JoinModal } from "../modals/joinModal";
import { Link } from "./link";
import { BaseSourceEntityShape } from "./baseSourceEntityShape";
import { Join } from "../../mappingModel/Agregators/join";

export class JoinLink extends Link {
    public handleRemoving(opt?: dia.Cell.DisconnectableOptions): this {
        const sourceEntity = this.getTargetElement() as BaseSourceEntityShape;
        sourceEntity.handleRemoving();
        this.join.owner.replaceChild(this.join, this.join.leftSourceEntity);
        this.joinModal.finalize();
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
                stroke: SECONDARY_DARK_COLOR,
            },
            outline: {
                stroke: SECONDARY_DARK_COLOR
            }
        
        }
    }
    
    defaults() {
        return util.defaultsDeep({
            type: 'JoinLink',
            z: -1,
            attrs: {
                line: {
                    stroke: SECONDARY_DARK_COLOR,
                    targetMarker: {
                        stroke: SECONDARY_DARK_COLOR,
                        d: '' // set the d attribute to an empty string to remove the arrow head
                    }
                },
                outline: {
                    stroke: SECONDARY_DARK_COLOR
                }
            },
            labels: [
                {
                    attrs: {
                        text: {
                            text: 'join'
                        }
                    }
                }
            ],
        }, super.defaults);
    }

    public constructor(public join: Join, public joinModal : JoinModal, ...args: any[]) {
        super();
        super.initialize.call(this, ...args);
    }

    handleDoubleClick() {
        this.joinModal.open();
    }
}