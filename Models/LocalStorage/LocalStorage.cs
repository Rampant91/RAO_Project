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

namespace Models.LocalStorage
{
    /// <summary>
    /// Надстройка под хранилище для реализации сохранения 
    /// </summary>
    ///<typeparam name="T">Тип Form**</typeparam>
    public class LocalStorage: INotifyPropertyChanged
    {
        Storage_Observable<Client_Model.Form> _Storage;
        /// <summary>
        /// Список форм (пересохраняет файл если прописан Path)
        /// </summary>
        public Storage_Observable<Client_Model.Form> Storage 
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

        string _Path;
        /// <summary>
        /// Путь до папки (пересохраняет файл если прописан Path)
        /// </summary>
        public string Path 
        {
            get
            {
                return _Path;
            } 
            set
            {
                if(value!=_Path)
                {
                    _Path = value;
                    if(!File.Exists(_Path))
                    {
                        try
                        {
                            var t = File.Create(_Path);
                            t.Close();
                        }
                        catch
                        {

                        }
                    }
                    NotifyPropertyChanged("Path");
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
        //Сделать
        public IEnumerable<Client_Model.Form> GetFilteredStorage
        {
            get
            {
                foreach (var item in Filters.CheckAndSort(Storage))
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Сохраняет формы в файл
        /// </summary>
        public Task SaveForms()
        {
            return new Task(() =>
            {
                if (Path != "")
                {
                     Serrialize();
                }
                else
                {
                }
            });
            
        }

        object locker = new object();
        /// <summary>
        /// Функция серриализует формы
        /// </summary>
        //Сделать
        void Serrialize()
        {
            lock (locker)
            {
                if (!File.Exists(_Path))
                {
                    File.Create(_Path);
                }
                Client_Model.Form[] lt = new Client_Model.Form[Storage.Count];
                Storage.CopyTo(lt, 0);
                XmlSerializer ser = new XmlSerializer(typeof(Client_Model.Form[]));
                StreamWriter wrt = new StreamWriter(Path);
                ser.Serialize(wrt, lt);
                wrt.Close();
            }
        }

        /// <summary>
        /// Загружает формы
        /// </summary>
        public Task LoadForms()
        {
            return new Task(() =>
            {
                if (Path != "")
                {
                    Deserrialize();
                }
                else
                {
                }
            });
        }

        /// <summary>
        /// Функция десерриализует формы 
        /// </summary>
        //Сделать
        void Deserrialize()
        {
            lock (locker)
            {
                if (File.Exists(Path))
                {
                    try
                    {
                        XmlSerializer ser = new XmlSerializer(typeof(Client_Model.Form[]));
                        StreamReader wrt = new StreamReader(Path);
                        var tl = (Client_Model.Form[])ser.Deserialize(wrt);
                        wrt.Close();
                        Storage.Clear();
                        for (int i = 0; i < tl.Length; i++)
                        {
                            Storage.Add(tl[i]);
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Функция инициализации
        /// </summary>
        //Сделать
        void Init()
        {
            if (!File.Exists(Path))
            {
                var t=File.Create(Path);
                t.Close();
            }
        }

        /// <summary>
        /// Конструктор (если прописан Path, то загружает данные из файлов)
        /// </summary>
        ///<param name="Path">Путь до файла</param>
        public LocalStorage(string Path)
        {
            _Path = Path;
            _Storage = new Storage_Observable<Client_Model.Form>();
            _Filters = new Filter.Filter<Client_Model.Form>();
            Init();
            Storage.CollectionChanged += StorageChanged;
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
            _Path = "";
            _Storage = new Storage_Observable<Client_Model.Form>();
            _Filters = new Filter.Filter<Client_Model.Form>();
            Storage.CollectionChanged += StorageChanged;
        }
    }
}
