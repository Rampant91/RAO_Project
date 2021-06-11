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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using FirebirdSql.Data.FirebirdClient;

namespace Models.Collections
{
    public partial class ObservableCollectionWithItemPropertyChanged<T> :DbSet<T>,INotifyCollectionChanged
        where T : class,IChanged
    {
        public ObservableCollectionWithItemPropertyChanged() : base() { }

        public ObservableCollectionWithItemPropertyChanged(IEnumerable<T> collection):base()
        {
            if (collection == null)
                throw new ArgumentNullException("collection");
            CopyFrom(collection);
        }

        public override EntityEntry<T> Add(T obj)
        {
            if(CollectionChanged!=null)
            {
                CollectionChanged(obj,new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
            }
            return base.Add(obj);
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private void CopyFrom(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        this.Add(enumerator.Current);
                    }
                }
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
    }
}
