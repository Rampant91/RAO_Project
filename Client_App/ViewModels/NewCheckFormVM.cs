using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels.Forms;
using Models.CheckForm;
using Models.DBRealization;
using Models.Forms.Form4;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        get
        {
            string title = $"Проверка_формы_{FormVM.Report.FormNum_DB}_";
            switch (FormVM.FormType[0])
            {
                case '1' or '2':
                    {
                        try
                        {
                            var master = FormVM.Reports?.Master_DB;
                            if (master?.RegNoRep != null)
                                title = title + $"{master.RegNoRep.Value}_";

                            if (master?.OkpoRep != null)
                                title = title + $"{master.OkpoRep.Value}_";
                        }
                        catch
                        {
                            // Rows10/20 могут отсутствовать
                        }
                        break;
                    }

                case '4':
                    {
                        var form40 = FormVM.Reports.Master_DB.Rows40[0];
                        title += $"{form40.CodeSubjectRF_DB}_";
                        title += $"{form40.SubjectRF_DB.Replace(" ", "_")}_";
                        break;
                    }

                case '5':
                    {
                        var form50 = FormVM.Reports.Master_DB.Rows50[0];
                        if (form50.Rosatom_DB)
                            title += $"ВИАЦ_Госкорпорации_Росатом_";
                        else if (form50.MinObr_DB)
                            title += $"ВИАЦ_Министерства_обороны_РФ";
                        else
                            title += $"{form50.ExecutiveAuthority_DB}";
                        break;
                    }
            }


            if (FormVM.FormType[0] == '1')
                title = title + $"{FormVM.Report.StartPeriod_DB}-{FormVM.Report.EndPeriod_DB}";
            else
                title = title + $"{FormVM.Report.Year_DB}";

            return title;
        }
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

    //Для формы 4.1
    #region RegNo

    private string _regNo;
    public string RegNo
    {
        get => _regNo;
        set
        {
            if (_regNo == value) return;
            _regNo = value;
            OnPropertyChanged();
        }
    }

    #endregion

    //Для формы 4.1
    #region Okpo

    private string _okpo;
    public string Okpo
    {
        get => _okpo;
        set
        {
            if (_okpo == value) return;
            _okpo = value;
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

    //Для формы 4.1
    #region DbValue

    private string _dbValue;
    public string? DbValue
    {
        get => _dbValue;
        set
        {
            if (_dbValue == value) return;
            _dbValue = value;
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


    #endregion

    #region PropertyChanged

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion
}