using System.ComponentModel;

namespace Collections
{
    public interface IKey:INotifyPropertyChanged
    {
        int Id { get; set; }
    }
}
