function HttpRequest() {
    let _TimedOut = false;
    let _CallBack = null;
    let xhr = new XMLHttpRequest();

    this.Execute = Execute;
    function Execute(sXMLNS, sURL, sMethod, sArguments, CallBack) {
        _CallBack = CallBack;

        let soap = '<?xml version="1.0" encoding="utf-8" ?>\n'
            + '<soap:Envelope xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">\n'
            + '<soap:Body>\n'
            + '<' + sMethod + ' xmlns="' + sXMLNS + '" >' + sArguments + '</' + sMethod + '>\n'
            + '</soap:Body>\n'
            + '</soap:Envelope>';

        xhr.open("POST", sURL, true);
        xhr.setRequestHeader("Content-Type", "text/xml");
        xhr.setRequestHeader("SOAPAction", sXMLNS + sMethod);
        xhr.onreadystatechange = xhr_onreadystatechange;
        xhr.timeout = 60000; //60 Seconds // 1 min
        xhr.ontimeout = xhr_ontimeout;
        xhr.send(soap);

    }

    function xhr_ontimeout() {
        _TimedOut = true;
        if (_CallBack) { _CallBack(null, "Timeout"); }
    }

    function xhr_onreadystatechange() {
        if (xhr.readyState != 4) return;
        if (_TimedOut) return;

        let Errors = "";
        let Faults = null;
        let ndResponse = null;

        let XMLDoc = xhr.responseXML;
        if (XMLDoc) {
            let nd = XMLDoc.documentElement;
            while (nd != null) {
                if (nd.nodeName == "faultstring") { Faults = nd; break; }
                else if (nd.nodeName == "soap:Fault") { Faults = nd; break; }
                if (nd.nodeName == "Response") { ndResponse = nd; break; }
                nd = nd.childNodes[0];
            }

            if (Faults) {
                let nvFaults = Faults.childNodes[0];
                if (nvFaults) { Errors = nvFaults.nodeValue; }
            }
            else if (ndResponse) {
                for (let ix = 0; ix < ndResponse.childNodes.length; ix++) {
                    let nd2 = ndResponse.childNodes[ix];
                    if (nd2.nodeName == "Errors") {
                        let nv2 = nd2.childNodes[0];
                        if (nv2) { Errors = nv2.nodeValue; }
                    }
                }
            }

            if (_CallBack) {
                _CallBack(ndResponse, Errors);
            }
            else {
                alert("No Callback Method");
            }
        }
    }
    return this;
}
