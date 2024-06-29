import { shapes, dia } from "@joint/core";

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

    /**
     * Handles the removal of the source of the link.
     * 
     * Removes the link only.
     * This method is called when the target of the link is removed.
     * @param opt The options for removing the link.
     */
    public abstract handleTargetRemoval(opt?: dia.Cell.DisconnectableOptions): this;

    /**
     * Removes the link and its target.
     * 
     * @param opt The options for removing the link.
     * @note This method is called when the source of the link is removed or the link should be removed.
     */
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