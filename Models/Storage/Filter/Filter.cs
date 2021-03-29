using System;
using System.Collections.Generic;
using System.Linq;
using Models;

namespace Models.Storage.Filter
{
    /// <summary>
    ///  Хранилище для фильтра
    /// </summary>
    ///<typeparam name="T">Тип Form**</typeparam>
    public class Filter<T>
    {
        /// <summary>
        ///  Список фильтров
        /// </summary>
        public List<Filter_Item<T>> Filter_List { get; set; }

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
        public string SortPath { get; set; }

        public Filter()
        {
            Filter_List = new List<Filter_Item<T>>();
            SortPath = "";
        }
    }
}
