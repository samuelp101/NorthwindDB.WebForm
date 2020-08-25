let PageMngr = null;

window.onload = window_onload;
function window_onload()
{
    try
    {
        PageMngr = new PageManager();
    }
    catch (ex)
    {
        alert(ex.description);
        window.status = ex.description;
    }
}

function PageManager()
{
    let frm = document.forms[0];
    let page = document.documentElement;

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
            return false;
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
        let url = BuildUrl();
        document.location.replace(url);
    }

    function BuildUrl()
    {
        let url = frm.action + "?";
        let elms = document.getElementsByTagName("INPUT");
        for (let ix = 0; ix < elms.length; ix++)
        {
            let elm = elms[ix];
            if (elm.type == "text" || elm.type == "hidden")
            {
                if (elm.id != "" && elm.value != "")
                {
                    let name1 = elm.id.replace("txt", "").replace("hdn", "");
                    let value1 = elm.value;
                    url += name1 + "=" + value1 + "&";
                }
            }
            else if (elm.type == "checkbox")
            {
                if (elm.checked)
                {
                    let name2 = elm.id.replace("chk", "");
                    let value2 = elm.checked ? "1" : "";
                    url += name2 + "=" + value2 + "&";
                }
            }
        }
        return url.substring(0, url.length - 1);
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