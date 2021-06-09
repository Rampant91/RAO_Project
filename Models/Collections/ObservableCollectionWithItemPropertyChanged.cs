using Models.DataAccess;
using Models.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;
using Models.Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Collections
{
    public partial class ObservableCollectionWithItemPropertyChanged<T> : ObservableCollection<T>
        where T : IChanged
    {
        public ObservableCollectionWithItemPropertyChanged() : base() { }

        public ObservableCollectionWithItemPropertyChanged(List<T> list)
            : base((list != null) ? new List<T>(list.Count) : list)
        {
            CopyFrom(list);
        }

        public ObservableCollectionWithItemPropertyChanged(IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            CopyFrom(collection);
        }

        private void CopyFrom(IEnumerable<T> collection)
        {
            IList<T> items = Items;
            if (collection != null && items != null)
            {
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        items.Add(enumerator.Current);
                    }
                }
            }
        }

        //public bool Equals(object obj)
        //{
        //    if (obj is ObservableCollectionWithItemPropertyChanged<T>)
        //    {
        //        var obj1 = this;
        //        var obj2 = obj as ObservableCollectionWithItemPropertyChanged<T>;

        //        if (obj1.Count == obj2.Count)
        //        {
        //            int count = 0;
        //            foreach (var item1 in obj1)
        //            {
        //                dynamic tmp1 = item1;
        //                dynamic tmp2 = obj2[count];
        //                if (tmp1!=tmp2)
        //                {
        //                    return false;
        //                }
        //                count++;
        //            }
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //    return true;
        //}

        //public static bool operator ==(ObservableCollectionWithItemPropertyChanged<T> obj1, ObservableCollectionWithItemPropertyChanged<T> obj2)
        //{
        //    return obj1.Equals(obj2);
        //}
        //public static bool operator !=(ObservableCollectionWithItemPropertyChanged<T> obj1, ObservableCollectionWithItemPropertyChanged<T> obj2)
        //{
        //    return !obj1.Equals(obj2);
        //}


        protected override void InsertItem(int index, T item)
        {
            if (item != null)
            {
                base.InsertItem(index, item);
                item.PropertyChanged += Item_PropertyChanged;
            }
        }

        protected override void RemoveItem(int index)
        {
            Items[index].PropertyChanged -= Item_PropertyChanged;
            base.RemoveItem(index);
        }

        protected virtual void MoveItem(int oldIndex, int newIndex)
        {
            T removedItem = this[oldIndex];
            base.RemoveItem(oldIndex);
            base.InsertItem(newIndex, removedItem);
        }

        protected override void ClearItems()
        {
            foreach (T item in Items)
            {
                item.PropertyChanged -= Item_PropertyChanged;
            }
            base.ClearItems();
        }

        protected override void SetItem(int index, T item)
        {
            T oldItem = Items[index];
            T newItem = item;
            oldItem.PropertyChanged -= Item_PropertyChanged;
            newItem.PropertyChanged += Item_PropertyChanged;
            base.SetItem(index, item);
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsChanged))
            {
                IsChanged = true;
                var arg = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                this.OnCollectionChanged(arg);

                var handler = ItemPropertyChanged;
                if (handler != null) { handler(sender, e); }
            }
        }

        [NotMapped]
        bool _isChanged = true;
        public bool IsChanged
        {
            get
            {
                return _isChanged;
            }
            set
            {
                if (_isChanged != value)
                {
                    _isChanged = value;
                }
            }
        }

        public event PropertyChangedEventHandler ItemPropertyChanged;
    }
}
