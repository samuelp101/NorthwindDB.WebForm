function GetNodeValueByName(itm, name) {
    let rnd = null;
    if (itm) {
        for (let ix = 0; ix < itm.childNodes.length; ix++) {
            let nd = itm.childNodes[ix];
            if (nd.nodeName == name) {
                rnd = nd;
                break;
            }
        }
    }
    return GetNodeValue(rnd);
}

function GetNodeByName(itm, name) {
    let rnd = null;
    if (itm) {
        for (let ix = 0; ix < itm.childNodes.length; ix++) {
            let nd = itm.childNodes[ix];
            if (nd.nodeName == name) {
                rnd = nd;
                break;
            }
        }
    }
    return rnd;
}

function GetNodeValue(nd) {
    let value = "";
    if (nd) {
        let nv = nd.childNodes[0];
        if (nv) { value = nv.nodeValue; }
    }
    return value;
}

if (!window.Node) {
    window.Node =
    {
        ELEMENT_NODE: 1,
        ATTRIBUTE_NODE: 2,
        TEXT_NODE: 3,
        CDATA_SECTION_NODE: 4,
        ENTITY_REFERENCE_NODE: 5,
        ENTITY_NODE: 6,
        PROCESSING_INSTRUCTION_NODE: 7,
        COMMENT_NODE: 8,
        DOCUMENT_NODE: 9,
        DOCUMENT_TYPE_NODE: 10,
        DOCUMENT_FRAGMENT_NODE: 11,
        NOTATION_NODE: 12
    };
}