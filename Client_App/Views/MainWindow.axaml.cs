using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Models.Collections;
using System.Threading.Tasks;
using Avalonia.Interactivity;
using Client_App.Commands.AsyncCommands;
using ReactiveUI;
using Client_App.Controls.DataGrid;
using Client_App.ViewModels;
using Client_App.VisualRealization.Short_Visual;
using MessageBox.Avalonia.Models;
using Models.Interfaces;

namespace Client_App.Views;

public class MainWindow : BaseWindow<MainWindowVM>
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
        set => SetAndRaise(SelectedReportsProperty, ref _selectedReports, value); // ����� if (value != null) 
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
        WindowState = WindowState.Minimized;
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
            .Select(elem => new ButtonDefinition { Name = elem })
            .ToList();
        par.ButtonDefinitions = lt;
        par.ContentTitle = "�����������";
            
        var msg = MessageBox.Avalonia.MessageBoxManager.GetMessageBoxCustomWindow(par);
        var answer = await msg.ShowDialog(this);
            
        interaction.SetOutput(answer);
    }

    #endregion

    #region Events

    private void OpenContactsButtonClicked(object? sender, RoutedEventArgs e)
    {
        var contactsWindow = new Contacts();
        contactsWindow.Show();
    }

    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);
        ShowInit();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        RemoveTmpData();
        base.OnClosing(e);
    }

    #region RemoveTmpData

    /// <summary>
    /// ����� ��������� ��������� ������� ����� tmp.
    /// </summary>
    private static void RemoveTmpData()
    {
        DirectoryInfo tmpDirInfo = new(BaseVM.TmpDirectory);
        foreach (var fileInfo in tmpDirInfo.GetFiles("*.*", SearchOption.AllDirectories))
        {
            try
            {
                File.Delete(fileInfo.FullName);
            }
            catch
            {
                // ignore
            }

        }
    }

    #endregion

    #endregion

    #region ShowInit_����������� ���� � �� ������

    private static void SetCommandList(DataGrid<Reports> grd1, DataGrid<Report> grd2, string paramVal, MainWindowVM dataContext)
    {
        #region Grd1_������ �����������_����������� ����

        grd1.CommandsList.Add(new KeyCommand
        {
            Key = Avalonia.Input.Key.T,
            KeyModifiers = Avalonia.Input.KeyModifiers.Control,
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = paramVal,
            ContextMenuText = ["�������� �����        Ctrl+T"],
            Command = dataContext.AddReports
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = true,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["������������� �����"],
            Command = dataContext.ChangeReports
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["��������� �����������"],
            Command = MainWindowVM.ExportReports
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["��������� ����������� � ��������� ��������� ���"],
            Command = MainWindowVM.ExportReportsWithDateRange
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["��������� ��� ����������� � ���� ����"],
            Command = dataContext.ExportAllReportsOneFile
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["��������� ��� ����������� � ��������� �����"],
            Command = dataContext.ExportAllReports
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["��������� ��� ����� �����������"],
            Command = dataContext.ExcelExportCheckAllForms
        });

        grd1.CommandsList.Add(new KeyCommand
        {
            Key = Avalonia.Input.Key.D,
            KeyModifiers = Avalonia.Input.KeyModifiers.Control,
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["������� �����           Ctrl+D"],
            Command = dataContext.DeleteReports
        });

        #endregion

        #region Grd2_������ ����_����������� ����

        grd2.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["�������� Excel", "��� ������"],
            Command = dataContext.ExcelExportFormPrint
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["�������� Excel", "��� �������"],
            Command = dataContext.ExcelExportFormAnalysis
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["��������"],
            Command = dataContext.ExportForm
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            IsDoubleTappedCommand = true,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            ContextMenuText = ["�������� �����"],
            Command = dataContext.ChangeForm
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            IsContextMenuCommand = true,
            IsDoubleTappedCommand = false,
            ParamName = "SelectedItems",
            ContextMenuText = ["��������� �����"],
            Command = dataContext.CheckFormFromMain
        });
        grd2.CommandsList.Add(new KeyCommand
        {
            Key = Avalonia.Input.Key.D,
            KeyModifiers = Avalonia.Input.KeyModifiers.Control,
            IsDoubleTappedCommand = false,
            IsContextMenuCommand = true,
            ParamName = "SelectedItems",
            IsUpdateCells = true,
            ContextMenuText = ["������� �����           Ctrl+D"],
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
            ContextMenuText = ["��������� �����������           Ctrl+J"],
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