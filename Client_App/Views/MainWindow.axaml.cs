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

        public MainWindow(ViewModels.MainWindowVM dataContext)
        {
            DataContext = dataContext;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
            this.WhenActivated(d => d(ViewModel!.ShowMessage.RegisterHandler(DoShowDialogAsync)));
            this.WhenActivated(d => d(ViewModel!.ShowMessageT.RegisterHandler(DoShowDialogAsyncT)));
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

            FormChangeOrCreate frm = new FormChangeOrCreate(interaction.Input);

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
            var dataContext = (ViewModels.MainWindowVM)this.DataContext;

            Panel tab10 = this.FindControl<Panel>("Forms_p1_0");
            Panel tab1X = this.FindControl<Panel>("Forms_p1_X");
            Panel tab1B = this.FindControl<Panel>("Forms_p1_B");
            Short_Visual.Form1_Visual.FormF_Visual(this,tab10, tab1X, tab1B);

            #region Form10 DataGrid
            var grd1 = (Controls.DataGrid.DataGrid<Reports>)tab10.Children[0];
            var grd2 = (Controls.DataGrid.DataGrid<Report>)tab1X.Children[0];

            #region Grd1
            grd1.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "1.0",
                ContextMenuText = new string[] { "Добавить форму" },
                Command = dataContext.AddReport
            });

            grd1.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = true,
                IsContextMenuCommand=true,
                ParamName="SelectedItems",
                ContextMenuText=new string[] { "Редактировать форму" },
                Command=dataContext.ChangeReport
            }) ;
            grd1.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                ContextMenuText = new string[] { "Удалить форму" },
                Command = dataContext.DeleteReport
            });

            #endregion
            #region Grd2
            //grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            //{
            //    IsDoubleTappedCommand = false,
            //    IsContextMenuCommand = true,
            //    ParamName = "1.0",
            //    ContextMenuText = new string[] { "Экспорт Excel", "Для печати" },
            //    Command = dataContext.AddReport
            //});
            //grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            //{
            //    IsDoubleTappedCommand = true,
            //    IsContextMenuCommand = true,
            //    ParamName = "SelectedItems",
            //    ContextMenuText = new string[] { "Экспорт Excel", "Для выгрузки" },
            //    Command = dataContext.ChangeReport
            //});
            //grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            //{
            //    IsDoubleTappedCommand = false,
            //    IsContextMenuCommand = true,
            //    ParamName = "SelectedItems",
            //    ContextMenuText = new string[] { "Экспорт" },
            //    Command = dataContext.DeleteReport
            //});
            grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = true,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                ContextMenuText = new string[] { "Изменить форму" },
                Command = dataContext.ChangeForm
            });
            grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                IsUpdateCells = true,
                ContextMenuText = new string[] { "Удалить форму" },
                Command = dataContext.DeleteForm
            });
            #endregion
            #endregion

            Panel tab20 = this.FindControl<Panel>("Forms_p2_0");
            Panel tab2X = this.FindControl<Panel>("Forms_p2_X");
            Panel tab2B = this.FindControl<Panel>("Forms_p2_B");
            Short_Visual.Form2_Visual.FormF_Visual(this,tab20, tab2X, tab2B);

            #region Form20 DataGrid
            var grd3 = (Controls.DataGrid.DataGrid<Reports>)tab20.Children[0];
            var grd4 = (Controls.DataGrid.DataGrid<Report>)tab2X.Children[0];

            #endregion
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
