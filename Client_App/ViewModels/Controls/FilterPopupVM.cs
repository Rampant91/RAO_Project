using Client_App.ViewModels.Forms;
using Models.Collections;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Client_App.ViewModels.Controls
{
    public class FilterPopupVM : INotifyPropertyChanged
    {
        public FilterPopupVM(BaseFormVM formVM)
        {
            _formVM = formVM;

        }

        private BaseFormVM _formVM;
        public BaseFormVM FormVM
        {
            get { return _formVM; }
        }
        private bool _popupIsOpen = false;
        public bool PopupIsOpen
        {
            get { return _popupIsOpen; }
            set
            {
                _popupIsOpen = value;
                OnPropertyChanged();
            }
        }
        public List<string> _valueCollection;
        public List<string>  ValueCollection
        {
            get { return _valueCollection; }
            set 
            { 
                _valueCollection = value; 
                OnPropertyChanged(); 
            }
        }

        public ICommand OpenPopupCommand => ReactiveCommand.Create(() =>
        {
            PopupIsOpen = !PopupIsOpen;
        });

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
