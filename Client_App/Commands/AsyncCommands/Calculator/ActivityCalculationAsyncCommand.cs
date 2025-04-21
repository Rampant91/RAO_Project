using System;
using Client_App.ViewModels.Calculator;
using System.Linq;
using System.Threading.Tasks;
using ExtendedNumerics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Client_App.Commands.AsyncCommands.Calculator;

public partial class ActivityCalculationAsyncCommand : BaseAsyncCommand
{
    private readonly ActivityCalculatorVM _activityCalculatorVM;

    public ActivityCalculationAsyncCommand(ActivityCalculatorVM activityCalculatorVM)
    {
        _activityCalculatorVM = activityCalculatorVM;
        _activityCalculatorVM.PropertyChanged += ActivityCalculatorVMPropertyChanged;
    }

    private void ActivityCalculatorVMPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ActivityCalculatorVM.SelectedNuclid.Halflife) 
            or nameof(ActivityCalculatorVM.SelectedNuclid.Unit) 
            or nameof(ActivityCalculatorVM.InitialActivity) 
            or nameof(ActivityCalculatorVM.TimePeriodDouble) 
            or nameof(ActivityCalculatorVM.SelectedTimeUnit))
        {
            OnCanExecuteChanged();
        }
    }

    public override Task AsyncExecute(object? parameter)
    {
        if (_activityCalculatorVM is { IsDateRange: false })
        {
            if (double.TryParse(_activityCalculatorVM.TimePeriodDouble, out var timePeriodDoubleValue)
                && double.TryParse(ToExponentialString(_activityCalculatorVM.InitialActivity), out var initialActivityDoubleValue))
            {
                var halfLife = GetTimeDoubleValueInMinutes(_activityCalculatorVM.SelectedNuclid.Halflife, _activityCalculatorVM.SelectedNuclid.Unit);
                var timePeriod = GetTimeDoubleValueInMinutes(timePeriodDoubleValue, _activityCalculatorVM.SelectedTimeUnit);
                var cycles = timePeriod / halfLife;
                if (cycles > 10000) cycles = 10000;
                var activity = initialActivityDoubleValue / BigDecimal.Pow(2, cycles);
                _activityCalculatorVM.ResidualActivity = ToExponentialString(activity);
            }
            else _activityCalculatorVM.ResidualActivity = string.Empty;
        }
        else if (_activityCalculatorVM is { IsDateRange: true })
        {
            if (double.TryParse(ToExponentialString(_activityCalculatorVM.InitialActivity), out var initialActivityDoubleValue) 
                && DateOnly.TryParse(_activityCalculatorVM.InitialActivityDate, out var initialActivityDate) 
                && DateOnly.TryParse(_activityCalculatorVM.ResidualActivityDate, out var residualActivityDate))
            {
                var halfLife = GetTimeDoubleValueInMinutes(_activityCalculatorVM.SelectedNuclid.Halflife, _activityCalculatorVM.SelectedNuclid.Unit);
                var timePeriod = GetTimeDoubleValueInMinutes(residualActivityDate.DayNumber - initialActivityDate.DayNumber, "сут");
                var cycles = timePeriod / halfLife;
                if (cycles > 10000) cycles = 10000;
                var activity = initialActivityDoubleValue / BigDecimal.Pow(2, cycles);
                _activityCalculatorVM.ResidualActivity = ToExponentialString(activity);
            }
            else _activityCalculatorVM.ResidualActivity = string.Empty;
        }
        return Task.CompletedTask;
    }

    private static double GetTimeDoubleValueInMinutes(double halfLife, string unit)
    {
        return unit switch
        {
            "мин" => halfLife,
            "час" => halfLife * 60,
            "сут" => halfLife * 60 * 24,
            "лет" => halfLife * 60 * 24 * 365,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }

    private protected static string ToExponentialString(object? value)
    {
        var tmp = (value?.ToString() ?? string.Empty)
            .Trim()
            .ToLower()
            .Replace('е', 'e');
        tmp = ReplaceDashes(tmp);
        if (tmp != "прим.")
        {
            tmp = tmp.Replace('.', ',');
        }
        if (tmp is "прим." or "-")
        {
            return tmp;
        }
        var doubleStartsWithBrackets = false;
        if (tmp.StartsWith('(') && tmp.EndsWith(')'))
        {
            doubleStartsWithBrackets = true;
            tmp = tmp
                .TrimStart('(')
                .TrimEnd(')');
        }
        var tmpNumWithoutSign = tmp.StartsWith('+') || tmp.StartsWith('-')
            ? tmp[1..]
            : tmp;
        var sign = tmp.StartsWith('-')
            ? "-"
            : string.Empty;
        if (!tmp.Contains('e')
            && tmpNumWithoutSign.Count(x => x is '+' or '-') == 1)
        {
            tmp = sign + tmpNumWithoutSign.Replace("+", "e+").Replace("-", "e-");
        }
        if (double.TryParse(tmp,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                CultureInfo.CreateSpecificCulture("ru-RU"),
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.###e+00}";
        }
        return doubleStartsWithBrackets
            ? $"({tmp})"
            : tmp;
    }

    private protected static string ReplaceDashes(string value) =>
        value switch
        {
            null => string.Empty,
            _ => DashesRegex().Replace(value, "-")
        };

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    protected static partial Regex DashesRegex();
}