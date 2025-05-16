using System;
using Client_App.ViewModels.Calculator;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Client_App.Commands.AsyncCommands.Calculator;

public partial class ActivityCalculationAsyncCommand : BaseAsyncCommand
{
    private readonly ActivityCalculatorVM _activityCalculatorVM;

    #region Constructor
    
    public ActivityCalculationAsyncCommand(ActivityCalculatorVM activityCalculatorVM)
    {
        _activityCalculatorVM = activityCalculatorVM;
        _activityCalculatorVM.PropertyChanged += ActivityCalculatorVMPropertyChanged;
    }

    #endregion

    #region PropertyChanged
    
    private void ActivityCalculatorVMPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(ActivityCalculatorVM.SelectedDictionaryNuclid.Halflife)
            or nameof(ActivityCalculatorVM.SelectedDictionaryNuclid.Unit)
            or nameof(ActivityCalculatorVM.InitialActivity)
            or nameof(ActivityCalculatorVM.TimePeriodDouble)
            or nameof(ActivityCalculatorVM.SelectedTimeUnit))
        {
            OnCanExecuteChanged();
        }
    }

    #endregion

    #region AsyncExecute
    
    public override Task AsyncExecute(object? parameter)
    {
        switch (_activityCalculatorVM)
        {
            case { IsDateRange: false }:
            {
                _activityCalculatorVM.IsDateRangeTextVisible = false;

                if (double.TryParse(ToExponentialString(_activityCalculatorVM.TimePeriodDouble), out var timePeriodDoubleValue) 
                    && double.TryParse(ToExponentialString(_activityCalculatorVM.InitialActivity), out var initialActivityDoubleValue))
                {
                    var timeParam = GetTimeDoubleValueInMinutes(timePeriodDoubleValue, _activityCalculatorVM.SelectedTimeUnit)
                                    / GetTimeDoubleValueInMinutes(_activityCalculatorVM.SelectedDictionaryNuclid.Halflife, _activityCalculatorVM.SelectedDictionaryNuclid.Unit);

                    var degree = -0.693 * timeParam;
                    var exp = Math.Exp(degree);
                    var activity = initialActivityDoubleValue * exp;

                    _activityCalculatorVM.ResidualActivity = ToExponentialString(activity);
                }
                else _activityCalculatorVM.ResidualActivity = string.Empty;

                break;
            }
            case { IsDateRange: true }:
            {
                if (!(double.TryParse(ToExponentialString(_activityCalculatorVM.InitialActivity), out var initialActivityDoubleValue)
                      && DateOnly.TryParse(_activityCalculatorVM.InitialActivityDate, out var initialActivityDate)
                      && DateOnly.TryParse(_activityCalculatorVM.ResidualActivityDate, out var residualActivityDate)))
                {
                    _activityCalculatorVM.IsDateRangeTextVisible = false;
                    _activityCalculatorVM.ResidualActivity = string.Empty;
                    return Task.CompletedTask;
                }

                _activityCalculatorVM.IsDateRangeTextVisible = initialActivityDate > residualActivityDate;
                if (initialActivityDate > residualActivityDate)
                {
                    _activityCalculatorVM.ResidualActivity = string.Empty;
                    return Task.CompletedTask;
                }

                var timeParam = GetTimeDoubleValueInMinutes(residualActivityDate.DayNumber - initialActivityDate.DayNumber, "сут")
                                / GetTimeDoubleValueInMinutes(_activityCalculatorVM.SelectedDictionaryNuclid.Halflife, _activityCalculatorVM.SelectedDictionaryNuclid.Unit);

                var degree = -0.693 * timeParam;
                var exp = Math.Exp(degree);
                var activity = initialActivityDoubleValue * exp;

                _activityCalculatorVM.ResidualActivity = ToExponentialString(activity);

                break;
            }
        }
        return Task.CompletedTask;
    }

    #endregion

    #region GetTimeDoubleValueInMinutes

    /// <summary>
    /// Переводит значение времени в минуты.
    /// </summary>
    /// <param name="timeValue">Значение времени, которое нужно конвертировать.</param>
    /// <param name="unit">Единица измерения.</param>
    /// <returns>Значение времени в минутах.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Разрешённые значения для unit: "мин", "час", "сут", "лет".</exception>
    private static double GetTimeDoubleValueInMinutes(double timeValue, string unit)
    {
        return unit switch
        {
            "мин" => timeValue,
            "час" => timeValue * 60,
            "сут" => timeValue * 60 * 24,
            "лет" => timeValue * 60 * 24 * 365,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }

    #endregion

    #region ToExponentialString

    private static string ToExponentialString(object? value)
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

    #endregion

    #region ReplaceDashes

    /// <summary>
    /// Заменяет в строчке все виды тире на стандартное.
    /// </summary>
    /// <param name="value">Строчка данных.</param>
    /// <returns>Строчка, в которой заменены все виды тире на стандартное.</returns>
    private static string ReplaceDashes(string value)
    {
        return value switch
        {
            null => string.Empty,
            _ => DashesRegex().Replace(value, "-")
        };
    }

    #endregion

    #region Regex

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    private static partial Regex DashesRegex();

    #endregion
}