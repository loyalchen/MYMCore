using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MYMCore.Extensions {
    public static class IEnumberableExtensions {
        public static string ToCSV<T>(this IEnumerable<T> source) where T : class {
            var sb = new StringBuilder();

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var csvInfo = props.Where(c => c.GetCustomAttribute<CSVMemberAttribute>() != null);

            var propertyInfos = csvInfo as PropertyInfo[] ?? csvInfo.ToArray();
            var columnNames = propertyInfos.Select(c => c.Name);

            sb.AppendLine(string.Join(",", columnNames)); //bulid header

            foreach (var item in source) {
                var fields = propertyInfos.Select(c => {
                    var o = c.GetValue(item);
                    var value = o?.ToString() ?? string.Empty;
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
            foreach (var prop in props) {
                FieldMappingAttribute attr;
                if (!prop.TryGetCustomAttribute(out attr)) {
                    continue;
                }
                var dc = tb.Columns.Add(string.IsNullOrEmpty(attr.FieldName) ? prop.Name : attr.FieldName, prop.PropertyType.GetCoreType());
                if (attr.OrderId != 0)
                    orderList.Add(new KeyValuePair<string, int>(dc.ColumnName, attr.OrderId));
                else
                    defaultList.Add(new KeyValuePair<string, int>(dc.ColumnName, dc.Ordinal));
            }

            foreach (var item in defaultList) {
                var ordinal = item.Value;

                KeyValuePair<string, int> temp;

                do {
                    temp = orderList.SingleOrDefault(c => c.Value == ordinal);

                    if (temp.Key != null) ordinal++;
                } while (temp.Key != null);

                finaltList.Add(new KeyValuePair<string, int>(item.Key, ordinal));
            }

            finaltList.AddRange(orderList);
            var count = 0;
            finaltList = finaltList.OrderBy(c => c.Value).Select(c => new KeyValuePair<string, int>(c.Key, count++)).ToList();

            foreach (var kvp in finaltList) {
                tb.Columns[kvp.Key].SetOrdinal(kvp.Value);
            }

            // set data to data table
            foreach (var item in collection) {
                var dr = tb.NewRow();

                foreach (var prop in props) {
                    FieldMappingAttribute attr;
                    if (!prop.TryGetCustomAttribute(out attr)) {
                        continue;
                    }
                    var fieldName = string.IsNullOrEmpty(attr.FieldName) ? prop.Name : attr.FieldName;
                    var value = prop.GetValue(item, null);

                    //We need to check whether the property is NULLABLE
                    if (prop.IsNullable())
                        dr[fieldName] = value ?? DBNull.Value;
                    else
                        dr[fieldName] = value;
                }

                tb.Rows.Add(dr);
            }

            return tb;
        }
    }
}
