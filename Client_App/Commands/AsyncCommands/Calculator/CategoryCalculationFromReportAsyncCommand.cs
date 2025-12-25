using Avalonia.Controls;
using Avalonia.Threading;
using DynamicData.Binding;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.DTO;
using Models.Forms;
using Models.Forms.Form1;
using Models.Interfaces;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Calculator;

public partial class CategoryCalculationFromReportAsyncCommand : BaseAsyncCommand
{
    #region Properties

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

    private static List<CalculatorRadionuclidDTO> R = [];

    #endregion

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> && parameter is not ObservableCollection<Form>) return;
        Form11 form = new();
        if (parameter is ObservableCollectionWithItemPropertyChanged<IKey> selectedForms)
        {
            if (selectedForms.Count != 1)
            {
                var msg = selectedForms.Count switch
                {
                    0 => "Не удалось выполнить расчёт категории. Не выбрана строчка.",
                    > 1 => "Не удалось выполнить расчёт категории. Выбрано более одной строчки.",
                    _ => string.Empty
                };

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Расчёт категории ЗРИ.",
                        ContentHeader = "Уведомление",
                        ContentMessage = msg,
                        MinWidth = 450,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                return;
            }
            form = (Form11)selectedForms.First();
        }
        if (parameter is ObservableCollection<Form> newSelectedForms)
        {
            if (newSelectedForms.Count != 1)
            {
                var msg = newSelectedForms.Count switch
                {
                    0 => "Не удалось выполнить расчёт категории. Не выбрана строчка.",
                    > 1 => "Не удалось выполнить расчёт категории. Выбрано более одной строчки.",
                    _ => string.Empty
                };

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Расчёт категории ЗРИ.",
                        ContentHeader = "Уведомление",
                        ContentMessage = msg,
                        MinWidth = 450,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                return;
            }
            form = (Form11)newSelectedForms.First();
        }

        R_Populate_From_File();

        var radsString = form.Radionuclids_DB;
        var radsSet = radsString
            .Split(';')
            .Select(x => x.Trim())
            .ToHashSet();

        if (form.Quantity_DB is not { } quantity)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Расчёт категории ЗРИ.",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Не удалось выполнить расчёт категории. Введено некорректное значение количества.",
                    MinWidth = 450,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            return;
        }

        if (radsSet.Count is 0)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Расчёт категории ЗРИ.",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Не удалось выполнить расчёт категории. Поле радионуклиды не заполнено.",
                    MinWidth = 450,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            return;
        }

        _ = CheckEquilibriumRads(radsSet);

        if (form.Activity_DB is not string activity 
            || !decimal.TryParse(ToExponentialString(activity), 
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                new CultureInfo("ru-RU", false),
                out var activityValue))
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Расчёт категории ЗРИ.",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Не удалось выполнить расчёт категории. Некорректно заполнена графа активности радионуклида.",
                    MinWidth = 450,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            return;
        }

        if (!radsSet
            .All(radName => R
                .Any(x => x.Name == radName)))
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Расчёт категории ЗРИ.",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Не удалось выполнить расчёт категории. " +
                                     "В графе радионуклидов присутствует нуклид, отсутствующий в справочнике.",
                    MinWidth = 450,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            return;
        }

        if (radsSet
            .Any(radName => R
                .Any(x => x.Name == radName && string.Equals(x.D, "неограниченно", StringComparison.OrdinalIgnoreCase))))
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Расчёт категории ЗРИ.",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Категория ЗРИ - 5. Опасность для человека очень маловероятна (A/D < 0,01).",
                    MinWidth = 450,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            return;
        }

        var countNonRadioactiveRads = 0;
        List<decimal> dValueList = [];

        foreach (var nuclid in radsSet)
        {
            var nuclidFromR = R.First(x => x.Name == nuclid);

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
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Расчёт категории ЗРИ.",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Не удалось выполнить расчёт категории. " +
                                         "Некорректное значение нормализующего фактора (D-величина) в справочнике.",
                        MinWidth = 450,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                return;
            }

            if (!decimal.TryParse(ToExponentialString(nuclidFromR.Mza),
                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowExponent | NumberStyles.AllowThousands,
                    new CultureInfo("ru-RU", false),
                    out var mza))
            {
                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Расчёт категории ЗРИ.",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Не удалось выполнить расчёт категории. Некорректное значение МЗА в справочнике.",
                        MinWidth = 450,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                return;
            }

            if (activityValue < mza)
            {
                countNonRadioactiveRads++;
                if (countNonRadioactiveRads == radsSet.Count)
                {
                    await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Расчёт категории ЗРИ.",
                            ContentHeader = "Уведомление",
                            ContentMessage = $"Нерадиоактивный, активность ниже МЗА ({nuclidFromR.Mza}).",
                            MinWidth = 450,
                            MinHeight = 150,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow));

                    return;
                }
            }
        }

        if (dValueList.Count == 0) return;

        var dMinValue = dValueList.Min();
        var dMaxValue = dValueList.Max();

        activityValue /= quantity != 0
            ? quantity
            : 1.0m;

        var adMinBound = dMaxValue == 0.0m
            ? decimal.MaxValue
            : activityValue / dMaxValue;

        var adMaxBound = dMinValue == 0.0m
            ? decimal.MaxValue
            : activityValue / dMinValue;

        short minCategory = 0;
        short maxCategory = 0;
        var dbBounds = new Dictionary<short, (decimal, decimal)>
        {
            { 1, (1000, decimal.MaxValue) },
            { 2, (10, 1000) },
            { 3, (1, 10) },
            { 4, (0.01m, 1) },
            { 5, (0, 0.01m) }
        };

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

        if (minCategory == maxCategory)
        {
            var category = minCategory.ToString();
            var msg = SetCategoryText(category);

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Расчёт категории ЗРИ.",
                    ContentHeader = "Уведомление",
                    ContentMessage = msg,
                    MinWidth = 450,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));
        }
        else
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Расчёт категории ЗРИ.",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Возможна категория опасности ЗРИ от {minCategory.ToString()} до {maxCategory.ToString()}.",
                    MinWidth = 450,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

        }
    }

    #region CheckEquilibriumRads

    /// <summary>
    /// Проверяет сет радионуклидов на равновесные, оставляет в нём только главные равновесные и неравновесные.
    /// </summary>
    /// <param name="radsSet">Сет радионуклидов.</param>
    /// <returns>CompletedTask.</returns>
    private static Task CheckEquilibriumRads(HashSet<string> radsSet)
    {
        if (radsSet.Count <= 1) return Task.CompletedTask;

        foreach (var eqRadsString in EquilibriumRadionuclids)
        {
            var eqRadsSet = eqRadsString
                .Split(", ")
                .ToHashSet();

            var intersection = radsSet
                .Intersect(eqRadsSet)
                .OrderByDescending(x => x == eqRadsSet.First())
                .ToHashSet();

            if (intersection.Count == eqRadsSet.Count)
            {
                radsSet.RemoveWhere(x => intersection.Skip(1).Contains(x));
            }
        }

        return Task.CompletedTask;
    }

    #endregion

    #region RFromFile

    private static void R_Populate_From_File()
    {

#if DEBUG
        var filePath = Path.Combine(Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\")), "data", "Spravochniki", "R.xlsx");
#else
        var filePath = Path.Combine(Path.GetFullPath(AppContext.BaseDirectory), "data", "Spravochniki", $"R.xlsx");
#endif

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        if (!File.Exists(filePath)) return;
        FileInfo excelImportFile = new(filePath);
        var xls = new ExcelPackage(excelImportFile);
        var worksheet = xls.Workbook.Worksheets["Лист1"];
        var i = 2;
        R.Clear();
        while (worksheet.Cells[i, 1].Text != string.Empty)
        {
            var name = worksheet.Cells[i, 1].Text;
            var abbreviation = worksheet.Cells[i, 4].Text;
            var halfLifeString = worksheet.Cells[i, 5].Text;
            var unit = worksheet.Cells[i, 6].Text;
            var d = worksheet.Cells[i, 15].Text;
            var mza = worksheet.Cells[i, 17].Text;

            if (double.TryParse(halfLifeString, out var halfLife)
                && !string.IsNullOrWhiteSpace(name)
                && !string.IsNullOrWhiteSpace(abbreviation)
                && !string.IsNullOrWhiteSpace(unit)
                && double.TryParse(mza, out _) 
                && (double.TryParse(d, out _) || string.Equals(d, "неограниченно", StringComparison.OrdinalIgnoreCase)))
            {
                R.Add(new CalculatorRadionuclidDTO
                {
                    Name = name,
                    Abbreviation = abbreviation,
                    D = d,
                    Halflife = halfLife,
                    Unit = unit,
                    Mza = mza
                });
            }
            i++;
        }
        R = [.. R.OrderBy(x => x.Name)];
    }

    #endregion

    #region SetCategoryText

    private static string SetCategoryText(string category)
    {
        return category switch
        {
            "1" => "Категория ЗРИ - 1. Чрезвычайно опасно для человека (A/D >= 1000)",
            "2" => "Категория ЗРИ - 2. Очень опасно для человека (10 <= A/D < 1000)",
            "3" => "Категория ЗРИ - 3. Опасно для человека (1 <= A/D < 10)",
            "4" => "Категория ЗРИ - 4. Опасность для человека маловероятна (0,01 <= A/D < 1)",
            "5" => "Категория ЗРИ - 5. Опасность для человека очень маловероятна (A/D < 0,01)",
            _ => "Не удалось выполнить расчёт категории."
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
