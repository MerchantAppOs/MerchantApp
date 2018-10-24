using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Merchant.Tools
{
    [Serializable]
    public class ObservableObject : INotifyPropertyChanged
    {
        [NonSerialized]
        private  PropertyChangedEventHandler _PropertyChanged;

           
        public virtual event PropertyChangedEventHandler PropertyChanged
        {
            add { _PropertyChanged += value; }
            remove { _PropertyChanged -= value; }
        }
        //protected virtual void SetAndNotify<T>(ref T field, T value, Expression<Func<T>> property)
        //{
        //    if (!object.ReferenceEquals(field, value))
        //    {
        //        field = value;
        //        this.OnPropertyChanged(property);
        //    }
        //}

        protected virtual void OnPropertyChanged([CallerMemberName]string prob="")
        {
            _PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(prob));
        }
    }
}
