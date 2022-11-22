using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Forms.DataAccess;

public abstract class RamAccess : INotifyPropertyChanged
{
    #region INotifyPropertyChanged

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;

    #endregion
}