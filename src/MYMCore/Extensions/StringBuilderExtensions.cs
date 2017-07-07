using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Extensions {
    public static class StringBuilderExtensions {
        /// <summary>
        /// Appends a copy of the specified string to this instance with a delimiter.
        /// </summary>
        /// <param name="sb">An instance of <see cref="StringBuilder"/></param>
        /// <param name="value">The string to append</param>
        /// <param name="delimiter">Default delimiter "; "</param>
        /// <returns></returns>
        public static StringBuilder AppendDelimiter(this StringBuilder sb, string value, string delimiter = default(string)) {
            if (sb == null) {
                throw new ArgumentNullException(nameof(sb));
            }

            delimiter = delimiter == default(string) ? "; " : delimiter;

            if (sb.Length > 0) {
                sb.Append($"{delimiter}");
            }

            sb.Append(value);

            return new StringBuilder(sb.ToString());
        }
    }
}
