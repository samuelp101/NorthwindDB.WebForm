using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharedLib.Data
{
    public static class MapSqlDataReader
    {
        // Map SqlDataReader current record's value to obj
        public static T MapDataReader<T>(this T obj, SqlDataReader rdr)
        {
            PropertyInfo[] props = obj.GetType().GetProperties();
            string OrderByValue = String.Empty;

            foreach (var prop in props)
            {
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    if (string.Compare(rdr.GetName(i), prop.Name, ignoreCase: true) == 0)
                    {
                        string propType = String.Empty;

                        if (prop.PropertyType.Name == "Nullable`1")
                        {
                            propType = Nullable.GetUnderlyingType(prop.PropertyType).Name;
                        }
                        else
                        {
                            propType = prop.PropertyType.Name;
                        }

                        switch (propType)
                        {
                            case "String":
                                prop.SetValue(obj, rdr.GetValue(i).ToString());
                                break;
                            case "Int16":
                                prop.SetValue(obj, rdr.IsDBNull(i) ? default(Int16) : (Int16)rdr.GetValue(i));
                                break;
                            case "Int32":
                                prop.SetValue(obj, rdr.IsDBNull(i) ? default(Int32) : (Int32)rdr.GetValue(i));
                                break;
                            case "Int64":
                                prop.SetValue(obj, rdr.IsDBNull(i) ? default(Int64) : (Int64)rdr.GetValue(i));
                                break;
                            case "Decimal":
                                prop.SetValue(obj, rdr.IsDBNull(i) ? default(Decimal) : (Decimal)rdr.GetValue(i));
                                break;
                            case "Boolean":
                                prop.SetValue(obj, rdr.IsDBNull(i) ? default(Boolean) : (Boolean)rdr.GetValue(i));
                                break;
                            case "Byte":
                                prop.SetValue(obj, rdr.IsDBNull(i) ? default(Byte) : (Byte)rdr.GetValue(i));
                                break;
                            case "DateTime":
                                prop.SetValue(obj, rdr.IsDBNull(i) ? default(DateTime) : (DateTime)rdr.GetValue(i));
                                break;
                        }
                    }                 
                }
            }
            return obj;
        }

        // Retrieve all SqlDataReader records into a collection
        public static List<T> SqlDataReaderToCollection<T>(this ICollection<T> objs, SqlDataReader rdr) where T : class
        {
            List<T> _objs = new List<T>();

            while (rdr.Read())
            {
                T myObj = (T)Activator.CreateInstance(typeof(T));
                _objs.Add(myObj.MapDataReader<T>(rdr));
            }
            return _objs;
        }
    }
}
