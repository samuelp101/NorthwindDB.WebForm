let PageMngr = null;

window.onload = window_onload;
function window_onload()
{
    try
    {
        PageMngr = new PageManager();
        PageMngr.DisplayInit();
        PageMngr.GetData();
    }
    catch (ex)
    {
        alert(ex.description);
        window.status = ex.description;
    }
}

function PageManager()
{
    let txtFromDate = document.getElementById("txtFromDate");
    let txtToDate = document.getElementById("txtToDate");
    let chkDisplaySubTotals = document.getElementById("chkDisplaySubTotals");
    let chkHideDetails = document.getElementById("chkHideDetails");

    let FromDate;
    let ToDate;
    let OrderBy = "";
    let AscDesc = "0";
    let OrderID = "";
    let CustomerID = "";
    let CompanyName = "";
    let Country = "";
    let SalesRep = "";
    let Shipper = "";
    let DisplaySubtotal = "0";
    let HideDetails = "0";
    let TotalOrder = 0.0;
    let TotalFreight = 0.0;

    this.DisplayInit = DisplayInit;
    function DisplayInit()
    {
        let _today = new Date();
        ToDate = _today.getFullYear() + "-" + (_today.getMonth() + 1) + "-" + _today.getDate();
        if (txtToDate) { txtToDate.value = ToDate;}

        let _fromDate = new Date();
        _fromDate.setMonth(_fromDate.getMonth() - 12);
        FromDate = _fromDate.getFullYear() + "-" + (_fromDate.getMonth() + 1) + "-" + _fromDate.getDate();
        if (txtFromDate) { txtFromDate.value = FromDate;}

        DisplaySubtotal = "0";
        chkDisplaySubTotals.checked = false;

        HideDetails = "0";
        chkHideDetails.checked = false;
    }

    let http = new HttpRequest();
    this.GetData = GetData;
    function GetData()
    {
        TotalOrder = 0.0;
        TotalFreight = 0.0;

        let _arguments = "<_FromDate>" + FromDate + "</_FromDate>\n"
            + "<_ToDate>" + ToDate + "</_ToDate>\n"
            + "<_OrderBy>" + OrderBy + "</_OrderBy>\n"
            + "<_AscDesc>" + AscDesc + "</_AscDesc>\n"
            + "<_OrderID>" + OrderID + "</_OrderID>\n"
            + "<_CustomerID>" + CustomerID + "</_CustomerID>\n"
            + "<_CompanyName>" + CompanyName + "</_CompanyName>\n"
            + "<_Country>" + Country + "</_Country>\n"
            + "<_SalesRep>" + SalesRep + "</_SalesRep>\n"
            + "<_Shipper>" + Shipper + "</_Shipper>\n";
        http.Execute("http://IotZones.com/", "/CustomerOrderWS.asmx", "Request", _arguments, onResponse);
    }

    function onResponse(RspNode, Errors)
    {
        try
        {
            if (Errors) { alert(Errors); }
            else if (RspNode != null)
            {
                var OrderData = GetNodeByName(RspNode, "Rows");
                if (OrderData != null) { DisplayXmlData(OrderData); }
            }
        }
        catch (ex)
        {
            alert(ex.description, true);
        }
    }

    let ReportBody = document.getElementById("ReportBody");
    let ReportFooterBody = document.getElementById("ReportFooterBody");
    function DisplayXmlData(_orderData)
    {
        let _displayData = "";
        let _orderByValue = "";
        let _subTotalOrder = 0.0;
        let _subTotalFreight = 0.0;

        if (_orderData.childNodes.length > 0)
        {
            _orderByValue = GetNodeValueByName(_orderData.childNodes[0], "OrderByValue");
        }

        for (let ix = 0; ix < _orderData.childNodes.length; ix++)
        {
            var itm = _orderData.childNodes[ix];
            if (itm.nodeType == 1) 
            {
                if (OrderBy != "" && chkDisplaySubTotals.checked)
                {
                    if (_orderByValue == GetNodeValueByName(itm, "OrderByValue"))
                    {
                        _subTotalOrder += parseFloat(GetNodeValueByName(itm, "OrderTotal").replace(/,/g, ''));
                        _subTotalFreight += parseFloat(GetNodeValueByName(itm, "Freight").replace(/,/g, ''));
                    }
                    else
                    {
                        _displayData += "<tr class='rwSubtotal'><th colspan='7'>Subtotal: ( " + _orderByValue + " ): </th>"
                            + "<th>" + _subTotalFreight.toFixed(2) + "</th>"
                            + "<th>" + _subTotalOrder.toFixed(2) + "</th></tr>";

                        _subTotalOrder = parseFloat(GetNodeValueByName(itm, "OrderTotal").replace(/,/g, ''));
                        _subTotalFreight = parseFloat(GetNodeValueByName(itm, "Freight").replace(/,/g, ''));                        
                        _orderByValue = GetNodeValueByName(itm, "OrderByValue");
                    }
                }

                let _orderTotal = parseFloat(GetNodeValueByName(itm, "OrderTotal").replace(/,/g, ''));
                let _freight = parseFloat(GetNodeValueByName(itm, "Freight").replace(/,/g, ''));

                if ((!chkHideDetails.checked) || (!chkDisplaySubTotals.checked))
                {
                    _displayData += "<tr><td>" + GetNodeValueByName(itm, "OrderID") + "</td>"
                        + "<td>" + GetNodeValueByName(itm, "CustomerID") + "</td>"
                        + "<td>" + GetNodeValueByName(itm, "CompanyName") + "</td>"
                        + "<td>" + GetNodeValueByName(itm, "Country") + "</td>"
                        + "<td>" + GetNodeValueByName(itm, "SalesRep") + "</td>"
                        + "<td>" + GetNodeValueByName(itm, "OrderDate") + "</td>"
                        + "<td>" + GetNodeValueByName(itm, "Shipper") + "</td>"
                        + "<td>" + _freight.toFixed(2) + "</td>"
                        + "<td>" + _orderTotal.toFixed(2) + "</td></tr>";
                }                

                TotalOrder += _orderTotal;
                TotalFreight += _freight;
                
            }
        }

        if (OrderBy != "" && chkDisplaySubTotals.checked)
        {
            _displayData += "<tr class='rwSubtotal'><th colspan='7'>Subtotal: ( " + _orderByValue + " ): </th>"
                + "<th>" + _subTotalFreight.toFixed(2) + "</th>"
                + "<th>" + _subTotalOrder.toFixed(2) + "</th></tr>";
        }

        if (ReportBody)
        {
            ReportBody.innerHTML = _displayData;
        }

        if (ReportFooterBody)
        {
            let _footerData = "<tr class='rwFooter'><th>Total :</th>"
                + "<th>" + TotalFreight.toFixed(2) + "</th>"
                + "<th>" + TotalOrder.toFixed(2) + "</th></tr>";

            ReportFooterBody.innerHTML = _footerData;
        }

    }

    let cmdGo = document.getElementById("cmdGo");
    if (cmdGo) { cmdGo.onclick = cmdGo_onclick; }
    function cmdGo_onclick()
    {
        SubmitForm();
    }

    let hdnOrderBy = document.getElementById("hdnOrderBy");
    let hdnAscDesc = document.getElementById("hdnAscDesc");

    let tblFormHeader = document.getElementById("tblFormHeader");
    if (tblFormHeader) { tblFormHeader.onclick = tblFormHeader_onclick; }
    function tblFormHeader_onclick(e)
    {
        let evt = e || window.event;
        let elm = evt.srcElement || evt.target;
        if ((elm.id == "chkDisplaySubTotals") || (elm.id == "chkHideDetails"))
        {
            SubmitForm();
        }
    }

    let tblHeader = document.getElementById("tblReportHeader");
    if (tblHeader) { tblHeader.onclick = tblHeader_onclick; }
    function tblHeader_onclick(e)
    {
        let evt = e || window.event;
        let elm = evt.srcElement || evt.target;
        if (elm.tagName == "A")
        {
            let OrderBy = elm.id.replace("OrderBy.", "");
            if (hdnOrderBy.value == OrderBy)
            {
                hdnAscDesc.value = (hdnAscDesc.value == "1") ? "" : "1";
            }
            else
            {
                hdnAscDesc.value = "";
                hdnOrderBy.value = OrderBy;
            }
            SubmitForm();
            return false;
        }
        return true;
    }

    let Filters = document.getElementById("Filters");
    if (Filters) { Filters.onkeypress = txt_keypress; }
    function txt_keypress(e)
    {
        let evt = e || window.event;
        let kc = evt.keyCode || evt.which;
        if (kc == 13)
        {
            SubmitForm();
            return false;
        }
        return true;
    }

    function SubmitForm()
    {
        let elms = document.getElementsByTagName("INPUT");
        for (let ix = 0; ix < elms.length; ix++)
        {
            let elm = elms[ix];
            if (elm.type == "text")
            {
                switch (elm.id)
                {
                    case "txtFromDate":
                        FromDate = elm.value;
                        break;
                    case "txtToDate":
                        ToDate = elm.value;
                        break;
                    case "txtOrderID":
                        OrderID = elm.value;
                        break;
                    case "txtCustomerID":
                        CustomerID = elm.value;
                        break;
                    case "txtCompanyName":
                        CompanyName = elm.value;
                        break;
                    case "txtCountry":
                        Country = elm.value;
                        break;
                    case "txtSalesRep":
                        SalesRep = elm.value;
                        break;
                    case "txtShipper":
                        Shipper = elm.value;
                        break;
                }
            }
            else if (elm.type == "hidden")
            {
                switch (elm.id)
                {
                    case "hdnOrderBy":
                        OrderBy = elm.value;
                        break;
                    case "hdnAscDesc":
                        AscDesc = elm.value;
                        break;
                }
            }
            else if (elm.type == "checkbox")
            {
                switch (elm.id)
                {
                    case "chkDisplaySubTotals":
                        DisplaySubtotal = elm.checked ? "1" : "";
                        break;
                    case "chkHideDetails":
                        HideDetails = elm.checked ? "1" : "";
                        break;
                }
            }
        }

        GetData();

    }

    let lnkIQYExport = document.getElementById("lnkIQYExport");
    if (lnkIQYExport) { lnkIQYExport.onclick = lnkIQYExport_onclick; }
    function lnkIQYExport_onclick(e)
    {
        let url = BuildUrl();
        url = url.replace("aspx", "iqy");
        document.location.replace(url);

        return false;

    }

    return this;
}