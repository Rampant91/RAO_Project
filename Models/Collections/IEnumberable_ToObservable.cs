using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Collections
{
    interface IEnumberable_ToObservable
    {
        ObservableCollection<T> ToObservable<T>();
    }
}
