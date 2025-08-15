using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using HarfBuzzSharp;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.CheckForm;

/// <summary>
/// Проверка отчётов по форме 2.8. 
/// </summary>
public class CheckF28 : CheckBase
{
    public override bool CanExecute(object? parameter) => true;

    #region AsyncExecute

    public override async Task<List<CheckError>> AsyncExecute(object? parameter)
    {
        return await Check_Total(parameter);
    }

    #endregion

    #region MainCheck

    public static async Task<List<CheckError>> Check_Total(object? parameter)
    {
        var cts = new CancellationTokenSource();
        List<CheckError> errorList = [];
        var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        var rep = parameter as Report;
        if (rep is null)
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        var db2 = new DBModel(StaticConfiguration.DBPath);

        string form20RegNo = rep!.Reports.Master_DB.RegNoRep.Value;
        string form20Okpo = rep!.Reports.Master_DB.OkpoRep.Value;

        string repYear = rep.Year_DB;
        string repFormNum = rep.FormNum_DB;

        if (string.IsNullOrWhiteSpace(form20RegNo))
        {
            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }


        int yearRealCurrent;
        int.TryParse(repYear, out yearRealCurrent);
        string yearPrevious = (yearRealCurrent - 1).ToString();

        Reports? reps28Prev = await db2.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(reps => reps.DBObservable)
            .Include(reps => reps.Master_DB).ThenInclude(report => report.Rows20)
            .Include(reps => reps.Report_Collection
                .Where(report =>
                    (report.FormNum_DB == "2.8")
                    && (report.Year_DB == yearPrevious)))
            .ThenInclude(x => x.Rows28)
            .Where(reps => reps.DBObservable != null)
            .FirstOrDefaultAsync(reps => reps.Master_DB.Rows20
                .Any(form20 => form20.RegNo_DB == form20RegNo), cts.Token);
        
        await db2.DisposeAsync();

        List<Form28> Rows28Cur = rep.Rows28.ToList();
        List<Form28> Rows28Prev = new();
        if (reps28Prev != null && reps28Prev.Report_Collection != null)
        {
            foreach (var key in reps28Prev.Report_Collection)
            {
                var report = (Report)key;
                report.Rows28 = new(report.Rows28.OrderBy(x => x.NumberInOrder_DB));
                Rows28Prev.AddRange(report.Rows28);
            }
        }

        Check_HeaderExpirationDate1(errorList, rep);
        Check_HeaderExpirationDate2(errorList, rep);
        Check_HeaderExpirationDate3(errorList, rep);
        Check_WasteMinmaxCur(errorList, rep);
        Check_PrevWastePercent(errorList, rep, Rows28Prev);
        Check_PrevStats(errorList, rep, Rows28Prev);

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

    #region Check_HeaderExpirationDate1

    private static void Check_HeaderExpirationDate1(List<CheckError> errorList, Report rep)
    {
        if (DateTime.TryParse(rep.ValidThru_28_DB, out var validThru27)
            && int.TryParse(rep.Year_DB, out var Year)
            && validThru27 < new DateTime(Year, 12, DateTime.DaysInMonth(Year, 12)))
        {
            errorList.Add(new CheckError
            {
                FormNum = "form_28",
                Row = "-",
                Column = "-",
                Value = "",
                Message = $"Срок действия разрешения на сброс в водные объекты истёк. Приведите сведения об актуальном разрешении."
            });
        }
    }

    #endregion
    #region Check_HeaderExpirationDate2

    private static void Check_HeaderExpirationDate2(List<CheckError> errorList, Report rep)
    {
        if (DateTime.TryParse(rep.ValidThru1_28_DB, out var validThru27)
            && int.TryParse(rep.Year_DB, out var Year)
            && validThru27 < new DateTime(Year, 12, DateTime.DaysInMonth(Year, 12)))
        {
            errorList.Add(new CheckError
            {
                FormNum = "form_28",
                Row = "-",
                Column = "-",
                Value = "",
                Message = $"Срок действия разрешения на сброс на рельеф местности истёк. Приведите сведения об актуальном разрешении."
            });
        }
    }

    #endregion
    #region Check_HeaderExpirationDate3

    private static void Check_HeaderExpirationDate3(List<CheckError> errorList, Report rep)
    {
        if (DateTime.TryParse(rep.ValidThru2_28_DB, out var validThru27)
            && int.TryParse(rep.Year_DB, out var Year)
            && validThru27 < new DateTime(Year, 12, DateTime.DaysInMonth(Year, 12)))
        {
            errorList.Add(new CheckError
            {
                FormNum = "form_28",
                Row = "-",
                Column = "-",
                Value = "",
                Message = $"Срок действия разрешения на передачу сточных вод в сети канализации истёк. Приведите сведения об актуальном разрешении."
            });
        }
    }

    #endregion
    #region Check_WasteMinmaxCur

    private static void Check_WasteMinmaxCur(List<CheckError> errorList, Report rep)
    {
        foreach (Form28 row in rep.Rows28)
        {
            double.TryParse(row.AllowedWasteRemovalVolume_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var allowedWasteValue);
            double.TryParse(row.RemovedWasteVolume_DB.ToLowerInvariant().Replace("(", "").Replace(")", "").Replace(".", ",").Replace("е", "e").Replace(" ", "")
                .Replace("e+", "e*").Replace("e", "e+").Replace("e+*", "e+"), out var factedWasteValue);
            if (allowedWasteValue < factedWasteValue)
            {
                errorList.Add(new CheckError
                {
                    FormNum = "form_28",
                    Row = $"{row.NumberInOrder_DB}",
                    Column = $"{GraphsList["RemovedWasteVolume_DB"]}",
                    Value = row.RemovedWasteVolume_DB,
                    Message = $"Величина фактического сброса превышает разрешенное. Проверьте правильность сведений."
                });
            }
        }
    }

    #endregion
    #region Check_PrevWastePercent

    private static void Check_PrevWastePercent(List<CheckError> errorList, Report rep, List<Form28> Rows28Prev)
    {
        foreach (Form28 rowCur in rep.Rows28)
        {
            foreach (Form28 rowPrev in Rows28Prev)
            {
                if (rowCur.WasteSourceName_DB.Trim().Equals(rowPrev.WasteSourceName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)
                    && rowCur.WasteRecieverName_DB.Trim().Equals(rowPrev.WasteRecieverName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)
                    && rowCur.RecieverTypeCode_DB.Trim().Equals(rowPrev.RecieverTypeCode_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)
                    && rowCur.PoolDistrictName_DB.Trim().Equals(rowPrev.PoolDistrictName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)
                    )
                {
                    if (!(double.TryParse(rowCur.AllowedWasteRemovalVolume_DB, out var wasteRemovalCur)
                        && double.TryParse(rowPrev.AllowedWasteRemovalVolume_DB, out var wasteRemovalPrev)
                        && wasteRemovalCur <= 1.2 * wasteRemovalPrev
                        && wasteRemovalPrev <= 1.2 * wasteRemovalCur))
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_28",
                            Row = $"{rowCur.NumberInOrder_DB}",
                            Column = $"{GraphsList["AllowedWasteRemovalVolume_DB"]}",
                            Value = $"{rowCur.AllowedWasteRemovalVolume_DB} (предыдущий год: {rowPrev.AllowedWasteRemovalVolume_DB})",
                            Message = $"Сведения о фактическом сбросе за предыдущий год существенно отличаются от данных представленных в отчете. Проверьте правильность сведений."
                        });
                    }
                    break;
                }
            }
        }
    }

    #endregion
    #region Check_PrevStats

    private static void Check_PrevStats(List<CheckError> errorList, Report rep, List<Form28> Rows28Prev)
    {
        foreach (Form28 rowCur in rep.Rows28)
        {
            foreach (Form28 rowPrev in Rows28Prev)
            {
                if (rowCur.WasteSourceName_DB.Trim().Equals(rowPrev.WasteSourceName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase))
                {
                    if (!(rowCur.WasteRecieverName_DB.Trim().Equals(rowPrev.WasteRecieverName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_28",
                            Row = $"{rowCur.NumberInOrder_DB}",
                            Column = $"{GraphsList["WasteRecieverName_DB"]}",
                            Value = $"{rowCur.WasteRecieverName_DB} (предыдущий год: {rowPrev.WasteRecieverName_DB})",
                            Message = $"Проверьте правильность представленных сведений в графе 3."
                        });
                    }
                    if (!(rowCur.RecieverTypeCode_DB.Trim().Equals(rowPrev.RecieverTypeCode_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_28",
                            Row = $"{rowCur.NumberInOrder_DB}",
                            Column = $"{GraphsList["RecieverTypeCode_DB"]}",
                            Value = $"{rowCur.RecieverTypeCode_DB} (предыдущий год: {rowPrev.RecieverTypeCode_DB})",
                            Message = $"Проверьте правильность представленных сведений в графе 3."
                        });
                    }
                    if (!(rowCur.PoolDistrictName_DB.Trim().Equals(rowPrev.PoolDistrictName_DB.Trim(), StringComparison.InvariantCultureIgnoreCase)))
                    {
                        errorList.Add(new CheckError
                        {
                            FormNum = "form_28",
                            Row = $"{rowCur.NumberInOrder_DB}",
                            Column = $"{GraphsList["PoolDistrictName_DB"]}",
                            Value = $"{rowCur.PoolDistrictName_DB} (предыдущий год: {rowPrev.PoolDistrictName_DB})",
                            Message = $"Проверьте правильность представленных сведений в графе 3."
                        });
                    }
                    break;
                }
            }
        }
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
        { "WasteSourceName_DB", "02 - Наименование, номер выпуска сточных вод" },
        { "WasteRecieverName_DB", "03 - Приемник отведенных вод - наименование" },
        { "RecieverTypeCode_DB", "04 - Приемник отведенных вод - код типа приемника" },
        { "PoolDistrictName_DB", "05 - Приемник отведенных вод - наименование бассейнового округа" },
        { "RemovedWasteVolume_DB", "06 - Допустимый объём водоотведения за год, тыс. куб. м" },
        { "AllowedWasteRemovalVolume_DB", "07 - Отведено за отчётный год, тыс. куб. м" },
    };

    #endregion

    #region ExcelGetFullPath

    /// <summary>
    /// Выводит сообщение, дающее выбор, открывать временную копию или сохранить файл.
    /// </summary>
    /// <param name="fileName">Имя файла.</param>
    /// <param name="cts">Токен.</param>
    /// <param name="progressBar">Окно прогрессбара.</param>
    /// <returns>Полный путь до файла и флаг, нужно ли открывать временную копию.</returns>
    private protected static async Task<(string fullPath, bool openTemp)> ExcelGetFullPath(string fileName, CancellationTokenSource cts,
        AnyTaskProgressBar? progressBar = null)
    {
        #region MessageSaveOrOpenTemp

        var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Сохранить" },
                    new ButtonDefinition { Name = "Открыть временную копию" }
                ],
                CanResize = true,
                ContentTitle = "Выгрузка в Excel",
                ContentHeader = "Уведомление",
                ContentMessage = "Что бы вы хотели сделать с данной выгрузкой?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

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

                            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                                {
                                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                                    ContentTitle = "Выгрузка в Excel",
                                    ContentHeader = "Ошибка",
                                    ContentMessage =
                                        $"Не удалось сохранить файл по пути: {fullPath}" +
                                        $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                        $"{Environment.NewLine}и используется другим процессом.",
                                    MinWidth = 400,
                                    MinHeight = 150,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(Desktop.MainWindow));

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
        return (fullPath, openTemp);
    }

    #endregion
}