using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.Extensions {
    public static class PropertyExtensions {
        /// <summary>
        /// Get the specified attribute of a property.
        /// A return value indicates whether get specified attribute succeeded.
        /// </summary>
        /// <typeparam name="T">The type of attribute.</typeparam>
        /// <param name="prop">The property to get the specified attribute.</param>
        /// <param name="attr">The return attribute from the property. if exists multiple <see cref="T"/>, the first attribute will be returned.</param>
        /// <returns></returns>
        public static bool TryGetCustomAttribute<T>(this PropertyInfo prop, out T attr) where T : Attribute {
            if (prop == null) {
                throw new ArgumentNullException(nameof(prop));
            }
            attr = null;

            var objAttrs = prop.GetCustomAttributes<T>(true);
            if (objAttrs.Any()) {
                attr = objAttrs.First();

                return true;
            }
            return false;
        }
    }
}
