using Microsoft.Data.SqlClient;
using SharedLib;
using SharedLib.Data;
using SharedLib.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace CustomerOrderWebForm
{
    public partial class CustomerOrderRaw : Page
    {

        #region Properties    

        private String _QueryString = String.Empty;

        protected HtmlGenericControl dvUserName;
        private String _UserName = String.Empty;
        private String UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value ?? "";
                if (_UserName.IndexOf("\\") > 1)
                {
                    _UserName = _UserName.Substring(_UserName.IndexOf("\\") + 1);
                }
            }
        }

        protected HtmlInputText txtFromDate;
        private DateTime _FromDate;
        public String FromDate
        {
            get { return _FromDate.ToString("MM/dd/yyyy"); }
            set
            {
                if (!String.IsNullOrEmpty(value)) { DateTime.TryParse(value, out _FromDate); }
                if (_FromDate < DateTime.Now.AddYears(-100)) { _FromDate = DateTime.Today.AddYears(-1); }
            }
        }

        protected HtmlInputText txtToDate;
        private DateTime _ToDate;
        public String ToDate
        {
            get { return _ToDate.ToString("MM/dd/yyyy"); }
            set
            {
                if (!String.IsNullOrEmpty(value)) { DateTime.TryParse(value, out _ToDate); }
                if (_ToDate < DateTime.Now.AddYears(-100)) { _ToDate = DateTime.Today; }
            }
        }

        protected HtmlInputCheckBox chkDisplaySubTotals;
        private Boolean _DisplaySubTotals = true;
        public String DisplaySubTotals
        {
            get { return _DisplaySubTotals ? "1" : ""; }
            set { _DisplaySubTotals = ((value ?? "") == "1"); }
        }

        protected HtmlInputCheckBox chkHideDetails;
        private Boolean _HideDetails = true;
        public String HideDetails
        {
            get { return _HideDetails ? "1" : ""; }
            set { _HideDetails = ((value ?? "") == "1"); }
        }

        protected HtmlInputText txtOrderID;
        private Int32 _OrderID;
        public String OrderID
        {
            get
            {
                string tmp = "";
                if (_OrderID > 0)
                {
                    tmp = _OrderID.ToString();
                }
                return tmp;
            }
            set { Int32.TryParse(value, out _OrderID); }
        }

        protected HtmlInputText txtCustomerID;
        private String _CustomerID;
        public String CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = (value ?? "").TrySubString(5); }
        }

        protected HtmlInputText txtCompanyName;
        private String _CompanyName;
        public String CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = (value ?? "").TrySubString(50); }
        }

        protected HtmlInputText txtCountry;
        private String _Country;
        public String Country
        {
            get { return _Country; }
            set { _Country = (value ?? "").TrySubString(15); }
        }

        protected HtmlInputText txtSalesRep;
        private String _SalesRep;
        public String SalesRep
        {
            get { return _SalesRep; }
            set { _SalesRep = (value ?? "").TrySubString(50); }
        }

        protected HtmlInputText txtOrderDate;
        private String _OrderDate;
        public String OrderDate
        {
            get { return _OrderDate; }
            set { _OrderDate = (value ?? "").TrySubString(25); }
        }

        protected HtmlInputText txtShipper;
        private String _Shipper;
        public String Shipper
        {
            get { return _Shipper; }
            set { _Shipper = (value ?? "").TrySubString(50); }
        }


        protected HtmlInputHidden hdnOrderBy;
        private String _OrderBy = "";
        public String OrderBy
        {
            get { return _OrderBy; }
            set
            {
                _OrderBy = value ?? "";
            }
        }

        protected HtmlInputHidden hdnAscDesc;
        private Boolean _AscDesc;
        public string AscDesc
        {
            get { return _AscDesc ? "1" : "0"; }
            set
            {
                _AscDesc = ((value ?? "") == "1");
            }
        }

        #endregion

        #region Errors
        private List<Exception> Exceptions = new List<Exception>();
        protected Literal ErrorMessage;
        #endregion

        protected Literal litReportBody;
        protected Literal litReportFooter;

        public List<Row> Rows = new List<Row>();
        private Row GrandTotal = new Row();

        public CustomerOrderRaw()
        {
            this.Load += Page_Load;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            GetParm();
            litReportBody.OnRender += ReportBody_OnRender;
            litReportFooter.OnRender += ReportFooter_OnRender;
            Page.RegisterAsyncTask(new PageAsyncTask(GetData));
            ErrorMessage.OnRender += ErrorMessage_OnRender;
        }

        private void GetParm()
        {
            UserName = User.Identity.Name;

            FromDate = Request.QueryString["FromDate"];
            txtFromDate.Value = FromDate;

            ToDate = Request.QueryString["ToDate"];
            txtToDate.Value = ToDate;

            DisplaySubTotals = Request.QueryString["DisplaySubTotals"];
            chkDisplaySubTotals.Checked = _DisplaySubTotals;

            HideDetails = Request.QueryString["HideDetails"];
            chkHideDetails.Checked = _HideDetails;

            OrderBy = Request.QueryString["OrderBy"];
            hdnOrderBy.Value = OrderBy;

            AscDesc = Request.QueryString["AscDesc"];
            hdnAscDesc.Value = AscDesc;

            OrderID = Request.QueryString["OrderID"];
            txtOrderID.Value = OrderID;

            CustomerID = Request.QueryString["CustomerID"];
            txtCustomerID.Value = CustomerID;

            CompanyName = Request.QueryString["CompanyName"];
            txtCompanyName.Value = CompanyName;

            Country = Request.QueryString["Country"];
            txtCountry.Value = Country;

            SalesRep = Request.QueryString["SalesRep"];
            txtSalesRep.Value = SalesRep;

            Shipper = Request.QueryString["Shipper"];
            txtShipper.Value = Shipper;
        }

        public async Task GetData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(Util.NorthwindDB))
                {
                    using (SqlCommand cm = new SqlCommand("dbo.spCustomersOrders_Get", conn))
                    {
                        cm.CommandType = CommandType.StoredProcedure;
                        cm.Parameters.Add("FromDate", SqlDbType.VarChar).Value = FromDate;
                        cm.Parameters.Add("ToDate", SqlDbType.VarChar).Value = ToDate;
                        cm.Parameters.Add("OrderBy", SqlDbType.VarChar).Value = OrderBy;
                        cm.Parameters.Add("AscDesc", SqlDbType.VarChar).Value = AscDesc;

                        if (OrderID != "") { cm.Parameters.Add("OrderID", SqlDbType.VarChar).Value = OrderID; }
                        if (CustomerID != "") { cm.Parameters.Add("CustomerID", SqlDbType.VarChar).Value = CustomerID; }
                        if (CompanyName != "") { cm.Parameters.Add("CompanyName", SqlDbType.VarChar).Value = CompanyName; }
                        if (Country != "") { cm.Parameters.Add("Country", SqlDbType.VarChar).Value = Country; }
                        if (SalesRep != "") { cm.Parameters.Add("SalesRep", SqlDbType.VarChar).Value = SalesRep; }
                        if (Shipper != "") { cm.Parameters.Add("Shipper", SqlDbType.VarChar).Value = Shipper; }

                        await conn.OpenAsync();
                        using (SqlDataReader rdr = await cm.ExecuteReaderAsync())
                        {
                            if (rdr.HasRows)
                            {
                                while(rdr.Read())
                                {
                                    Row rw = new Row();

                                    rw.CustomerID = rdr["CustomerID"].ToString();
                                    rw.OrderID = rdr["OrderID"].ToString();
                                    rw.CompanyName = rdr["CompanyName"].ToString();
                                    rw.Country = rdr["Country"].ToString();
                                    rw.SalesRepID = rdr["SalesRepID"].ToString();
                                    rw.SalesRep = rdr["SalesRep"].ToString();
                                    rw.OrderDate = rdr["OrderDate"].ToString();
                                    rw.Shipper = rdr["Shipper"].ToString();
                                    rw.Freight = rdr["Freight"].ToString();
                                    rw.OrderTotal = rdr["OrderTotal"].ToString();

                                    SetOrderByValue(ref rw, OrderBy);
                                    Rows.Add(rw);
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
            }

        }

        public void SetOrderByValue(ref Row rw, string OrderBy)
        {
            switch (OrderBy)
            {
                case "CustomerID":
                    rw.OrderByValue = rw.CustomerID;
                    break;
                case "CompanyName":
                    rw.OrderByValue = rw.CompanyName;
                    break;
                case "Country":
                    rw.OrderByValue = rw.Country;
                    break;
                case "SalesRep":
                    rw.OrderByValue = rw.SalesRep;
                    break;
                case "Shipper":
                    rw.OrderByValue = rw.Shipper;
                    break;
            }
        }

        private void ReportBody_OnRender(HtmlTextWriter sw)
        {
            Row SubTotal = new Row();
            if (Rows.Count > 0) { SubTotal.OrderByValue = Rows[0].OrderByValue; }

            try
            {
                foreach (var rw in Rows)
                {
                    if (_DisplaySubTotals)
                    {
                        if (SubTotal.OrderByValue != rw.OrderByValue)
                        {
                            RenderSubTotal(sw, SubTotal);
                            SubTotal.Reset();
                        }
                    }

                    if ((!_DisplaySubTotals) || (!_HideDetails))
                    {
                        RenderRow(sw, rw);
                    }

                    SubTotal.Add(rw);
                    GrandTotal.Add(rw);
                }

                if (_DisplaySubTotals && (Rows.Count > 0))
                {
                    RenderSubTotal(sw, SubTotal);
                }
                GrandTotal.OrderByValue = "Total: ";

            }
            catch (Exception ex)
            {
                Exceptions.Add(ex);
            }
        }

        private void ReportFooter_OnRender(HtmlTextWriter sw)
        {
            sw.WriteLine("<tr class='rwFooter'>");
            sw.WriteLine("<th>{0}</th>", "Subtotal: ");
            sw.WriteLine("<th>{0}</th>", GrandTotal.Freight);
            sw.WriteLine("<th>{0}</th>", GrandTotal.OrderTotal);
            sw.WriteLine("<th></th>");
            sw.WriteLine("</tr>");
        }

        private void RenderRow(HtmlTextWriter sw, Row rw)
        {
            sw.WriteLine("<tr>");
            sw.WriteLine("<td>{0}</td>", rw.OrderID);
            sw.WriteLine("<td>{0}</td>", rw.CustomerID);
            sw.WriteLine("<td>{0}</td>", rw.CompanyName);
            sw.WriteLine("<td>{0}</td>", rw.Country);
            sw.WriteLine("<td>{0}</td>", rw.SalesRep);
            sw.WriteLine("<td>{0}</td>", rw.OrderDate);
            sw.WriteLine("<td>{0}</td>", rw.Shipper);
            sw.WriteLine("<td>{0}</td>", rw.Freight);
            sw.WriteLine("<td>{0}</td>", rw.OrderTotal);
            sw.WriteLine("</tr>");
        }

        private void RenderSubTotal(HtmlTextWriter sw, Row rw)
        {
            sw.WriteLine("<tr class='rwSubtotal'>");
            sw.WriteLine($"<th colspan='7'>Subtotal( {rw.OrderByValue} ): </th>");
            sw.WriteLine("<th>{0}</th>", rw.Freight);
            sw.WriteLine("<th>{0}</th>", rw.OrderTotal);
            sw.WriteLine("</tr>");
        }

        private void ErrorMessage_OnRender(HtmlTextWriter sw)
        {
            if (Exceptions.Count > 0)
            {
                foreach (Exception ex in Exceptions)
                {
                    sw.Write(ex.Message);
                }
                // Add error logging routines
            }
        }

        public class Row : CustomerOrder
        {
            public String OrderByValue = String.Empty;

            public Row() { }

            public void Add(Row rw)
            {
                OrderByValue = rw.OrderByValue;
                _OrderTotal += rw._OrderTotal;
                _Freight += rw._Freight;
            }

            public void Reset()
            {
                _OrderTotal = 0;
                _Freight = 0;
            }
        }

    }
}