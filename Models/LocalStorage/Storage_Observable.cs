using System;
using System.Collections.ObjectModel;

namespace Models.LocalStorage
{
    [Serializable]
    /// <summary>
    ///  Хранилище для форм (на будущее если надо будет что либо добавить)
    /// </summary>
    ///<typeparam name="T">Тип Form**</typeparam>
    public class Storage_Observable<T> : ObservableCollection<T>
    {
        public Storage_Observable():base()
        {

        }
    }
}
