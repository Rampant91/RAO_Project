using System;
using System.Collections.ObjectModel;
using Models.Client_Model;

namespace Models.Storage
{
    [Serializable]
    /// <summary>
    ///  Хранилище для форм (на будущее если надо будет что либо добавить)
    /// </summary>
    ///<typeparam name="T">Тип Form**</typeparam>
    public class Report_Observable : ObservableCollection<Report>
    {
        public Report_Observable():base()
        {

        }
    }
}
