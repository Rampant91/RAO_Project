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
using Client_App.Controls.DataGrid;
namespace Client_App.Views
{
    public class MainWindow : ReactiveWindow<ViewModels.MainWindowVM>
    {
        #region SelectedReports
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
        #endregion

        #region Contructures
        public MainWindow(ViewModels.MainWindowVM dataContext)
        {
            DataContext = dataContext;
            Init();
        }
        public MainWindow()
        {
            DataContext = new ViewModels.MainWindowVM();
            Init();
        }
        private void Init()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
            this.WhenActivated(d => d(ViewModel!.ShowMessage.RegisterHandler(DoShowDialogAsyncT)));
        }
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        #endregion

        #region ShowDialog
        private async Task DoShowDialogAsync(InteractionContext<ViewModels.ChangeOrCreateVM, object> interaction)
        {

            FormChangeOrCreate frm = new FormChangeOrCreate(interaction.Input);

            await frm.ShowDialog(this);
            interaction.SetOutput(null);
        }

        private async Task DoShowDialogAsyncT(InteractionContext<List<string>, string> interaction)
        {
            MessageBox.Avalonia.DTO.MessageBoxCustomParams par = new MessageBox.Avalonia.DTO.MessageBoxCustomParams();
            List<MessageBox.Avalonia.Models.ButtonDefinition> lt = new List<MessageBox.Avalonia.Models.ButtonDefinition>();
            par.ContentMessage = interaction.Input[0];
            interaction.Input.RemoveAt(0);
            par.ContentHeader = interaction.Input[0];
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
            
            var mssg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(par);
            var answ = await mssg.ShowDialog(this);
            
            interaction.SetOutput(answ);
        }
        #endregion

        #region Events
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            ShowInit();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
        }
        #endregion

        #region ShowInit
        public static void SetCommandList(DataGrid<Reports> grd1, DataGrid<Report> grd2, string paramVal, ViewModels.MainWindowVM dataContext)
        {
            #region Grd1
            grd1.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                Key = Avalonia.Input.Key.T,
                KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = paramVal,
                ContextMenuText = new string[] { "Добавить форму        Ctrl+T" },
                Command = dataContext.AddReport
            });

            grd1.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = true,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                ContextMenuText = new string[] { "Редактировать форму" },
                Command = dataContext.ChangeReport
            });
            grd1.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                Key = Avalonia.Input.Key.D,
                KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                ContextMenuText = new string[] { "Удалить форму           Ctrl+D" },
                Command = dataContext.DeleteReport
            });

            #endregion
            #region Grd2
            grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                ContextMenuText = new string[] { "Выгрузка Excel", "Для печати" },
                Command = dataContext.Print_Excel_Export
            });
            grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                ContextMenuText = new string[] { "Выгрузка Excel", "Для анализа" },
                Command = dataContext.Excel_Export
            });
            grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                ContextMenuText = new string[] { "Выгрузка" },
                Command = dataContext.ExportForm
            });
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
                Key = Avalonia.Input.Key.D,
                KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                IsUpdateCells = true,
                ContextMenuText = new string[] { "Удалить форму           Ctrl+D" },
                Command = dataContext.DeleteForm
            });
            grd2.CommandsList.Add(new Controls.DataGrid.KeyComand()
            {
                Key = Avalonia.Input.Key.J,
                KeyModifiers = Avalonia.Input.KeyModifiers.Control,
                IsDoubleTappedCommand = false,
                IsContextMenuCommand = true,
                ParamName = "SelectedItems",
                IsUpdateCells = true,
                ContextMenuText = new string[] { "Сохранить комментарий           Ctrl+J" },
                Command = dataContext.SaveReport
            });
            #endregion
        }
        private void ShowInit()
        {
            var dataContext = (ViewModels.MainWindowVM)this.DataContext;

            Panel tab10 = this.FindControl<Panel>("Forms_p1_0");
            Panel tab1X = this.FindControl<Panel>("Forms_p1_X");
            Panel tab1B = this.FindControl<Panel>("Forms_p1_B");
            Short_Visual.Form1_Visual.FormF_Visual(this, tab10, tab1X, tab1B);

            #region Form10 DataGrid
            var grd1 = (Controls.DataGrid.DataGrid<Reports>)tab10.Children[0];
            var grd2 = (Controls.DataGrid.DataGrid<Report>)tab1X.Children[0];

            SetCommandList(grd1, grd2, "1.0", dataContext);
            #endregion

            Panel tab20 = this.FindControl<Panel>("Forms_p2_0");
            Panel tab2X = this.FindControl<Panel>("Forms_p2_X");
            Panel tab2B = this.FindControl<Panel>("Forms_p2_B");
            Short_Visual.Form2_Visual.FormF_Visual(this, tab20, tab2X, tab2B);

            #region Form20 DataGrid
            var grd3 = (Controls.DataGrid.DataGrid<Reports>)tab20.Children[0];
            var grd4 = (Controls.DataGrid.DataGrid<Report>)tab2X.Children[0];

            SetCommandList(grd3, grd4, "2.0", dataContext);
            #endregion
        }
        #endregion
    }
}
