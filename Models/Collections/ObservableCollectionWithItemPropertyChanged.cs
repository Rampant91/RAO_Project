using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collections
{
    public class ObservableCollectionWithItemPropertyChanged<T> : ObservableCollection<T>, IKey
        where T : class
    {
        [NotMapped] private bool _isChanged = true;

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                if (_isChanged != value) _isChanged = value;
            }
        }

        public int Id { get; set; }
    }
}