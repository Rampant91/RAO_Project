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

namespace Models.LocalStorage
{
    /// <summary>
    /// Хранилище всех форм 
    /// </summary>
    public class LocalDictionary : INotifyPropertyChanged
    {
        public ObservableConcurrentDictionary<string,LocalStorage> Forms { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        string _Path;
        /// <summary>
        /// Путь до папки (изменяет пути до файлов коллекций форм, а значит пересохраняет данные, если не "")
        /// </summary>
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                if (value != "")
                {
                    if ((value) != _Path)
                    {
                        _Path = value;
                        Directory.CreateDirectory(_Path);
                        foreach(var item in Forms)
                        {
                            item.Value.Path = _Path + "/Forms" + item.Key + ".raodb";
                        }

                        NotifyPropertyChanged("Path");
                    }
                }
                else
                {
                    _Path = "";
                    foreach (var item in Forms)
                    {
                        item.Value.Path = "";
                    }
                }
            }
        }

        object locker = new object();
        public async Task SaveForms()
        {
            lock (locker)
            {
                List<Task> lst = new List<Task>();
                foreach (var item in Forms)
                {
                    lst.Add(item.Value.SaveForms());
                }

                foreach (var item in lst)
                {
                    item.Start();
                }
                foreach (var item in lst)
                {
                    item.Wait();
                }
            }
        }
        public async Task LoadForms()
        {
            lock (locker)
            {
                List<Task> lst = new List<Task>();
                foreach (var item in Forms)
                {
                    lst.Add(item.Value.LoadForms());
                }

                foreach (var item in lst)
                {
                    item.Start();
                }
                foreach (var item in lst)
                {
                    item.Wait();
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
                _ChooseTab = value+1;
            }
        }

        /// <summary>
        /// Дает итератор с фильтрованными и сортированными формами
        /// </summary>
        public IEnumerable<Client_Model.Form> GetFilteredDictionary
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
        /// Дает итератор с фильтрованными и сортированными формами
        /// </summary>
        IEnumerable<Client_Model.Form> GetFullStorage(char TP)
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

        public List<List<Form>> ToList()
        {
            List<List<Form>> lst = new List<List<Form>>();
            foreach(var item in Forms)
            {
                List<Form> lt = new List<Form>();
                foreach(var it in item.Value.Storage)
                {
                    lt.Add(it);
                }
                lst.Add(lt);
            }
            return lst;
        }

        public void FormsChanged(object sender,EventArgs e)
        {
            NotifyPropertyChanged("GetFilteredDictionary");
        }

        /// <summary>
        /// Конструктор (если прописан Path, то загружает данные из файлов)
        /// </summary>
        ///<param name="Path">Путь до папки</param>
        public LocalDictionary(string Path)
        {
            _Path = Path;
            _Path += "/Forms";
            Forms = new ObservableConcurrentDictionary<string, LocalStorage>();
            Directory.CreateDirectory(_Path);
            var _classes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var item in _classes)
            {
                var name = item.Name;
                if(name.Contains("Form"))
                {
                    try
                    {
                        int num = Convert.ToInt32(name.Replace("Form",""));
                        var lc = new LocalStorage(_Path + "/Forms" + num.ToString() + ".raodb");
                        lc.Forms = this;
                        lc.Storage.CollectionChanged += FormsChanged;
                        Forms.Add(num.ToString(),lc);
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Конструктор 
        /// </summary>
        public LocalDictionary()
        {
            _Path = "";
            Forms = new ObservableConcurrentDictionary<string, LocalStorage>();
            var _classes = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var item in _classes)
            {
                var name = item.Name;
                if (name.Contains("Form"))
                {
                    try
                    {
                        int num = Convert.ToInt32(name.Replace("Form", ""));
                        var lc = new LocalStorage();
                        lc.Forms = this;
                        lc.Storage.CollectionChanged += FormsChanged;
                        Forms.Add(num.ToString(), lc);
                    }
                    catch { }
                }
            }

        }
    }
}
