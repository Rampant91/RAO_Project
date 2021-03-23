using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.IO;
using Models;
using Models.Client_Model;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System.Collections.Concurrent;
using System.Reflection;

namespace Models.Storage
{
    /// <summary>
    /// Хранилище всех форм 
    /// </summary>
    [Serializable]
    public class LocalDictionary : INotifyPropertyChanged
    {
        ObservableConcurrentDictionary<string, LocalStorage> _Forms;
        /// <summary>
        /// Список форм (пересохраняет файл если прописан Path)
        /// </summary>
        public ObservableConcurrentDictionary<string, LocalStorage> Forms
        {
            get
            {
                return _Forms;
            }
            set
            {
                if (value != _Forms)
                {
                    _Forms = value;
                    NotifyPropertyChanged("Forms");
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

        int _ChooseTab = 1;
        public int ChooseTab 
        {
            get
            {
                return _ChooseTab - 1;
            }
            set
            {
                _ChooseTab = value + 1;
                if (value+1 != _ChooseTab)
                {
                    _ChooseTab = value + 1;
                    NotifyPropertyChanged("ChooseTab");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Дает итератор с фильтрованными и сортированными формами
        /// </summary>
        public IEnumerable<Client_Model.Report> GetFilteredDictionary
        {
            get
            {
                var Storage = Forms[_ChooseTab.ToString()+"1"];
                if (Storage.Storage.Count > 0)
                {
                    var tp = Storage.Storage[0].GetType().Name.Replace("Form", "")[0];
                    foreach (var item in Storage.Filters.CheckAndSort(GetFullStorage(tp)))
                    {
                        yield return item;
                    }
                }
            }
        }

        /// <summary>
        /// Дает итератор со всеми формами определенного типа
        /// </summary>
        IEnumerable<Client_Model.Report> GetFullStorage(char TP)
        {
            foreach (var i in Forms)
            {
                if (i.Key[0] == TP)
                {
                    foreach (var item in i.Value.Storage)
                    {
                        yield return item;
                    }
                }
            }
        }

        //public List<List<Form>> ToList()
        //{
        //    List<List<Form>> lst = new List<List<Form>>();
        //    foreach(var item in Forms)
        //    {
        //        List<Form> lt = new List<Form>();
        //        foreach(var it in item.Value.Storage)
        //        {
        //            lt.Add(it);
        //        }
        //        lst.Add(lt);
        //    }
        //    return lst;
        //}

        public void FormsChanged(object sender,EventArgs e)
        {
            NotifyPropertyChanged("GetFilteredDictionary");
            NotifyPropertyChanged("Forms");
        }

        /// <summary>
        /// Конструктор 
        /// </summary>
        public LocalDictionary()
        {
            Forms = new ObservableConcurrentDictionary<string, LocalStorage>();

            Init_1();
            Init_2();
            Init_3();
            Init_4();
            Init_5();
        }

        void Init_1()
        {
            LocalStorage str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("10", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("11", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("12", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("13", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("14", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("15", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("16", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("17", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("18", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("19", str);
        }
        void Init_2()
        {
            LocalStorage str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("20", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("21", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("22", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("23", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("24", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("25", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("26", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("27", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("28", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("29", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("210", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("211", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("212", str);
        }
        void Init_3()
        {
            LocalStorage str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("30", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("31", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("32", str);
        }
        void Init_4()
        {
            LocalStorage str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("40", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("41", str);
        }
        void Init_5()
        {
            LocalStorage str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("50", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("51", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("52", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("53", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("54", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("55", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("56", str);
            str = new LocalStorage();
            str.Storage.CollectionChanged += FormsChanged;
            Forms.Add("57", str);
        }
    }
}
