using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Client_App.Views;
using Models.Collections;
using System.ComponentModel;
using ReactiveUI;

namespace Client_App;

public partial class CompareReportsTitleForm : Window
{
    public Report NewMasterRep = new();
    private Report RepInBase;

    public CompareReportsTitleForm(){}

    public CompareReportsTitleForm(Report repInBase, Report repImport)
    {
        var desktop = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        InitializeComponent(repInBase, repImport);
        //ShowDialog(desktop?.MainWindow);
    }

    private void InitializeComponent(Report repInBase, Report repImport)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new CompareReportsTitleFormVM(repInBase, repImport);
        RepInBase = repInBase;
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        var vm = DataContext as CompareReportsTitleFormVM;
        RepInBase.Rows10[0].SubjectRF_DB = vm.SubjectRF;
        base.OnClosing(e);
    }
}