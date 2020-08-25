using System;
using System.Collections.Generic;
using System.Reflection;

namespace SharedLib
{
    public static class ConsoleOutput
    {
        public static void OutputObjectToConsole<T>(this T obj)
        {
            PropertyInfo[] props = obj.GetType().GetProperties();

            for (int i = 0; i < props.Length; i++)
            {
                string propName = props[i].Name;

                string propValue = "";
                if (props[i].GetValue(obj) != null)
                {
                    propValue = props[i].GetValue(obj).ToString();
                }

                Console.WriteLine($"{propName} : {propValue}");
            }
        }

        public static void OutputCollectionToConsole<T>(this ICollection<T> objs)
        {
            foreach (var obj in objs)
            {
                obj.OutputObjectToConsole<T>();
                Console.WriteLine("----------------------------------------");
            }
        }
    }
}
