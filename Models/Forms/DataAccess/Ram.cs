using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.DataAccess
{
    public abstract class RamAccess: INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
