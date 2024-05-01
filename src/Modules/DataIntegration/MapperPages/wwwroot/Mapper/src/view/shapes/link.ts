import { shapes, dia, util } from "@joint/core";
import { LIGHT_COLOR, SECONDARY_DARK_COLOR } from "../../constants";

export abstract class Link extends shapes.standard.DoubleLink {
    // defaults() {
    //     return util.defaultsDeep(super.defaults);
    // }

    /**
     * Returns the attributes for the link when it is highlighted.
     */
    protected abstract getHighlightedAttrs(); 

    /**
     * Returns the attributes for the link when it is unhighlighted.
     */
    protected abstract getUnhighlightedAttrs(); 

    public abstract handleRemoving(opt?: dia.Cell.DisconnectableOptions): this;

    /**
     * Highlights the link.
     * 
     * Sets the z index to a high value and other attributes to highlight the link.
     */
    public highlight() : void {
        this.attr({z: 100, ...this.getHighlightedAttrs()});
    }

    /**
     * Unhighlights the link.
     * 
     * Restores the z index and other attributes to unhighlight the link.
     */
    public unhighlight(): void {
        this.attr({z: -1, ...this.getUnhighlightedAttrs()});
    }
} 