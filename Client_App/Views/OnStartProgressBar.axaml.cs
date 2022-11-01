using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System.Threading.Tasks;
using ReactiveUI;
using Client_App.ViewModels;
using Avalonia.Controls.ApplicationLifetimes;

namespace Client_App.Views
{
    public partial class OnStartProgressBar : ReactiveWindow<ViewModels.OnStartProgressBarVM>
    {
        public OnStartProgressBar()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        }

        private async Task DoShowDialogAsync(InteractionContext<ViewModels.MainWindowVM, object> interaction)
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow(interaction.Input);
                desktop.MainWindow.Show();
                this.Close();
            }
            interaction.SetOutput(null);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            this.DataContext = new OnStartProgressBarVM();
        }
    }
}
