using System.ComponentModel;

namespace Models.Interfaces;

public interface IKey : INotifyPropertyChanged, INumberInOrder, IExcel
{
    int Id { get; set; } 
}