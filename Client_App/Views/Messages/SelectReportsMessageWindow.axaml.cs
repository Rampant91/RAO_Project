using System.Collections.Generic;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Client_App.ViewModels.Messages;
using Models.Collections;

namespace Client_App.Views.Messages;

public partial class SelectReportsMessageWindow : BaseWindow<BaseVM>
{
    public SelectReportsMessageWindow() { }

    public SelectReportsMessageWindow(List<Reports> repsList)
    {
        InitializeComponent(repsList);
    }

    public SelectReportsMessageWindow(List<Reports> repsList, string fileName, int totalReports, int currentReportIndex, Reports impReps)
    {
        InitializeComponent(repsList, fileName, totalReports, currentReportIndex, impReps);
    }

    private void InitializeComponent(List<Reports> repsList)
    {
        AvaloniaXamlLoader.Load(this);
        var viewModel = new SelectReportsMessageWindowVM(repsList);
        DataContext = viewModel;
        
        // Подписываемся на события ViewModel
        viewModel.OkRequested += (organization) => 
        {
            SelectedOrganization = organization;
            Close(organization);
        };
        
        viewModel.CancelRequested += () => Close(null);
    }

    private void InitializeComponent(List<Reports> repsList, string fileName, int totalReports, int currentReportIndex, Reports impReps)
    {
        AvaloniaXamlLoader.Load(this);
        var viewModel = new SelectReportsMessageWindowVM(repsList, fileName, totalReports, currentReportIndex, impReps);
        DataContext = viewModel;
        
        // Подписываемся на события ViewModel
        viewModel.OkRequested += (organization) => 
        {
            SelectedOrganization = organization;
            Close(organization);
        };
        
        viewModel.CancelRequested += () => Close(null);
    }
    
    public SelectReportsMessageWindowVM.OrganizationInfo SelectedOrganization { get; private set; }
}