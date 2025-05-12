using Client_App.ViewModels.Calculator;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Models.DTO;
using System.Text.RegularExpressions;

namespace Client_App.Commands.AsyncCommands.Calculator;

public partial class CategoryCalculationAsyncCommand : BaseAsyncCommand
{
    #region Properties

    private readonly CategoryCalculatorVM _categoryCalculatorVM;

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

    public CategoryCalculationAsyncCommand(CategoryCalculatorVM categoryCalculatorVM)
    {
        _categoryCalculatorVM = categoryCalculatorVM;
        _categoryCalculatorVM.PropertyChanged += CategoryCalculatorVMPropertyChanged;
    }

    #endregion

    #region PropertyChanged

    private void CategoryCalculatorVMPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is nameof(CategoryCalculatorVM.SelectedRadionuclids) 
            or nameof(CategoryCalculatorVM.Activity)
            or nameof(CategoryCalculatorVM.Quantity))
        {
            OnCanExecuteChanged();
        }
    }

    #endregion

    public override Task AsyncExecute(object? parameter)
    {
        var dbBounds = new Dictionary<short, (decimal, decimal)>
        {
            { 1, (1000, decimal.MaxValue) },
            { 2, (10, 1000) },
            { 3, (1, 10) },
            { 4, (0.01m, 1) },
            { 5, (0, 0.01m) }
        };
        var activity = ToExponentialString(_categoryCalculatorVM.Activity);
        if (!uint.TryParse(_categoryCalculatorVM.Quantity, out var quantityUintValue))
        {
            _categoryCalculatorVM.Quantity = string.Empty;
            return Task.CompletedTask;
        }
        
        var radsSet = _categoryCalculatorVM.SelectedRadionuclids.ToHashSet();

        List<decimal> dValueList = [];
        _ = CheckEquilibriumRads(radsSet);
        
        foreach (var nuclidName in radsSet.Select(x => x.Name))
        {
            var nuclidFromR = _categoryCalculatorVM.RadionuclidDictionary
                !.FirstOrDefault(x => x.Name == nuclidName);

            if (nuclidFromR is null) continue;
            var expFromR = ToExponentialString(nuclidFromR.D);
            if (decimal.TryParse(ToExponentialString(expFromR),
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
                    CultureInfo.CreateSpecificCulture("ru-RU"),
                    out var value))
            {
                dValueList.Add(decimal.Multiply(value, 1e12m));
            }
        }

        var dMinValue = dValueList.Min();
        var dMaxValue = dValueList.Max();
        var valid = decimal.TryParse(activity,
            NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands | NumberStyles.AllowLeadingSign,
            CultureInfo.CreateSpecificCulture("ru-RU"),
            out var aValue);
        aValue /= quantityUintValue != 0
            ? quantityUintValue
            : 1.0m;
        if (valid)
        {
            var adMinBound = dMaxValue == 0.0m
                ? decimal.MaxValue
                : aValue / dMaxValue;
            var adMaxBound = dMinValue == 0.0m
                ? decimal.MaxValue
                : aValue / dMinValue;

            for (short category = 1; category <= 5; category++)
            {
                if (dbBounds[category].Item1 <= adMinBound
                    && dbBounds[category].Item2 > adMaxBound)
                {
                    _categoryCalculatorVM.Category = category.ToString();
                    return Task.CompletedTask;
                }
            }
        }

        _categoryCalculatorVM.Category = string.Empty;
        return Task.CompletedTask;
    }

    #region CheckEquilibriumRads

    /// <summary>
    /// Проверяет сет радионуклидов на равновесные, оставляет в нём только главные и возвращает флаг, были ли в сете равновесные радионуклиды.
    /// </summary>
    /// <param name="radsSet">Сет радионуклидов.</param>
    /// <returns>Флаг, были ли в сете равновесные радионуклиды.</returns>
    private static bool CheckEquilibriumRads(HashSet<CalculatorRadionuclidDTO> radsSet)
    {
        var radsNameSet = radsSet
            .Select(x => x.Name)
            .ToHashSet();

        var isEqRads = false;
        if (radsNameSet.Count <= 1) return isEqRads;
        isEqRads = EquilibriumRadionuclids.All(x =>
        {
            x = x.Replace(" ", string.Empty);
            var eqSet = x.Split(',').ToHashSet();
            if (radsNameSet.Intersect(eqSet).Any())
            {
                isEqRads = true;
                radsNameSet.ExceptWith(eqSet.Skip(1));
            }
            return isEqRads;
        });
        return isEqRads;
    }

    #endregion

    #region ToExponentialString

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