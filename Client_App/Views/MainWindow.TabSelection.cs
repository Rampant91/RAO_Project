using Avalonia.Controls;

namespace Client_App.Views;

public partial class MainWindow
{
    private void TabControl_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (sender is not TabControl tc)
            return;

        // Вкладки: индекс 0 — скрытый, 1 — Forms1, 2 — Forms2 и т.д.
        var target = SelectedReports; // по умолчанию текущее значение

        if (tc.SelectedIndex == 1)
        {
            target = SelectedReports1;
        }
        else if (tc.SelectedIndex == 2)
        {
            target = SelectedReports2;
        }
        else
        {
            // Резервная логика через тип формы из VM
            if (DataContext is ViewModels.MainWindowVM vm)
            {
                var formNum = vm.SelectedReportTypeToString;
                if (!string.IsNullOrEmpty(formNum))
                {
                    if (formNum.StartsWith("1"))
                        target = SelectedReports1;
                    else if (formNum.StartsWith("2"))
                        target = SelectedReports2;
                }
            }
        }

        if (!ReferenceEquals(SelectedReports, target))
        {
            SelectedReports = target;
        }
    }
}