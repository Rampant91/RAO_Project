using Client_App.ViewModels.Calculator;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Models.DTO;
using System.Text.RegularExpressions;
using System;

namespace Client_App.Commands.AsyncCommands.Calculator;

public partial class CategoryCalculationAsyncCommand : BaseAsyncCommand
{
    #region Properties

    private readonly CategoryCalculatorVM _vm;

    private static readonly string[] EquilibriumRadionuclids =
    [
        "германий-68, галлий-68",
        "рубидий-83, криптон-83м",
        "стронций-82, рубидий-82",
        "стронций-90, иттрий-90",
        "иттрий-87, стронций-87м",
        "цирконий-93, ниобий-93м",
        "цирконий-97, ниобий-97",
        "рутений-106, родий-106",
        "серебро-108м, серебро-108",
        "олово-121м, олово-121",
        "олово-126, олово-126м",
        "ксенон-122, иод-122",
        "цезий-137, барий-137м",
        "барий-140, лантан-140",
        "церий-134, лантан-134",
        "церий-144, празеодим-144",
        "гадолиний-146, европий-146",
        "гафний-172, лютеций-172",
        "вольфрам-178, тантал-178",
        "вольфрам-188, рений-188",
        "рений-189, осмий-189м",
        "иридий-189, осмий-189м",
        "платина-188, иридий-188",
        "ртуть-194, золото-194",
        "ртуть-195м, ртуть-195",
        "свинец-210, висмут-210, полоний-210",
        "свинец-212, висмут-212, титан-208, полоний-212",
        "висмут-210м, титан-206",
        "висмут-212, титан-208, полоний-212",
        "радон-220, полоний-216",
        "радон-222, полоний-218, свинец-214, висмут-214, полоний-214",
        "радий-223, радон-219, полоний-215, свинец-211, висмут-214, титан-207",
        "радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "радий-226, радон-222, полоний-218, свинец-214, висмут-214, полоний-214, свинец-210, висмут-210, полоний-210",
        "радий-228, актиний-228",
        "актиний-225, франций-221, астат-217, висмут-213, полоний-213, титан-209, свинец-209",
        "актиний-227, франций-223",
        "торий-226, радий-222, радон-218, полоний-214",
        "торий-228, радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "торий-229, радий-225, актиний-225, франций-221, астат-217, висмут-213, полоний-213, свинец-209",
        "торий-232, радий-228, актиний-228, торий-228, радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "торий природный, радий-228, актиний-228, торий-228, радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "торий-234, протактиний-234м",
        "уран-230, торий-226, радий-222, радон-218, полоний-214",
        "уран-232, торий-228, радий-224, радон-220, полоний-216, свинец-212, висмут-212, титан-208, полоний-212",
        "уран-235, торий-231",
        "уран-238, торий-234, протактиний-234м",
        "уран природный, торий-234, протактиний-234м, уран-234, торий-230, радий-226, радон-222, полоний-218, свинец-214, висмут-214, полоний-214, свинец-210, висмут-210, полоний-210",
        "уран-240, нептуний-240м",
        "нептуний-237, протактиний-233",
        "америций-242м, америций-242",
        "америций-243, нептуний-239"
    ];

    #endregion

    #region Constructor

    public CategoryCalculationAsyncCommand(CategoryCalculatorVM vm)
    {
        _vm = vm;
        _vm.PropertyChanged += VMPropertyChanged;
    }

    #endregion

    #region PropertyChanged

