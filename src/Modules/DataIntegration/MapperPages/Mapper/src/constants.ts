import { dia, g } from '@joint/core'

export const GRID_SIZE = 8;
export const PADDING_S = GRID_SIZE;
export const PADDING_L = GRID_SIZE * 2;
export const FONT_FAMILY = 'sans-serif';
export const LIGHT_COLOR = '#FFF';
export const DARK_COLOR = '#333';
export const SECONDARY_DARK_COLOR = '#999';
export const HIGHLIGHTED_OUTLINE_COLOR = '#ffd966';
export const LINE_WIDTH = 2;

export const HEADER_ICON_SIZE = 50;
export const HEADER_HEIGHT = 80;

export const DATABASE_ENTITY_MAX_PORT_COUNT = 20;
export const SOURCE_DATABASE_ENTITY_GROUP_NAME = 'sourceDbEntity';
export const DEFAULT_SOURCE_ENTITY_DESCRIPTION = 'Represents a table from the source database.';
export const TARGET_DATABASE_ENTITY_GROUP_NAME = 'targetDbEntity';
export const DEFAULT_TARGET_ENTITY_DESCRIPTION = 'Represents a table from the target database.';
export const DATABASE_ENTITY_PROPERTY_HEIGHT = 32;
export const DATABASE_ENTITY_PROPERTY_WIDTH = GRID_SIZE * 35;
export const DATABASE_ENTITY_PROPERTY_GAP = 1;

export const propertyPosition = (portsArgs: dia.Element.Port[], elBBox: dia.BBox): g.Point[] => {
    return portsArgs.map((_port: dia.Element.Port, index: number, { length }) => {
        const bottom = elBBox.height - (DATABASE_ENTITY_PROPERTY_HEIGHT) / 2 - PADDING_S;
        const y = (length - 1 - index) * (DATABASE_ENTITY_PROPERTY_HEIGHT + DATABASE_ENTITY_PROPERTY_GAP);
        return new g.Point(0, bottom - y);
    });
};

export const propertyAttributes = {
    attrs: {
        portBody: {
            magnet: 'active',
            width: 'calc(w)',
            height: 'calc(h)',
            x: '0',
            y: 'calc(-0.5*h)',
            fill: DARK_COLOR
        },
        portLabel: {
            pointerEvents: 'none',
            fontFamily: FONT_FAMILY,
            fontWeight: 400,
            fontSize: 13,
            fill: LIGHT_COLOR,
            textAnchor: 'start',
            textVerticalAnchor: 'middle',
            textWrap: {
                width: - PADDING_L - 2 * PADDING_S,
                maxLineCount: 1,
                ellipsis: true
            },
            x: PADDING_L + PADDING_S
        },

    },
    size: {
        width: DATABASE_ENTITY_PROPERTY_WIDTH,
        height: DATABASE_ENTITY_PROPERTY_HEIGHT
    },
    markup: [{
        tagName: 'rect',
        selector: 'portBody'
    }, {
        tagName: 'image',
        selector: 'portImage'
    }, {
        tagName: 'text',
        selector: 'portLabel',
    }, {
        tagName: 'g',
        selector: 'portRemoveButton',
        children: [{
            tagName: 'rect',
            selector: 'portRemoveButtonBody'
        }, {
            tagName: 'path',
            selector: 'portRemoveButtonIcon'
        }]
    }]
};

export const headerAttributes = {
    attrs: {
        root: {
            magnet: false
        },
        body: {
            width: 'calc(w)',
            height: 'calc(h)',
            fill: LIGHT_COLOR,
            strokeWidth: LINE_WIDTH / 2,
            stroke: SECONDARY_DARK_COLOR,
            rx: 3,
            ry: 3,
        },
        icon: {
            width: HEADER_ICON_SIZE,
            height: HEADER_ICON_SIZE,
            x: PADDING_L,
            y: (HEADER_HEIGHT - HEADER_ICON_SIZE) / 2,
            xlinkHref: 'https://d2slcw3kip6qmk.cloudfront.net/marketing/pages/chart/erd-symbols/Entity.PNG'
        },
        label: {
            transform: `translate(${PADDING_L + HEADER_ICON_SIZE + PADDING_L},${PADDING_L})`,
            fontFamily: FONT_FAMILY,
            fontWeight: 600,
            fontSize: 16,
            fill: DARK_COLOR,
            text: 'Label',
            textWrap: {
                width: - PADDING_L - HEADER_ICON_SIZE - PADDING_L - PADDING_L,
                maxLineCount: 1,
                ellipsis: true
            },
            textVerticalAnchor: 'top',
        },
        description: {
            transform: `translate(${PADDING_L + HEADER_ICON_SIZE + PADDING_L},${PADDING_L + 20})`,
            fontFamily: FONT_FAMILY,
            fontWeight: 400,
            fontSize: 13,
            lineHeight: 13,
            fill: SECONDARY_DARK_COLOR,
            textVerticalAnchor: 'top',
            text: 'Description',
            textWrap: {
                width: - PADDING_L - HEADER_ICON_SIZE - PADDING_L - PADDING_L,
                maxLineCount: 2,
                ellipsis: true
            }
        },
    },
    markup: [{
        tagName: 'rect',
        selector: 'body',
    }, {
        tagName: 'text',
        selector: 'label',
    }, {
        tagName: 'text',
        selector: 'description',
    }, {
        tagName: 'image',
        selector: 'icon',
    }, {
        tagName: 'g',
        selector: 'portAddButton',
        children: [{
            tagName: 'rect',
            selector: 'portAddButtonBody'
        }, {
            tagName: 'path',
            selector: 'portAddButtonIcon'
        }]
    }]
};
