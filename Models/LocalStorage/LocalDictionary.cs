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
                        Forms.Add(num.ToString(),new LocalStorage(_Path + "/Forms"+num.ToString()+".raodb"));
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
                        Forms.Add(num.ToString(), new LocalStorage());
                    }
                    catch { }
                }
            }

        }
    }
}
