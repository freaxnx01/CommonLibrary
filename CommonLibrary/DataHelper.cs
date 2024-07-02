using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;

namespace Library
{
    public static class DataHelper
    {
        public static DataTable ConvertObjectToDataTable(object o)
        {
            if (o == null)
            {
                return null;
            }

            DataTable table = new DataTable(o.GetType().Name);

            PropertyInfo[] props = o.GetType().GetProperties();

            foreach (PropertyInfo pi in props)
            {
                table.Columns.Add(pi.Name, pi.PropertyType);
            }

            DataRow row = table.NewRow();

            foreach (PropertyInfo pi in props)
            {
                if (pi.GetValue(o, null) != null)
                {
                    row[pi.Name] = pi.GetValue(o, null);
                }
            }

            table.Rows.Add(row);

            return table;
        }
    }
}
