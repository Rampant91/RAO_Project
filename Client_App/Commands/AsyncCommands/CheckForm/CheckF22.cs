using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Properties;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Models.Helpers;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Проверка отчётов по форме 2.2. 
/// </summary>
public class CheckF22 : CheckBase
{
    public override bool CanExecute(object? parameter) => true;

    private static string? _dbWithForm1Prev;

    private const string Form15Plug = "!1,5";
    private const string FormGenericPlug = "!1,X";

    private static readonly Dictionary<string, string> GraphsList = new()
    {
        { "NumberInOrder_DB", "01 - № п/п" },
        { "RefineMachineName_DB", "02 - Пункт хранения - наименование" },
        { "MachineCode_DB", "03 - Пункт хранения - код" },
        { "MachinePower_DB", "04 - УКТ, упаковка или иная учетная единица - наименование" },
        { "NumberOfHoursPerYear_DB", "05 - УКТ, упаковка или иная учетная единица - тип" },
        { "CodeRAOIn_DB", "06 - УКТ, упаковка или иная учетная единица - количество, шт." },
        { "StatusRAOIn_DB", "07 - Код РАО" },
        { "VolumeIn_DB", "08 - Статус РАО" },
        { "MassIn_DB", "09 - Объем, куб.м - РАО без упаковки" },
        { "QuantityIn_DB", "10 - Объем, куб.м - РАО с упаковкой" },
        { "TritiumActivityIn_DB", "11 - Масса, т - РАО без упаковки (нетто)" },
        { "BetaGammaActivityIn_DB", "12 - Масса, т - РАО с упаковкой (брутто)" },
        { "AlphaActivityIn_DB", "13 - Количество ОЗИИИ, шт." },
        { "TransuraniumActivityIn_DB", "14 - Суммарная активность, Бк - тритий" },
        { "CodeRAOout_DB", "15 - Суммарная активность, Бк - бета-, гамма- излучающие радионуклиды (исключая тритий)" },
        { "StatusRAOout_DB", "16 - Суммарная активность, Бк - альфа-излучающие радионуклиды (исключая трансурановые)" },
        { "VolumeOut_DB", "17 - Суммарная активность, Бк - трансурановые радионуклиды" },
        { "MassOut_DB", "18 - Основные радионуклиды" },
        { "QuantityOZIIIout_DB", "19 - Субсидия, %" },
        { "TritiumActivityOut_DB", "20 - Номер мероприятия ФЦП" }
    };

    private static readonly List<string> ValidOperationCodesPlus =
    [
        "11", "12", "13", "14", "16", "18", "31", "32", "33", "34", "35", "36", "37", "38", "39", "41", "52", "55",
        "56", "57", "59", "73", "74", "75", "76", "85", "86", "88", "97"
    ];

    private static readonly List<string> ValidOperationCodesMinus =
    [
        "21", "22", "23", "24", "25", "26", "27", "28", "29", "42", "43", "44", "45", "46", "47", "48", "49", "51",
        "68", "71", "72", "84", "98"
    ];

    //each unit is identified as a unique combination of these values (keys);
    //setting any one to false ignores it when generating the unified key
    private const bool KeyInclude1 = true;  //storage name
    private const bool KeyInclude2 = true;  //storage code
    private const bool KeyInclude3 = false; //pack type
    private const bool KeyInclude4 = true;  //code RAO
    private const bool KeyInclude5 = true;  //status RAO
    private const bool KeyInclude6 = false; //FCP

    #region AsyncExecute

    public override async Task<List<CheckError>> AsyncExecute(object? parameter) 
        => await MainCheck(parameter).ConfigureAwait(false);

    #endregion

    #region MainCheck

    private static async Task<List<CheckError>> MainCheck(object? parameter, string? regNum = null)
    {
        var cts = new CancellationTokenSource();
        List<CheckError> errorList = [];
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var rep = parameter as Report;
        if (rep is null && regNum == null)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        
        var db = new DBModel(StaticConfiguration.DBPath);
        var db2 = new DBModel(StaticConfiguration.DBPath);

        var form20RegNo = regNum ?? rep!.Reports.Master_DB.RegNoRep.Value;
        var form20Okpo = rep!.Reports.Master_DB.OkpoRep.Value;

        var repYearText = rep.Year_DB?.ToString() ?? "";
        var repFormNum = rep.FormNum_DB;
        ObservableCollectionWithItemPropertyChanged<Form22> repRows22 = rep.Rows22;

        if (string.IsNullOrWhiteSpace(form20RegNo))
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        progressBarVM.SetProgressBar(5, "Поиск организации с формой 1.X для проверяемого рег. номера",
            $"Проверка {rep.Reports.Master_DB.RegNoRep.Value}_{rep.Reports.Master_DB.OkpoRep.Value}", "Проверка отчёта");

        var repsWithForm1Exist = await db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows10)
            .Where(reps => reps.DBObservable != null)
            .AnyAsync(reps => reps.Master_DB.Rows10
                .Any(form10 => form10.RegNo_DB == form20RegNo), cts.Token);

        if (!repsWithForm1Exist)
        {
            var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

            #region MessageFailedToOpenForm

            var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ButtonDefinitions = _dbWithForm1Prev == null ?
                    [
                        new ButtonDefinition { Name = "Выбрать файл БД", IsDefault = true },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ] 
                        :
                    [
                        new ButtonDefinition { Name = "Использовать ранее выбранный файл БД", IsDefault = true },
                        new ButtonDefinition { Name = "Выбрать файл БД" },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ],
                    CanResize = true,
                    ContentTitle = "Проверка формы",
                    ContentHeader = "Ошибка",
                    ContentMessage = "В текущей базе данных отсутствует форма 1.X для проверяемой организации." +
                                     $"{Environment.NewLine}Можете выбрать файл базы данных, содержащий форму 1.X для данной организации " +
                                     $"{Environment.NewLine}или операция проверки формы будет отменена.",
                    MinWidth = 400,
                    MinHeight = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(desktop.MainWindow));

            #endregion

            if (answer is not "Выбрать файл БД" and not "Использовать ранее выбранный файл БД")
            {
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            }

            OpenFileDialog dial = new() { AllowMultiple = false };
            var filter = new FileDialogFilter
            {
                Extensions = { "RAODB" }
            };
            dial.Filters = [filter];

