using System;
using System.ComponentModel;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace MYMCore.Extensions {
    public static class ObjectExtensions {
        public static T Clone<T>(this T source) where T : class, new() {
            if (!typeof(T).IsSerializable) {
                throw new ArgumentException("The type must be serializable.", nameof(source));
            }

            if (source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            T rs;
            using (var ms = new MemoryStream()) {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, source);
                ms.Position = 0;
                rs = (T)bf.Deserialize(ms);
            }
            return rs;
        }

        public static string GetPropertyName<TViewModel, TProperty>(this TViewModel source, Expression<Func<TViewModel, TProperty>> propertyExpression)
            where TViewModel : INotifyPropertyChanged {
            return GetPropertyName(propertyExpression);
        }


        public static string GetPropertyName<T>(this object source, Expression<Action<T>> propertyExpression) {
            return GetPropertyName(propertyExpression);
        }

        private static string GetPropertyName(LambdaExpression propertyExpression) {
            if (propertyExpression == null) {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null) {
                throw new ArgumentException("Not member access expression", nameof(propertyExpression));
            }

            var property = memberExpression.Member as PropertyInfo;
            if (property == null) {
                throw new ArgumentException("Expression not property", nameof(propertyExpression));
            }

            if (property.GetMethod.IsStatic) {
                throw new ArgumentException("Static expression", nameof(propertyExpression));
            }

            return property.Name;
        }
    }
}
