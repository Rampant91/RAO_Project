using System.Collections.Generic;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public partial class SkippedIdenticalReportsMessageWindow : BaseWindow<BaseVM>
{
    public SkippedIdenticalReportsMessageWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new SkippedIdenticalReportsMessageWindowVM();
    }

    public SkippedIdenticalReportsMessageWindow(
        IReadOnlyList<SkippedIdenticalReportInfo> importedReports,
        IReadOnlyList<SkippedIdenticalReportInfo> skippedReports,
        string headerTitle = SkippedIdenticalReportsMessageWindowVM.Form1HeaderTitle)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new SkippedIdenticalReportsMessageWindowVM(importedReports, skippedReports, headerTitle);
    }

    private void OnOkClicked(object? sender, RoutedEventArgs e) => Close();
}
