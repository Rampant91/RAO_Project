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

    public SkippedIdenticalReportsMessageWindow(IReadOnlyList<SkippedIdenticalReportInfo> reports)
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new SkippedIdenticalReportsMessageWindowVM(reports);
    }

    private void OnOkClicked(object? sender, RoutedEventArgs e) => Close();
}
