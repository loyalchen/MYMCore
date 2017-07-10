using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Extensions {
    public static class IEnumberableExtensions {
        public static string ToCSV<T>(this IEnumerable<T> source) where T : class {
            var sb = new StringBuilder();

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var csvInfo = props.Where(c => c.GetCustomAttribute<CSVMemberAttribute>() != null);

            var columnNames = csvInfo.Select(c => c.Name);

            sb.AppendLine(string.Join(",", columnNames)); //bulid header

            foreach (T item in source) {
                var fields = csvInfo.Select(c => {
                    var o = c.GetValue(item);
                    var value = o != null ? o.ToString() : string.Empty;
                    value = string.Concat("\"", value.Replace("\"", "\"\""), "\"");
                    return value;
                });
                sb.AppendLine(string.Join(",", fields));
            }

            return sb.ToString();
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> collection) {
            var tb = new DataTable(typeof(T).Name);

            var orderList = new List<KeyValuePair<string, int>>();
            var defaultList = new List<KeyValuePair<string, int>>();
            var finaltList = new List<KeyValuePair<string, int>>();

            // create data table schema by T
            var props = typeof(T).GetProperties();
            foreach (PropertyInfo prop in props) {
                FieldMappingAttribute attr;
                if (prop.TryGetCustomAttribute(out attr)) {
                    Type t = prop.PropertyType.GetCoreType();
                    DataColumn dc = tb.Columns.Add(string.IsNullOrEmpty(attr.FieldName) ? prop.Name : attr.FieldName, t);
                    if (attr.OrderId != 0)
                        orderList.Add(new KeyValuePair<string, int>(dc.ColumnName, attr.OrderId));
                    else
                        defaultList.Add(new KeyValuePair<string, int>(dc.ColumnName, dc.Ordinal));
                }
            }

            foreach (var item in defaultList) {
                int ordinal = item.Value;

                KeyValuePair<string, int> temp;

                do {
                    temp = orderList.SingleOrDefault(c => c.Value == ordinal);

                    if (temp.Key != null) ordinal++;
                } while (temp.Key != null);

                finaltList.Add(new KeyValuePair<string, int>(item.Key, ordinal));
            }

            finaltList.AddRange(orderList);
            int count = 0;
            finaltList = finaltList.OrderBy(c => c.Value).Select(c => new KeyValuePair<string, int>(c.Key, count++)).ToList();

            for (int i = 0; i < finaltList.Count; i++) {
                KeyValuePair<string, int> kvp = finaltList[i];
                tb.Columns[kvp.Key].SetOrdinal(kvp.Value);
            }

            // set data to data table
            foreach (T item in collection) {
                DataRow dr = tb.NewRow();

                foreach (PropertyInfo prop in props) {
                    FieldMappingAttribute attr;
                    if (prop.TryGetCustomAttribute(out attr)) {
                        string fieldName = string.IsNullOrEmpty(attr.FieldName) ? prop.Name : attr.FieldName;
                        object value = prop.GetValue(item, null);

                        //We need to check whether the property is NULLABLE
                        if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            dr[fieldName] = value == null ? DBNull.Value : value;
                        else
                            dr[fieldName] = value;
                    }
                }

                tb.Rows.Add(dr);
            }

            return tb;
        }
    }
}
