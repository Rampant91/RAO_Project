using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels.Forms;
using Models.CheckForm;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client_App.ViewModels;

public class NewCheckFormVM : BaseVM, INotifyPropertyChanged
{
    #region Constructure

    public NewCheckFormVM() { }

    public NewCheckFormVM(BaseFormVM formVM, List<CheckError> checkError)
    {
        FormVM = formVM;
        CheckError = checkError;

        ExcelExportCheckForm = new ExcelExportCheckFormAsyncCommand();
    }

    #endregion

    #region Commands

    public ICommand ExcelExportCheckForm { get; set; }            //  Создать и открыть новое окно формы для выбранной организации

    #endregion

    #region Properties

    public readonly BaseFormVM FormVM;

    private string _titleName;
    public string TitleName
    {
        get => $"Проверка_формы_{FormVM.Reports.Master_DB.RegNoRep.Value}_" +
               $"{FormVM.Reports.Master_DB.OkpoRep.Value}_" +
               $"{FormVM.Report.FormNum_DB}_" +
               ((FormVM.Report.FormNum_DB[..1] == "2") ? $"{FormVM.Report.Year_DB}" : $"{FormVM.Report.StartPeriod_DB}-{FormVM.Report.EndPeriod_DB}");
        set
        {
            if (_titleName == value) return;
            _titleName = value;
            OnPropertyChanged();
        }
    }

    private List<CheckError> _checkError;
    public List<CheckError> CheckError
    {
        get => _checkError;
        set
        {
            if (_checkError == value) return;
            _checkError = value;
            OnPropertyChanged();
        }
    }

    #region Column

    private string _column;
    public string Column
    {
        get => _column;
        set
        {
            if (_column == value) return;
            _column = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region FormNum

    private string _formNum;
    public string FormNum
    {
        get => _formNum;
        set
        {
            if (_formNum == value) return;
            _formNum = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Index

    private int _index;
    public int Index
    {
        get => _index;
        set
        {
            if (_index == value) return;
            _index = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Message

    private string _message;
    public string Message
    {
        get => _message;
        set
        {
            if (_message == value) return;
            _message = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Value

    private string _value;
    public string? Value
    {
        get => _value;
        set
        {
            if (_value == value) return;
            _value = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Row

    private string _row;
    public string Row
    {
        get => _row;
        set
        {
            if (_row == value) return;
            _row = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    #region PropertyChanged

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion
}