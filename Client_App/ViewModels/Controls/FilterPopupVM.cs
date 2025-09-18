using Client_App.ViewModels.Forms;
using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client_App.ViewModels.Controls;

public class FilterPopupVM : INotifyPropertyChanged
{
    #region Constructor

    public FilterPopupVM(BaseFormVM formVM)
    {
        _formVM = formVM;
    }

    #endregion

    #region Commands

    public ICommand OpenPopupCommand => ReactiveCommand.Create(() =>
    {
        PopupIsOpen = !PopupIsOpen;

    });

    #endregion

    #region Properties

    private BaseFormVM _formVM;
    public BaseFormVM FormVM => _formVM;

    private bool _popupIsOpen;
    public bool PopupIsOpen
    {
        get => _popupIsOpen;
        set
        {
            _popupIsOpen = value;
            OnPropertyChanged();
        }
    }

    public List<string> _valueCollection;
    public List<string> ValueCollection
    {
        get => _valueCollection;
        set
        {
            _valueCollection = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}