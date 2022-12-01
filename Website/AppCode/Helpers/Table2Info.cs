using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace Website.AppCode
{
    public static class Table2Info
    {
        public static T ToObject<T>(this DataRow row) where T : new()
        {
            T item = new T();
            IList<PropertyInfo> properties = item.GetType().GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    if (row.Table.Columns.Contains(property.Name))
                    {
                        object val = row[property.Name];
                        if (val != DBNull.Value)
                        {
                            DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), val.ToString());
                            property.SetValue(item, day, null);
                        }

                    }
                }
                else
                {
                    if (row.Table.Columns.Contains(property.Name))
                    {
                        object val = row[property.Name];
                        if (val != DBNull.Value)
                            property.SetValue(item, val, null);
                    }
                }
            }
            return item;
        }
        public static List<T> ToList<T>(this DataTable table) where T : new()
        {
            IList<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            List<T> result = new List<T>();

            foreach (var row in table.Rows)
            {
                var item = CreateItemFromRow<T>((DataRow)row, properties);
                result.Add(item);
            }

            return result;
        }

        private static T CreateItemFromRow<T>(DataRow row, IList<PropertyInfo> properties) where T : new()
        {
            T item = new T();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(System.DayOfWeek))
                {
                    if (row.Table.Columns.Contains(property.Name))
                    {
                        object val = row[property.Name];
                        if (val != DBNull.Value)
                        {
                            DayOfWeek day = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), val.ToString());
                            property.SetValue(item, day, null);
                        }

                    }
                }
                else
                {
                    if (row.Table.Columns.Contains(property.Name))
                    {
                        object val = row[property.Name];
                        if (val != DBNull.Value)
                            property.SetValue(item, val, null);
                    }
                }
            }
            return item;
        }
    }
}