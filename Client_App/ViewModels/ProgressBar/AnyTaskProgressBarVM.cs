using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.Views.ProgressBar;
using Models.DBRealization;
using ReactiveUI;

namespace Client_App.ViewModels.ProgressBar;

public class AnyTaskProgressBarVM : BaseVM, INotifyPropertyChanged
{
    private IBackgroundLoader _backgroundWorker;
    private Task MainTask { get; set; }
    private CancellationTokenSource CancellationTokenSource { get; set; }
    private AnyTaskProgressBar AnyTaskProgressBar_DB { get; set; }

    public Interaction<MainWindowVM, object> ShowDialog { get; }

    public bool IsShowDialog { get; set; }
    public event EventHandler ClosingRequest;
    public ICommand ExcelExportCancel { get; set; } //Отмена экспорта в .xlsx

    public AnyTaskProgressBarVM() {}

    public AnyTaskProgressBarVM(AnyTaskProgressBar anyTaskProgressBar, CancellationTokenSource cts, IBackgroundLoader backgroundWorker, bool isShowDialog = false)
    {
        ExcelExportCancel = new ExcelExportCancelAsyncCommand(anyTaskProgressBar);
        _backgroundWorker = backgroundWorker;
        AnyTaskProgressBar_DB = anyTaskProgressBar;
        CancellationTokenSource = cts;
        LoadStatus = "Начало экспорта";
        ValueBar = 1;
        if (isShowDialog) IsShowDialog = true;
        //_backgroundWorker.BackgroundWorker(() =>
        //{

        //}, () => AnyTaskProgressBar.Close());
    }

    #region LoadStatus

    private string _loadStatus;
    public string LoadStatus
    {
        get => _loadStatus;
        set
        {
            if (value == _loadStatus) return;
            _loadStatus = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region ExportName

    private string _exportName;
    public string ExportName
    {
        get => _exportName;
        set
        {
            if (value == _exportName) return;
            _exportName = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region ExportType

    private string _exportType;
    public string ExportType
    {
        get => _exportType;
        set
        {
            if (value == _exportType) return;
            _exportType = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region ValueBar

    private int _valueBar;
    public int ValueBar
    {
        get => _valueBar;
        set
        {
            if (_valueBar == value) return;
            _valueBar = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region OnPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}