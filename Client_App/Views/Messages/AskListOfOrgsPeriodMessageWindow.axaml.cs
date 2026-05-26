using System;
using System.Globalization;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Messages;

namespace Client_App.Views.Messages;

public partial class AskListOfOrgsPeriodMessageWindow : Window
{
    public AskListOfOrgsPeriodMessageWindow()
    {
        AvaloniaXamlLoader.Load(this);
        DataContext = new AskListOfOrgsPeriodMessageVM();
    }

    private void OnOkButtonClicked(object? sender, RoutedEventArgs e)
    {
        var vm = (AskListOfOrgsPeriodMessageVM)DataContext!;

        var form1Start = TryParseDateBound(vm.Form1StartDate, isEnd: false);
        var form1End = TryParseDateBound(vm.Form1EndDate, isEnd: true);
        var form2Start = TryParseYearBound(vm.Form2StartYear, isEnd: false);
        var form2End = TryParseYearBound(vm.Form2EndYear, isEnd: true);

        Close(("Ок", form1Start, form1End, form2Start, form2End));
    }

    private void OnCancelButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close(("Отмена", DateOnly.MinValue, DateOnly.MaxValue, int.MinValue, int.MaxValue));
    }

    private static DateOnly TryParseDateBound(string? text, bool isEnd)
    {
        if (string.IsNullOrWhiteSpace(text))
            return isEnd ? DateOnly.MaxValue : DateOnly.MinValue;

        if (DateOnly.TryParseExact(text.Trim(), "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
            return date;

        if (DateOnly.TryParse(text.Trim(), out date))
            return date;

        return isEnd ? DateOnly.MaxValue : DateOnly.MinValue;
    }

    private static int TryParseYearBound(string? text, bool isEnd)
    {
        var digits = new string((text ?? string.Empty).Where(char.IsDigit).ToArray());
        if (digits.Length == 4 && int.TryParse(digits, out var year))
            return year;

        return isEnd ? int.MaxValue : int.MinValue;
    }
}
