using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;
using System.Reactive.Linq;
using Client_App.Commands.AsyncCommands;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.Interfaces.Logger;

namespace Client_App.ViewModels;

public class OnStartProgressBarVM : BaseVM, INotifyPropertyChanged
{
    private Task MainTask { get; set; }

    public OnStartProgressBarVM(IBackgroundLoader backgroundWorker)
    {
        ShowDialog = new Interaction<MainWindowVM, object>();
        backgroundWorker.BackgroundWorker(() =>
        {
            ServiceExtension.LoggerManager.CreateFile("Import.log");
            ServiceExtension.LoggerManager.CreateFile("Crash.log");
        }, () =>
        {
            MainTask = new Task(async () => await Start().ConfigureAwait(false));
            MainTask.GetAwaiter().OnCompleted(async () => await ShowDialog.Handle(VMDataContext));
            MainTask.Start();
        });
    }

    private double _onStartProgressBar;
    public double OnStartProgressBar
    {
        get => _onStartProgressBar;
        set
        {
            if (_onStartProgressBar.Equals(value)) return;
            _onStartProgressBar = value;
            OnPropertyChanged();
        }
    }

    private string _loadStatus;
    public string LoadStatus
    {
        get => _loadStatus;
        set
        {
            if (_loadStatus == value) return;
            _loadStatus = value;
            OnPropertyChanged();
        }
    }

    private MainWindowVM VMDataContext {get;set;}

    private async Task Start()
    {
        VMDataContext = new MainWindowVM();
        VMDataContext.PropertyChanged += OnVMPropertyChanged;
        await new InitializationAsyncCommand(VMDataContext).AsyncExecute(this);
    }

    private void OnVMPropertyChanged(object sender,PropertyChangedEventArgs args)
    {
        if(args.PropertyName==nameof(OnStartProgressBar))
        {
            OnStartProgressBar = VMDataContext.OnStartProgressBar;
        }
    }
    public Interaction<MainWindowVM, object> ShowDialog { get; private set; }

    #region INotifyPropertyChanged

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion
}