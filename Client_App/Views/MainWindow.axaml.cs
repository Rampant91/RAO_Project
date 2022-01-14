using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Avalonia.Data;
using Models.Collections;
using System.Threading.Tasks;
using ReactiveUI;

namespace Client_App.Views
{
    public class MainWindow : ReactiveWindow<ViewModels.MainWindowVM>
    {

        public static readonly DirectProperty<MainWindow, IEnumerable<IKey>> SelectedReportsProperty =
            AvaloniaProperty.RegisterDirect<MainWindow, IEnumerable<IKey>>(
                nameof(SelectedReports),
                o => o.SelectedReports,
                (o, v) => o.SelectedReports = v);

        private IEnumerable<IKey> _selectedReports = new ObservableCollectionWithItemPropertyChanged<IKey>();

        public IEnumerable<IKey> SelectedReports
        {
            get => _selectedReports;
            set
            {
                if (value != null) SetAndRaise(SelectedReportsProperty, ref _selectedReports, value);
            }
        }
        public MainWindow()
        {
            DataContext = new ViewModels.MainWindowVM();
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
            this.WhenActivated(d => d(ViewModel!.ShowMessage.RegisterHandler(DoShowDialogAsync)));
            this.WhenActivated(d => d(ViewModel!.ShowMessageT.RegisterHandler(DoShowDialogAsyncT)));
        }

        private async Task DoShowDialogAsync(InteractionContext<ViewModels.ChangeOrCreateVM, object> interaction)
        {
            FormChangeOrCreate frm = new FormChangeOrCreate(interaction.Input.FormType);
            frm.DataContext = interaction.Input;

            await frm.ShowDialog(this);
            interaction.SetOutput(null);
        }
        private async Task DoShowDialogAsync(InteractionContext<string, string> interaction)
        {
            MessageBox.Avalonia.DTO.MessageBoxCustomParams par = new MessageBox.Avalonia.DTO.MessageBoxCustomParams();
            List<MessageBox.Avalonia.Models.ButtonDefinition> lt = new List<MessageBox.Avalonia.Models.ButtonDefinition>();
            lt.Add(new MessageBox.Avalonia.Models.ButtonDefinition
            {
                Type = MessageBox.Avalonia.Enums.ButtonType.Default,
                Name = "Да"
            });
            lt.Add(new MessageBox.Avalonia.Models.ButtonDefinition
            {
                Type = MessageBox.Avalonia.Enums.ButtonType.Default,
                Name = "Нет"
            });
            par.ButtonDefinitions = lt;
            par.ContentTitle = "Уведомление";
            par.ContentHeader = "Уведомление";
            par.ContentMessage = interaction.Input;
            var mssg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(par);
            var answ = await mssg.ShowDialog(this);

            interaction.SetOutput(answ);
        }

        private async Task DoShowDialogAsyncT(InteractionContext<List<string>, string> interaction)
        {
            MessageBox.Avalonia.DTO.MessageBoxCustomParams par = new MessageBox.Avalonia.DTO.MessageBoxCustomParams();
            List<MessageBox.Avalonia.Models.ButtonDefinition> lt = new List<MessageBox.Avalonia.Models.ButtonDefinition>();
            par.ContentMessage = interaction.Input[0];
            interaction.Input.RemoveAt(0);
            foreach (var elem in interaction.Input)
            {
                lt.Add(new MessageBox.Avalonia.Models.ButtonDefinition
                {
                    Type = MessageBox.Avalonia.Enums.ButtonType.Default,
                    Name = elem
                });

            }
            par.ButtonDefinitions = lt;
            par.ContentTitle = "Уведомление";
            par.ContentHeader = "Уведомление";
            var mssg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(par);
            var answ = await mssg.ShowDialog(this);

            interaction.SetOutput(answ);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            Init();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        private void Init()
        {
            Panel tab10 = this.FindControl<Panel>("Forms_p1_0");
            Panel tab1X = this.FindControl<Panel>("Forms_p1_X");
            Panel tab1B = this.FindControl<Panel>("Forms_p1_B");
            Short_Visual.Form1_Visual.FormF_Visual(this,tab10, tab1X, tab1B);

            Panel tab20 = this.FindControl<Panel>("Forms_p2_0");
            Panel tab2X = this.FindControl<Panel>("Forms_p2_X");
            Panel tab2B = this.FindControl<Panel>("Forms_p2_B");
            Short_Visual.Form2_Visual.FormF_Visual(this,tab20, tab2X, tab2B);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
