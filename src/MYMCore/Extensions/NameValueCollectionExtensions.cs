using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace MYMCore.Extensions {
    public static class NameValueCollectionExtensions {
        public static string GetValue(this NameValueCollection collection, string name, string defaultValue = null) {
            return collection[name] ?? defaultValue;
        }

        public static int? GetInt(this NameValueCollection collection, string name) {
            var value = collection[name];
            if (value == null) {
                return null;
            }

            int integer;
            if (Int32.TryParse(value, out integer)) {
                return integer;
            }

            return null;
        }

        public static int GetInt(this NameValueCollection collection, string name, int defaultValue) {
            return GetInt(collection, name) ?? defaultValue;
        }

        public static bool? GetBool(this NameValueCollection collection, string name) {
            var value = collection[name];
            if (value == null) {
                return null;
            }

            bool boolean;
            if (bool.TryParse(value, out boolean)) {
                return boolean;
            }

            return null;
        }

        public static bool GetBool(this NameValueCollection collecion, string name, bool defaultValue) {
            return GetBool(collecion, name) ?? defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <param name="separators">Default separator is ','</param>
        /// <returns></returns>
        public static List<string> GetStringList(this NameValueCollection collection, string name, string defaultValue = null, char[] separators = null) {
            var value = collection[name];

            if (value == null && defaultValue == null) {
                return null;
            }

            if (value == null) {
                value = defaultValue;
            }

            if (separators == null) {
                separators = new char[] { ',' };
            }

            return value
                .Split(separators, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim())
                .ToList();
        }
    }
}
