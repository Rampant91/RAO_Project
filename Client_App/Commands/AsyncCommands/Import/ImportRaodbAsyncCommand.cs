using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using System.Collections.Generic;
using System.Linq;
using Client_App.Interfaces.Logger;
using MessageBox.Avalonia.Enums;
using Models.DTO;
using Avalonia.Threading;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels;

namespace Client_App.Commands.AsyncCommands.Import;

//  Импорт -> Из RAODB
public class ImportRaodbAsyncCommand(MainWindowVM mainWindowVM) : ImportBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        RepsWhereTitleFormCheckIsCancel.Clear();
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        string[] extensions = ["raodb", "RAODB"];
        var answer = await GetSelectedFilesFromDialog("RAODB", extensions);
        if (answer is null) return;
        SkipNewOrg = false;
        SkipInter = false;
        SkipLess = false;
        SkipNew = false;
        SkipReplace = false;
        HasMultipleReport = false;
        AtLeastOneImportDone = false;

        var countReadFiles = answer.Length;

        var impReportsList = new List<Reports>();
        foreach (var path in answer) // Для каждого импортируемого файла
        {
            if (path == "") continue;
            TmpImpFilePath = GetRaoFileName();
            SourceFile = new FileInfo(path);
            SourceFile.CopyTo(TmpImpFilePath, true);

            var reportsCollection = new List<Reports>();
            var fileIsCorrupted = false;
            try
            {
                reportsCollection = await GetReportsFromDataBase(TmpImpFilePath);
            }
            catch
            {
                fileIsCorrupted = true;
            }

            if (fileIsCorrupted || reportsCollection.Count == 0)
            {
                #region MessageFailedToReadFile

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Импорт из .raodb",
                        ContentHeader = "Ошибка",
                        ContentMessage =
                            $"Не удалось прочесть файл {path}," +
                            $"{Environment.NewLine}файл поврежден или не содержит данных.",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow)); 

                #endregion

                countReadFiles--;
                continue;
            }
            if (!HasMultipleReport)
            {
                HasMultipleReport = reportsCollection.Sum(x => x.Report_Collection.Count) > 1 || answer.Length > 1;
            }

            foreach (var impReps in reportsCollection) // Для каждой импортируемой организации
            {
                var dateTime = DateTime.Now;

                impReportsList.Add(impReps);
                await impReps.SortAsync();
                await RestoreReportsOrders(impReps);
                if (impReps.Master.Rows10.Count != 0)
                {
                    impReps.Master_DB.ReportChangedDate = dateTime;
                    impReps.Master.Rows10[1].RegNo_DB = impReps.Master.Rows10[0].RegNo_DB;
                }

                if (impReps.Master.Rows20.Count != 0)
                {
                    impReps.Master_DB.ReportChangedDate = dateTime;
                    impReps.Master.Rows20[1].RegNo_DB = impReps.Master.Rows20[0].RegNo_DB;
                }

                if (impReps.Master.Rows40.Count != 0)
                {
                    impReps.Master_DB.ReportChangedDate = dateTime;
                }

                var baseReps11 = GetReports11FromLocalEqual(impReps);
                var baseReps21 = GetReports21FromLocalEqual(impReps);
                var baseReps41 = GetReports41FromLocalEqual(impReps);
                FillEmptyRegNo(ref baseReps11);
                FillEmptyRegNo(ref baseReps21);
                impReps.CleanIds();
                ProcessIfNoteOrder0(impReps);

                ImpRepFormCount = impReps.Report_Collection.Count;
                ImpRepFormNum = impReps.Master.FormNum_DB;
                if (impReps.Master.OkpoRep!= null)
                    BaseRepsOkpo = impReps.Master.OkpoRep.Value;
                if (impReps.Master.RegNoRep != null)
                    BaseRepsRegNum = impReps.Master.RegNoRep.Value;
                if (impReps.Master.ShortJurLicoRep != null)
                    BaseRepsShortName = impReps.Master.ShortJurLicoRep.Value;

                foreach (var key in impReps.Report_Collection)
                {
                    var report = (Report)key;
                    report.ReportChangedDate = dateTime;
                }
                var impRepsReportList = impReps.Report_Collection.ToList();
                if (baseReps11 != null)
                {
                    await ProcessIfHasReports11(baseReps11, impReps, impRepsReportList);
                }
                else if (baseReps21 != null)
                {
                    await ProcessIfHasReports21(baseReps21, impReps, impRepsReportList);
                }
                else if (baseReps41 != null)
                {
                    await ProcessIfHasReports41(baseReps41, impReps, impRepsReportList);
                }
                else if (baseReps11 == null && baseReps21 == null && baseReps41 == null)
                {
                    #region AddNewOrg

                    var an = "Добавить";
                    if (!SkipNewOrg)
                    {
                        if (answer.Length > 1 || reportsCollection.Count > 1)
                        {
                            #region MessageNewOrg

                            an = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions =
                                    [
                                        new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                        new ButtonDefinition { Name = "Да для всех" },
                                        new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                    ],
                                    ContentTitle = "Импорт из .raodb",
                                    ContentHeader = "Уведомление",
                                    ContentMessage =
                                        $"Будет добавлена новая организация ({ImpRepFormNum}) содержащая {ImpRepFormCount} форм отчетности." +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                        $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}" +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений " +
                                        $"{Environment.NewLine}импортировать все новые организации.",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(Desktop.MainWindow));

                            #endregion

                            if (an is "Да для всех") SkipNewOrg = true;
                        }
                        else
                        {
                            #region MessageNewOrg

                            an = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions =
                                    [
                                        new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                        new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                    ],
                                    ContentTitle = "Импорт из .raodb",
                                    ContentHeader = "Уведомление",
                                    ContentMessage =
                                        $"Будет добавлена новая организация ({ImpRepFormNum}) содержащая {ImpRepFormCount} форм отчетности." +
                                        $"{Environment.NewLine}" +
                                        $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}" +
                                        $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}" +
                                        $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}",
                                    MinWidth = 400,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                                })
                                .ShowDialog(Desktop.MainWindow));

                            #endregion
                        }
                    }

                    if (an is "Добавить" or "Да для всех")
                    {
                        ReportsStorage.LocalReports.Reports_Collection.Add(impReps);
                        AtLeastOneImportDone = true;

                        #region LoggerImport

                        var sortedRepList = impReps.Report_Collection
                            .OrderBy(x => x.FormNum_DB)
                            .ThenBy(x => DateOnly.TryParse(x.StartPeriod_DB, out var stDate) ? stDate : DateOnly.MaxValue)
                            .ThenBy(x => DateOnly.TryParse(x.EndPeriod_DB, out var endDate) ? endDate : DateOnly.MaxValue)
                            .ToList();
                        foreach (var rep in sortedRepList)
                        {
                            ImpRepCorNum = rep.CorrectionNumber_DB;
                            ImpRepFormCount = rep.Rows.Count;
                            ImpRepFormNum = rep.FormNum_DB;
                            ImpRepStartPeriod = rep.StartPeriod_DB;
                            ImpRepEndPeriod = rep.EndPeriod_DB;
                            Act = "\t\t\t";
                            LoggerImportDTO = new LoggerImportDTO
                            {
                                Act = Act, CorNum = ImpRepCorNum, CurrentLogLine = CurrentLogLine, EndPeriod = ImpRepEndPeriod,
                                FormCount = ImpRepFormCount, FormNum = ImpRepFormNum, StartPeriod = ImpRepStartPeriod,
                                Okpo = BaseRepsOkpo, OperationDate = OperationDate, RegNum = BaseRepsRegNum,
                                ShortName = BaseRepsShortName, SourceFileFullPath = SourceFile!.FullName, Year = ImpRepYear
                            };
                            ServiceExtension.LoggerManager.Import(LoggerImportDTO);
                            IsFirstLogLine = false;
                            CurrentLogLine++;
                        }

                        #endregion
                    }

                    #endregion
                }

                switch (impReps.Master_DB.FormNum_DB)
                {
                    case "1.0":
                        await impReps.Master_DB.Rows10.QuickSortAsync();
                        break;
                    case "2.0":
                        await impReps.Master_DB.Rows20.QuickSortAsync();
                        break;
                    case "4.0":
                        await impReps.Master_DB.Rows40.QuickSortAsync();
                        break;
                }
            }

            // Если убрать сохранение, то не перезаписывается базовый отчёт (номер корректировки) и при импорте нескольких файлов одинакового отчёта,
            // но с разными номерами, в организации появлялись дубли, вместо перезаписи имеющегося отчёта.
            try
            {
                await StaticConfiguration.DBModel.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        try
        {
            var comparator = new CustomReportsComparer();
            var tmpReportsList = new List<Reports>(ReportsStorage.LocalReports.Reports_Collection);
            if (tmpReportsList.All(x => x.Master_DB.RegNoRep != null && x.Master_DB.OkpoRep != null))
            {
                var tmpReportsOrderedEnum = tmpReportsList
                    .OrderBy(x => x.Master_DB.RegNoRep.Value, comparator)
                    .ThenBy(x => x.Master_DB.OkpoRep.Value, comparator);

                ReportsStorage.LocalReports.Reports_Collection.Clear();
                ReportsStorage.LocalReports.Reports_Collection.AddRange(tmpReportsOrderedEnum);
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Warning(msg);
            return;
        }

        //await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();

        try
        {
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            #region MessageImportError

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Импорт из .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = "При сохранении импортированных данных возникла ошибка.\n",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        //try
        //{
        //    if (impReportsList.All(x => x.Master_DB.FormNum_DB is "1.0" or "2.0"))
        //    {
        //        await SetDataGridPage(impReportsList);
        //    }
        //}
        //catch {}

        var suffix = answer.Length.ToString().EndsWith('1') && !answer.Length.ToString().EndsWith("11")
                ? "а"
                : "ов";

        if (AtLeastOneImportDone)
        {
            #region MessageImportDone
            
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт {countReadFiles} из {answer.Length} файл{suffix} .raodb успешно завершен.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            var mainWindowVM = Desktop.MainWindow.DataContext as MainWindowVM;
            mainWindowVM.UpdateReportsCollection();
            mainWindowVM.UpdateOrgsPageInfo();
            mainWindowVM.UpdateTotalReportCount();
        }
        else
        {
            #region MessageImportCancel

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт из {answer.Length} файл{suffix} .raodb был отменен.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion
        }

    }

    #region GetReportsFromDataBase

    private static async Task<List<Reports>> GetReportsFromDataBase(string file)
    {
        await using DBModel db = new(file);

        #region Test Version

        var t = await db.Database.GetPendingMigrationsAsync();
        var a = db.Database.GetMigrations();
        var b = await db.Database.GetAppliedMigrationsAsync();

        #endregion

        await db.Database.MigrateAsync();
        await db.LoadTablesAsync();
        await InitializationAsyncCommand.ProcessDataBaseFillEmpty(db);

        return db.DBObservableDbSet.Local.First().Reports_Collection.ToList().Count != 0
            ? db.DBObservableDbSet.Local.First().Reports_Collection.ToList()
            : await db.ReportsCollectionDbSet.ToListAsync();
    }

    #endregion

    #region RestoreReportsOrders

    private static async Task RestoreReportsOrders(Reports item)
    {
        if (item.Master_DB.FormNum_DB == "1.0")
        {
            if (item.Master_DB.Rows10[0].Id > item.Master_DB.Rows10[1].Id)
            {
                if (item.Master_DB.Rows10[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[0].NumberInOrder_DB = 2;
                }

                if (item.Master_DB.Rows10[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[1].NumberInOrder_DB = item.Master_DB.Rows10[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows10.Sorted = false;
                await item.Master_DB.Rows10.QuickSortAsync();
            }
            else
            {
                if (item.Master_DB.Rows10[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[0].NumberInOrder_DB = 1;
                }

                if (item.Master_DB.Rows10[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows10[1].NumberInOrder_DB = item.Master_DB.Rows10[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows10.Sorted = false;
                await item.Master_DB.Rows10.QuickSortAsync();
            }
        }

        if (item.Master_DB.FormNum_DB == "2.0")
        {
            if (item.Master_DB.Rows20[0].Id > item.Master_DB.Rows20[1].Id)
            {
                if (item.Master_DB.Rows20[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[0].NumberInOrder_DB = 2;
                }

                if (item.Master_DB.Rows20[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[1].NumberInOrder_DB = item.Master_DB.Rows20[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows20.Sorted = false;
                await item.Master_DB.Rows20.QuickSortAsync();
            }
            else
            {
                if (item.Master_DB.Rows20[0].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[0].NumberInOrder_DB = 1;
                }

                if (item.Master_DB.Rows20[1].NumberInOrder_DB == 0)
                {
                    item.Master_DB.Rows20[1].NumberInOrder_DB = item.Master_DB.Rows20[1].NumberInOrder_DB == 2
                        ? 1
                        : 2;
                }

                item.Master_DB.Rows20.Sorted = false;
                await item.Master_DB.Rows20.QuickSortAsync();
            }
        }
    }

    #endregion
}