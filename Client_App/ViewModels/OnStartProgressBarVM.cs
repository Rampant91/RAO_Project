using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ReactiveUI;
using System.Reactive.Linq;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.Interfaces.Logger;

namespace Client_App.ViewModels;

public class OnStartProgressBarVM : BaseVM, INotifyPropertyChanged
{
    public Task MainTask { get; set; }
    public OnStartProgressBarVM(IBackgroundLoader backgroundWorker)
    {
        ShowDialog = new Interaction<MainWindowVM, object>();
        backgroundWorker.BackgroundWorker(() =>
        {
            ServiceExtension.LoggerManager.CreateFile("Import.log");
        }, () =>
        {
            MainTask=new Task(() => Start());
            MainTask.GetAwaiter().OnCompleted(async ()=> await ShowDialog.Handle(VMDataContext));
            MainTask.Start();
        });
    }
    private double _OnStartProgressBar;
    public double OnStartProgressBar
    {
        get => _OnStartProgressBar;
        set
        {
            if (_OnStartProgressBar == value) return;
            _OnStartProgressBar = value;
            OnPropertyChanged();
        }
    }

    private string _LoadStatus;
    public string LoadStatus
    {
        get => _LoadStatus;
        set
        {
            if (_LoadStatus == value) return;
            _LoadStatus = value;
            OnPropertyChanged();
        }
    }

    public MainWindowVM VMDataContext {get;set;}
    public async Task Start()
    {
        VMDataContext = new MainWindowVM();
        VMDataContext.PropertyChanged += OnVMPropertyChanged;
        await VMDataContext.Init(this);
    }
    public void OnVMPropertyChanged(object sender,PropertyChangedEventArgs args)
    {
        if(args.PropertyName==nameof(OnStartProgressBar))
        {
            OnStartProgressBar = VMDataContext.OnStartProgressBar;
        }
    }
    public Interaction<MainWindowVM, object> ShowDialog { get; private set; }

    #region INotifyPropertyChanged
    protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
    public event PropertyChangedEventHandler PropertyChanged;
    #endregion
}