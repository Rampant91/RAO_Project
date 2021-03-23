using System;
using System.Collections.ObjectModel;

namespace Models.Client_Model
{
    [Serializable]
    /// <summary>
    ///  Хранилище для форм (на будущее если надо будет что либо добавить)
    /// </summary>
    ///<typeparam name="T">Тип Form**</typeparam>
    public class Row_Observable<T> : ObservableCollection<T>
    {
        public Row_Observable():base()
        {

        }
    }
}
