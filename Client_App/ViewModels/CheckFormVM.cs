using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Models.CheckForm;

namespace Client_App.ViewModels;

public class CheckFormVM : BaseVM, INotifyPropertyChanged 
{
    #region Constructure

    public CheckFormVM(){}

    public CheckFormVM(ChangeOrCreateVM changeOrCreateVM, List<CheckError> checkError)
    {
        ChangeOrCreateVM = changeOrCreateVM;
        CheckError = checkError;
    }

    #endregion

    #region Properties

    public readonly ChangeOrCreateVM ChangeOrCreateVM;

    private string _titleName;
    public string TitleName
    {
        get => $"Проверка формы {ChangeOrCreateVM.Storage.FormNum_DB}";
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