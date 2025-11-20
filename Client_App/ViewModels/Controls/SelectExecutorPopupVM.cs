using Client_App.ViewModels.Forms;
using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Models.DTO;

namespace Client_App.ViewModels.Controls;

public class SelectExecutorPopupVM : INotifyPropertyChanged
{
    #region Constructor

    public SelectExecutorPopupVM(BaseFormVM formVM)
    {
        _formVM = formVM;
        _executorDataCollection = FormVM.Report.Reports.Report_Collection
            .Where(x => x.FormNum_DB == formVM.FormType)
            .Select(x => new ExecutorDataDTO() 
            { 
                Name = x.FIOexecutor_DB, Grade = x.GradeExecutor_DB, Phone = x.ExecPhone_DB, Email = x.ExecEmail_DB
            })
            .Distinct()
            .ToList();
        OpenPopupCommand = ReactiveCommand.Create(() =>
        {
            PopupIsOpen = !PopupIsOpen;
        });
    }

    #endregion

    #region Commands

    public ICommand OpenPopupCommand { get; }

    #endregion

    #region Properties

    #region PopupIsOpen

    private BaseFormVM _formVM;
    public BaseFormVM FormVM => _formVM;

    private bool _popupIsOpen = false;
    public bool PopupIsOpen
    {
        get => _popupIsOpen;
        set
        {
            _popupIsOpen = value;
            OnPropertyChanged();
        }
    }

    private List<ExecutorDataDTO> _executorDataCollection;
    public List<ExecutorDataDTO> ExecutorDataCollection => _executorDataCollection;

    #endregion

    #endregion

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}