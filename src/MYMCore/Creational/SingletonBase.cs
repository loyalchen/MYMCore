using System;
using System.Diagnostics;

namespace MYMCore.Creational {
    public class SingletonBase<T> where T : class {
        protected SingletonBase() { }
        private static readonly Lazy<T> _instance = new Lazy<T>(() => {
            var instance = (T)Activator.CreateInstance(typeof(T), true);
            var Initinalizable = instance as IInitializable;
            if (Initinalizable != null) {
                Initinalizable.Initialize();
            }

            return instance;
        });

        /// <summary>
        /// The singletion instance of <see cref="T"/>
        /// </summary>
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [DebuggerNonUserCode]
        public static T Current => _instance.Value;
    }
}
