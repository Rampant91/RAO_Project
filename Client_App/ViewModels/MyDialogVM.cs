using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels
{
    public class MyDialogVM : BaseVM, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public MyDialogVM()
        {

        }
    }
}
