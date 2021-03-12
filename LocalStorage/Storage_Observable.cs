using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;
using System.Threading;

namespace LocalStorage
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
