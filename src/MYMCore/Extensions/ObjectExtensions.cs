using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MYMCore.Extensions {
    public static class ObjectExtensions {
        public static T Clone<T>(this T source) where T: class, new() {
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
    }
}
