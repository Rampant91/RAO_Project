using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;
using Collections;

namespace Collections
{
    public partial class ObservableCollectionWithItemPropertyChanged<T> : ObservableCollection<T>, IKey
        where T : class, IChanged
    {
        public int Id { get; set; }
        public ObservableCollectionWithItemPropertyChanged() : base()
        {

        }

        private void CopyFrom(IEnumerable<T> collection)
        {
            if (collection != null)
            {
                using (IEnumerator<T> enumerator = collection.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        Add(enumerator.Current);
                    }
                }
            }
        }

        [NotMapped]
        private bool _isChanged = true;
        public bool IsChanged
        {
            get => _isChanged;
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