    private void VMPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(CategoryCalculatorVM.SelectedRadionuclids)
            or nameof(CategoryCalculatorVM.Quantity))
        {
            OnCanExecuteChanged();
        }
    }
    
    #endregion

    public override async Task AsyncExecute(object? parameter)
    {
        var dbBounds = new Dictionary<short, (decimal, decimal)>
        {
            { 1, (1000, decimal.MaxValue) },
            { 2, (10, 1000) },
            { 3, (1, 10) },
            { 4, (0.01m, 1) },
            { 5, (0, 0.01m) }
        };

        var radsSet = _vm.SelectedRadionuclids.ToHashSet();

        if (!uint.TryParse(_vm.Quantity, out var quantityUintValue))
        {
            _vm.Quantity = string.Empty;
            _vm.ActivityToNormalizingD = string.Empty;
            _vm.Category = string.Empty;
            _vm.CategoryText = "Введено некорректное значение количества.";
            return;
        }

        if (radsSet.Count is 0)
        {
            _vm.ActivityToNormalizingD = string.Empty;
            _vm.Category = string.Empty;
            _vm.CategoryText = "Выберите радионуклиды из списка и заполните их активность.";
            return;
        }

        _ = CheckEquilibriumRads(radsSet);

        var activityValid = await CheckActivity(radsSet);
        if (!activityValid) return;

        if (radsSet.Any(x => string.Equals(x.D, "неограниченно", StringComparison.OrdinalIgnoreCase)))
        {
            _vm.ActivityToNormalizingD = string.Empty;
            _vm.Category = "5";
            _vm.CategoryText = SetCategoryText();
            return;
        }

        #region PolyActivity

        if (!_vm.IsSingleActivity || radsSet.Count == 1)
        {
            decimal adSum = 0;
            var countNonRadioactiveRads = 0;
            foreach (var nuclid in radsSet)
            {
                var currentActivity = _vm.IsSingleActivity ? _vm.Activity : nuclid.Activity;
                var nuclidFromR = _vm.RadionuclidsFullList
                    !.First(x => x.Name == nuclid.Name);

                var dFromR = ToExponentialString(nuclidFromR.D);

                if (!decimal.TryParse(ToExponentialString(dFromR),
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands |
                        NumberStyles.AllowLeadingSign,
                        new CultureInfo("ru-RU", false),
                        out var dValueFromR))
                {
                    _vm.ActivityToNormalizingD = string.Empty;
                    _vm.Category = string.Empty;
                    _vm.CategoryText = "Некорректное значение нормализующего фактора (D-величина) в справочнике.";
                    return;
                }

                if (!decimal.TryParse(ToExponentialString(nuclid.Mza),
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                        new CultureInfo("ru-RU", false),
                        out var mza))
                {
                    _vm.ActivityToNormalizingD = string.Empty;
                    _vm.Category = string.Empty;
                    _vm.CategoryText = "Некорректное значение МЗА в справочнике.";
                    return;
                }

                var activity = decimal.Parse(
                    ToExponentialString(currentActivity),
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                    new CultureInfo("ru-RU", false));

                var d = decimal.Multiply(dValueFromR, 1e12m);

                adSum += activity / quantityUintValue / d;

                if (activity < mza)
                {
                    countNonRadioactiveRads++;
                    if (countNonRadioactiveRads == radsSet.Count)
                    {
                        _vm.ActivityToNormalizingD = ToExponentialString(adSum.ToString(CultureInfo.CurrentCulture));
                        _vm.Category = string.Empty;
                        _vm.CategoryText = "Нерадиоактивный, активность ниже МЗА.";
                        return;
                    }
                }
            }

            for (short category = 1; category <= 5; category++)
            {
                if (dbBounds[category].Item1 <= adSum
                    && dbBounds[category].Item2 > adSum)
                {
                    _vm.ActivityToNormalizingD = ToExponentialString(adSum.ToString(CultureInfo.CurrentCulture));
                    _vm.Category = category.ToString();
                    _vm.CategoryText = SetCategoryText();
                    return;
                }
            }
        }

        #endregion

        #region SingleActivity

        else
        {
            var countNonRadioactiveRads = 0;
            List<decimal> dValueList = [];

            var activity = decimal.Parse(
                ToExponentialString(_vm.Activity),
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                new CultureInfo("ru-RU", false));

            foreach (var nuclid in radsSet)
            {
                var nuclidFromR = _vm.RadionuclidsFullList
                    !.First(x => x.Name == nuclid.Name);

                var dFromR = ToExponentialString(nuclidFromR.D);

                if (decimal.TryParse(ToExponentialString(dFromR),
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands |
                        NumberStyles.AllowLeadingSign,
                        new CultureInfo("ru-RU", false),
                        out var value))
                {
                    dValueList.Add(decimal.Multiply(value, 1e12m));
                }
                else
                {
                    _vm.ActivityToNormalizingD = string.Empty;
                    _vm.Category = string.Empty;
                    _vm.CategoryText = "Некорректное значение нормализующего фактора (D-величина) в справочнике.";
                    return;
                }

                if (!decimal.TryParse(ToExponentialString(nuclid.Mza),
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                        new CultureInfo("ru-RU", false),
                        out var mza))
                {
                    _vm.ActivityToNormalizingD = string.Empty;
                    _vm.Category = string.Empty;
                    _vm.CategoryText = "Некорректное значение МЗА в справочнике.";
                    return;
                }
                
                if (activity < mza)
                {
                    countNonRadioactiveRads++;
                    if (countNonRadioactiveRads == radsSet.Count)
                    {
                        _vm.ActivityToNormalizingD = string.Empty;
                        _vm.Category = string.Empty;
                        _vm.CategoryText = "Нерадиоактивный, активность ниже МЗА.";
                        return;
                    }
                }
            }

            if (dValueList.Count == 0) return;

            var dMinValue = dValueList.Min();
            var dMaxValue = dValueList.Max();

            activity /= quantityUintValue != 0
                ? quantityUintValue
                : 1.0m;

            var adMinBound = dMaxValue == 0.0m
                ? decimal.MaxValue
                : activity / dMaxValue;

            var adMaxBound = dMinValue == 0.0m
                ? decimal.MaxValue
                : activity / dMinValue;

            short minCategory = 0;
            short maxCategory = 0;
            for (short category = 1; category <= 5; category++)
            {
                if (adMaxBound >= dbBounds[category].Item1 && adMaxBound < dbBounds[category].Item2)
                {
                    maxCategory = category;
                }

                if (adMinBound >= dbBounds[category].Item1 && adMinBound < dbBounds[category].Item2)
                {
                    minCategory = category;
                }
            }

            _vm.ActivityToNormalizingD = string.Empty;
            if (minCategory == maxCategory)
            {
                _vm.Category = minCategory.ToString();
                _vm.CategoryText = SetCategoryText();
            }
            else
            {
                _vm.Category = string.Empty;
                _vm.CategoryText = $"Возможна категория опасности ЗРИ от {minCategory.ToString()} до {maxCategory.ToString()}.";
            }
        }

        #endregion
    }

    #region CheckActivity

    private async Task<bool> CheckActivity(HashSet<CalculatorRadionuclidDTO> radsSet)
    {
        bool activityValid;
        if (_vm.IsSingleActivity)
        {
            activityValid = decimal.TryParse(ToExponentialString(_vm.Activity),
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                new CultureInfo("ru-RU", false),
                out _);
        }
        else if (radsSet.Count == 1)
        {
            activityValid = decimal.TryParse(ToExponentialString(radsSet.First().Activity),
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                new CultureInfo("ru-RU", false),
                out _);
        }
        else
        {
            activityValid = radsSet.ToList().All(nuclid =>
                decimal.TryParse(ToExponentialString(nuclid.Activity),
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                    new CultureInfo("ru-RU", false),
                    out _));
        }

        if (!activityValid)
        {
            _vm.ActivityToNormalizingD = string.Empty;
            _vm.Category = string.Empty;
            _vm.CategoryText = "Некорректно заполнено поле активности радионуклида.";
        }

        return activityValid;
    }

    #endregion

    #region CheckEquilibriumRads

    /// <summary>
    /// Проверяет сет радионуклидов на равновесные, оставляет в нём только главные равновесные и неравновесные.
    /// </summary>
    /// <param name="radsSet">Сет радионуклидов.</param>
    /// <returns>CompletedTask.</returns>
    private static Task CheckEquilibriumRads(HashSet<CalculatorRadionuclidDTO> radsSet)
    {
        if (radsSet.Count <= 1) return Task.CompletedTask;

        foreach (var eqRadsString in EquilibriumRadionuclids)
        {
            var eqRadsSet = eqRadsString
                .Split(", ")
                .ToHashSet();

            var intersection = radsSet
                .Select(x => x.Name)
                .Intersect(eqRadsSet)
                .OrderByDescending(x => x == eqRadsSet.First())
                .ToHashSet();

            if (intersection.Count == eqRadsSet.Count)
            {
                radsSet.RemoveWhere(x => intersection.Skip(1).Contains(x.Name));
            }
        }

        return Task.CompletedTask;
    }

    #endregion

    #region SetCategoryText

    private string SetCategoryText()
    {
        return _vm.Category switch
        {
            "1" => "Чрезвычайно опасно для человека (A/D >= 1000)",
            "2" => "Очень опасно для человека (10 <= A/D < 1000)",
            "3" => "Опасно для человека (1 <= A/D < 10)",
            "4" => "Опасность для человека маловероятна (0,01 <= A/D < 1)",
            "5" => "Опасность для человека очень маловероятна (A/D < 0,01)",
            _ => string.Empty
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
                new CultureInfo("ru-RU", false),
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
    private protected static string ReplaceDashes(string value)
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
    protected static partial Regex DashesRegex();

    #endregion
}