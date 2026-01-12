using Client_App.Commands.AsyncCommands.Save;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form2;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands;

namespace Client_App.ViewModels.Forms.Forms2;
public class Form_20VM : BaseVM, INotifyPropertyChanged
{
    #region Storages

    private Reports? _reports;
    public Reports? Storages
    {
        get => _reports;
        set
        {
            if (_reports != value)
            {
                _reports = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region Storage

    private Report _storage;
    public Report Storage
    {
        get => _storage;
        set
        {
            if (_storage != value)
            {
                _storage = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region DBO

    private DBObservable _DBO;
    public DBObservable DBO
    {
        get => _DBO;
        set
        {
            if (_DBO != value)
            {
                _DBO = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region IsCanSaveReportEnabled

    private bool _isCanSaveReportEnabled;
    public bool IsCanSaveReportEnabled
    {
        get => _isCanSaveReportEnabled;
        set
        {
            if (_isCanSaveReportEnabled != value)
            {
                _isCanSaveReportEnabled = value;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region FormType

    public string FormType { get; set; } = "2.0";

    #endregion

    public string WindowHeader => $"Форма {Storage.FormNum_DB}: Титульный лист организации";

    #region Properties

    #region IsSeparateDivision

    private bool _isSeparateDivision = true;
    public bool IsSeparateDivision
    {
        get => _isSeparateDivision;
        set
        {
            if (_isSeparateDivision == value) return;
            _isSeparateDivision = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    #region Commands

    public ICommand SaveReport => new SaveReportAsyncCommand(this);             //  Сохранить отчет
    public ICommand ChangeReportOrder => new ChangeReportOrderAsyncCommand(this);       //  Поменять местами юр. лицо и обособленное подразделение
    public ICommand Reorganize => new ReorganizeReportAsyncCommand(this);        // Реорганизовать компанию (Добавить или убрать поля для обособленного подразделения)

    #endregion

    #region Constructor

    public Form_20VM() { }

    public Form_20VM(in DBObservable dbObservable)
    {
        Storage = new Report { FormNum_DB = "2.0" };

        var ty1 = (Form20)FormCreator.Create("2.0");
        ty1.NumberInOrder_DB = 1;
        var ty2 = (Form20)FormCreator.Create("2.0");
        ty2.NumberInOrder_DB = 2;
        Storage.Rows20.Add(ty1);
        Storage.Rows20.Add(ty2);
        DBO = dbObservable;
    }

    public Form_20VM(string formNum, in Report rep)
    {
        if (rep.FormNum_DB is "1.0" or "2.0")
        {
            Storage = rep;
        }

        FormType = formNum;
        StaticConfiguration.DBModel.SaveChanges();
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