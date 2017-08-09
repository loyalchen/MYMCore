using System;
using System.Linq;
using System.Reflection;

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

            var objAttrs = prop.GetCustomAttributes<T>(true).ToList();
            if (!objAttrs.Any()) {
                return false;
            }
            attr = objAttrs.First();

            return true;
        }
        /// <summary>
        /// Gets a value indicating whether the type of current property is nullable.
        /// </summary>
        /// <param name="propertyInfo">Current property</param>
        /// <exception cref="ArgumentNullException">The current property is null.</exception>
        /// <returns>Ture if the type of current property is nullable;Otherwise false.</returns>
        public static bool IsNullable(this PropertyInfo propertyInfo) {
            if (propertyInfo == null) {
                throw new ArgumentNullException(nameof(propertyInfo));
            }
            return propertyInfo.PropertyType.IsGenericType &&
                   propertyInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
        }
    }
}
