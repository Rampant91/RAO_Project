using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collections
{
    public class ObservableCollectionWithItemPropertyChanged<T> : ObservableCollection<T>, IKey
        where T : class
    {
        public int Id { get; set; }


    }
}