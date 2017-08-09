using System;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace MYMCore.Behavioral.Compare {
    public class ObjCompare {
        /// <summary>
        /// Get the Base64String of specified object with specified <see cref="BindingFlags"/> properties and fileds.
        /// The properties and fields need the <seealso cref="ComparePropertyAttribute"/> to be applied.
        /// </summary>
        /// <param name="obj">The object to be convert to Base64String.</param>
        /// <param name="bindingFlags">The flags for properties and fields.</param>
        /// <returns>An Base64String convert form specified object.</returns>
        public static string AnalyseOriginalVersion(object obj, BindingFlags bindingFlags) {
            var comparedValues = new StringBuilder();
            var props = obj.GetType().GetProperties(bindingFlags);
            var fields = obj.GetType().GetFields(bindingFlags);

            foreach (var item in props) {
                var propertyValue = item.GetValue(obj);
                if (propertyValue == null) {
                    continue;
                }
                if (!item.GetCustomAttributes<ComparePropertyAttribute>(true).Any()) {
                    continue;
                }
                if (item.PropertyType.IsPrimitive || item.PropertyType.IsValueType || item.PropertyType == typeof(string) || item.PropertyType == typeof(DateTime)) {
                    comparedValues.Append(propertyValue);
                }
                else {
                    comparedValues.Append(AnalyseOriginalVersion(propertyValue, bindingFlags));
                }
            }

            foreach (var item in fields) {
                var fieldValue = item.GetValue(obj);
                if (fieldValue == null) {
                    continue;
                }
                if (!item.GetCustomAttributes<ComparePropertyAttribute>(true).Any()) {
                    continue;
                }
                if (item.FieldType.IsPrimitive || item.FieldType == typeof(string) || item.FieldType == typeof(DateTime)) {
                    comparedValues.Append(fieldValue);
                }
                else {
                    comparedValues.Append(AnalyseOriginalVersion(fieldValue, bindingFlags));
                }
            }

            return Convert.ToBase64String(MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(comparedValues.ToString())));
        }
    }
}
