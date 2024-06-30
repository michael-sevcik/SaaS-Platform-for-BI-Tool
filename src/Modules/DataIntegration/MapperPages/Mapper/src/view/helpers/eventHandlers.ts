import { dia, elementTools, linkTools } from "@joint/core";
import type { Link } from "../shapes/link";
import { BaseSourceEntityShape } from "../shapes/baseSourceEntityShape";
import type { BaseEntityShape } from "../shapes/baseEntityShape";
import { PropertyLink } from "../shapes/propertyLink";
import { contentDiv, TARGET_DATABASE_ENTITY_GROUP_NAME } from "../../constants";

/**
 * Adds tools to the element view when the mouse enters the element.
 * @param elementView The element view.
 */
export function onPaperElementMouseEnter(elementView: dia.ElementView) {
    const baseEntityShape = elementView.model as BaseEntityShape;
    if (baseEntityShape instanceof BaseSourceEntityShape) {
        const toolsView = new dia.ToolsView({
            tools: [new elementTools.Remove({ action: () => baseEntityShape.handleRemoving() })]
        });
        elementView.addTools(toolsView);
    }
}

/**
 * Removes tools from the link view when the mouse leaves the link.
 * @param linkView The link view.
 */
export function onPaperElementMouseLeave(linkView: dia.ElementView) {
    linkView.removeTools();
}

/**
 * Adds tools to the link view when the mouse enters the link.
 * @param linkView The link view.
 */
export function onPaperLinkMouseEnter(linkView: dia.LinkView) {
    const link = linkView.model as Link;
    const toolsView = new dia.ToolsView({
        tools: [new linkTools.Remove({ action: () => link.handleRemoving() })]
    });
    linkView.addTools(toolsView);
    if (link instanceof PropertyLink) console.debug('Property link');
    link.highlight();
}

/**
 * Removes tools from the link view when the mouse leaves the link.
 * @param linkView The link view.
 */
export function onPaperLinkMouseLeave(linkView: dia.LinkView) {
    linkView.removeTools();
    const link = linkView.model as Link;
    link.unhighlight();
}

/**
 * Handles the pointer move event to restrict element movement within container boundaries.
 * @param cellView The cell view.
 * @param evt The event object.
 * @param x The x-coordinate.
 * @param y The y-coordinate.
 */
export function handlePointerMove(cellView: dia.CellView, evt: dia.Event, x: number, y: number) {
    if (cellView.model.isElement()) {
        const element = cellView.model;
        const boundingBox = element.getBBox();
        const containerRect = contentDiv.getBoundingClientRect();
        const containerMinX = 0, containerMinY = 0, containerMaxX = containerRect.width, containerMaxY = containerRect.height;
        if (boundingBox.x < containerMinX || boundingBox.x + boundingBox.width > containerMaxX || boundingBox.y < containerMinY || boundingBox.y + boundingBox.height > containerMaxY) {
            var newX = Math.min(Math.max(boundingBox.x, containerMinX), containerMaxX - boundingBox.width);
            var newY = Math.min(Math.max(boundingBox.y, containerMinY), containerMaxY - boundingBox.height);
            element.position(newX, newY);
        }
    }
}

/**
 * Validates the source magnet for connection.
 * @param sourceView The source element view.
 * @param sourceMagnet The source magnet.
 * @returns True if the magnet is valid, false otherwise.
 */
export function validateMagnet(sourceView: dia.ElementView, sourceMagnet: SVGAElement) {
    const sourceGroup = sourceView.findAttribute("port-group", sourceMagnet);
    const sourcePort = sourceView.findAttribute("port", sourceMagnet);
    const source = sourceView.model;

    console.debug(sourceMagnet);
    console.debug(sourceView);
    console.debug(sourceGroup);

    if (sourceGroup === TARGET_DATABASE_ENTITY_GROUP_NAME) {
        console.debug("paper<validateMagnet>", "It's not possible to create a link from an inbound port.");
        return false;
    }

    return true;
}