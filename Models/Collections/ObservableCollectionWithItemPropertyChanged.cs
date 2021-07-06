using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collections
{
    public class ObservableCollectionWithItemPropertyChanged<T> : List<T>, IKey
        where T : class, IChanged
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