            string[]? dbWithForm1;
            if (answer is "Использовать ранее выбранный файл БД" && _dbWithForm1Prev != null)
            {
                dbWithForm1 = [_dbWithForm1Prev];
            }
            else
            {
                dbWithForm1 = await dial.ShowAsync(desktop.MainWindow);
                if (dbWithForm1 is null)
                {
                    await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                }
            }
            var dbWithForm1FullPath = dbWithForm1![0];
            _dbWithForm1Prev = dbWithForm1FullPath;
            db = new DBModel(dbWithForm1FullPath);
        }

        progressBarVM.SetProgressBar(7, "Загрузка данных форм 1.5–1.8");

        var repsWithForm1Base = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows10)
            .Include(reps => reps.Report_Collection
                .Where(report =>
                    (report.FormNum_DB == "1.5" || report.FormNum_DB == "1.6" || report.FormNum_DB == "1.7" || report.FormNum_DB == "1.8")
                    && ((report.StartPeriod_DB.Length >= 4
                         && report.StartPeriod_DB.Substring(report.StartPeriod_DB.Length - 4) == repYearText)
                        || (report.EndPeriod_DB.Length >= 4
                            && report.EndPeriod_DB.Substring(report.EndPeriod_DB.Length - 4) == repYearText))))
            .ThenInclude(x => x.Rows15)
            .Include(reps => reps.Report_Collection).ThenInclude(report => report.Rows16)
            .Include(reps => reps.Report_Collection).ThenInclude(report => report.Rows17)
            .Include(reps => reps.Report_Collection).ThenInclude(report => report.Rows18)
            .Where(reps => reps.DBObservable != null);

        var forms1 = repsWithForm1Base.Where(reps => reps.Master_DB.Rows10.Any(form10 => form10.RegNo_DB == form20RegNo)).ToList();


        progressBarVM.SetProgressBar(8, "Формирование списка операций из форм 1.5–1.8 за текущий год");

        Reports? repsWithForm1;

        if (forms1.Count > 1)
        {
            List<string> okpoList = [];
            okpoList.AddRange(forms1.Select(form => form.Master_DB.Rows10.Last().Okpo_DB));
            bool fusion = true;
            if (fusion)
            {
                repsWithForm1 = new Reports();
                foreach (var okpo in okpoList)
                {
                    repsWithForm1.Report_Collection
                        .AddRange(repsWithForm1Base
                            .FirstOrDefaultAsync(reps => reps.Master_DB.Rows10
                                .Any(form10 => form10.RegNo_DB == form20RegNo && form10.Okpo_DB == okpo), cts.Token)
                            .Result!.Report_Collection);
                }
            }
            else
            {
                var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

                var buttons = okpoList
                    .Select(okpo => new ButtonDefinition
                    {
                        Name = okpo, IsDefault = true
                    })
                    .ToList();

                #region MessageMoreThanOneOrgFound

                var messageBoxCustomParams = new MessageBoxCustomParams
                {
                    ButtonDefinitions = buttons,
                    CanResize = true,
                    ContentTitle = "Проверка формы",
                    ContentHeader = "Внимание",
                    ContentMessage = $"Найдено более одной организации с рег. номером {form20RegNo}." +
                                         $"{Environment.NewLine}Пожалуйста, выберите нужную организацию по коду ОКПО.",
                    MinWidth = 400,
                    MinHeight = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                };

                var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxCustom(messageBoxCustomParams)
                    .ShowWindowDialogAsync(desktop.MainWindow));

                if (answer != null)
                {
                    repsWithForm1 = await repsWithForm1Base
                        .FirstOrDefaultAsync(reps => reps.Master_DB.Rows10
                            .Any(form10 => form10.RegNo_DB == form20RegNo && form10.Okpo_DB == answer), cts.Token);
                }
                else
                {
                    repsWithForm1 = await repsWithForm1Base
                        .FirstOrDefaultAsync(reps => reps.Master_DB.Rows10
                            .Any(form10 => form10.RegNo_DB == form20RegNo), cts.Token);
                }
            }
            #endregion
        }
        else
        {
            repsWithForm1 = await repsWithForm1Base
                .FirstOrDefaultAsync(reps => reps.Master_DB.Rows10
                    .Any(form10 => form10.RegNo_DB == form20RegNo), cts.Token);
        }

        progressBarVM.SetProgressBar(9, "Получение данных формы 2.2 за предыдущий год");

        var yearRealCurrent = rep.Year_DB ?? 0;
        var yearPrevious = yearRealCurrent - 1;
        var yearPreviousText = yearPrevious.ToString();

        var repsWithForm2 = await db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows20)
            .Include(reps => reps.Report_Collection
                .Where(report =>
                    (report.FormNum_DB == "2.2")
                    && (report.Year_DB == yearPrevious)))
            .ThenInclude(x => x.Rows22)
            .Where(reps => reps.DBObservable != null)
            .FirstOrDefaultAsync(reps => reps.Master_DB.Rows20
                .Any(form20 => form20.RegNo_DB == form20RegNo), cts.Token);

        await db.DisposeAsync();
        await db2.DisposeAsync();

        progressBarVM.SetProgressBar(10, "Проверка наличия организации в базе данных");
        if (repsWithForm1 is null)
        {
            #region MessageCheckFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {repFormNum}",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Не удалось проверить форму, поскольку в выбранном файле БД отсутствуют записи для организации {form20RegNo}_{form20Okpo}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        progressBarVM.SetProgressBar(10, "Преобразование данных форм 1.5–1.8");

        List<Form22> forms22ExpectedBase = [];
        List<(string, string, string)> forms22MetadataBase = [];
        Form17? formHeader17 = null;
        Form18? formHeader18 = null;

        double progress = 10;
        var incProgress = (20.0 - progress) / repsWithForm1!.Report_Collection.Count;
        var iterationCount = 0;

        foreach (var key in repsWithForm1!.Report_Collection)
        {
            progress += incProgress;
            iterationCount++;
            progressBarVM.SetProgressBar((int)progress, $"Преобразование данных форм 1.5–1.8 ({iterationCount}/{repsWithForm1!.Report_Collection.Count})");

            var report = (Report)key;
            Form22? form22New;
            switch (report.FormNum_DB)
            {
                case "1.5":
                {
                    report.Rows15 = new ObservableCollectionWithItemPropertyChanged<Form15>(report.Rows15
                            .OrderBy(x => x.NumberInOrder_DB));
                    foreach (var key1 in report.Rows15)
                    {
                        var form = (Form15)key1;
                        form22New = FormConvert(form, repYearText);
                        if (form22New != null)
                        {
                            forms22MetadataBase.Add((form22New.FormNum_DB, $"{report.StartPeriod_DB} - {report.EndPeriod_DB}", form22New.NumberInOrder_DB.ToString()));
                            forms22ExpectedBase.Add(form22New);
                        }
                    }
                    break;
                }
                case "1.6":
                {
                    report.Rows16 = new ObservableCollectionWithItemPropertyChanged<Form16>(report.Rows16
                        .OrderBy(x => x.NumberInOrder_DB));
                    foreach (var key1 in report.Rows16)
                    {
                        var form = (Form16)key1;
                        form22New = FormConvert(form, repYearText);
                        if (form22New != null)
                        {
                            //if (report.StartPeriod_DB == "28.06.2024" && form22New.NumberInOrder_DB == 3 && form22New.CodeRAO_DB == "20412200592") form22New.CodeRAO_DB = "21412200592";
                            forms22MetadataBase.Add((form22New.FormNum_DB, $"{report.StartPeriod_DB} - {report.EndPeriod_DB}", form22New.NumberInOrder_DB.ToString()));
                            forms22ExpectedBase.Add(form22New);
                        }
                    }
                    break;
                }
                case "1.7":
                {
                    report.Rows17 = new ObservableCollectionWithItemPropertyChanged<Form17>(report.Rows17
                        .OrderBy(x => x.NumberInOrder_DB));
                    foreach (var key1 in report.Rows17)
                    {
                        var form = (Form17)key1;
                        if (!DashStringHelper.IsNullOrWhiteSpaceOrDash(form.OperationCode_DB)) formHeader17 = form;
                        form22New = FormConvert(form, formHeader17, repYearText);
                        if (form22New != null)
                        {
                            forms22MetadataBase.Add((form22New.FormNum_DB, $"{report.StartPeriod_DB} - {report.EndPeriod_DB}", form22New.NumberInOrder_DB.ToString()));
                            forms22ExpectedBase.Add(form22New);
                        }
                    }
                    break;
                }
                case "1.8":
                {
                    report.Rows18 = new ObservableCollectionWithItemPropertyChanged<Form18>(report.Rows18
                        .OrderBy(x => x.NumberInOrder_DB));
                    foreach (var key1 in report.Rows18)
                    {
                        var form = (Form18)key1;
                        if (!DashStringHelper.IsNullOrWhiteSpaceOrDash(form.OperationCode_DB)) formHeader18 = form;
                        form22New = FormConvert(form, formHeader18, repYearText);
                        if (form22New != null)
                        {
                            forms22MetadataBase.Add((form22New.FormNum_DB, $"{report.StartPeriod_DB} - {report.EndPeriod_DB}", form22New.NumberInOrder_DB.ToString()));
                            forms22ExpectedBase.Add(form22New);
                        }
                    }
                    break;
                }
            }
        }

        progressBarVM.SetProgressBar(20, "Сборка и группировка строк формы 2.2");
        if (repsWithForm2 is { Report_Collection: not null })
        {
            progress = 20;
            incProgress = (25.0 - progress) / repsWithForm2.Report_Collection.Count;
            iterationCount = 0;

            foreach (var key in repsWithForm2.Report_Collection)
            {
                progress += incProgress;
                iterationCount++;
                progressBarVM.SetProgressBar((int)progress, $"Сборка и группировка строк формы 2.2 ({iterationCount}/{repsWithForm2.Report_Collection.Count})");

                var report = (Report)key;
                report.Rows22 = new ObservableCollectionWithItemPropertyChanged<Form22>(report.Rows22
                    .OrderBy(x => x.NumberInOrder_DB));
                foreach (var key1 in report.Rows22)
                {
                    var form = (Form22)key1;
                    var form22New = FormConvert(form, repYearText);
                    if (form22New != null)
                    {
                        forms22MetadataBase.Add((form22New.FormNum_DB, yearPreviousText, form22New.NumberInOrder_DB.ToString()));
                        forms22ExpectedBase.Add(form22New);
                    }
                }
                break;
            }
        }

        progressBarVM.SetProgressBar(25, "Добавление остатков предыдущего года из формы 2.2");

        Dictionary<(string, string, string, string, string, string), Form22> forms22ExpectedDict = [];
        Dictionary<(string, string, string, string, string, string), Form22> forms22RealDict = [];
        Dictionary<(string, string, string, string, string, string), Form22> forms22ExpectedSubDict = [];
        Dictionary<(string, string, string, string, string, string), Form22> forms22RealSubDict = [];
        Dictionary<(string, string, string, string, string, string), Dictionary<string, Dictionary<string, List<string>>>> forms22MetadataDict = [];

        progress = 25;
        incProgress = (35.0 - progress) / forms22ExpectedBase.Count;
        iterationCount = 0;

        for (var i = 0; i < forms22ExpectedBase.Count; i++)
        {
            progress += incProgress;
            iterationCount++;
            progressBarVM.SetProgressBar((int)progress, $"Добавление остатков предыдущего года из формы 2.2 ({iterationCount}/{forms22ExpectedBase.Count})");

            TryParseDoubleExtended(forms22ExpectedBase[i].Subsidy_DB.Replace("%", ""), out var subsidy);
            (string, string, string, string, string, string) key = (
                KeyInclude1 ? forms22ExpectedBase[i].StoragePlaceName_DB.Replace(" ", "").ToLower() : "",
                KeyInclude2 ? forms22ExpectedBase[i].StoragePlaceCode_DB : "",
                KeyInclude3 ? forms22ExpectedBase[i].PackType_DB.Replace(" ", "").ToLower() : "",
                KeyInclude4 ? forms22ExpectedBase[i].CodeRAO_DB : "",
                KeyInclude5 ? forms22ExpectedBase[i].StatusRAO_DB : "",
                KeyInclude6 ? forms22ExpectedBase[i].FcpNumber_DB : ""
            );

            if (!forms22ExpectedDict.TryGetValue(key, out var form22))
            {
                forms22ExpectedDict[key] = Form22_Copy(forms22ExpectedBase[i]);
                Form22_Subtract(forms22ExpectedDict[key], forms22ExpectedBase[i]);
            }
            if (forms22ExpectedDict.TryGetValue(key, out form22))
            {
                if (forms22ExpectedBase[i].NumberOfFields_DB < 0)
                {
                    Form22_Subtract(form22, forms22ExpectedBase[i]);
                }
                else
                {
                    Form22_Add(form22, forms22ExpectedBase[i]);
                }
            }
            if (subsidy >= 0)
            {
                var form22ExpectedBaseSub = Form22_CopySub(forms22ExpectedBase[i]);
                if (!forms22ExpectedSubDict.TryGetValue(key, out var form22Sub))
                {
                    forms22ExpectedSubDict[key] = Form22_Copy(form22ExpectedBaseSub);
                    Form22_Subtract(forms22ExpectedSubDict[key], forms22ExpectedSubDict[key]);
                }
                if (forms22ExpectedSubDict.TryGetValue(key, out form22Sub))
                {

                    if (form22ExpectedBaseSub.NumberOfFields_DB < 0)
                    {
                        Form22_Subtract(form22Sub, form22ExpectedBaseSub);
                    }
                    else
                    {
                        Form22_Add(form22Sub, form22ExpectedBaseSub);
                    }
                }
            }

            if (!forms22MetadataDict.TryGetValue(key, out var value))
            {
                value = [];
                forms22MetadataDict[key] = value;
            }
            if (!value.TryGetValue(forms22MetadataBase[i].Item1, out var value1))
            {
                value1 = [];
                value[forms22MetadataBase[i].Item1] = value1;
            }
            if (!value1.ContainsKey($"{forms22MetadataBase[i].Item2}"))
            {
                value1[forms22MetadataBase[i].Item2] = [];
            }

            value1[forms22MetadataBase[i].Item2].Add(forms22MetadataBase[i].Item3);
        }

        progressBarVM.SetProgressBar(35, "Построение словарей строк с учётом ключей");

        progress = 35;
        incProgress = (45.0 - progress) / repRows22.Count;
        iterationCount = 0;

        
        List<Form22> forms22Real = [];
        foreach (var key1 in repRows22)
        {
            progress += incProgress;
            iterationCount++;
            progressBarVM.SetProgressBar((int)progress, $"Построение словарей строк с учётом ключей ({iterationCount}/{repRows22.Count})");

            var form = (Form22)key1;

            if (string.IsNullOrWhiteSpace(form.FcpNumber_DB.Trim())) form.FcpNumber_DB = "-";

            TryParseDoubleExtended(form.Subsidy_DB.Replace("%", ""), out var subsidy);
            if (!DashStringHelper.IsNullOrWhiteSpaceOrDash(form.CodeRAO_DB)
                && !DashStringHelper.IsNullOrWhiteSpaceOrDash(form.StatusRAO_DB))
            {
                var key = (
                    KeyInclude1 ? form.StoragePlaceName_DB.Replace(" ", "").ToLower() : "",
                    KeyInclude2 ? form.StoragePlaceCode_DB : "",
                    KeyInclude3 ? form.PackType_DB.Replace(" ", "").ToLower() : "",
                    KeyInclude4 ? form.CodeRAO_DB : "",
                    KeyInclude5 ? form.StatusRAO_DB : "",
                    KeyInclude6 ? form.FcpNumber_DB : ""
                );
                if (!forms22RealDict.TryGetValue(key, out var value))
                {
                    forms22RealDict[key] = Form22_Copy(form);
                }
                else
                {
                    var errorValue = ItemName(form);
                    var errorMessage = $"В форме 2.2 уже присутствует строка с указанными РАО (строка {value.NumberInOrder_DB}). " +
                                       $"Следует объединить данные в соответствии с ЕОМУ (пункт 18.13, абзац 7).";
                    Form22_Add(value, form);
                    var errorDouble = errorList
                        .SingleOrDefault(x => string.Equals(errorValue, x.Value) 
                                              && string.Equals(errorMessage, x.Message));
                    if (errorDouble == null)
                    {
                        //errorList.Add(new CheckError
                        //{
                        //    FormNum = "form_22",
                        //    Row = form.NumberInOrder_DB.ToString(),
                        //    Column = "-",
                        //    Value = errorValue,
                        //    Message = errorMessage
                        //});
                    }
                    else
                    {
                        errorList[errorList.IndexOf(errorDouble)].Row += $", {form.NumberInOrder_DB}";
                    }
                }

                if (!(subsidy >= 0)) continue;

                var formSub = Form22_CopySub(form);
                if (!forms22RealSubDict.TryGetValue(key, out var value1))
                {
                    forms22RealSubDict[key] = Form22_Copy(formSub);
                }
                else
                {
                    Form22_Add(value1, formSub);
                    var errorValue = ItemName(formSub);
                    if (errorList.Any(x => x.Value == errorValue)) continue;
                        
                    var errorMessage = $"В форме 2.2 уже присутствует строка с указанными РАО (строка {forms22RealSubDict[key].NumberInOrder_DB})." +
                                       $" Следует объединить данные в соответствии с ЕОМУ (пункт 18.13, абзац 7).";
                    var errorDouble = errorList
                        .SingleOrDefault(x => string.Equals(errorValue, x.Value) 
                                              && string.Equals(errorMessage, x.Message));
                    if (errorDouble == null)
                    {
                        //errorList.Add(new CheckError
                        //{
                        //    FormNum = "form_22",
                        //    Row = formSub.NumberInOrder_DB.ToString(),
                        //    Column = "-",
                        //    Value = errorValue,
                        //    Message = errorMessage
                        //});
                    }
                    else
                    {
                        errorList[errorList.IndexOf(errorDouble)].Row += $", {form.NumberInOrder_DB}";
                    }
                }
            }
        }
        
        progressBarVM.SetProgressBar(45, "Обработка фактических строк формы 2.2 текущего отчёта");
        forms22Real = forms22RealDict.Keys.Select(key => forms22RealDict[key]).ToList();
        forms22Real = [];

        progress = 45;
        incProgress = (50.0 - progress) / forms22RealDict.Count;
        iterationCount = 0;
        
        foreach (var key in forms22RealDict.Keys)
        {
            progress += incProgress;
            iterationCount++;
            progressBarVM.SetProgressBar((int)progress, $"Обработка строк формы 2.2 текущего отчёта ({iterationCount}/{forms22RealDict.Count})");

            forms22Real.Add(forms22RealDict[key]);
        }

        progressBarVM.SetProgressBar(50, "Обработка строк формы 2.2 текущего отчёта");
        //the converted values should be compared to the rows in reps.
        List<(Form22, string, string)> forms22Expected = [];
        List<(Form22, string)> form15PlugDoubles = [];

        progress = 50;
        incProgress = (80.0 - progress) / forms22ExpectedDict.Count;
        iterationCount = 0;

        foreach (var key in forms22ExpectedDict.Keys)
        {
            progress += incProgress;
            iterationCount++;
            progressBarVM.SetProgressBar((int)progress, $"Обработка строк формы 2.2 текущего отчёта ({iterationCount}/{forms22ExpectedDict.Count})");

            List<string> addressSubstrings = [];
            List<string> formsSubstrings = [];
            foreach (var keyForm in forms22MetadataDict[key].Keys)
            {
                var addressSubstring = "";
                addressSubstring += $"форма {keyForm}: \r\n";
                formsSubstrings.Add(keyForm);
                var periods = forms22MetadataDict[key][keyForm].Keys.ToList();
                if (keyForm == "2.2")
                {
                    periods = new List<string>([yearPreviousText]);
                }
                else
                {
                    for (var i = 0; i < periods.Count; i++)
                        periods[i] = $"{periods[i]
                            .Substring(6, 4)}.{periods[i]
                            .Substring(3, 2)}.{periods[i][..2]}{periods[i][10..]}";
                    periods.Sort();
                    for (var i = 0; i < periods.Count; i++)
                        periods[i] = $"{periods[i]
                            .Substring(8, 2)}.{periods[i]
                            .Substring(5, 2)}.{periods[i][..4]}{periods[i][10..]}";
                }
                foreach (var keyPeriod in periods)
                {
                    addressSubstring += $"отчёт {keyPeriod}: ";
                    var lines = forms22MetadataDict[key][keyForm][keyPeriod].ToList();
                    List<int> linesReal = [];
                    linesReal.AddRange(lines.Select(int.Parse));
                    linesReal.Sort();
                    for (var i = linesReal.Count - 1; i > 1; i--)
                    {
                        if (linesReal[i] == linesReal[i - 1] + 1 && (linesReal[i - 1] == linesReal[i - 2] + 1
                            || (i < linesReal.Count - 1 && linesReal[i + 1] == -1)))
                        {
                            linesReal[i] = -1;
                        }
                    }
                    if (linesReal.Count >= 3 && linesReal[1] == linesReal[0] + 1 && linesReal[2] == -1) linesReal[1] = -1;
                    lines = [];
                    var lineRange = 0;
                    foreach (var line in linesReal)
                    {
                        if (line == -1) lineRange++;
                        else
                        {
                            if (lineRange > 0) lines[^1] = $"{lines[^1]} - {int.Parse(lines[^1]) + lineRange}";
                            lines.Add(line.ToString());
                            lineRange = 0;
                        }
                    }

                    lines[^1] = lineRange switch
                    {
                        > 1 => $"{lines[^1]} - {int.Parse(lines[^1]) + lineRange}",
                        > 0 => $"{lines[^1]}, {int.Parse(lines[^1]) + lineRange}",
                        _ => lines[^1]
                    };
                    addressSubstring += $"строк{(linesReal.Count == 1 ? "а" : "и")} {string.Join(", ", lines)} \r\n";
                }
                addressSubstrings.Add(addressSubstring);
            }
            var addressString = string.Join("; \r\n", addressSubstrings);
            var formsString = string.Join(", ", formsSubstrings);
            forms22Expected.Add((forms22ExpectedDict[key], addressString, formsString));
        }
        progressBarVM.SetProgressBar(80, "Сравнение ожидаемых и фактических данных");

        progress = 80;
        incProgress = (90.0 - progress) / forms22Real.Count;
        iterationCount = 0;

        foreach (var formReal in forms22Real)
        {
            progress += incProgress;
            iterationCount++;
            progressBarVM.SetProgressBar((int)progress, $"Сравнение ожидаемых и фактических данных ({iterationCount}/{forms22Real.Count})");

            var form15PlugItemName = ItemName(formReal, false);
            var form22RealPure = Form22_Copy(formReal);
            Form22_ToDecExp(form22RealPure);
            var matchFound = false;
            for (var i = forms22Expected.Count - 1; i >= 0; i--)
            {
                Form22 form22ExpectedPure = Form22_Copy(forms22Expected[i].Item1);
                Form22_ToDecExp(form22ExpectedPure);
                (Form22, string, string) form22Expected = (form22ExpectedPure, forms22Expected[i].Item2, forms22Expected[i].Item3);
                
                var mismatches = Form22_Match(
                    form22Expected.Item1, 
                    form22RealPure, 
                    $"форм{(form22Expected.Item3.Contains(',') ? "ы" : "а")} " + 
                    $"{form22Expected.Item3}" + 
                    $"{(form22Expected.Item3 == "2.2" ? " (" + yearPreviousText + ")" : "")}", 
                    $"форма 2.2 ({yearRealCurrent})", form22Expected.Item1.CodeRAO_DB == Form15Plug);

                if (mismatches == null)
                {
                    if (TryParseDoubleExtended(form22ExpectedPure.QuantityOZIII_DB, out var quantityVal) 
                        && Math.Abs(quantityVal) <= 1e-14
                    && TryParseDoubleExtended(form22ExpectedPure.VolumeOutOfPack_DB, out var volumeOutOfPackVal) 
                        && Math.Abs(volumeOutOfPackVal) <= 1e-14
                    && TryParseDoubleExtended(form22ExpectedPure.VolumeInPack_DB, out var volumeInPackDB) 
                        && Math.Abs(volumeInPackDB) <= 1e-14)
                    {
                        forms22Expected.RemoveAt(i);
                        matchFound = true;
                        continue;
                    }
                    else
                    {
                        continue;
                    }
                }
                matchFound = true;
                if (mismatches.Count > 0)
                {
                    List<int> listColumns = [];
                    List<string> listHints = [];
                    foreach (var mismatch in mismatches)
                    {
                        listColumns.Add(mismatch.Item1);
                        listHints.Add($"{mismatch.Item2}: {mismatch.Item3}, {mismatch.Item4}");
                    }
                    var columns = string.Join(", ", listColumns);
                    var hints = string.Join(";\n", listHints);
                    errorList.Add(new CheckError
                    {
                        FormNum = "form_22",
                        Row = form22RealPure.NumberInOrder_DB.ToString(),
                        Column = columns,
                        Value = ItemName(form22RealPure) +
                        $"\n\n{forms22Expected[i].Item2}",
                        Message = $"Сведения о РАО не совпадают:\n\n{hints}"
                    });
                }
                forms22Expected.RemoveAt(i);
                break;
            }
            if (!matchFound && form15PlugDoubles.All(x => x.Item2 != form15PlugItemName))
            {
                for (var i = forms22Expected.Count - 1; i >= 0; i--)
                {
                    var form22Expected = forms22Expected[i];

                    var mismatches = Form22_Match(
                        form22Expected.Item1, 
                        form22RealPure, 
                        $"форм{(forms22Expected[i].Item3.Contains(',') ? "ы" : "а")} " +
                        $"{form22Expected.Item3}" +
                        $"{(form22Expected.Item3 == "2.2" ? " (" + yearPreviousText + ")" : "")}", 
                        $"форма 2.2 ({yearRealCurrent})", 
                        form22Expected.Item1.CodeRAO_DB == Form15Plug, 
                        true);

                    if (mismatches == null) continue;
                    form15PlugDoubles.Add((form22Expected.Item1, form15PlugItemName));
                    matchFound = true;
                    if (mismatches.Count > 0)
                    {
                        List<int> listColumns = [];
                        List<string> listHints = [];
                        foreach (var mismatch in mismatches)
                        {
                            listColumns.Add(mismatch.Item1);
                            listHints.Add($"{mismatch.Item2}: {mismatch.Item3}, {mismatch.Item4}");
                        }
                        var columns = string.Join(", ", listColumns);
                        var hints = string.Join(";\n", listHints);
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_22",
                            Row = form22RealPure.NumberInOrder_DB.ToString(),
                            Column = columns,
                            Value = ItemName(form22RealPure) +
                            $"\n\n{forms22Expected[i].Item2}",
                            Message = $"Сведения о РАО не совпадают:\n\n{hints}"
                        });
                    }
                    forms22Expected.RemoveAt(i);
                    break;
                }
            }
            if (!matchFound && form15PlugDoubles.All(x => x.Item2 != form15PlugItemName))
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_22",
                    Row = form22RealPure.NumberInOrder_DB.ToString(),
                    Column = "-",
                    Value = ItemName(form22RealPure),
                    Message = $"В форме 2.2 ({yearPreviousText}) и в формах 1.5 - 1.8 ({yearRealCurrent}) " +
                              $"не найдена информация об указанных РАО."
                });
            }
        }
        progressBarVM.SetProgressBar(90, "Обработка не найденных в отчёте строк");

        progress = 90;
        incProgress = (95.0 - progress) / forms22Expected.Count;
        iterationCount = 0;

        foreach (var formExpected in forms22Expected)
        {
            progress += incProgress;
            iterationCount++;
            progressBarVM.SetProgressBar((int)progress, $"Обработка не найденных в отчёте строк ({iterationCount}/{forms22Expected.Count})");

            //if (int.TryParse(formExpected.Item1.PackQuantity_DB, out int packQuantity) && packQuantity == 0) continue;
            List<string> negatives = [];
            var nonZero = 0;
            if (TryParseDoubleExtended(formExpected.Item1.VolumeOutOfPack_DB, out var zeroCheck))
            {
                nonZero += (Math.Abs(zeroCheck) > 0.00001) ? 1 : 0;
                if (zeroCheck < -0.00001)
                    negatives.Add($"Объем без упаковки, куб. м: {zeroCheck}");
            }
            if (TryParseDoubleExtended(formExpected.Item1.VolumeInPack_DB, out zeroCheck))
            {
                nonZero += (Math.Abs(zeroCheck) > 0.00001) ? 1 : 0;
                if (zeroCheck < -0.00001)
                    negatives.Add($"Объем с упаковкой, куб. м: {zeroCheck}");
            }
            if (TryParseDoubleExtended(formExpected.Item1.MassOutOfPack_DB, out zeroCheck))
            {
                nonZero += (Math.Abs(zeroCheck) > 0.00001) ? 1 : 0;
                if (zeroCheck < -0.00001)
                    negatives.Add($"Масса без упаковки (нетто), т: {zeroCheck}");
            }
            if (TryParseDoubleExtended(formExpected.Item1.MassInPack_DB, out zeroCheck))
            {
                nonZero += (Math.Abs(zeroCheck) > 0.00001) ? 1 : 0;
                if (zeroCheck < -0.00001)
                    negatives.Add($"Масса с упаковкой (брутто), т: {zeroCheck}");
            }
            if (TryParseDoubleExtended(formExpected.Item1.TritiumActivity_DB, out zeroCheck))
            {
                nonZero += (Math.Abs(zeroCheck) > 0.00001) ? 1 : 0;
                if (zeroCheck < -0.00001)
                    negatives.Add($"Суммарная активность, Бк - тритий: {zeroCheck}");
            }
            if (TryParseDoubleExtended(formExpected.Item1.BetaGammaActivity_DB, out zeroCheck))
            {
                nonZero += (Math.Abs(zeroCheck) > 0.00001) ? 1 : 0;
                if (zeroCheck < -0.00001)
                    negatives.Add($"Суммарная активность, Бк - бета-, гамма- излучающие радионуклиды (исключая тритий): {zeroCheck}");
            }
            if (TryParseDoubleExtended(formExpected.Item1.AlphaActivity_DB, out zeroCheck))
            {
                nonZero += (Math.Abs(zeroCheck) > 0.00001) ? 1 : 0;
                if (zeroCheck < -0.00001)
                    negatives.Add($"Суммарная активность, Бк - альфа-излучающие радионуклиды (исключая трансурановые): {zeroCheck}");
            }
            if (TryParseDoubleExtended(formExpected.Item1.TransuraniumActivity_DB, out zeroCheck))
            {
                nonZero += (Math.Abs(zeroCheck) > 0.00001) ? 1 : 0;
                if (zeroCheck < -0.00001)
                    negatives.Add($"Суммарная активность, Бк - трансурановые: {zeroCheck}");
            }
            if (nonZero == 0) continue;
            if (negatives.Count > 0)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_22",
                    Row = "-",
                    Column = "-",
                    Value = ItemName(formExpected.Item1) +
                    $"\n\n{formExpected.Item2}",
                    Message = $"Для указанных РАО обнаружен отрицательный баланс на конец {yearRealCurrent} года:\n\n{string.Join(";\n", negatives)}"
                });
            }
            else
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_22",
                    Row = "-",
                    Column = "-",
                    Value = ItemName(formExpected.Item1) +
                    $"\n\n{formExpected.Item2}",
                    Message = $"В форме 2.2 ({yearRealCurrent}) не найдена информация об указанных РАО."
                });
            }
        }
        progressBarVM.SetProgressBar(95, "Формирование списка ошибок, сортировка и нумерация");
        errorList.Sort((i, j) =>
            int.TryParse(i.Row.Split(',')[0], out var iRowReal)
            && int.TryParse(j.Row.Split(',')[0], out var jRowReal)
                ? iRowReal - jRowReal
                : string.Compare(i.Row, j.Row));
        var index = 0;
        progressBarVM.SetProgressBar(95, "Уточнение наименований столбцов для отображения ошибок");

        progress = 95;
        incProgress = (100.0 - progress) / errorList.Count;
        iterationCount = 0;

        foreach (var error in errorList)
        {
            progress += incProgress;
            iterationCount++;
            progressBarVM.SetProgressBar((int)progress, $"Уточнение наименований столбцов для отображения ошибок ({iterationCount}/{errorList.Count})");
            if (GraphsList.TryGetValue(error.Column, out var columnFrontName))
            {
                error.Column = columnFrontName;
            }
            index++;
            error.Index = index;
        }

        progressBarVM.SetProgressBar(100, "Завершение проверки формы 2.2");
        await progressBar.CloseAsync();

        #region Check22ExportSummary

        if (Settings.Default.AppLaunchedInNorao)
        {
            var f22Expected = forms22ExpectedDict.Values.ToList();
            var f22Real = forms22RealDict.Values.ToList();
            var f221Expected = forms22ExpectedSubDict.Where(x => x.Key.Item5 == "1").Select(x => x.Value).ToList();
            var f221Real = forms22RealSubDict.Where(x => x.Key.Item5 == "1").Select(x => x.Value).ToList();
            var f22SubExpected = forms22ExpectedSubDict.Values.ToList();
            var f22SubReal = forms22RealSubDict.Values.ToList();
            await Check22ExportSummary(form20RegNo, f22Expected, f22Real, f221Expected, f221Real, f22SubExpected, f22SubReal, yearPreviousText, yearRealCurrent.ToString());
        }

        #endregion

        return errorList;
    }

    #endregion

    #region CancelCommandAndCloseProgressBarWindow

    /// <summary>
    /// Отмена исполняемой команды и закрытие окна прогрессбара.
    /// </summary>
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <returns></returns>
    private static async Task CancelCommandAndCloseProgressBarWindow(CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null)
    {
        await cts.CancelAsync();
        if (progressBar is not null) await progressBar.CloseAsync();
        cts.Token.ThrowIfCancellationRequested();
    }

    #endregion

    #region FormConvert

    private static Form22? FormConvert(Form15 form, string year)
    {
        /*List<string> validOperationCodesPlus = new([
            "11", "12", "13", "14", "16", "18",
            "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "41", "52", "73", "74", "75", "76", "88", "97"
        ]);
        List<string> ValidOperationCodesMinus = new([
            "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "44", "45", "49", "51", "68", "71", "98"
        ]);*/
        if (!ValidOperationCodesPlus.Contains(form.OperationCode_DB) && !ValidOperationCodesMinus.Contains(form.OperationCode_DB))    //filter out operation codes
        {
            return null;
        }
        if (!(form.OperationDate_DB.Length >= 4 && form.OperationDate_DB.Substring(form.OperationDate_DB.Length - 4) == year))
        {
            return null;    //the operation isn't from this year
        }
        Form22 res = new()
        {
            FormNum_DB = "1.5",
            NumberInOrder_DB = form.NumberInOrder_DB,
            CodeRAO_DB = (Form15Plug ?? "").Trim(),
            StatusRAO_DB = (form.StatusRAO_DB ?? "").Trim(),
            StoragePlaceCode_DB = (form.StoragePlaceCode_DB ?? "").Trim(),
            FcpNumber_DB = (form.FcpNumber_DB ?? "").Replace('.', ',').Trim(),
            StoragePlaceName_DB = (form.StoragePlaceName_DB ?? "").Trim(),
            PackName_DB = (form.PackName_DB ?? "").Trim(),
            PackType_DB = (form.PackType_DB ?? "").Trim(),
            PackQuantity_DB = "1",
            VolumeOutOfPack_DB = (Form15Plug ?? "").Trim(),
            VolumeInPack_DB = (Form15Plug ?? "").Trim(),
            MassOutOfPack_DB = (Form15Plug ?? "").Trim(),
            MassInPack_DB = (Form15Plug ?? "").Trim(),
            QuantityOZIII_DB = (form.Quantity_DB?.ToString() ?? "").Replace('.', ',').Trim(),
            TritiumActivity_DB = (Form15Plug ?? "").Trim(),
            BetaGammaActivity_DB = (Form15Plug ?? "").Trim(),
            AlphaActivity_DB = (Form15Plug ?? "").Trim(),
            TransuraniumActivity_DB = (Form15Plug ?? "").Trim(),
            MainRadionuclids_DB = (form.Radionuclids_DB ?? "").Trim(),
            Subsidy_DB = (form.Subsidy_DB ?? "").Replace('.', ',').Trim(),
        };
        if (string.IsNullOrWhiteSpace(res.FcpNumber_DB)) res.FcpNumber_DB = "-";
        int.TryParse(form.OperationCode_DB, out var directionMarker);
        if (ValidOperationCodesPlus.Contains(form.OperationCode_DB))
        {
            //plus
            res.NumberOfFields_DB = directionMarker;
            return res;
        }
        else if (ValidOperationCodesMinus.Contains(form.OperationCode_DB))
        {
            //minus
            res.NumberOfFields_DB = -directionMarker;
            return res;
        }
        return null;
    }

    private static Form22? FormConvert(Form16 form, string year)
    {
        /*List<string> validOperationCodesPlus = new([
            "11", "12", "13", "14", "16", "18",
            "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "41", "52", "56", "57", "59", "73", "74", "75", "76", "88", "97"
        ]);
        List<string> ValidOperationCodesMinus = new([
            "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "44", "45", "49", "51", "68", "71", "98"
        ]);*/
        if (!ValidOperationCodesPlus.Contains(form.OperationCode_DB) && !ValidOperationCodesMinus.Contains(form.OperationCode_DB))    //filter out operation codes
        {
            return null;
        }
        if (!(form.OperationDate_DB.Length >= 4 && form.OperationDate_DB.Substring(form.OperationDate_DB.Length - 4) == year))
        {
            return null;    //the operation isn't from this year
        }
        if (DashStringHelper.IsNullOrWhiteSpaceOrDash(form.CodeRAO_DB) && DashStringHelper.IsNullOrWhiteSpaceOrDash(form.StatusRAO_DB))
        {
            return null;    //header line
        }
        Form22 res = new()
        {
            FormNum_DB = "1.6",
            NumberInOrder_DB = form.NumberInOrder_DB,
            CodeRAO_DB = (form.CodeRAO_DB ?? "").Trim(),
            StatusRAO_DB = (form.StatusRAO_DB ?? "").Trim(),
            StoragePlaceCode_DB = (form.StoragePlaceCode_DB ?? "").Trim(),
            FcpNumber_DB = (form.FcpNumber_DB ?? "").Replace('.',',').Trim(),
            StoragePlaceName_DB = (form.StoragePlaceName_DB ?? "").Trim(),
            PackName_DB = (form.PackName_DB ?? "").Trim(),
            PackType_DB = (form.PackType_DB ?? "").Trim(),
            PackQuantity_DB = "1",
            VolumeOutOfPack_DB = (form.Volume_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            VolumeInPack_DB = (FormGenericPlug ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassOutOfPack_DB = (form.Mass_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassInPack_DB = (FormGenericPlug ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            QuantityOZIII_DB = (form.QuantityOZIII_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TritiumActivity_DB = (form.TritiumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            BetaGammaActivity_DB = (form.BetaGammaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            AlphaActivity_DB = (form.AlphaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TransuraniumActivity_DB = (form.TransuraniumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MainRadionuclids_DB = (form.MainRadionuclids_DB ?? "").Trim(),
            Subsidy_DB = (form.Subsidy_DB ?? "").Replace('.', ',').Trim(),
        };
        if (string.IsNullOrWhiteSpace(res.FcpNumber_DB)) res.FcpNumber_DB = "-";
        int.TryParse(form.OperationCode_DB, out var directionMarker);
        if (ValidOperationCodesPlus.Contains(form.OperationCode_DB))
        {
            //plus
            res.NumberOfFields_DB = directionMarker;
            return res;
        }
        else if (ValidOperationCodesMinus.Contains(form.OperationCode_DB))
        {
            //minus
            res.NumberOfFields_DB = -directionMarker;
            return res;
        }
        return null;
    }

    private static Form22? FormConvert(Form17 form, Form17? formHeader, string year)
    {
        var formTrue = formHeader ?? form;
        //List<string> validOperationCodesPlus = new(["12", "18", "31", "32", "33", "34", "35", "36", "37", "38", "39", "52", "55"]);
        //List<string> ValidOperationCodesMinus = new(["21", "22", "23", "24", "25", "26", "27", "28", "29", "51"]);
        if (DashStringHelper.IsNullOrWhiteSpaceOrDash(form.CodeRAO_DB))
        {
            return null;    //empty line
        }
        if (!ValidOperationCodesPlus.Contains(formTrue.OperationCode_DB) && !ValidOperationCodesMinus.Contains(formTrue.OperationCode_DB))
        {
            return null;    //filter out operation codes
        }
        if (!(formTrue.OperationDate_DB.Length >= 4 && formTrue.OperationDate_DB.Substring(formTrue.OperationDate_DB.Length - 4) == year))
        {
            return null;    //the operation isn't from this year
        }
        if (DashStringHelper.IsNullOrWhiteSpaceOrDash(form.CodeRAO_DB) && DashStringHelper.IsNullOrWhiteSpaceOrDash(form.StatusRAO_DB))
        {
            return null;    //header line
        }
        Form22 res = new()
        {
            FormNum_DB = "1.7",
            NumberInOrder_DB = form.NumberInOrder_DB,
            CodeRAO_DB = (form.CodeRAO_DB ?? "").Trim(),
            StatusRAO_DB = (form.StatusRAO_DB ?? "").Trim(),
            StoragePlaceCode_DB = (formTrue.StoragePlaceCode_DB ?? "").Trim(),
            FcpNumber_DB = (form.FcpNumber_DB ?? "").Replace('.', ',').Trim(),
            StoragePlaceName_DB = (formTrue.StoragePlaceName_DB ?? "").Trim(),
            PackName_DB = (form.PackName_DB ?? "").Trim(),
            PackType_DB = (form.PackType_DB ?? "").Trim(),
            PackQuantity_DB = "1",
            VolumeOutOfPack_DB = (form.VolumeOutOfPack_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            VolumeInPack_DB = (form.Volume_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassOutOfPack_DB = (form.MassOutOfPack_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassInPack_DB = (form.Mass_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            QuantityOZIII_DB = (form.Quantity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TritiumActivity_DB = (form.TritiumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            BetaGammaActivity_DB = (form.BetaGammaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            AlphaActivity_DB = (form.AlphaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TransuraniumActivity_DB = (form.TransuraniumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MainRadionuclids_DB = (form.Radionuclids_DB ?? "").Trim(),
            Subsidy_DB = (form.Subsidy_DB ?? "").Replace('.', ',').Trim(),
        };
        if (string.IsNullOrWhiteSpace(res.FcpNumber_DB)) res.FcpNumber_DB = "-";
        int.TryParse(formTrue.OperationCode_DB, out var directionMarker);
        if (ValidOperationCodesPlus.Contains(formTrue.OperationCode_DB))
        {
            //plus
            res.NumberOfFields_DB = directionMarker;
            return res;
        }
        else if (ValidOperationCodesMinus.Contains(formTrue.OperationCode_DB))
        {
            //minus
            res.NumberOfFields_DB = -directionMarker;
            return res;
        }
        return null;
    }

    private static Form22? FormConvert(Form18 form, Form18? formHeader, string year)
    {
        var formTrue = formHeader ?? form;
        //List<string> validOperationCodesPlus = new(["12", "18", "31", "32", "33", "34", "35", "36", "37", "38", "39", "52", "55"]);
        //List<string> ValidOperationCodesMinus = new(["21", "22", "23", "24", "25", "26", "27", "28", "29", "51"]);
        if (DashStringHelper.IsNullOrWhiteSpaceOrDash(form.CodeRAO_DB))
        {
            return null;    //empty line
        }
        if (!ValidOperationCodesPlus.Contains(formTrue.OperationCode_DB) && !ValidOperationCodesMinus.Contains(formTrue.OperationCode_DB))
        {
            return null;    //filter out operation codes
        }
        if (!(formTrue.OperationDate_DB.Length >= 4 && formTrue.OperationDate_DB.Substring(formTrue.OperationDate_DB.Length - 4) == year))
        {
            return null;    //the operation isn't from this year
        }
        if (DashStringHelper.IsNullOrWhiteSpaceOrDash(form.CodeRAO_DB) && DashStringHelper.IsNullOrWhiteSpaceOrDash(form.StatusRAO_DB))
        {
            return null;    //header line
        }
        Form22 res = new()
        {
            FormNum_DB = "1.8",
            NumberInOrder_DB = form.NumberInOrder_DB,
            CodeRAO_DB = (form.CodeRAO_DB ?? "").Trim(),
            StatusRAO_DB = (form.StatusRAO_DB ?? "").Trim(),
            StoragePlaceCode_DB = (formTrue.StoragePlaceCode_DB ?? "").Trim(),
            FcpNumber_DB = (form.FcpNumber_DB ?? "").Replace('.', ',').Trim(),
            StoragePlaceName_DB = (formTrue.StoragePlaceName_DB ?? "").Trim(),
            PackName_DB = (FormGenericPlug ?? "").Trim(),
            PackType_DB = (FormGenericPlug ?? "").Trim(),
            PackQuantity_DB = "1",
            VolumeOutOfPack_DB = (form.Volume20_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            VolumeInPack_DB = (form.Volume6_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassOutOfPack_DB = (form.Mass21_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassInPack_DB = (form.Mass7_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            QuantityOZIII_DB = (FormGenericPlug ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TritiumActivity_DB = (form.TritiumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            BetaGammaActivity_DB = (form.BetaGammaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            AlphaActivity_DB = (form.AlphaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TransuraniumActivity_DB = (form.TransuraniumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MainRadionuclids_DB = (form.Radionuclids_DB ?? "").Trim(),
            Subsidy_DB = (form.Subsidy_DB ?? "").Replace('.', ',').Trim(),
        };
        if (string.IsNullOrWhiteSpace(res.FcpNumber_DB)) res.FcpNumber_DB = "-";
        int.TryParse(formTrue.OperationCode_DB, out var directionMarker);
        if (ValidOperationCodesPlus.Contains(formTrue.OperationCode_DB))
        {
            //plus
            res.NumberOfFields_DB = directionMarker;
            return res;
        }
        else if (ValidOperationCodesMinus.Contains(formTrue.OperationCode_DB))
        {
            //minus
            res.NumberOfFields_DB = -directionMarker;
            return res;
        }
        return null;
    }
    private static Form22? FormConvert(Form22 form, string year)
    {
        if (DashStringHelper.IsNullOrWhiteSpaceOrDash(form.CodeRAO_DB) && DashStringHelper.IsNullOrWhiteSpaceOrDash(form.StatusRAO_DB))
        {
            return null;    //header line
        }
        Form22 res = new()
        {
            FormNum_DB = "2.2",
            NumberInOrder_DB = form.NumberInOrder_DB,
            NumberOfFields_DB = 1,
            CodeRAO_DB = (form.CodeRAO_DB ?? "").Trim(),
            StatusRAO_DB = (form.StatusRAO_DB ?? "").Trim(),
            StoragePlaceCode_DB = (form.StoragePlaceCode_DB ?? "").Trim(),
            FcpNumber_DB = (form.FcpNumber_DB ?? "").Replace('.', ',').Trim(),
            StoragePlaceName_DB = (form.StoragePlaceName_DB ?? "").Trim(),
            PackName_DB = (form.PackName_DB ?? "").Trim(),
            PackType_DB = (form.PackType_DB ?? "").Trim(),
            PackQuantity_DB = (form.PackQuantity_DB ?? "").Trim(),
            VolumeOutOfPack_DB = (form.VolumeOutOfPack_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            VolumeInPack_DB = (form.VolumeInPack_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassOutOfPack_DB = (form.MassOutOfPack_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassInPack_DB = (form.MassInPack_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            QuantityOZIII_DB = (form.QuantityOZIII_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TritiumActivity_DB = (form.TritiumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            BetaGammaActivity_DB = (form.BetaGammaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            AlphaActivity_DB = (form.AlphaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TransuraniumActivity_DB = (form.TransuraniumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MainRadionuclids_DB = (form.MainRadionuclids_DB ?? "").Trim(),
            Subsidy_DB = (form.Subsidy_DB ?? "").Trim(),
        };
        if (string.IsNullOrWhiteSpace(res.FcpNumber_DB)) res.FcpNumber_DB = "-";
        return res;
    }

    #endregion

    #region Form22_Copy

    private static Form22 Form22_Copy(Form22 form, string? inOrOutParam = null)
    {
        try
        {

            if (form == null)
            {
                #region MessageCopyFailed

                Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = $"Проверка формы 2.2",
                        ContentHeader = "Ошибка",
                        ContentMessage = $"Ошибка при копировании строки формы 2.2\n" +
                        $"Не удалось получить строку",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).ShowWindowDialogAsync(Desktop.MainWindow));

                #endregion
                throw new ArgumentNullException(nameof(form)); 
            }

            if (string.IsNullOrWhiteSpace(form.FcpNumber_DB)) form.FcpNumber_DB = "-";

            Form22 res = new()
            {
                NumberInOrder_DB = form.NumberInOrder_DB,
                NumberOfFields_DB = form.NumberOfFields_DB,
                FormNum_DB = form.FormNum_DB?.Trim() ?? "",
                CodeRAO_DB = form.CodeRAO_DB?.Trim() ?? "",
                StatusRAO_DB = form.StatusRAO_DB?.Trim() ?? "",
                StoragePlaceCode_DB = form.StoragePlaceCode_DB?.Trim() ?? "",
                FcpNumber_DB = form.FcpNumber_DB.Replace('.', ',').Trim() ?? "",
                StoragePlaceName_DB = form.StoragePlaceName_DB?.Trim() ?? "",
                PackName_DB = form.PackName_DB?.Trim() ?? "",
                PackType_DB = form.PackType_DB?.Trim() ?? "",
                PackQuantity_DB = form.PackQuantity_DB?.Trim() ?? "",
                VolumeOutOfPack_DB = form.VolumeOutOfPack_DB?.Trim() ?? "",
                VolumeInPack_DB = form.VolumeInPack_DB?.Trim() ?? "",
                MassOutOfPack_DB = form.MassOutOfPack_DB?.Trim() ?? "",
                MassInPack_DB = form.MassInPack_DB?.Trim() ?? "",
                QuantityOZIII_DB = form.QuantityOZIII_DB?.Trim() ?? "",
                TritiumActivity_DB = form.TritiumActivity_DB?.Trim() ?? "",
                BetaGammaActivity_DB = form.BetaGammaActivity_DB?.Trim() ?? "",
                AlphaActivity_DB = form.AlphaActivity_DB?.Trim() ?? "",
                TransuraniumActivity_DB = form.TransuraniumActivity_DB?.Trim() ?? "",
                MainRadionuclids_DB = form.MainRadionuclids_DB?.Trim() ?? "",
                Subsidy_DB = form.Subsidy_DB?.Trim() ?? "",
            };

            return res;
        }
        catch (ArgumentNullException argumentNullException)
        {
            throw argumentNullException;
        }
        catch (Exception ex)
        {
            #region MessageCopyFailed

            Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы 2.2",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"Ошибка во время копирования строки №{form?.NumberInOrder_DB} отчета по форме {form?.FormNum_DB}\n" +
                    $"Проверьте строку №{form?.NumberInOrder_DB} на правильность заполнения\n" +
                    $"Дополнительная информация об ошибке:\n" +
                    $"{ex.Message}\n",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion
            throw ex;
        }
    }

    #endregion

    #region Form22_CopySub

    private static Form22 Form22_CopySub(Form22 form, string? inOrOutParam = null)
    {
        Form22 res = Form22_Copy(form, inOrOutParam);
        TryParseDoubleExtended(res.VolumeOutOfPack_DB, out double volumeDouble);
        TryParseDoubleExtended(res.QuantityOZIII_DB, out double quantityDouble);
        TryParseDoubleExtended(res.Subsidy_DB, out double subsidyDouble);
        subsidyDouble /= 100.0;
        volumeDouble *= subsidyDouble;
        quantityDouble *= subsidyDouble;
        res.VolumeOutOfPack_DB = volumeDouble.ToString();
        res.QuantityOZIII_DB = quantityDouble.ToString();
        return res;
    }

    #endregion

    #region Form22_Add

    private static void Form22_Add(Form22 receiver, Form22 giver)
    {
        bool form15Touch = receiver.CodeRAO_DB == Form15Plug || giver.CodeRAO_DB == Form15Plug;
        if (form15Touch) receiver.CodeRAO_DB = Form15Plug;
        receiver.VolumeOutOfPack_DB = Form22_SubAdd(receiver.VolumeOutOfPack_DB, giver.VolumeOutOfPack_DB);
        receiver.VolumeInPack_DB = Form22_SubAdd(receiver.VolumeInPack_DB, giver.VolumeInPack_DB);
        receiver.MassOutOfPack_DB = Form22_SubAdd(receiver.MassOutOfPack_DB, giver.MassOutOfPack_DB);
        receiver.MassInPack_DB = Form22_SubAdd(receiver.MassInPack_DB, giver.MassInPack_DB);
        receiver.QuantityOZIII_DB = Form22_SubAdd(receiver.QuantityOZIII_DB, giver.QuantityOZIII_DB);
        receiver.TritiumActivity_DB = Form22_SubAdd(receiver.TritiumActivity_DB, giver.TritiumActivity_DB);
        receiver.BetaGammaActivity_DB = Form22_SubAdd(receiver.BetaGammaActivity_DB, giver.BetaGammaActivity_DB);
        receiver.AlphaActivity_DB = Form22_SubAdd(receiver.AlphaActivity_DB, giver.AlphaActivity_DB);
        receiver.TransuraniumActivity_DB = Form22_SubAdd(receiver.TransuraniumActivity_DB, giver.TransuraniumActivity_DB);
        //receiver.PackQuantity_DB = Form22_SubAdd(receiver.PackQuantity_DB, giver.PackQuantity_DB);
        if (string.IsNullOrWhiteSpace(receiver.FcpNumber_DB)) receiver.FcpNumber_DB = "-";
    }
    private static void Form22_Subtract(Form22 giver, Form22 taker)
    {
        bool form15Touch = giver.CodeRAO_DB == Form15Plug || taker.CodeRAO_DB == Form15Plug;
        if (form15Touch) giver.CodeRAO_DB = Form15Plug;
        giver.VolumeOutOfPack_DB = Form22_SubSubtract(giver.VolumeOutOfPack_DB, taker.VolumeOutOfPack_DB);
        giver.VolumeInPack_DB = Form22_SubSubtract(giver.VolumeInPack_DB, taker.VolumeInPack_DB);
        giver.MassOutOfPack_DB = Form22_SubSubtract(giver.MassOutOfPack_DB, taker.MassOutOfPack_DB);
        giver.MassInPack_DB = Form22_SubSubtract(giver.MassInPack_DB, taker.MassInPack_DB);
        giver.QuantityOZIII_DB = Form22_SubSubtract(giver.QuantityOZIII_DB, taker.QuantityOZIII_DB);
        giver.TritiumActivity_DB = Form22_SubSubtract(giver.TritiumActivity_DB, taker.TritiumActivity_DB);
        giver.BetaGammaActivity_DB = Form22_SubSubtract(giver.BetaGammaActivity_DB, taker.BetaGammaActivity_DB);
        giver.AlphaActivity_DB = Form22_SubSubtract(giver.AlphaActivity_DB, taker.AlphaActivity_DB);
        giver.TransuraniumActivity_DB = Form22_SubSubtract(giver.TransuraniumActivity_DB, taker.TransuraniumActivity_DB);
        //giver.PackQuantity_DB = Form22_SubSubtract(giver.PackQuantity_DB, taker.PackQuantity_DB);
        if (string.IsNullOrWhiteSpace(giver.FcpNumber_DB)) giver.FcpNumber_DB = "-";
    }

    /// <summary>
    /// Tries parsing the two parameters and assigning their sum to the return value, converting it back to string afterward.
    /// If the parsing fails, returns the first parameter as is.
    /// </summary>
    /// <param name="receiver">The first parameter</param>
    /// <param name="giver">The second parameter</param>
    /// <returns>A string representation of the sum of the parameters. If the summation fails, returns the first parameter.</returns>
    private static string Form22_SubAdd(string receiver, string giver)
    {
        if (receiver == FormGenericPlug || giver == FormGenericPlug) return FormGenericPlug;
        if (receiver == Form15Plug || giver == Form15Plug) return Form15Plug;
        var receiverRealRaw = DashStringHelper.IsNullOrWhiteSpaceOrDash(receiver)
            ? "0" : receiver;
        var giverRealRaw = DashStringHelper.IsNullOrWhiteSpaceOrDash(giver)
            ? "0" : giver;
        double receiverReal = decimal.TryParse(receiverRealRaw, out var receiverDecimal)
            ? (double)receiverDecimal
            : TryParseDoubleExtended(receiverRealRaw, out var receiverTrue)
            ? receiverTrue
            : 0;
        double giverReal = decimal.TryParse(giverRealRaw, out var giverDecimal)
            ? (double)giverDecimal
            : TryParseDoubleExtended(giverRealRaw, out var giverTrue)
            ? giverTrue
            : 0;
        return (receiverReal + giverReal).ToString();
    }

    /// <summary>
    /// Tries parsing the two parameters and assigning their difference to the return value, converting it back to string afterward.
    /// If the parsing fails, returns the first parameter as is.
    /// </summary>
    /// <param name="giver">The first parameter</param>
    /// <param name="taker">The second parameter</param>
    /// <returns>A string representation of the difference of the parameters. If the subtraction fails, returns the first parameter.</returns>
    private static string Form22_SubSubtract(string giver, string taker)
    {
        if (giver == FormGenericPlug || taker == FormGenericPlug) return FormGenericPlug;
        if (giver == Form15Plug || taker == Form15Plug) return Form15Plug;
        var giverRealRaw = DashStringHelper.IsNullOrWhiteSpaceOrDash(giver)
            ? "0" : giver;
        var takerRealRaw = DashStringHelper.IsNullOrWhiteSpaceOrDash(taker)
            ? "0" : taker;
        double takerReal = decimal.TryParse(takerRealRaw, out var takerDecimal)
            ? (double)takerDecimal
            : TryParseDoubleExtended(takerRealRaw, out var takerTrue)
            ? takerTrue
            : 0;
        double giverReal = decimal.TryParse(giverRealRaw, out var giverDecimal)
            ? (double)giverDecimal
            : TryParseDoubleExtended(giverRealRaw, out var giverTrue)
            ? giverTrue
            : 0;
        return (giverReal - takerReal).ToString();
    }
    private static string Form22_SubToExp(string input)
    {
        string inputReal = input.Replace('.', ',').Trim();
        if (TryParseDoubleExtended(inputReal, out var inputValueDouble))
        {
            return inputValueDouble.ToString("E5").Replace("+0", "+").Replace("-0", "-");
        }
        else
        {
            return inputReal;
        }
    }
    private static void Form22_ToDecExp(Form22 form, string? direction = null)
    {
        form.VolumeOutOfPack_DB = Form22_SubToDec(form.VolumeOutOfPack_DB);
        form.VolumeInPack_DB = Form22_SubToDec(form.VolumeInPack_DB);
        form.MassOutOfPack_DB = Form22_SubToDec(form.MassOutOfPack_DB);
        form.MassInPack_DB = Form22_SubToDec(form.MassInPack_DB);
        form.QuantityOZIII_DB = Form22_SubToDec(form.QuantityOZIII_DB,0);
        form.TritiumActivity_DB = Form22_SubToExp(form.TritiumActivity_DB);
        form.BetaGammaActivity_DB = Form22_SubToExp(form.BetaGammaActivity_DB);
        form.AlphaActivity_DB = Form22_SubToExp(form.AlphaActivity_DB);
        form.TransuraniumActivity_DB = Form22_SubToExp(form.TransuraniumActivity_DB);
        //form.PackQuantity_DB = Form22_SubToDec(form.PackQuantity_DB,0);
    }
    private static string Form22_SubToDec(string input,int precision = 5)
    {
        string inputReal = input.Replace('.', ',').Trim();
        if (decimal.TryParse(inputReal, out var inputValueDecimal))
        {
            return inputValueDecimal.ToString($"F{precision}");
        }
        if (TryParseDoubleExtended(inputReal, out var inputValueDouble))
        {
            return ((decimal)inputValueDouble).ToString($"F{precision}");
        }
        else
        {
            return inputReal;
        }
    }

    #endregion

    #region Form22_SubMatch

    private static void Form22_SubMatchDec(string form1Val, string form2Val, string humanName, double valB, List<(int, string, string, string)> res, int columnNum, string forms1, string forms2, bool form15Fix)
    {
        if (form1Val == Form15Plug || form2Val == Form15Plug) return;
        if (form1Val == FormGenericPlug || form2Val == FormGenericPlug) return;
        if (decimal.TryParse(form1Val, out var val1Dec)
            && decimal.TryParse(form2Val, out var val2Dec))
        {
            if (!decimal.Equals(val1Dec, val2Dec) && !(form15Fix && decimal.Compare(val1Dec, val2Dec) < 0))
            {
                res.Add((columnNum, $"{humanName}", $"{forms1}: {form1Val}", $"{forms2}: {form2Val}"));
                return;
            }
            else
            {
                return;
            }
        }
        TryParseDoubleExtended(form1Val, out var val1);
        TryParseDoubleExtended(form2Val, out var val2);
        val1 = Math.Round(val1, 5);
        val2 = Math.Round(val2, 5);
        if (!((DashStringHelper.IsDash(form1Val) && DashStringHelper.IsDash(form2Val))
              || (val1 < 0.00001 && DashStringHelper.IsDash(form2Val))
              || (DashStringHelper.IsDash(form1Val) && val2 < 0.00001)
              || val1 >= val2 * (1.0 - valB)
              && val2 >= val1 * (1.0 - valB)))
        {
            res.Add((columnNum, $"{humanName}", $"{forms1}: {Form22_SubToDec(form1Val)}", $"{forms2}: {Form22_SubToDec(form2Val)}"));
        }
    }
    private static void Form22_SubMatchExp(string form1Val, string form2Val, string humanName, double valB, List<(int, string, string, string)> res, int columnNum, string forms1, string forms2, bool form15Fix, bool allowLesser = false)
    {
        if (form1Val == Form15Plug || form2Val == Form15Plug) return;
        if (form1Val == FormGenericPlug || form2Val == FormGenericPlug) return;
        if (decimal.TryParse(form1Val, out var val1Dec)
            && decimal.TryParse(form2Val, out var val2Dec))
        {
            if ((val1Dec >= val2Dec * (decimal)(1.0 - valB) || val2Dec >= val1Dec * (decimal)(1.0 - valB)) && !(form15Fix && decimal.Compare(val1Dec, val2Dec) < 0) && !(allowLesser && decimal.Compare(val1Dec, val2Dec) > 0))
            {
                res.Add((columnNum, $"{humanName}", $"{Form22_SubToExp(form1Val)}: {form1Val}", $"{Form22_SubToExp(form2Val)}: {form2Val}"));
                return;
            }
            else
            {
                return;
            }
        }
        TryParseDoubleExtended(form1Val, out var val1);
        TryParseDoubleExtended(form2Val, out var val2);
        val1 = Math.Round(val1, 5);
        val2 = Math.Round(val2, 5);
        if (!((DashStringHelper.IsDash(form1Val) && DashStringHelper.IsDash(form2Val))
              || (val1 < 0.00001 && DashStringHelper.IsDash(form2Val))
              || (DashStringHelper.IsDash(form1Val) && val2 < 0.00001)
              || val1 >= val2 * (1.0 - valB)
              && val2 >= val1 * (1.0 - valB))
              && !(allowLesser && val1 > val2))
        {
            res.Add((columnNum, $"{humanName}", $"{forms1}: {Form22_SubToExp(form1Val)}", $"{forms2}: {Form22_SubToExp(form2Val)}"));
        }
    }

    #endregion

    #region Form22_Match

    private static List<(int, string, string, string)>? Form22_Match(Form22 form1, Form22 form2, string forms1, string forms2, bool form15Fix, bool form15PlugLeftover = false)
    {
        const double valB = 0.00001;
        const double valBact = 0.05;
        List<(int, string, string, string)> res = [];
        if (!KeyInclude4 || (form1.CodeRAO_DB == form2.CodeRAO_DB || (form15PlugLeftover && (form1.CodeRAO_DB == Form15Plug || form2.CodeRAO_DB == Form15Plug)))
           )
        {
            if ((!KeyInclude5 || form1.StatusRAO_DB.Trim() == form2.StatusRAO_DB.Trim())
                && (!KeyInclude1 || form1.StoragePlaceName_DB.Replace(" ", "").ToLower() == form2.StoragePlaceName_DB.Replace(" ", "").ToLower())
                && (!KeyInclude2 || form1.StoragePlaceCode_DB.Trim() == form2.StoragePlaceCode_DB.Trim())
                && (!KeyInclude3 || form1.PackType_DB.Replace(" ","").ToLower() == form2.PackType_DB.Replace(" ", "").ToLower())
                && (!KeyInclude6 || form1.FcpNumber_DB.Replace('-',' ').Trim().TrimEnd('0') == form2.FcpNumber_DB.Replace('-', ' ').Trim().TrimEnd('0')))
            {
                //Form22_SubMatchDec(form1.PackQuantity_DB, form2.PackQuantity_DB, "УКТ, упаковки или иная учетная единица - количество, шт.", valB, res, 6, forms1, forms2, form15Fix);
                Form22_SubMatchDec(form1.VolumeOutOfPack_DB, form2.VolumeOutOfPack_DB, "Объем без упаковки, куб. м", valB, res, 9, forms1, forms2, form15Fix);
                Form22_SubMatchDec(form1.VolumeInPack_DB, form2.VolumeInPack_DB, "Объем с упаковкой, куб. м", valB, res, 10, forms1, forms2, form15Fix);
                Form22_SubMatchDec(form1.MassOutOfPack_DB, form2.MassOutOfPack_DB, "Масса без упаковки (нетто), т", valB, res, 11, forms1, forms2, form15Fix);
                Form22_SubMatchDec(form1.MassInPack_DB, form2.MassInPack_DB, "Масса с упаковкой (брутто), т", valB, res, 12, forms1, forms2, form15Fix);
                Form22_SubMatchDec(form1.QuantityOZIII_DB, form2.QuantityOZIII_DB, "Количество ОЗИИИ, шт.", valB, res, 13, forms1, forms2, form15Fix);
                Form22_SubMatchExp(form1.TritiumActivity_DB, form2.TritiumActivity_DB, "Суммарная активность, Бк - тритий", valBact, res, 14, forms1, forms2, form15Fix, true);
                Form22_SubMatchExp(form1.BetaGammaActivity_DB, form2.BetaGammaActivity_DB, "Суммарная активность, Бк - бета-, гамма- излучающие радионуклиды (исключая тритий)", valBact, res, 15, forms1, forms2, form15Fix, true);
                Form22_SubMatchExp(form1.AlphaActivity_DB, form2.AlphaActivity_DB, "Суммарная активность, Бк - альфа-излучающие радионуклиды (исключая трансурановые)", valBact, res, 16, forms1, forms2, form15Fix, true);
                Form22_SubMatchExp(form1.TransuraniumActivity_DB, form2.TransuraniumActivity_DB, "Суммарная активность, Бк - трансурановые", valBact, res, 17, forms1, forms2, form15Fix, true);
                return res;
            }
            return null;
        }
        return null;
    }

    #endregion

    #region ItemName

    private static string ItemName(Form22 item, bool includeCodeRAO = true)
    {
        List<string> result = new();
        if (KeyInclude1) result.Add($"наименование пункта хранения {item.StoragePlaceName_DB}");
        if (KeyInclude2) result.Add($"код пункта хранения {item.StoragePlaceCode_DB}");
        if (KeyInclude3) result.Add($"тип упаковки {item.PackType_DB}");
        if (KeyInclude4 && includeCodeRAO) result.Add($"код РАО {(item.CodeRAO_DB is Form15Plug or FormGenericPlug ? "-" : item.CodeRAO_DB)}");
        if (KeyInclude5) result.Add($"статус РАО {item.StatusRAO_DB}");
        if (KeyInclude6) result.Add($"номер мероприятия ФЦП {item.FcpNumber_DB}");
        if (result.Count > 0) result[0] = $"{result[0][..1].ToUpper()}{result[0][1..]}";
        return string.Join(", ", result);
    }

    #endregion

    #region Check22ExportSummary

    private static async Task Check22ExportSummary(string regNum, List<Form22> f22Expected, List<Form22> f22Real, List<Form22> f221Expected, 
        List<Form22> f221Real, List<Form22> f22SubExpected, List<Form22> f22SubReal, string yearPrev, string yearCur)
    {
        Dictionary<(string, string), Dictionary<string, double>> rows = [];
        foreach (var form22 in f22Expected)
        {
            TryParseDoubleExtended(form22.VolumeOutOfPack_DB, out var doubleVolumeOutOfPack_DB);
            TryParseDoubleExtended(form22.QuantityOZIII_DB, out var doubleQuantityOZIII_DB);
            TryParseDoubleExtended(form22.MassOutOfPack_DB, out var doubleMassOutOfPack_DB);
            TryParseDoubleExtended(form22.AlphaActivity_DB, out var doubleActivityA_DB);
            TryParseDoubleExtended(form22.BetaGammaActivity_DB, out var doubleActivityBG_DB);
            TryParseDoubleExtended(form22.TransuraniumActivity_DB, out var doubleActivityU_DB);
            TryParseDoubleExtended(form22.TritiumActivity_DB, out var doubleActivityT_DB);
            var doubleActivity_DB = doubleActivityA_DB + doubleActivityBG_DB + doubleActivityU_DB + doubleActivityT_DB;
            Dictionary<string, double> values = new()
            {
                { "VolumeOutOfPack_DB_TE", doubleVolumeOutOfPack_DB },
                { "QuantityOZIII_DB_TE", doubleQuantityOZIII_DB },
                { "MassOutOfPack_DB_TE", doubleMassOutOfPack_DB },
                { "Activity_DB_TE", doubleActivity_DB },
            };
            var key = (form22.StoragePlaceName_DB, form22.StoragePlaceCode_DB);
            if (!rows.TryGetValue(key, out var value))
            {
                value = [];
                rows.Add(key, value);
                rows[key].Add("VolumeOutOfPack_DB_TE", 0);
                rows[key].Add("VolumeOutOfPack_DB_TR", 0);
                rows[key].Add("MassOutOfPack_DB_TE", 0);
                rows[key].Add("MassOutOfPack_DB_TR", 0);
                rows[key].Add("Activity_DB_TE", 0);
                rows[key].Add("Activity_DB_TR", 0);
                rows[key].Add("VolumeOutOfPack_DB_1E", 0);
                rows[key].Add("VolumeOutOfPack_DB_1R", 0);
                rows[key].Add("VolumeOutOfPack_DB_SE", 0);
                rows[key].Add("VolumeOutOfPack_DB_SR", 0);
                rows[key].Add("QuantityOZIII_DB_TE", 0);
                rows[key].Add("QuantityOZIII_DB_TR", 0);
                rows[key].Add("QuantityOZIII_DB_1E", 0);
                rows[key].Add("QuantityOZIII_DB_1R", 0);
                rows[key].Add("QuantityOZIII_DB_SE", 0);
                rows[key].Add("QuantityOZIII_DB_SR", 0);
            }

            value["VolumeOutOfPack_DB_TE"] += values["VolumeOutOfPack_DB_TE"];
            value["QuantityOZIII_DB_TE"] += values["QuantityOZIII_DB_TE"];
            value["MassOutOfPack_DB_TE"] += values["MassOutOfPack_DB_TE"];
            value["Activity_DB_TE"] += values["Activity_DB_TE"];
        }
        foreach (var form22 in f22Real)
        {
            TryParseDoubleExtended(form22.VolumeOutOfPack_DB, out var doubleVolumeOutOfPack_DB);
            TryParseDoubleExtended(form22.QuantityOZIII_DB, out var doubleQuantityOZIII_DB);
            TryParseDoubleExtended(form22.MassOutOfPack_DB, out var doubleMassOutOfPack_DB);
            TryParseDoubleExtended(form22.AlphaActivity_DB, out var doubleActivityA_DB);
            TryParseDoubleExtended(form22.BetaGammaActivity_DB, out var doubleActivityBG_DB);
            TryParseDoubleExtended(form22.TransuraniumActivity_DB, out var doubleActivityU_DB);
            TryParseDoubleExtended(form22.TritiumActivity_DB, out var doubleActivityT_DB);
            var doubleActivity_DB = doubleActivityA_DB + doubleActivityBG_DB + doubleActivityU_DB + doubleActivityT_DB;
            Dictionary<string, double> values = new()
            {
                { "VolumeOutOfPack_DB_TR", doubleVolumeOutOfPack_DB },
                { "QuantityOZIII_DB_TR", doubleQuantityOZIII_DB },
                { "MassOutOfPack_DB_TR", doubleMassOutOfPack_DB },
                { "Activity_DB_TR", doubleActivity_DB },
            };
            var key = (form22.StoragePlaceName_DB, form22.StoragePlaceCode_DB);
            if (!rows.TryGetValue(key, out var value))
            {
                value = [];
                rows.Add(key, value);
                rows[key].Add("VolumeOutOfPack_DB_TE", 0);
                rows[key].Add("VolumeOutOfPack_DB_TR", 0);
                rows[key].Add("MassOutOfPack_DB_TE", 0);
                rows[key].Add("MassOutOfPack_DB_TR", 0);
                rows[key].Add("Activity_DB_TE", 0);
                rows[key].Add("Activity_DB_TR", 0);
                rows[key].Add("VolumeOutOfPack_DB_1E", 0);
                rows[key].Add("VolumeOutOfPack_DB_1R", 0);
                rows[key].Add("VolumeOutOfPack_DB_SE", 0);
                rows[key].Add("VolumeOutOfPack_DB_SR", 0);
                rows[key].Add("QuantityOZIII_DB_TE", 0);
                rows[key].Add("QuantityOZIII_DB_TR", 0);
                rows[key].Add("QuantityOZIII_DB_1E", 0);
                rows[key].Add("QuantityOZIII_DB_1R", 0);
                rows[key].Add("QuantityOZIII_DB_SE", 0);
                rows[key].Add("QuantityOZIII_DB_SR", 0);
            }

            value["VolumeOutOfPack_DB_TR"] += values["VolumeOutOfPack_DB_TR"];
            value["QuantityOZIII_DB_TR"] += values["QuantityOZIII_DB_TR"];
            value["MassOutOfPack_DB_TR"] += values["MassOutOfPack_DB_TR"];
            value["Activity_DB_TR"] += values["Activity_DB_TR"];
        }
        foreach (var form22 in f221Expected)
        {
            TryParseDoubleExtended(form22.VolumeOutOfPack_DB, out var doubleVolumeOutOfPack_DB);
            TryParseDoubleExtended(form22.QuantityOZIII_DB, out var doubleQuantityOZIII_DB);
            Dictionary<string, double> values = new()
            {
                { "VolumeOutOfPack_DB_1E", doubleVolumeOutOfPack_DB },
                { "QuantityOZIII_DB_1E", doubleQuantityOZIII_DB }
            };
            var key = (form22.StoragePlaceName_DB, form22.StoragePlaceCode_DB);
            if (!rows.TryGetValue(key, out var value))
            {
                value = [];
                rows.Add(key, value);
                rows[key].Add("VolumeOutOfPack_DB_TE", 0);
                rows[key].Add("VolumeOutOfPack_DB_TR", 0);
                rows[key].Add("MassOutOfPack_DB_TE", 0);
                rows[key].Add("MassOutOfPack_DB_TR", 0);
                rows[key].Add("Activity_DB_TE", 0);
                rows[key].Add("Activity_DB_TR", 0);
                rows[key].Add("VolumeOutOfPack_DB_1E", 0);
                rows[key].Add("VolumeOutOfPack_DB_1R", 0);
                rows[key].Add("VolumeOutOfPack_DB_SE", 0);
                rows[key].Add("VolumeOutOfPack_DB_SR", 0);
                rows[key].Add("QuantityOZIII_DB_TE", 0);
                rows[key].Add("QuantityOZIII_DB_TR", 0);
                rows[key].Add("QuantityOZIII_DB_1E", 0);
                rows[key].Add("QuantityOZIII_DB_1R", 0);
                rows[key].Add("QuantityOZIII_DB_SE", 0);
                rows[key].Add("QuantityOZIII_DB_SR", 0);
            }

            value["VolumeOutOfPack_DB_1E"] += values["VolumeOutOfPack_DB_1E"];
            value["QuantityOZIII_DB_1E"] += values["QuantityOZIII_DB_1E"];
        }
        foreach (var form22 in f221Real)
        {
            TryParseDoubleExtended(form22.VolumeOutOfPack_DB, out var doubleVolumeOutOfPack_DB);
            TryParseDoubleExtended(form22.QuantityOZIII_DB, out var doubleQuantityOZIII_DB);
            Dictionary<string, double> values = new()
            {
                { "VolumeOutOfPack_DB_1R", doubleVolumeOutOfPack_DB },
                { "QuantityOZIII_DB_1R", doubleQuantityOZIII_DB }
            };
            var key = (form22.StoragePlaceName_DB, form22.StoragePlaceCode_DB);
            if (!rows.TryGetValue(key, out var value))
            {
                value = [];
                rows.Add(key, value);
                rows[key].Add("VolumeOutOfPack_DB_TE", 0);
                rows[key].Add("VolumeOutOfPack_DB_TR", 0);
                rows[key].Add("MassOutOfPack_DB_TE", 0);
                rows[key].Add("MassOutOfPack_DB_TR", 0);
                rows[key].Add("Activity_DB_TE", 0);
                rows[key].Add("Activity_DB_TR", 0);
                rows[key].Add("VolumeOutOfPack_DB_1E", 0);
                rows[key].Add("VolumeOutOfPack_DB_1R", 0);
                rows[key].Add("VolumeOutOfPack_DB_SE", 0);
                rows[key].Add("VolumeOutOfPack_DB_SR", 0);
                rows[key].Add("QuantityOZIII_DB_TE", 0);
                rows[key].Add("QuantityOZIII_DB_TR", 0);
                rows[key].Add("QuantityOZIII_DB_1E", 0);
                rows[key].Add("QuantityOZIII_DB_1R", 0);
                rows[key].Add("QuantityOZIII_DB_SE", 0);
                rows[key].Add("QuantityOZIII_DB_SR", 0);
            }

            value["VolumeOutOfPack_DB_1R"] += values["VolumeOutOfPack_DB_1R"];
            value["QuantityOZIII_DB_1R"] += values["QuantityOZIII_DB_1R"];
        }
        foreach (var form22 in f22SubExpected)
        {
            TryParseDoubleExtended(form22.VolumeOutOfPack_DB, out var doubleVolumeOutOfPack_DB);
            TryParseDoubleExtended(form22.QuantityOZIII_DB, out var doubleQuantityOZIII_DB);
            Dictionary<string, double> values = new()
            {
                { "VolumeOutOfPack_DB_SE", doubleVolumeOutOfPack_DB },
                { "QuantityOZIII_DB_SE", doubleQuantityOZIII_DB }
            };
            var key = (form22.StoragePlaceName_DB, form22.StoragePlaceCode_DB);
            if (!rows.ContainsKey(key))
            {
                rows.Add(key, new Dictionary<string, double>());
                rows[key].Add("VolumeOutOfPack_DB_TE", 0);
                rows[key].Add("VolumeOutOfPack_DB_TR", 0);
                rows[key].Add("MassOutOfPack_DB_TE", 0);
                rows[key].Add("MassOutOfPack_DB_TR", 0);
                rows[key].Add("Activity_DB_TE", 0);
                rows[key].Add("Activity_DB_TR", 0);
                rows[key].Add("VolumeOutOfPack_DB_1E", 0);
                rows[key].Add("VolumeOutOfPack_DB_1R", 0);
                rows[key].Add("VolumeOutOfPack_DB_SE", 0);
                rows[key].Add("VolumeOutOfPack_DB_SR", 0);
                rows[key].Add("QuantityOZIII_DB_TE", 0);
                rows[key].Add("QuantityOZIII_DB_TR", 0);
                rows[key].Add("QuantityOZIII_DB_1E", 0);
                rows[key].Add("QuantityOZIII_DB_1R", 0);
                rows[key].Add("QuantityOZIII_DB_SE", 0);
                rows[key].Add("QuantityOZIII_DB_SR", 0);
            }
            rows[key]["VolumeOutOfPack_DB_SE"] += values["VolumeOutOfPack_DB_SE"];
            rows[key]["QuantityOZIII_DB_SE"] += values["QuantityOZIII_DB_SE"];
        }
        foreach (var form22 in f22SubReal)
        {
            TryParseDoubleExtended(form22.VolumeOutOfPack_DB, out var doubleVolumeOutOfPack_DB);
            TryParseDoubleExtended(form22.QuantityOZIII_DB, out var doubleQuantityOZIII_DB);
            Dictionary<string, double> values = new()
            {
                { "VolumeOutOfPack_DB_SR", doubleVolumeOutOfPack_DB },
                { "QuantityOZIII_DB_SR", doubleQuantityOZIII_DB }
            };
            var key = (form22.StoragePlaceName_DB, form22.StoragePlaceCode_DB);
            if (!rows.TryGetValue(key, out var value))
            {
                value = [];
                rows.Add(key, value);
                rows[key].Add("VolumeOutOfPack_DB_TE", 0);
                rows[key].Add("VolumeOutOfPack_DB_TR", 0);
                rows[key].Add("MassOutOfPack_DB_TE", 0);
                rows[key].Add("MassOutOfPack_DB_TR", 0);
                rows[key].Add("Activity_DB_TE", 0);
                rows[key].Add("Activity_DB_TR", 0);
                rows[key].Add("VolumeOutOfPack_DB_1E", 0);
                rows[key].Add("VolumeOutOfPack_DB_1R", 0);
                rows[key].Add("VolumeOutOfPack_DB_SE", 0);
                rows[key].Add("VolumeOutOfPack_DB_SR", 0);
                rows[key].Add("QuantityOZIII_DB_TE", 0);
                rows[key].Add("QuantityOZIII_DB_TR", 0);
                rows[key].Add("QuantityOZIII_DB_1E", 0);
                rows[key].Add("QuantityOZIII_DB_1R", 0);
                rows[key].Add("QuantityOZIII_DB_SE", 0);
                rows[key].Add("QuantityOZIII_DB_SR", 0);
            }

            value["VolumeOutOfPack_DB_SR"] += values["VolumeOutOfPack_DB_SR"];
            value["QuantityOZIII_DB_SR"] += values["QuantityOZIII_DB_SR"];
        }

        var cts = new CancellationTokenSource();
        var fileName = $"Таблица_{regNum}_{BaseVM.DbFileName}_{Assembly.GetExecutingAssembly().GetName().Version}";
        var (fullPath, openTemp) = await ExcelGetFullPath(fileName, cts);

        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        var xls = new ExcelPackage(new FileInfo(fullPath));
        var worksheet1 = xls.Workbook.Worksheets.Add("Таблица");
        worksheet1.Cells[1, 1].SetCellValue(0, 0, "№");
        worksheet1.Cells[1, 2].SetCellValue(0, 0, "Наименование пункта хранения");
        worksheet1.Cells[1, 3].SetCellValue(0, 0, "Код пункта хранения");
        worksheet1.Cells[1, 4].SetCellValue(0, 0, $"Объем {yearCur} (всего, ожидаемое значение {yearPrev} год + опер. за {yearCur})");
        worksheet1.Cells[1, 5].SetCellValue(0, 0, $"Объем {yearCur} (всего, фактическое значение)");
        worksheet1.Cells[1, 6].SetCellValue(0, 0, $"Масса {yearCur} (всего, ожидаемое значение {yearPrev} год + опер. за {yearCur})");
        worksheet1.Cells[1, 7].SetCellValue(0, 0, $"Масса {yearCur} (всего, фактическое значение)");
        worksheet1.Cells[1, 8].SetCellValue(0, 0, $"Активность {yearCur} (всего, ожидаемое значение {yearPrev} год + опер. за {yearCur})");
        worksheet1.Cells[1, 9].SetCellValue(0, 0, $"Активность {yearCur} (всего, фактическое значение)");
        worksheet1.Cells[1, 10].SetCellValue(0, 0, $"Кол-во ОЗИИИ {yearCur} (всего, ожидаемое значение {yearPrev} год + опер. за {yearCur})");
        worksheet1.Cells[1, 11].SetCellValue(0, 0, $"Кол-во ОЗИИИ {yearCur} (всего, фактическое значение)");
        worksheet1.Cells[1, 12].SetCellValue(0, 0, $"Объем {yearCur} (суб, ожидаемое значение {yearPrev} год + опер. за {yearCur})");
        worksheet1.Cells[1, 13].SetCellValue(0, 0, $"Объем {yearCur} (суб, фактическое значение)");
        worksheet1.Cells[1, 14].SetCellValue(0, 0, $"Кол-во ОЗИИИ {yearCur} (суб, ожидаемое значение {yearPrev} год + опер. за {yearCur})");
        worksheet1.Cells[1, 15].SetCellValue(0, 0, $"Кол-во ОЗИИИ {yearCur} (суб, фактическое значение)");
        worksheet1.Cells[1, 16].SetCellValue(0, 0, $"Объем {yearCur} (накоп, ожидаемое значение {yearPrev} год + опер. за {yearCur})");
        worksheet1.Cells[1, 17].SetCellValue(0, 0, $"Объем {yearCur} (накоп, фактическое значение)");
        worksheet1.Cells[1, 18].SetCellValue(0, 0, $"Кол-во ОЗИИИ {yearCur} (накоп, ожидаемое значение {yearPrev} год + опер. за {yearCur})");
        worksheet1.Cells[1, 19].SetCellValue(0, 0, $"Кол-во ОЗИИИ {yearCur} (накоп, фактическое значение)");
        for (var i = 1; i <= 19; i++) worksheet1.Column(i).AutoFit();
        worksheet1.Column(2).Width = worksheet1.Column(1).Width * 3;
        worksheet1.Column(3).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(4).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(5).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(6).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(7).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(8).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(9).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(10).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(11).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(12).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(13).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(14).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(15).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(16).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(17).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(18).Width = worksheet1.Column(1).Width * 2;
        worksheet1.Column(19).Width = worksheet1.Column(1).Width * 2;
        var rowCurrent = 1;
        for (var col = 1; col <= 19; col++)
        {
            worksheet1.Cells[rowCurrent, col].Style.WrapText = true;
            worksheet1.Cells[rowCurrent, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            worksheet1.Cells[rowCurrent, col].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
        }
        rowCurrent += 1;
        foreach (var entry in rows)
        {
            worksheet1.Cells[rowCurrent, 1].SetCellValue(0, 0, rowCurrent - 1);
            worksheet1.Cells[rowCurrent, 2].SetCellValue(0, 0, entry.Key.Item1);
            worksheet1.Cells[rowCurrent, 3].SetCellValue(0, 0, entry.Key.Item2);
            worksheet1.Cells[rowCurrent, 4].SetCellValue(0, 0, entry.Value["VolumeOutOfPack_DB_TE"]);
            worksheet1.Cells[rowCurrent, 5].SetCellValue(0, 0, entry.Value["VolumeOutOfPack_DB_TR"]);
            if (entry.Value["VolumeOutOfPack_DB_TE"].ToString("F5") != entry.Value["VolumeOutOfPack_DB_TR"].ToString("F5"))
            {
                //worksheet1.Cells[rowCurrent, 4, rowCurrent, 5].Style.Fill.BackgroundColor.SetColor(255, 224, 224, 192);
                worksheet1.Cells[rowCurrent, 4, rowCurrent, 5].Style.Font.Bold = true;
            }
            worksheet1.Cells[rowCurrent, 6].SetCellValue(0, 0, entry.Value["MassOutOfPack_DB_TE"]);
            worksheet1.Cells[rowCurrent, 7].SetCellValue(0, 0, entry.Value["MassOutOfPack_DB_TR"]);
            if (entry.Value["MassOutOfPack_DB_TE"].ToString("F5") != entry.Value["MassOutOfPack_DB_TR"].ToString("F5"))
            {
                //worksheet1.Cells[rowCurrent, 4, rowCurrent, 5].Style.Fill.BackgroundColor.SetColor(255, 224, 224, 192);
                worksheet1.Cells[rowCurrent, 6, rowCurrent, 7].Style.Font.Bold = true;
            }
            worksheet1.Cells[rowCurrent, 8].SetCellValue(0, 0, entry.Value["Activity_DB_TE"]);
            worksheet1.Cells[rowCurrent, 9].SetCellValue(0, 0, entry.Value["Activity_DB_TR"]);
            if (entry.Value["Activity_DB_TE"].ToString("F5") != entry.Value["Activity_DB_TR"].ToString("F5"))
            {
                //worksheet1.Cells[rowCurrent, 4, rowCurrent, 5].Style.Fill.BackgroundColor.SetColor(255, 224, 224, 192);
                worksheet1.Cells[rowCurrent, 8, rowCurrent, 9].Style.Font.Bold = true;
            }
            worksheet1.Cells[rowCurrent, 10].SetCellValue(0, 0, entry.Value["QuantityOZIII_DB_TE"]);
            worksheet1.Cells[rowCurrent, 11].SetCellValue(0, 0, entry.Value["QuantityOZIII_DB_TR"]);
            if (entry.Value["QuantityOZIII_DB_TE"].ToString("F5") != entry.Value["QuantityOZIII_DB_TR"].ToString("F5"))
            {
                //worksheet1.Cells[rowCurrent, 6, rowCurrent, 7].Style.Fill.BackgroundColor.SetColor(255, 224, 224, 192);
                worksheet1.Cells[rowCurrent, 10, rowCurrent, 11].Style.Font.Bold = true;
            }
            worksheet1.Cells[rowCurrent, 12].SetCellValue(0, 0, entry.Value["VolumeOutOfPack_DB_SE"]);
            worksheet1.Cells[rowCurrent, 13].SetCellValue(0, 0, entry.Value["VolumeOutOfPack_DB_SR"]);
            if (entry.Value["VolumeOutOfPack_DB_SE"].ToString("F5") != entry.Value["VolumeOutOfPack_DB_SR"].ToString("F5"))
            {
                //worksheet1.Cells[rowCurrent, 8, rowCurrent, 9].Style.Fill.BackgroundColor.SetColor(255, 224, 224, 192);
                worksheet1.Cells[rowCurrent, 12, rowCurrent, 13].Style.Font.Bold = true;
            }
            worksheet1.Cells[rowCurrent, 14].SetCellValue(0, 0, entry.Value["QuantityOZIII_DB_SE"]);
            worksheet1.Cells[rowCurrent, 15].SetCellValue(0, 0, entry.Value["QuantityOZIII_DB_SR"]);
            if (entry.Value["QuantityOZIII_DB_SE"].ToString("F5") != entry.Value["QuantityOZIII_DB_SR"].ToString("F5"))
            {
                //worksheet1.Cells[rowCurrent, 10, rowCurrent, 11].Style.Fill.BackgroundColor.SetColor(255, 224, 224, 192);
                worksheet1.Cells[rowCurrent, 14, rowCurrent, 15].Style.Font.Bold = true;
            }
            worksheet1.Cells[rowCurrent, 16].SetCellValue(0, 0, entry.Value["VolumeOutOfPack_DB_1E"]);
            worksheet1.Cells[rowCurrent, 17].SetCellValue(0, 0, entry.Value["VolumeOutOfPack_DB_1R"]);
            if (entry.Value["VolumeOutOfPack_DB_1E"].ToString("F5") != entry.Value["VolumeOutOfPack_DB_1R"].ToString("F5"))
            {
                //worksheet1.Cells[rowCurrent, 12, rowCurrent, 13].Style.Fill.BackgroundColor.SetColor(255, 224, 224, 192);
                worksheet1.Cells[rowCurrent, 16, rowCurrent, 17].Style.Font.Bold = true;
            }
            worksheet1.Cells[rowCurrent, 18].SetCellValue(0, 0, entry.Value["QuantityOZIII_DB_1E"]);
            worksheet1.Cells[rowCurrent, 19].SetCellValue(0, 0, entry.Value["QuantityOZIII_DB_1R"]);
            if (entry.Value["QuantityOZIII_DB_1E"].ToString("F5") != entry.Value["QuantityOZIII_DB_1R"].ToString("F5"))
            {
                //worksheet1.Cells[rowCurrent, 14, rowCurrent, 15].Style.Fill.BackgroundColor.SetColor(255, 224, 224, 192);
                worksheet1.Cells[rowCurrent, 18, rowCurrent, 19].Style.Font.Bold = true;
            }
            for (var col = 1; col <= 19; col++)
            {
                worksheet1.Cells[rowCurrent, col].Style.WrapText = true;
                worksheet1.Cells[rowCurrent, col].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                worksheet1.Cells[rowCurrent, col].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            }
            rowCurrent += 1;
        }
        await ExcelSaveAndOpen(xls, fullPath, openTemp, cts).ConfigureAwait(false);
    }

    #endregion

    #region ExcelGetFullPath

    /// <summary>
    /// Выводит сообщение, дающее выбор, открывать временную копию или сохранить файл.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <returns>Полный путь до файла и флаг, нужно ли открывать временную копию.</returns>
    private static async Task<(string fullPath, bool openTemp)> ExcelGetFullPath(string fileName, CancellationTokenSource cts,
        AnyTaskProgressBar? progressBar = null)
    {
        #region MessageSaveOrOpenTemp

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Сохранить" },
                    new ButtonDefinition { Name = "Открыть временную копию" }
                ],
                CanResize = true,
                ContentTitle = "Выгрузка в .xlsx",
                ContentHeader = "Уведомление",
                ContentMessage = "Что бы вы хотели сделать с данной выгрузкой?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            }).ShowWindowDialogAsync(Desktop.MainWindow));

        #endregion

        var fullPath = "";
        var openTemp = res is "Открыть временную копию";

        switch (res)
        {
            case "Открыть временную копию":
            {
                DirectoryInfo tmpFolder = new(Path.Combine(BaseVM.SystemDirectory, "RAO", "temp"));
                var count = 0;

                fullPath = Path.Combine(tmpFolder.FullName, fileName + ".xlsx");
                while (File.Exists(fullPath))
                {
                    fullPath = Path.Combine(tmpFolder.FullName, fileName + $"_{++count}.xlsx");
                }

                break;
            }
            case "Сохранить":
            {
                SaveFileDialog dial = new();
                var filter = new FileDialogFilter
                {
                    Name = "Excel",
                    Extensions = { "xlsx" }
                };
                dial.Filters.Add(filter);
                dial.InitialFileName = fileName;
                fullPath = await dial.ShowAsync(Desktop.MainWindow);
                if (string.IsNullOrEmpty(fullPath)) await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                if (!fullPath.EndsWith(".xlsx")) fullPath += ".xlsx"; //В проводнике Linux в имя файла не подставляется расширение из фильтра, добавляю руками если его нет
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath!);
                    }
                    catch
                    {
                        #region MessageFailedToSaveFile

                        await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                            .GetMessageBoxStandard(new MessageBoxStandardParams
                            {
                                ButtonDefinitions = ButtonEnum.Ok,
                                ContentTitle = "Выгрузка в .xlsx",
                                ContentHeader = "Ошибка",
                                ContentMessage =
                                    $"Не удалось сохранить файл по пути: {fullPath}" +
                                    $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                    $"{Environment.NewLine}и используется другим процессом.",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                                Topmost = true,
                            }).ShowWindowDialogAsync(Desktop.MainWindow));

                        #endregion

                        await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                    }
                }

                break;
            }
            default:
            {
                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                break;
            }
        }
        return(fullPath, openTemp);
    }

    #endregion

    #region ExcelSaveAndOpen

    /// <summary>
    /// Сохранить изменения в .xlsx и открыть временную копию при необходимости.
    /// </summary>
    /// <param name="excelPackage">Пакет данных .xlsx.</param>
    /// <param name="fullPath">Полный путь к файлу .xlsx.</param>
    /// <param name="openTemp">Флаг, открывать ли временную копию.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <param name="isBackground">Признак выполнения команды в фоне.</param>
    /// <returns>Открывает файл выгрузки в .xlsx.</returns>
    private static async Task ExcelSaveAndOpen(ExcelPackage excelPackage, string fullPath, bool openTemp,
        CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null, bool isBackground = false)
    {
        try
        {
            await excelPackage.SaveAsync(cancellationToken: cts.Token);
        }
        catch (ObjectDisposedException ex)
        {
            return;
        }
        catch (Exception ex)
        {
            #region MessageFailedToSaveFile

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    CanResize = true,
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Не удалось сохранить файл по указанному пути:" +
                                     $"{Environment.NewLine}{fullPath}",
                    MinWidth = 400,
                    MinHeight = 175,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion

            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Warning(msg);

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        if (isBackground) return;
        if (openTemp)
        {
            Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
        }
        else
        {
            #region MessageExcelExportComplete

            var answer =
                await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxCustom(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок" },
                        new ButtonDefinition { Name = "Открыть выгрузку" }
                    ],
                    ContentTitle = "Выгрузка в .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Выгрузка сохранена по пути:" +
                                     $"{Environment.NewLine}{fullPath}",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion

            if (answer is "Открыть выгрузку")
            {
                Process.Start(new ProcessStartInfo { FileName = fullPath, UseShellExecute = true });
            }
        }
    }

    #endregion
}