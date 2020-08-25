using System;
using System.Data;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Collections.Generic;
using System.Reflection;
using System.Data.SqlClient;

namespace CustomerOrderWebForm
{
    public static class Util
    {
        public static readonly String NorthwindDB = ConfigurationManager.ConnectionStrings["NorthwindDB"].ConnectionString;

        public static void MapValues(this SqlDataReader rs, IProperties itm)
        {
            foreach (var prt in itm.Properties)
            {
                for (var ix = 0; ix < rs.FieldCount; ix++)
                {
                    if (string.Compare(rs.GetName(ix), prt.Name, ignoreCase: true) == 0)
                    {
                        prt.SetValue(itm, rs.GetValue(ix).ToString());
                        break;
                    }
                }
            }
        }      

    }
}
