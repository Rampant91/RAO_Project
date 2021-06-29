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
    public partial class ObservableCollectionWithItemPropertyChanged<T> : List<T>, IKey
        where T : class, IChanged
    {
        public int Id { get; set; }
        public ObservableCollectionWithItemPropertyChanged() : base()
        {

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
