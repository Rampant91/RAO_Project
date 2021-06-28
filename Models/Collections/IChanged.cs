using System.ComponentModel;

namespace Collections
{
    public interface IChanged:INotifyPropertyChanged
    {
        bool IsChanged { get; set; }
    }
}
