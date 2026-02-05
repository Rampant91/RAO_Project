using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Properties;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using DynamicData;
using FirebirdSql.Data.FirebirdClient;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Экспорт отчёта в .RAODB
/// </summary>
public class ExportFormAsyncCommand : ExportRaodbBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        int repId;
        string formNum;
        if (parameter is ObservableCollectionWithItemPropertyChanged<IKey> param)
        {
            foreach (var item in param)
            {
                var a = DateTime.Now.Date;
                var aDay = a.Day.ToString();
                var aMonth = a.Month.ToString();
                if (aDay.Length < 2) aDay = $"0{aDay}";
                if (aMonth.Length < 2) aMonth = $"0{aMonth}";
                ((Report)item).ExportDate.Value = $"{aDay}.{aMonth}.{a.Year}";
            }
            repId = param.First().Id;
            formNum = ((Report)param.First()).FormNum.Value;
        }
        else if (parameter is Report report)
        {
            var a = DateTime.Now.Date;
            var aDay = a.Day.ToString();
            var aMonth = a.Month.ToString();
            if (aDay.Length < 2) aDay = $"0{aDay}";
            if (aMonth.Length < 2) aMonth = $"0{aMonth}";
            report.ExportDate.Value = $"{aDay}.{aMonth}.{a.Year}";

            repId = report.Id;
            formNum = report.FormNum.Value;
        }
        else return;
        var cts = new CancellationTokenSource();


        

        #region ProgressBarInitialization

        await Dispatcher.UIThread.InvokeAsync(() => ProgressBar = new AnyTaskProgressBar(cts));
        var progressBar = ProgressBar;
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        progressBarVM.ExportType = "Экспорт_RAODB";
        progressBarVM.ExportName = "Выгрузка отчёта";
        progressBarVM.ValueBar = 5;
        var loadStatus = "Создание временной БД";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        var dbReadOnlyPath = CreateTempDataBase();

        var dt = DateTime.Now;
        var fileNameTmp = $"Report_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        #region Progress = 10

        loadStatus = "Загрузка данных организации";
        progressBarVM.ValueBar = 10;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        #region GetReportWithoutForms

        var reportWithoutRows = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows40)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows50)
            .First(x => x.Id == repId);

        #endregion

        #region Progress = 15

        if (formNum.Split('.')[0] is "1" or "2")
        {
            progressBarVM.ExportName = $"Выгрузка отчёта {reportWithoutRows.Reports.Master_DB.RegNoRep.Value}_" +
                                   $"{reportWithoutRows.Reports.Master_DB.OkpoRep.Value}_" +
                                   $"{reportWithoutRows.FormNum_DB}_" +
                                   $"{reportWithoutRows.StartPeriod_DB}_" +
                                   $"{reportWithoutRows.EndPeriod_DB}";
        }
        else if (formNum.Split('.')[0] is "4")
        {
            progressBarVM.ExportName = $"Выгрузка отчёта " +
                                   $"{reportWithoutRows.FormNum_DB}_" +
                                   $"{reportWithoutRows.Year_DB}_";
        }
        loadStatus = "Загрузка отчёта";
        progressBarVM.ValueBar = 15;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        //TODO
        //Нужно переделать с использованием фабрики, но так, чтобы работала асинхронность (почему-то запрос к ДБ в ней выполняется синхронно)

        #region GetReportWithForm

        var exportReport = await dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(report => report.Reports)
            .Include(x => x.Rows11.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows12.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows13.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows14.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows15.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows16.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows17.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows18.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows19.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows21.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows22.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows23.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows24.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows25.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows26.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows27.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows28.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows29.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows210.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows211.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows212.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows41.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows51.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows52.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows53.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows54.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows55.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows56.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows57.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Notes.OrderBy(x => x.Order))
            .FirstAsync(x => x.Id == repId, cancellationToken: cts.Token);

        if (exportReport.Rows.Count is 0 && formNum is not "2.6")   //Для формы 2.6 допустимы отчёты без строчек.
        {
            #region FailedToExportReportMessage

            await Dispatcher.UIThread.InvokeAsync(() =>
                MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в .raodb",
                        ContentHeader = "Ошибка",
                        ContentMessage = "Не удалось выгрузить отчёт, поскольку в нём отсутствуют заполненные строчки.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    }).ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        #endregion

        #region Progress = 25

        loadStatus = "Обновление даты выгрузки";
        progressBarVM.ValueBar = 25;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})"; 
        
        #endregion

        var dtDay = dt.Day.ToString();
        var dtMonth = dt.Month.ToString();
        if (dtDay.Length < 2) dtDay = $"0{dtDay}";
        if (dtMonth.Length < 2) dtMonth = $"0{dtMonth}";
        exportReport.ExportDate.Value = $"{dtDay}.{dtMonth}.{dt.Year}";

        await StaticConfiguration.DBModel.SaveChangesAsync(cts.Token);

        var fullPathTmp = Path.Combine(BaseVM.TmpDirectory, $"{fileNameTmp}_exp.RAODB");

        Reports orgWithExpForm = new()
        {
            Master = reportWithoutRows.Reports.Master,
            Report_Collection = new ObservableCollectionWithItemPropertyChanged<Report>([exportReport])
        };

        exportReport.Reports = orgWithExpForm;

        progressBarVM.SetProgressBar(28, "Проверка отчёта");
        await CheckForm(exportReport, cts, progressBar);

        var filename = reportWithoutRows.Reports.Master_DB.FormNum_DB switch
        {
            "1.0" =>
                StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.RegNoRep.Value) +
                $"_{StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.OkpoRep.Value)}" +
                $"_{exportReport.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(exportReport.StartPeriod_DB)}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(exportReport.EndPeriod_DB)}" +
                $"_{exportReport.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            "2.0" when orgWithExpForm.Master.Rows20.Count > 0 =>
                StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.RegNoRep.Value) +
                $"_{StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.OkpoRep.Value)}" +
                $"_{exportReport.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(exportReport.Year_DB)}" +
                $"_{exportReport.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            "4.0" when orgWithExpForm.Master.Rows40.Count > 0 =>
                $"{orgWithExpForm.Master.Rows40.OrderBy(r =>r.NumberInOrder_DB).ToList()[0].CodeSubjectRF_DB}" +
                $"_{exportReport.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(exportReport.Year_DB)}" +
                $"_{exportReport.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            "5.0" when orgWithExpForm.Master.Rows50.Count > 0 =>
                $"{exportReport.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(exportReport.Year_DB)}" +
                $"_{exportReport.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            _ => throw new ArgumentOutOfRangeException()
        };

        var folderPath = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;

        var fullPath = Path.Combine(folderPath, $"{filename}.RAODB");

        if (File.Exists(fullPath))
        {
            try
            {
                File.Delete(fullPath);
            }
            catch (Exception)
            {
                #region FailedToSaveFileMessage

                await Dispatcher.UIThread.InvokeAsync(() =>
                    MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = ButtonEnum.Ok,
                            ContentTitle = "Выгрузка в .raodb",
                            ContentHeader = "Ошибка",
                            ContentMessage =
                                "Не удалось сохранить файл по пути:" +
                                $"{Environment.NewLine}{fullPath}" +
                                $"{Environment.NewLine}" +
                                $"{Environment.NewLine}Файл с таким именем уже существует в этом расположении" +
                                $"{Environment.NewLine}и используется другим процессом.",
                            MinWidth = 400,
                            MinHeight = 150,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        }).ShowDialog(Desktop.MainWindow));

                #endregion

                await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
            }
        }

        await using var tempDb = new DBModel(fullPathTmp);

        #region Progress = 30

        loadStatus = "Создание базы данных";
        progressBarVM.ValueBar = 30;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})"; 

        #endregion

        await tempDb.Database.MigrateAsync(cancellationToken: cts.Token);

        #region Progress = 40

        loadStatus = "Добавление коллекций организации";
        progressBarVM.ValueBar = 40;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})"; 
        
        #endregion

        await tempDb.ReportsCollectionDbSet.AddAsync(orgWithExpForm, cts.Token);
        if (!tempDb.DBObservableDbSet.Any())
        {
            tempDb.DBObservableDbSet.Add(new DBObservable());
            tempDb.DBObservableDbSet.Local.First().Reports_Collection.AddRange(tempDb.ReportsCollectionDbSet.Local);
        }

        #region Progress = 50
        
        loadStatus = "Сохранение данных";
        progressBarVM.ValueBar = 50;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})"; 
        
        #endregion

        await tempDb.SaveChangesAsync(cts.Token);

        var t = tempDb.Database.GetDbConnection() as FbConnection;
        await t.CloseAsync();
        await t.DisposeAsync();

        await tempDb.Database.CloseConnectionAsync();

        #region Progress = 95

        loadStatus = "Копирование файла в папку назначения";
        progressBarVM.ValueBar = 95;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})"; 
        
        #endregion

        try
        {
            File.Copy(fullPathTmp, fullPath);
            File.Delete(fullPathTmp);
        }
        catch (Exception e)
        {
            #region FailedCopyFromTempMessage

            await Dispatcher.UIThread.InvokeAsync(() =>
                MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в .RAODB",
                        ContentHeader = "Ошибка",
                        ContentMessage = "При копировании файла базы данных из временной папки возникла ошибка." +
                                         $"{Environment.NewLine}Экспорт не выполнен.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }).ShowDialog(Desktop.MainWindow));

            #endregion

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }

        //Создаёт .zip архив рядом с файлом выгрузки.

        //var sourceFile = fullPath;
        //var parentDirectory = Path.GetDirectoryName(sourceFile);
        //if (!string.IsNullOrEmpty(parentDirectory))
        //{
        //    var zipName = Path.GetFileNameWithoutExtension(sourceFile) + ".zip";
        //    var destinationArchive = Path.Combine(parentDirectory, zipName);
        //    if (File.Exists(destinationArchive))
        //    {
        //        try { File.Delete(destinationArchive); } catch { }
        //    }
        //    using var archive = ZipFile.Open(destinationArchive, ZipArchiveMode.Create);
        //    archive.CreateEntryFromFile(sourceFile, Path.GetFileName(sourceFile), CompressionLevel.SmallestSize);
        //}

        #region Progress = 100

        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})"; 
        
        #endregion

        if (!cts.IsCancellationRequested)
        {
            string? answer = null;
            if (orgWithExpForm.Master.FormNum_DB is "1.0" or "2.0")
            {
                #region ExportCompliteMessage 1.0, 2.0

                answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Открыть расположение файла" }
                        ],
                        ContentTitle = "Выгрузка в .raodb",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            "Файл экспорта формы сохранен по пути:" +
                            $"{Environment.NewLine}{fullPath}" +
                            $"{Environment.NewLine}" +
                            $"{Environment.NewLine}Регистрационный номер - {orgWithExpForm.Master.RegNoRep.Value}" +
                            $"{Environment.NewLine}ОКПО - {orgWithExpForm.Master.OkpoRep.Value}" +
                            $"{Environment.NewLine}Сокращенное наименование - {orgWithExpForm.Master.ShortJurLicoRep.Value}" +
                            $"{Environment.NewLine}" +
                            $"{Environment.NewLine}Номер формы - {exportReport.FormNum_DB}" +
                            $"{Environment.NewLine}Начало отчетного периода - {exportReport.StartPeriod_DB}" +
                            $"{Environment.NewLine}Конец отчетного периода - {exportReport.EndPeriod_DB}" +
                            $"{Environment.NewLine}Дата выгрузки - {exportReport.ExportDate_DB}" +
                            $"{Environment.NewLine}Номер корректировки - {exportReport.CorrectionNumber_DB}" +
                            $"{Environment.NewLine}Количество строк - {exportReport.Rows.Count}{InventoryCheck(exportReport)}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }).ShowDialog(Desktop.MainWindow));

                #endregion
            }
            else if (orgWithExpForm.Master.FormNum_DB is "4.0")
            {
                #region ExportCompliteMessage 4.0

                answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Открыть расположение файла" }
                        ],
                        ContentTitle = "Выгрузка в .raodb",
                        ContentHeader = "Уведомление",
                        ContentMessage =
                            "Файл экспорта формы сохранен по пути:" +
                            $"{Environment.NewLine}{fullPath}" +
                            $"{Environment.NewLine}" +
                            $"{Environment.NewLine}Сокращенное наименование - {orgWithExpForm.Master.Rows40[0].SubjectRF.Value}" +
                            $"{Environment.NewLine}" +
                            $"{Environment.NewLine}Номер формы - {exportReport.FormNum_DB}" +
                            $"{Environment.NewLine}Год - {exportReport.Year_DB}" +
                            $"{Environment.NewLine}Дата выгрузки - {exportReport.ExportDate_DB}" +
                            $"{Environment.NewLine}Номер корректировки - {exportReport.CorrectionNumber_DB}" +
                            $"{Environment.NewLine}Количество строк - {exportReport.Rows.Count}{InventoryCheck(exportReport)}",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }).ShowDialog(Desktop.MainWindow));

                #endregion
            }

            if (answer is "Открыть расположение файла")
            {
                Process.Start("explorer", folderPath);
            }
        }

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }

    private static async Task CheckForm(Report exportReport, CancellationTokenSource cts, AnyTaskProgressBar progressBar)
    {
        var errorList = new List<CheckError>();
        try
        {
            errorList.Add(exportReport.FormNum_DB switch
            {
                "1.1" => CheckF11.Check_Total(exportReport.Reports, exportReport),
                "1.2" => CheckF12.Check_Total(exportReport.Reports, exportReport),
                "1.3" => CheckF13.Check_Total(exportReport.Reports, exportReport),
                "1.4" => CheckF14.Check_Total(exportReport.Reports, exportReport),
                "1.5" => CheckF15.Check_Total(exportReport.Reports, exportReport),
                "1.6" => CheckF16.Check_Total(exportReport.Reports, exportReport),
                "1.7" => CheckF17.Check_Total(exportReport.Reports, exportReport),
                "1.8" => CheckF18.Check_Total(exportReport.Reports, exportReport),
                //"2.1" => await new CheckF21().AsyncExecute(exportReport),
                //"2.2" => await new CheckF22().AsyncExecute(exportReport),
                //"2.3" => await new CheckF23().AsyncExecute(exportReport),
                //"2.4" => await new CheckF24().AsyncExecute(exportReport),
                //"2.5" => await new CheckF25().AsyncExecute(exportReport),
                //"2.6" => await new CheckF26().AsyncExecute(exportReport),
                //"2.7" => await new CheckF27().AsyncExecute(exportReport),
                //"2.8" => await new CheckF28().AsyncExecute(exportReport),
                //"2.9" => await new CheckF29().AsyncExecute(exportReport),
                //"2.10" => await new CheckF210().AsyncExecute(exportReport),
                //"2.11" => await new CheckF211().AsyncExecute(exportReport),
                _ => []
            });
        }
        catch (Exception ex)
        {
            //ignored
        }

        if (!errorList.Any(x => x.IsCritical)) return;

        if (!Settings.Default.AppLaunchedInNorao)
        {
            #region ExportTerminatedDueToCriticalErrors

            await Dispatcher.UIThread.InvokeAsync(() =>
                MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в .raodb",
                        ContentHeader = "Ошибка",
                        ContentMessage = "Выгрузка отчёта невозможна из-за наличия в нём критических ошибок (выделены красным)." +
                                         $"{Environment.NewLine}Устраните ошибки и повторите операцию выгрузки.",
                        MinWidth = 250,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }).ShowDialog(Desktop.MainWindow));

            #endregion

            await Dispatcher.UIThread.InvokeAsync(() => new Views.CheckForm(new ChangeOrCreateVM(exportReport.FormNum_DB, exportReport), errorList));

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
        else
        {
            #region ReportHasCriticalErrors

            var answer = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да" },
                        new ButtonDefinition { Name = "Отмена" }
                    ],
                    ContentTitle = "Выгрузка в .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"В отчёте присутствуют критические ошибки (выделены красным). " +
                                     $"{Environment.NewLine}Всё равно выгрузить отчёт?",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            if (answer is "Да") return;
            
            await Dispatcher.UIThread.InvokeAsync(() => new Views.CheckForm(new ChangeOrCreateVM(exportReport.FormNum_DB, exportReport), errorList));

            await CancelCommandAndCloseProgressBarWindow(cts, progressBar);
        }
    }

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

    #region InventoryCheck

    /// <summary>
    /// Проверяет, является ли отчёт инвентаризационным (наличие в отчёте кода операции 10 у форм 1.1)
    /// </summary>
    /// <param name="rep">Отчёт</param>
    /// <returns>Заглавные буквы - все формы с кодом 10, обычные - хотя бы одна, пустая строчка - код 10 отсутствует</returns>
    private static string InventoryCheck(Report? rep)
    {
        if (rep is null)
        {
            return "";
        }

        var countCode10 = 0;
        foreach (var key in rep.Rows)
        {
            if (key is Form1 { OperationCode_DB: "10" })
            {
                countCode10++;
            }
        }

        return countCode10 == rep.Rows.Count
            ? " (ИНВ)"
            : countCode10 > 0
                ? " (инв)"
                : "";
    }

    #endregion
}