using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace Models.Storage
{
    /// <summary>
    /// Надстройка под хранилище для реализации сохранения 
    /// </summary>
    ///<typeparam name="T">Тип Form**</typeparam>
    ///
    [Serializable]
    public class LocalStorage: INotifyPropertyChanged
    {
        Report_Observable _Storage;
        /// <summary>
        /// Список форм (пересохраняет файл если прописан Path)
        /// </summary>
        public Report_Observable Storage 
        {
            get
            {
                return _Storage;
            }
            set
            {
                if (value != _Storage)
                {
                    _Storage = value;
                    NotifyPropertyChanged("Storage");
                }
            }
        }

        Filter.Filter<Client_Model.Form> _Filters;
        /// <summary>
        /// Фильтры
        /// </summary>
        public Filter.Filter<Client_Model.Form> Filters 
        {
            get
            {
                return _Filters;
            }
            set
            {
                if (value.GetType() == _Filters.GetType())
                {
                    if (value != _Filters)
                    {
                        _Filters = value;
                        NotifyPropertyChanged("Filters");
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public LocalDictionary Forms { get; set; }

        /// <summary>
        /// Дает итератор с фильтрованными и сортированными формами
        /// </summary>
        public IEnumerable<Client_Model.Report> GetFilteredStorage
        {
            get
            {
                if (Storage.Count > 0)
                {
                    foreach (var item in Filters.CheckAndSort(Storage))
                    {
                        yield return item;
                    }
                }
            }
        }

        public void StorageChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("GetFilteredStorage");
            NotifyPropertyChanged("Storage");
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public LocalStorage()
        {
            _Storage = new Report_Observable();
            _Filters = new Filter.Filter<Client_Model.Form>();
            Storage.CollectionChanged += StorageChanged;
        }
    }
}
