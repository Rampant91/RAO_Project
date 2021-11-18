using System.ComponentModel;
using Models.Collections;

namespace Models.Collections
{
    public interface IKey:INotifyPropertyChanged,IExcel
    {
        int Id { get; set; }
    }
}
