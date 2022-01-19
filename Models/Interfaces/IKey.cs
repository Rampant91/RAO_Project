using System.ComponentModel;
using Models.Collections;

namespace Models.Collections
{
    public interface IKey:INotifyPropertyChanged,INumberInOrder,IExcel
    {
        int Id { get; set; } 
    }
}
