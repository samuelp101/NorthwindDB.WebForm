<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CustomerOrderORM.aspx.cs" Inherits="CustomerOrderWebForm.CustomerOrderORM" Async="true" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Orders ORM</title>
    <meta http-equiv="Content-Type" content="text/html;charset=utf-8" />
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />
    <link rel="stylesheet" type="text/css" href="CustomerOrderORM.css" />
    <script type ="text/javascript" src ="CustomerOrderORM.js" defer="defer" ></script>
</head>
<body>
<form method="post" action="CustomerOrderORM.aspx">
    <div id="PageLayout">
      <div>
        <table id="tblFormHeader">
          <colgroup>
            <col style="width: 75px" />
            <col style="width: 275px;" />
            <col style="width: auto;"/>           
          </colgroup>
          <tr>
            <td><img src="images/Logo.png" alt="Logo" id="Logo" /></td>
            <td style="vertical-align: bottom; text-align:left;">
              <div id="FormTitle">Customers Orders ORM</div>
              <div id="dvUserName" runat="server"></div>
            </td>
            <td style="vertical-align:bottom">
              <table>
                <colgroup>
                  <col style="width:auto"/> 
                  <col style="width:100px;"/>
                  <col style="width:10px;"/>
                </colgroup>
                <tr>
                  <td>&nbsp;</td>
                  <th style="text-align:right" ><a href="#" id="lnkIQYExport">Excel Export</a></th>
                  <th></th>
                </tr>
              </table>
              <br /><br />
              <table>
                <colgroup>
                  <col style="width: auto" />
                  <col style="width: 100px;" />
                  <col style="width: 100px;" />
                  <col style="width: 65px;" />
                  <col style="width: 80px;" />
                  <col style="width: 25px;" />
                  <col style="width: 80px;" />
                  <col style="width: 50px;" />
                  <col style="width: 5px;" />
                </colgroup>
                <tr>
                  <td></td>
                  <td><input type="checkbox" id="chkHideDetails" runat="server" />Hide Details</td>
                  <td><input type="checkbox" id="chkDisplaySubTotals" runat="server" />Display Subtotal</td>
                  <td style="text-align: right">From&nbsp;</td>
                  <td><input type="text" maxlength="15" id="txtFromDate" runat="server" /></td>
                  <td style="text-align: center">To</td>
                  <td><input type="text" maxlength="15" id="txtToDate" runat="server" /></td>
                  <td style="text-align: right"><input type="button" id="cmdGo" value="Go" /></td>
                  <td></td>
                </tr>
              </table>
            </td>            
          </tr>
        </table>
      </div>

      <br />

      <div id="dvReportHeader">
        <table id="tblReportHeader">
          <colgroup>
            <col style="width:60px;" />
            <col style="width:70px;" />
            <col style="width:150px;" />
            <col style="width:100px;" />
            <col style="width:auto;" />
            <col style="width:75px;" />
            <col style="width:120px;" />
            <col style="width:75px;" />
            <col style="width:75px;" />
            <col style="width:17px;" />
          </colgroup>
          <thead>
            <tr id="Filters">
              <td style="text-align:center;"><input type="text" id="txtOrderID" runat="server"/></td>
              <td style="text-align:center;"><input type="text" id="txtCustomerID" runat="server"/></td>
              <td style="text-align:center;"><input type="text" id="txtCompanyName" runat="server"/></td>
              <td style="text-align:center;"><input type="text" id="txtCountry" runat="server" /></td>
              <td style="text-align:center;"><input type="text" id="txtSalesRep" runat="server" /></td>
              <td></td>
              <td style="text-align:center;"><input type="text" id="txtShipper" runat="server" /></td>
              <td></td>
              <td></td>
              <td></td>
            </tr>
            <tr id="ColumnHeader">
              <th><a href='#' id="OrderBy.OrderID" >OrderID</a></th>
              <th><a href='#' id="OrderBy.CustomerID" >CustomerID</a></th>
              <th><a href='#' id="OrderBy.CompanyName" >CompanyName</a></th>
              <th><a href='#' id="OrderBy.Country" >Country</a></th>
              <th><a href='#' id="OrderBy.SalesRep" >SalesRep</a></th>
              <th><a href='#' id="OrderBy.OrderDate" >OrderDate</a></th>
              <th><a href='#' id="OrderBy.Shipper" >Shipper</a></th>
              <th><a href='#' id="OrderBy.Freight" >Freight</a></th>
              <th><a href='#' id="OrderBy.OrderTotal" >OrderTotal</a></th>
              <th></th>
            </tr>
          </thead>
        </table>
      </div>

      <div id="dvReportBody">
        <table id="tblReportBody">
          <colgroup>
            <col style="width:60px;" />
            <col style="width:70px;" />
            <col style="width:150px;" />
            <col style="width:100px;" />
            <col style="width:auto;" />
            <col style="width:75px;" />
            <col style="width:120px;" />
            <col style="width:75px;" />
            <col style="width:75px;" />
          </colgroup>
          <thead>
            <tr>
              <th>OrderID</th>
              <th>CustomerID</th>
              <th>CompanyName</th>
              <th>Country</th>
              <th>SalesRep</th>
              <th>OrderDate</th>
              <th>Shipper</th>
              <th>Freight</th>
              <th>Total</th>
            </tr>
          </thead>
          <tbody id="LineItems">
            <lx:Literal id="litReportBody" runat="server" />
          </tbody>
        </table>
      </div>

      <div id="dvReportFooter">
        <table id="tblReportFooter">
          <colgroup>        
            <col style="width:auto;" />
            <col style="width:75px;" />
            <col style="width:75px;" />
            <col style="width:17px;" />
          </colgroup>
          <tbody>
            <lx:Literal id = "litReportFooter" runat="server" /> 
          </tbody>
        </table>
      </div>

      <br />

      <div id="dvErrorMessage" >
        <lx:Literal ID="ErrorMessage" runat="server" />
        <input type="hidden" id="hdnOrderBy" runat="server"/>
        <input type="hidden" id="hdnAscDesc" runat="server" />
      </div>

    </div>        
</form>
</body>
</html>
