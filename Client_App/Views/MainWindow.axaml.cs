using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Models.Collections;
using System.Threading.Tasks;
using ReactiveUI;
using Client_App.Controls.DataGrid;
using Client_App.ViewModels;
using Client_App.VisualRealization.Short_Visual;
using MessageBox.Avalonia.Models;
using Models.Interfaces;

namespace Client_App.Views;

public class MainWindow : ReactiveWindow<MainWindowVM>
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
    public MainWindow(MainWindowVM dataContext)
    {
        DataContext = dataContext;
        Init();
    }
    public MainWindow()
    {
        DataContext = new MainWindowVM();
        Init();
    }
    private void Init()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        this.WhenActivated(d => d(MainWindowVM.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        this.WhenActivated(d => d(MainWindowVM.ShowMessage.RegisterHandler(DoShowDialogAsyncT)));
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    #endregion

    #region ShowDialog
    private async Task DoShowDialogAsync(InteractionContext<ChangeOrCreateVM, object> interaction)
    {
        FormChangeOrCreate frm = new(interaction.Input);

        await frm.ShowDialog(this);
        interaction.SetOutput(null);
    }

    private async Task DoShowDialogAsyncT(InteractionContext<List<string>, string> interaction)
    {
        MessageBox.Avalonia.DTO.MessageBoxCustomParams par = new() { ContentMessage = interaction.Input[0] };
        interaction.Input.RemoveAt(0);
        par.ContentHeader = interaction.Input[0];
        interaction.Input.RemoveAt(0);
        var lt = interaction.Input
            .Select(elem => new ButtonDefinition { Type = MessageBox.Avalonia.Enums.ButtonType.Default, Name = elem })
            .ToList();
        par.ButtonDefinitions = lt;
        par.ContentTitle = "Уведомление";
            
        var msg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(par);
        var answer = await msg.ShowDialog(this);
            
        interaction.SetOutput(answer);
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

    #region ShowInit_Контекстное меню и не только

    private static void SetCommandList(DataGrid<Reports> grd1, DataGrid<Report> grd2, string paramVal,
        MainWindowVM dataContext)
    {
        #region Grd1_Список организаций_Контекстное меню

        grd1.CommandsList.Add(new KeyCommand
        {
            Key = Avalonia.Input.Key.T,
            KeyModifiers = Avalonia.Input.KeyModifiers.Control,
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = paramVal,
            ContextMenuText = new[] { "Добавить форму        Ctrl+T" },
            Command = dataContext.AddReports
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = true,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Редактировать форму" },
            Command = dataContext.ChangeReports
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Выгрузить организацию" },
            Command = dataContext.ExportOrg
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Выгрузить организацию с указанием диапазона дат" },
            Command = dataContext.ExportOrgWithDateRange
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Выгрузить все организации" },
            Command = dataContext.ExportAllOrg
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            Key = Avalonia.Input.Key.D,
            KeyModifiers = Avalonia.Input.KeyModifiers.Control,
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Удалить форму           Ctrl+D" },
            Command = dataContext.DeleteReports
        });

        #endregion

        #region Grd2_Список форм_Контекстное меню
        grd2.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Выгрузка Excel", "Для печати" },
            Command = dataContext.ExcelExportFormPrint
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Выгрузка Excel", "Для анализа" },
            Command = dataContext.ExcelExportFormAnalysis
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Выгрузка" },
            Command = dataContext.ExportForm
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = true,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = new[] { "Изменить форму" },
            Command = dataContext.ChangeForm
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            Key = Avalonia.Input.Key.D,
            KeyModifiers = Avalonia.Input.KeyModifiers.Control,
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            IsUpdateCells = true,
            ContextMenuText = new[] { "Удалить форму           Ctrl+D" },
            Command = dataContext.DeleteForm
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            Key = Avalonia.Input.Key.J,
            KeyModifiers = Avalonia.Input.KeyModifiers.Control,
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            IsUpdateCells = true,
            ContextMenuText = new[] { "Сохранить комментарий           Ctrl+J" },
            Command = dataContext.SaveReports
        });
        #endregion
    }

    private void ShowInit()
    {
        var dataContext = (MainWindowVM)DataContext;

        var tab10 = this.FindControl<Panel>("Forms_p1_0");
        var tab1X = this.FindControl<Panel>("Forms_p1_X");
        var tab1B = this.FindControl<Panel>("Forms_p1_B");
        Form1_Visual.FormF_Visual(this, tab10, tab1X, tab1B);

        #region Form10 DataGrid
        var grd1 = (DataGrid<Reports>)tab10.Children[0];
        var grd2 = (DataGrid<Report>)tab1X.Children[0];

        SetCommandList(grd1, grd2, "1.0", dataContext);
        #endregion

        var tab20 = this.FindControl<Panel>("Forms_p2_0");
        var tab2X = this.FindControl<Panel>("Forms_p2_X");
        var tab2B = this.FindControl<Panel>("Forms_p2_B");
        Form2_Visual.FormF_Visual(this, tab20, tab2X, tab2B);

        #region Form20 DataGrid
        var grd3 = (DataGrid<Reports>)tab20.Children[0];
        var grd4 = (DataGrid<Report>)tab2X.Children[0];

        SetCommandList(grd3, grd4, "2.0", dataContext);
        #endregion
    }
    #endregion
}