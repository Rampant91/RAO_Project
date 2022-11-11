using System.ComponentModel;

namespace Models.Collections;

public interface IKey:INotifyPropertyChanged,INumberInOrder,IExcel
{
    int Id { get; set; } 
}