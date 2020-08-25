using System;

namespace SharedLib.Models
{
    public class CustomerOrder
    {
        private Int32 _OrderID;
        public String OrderID 
        { 
            get { return _OrderID.ToString("#"); }
            set { Int32.TryParse(value, out _OrderID); } 
        }

        private String _CustomerID = String.Empty;
        public string CustomerID 
        { 
            get { return _CustomerID; }
            set { _CustomerID = value.TrySubString(5); }
        }

        private String _CompanyName = String.Empty;
        public String CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value.TrySubString(50); }
        }

        private String _Country = String.Empty;
        public String Country
        {
            get { return _Country; }
            set { _Country = value.TrySubString(15); }
        }

        private Int32 _SalesRepID;
        public String SalesRepID
        {
            get { return _SalesRepID.ToString("#"); }
            set { Int32.TryParse(value, out _SalesRepID); }
        }

        private String _SalesRep = String.Empty;
        public String SalesRep
        {
            get { return _SalesRep; }
            set { _SalesRep = value.TrySubString(50); }
        }

        public DateTime _OrderDate = DateTime.MinValue;
        public String OrderDate
        {
            get { return _OrderDate.Ticks > 0 ? _OrderDate.ToString("MM/dd/yy") : ""; }
            set { DateTime.TryParse(value, out _OrderDate); }
        }

        private String _Shipper = String.Empty;
        public String Shipper
        {
            get { return _Shipper; }
            set { _Shipper = value.TrySubString(50); }
        }

        public Decimal _Freight;
        public String Freight
        {
            get { return _Freight.ToString("#,##0.00"); }
            set { Decimal.TryParse(value, out _Freight); }
        }

        public Decimal _OrderTotal;
        public String OrderTotal
        {
            get { return _OrderTotal.ToString("#,##0.00"); }
            set { Decimal.TryParse(value, out _OrderTotal); }
        }
       
    }
}
