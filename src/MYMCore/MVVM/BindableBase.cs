using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MYMCore.MVVM {
    public abstract class BindableBase : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of property used to notify listeners./// </param>
        protected void OnPropertyChanged(string propertyName) {
            var eventHandler = PropertyChanged;
            eventHandler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
