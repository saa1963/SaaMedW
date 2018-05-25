using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace SaaMedW
{
    [Notify]
    public class Invoice1 : INotifyPropertyChanged
    {
        public string Name { get { return name; } set { SetProperty(ref name, value, namePropertyChangedEventArgs); } }

        #region NotifyPropertyChangedGenerator

        public event PropertyChangedEventHandler PropertyChanged;

        private string name;
        private static readonly PropertyChangedEventArgs namePropertyChangedEventArgs = new PropertyChangedEventArgs(nameof(Name));

        private void SetProperty<T>(ref T field, T value, PropertyChangedEventArgs ev)
        {
            if (!System.Collections.Generic.EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, ev);
            }
        }

        #endregion
    }
}
