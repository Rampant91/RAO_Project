using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Client_App.ViewModels.Calculator;

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
        if (e.PropertyName is nameof(ActivityCalculatorVM.SelectedDictionaryNuclid)
            or nameof(ActivityCalculatorVM.InitialActivity)
            or nameof(ActivityCalculatorVM.InitialActivityDate)
            or nameof(ActivityCalculatorVM.ResidualActivityDate)
            or nameof(ActivityCalculatorVM.TimePeriodDouble)
            or nameof(ActivityCalculatorVM.SelectedTimeUnit)
            or nameof(ActivityCalculatorVM.IsDateRange))
        {
            OnCanExecuteChanged();
        }
    }

    #endregion

    #region AsyncExecute

    public override Task AsyncExecute(object? parameter)
    {
        if (_activityCalculatorVM.IsDateRange)
            CalculateByDateRange();
        else
            CalculateByTimePeriod();

        return Task.CompletedTask;
    }

    #endregion

    #region Calculate

    private void CalculateByDateRange()
    {
        var hasActivity = double.TryParse(
            ToExponentialString(_activityCalculatorVM.InitialActivity),
            out var initialActivity);
        var hasInitialDate = DateOnly.TryParse(_activityCalculatorVM.InitialActivityDate, out var initialDate);
        var hasResidualDate = DateOnly.TryParse(_activityCalculatorVM.ResidualActivityDate, out var residualDate);

        if (hasInitialDate && hasResidualDate)
        {
            _activityCalculatorVM.IsDateRangeTextVisible = initialDate > residualDate;
            if (initialDate > residualDate)
            {
                _activityCalculatorVM.ResidualActivity = string.Empty;
                return;
            }
        }
        else
        {
            _activityCalculatorVM.IsDateRangeTextVisible = false;
        }

        if (!hasActivity
            || !hasInitialDate
            || !hasResidualDate
            || !TryGetHalfLifeMinutes(out var halfLifeMinutes))
        {
            _activityCalculatorVM.ResidualActivity = string.Empty;
            return;
        }

        var timeParam = GetTimeDoubleValueInMinutes(residualDate.DayNumber - initialDate.DayNumber, "сут")
                        / halfLifeMinutes;
        _activityCalculatorVM.ResidualActivity = ToExponentialString(Decay(initialActivity, timeParam));
    }

    private void CalculateByTimePeriod()
    {
        _activityCalculatorVM.IsDateRangeTextVisible = false;

        if (!double.TryParse(ToExponentialString(_activityCalculatorVM.TimePeriodDouble), out var timePeriod)
            || !double.TryParse(ToExponentialString(_activityCalculatorVM.InitialActivity), out var initialActivity)
            || string.IsNullOrWhiteSpace(_activityCalculatorVM.SelectedTimeUnit)
            || !TryGetHalfLifeMinutes(out var halfLifeMinutes))
        {
            _activityCalculatorVM.ResidualActivity = string.Empty;
            return;
        }

        var timeParam = GetTimeDoubleValueInMinutes(timePeriod, _activityCalculatorVM.SelectedTimeUnit)
                        / halfLifeMinutes;
        _activityCalculatorVM.ResidualActivity = ToExponentialString(Decay(initialActivity, timeParam));
    }

    /// <summary>
    /// Период полураспада выбранного радионуклида в минутах.
    /// false — если нуклид/единица ещё не заданы (любой порядок ввода).
    /// </summary>
    private bool TryGetHalfLifeMinutes(out double halfLifeMinutes)
    {
        halfLifeMinutes = 0;
        var nuclid = _activityCalculatorVM.SelectedDictionaryNuclid;
        if (nuclid is null || string.IsNullOrWhiteSpace(nuclid.Unit) || nuclid.Halflife <= 0)
            return false;

        try
        {
            halfLifeMinutes = GetTimeDoubleValueInMinutes(nuclid.Halflife, nuclid.Unit);
            return halfLifeMinutes > 0;
        }
        catch (ArgumentOutOfRangeException)
        {
            return false;
        }
    }

    private static double Decay(double initialActivity, double timeParam) =>
        initialActivity * Math.Exp(-0.693 * timeParam);

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
                new CultureInfo("ru-RU", useUserOverride: false),
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
