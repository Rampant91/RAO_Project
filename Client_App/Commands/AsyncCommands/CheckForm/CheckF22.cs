using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Threading;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form2;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Проверка отчётов по форме 2.2. 
/// </summary>
public class CheckF22 : CheckBase
{
    public override bool CanExecute(object? parameter) => true;

    private static string? dbWithForm1Prev = null;

    const string form15Plug = "!1,5";
    const string formGenericPlug = "!1,X";

    static List<string> validOperationCodesPlus = new()
    {
        "11", "12", "13", "14", "16", "18", "31", "32", "33", "34", "35", "36", "37", "38", "39", "41", "52", "55", "56", "57", "59", "73", "74", "75", "76", "85", "86", "88", "97"
    };
    static List<string> validOperationCodesMinus = new()
    {
        "21", "22", "23", "24", "25", "26", "27", "28", "29", "42", "43", "44", "45", "46", "47", "48", "49", "51", "68", "71", "72", "84", "98"
    };
    //each unit is identified as a unique combination of these values (keys); setting any one to false ignores it when generating the unified key
    static bool keyInclude1 = true;     //storage name
    static bool keyInclude2 = true;     //storage code
    static bool keyInclude3 = false;     //pack type
    static bool keyInclude4 = true;     //code RAO
    static bool keyInclude5 = true;     //status RAO
    static bool keyInclude6 = false;     //FCP

    #region AsyncExecute

    public override async Task<List<CheckError>> AsyncExecute(object? parameter)
    {
        return await MainCheck(parameter);
    }

    #endregion

    #region MainCheck

    public async Task<List<CheckError>> MainCheck(object? parameter, string? regno = null)
    {
        var cts = new CancellationTokenSource();
        List<CheckError> errorList = [];
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var rep = parameter as Report;
        if (rep is null && regno == null)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        var db = new DBModel(StaticConfiguration.DBPath);
        var db2 = new DBModel(StaticConfiguration.DBPath);

        string form20RegNo = regno == null ? rep!.Reports.Master_DB.RegNoRep.Value : regno;
        string form20Okpo = rep!.Reports.Master_DB.OkpoRep.Value;

        string repYear = rep.Year_DB;
        string repFormNum = rep.FormNum_DB;
        ObservableCollectionWithItemPropertyChanged<Form22> repRows22 = rep.Rows22;

        if (string.IsNullOrWhiteSpace(form20RegNo))
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        progressBarVM.SetProgressBar(5, "Поиск соответствующей формы 1.0",
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

            var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = dbWithForm1Prev == null ?
                    [
                        new ButtonDefinition { Name = "Выбрать файл БД", IsDefault = true },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ] :
                    [
                        new ButtonDefinition { Name = "Использовать ранее выбранный файл БД", IsDefault = true },
                        new ButtonDefinition { Name = "Выбрать файл БД", IsDefault = true },
                        new ButtonDefinition { Name = "Отмена", IsCancel = true }
                    ],
                    CanResize = true,
                    ContentTitle = "Проверка формы",
                    ContentHeader = "Ошибка",
                    ContentMessage = "В текущей базе данных отсутствует форма 1.0 для проверяемой организации." +
                                     $"{Environment.NewLine}Можете выбрать файл базы данных, содержащий форму 1.0 для данной организации " +
                                     $"{Environment.NewLine}или операция проверки формы будет отменена.",
                    MinWidth = 400,
                    MinHeight = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(desktop.MainWindow));

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

            string[]? dbWithForm1 = null;
            string dbWithForm1FullPath;
            if (answer is "Использовать ранее выбранный файл БД" && dbWithForm1Prev != null)
            {
                dbWithForm1 = [dbWithForm1Prev];
            }
            else
            {
                dbWithForm1 = await dial.ShowAsync(desktop.MainWindow);
                if (dbWithForm1 is null)
                {
                    await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
                }
            }
            dbWithForm1FullPath = dbWithForm1![0];
            dbWithForm1Prev = dbWithForm1FullPath;
            db = new DBModel(dbWithForm1FullPath);
        }


