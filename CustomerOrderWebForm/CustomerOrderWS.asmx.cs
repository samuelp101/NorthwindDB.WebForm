using Microsoft.Data.SqlClient;
using SharedLib;
using SharedLib.Data;
using SharedLib.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Reflection;
using System.Web.Services;
using System.Xml.Serialization;

namespace CustomerOrderWebForm
{
    [WebService(Namespace = "http://IotZones.com/")]
    public class CustomerOrderWS : WebService
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [return: XmlElement(ElementName = "Response")]
        public Response Request(String _FromDate, String _ToDate, String _OrderBy, String _AscDesc, String _OrderID,
                                String _CustomerID, String _CompanyName, String _Country, String _SalesRep, String _Shipper)
        {
            Response rs = new Response();
            try
            {
                rs.UserName = User.Identity.Name;
                rs.FromDate = _FromDate;
                rs.ToDate = _ToDate;
                rs.OrderBy = _OrderBy;
                rs.AscDesc = _AscDesc;
                rs.OrderID = _OrderID;
                rs.CustomerID = _CustomerID;
                rs.CompanyName = _CompanyName;
                rs.Country = _Country;
                rs.SalesRep = _SalesRep;
                rs.Shipper = _Shipper;

                rs.GetData();
            }
            catch (Exception ex)
            {
                rs.Errors += ex.Message;
            }
            
            if (rs.Errors != "")
            {
                // Add error logging routine
            }
            return rs;
        }

        public class Response
        {
            #region Props

            private String _Errors = String.Empty;
            public String Errors
            {
                get { return _Errors; }
                set { _Errors += value; }
            }

            private string _UserName = String.Empty;
            public string UserName
            {
                get { return _UserName; }
                set
                {
                    _UserName = value ?? "";
                    _UserName = _UserName.Substring(_UserName.IndexOf(@"\") + 1);
                }
            }

            public string FromDate = String.Empty;
            public string ToDate = String.Empty;
            public string OrderBy = String.Empty;
            public string AscDesc = String.Empty;

            public string OrderID = String.Empty;
            public string CustomerID = String.Empty;
            public string CompanyName = String.Empty;
            public string Country = String.Empty;
            public string SalesRep = String.Empty;
            public string Shipper = String.Empty;

            #endregion

            public List<Row> Rows = new List<Row>();

            public Response() { }

            public void GetData()
            {
                try
                {
                    string connString = ConfigurationManager.ConnectionStrings["NorthwindDB"].ConnectionString;
                    using (SqlConnection cn = new SqlConnection(connString))
                    {
                        using (SqlCommand cm = new SqlCommand("dbo.spCustomersOrders_Get", cn))
                        {
                            cm.CommandType = CommandType.StoredProcedure;

                            if (FromDate != "") { cm.Parameters.Add("FromDate", SqlDbType.VarChar).Value = FromDate; }
                            if (ToDate != "") { cm.Parameters.Add("ToDate", SqlDbType.VarChar).Value = ToDate; }
                            if (OrderBy != "") { cm.Parameters.Add("OrderBy", SqlDbType.VarChar).Value = OrderBy; }
                            if (AscDesc != "") { cm.Parameters.Add("AscDesc", SqlDbType.VarChar).Value = AscDesc; }
                            if (OrderID != "") { cm.Parameters.Add("OrderID", SqlDbType.VarChar).Value = OrderID; }
                            if (CustomerID != "") { cm.Parameters.Add("CustomerID", SqlDbType.VarChar).Value = CustomerID; }
                            if (CompanyName != "") { cm.Parameters.Add("CompanyName", SqlDbType.VarChar).Value = CompanyName; }
                            if (Country != "") { cm.Parameters.Add("Country", SqlDbType.VarChar).Value = Country; }
                            if (SalesRep != "") { cm.Parameters.Add("SalesRep", SqlDbType.VarChar).Value = SalesRep; }
                            if (Shipper != "") { cm.Parameters.Add("Shipper", SqlDbType.VarChar).Value = Shipper; }

                            cn.Open();
                            using (SqlDataReader rdr = cm.ExecuteReader())
                            {
                                while (rdr.Read())
                                {
                                    Row rw = new Row();
                                    rw = rw.MapDataReader<Row>(rdr);
                                    SetOrderByValue(ref rw, OrderBy);
                                    Rows.Add(rw);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Errors += ex.Message + Environment.NewLine;
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

            public class Row : CustomerOrder, IProperties
            {
                public String OrderByValue = String.Empty;

                private static readonly PropertyInfo[] _Properties = typeof(CustomerOrder).GetProperties();

                PropertyInfo[] IProperties.Properties
                {
                    get { return _Properties; }
                }

                public PropertyInfo[] Properties => throw new NotImplementedException();

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
}
