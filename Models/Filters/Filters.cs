using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections;
using System.Collections.ObjectModel;


namespace Models.Filter
{
    /// <summary>
    ///  Хранилище для фильтра
    /// </summary>
    ///<typeparam name="T">Тип Form**</typeparam>
    public class Filters<T>:INotifyPropertyChanged
    {
        /// <summary>
        ///  Список фильтров
        /// </summary>
        public ObservableCollection<Filters_Item<T>> Filter_List { get; set; }

        bool CheckObject(T obj)
        {
            bool flag = true;
            foreach(var item in Filter_List)
            {
                if (!item.CheckObject(obj))
                {
                    flag = false;
                    break;
                }
            }
            return flag;
        }

        public IEnumerable<T> CheckAndSort(IEnumerable<T> Storage)
        {
            if (Storage.Count() > 0)
            {
                IEnumerable<T> str = Storage;

                if (SortPath != "")
                {
                    str = Storage.OrderByDescending(i =>
                    {
                        var prop = i.GetType().GetProperty(SortPath);
                        if (prop != null)
                        {
                            return prop.GetValue(i);
                        }
                        else
                        {
                            return null;
                        }
                    });
                }

                if (str != null)
                {
                    if (Filter_List.Count != 0)
                    {
                        foreach (var item in str)
                        {
                            if (CheckObject(item))
                            {
                                yield return item;
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in str)
                        {
                            yield return item;
                        }
                    }
                }
            }
            yield break;
        }

        /// <summary>
        ///  Имя поля по которому сортируется
        /// </summary>
        string _SortPath = "";
        public string SortPath 
        {
            get
            {
                return _SortPath;
            }
            set
            {
                if (_SortPath != value)
                {
                    _SortPath = value;
                    OnPropertyChanged(nameof(SortPath));
                }
            }
        }

        public Filters()
        {
            Filter_List = new ObservableCollection<Filters_Item<T>>();
            SortPath = "";
        }

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed
    }
}