        var repsWithForm1Base = db.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows10)
            .Include(reps => reps.Report_Collection
                .Where(report =>
                    (report.FormNum_DB == "1.5" || report.FormNum_DB == "1.6" || report.FormNum_DB == "1.7" || report.FormNum_DB == "1.8")
                    && (report.StartPeriod_DB.Length >= 4
                        && report.StartPeriod_DB.Substring(report.StartPeriod_DB.Length - 4) == repYear
                        || report.EndPeriod_DB.Length >= 4
                        && report.EndPeriod_DB.Substring(report.EndPeriod_DB.Length - 4) == repYear)))
            .ThenInclude(x => x.Rows15)
            .Include(reps => reps.Report_Collection).ThenInclude(report => report.Rows16)
            .Include(reps => reps.Report_Collection).ThenInclude(report => report.Rows17)
            .Include(reps => reps.Report_Collection).ThenInclude(report => report.Rows18)
            .Where(reps => reps.DBObservable != null);

        var forms1 = repsWithForm1Base.Where(reps => reps.Master_DB.Rows10.Any(form10 => form10.RegNo_DB == form20RegNo)).ToList();

        Reports? repsWithForm1;

        if (forms1.Count > 1)
        {
            List<string> okpoList = new();
            foreach (var form in forms1)
            {
                okpoList.Add(form.Master_DB.Rows10.Last().Okpo_DB);
            }
            bool fusion = true;
            if (fusion)
            {
                repsWithForm1 = new();
                foreach (var okpo in okpoList)
                {
                    repsWithForm1.Report_Collection.AddRange(repsWithForm1Base
                            .FirstOrDefaultAsync(reps => reps.Master_DB.Rows10
                                .Any(form10 => form10.RegNo_DB == form20RegNo && form10.Okpo_DB == okpo), cts.Token).Result!.Report_Collection);
                }
            }
            else
            {
                var desktop = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

                List<ButtonDefinition> buttons = new List<ButtonDefinition>();
                foreach (var okpo in okpoList)
                {
                    buttons.Add(new ButtonDefinition { Name = okpo, IsDefault = true });
                }

                #region MessageMoreThanOneOrgFound

                var messageBoxCustomParams = new MessageBoxCustomParams
                {
                    ButtonDefinitions = buttons,
                    CanResize = true,
                    ContentTitle = "Проверка формы",
                    ContentHeader = "Внимание",
                    ContentMessage = $"Найдено более одной организации с регистрационным номером {form20RegNo}." +
                                         $"{Environment.NewLine}Пожалуйста, выберите нужную организацию по коду ОКПО.",
                    MinWidth = 400,
                    MinHeight = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(messageBoxCustomParams)
                    .ShowDialog(desktop.MainWindow));

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

        int yearRealCurrent;
        int.TryParse(repYear, out yearRealCurrent);
        string yearPrevious = (yearRealCurrent - 1).ToString();

        Reports? repsWithForm2 = await db2.ReportsCollectionDbSet
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

        if (repsWithForm1 is null)
        {
            #region MessageCheckFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = $"Проверка формы {repFormNum}",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Не удалось проверить форму, поскольку в выбранном файле БД отсутствуют записи для организации {form20RegNo}_{form20Okpo}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        List<Form22> forms22ExpectedBase = [];
        List<(string, string, string)> forms22MetadataBase = [];
        Form17? formHeader17 = null;
        Form18? formHeader18 = null;
        foreach (var key in repsWithForm1!.Report_Collection)
        {
            var report = (Report)key;
            Form22? form22New;
            switch (report.FormNum_DB)
            {
                case "1.5":
                    {
                        report.Rows15 = new(report.Rows15.OrderBy(x => x.NumberInOrder_DB));
                        foreach (var key1 in report.Rows15)
                        {
                            var form = (Form15)key1;
                            form22New = FormConvert(form, repYear);
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
                        report.Rows16 = new(report.Rows16.OrderBy(x => x.NumberInOrder_DB));
                        foreach (var key1 in report.Rows16)
                        {
                            var form = (Form16)key1;
                            form22New = FormConvert(form, repYear);
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
                        report.Rows17 = new(report.Rows17.OrderBy(x => x.NumberInOrder_DB));
                        foreach (var key1 in report.Rows17)
                        {
                            var form = (Form17)key1;
                            if (form.OperationCode_DB != "-" && !string.IsNullOrWhiteSpace(form.OperationCode_DB)) formHeader17 = form;
                            form22New = FormConvert(form, formHeader17, repYear);
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
                        report.Rows18 = new(report.Rows18.OrderBy(x => x.NumberInOrder_DB));
                        foreach (var key1 in report.Rows18)
                        {
                            var form = (Form18)key1;
                            if (form.OperationCode_DB != "-" && !string.IsNullOrWhiteSpace(form.OperationCode_DB)) formHeader18 = form;
                            form22New = FormConvert(form, formHeader18, repYear);
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
        if (repsWithForm2 != null && repsWithForm2.Report_Collection != null)
        {
            foreach (var key in repsWithForm2.Report_Collection)
            {
                var report = (Report)key;
                Form22? form22New;
                report.Rows22 = new(report.Rows22.OrderBy(x => x.NumberInOrder_DB));
                foreach (var key1 in report.Rows22)
                {
                    var form = (Form22)key1;
                    form22New = FormConvert(form, repYear);
                    if (form22New != null)
                    {
                        forms22MetadataBase.Add((form22New.FormNum_DB, yearPrevious, form22New.NumberInOrder_DB.ToString()));
                        forms22ExpectedBase.Add(form22New);
                    }
                }
                break;
            }
        }
        Dictionary<(string, string, string, string, string, string), Form22> forms22ExpectedDict = new();
        Dictionary<(string, string, string, string, string, string), Form22> forms22RealDict = new();
        Dictionary<(string, string, string, string, string, string), Form22> forms22ExpectedSubDict = new();
        Dictionary<(string, string, string, string, string, string), Form22> forms22RealSubDict = new();
        Dictionary<(string, string, string, string, string, string), Dictionary<string, Dictionary<string, List<string>>>> forms22MetadataDict = new();
        for (var i = 0; i < forms22ExpectedBase.Count; i++)
        {
            double subsidy = -1.0;
            TryParseDoubleExtended(forms22ExpectedBase[i].Subsidy_DB.Replace("%", ""), out subsidy);
            (string, string, string, string, string, string) key = (
                keyInclude1 ? forms22ExpectedBase[i].StoragePlaceName_DB.Replace(" ", "").ToLower() : "",
                keyInclude2 ? forms22ExpectedBase[i].StoragePlaceCode_DB : "",
                keyInclude3 ? forms22ExpectedBase[i].PackType_DB.Replace(" ", "").ToLower() : "",
                keyInclude4 ? forms22ExpectedBase[i].CodeRAO_DB : "",
                keyInclude5 ? forms22ExpectedBase[i].StatusRAO_DB : "",
                keyInclude6 ? forms22ExpectedBase[i].FcpNumber_DB : ""
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
                Form22 form22ExpectedBaseSub = Form22_CopySub(forms22ExpectedBase[i]);
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


            if (!forms22MetadataDict.ContainsKey(key))
            {
                forms22MetadataDict[key] = [];
            }
            if (!forms22MetadataDict[key].ContainsKey(forms22MetadataBase[i].Item1))
            {
                forms22MetadataDict[key][forms22MetadataBase[i].Item1] = [];
            }
            if (!forms22MetadataDict[key][forms22MetadataBase[i].Item1].ContainsKey($"{forms22MetadataBase[i].Item2}"))
            {
                forms22MetadataDict[key][forms22MetadataBase[i].Item1][forms22MetadataBase[i].Item2] = [];
            }
            forms22MetadataDict[key][forms22MetadataBase[i].Item1][forms22MetadataBase[i].Item2].Add(forms22MetadataBase[i].Item3);
        }
        List<Form22> forms22Real = [];
        for (int i = 0; i < repRows22.Count; i++)
        {
            if (string.IsNullOrWhiteSpace(repRows22[i].FcpNumber_DB.Trim())) repRows22[i].FcpNumber_DB = "-";
        }
        foreach (var key1 in repRows22)
        {
            var form = (Form22)key1;
            double subsidy = -1.0;
            TryParseDoubleExtended(form.Subsidy_DB.Replace("%", ""), out subsidy);
            if (form.CodeRAO_DB != "-" && !string.IsNullOrWhiteSpace(form.CodeRAO_DB))
            {
                var key = (
                    keyInclude1 ? form.StoragePlaceName_DB.Replace(" ", "").ToLower() : "",
                    keyInclude2 ? form.StoragePlaceCode_DB : "",
                    keyInclude3 ? form.PackType_DB.Replace(" ", "").ToLower() : "",
                    keyInclude4 ? form.CodeRAO_DB : "",
                    keyInclude5 ? form.StatusRAO_DB : "",
                    keyInclude6 ? form.FcpNumber_DB : ""
                );
                if (!forms22RealDict.ContainsKey(key))
                {
                    forms22RealDict[key] = Form22_Copy(form);
                }
                else
                {
                    string errorValue = ItemName(form);
                    string errorMessage = $"В форме 2.2 уже присутствует строка с указанными РАО (строка {forms22RealDict[key].NumberInOrder_DB}). Следует объединить данные в соответствии с ЕОМУ (пункт 18.13, абзац 7).";
                    Form22_Add(forms22RealDict[key], form);
                    CheckError? errorDouble = errorList.SingleOrDefault(x => string.Equals(errorValue, x.Value) && string.Equals(errorMessage, x.Message));
                    if (errorDouble == null)
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_22",
                            Row = form.NumberInOrder_DB.ToString(),
                            Column = "-",
                            Value = errorValue,
                            Message = errorMessage
                        });
                    }
                    else
                    {
                        errorList[errorList.IndexOf(errorDouble)].Row += $", {form.NumberInOrder_DB}";
                    }
                }
                if (subsidy >= 0)
                {
                    Form22 formSub = Form22_CopySub(form);
                    if (!forms22RealSubDict.ContainsKey(key))
                    {
                        forms22RealSubDict[key] = Form22_Copy(formSub);
                    }
                    else
                    {
                        Form22_Add(forms22RealSubDict[key], formSub);
                        string errorValue = ItemName(formSub);
                        if (!errorList.Any(x => x.Value == errorValue))
                        {
                            string errorMessage = $"В форме 2.2 уже присутствует строка с указанными РАО (строка {forms22RealSubDict[key].NumberInOrder_DB}). Следует объединить данные в соответствии с ЕОМУ (пункт 18.13, абзац 7).";
                            CheckError? errorDouble = errorList.SingleOrDefault(x => string.Equals(errorValue, x.Value) && string.Equals(errorMessage, x.Message));
                            if (errorDouble == null)
                            {
                                errorList.Add(new CheckError
                                {
                                    FormNum = "form_22",
                                    Row = formSub.NumberInOrder_DB.ToString(),
                                    Column = "-",
                                    Value = errorValue,
                                    Message = errorMessage
                                });
                            }
                            else
                            {
                                errorList[errorList.IndexOf(errorDouble)].Row += $", {form.NumberInOrder_DB}";
                            }
                        }
                    }
                }
            }
        }
        forms22Real = forms22RealDict.Keys.Select(key => forms22RealDict[key]).ToList();
        forms22Real = [];
        foreach (var key in forms22RealDict.Keys)
        {
            forms22Real.Add(forms22RealDict[key]);
        }
        //the converted values should be compared to the rows in reps.
        List<(Form22, string, string)> forms22Expected = [];
        List<(Form22, string)> form15PlugDoubles = new();
        foreach (var key in forms22ExpectedDict.Keys)
        {
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
                    periods = new([yearPrevious]);
                }
                else
                {
                    for (var i = 0; i < periods.Count; i++)
                        periods[i] = $"{periods[i].Substring(6, 4)}.{periods[i].Substring(3, 2)}.{periods[i][..2]}{periods[i][10..]}";
                    periods.Sort();
                    for (var i = 0; i < periods.Count; i++)
                        periods[i] = $"{periods[i].Substring(8, 2)}.{periods[i].Substring(5, 2)}.{periods[i][..4]}{periods[i][10..]}";
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
                            || i < linesReal.Count - 1 && linesReal[i + 1] == -1))
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
        foreach (var formReal in forms22Real)
        {
            string form15PlugItemName = ItemName(formReal, false);
            Form22 form22RealPure = Form22_Copy(formReal);
            Form22_ToDecExp(form22RealPure);
            var matchFound = false;
            for (int i = forms22Expected.Count - 1; i >= 0; i--)
            {
                Form22 form22ExpectedPure = Form22_Copy(forms22Expected[i].Item1);
                Form22_ToDecExp(form22ExpectedPure);
                (Form22, string, string) form22Expected = (form22ExpectedPure, forms22Expected[i].Item2, forms22Expected[i].Item3);
                var mismatches = Form22_Match(form22Expected.Item1, form22RealPure, $"форм{(form22Expected.Item3.Contains(',') ? "ы" : "а")} {form22Expected.Item3}{(form22Expected.Item3 == "2.2" ? " (" + yearPrevious + ")" : "")}", $"форма 2.2 ({yearRealCurrent})", form22Expected.Item1.CodeRAO_DB == form15Plug);
                if (mismatches == null)
                {
                    if (TryParseDoubleExtended(form22ExpectedPure.QuantityOZIII_DB, out var quantityVal) && Math.Abs(quantityVal) <= 1e-14
                    && TryParseDoubleExtended(form22ExpectedPure.VolumeOutOfPack_DB, out var VolumeOutOfPackVal) && Math.Abs(VolumeOutOfPackVal) <= 1e-14
                    && TryParseDoubleExtended(form22ExpectedPure.VolumeInPack_DB, out var VolumeInPack_DB) && Math.Abs(VolumeInPack_DB) <= 1e-14)
                    {
                        forms22Expected.RemoveAt(i);
                        matchFound = true;
                        break;
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
            if (!matchFound && !form15PlugDoubles.Any(x => x.Item2 == form15PlugItemName))
            {
                for (int i = forms22Expected.Count - 1; i >= 0; i--)
                {
                    (Form22, string, string) form22Expected = forms22Expected[i];
                    var mismatches = Form22_Match(form22Expected.Item1, form22RealPure, $"форм{(forms22Expected[i].Item3.Contains(',') ? "ы" : "а")} {form22Expected.Item3}{(form22Expected.Item3 == "2.2" ? " (" + yearPrevious + ")" : "")}", $"форма 2.2 ({yearRealCurrent})", form22Expected.Item1.CodeRAO_DB == form15Plug, true);
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
            if (!matchFound && !form15PlugDoubles.Any(x => x.Item2 == form15PlugItemName))
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_22",
                    Row = form22RealPure.NumberInOrder_DB.ToString(),
                    Column = "-",
                    Value = ItemName(form22RealPure),
                    Message = $"В форме 2.2 ({yearPrevious}) и в формах 1.5 - 1.8 ({yearRealCurrent}) не найдена информация об указанных РАО."
                });
            }
        }
        foreach (var formExpected in forms22Expected)
        {
            //if (int.TryParse(formExpected.Item1.PackQuantity_DB, out int packQuantity) && packQuantity == 0) continue;
            double zeroCheck;
            List<string> negatives = new();
            int nonZero = 0;
            if (TryParseDoubleExtended(formExpected.Item1.VolumeOutOfPack_DB, out zeroCheck))
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
        errorList.Sort((i, j) =>
            int.TryParse(i.Row.Split(',')[0], out var iRowReal)
            && int.TryParse(j.Row.Split(',')[0], out var jRowReal)
                ? iRowReal - jRowReal
                : string.Compare(i.Row, j.Row));
        var index = 0;
        foreach (var error in errorList)
        {
            if (GraphsList.TryGetValue(error.Column, out var columnFrontName))
            {
                error.Column = columnFrontName;
            }
            index++;
            error.Index = index;
        }

        progressBarVM.SetProgressBar(100, "Завершение проверки");
        await progressBar.CloseAsync();

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

    #region Properties

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

    #endregion

    #region FormConvert

    private static Form22? FormConvert(Form15 form, string year)
    {
        /*List<string> validOperationCodesPlus = new([
            "11", "12", "13", "14", "16", "18",
            "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "41", "52", "73", "74", "75", "76", "88", "97"
        ]);
        List<string> validOperationCodesMinus = new([
            "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "44", "45", "49", "51", "68", "71", "98"
        ]);*/
        if (!validOperationCodesPlus.Contains(form.OperationCode_DB) && !validOperationCodesMinus.Contains(form.OperationCode_DB))    //filter out operation codes
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
            CodeRAO_DB = (form15Plug ?? "").Trim(),
            StatusRAO_DB = (form.StatusRAO_DB ?? "").Trim(),
            StoragePlaceCode_DB = (form.StoragePlaceCode_DB ?? "").Trim(),
            FcpNumber_DB = (form.FcpNumber_DB ?? "").Replace('.', ',').Trim(),
            StoragePlaceName_DB = (form.StoragePlaceName_DB ?? "").Trim(),
            PackName_DB = (form.PackName_DB ?? "").Trim(),
            PackType_DB = (form.PackType_DB ?? "").Trim(),
            PackQuantity_DB = "1",
            VolumeOutOfPack_DB = (form15Plug ?? "").Trim(),
            VolumeInPack_DB = (form15Plug ?? "").Trim(),
            MassOutOfPack_DB = (form15Plug ?? "").Trim(),
            MassInPack_DB = (form15Plug ?? "").Trim(),
            QuantityOZIII_DB = (form.Quantity_DB?.ToString() ?? "").Replace('.', ',').Trim(),
            TritiumActivity_DB = (form15Plug ?? "").Trim(),
            BetaGammaActivity_DB = (form15Plug ?? "").Trim(),
            AlphaActivity_DB = (form15Plug ?? "").Trim(),
            TransuraniumActivity_DB = (form15Plug ?? "").Trim(),
            MainRadionuclids_DB = (form.Radionuclids_DB ?? "").Trim(),
            Subsidy_DB = (form.Subsidy_DB ?? "").Replace('.', ',').Trim(),
        };
        if (string.IsNullOrWhiteSpace(res.FcpNumber_DB)) res.FcpNumber_DB = "-";
        int.TryParse(form.OperationCode_DB, out var directionMarker);
        if (validOperationCodesPlus.Contains(form.OperationCode_DB))
        {
            //plus
            res.NumberOfFields_DB = directionMarker;
            return res;
        }
        else if (validOperationCodesMinus.Contains(form.OperationCode_DB))
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
        List<string> validOperationCodesMinus = new([
            "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "44", "45", "49", "51", "68", "71", "98"
        ]);*/
        if (!validOperationCodesPlus.Contains(form.OperationCode_DB) && !validOperationCodesMinus.Contains(form.OperationCode_DB))    //filter out operation codes
        {
            return null;
        }
        if (!(form.OperationDate_DB.Length >= 4 && form.OperationDate_DB.Substring(form.OperationDate_DB.Length - 4) == year))
        {
            return null;    //the operation isn't from this year
        }
        if ((string.IsNullOrWhiteSpace(form.CodeRAO_DB) || form.CodeRAO_DB.Trim()=="-") && (string.IsNullOrWhiteSpace(form.StatusRAO_DB) || form.StatusRAO_DB.Trim() == "-"))
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
            VolumeInPack_DB = (formGenericPlug ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassOutOfPack_DB = (form.Mass_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassInPack_DB = (formGenericPlug ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
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
        if (validOperationCodesPlus.Contains(form.OperationCode_DB))
        {
            //plus
            res.NumberOfFields_DB = directionMarker;
            return res;
        }
        else if (validOperationCodesMinus.Contains(form.OperationCode_DB))
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
        //List<string> validOperationCodesMinus = new(["21", "22", "23", "24", "25", "26", "27", "28", "29", "51"]);
        if (form.CodeRAO_DB == "-" || string.IsNullOrWhiteSpace(form.CodeRAO_DB))
        {
            return null;    //empty line
        }
        if (!validOperationCodesPlus.Contains(formTrue.OperationCode_DB) && !validOperationCodesMinus.Contains(formTrue.OperationCode_DB))
        {
            return null;    //filter out operation codes
        }
        if (!(formTrue.OperationDate_DB.Length >= 4 && formTrue.OperationDate_DB.Substring(formTrue.OperationDate_DB.Length - 4) == year))
        {
            return null;    //the operation isn't from this year
        }
        if ((string.IsNullOrWhiteSpace(form.CodeRAO_DB) || form.CodeRAO_DB.Trim() == "-") && (string.IsNullOrWhiteSpace(form.StatusRAO_DB) || form.StatusRAO_DB.Trim() == "-"))
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
        if (validOperationCodesPlus.Contains(formTrue.OperationCode_DB))
        {
            //plus
            res.NumberOfFields_DB = directionMarker;
            return res;
        }
        else if (validOperationCodesMinus.Contains(formTrue.OperationCode_DB))
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
        //List<string> validOperationCodesMinus = new(["21", "22", "23", "24", "25", "26", "27", "28", "29", "51"]);
        if (form.CodeRAO_DB == "-" || string.IsNullOrWhiteSpace(form.CodeRAO_DB))
        {
            return null;    //empty line
        }
        if (!validOperationCodesPlus.Contains(formTrue.OperationCode_DB) && !validOperationCodesMinus.Contains(formTrue.OperationCode_DB))
        {
            return null;    //filter out operation codes
        }
        if (!(formTrue.OperationDate_DB.Length >= 4 && formTrue.OperationDate_DB.Substring(formTrue.OperationDate_DB.Length - 4) == year))
        {
            return null;    //the operation isn't from this year
        }
        if ((string.IsNullOrWhiteSpace(form.CodeRAO_DB) || form.CodeRAO_DB.Trim() == "-") && (string.IsNullOrWhiteSpace(form.StatusRAO_DB) || form.StatusRAO_DB.Trim() == "-"))
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
            PackName_DB = (formGenericPlug ?? "").Trim(),
            PackType_DB = (formGenericPlug ?? "").Trim(),
            PackQuantity_DB = "1",
            VolumeOutOfPack_DB = (form.Volume20_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            VolumeInPack_DB = (form.Volume6_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassOutOfPack_DB = (form.Mass21_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MassInPack_DB = (form.Mass7_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            QuantityOZIII_DB = (formGenericPlug ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TritiumActivity_DB = (form.TritiumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            BetaGammaActivity_DB = (form.BetaGammaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            AlphaActivity_DB = (form.AlphaActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            TransuraniumActivity_DB = (form.TransuraniumActivity_DB ?? "").Replace('.', ',').Replace('(', ' ').Replace(')', ' ').Trim(),
            MainRadionuclids_DB = (form.Radionuclids_DB ?? "").Trim(),
            Subsidy_DB = (form.Subsidy_DB ?? "").Replace('.', ',').Trim(),
        };
        if (string.IsNullOrWhiteSpace(res.FcpNumber_DB)) res.FcpNumber_DB = "-";
        int.TryParse(formTrue.OperationCode_DB, out var directionMarker);
        if (validOperationCodesPlus.Contains(formTrue.OperationCode_DB))
        {
            //plus
            res.NumberOfFields_DB = directionMarker;
            return res;
        }
        else if (validOperationCodesMinus.Contains(formTrue.OperationCode_DB))
        {
            //minus
            res.NumberOfFields_DB = -directionMarker;
            return res;
        }
        return null;
    }
    private static Form22? FormConvert(Form22 form, string year)
    {
        if ((string.IsNullOrWhiteSpace(form.CodeRAO_DB) || form.CodeRAO_DB.Trim() == "-") && (string.IsNullOrWhiteSpace(form.StatusRAO_DB) || form.StatusRAO_DB.Trim() == "-"))
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
        Form22 res = new()
        {
            NumberInOrder_DB = form.NumberInOrder_DB,
            NumberOfFields_DB = form.NumberOfFields_DB,
            FormNum_DB = form.FormNum_DB.Trim(),
            CodeRAO_DB = form.CodeRAO_DB.Trim(),
            StatusRAO_DB = form.StatusRAO_DB.Trim(),
            StoragePlaceCode_DB = form.StoragePlaceCode_DB.Trim(),
            FcpNumber_DB = form.FcpNumber_DB.Replace('.', ',').Trim(),
            StoragePlaceName_DB = form.StoragePlaceName_DB.Trim(),
            PackName_DB = form.PackName_DB.Trim(),
            PackType_DB = form.PackType_DB.Trim(),
            PackQuantity_DB = form.PackQuantity_DB.Trim(),
            VolumeOutOfPack_DB = form.VolumeOutOfPack_DB.Trim(),
            VolumeInPack_DB = form.VolumeInPack_DB.Trim(),
            MassOutOfPack_DB = form.MassOutOfPack_DB.Trim(),
            MassInPack_DB = form.MassInPack_DB.Trim(),
            QuantityOZIII_DB = form.QuantityOZIII_DB.Trim(),
            TritiumActivity_DB = form.TritiumActivity_DB.Trim(),
            BetaGammaActivity_DB = form.BetaGammaActivity_DB.Trim(),
            AlphaActivity_DB = form.AlphaActivity_DB.Trim(),
            TransuraniumActivity_DB = form.TransuraniumActivity_DB.Trim(),
            MainRadionuclids_DB = form.MainRadionuclids_DB.Trim(),
            Subsidy_DB = form.Subsidy_DB.Trim(),
        };
        if (string.IsNullOrWhiteSpace(res.FcpNumber_DB)) res.FcpNumber_DB = "-";
        return res;
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
        bool form15Touch = receiver.CodeRAO_DB == form15Plug || giver.CodeRAO_DB == form15Plug;
        if (form15Touch) receiver.CodeRAO_DB = form15Plug;
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
        bool form15Touch = giver.CodeRAO_DB == form15Plug || taker.CodeRAO_DB == form15Plug;
        if (form15Touch) giver.CodeRAO_DB = form15Plug;
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
        if (receiver == formGenericPlug || giver == formGenericPlug) return formGenericPlug;
        if (receiver == form15Plug || giver == form15Plug) return form15Plug;
        var res = receiver;
        var receiverReal = receiver == "-" || string.IsNullOrWhiteSpace(receiver) || receiver == form15Plug
            ? "0"
            : receiver;
        var giverReal = giver == "-" || string.IsNullOrWhiteSpace(giver) || giver == form15Plug
            ? "0"
            : giver;
        if (decimal.TryParse(receiverReal, out var receiverDecimal)
            && decimal.TryParse(giverReal, out var giverDecimal))
        {
            res = decimal.Add(receiverDecimal, giverDecimal).ToString();
        }
        else if (TryParseDoubleExtended(receiverReal, out var receiverTrue)
            && TryParseDoubleExtended(giverReal, out var giverTrue))
        {
            res = (receiverTrue + giverTrue).ToString();
        }
        return res;
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
        if (giver == formGenericPlug || taker == formGenericPlug) return formGenericPlug;
        if (giver == form15Plug || taker == form15Plug) return form15Plug;
        var res = giver;
        var giverReal = giver == "-" || string.IsNullOrWhiteSpace(giver) || giver == form15Plug
            ? "0"
            : giver;
        var takerReal = giver == "-" || string.IsNullOrWhiteSpace(taker) || taker == form15Plug
            ? "0"
            : taker;
        if (decimal.TryParse(giverReal, out var giverDecimal)
            && decimal.TryParse(takerReal, out var takerDecimal))
        {
            res = decimal.Subtract(giverDecimal, takerDecimal).ToString();
        }
        else if (TryParseDoubleExtended(giverReal, out var giverTrue)
            && TryParseDoubleExtended(takerReal, out var takerTrue))
        {
            res = (giverTrue - takerTrue).ToString();
        }
        return res;
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
        if (form1Val == form15Plug || form2Val == form15Plug) return;
        if (form1Val == formGenericPlug || form2Val == formGenericPlug) return;
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
        if (!((form1Val == "-" && form2Val == "-")
              || (val1 < 0.00001 && form2Val == "-")
              || (form1Val == "-" && val2 < 0.00001)
              || val1 >= val2 * (1.0 - valB)
              && val2 >= val1 * (1.0 - valB)))
        {
            res.Add((columnNum, $"{humanName}", $"{forms1}: {Form22_SubToDec(form1Val)}", $"{forms2}: {Form22_SubToDec(form2Val)}"));
        }
    }
    private static void Form22_SubMatchExp(string form1Val, string form2Val, string humanName, double valB, List<(int, string, string, string)> res, int columnNum, string forms1, string forms2, bool form15Fix, bool allowLesser = false)
    {
        if (form1Val == form15Plug || form2Val == form15Plug) return;
        if (form1Val == formGenericPlug || form2Val == formGenericPlug) return;
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
        if (!((form1Val == "-" && form2Val == "-")
              || (val1 < 0.00001 && form2Val == "-")
              || (form1Val == "-" && val2 < 0.00001)
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
        if (!keyInclude4 || (form1.CodeRAO_DB == form2.CodeRAO_DB || (form15PlugLeftover && (form1.CodeRAO_DB == form15Plug || form2.CodeRAO_DB == form15Plug)))
           )
        {
            if ((!keyInclude5 || form1.StatusRAO_DB.Trim() == form2.StatusRAO_DB.Trim())
                && (!keyInclude1 || form1.StoragePlaceName_DB.Replace(" ", "").ToLower() == form2.StoragePlaceName_DB.Replace(" ", "").ToLower())
                && (!keyInclude2 || form1.StoragePlaceCode_DB.Trim() == form2.StoragePlaceCode_DB.Trim())
                && (!keyInclude3 || form1.PackType_DB.Replace(" ","").ToLower() == form2.PackType_DB.Replace(" ", "").ToLower())
                && (!keyInclude6 || form1.FcpNumber_DB.Replace('-',' ').Trim().TrimEnd('0') == form2.FcpNumber_DB.Replace('-', ' ').Trim().TrimEnd('0')))
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
        if (keyInclude1) result.Add($"наименование пункта хранения {item.StoragePlaceName_DB}");
        if (keyInclude2) result.Add($"код пункта хранения {item.StoragePlaceCode_DB}");
        if (keyInclude3) result.Add($"тип упаковки {item.PackType_DB}");
        if (keyInclude4 && includeCodeRAO) result.Add($"код РАО {(item.CodeRAO_DB == form15Plug || item.CodeRAO_DB == formGenericPlug ? "-" : item.CodeRAO_DB)}");
        if (keyInclude5) result.Add($"статус РАО {item.StatusRAO_DB}");
        if (keyInclude6) result.Add($"номер мероприятия ФЦП {item.FcpNumber_DB}");
        if (result.Count > 0) result[0] = $"{result[0].Substring(0, 1).ToUpper()}{result[0].Substring(1)}";
        return string.Join(", ", result);
    }

    #endregion

}