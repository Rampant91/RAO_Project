using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Resources.CustomComparers;
using Client_App.ViewModels;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views.Messages;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static Client_App.ViewModels.Messages.SelectReportsMessageWindowVM;

namespace Client_App.Commands.AsyncCommands.Import;

//  Импорт -> Из RAODB
public class ImportRaodbAsyncCommand : ImportBaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlBaseVM;

    public ImportRaodbAsyncCommand() { }

    public ImportRaodbAsyncCommand(FormsTabControlBaseVM formsTabControlBaseVM)
    {
        _formsTabControlBaseVM = formsTabControlBaseVM;

        formsTabControlBaseVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName is nameof(FormsTabControlBaseVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) =>
        parameter switch
        {
            Reports => true,
            "Selected" => _formsTabControlBaseVM.SelectedReports is not null,
            "Auto" or "FromList" => true,
            _ => false
        };

    public override async Task AsyncExecute(object? parameter)
    {
        RepsWhereTitleFormCheckIsCancel.Clear();
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        string[] extensions = ["raodb", "RAODB"];
        var answer = await GetSelectedFilesFromDialog("RAODB", extensions);
        if (answer is null) return;
        ClearImportSummaryReports();
        var countReadFiles = 0;
        var importSummaryShown = false;
        try
        {
            SkipNewOrg = false;
            SkipInter = false;
            SkipReplace = false;
            HasMultipleReport = false;
            AtLeastOneImportDone = false;

            countReadFiles = answer.Length;

            var impReportsList = new List<Reports>();
            foreach (var path in answer) // Для каждого импортируемого файла
            {
                if (path == "") continue;
                TmpImpFilePath = GetRaoFileName();
                SourceFile = new FileInfo(path);
                SourceFile.CopyTo(TmpImpFilePath, true);

                var repsList = new List<Reports>();
                var fileIsCorrupted = false;
                try
                {
                    repsList = await GetReportsFromDataBase(TmpImpFilePath);
                }
                catch
                {
                    fileIsCorrupted = true;
                }

                if (fileIsCorrupted || repsList.Count == 0)
                {
                    #region MessageFailedToReadFile

                await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Импорт из .raodb",
                        ContentHeader = "Ошибка",
                        ContentMessage =
                            $"Не удалось прочесть файл {path}," +
                            $"{Environment.NewLine}файл поврежден или не содержит данных.",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    }).ShowWindowDialogAsync(Desktop.MainWindow)); 

                #endregion

                    countReadFiles--;
                    continue;
                }
                if (!HasMultipleReport)
                {
                    HasMultipleReport = repsList.Sum(x => x.Report_Collection.Count) > 1 || answer.Length > 1;
                }

                foreach (var impReps in repsList) // Для каждой импортируемой организации
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

                    if (impReps.Master.Rows40.Count != 0
                        || impReps.Master.Rows50.Count !=0)
                    {
                        impReps.Master_DB.ReportChangedDate = dateTime;
                    }

                    Reports? baseReps11;
                    Reports? baseReps21;
                    Reports? baseReps41;
                    Reports? baseReps51;
                    var executeMode = parameter switch
                    {
                        Reports => "Selected",
                        string mode => mode,
                        _ => null
                    };

                    switch (executeMode)
                    {
                        case "Auto":
                        {
                            baseReps11 = GetReports11FromLocalEqual(impReps);
                            baseReps21 = GetReports21FromLocalEqual(impReps);
                            baseReps41 = GetReports41FromLocalEqual(impReps);
                            baseReps51 = GetReports51FromLocalEqual(impReps);

                            break;
                        }
                        case "Selected":
                        {
                            var selectedReports = parameter as Reports ?? _formsTabControlBaseVM.SelectedReports;
                            if (selectedReports is null) return;
                            var selectedReportsInfo = new OrganizationInfo
                            {
                                RegNum = selectedReports.Master_DB.RegNoRep.Value,
                                Okpo = selectedReports.Master_DB.OkpoRep.Value
                            };

                            var impRepsFromDb = await GetSelectedReportsFromDB(selectedReportsInfo, impReps.Master_DB.FormNum_DB);

                            baseReps11 = GetReports11FromLocalEqual(impRepsFromDb);
                            baseReps21 = GetReports21FromLocalEqual(impRepsFromDb);
                            baseReps41 = GetReports41FromLocalEqual(impRepsFromDb);
                            baseReps51 = GetReports51FromLocalEqual(impRepsFromDb);

                            break;
                        }
                        case "FromList":
                        {
                            var localRepsList = await GetReportsListFromDB(impReps.Master_DB.FormNum_DB);
                            var currentReportIndex = impReportsList.IndexOf(impReps) + 1;
                            var selectReportsMessageWindow = new SelectReportsMessageWindow(localRepsList, SourceFile!.Name, impReportsList.Count, currentReportIndex, impReps);
                            var selectedReports = await selectReportsMessageWindow.ShowDialog<OrganizationInfo>(Desktop.MainWindow);
                            if (selectedReports is null) return;
                            var impRepsFromDb = await GetSelectedReportsFromDB(selectedReports, impReps.Master_DB.FormNum_DB);
                            
                            baseReps11 = GetReports11FromLocalEqual(impRepsFromDb);
                            baseReps21 = GetReports21FromLocalEqual(impRepsFromDb);
                            baseReps41 = GetReports41FromLocalEqual(impRepsFromDb);
                            baseReps51 = GetReports51FromLocalEqual(impRepsFromDb);

                            break;
                        }
                        default: return;
                    }


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
                    else if (baseReps51 != null)
                    {
                        await ProcessIfHasReports51(baseReps51, impReps, impRepsReportList);
                    }
                    else if (baseReps11 == null && baseReps21 == null && baseReps41 == null && baseReps51 == null)
                    {
                        #region AddNewOrg

                        var an = "Добавить";
                        if (!SkipNewOrg)
                        {
                            if (answer.Length > 1 || repsList.Count > 1)
                            {
                                #region MessageNewOrg

                                an = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                                    .GetMessageBoxCustom(new MessageBoxCustomParams
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
                                    }).ShowWindowDialogAsync(Desktop.MainWindow));

                                #endregion

                                if (an is "Да для всех") SkipNewOrg = true;
                            }
                            else
                            {
                                #region MessageNewOrg

                                an = await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                                    .GetMessageBoxCustom(new MessageBoxCustomParams
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
                                    }).ShowWindowDialogAsync(Desktop.MainWindow));

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
                                if(rep.Rows!=null)
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
                                RecordImportedReport(impReps);
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
                        case "5.0":
                            await impReps.Master_DB.Rows50.QuickSortAsync();
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

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .xlsx",
                    ContentHeader = "Уведомление",
                    ContentMessage = "При сохранении импортированных данных возникла ошибка.\n",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion

                return;
            }
        }
        finally
        {
            importSummaryShown = await ShowImportSummaryMessageIfAnyAsync();
        }

        //try
        //{
        //    if (impReportsList.All(x => x.Master_DB.FormNum_DB is "1.0" or "2.0"))
        //    {
        //        await SetDataGridPage(impReportsList);
        //    }
        //}
        //catch {}

        if (AtLeastOneImportDone)
        {
            var mainWindowVM = Desktop.MainWindow.DataContext as MainWindowVM;
            mainWindowVM.UpdateReportsCollection();
            mainWindowVM.UpdateOrgsPageInfo();
            mainWindowVM.UpdateTotalReportCount();
        }
        else if (!importSummaryShown)
        {
            #region MessageImportCancel

            var suffix = answer.Length.ToString() is [.., '1'] && !answer.Length.ToString().EndsWith("11")
                ? "а"
                : "ов";

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт из {answer.Length} файл{suffix} .raodb был отменен.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion
        }

    }

    #region GetReportsFromDataBase

    private static async Task<List<Reports>> GetReportsFromDataBase(string file)
    {
        await using DBModel db = new(file);

        #region Test Version

        //var t = await db.Database.GetPendingMigrationsAsync();
        //var a = db.Database.GetMigrations();
        //var b = await db.Database.GetAppliedMigrationsAsync();

        #endregion

        await db.MigrateDatabaseAsync();
        await db.LoadTablesAsync();

        // Источник — таблица Reports, не DBObservable:
        // в старых выгрузках DBObservableId часто null — организации не в Reports_Collection корня,
        // но Master и отчёты в БД есть. Local после LoadTables сохраняет Report_Collection (fixup).
        var reports = db.ReportsCollectionDbSet.Local.ToList();
        if (reports.Count == 0)
        {
            reports = await db.ReportsCollectionDbSet
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows40)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows50)
                .Include(x => x.Report_Collection)
                .ToListAsync();
        }
        else
        {
            foreach (var reps in reports)
            {
                if (reps.Master_DB is null)
                {
                    await db.Entry(reps).Reference(x => x.Master_DB).LoadAsync();
                }

                if (reps.Report_Collection.Count == 0)
                {
                    await db.Entry(reps).Collection(x => x.Report_Collection).LoadAsync();
                }
            }
        }

        // LoadTables/fixup не всегда наполняет Rows у отчёта — без строк сравнение
        // содержимого при импорте даёт ложную «Полную копию».
        foreach (var reps in reports)
        {
            foreach (var key in reps.Report_Collection)
            {
                await EnsureImportReportRowsLoadedAsync(db, (Report)key);
            }
        }

        await InitializationAsyncCommand.ProcessDataBaseFillEmpty(db);
        return reports;
    }

    /// <summary>
    /// Явно подгружает строки формы и примечания отчёта из открытого контекста .raodb.
    /// </summary>
    private static async Task EnsureImportReportRowsLoadedAsync(DBModel db, Report report)
    {
        if (report.Rows.Count == 0)
        {
            switch (report.FormNum_DB)
            {
                case "1.0":
                    await db.Entry(report).Collection(x => x.Rows10).LoadAsync();
                    break;
                case "1.1":
                    await db.Entry(report).Collection(x => x.Rows11).LoadAsync();
                    break;
                case "1.2":
                    await db.Entry(report).Collection(x => x.Rows12).LoadAsync();
                    break;
                case "1.3":
                    await db.Entry(report).Collection(x => x.Rows13).LoadAsync();
                    break;
                case "1.4":
                    await db.Entry(report).Collection(x => x.Rows14).LoadAsync();
                    break;
                case "1.5":
                    await db.Entry(report).Collection(x => x.Rows15).LoadAsync();
                    break;
                case "1.6":
                    await db.Entry(report).Collection(x => x.Rows16).LoadAsync();
                    break;
                case "1.7":
                    await db.Entry(report).Collection(x => x.Rows17).LoadAsync();
                    break;
                case "1.8":
                    await db.Entry(report).Collection(x => x.Rows18).LoadAsync();
                    break;
                case "1.9":
                    await db.Entry(report).Collection(x => x.Rows19).LoadAsync();
                    break;
                case "2.0":
                    await db.Entry(report).Collection(x => x.Rows20).LoadAsync();
                    break;
                case "2.1":
                    await db.Entry(report).Collection(x => x.Rows21).LoadAsync();
                    break;
                case "2.2":
                    await db.Entry(report).Collection(x => x.Rows22).LoadAsync();
                    break;
                case "2.3":
                    await db.Entry(report).Collection(x => x.Rows23).LoadAsync();
                    break;
                case "2.4":
                    await db.Entry(report).Collection(x => x.Rows24).LoadAsync();
                    break;
                case "2.5":
                    await db.Entry(report).Collection(x => x.Rows25).LoadAsync();
                    break;
                case "2.6":
                    await db.Entry(report).Collection(x => x.Rows26).LoadAsync();
                    break;
                case "2.7":
                    await db.Entry(report).Collection(x => x.Rows27).LoadAsync();
                    break;
                case "2.8":
                    await db.Entry(report).Collection(x => x.Rows28).LoadAsync();
                    break;
                case "2.9":
                    await db.Entry(report).Collection(x => x.Rows29).LoadAsync();
                    break;
                case "2.10":
                    await db.Entry(report).Collection(x => x.Rows210).LoadAsync();
                    break;
                case "2.11":
                    await db.Entry(report).Collection(x => x.Rows211).LoadAsync();
                    break;
                case "2.12":
                    await db.Entry(report).Collection(x => x.Rows212).LoadAsync();
                    break;
                case "4.0":
                    await db.Entry(report).Collection(x => x.Rows40).LoadAsync();
                    break;
                case "4.1":
                    await db.Entry(report).Collection(x => x.Rows41).LoadAsync();
                    break;
                case "5.0":
                    await db.Entry(report).Collection(x => x.Rows50).LoadAsync();
                    break;
                case "5.1":
                    await db.Entry(report).Collection(x => x.Rows51).LoadAsync();
                    break;
                case "5.2":
                    await db.Entry(report).Collection(x => x.Rows52).LoadAsync();
                    break;
                case "5.3":
                    await db.Entry(report).Collection(x => x.Rows53).LoadAsync();
                    break;
                case "5.4":
                    await db.Entry(report).Collection(x => x.Rows54).LoadAsync();
                    break;
                case "5.5":
                    await db.Entry(report).Collection(x => x.Rows55).LoadAsync();
                    break;
                case "5.6":
                    await db.Entry(report).Collection(x => x.Rows56).LoadAsync();
                    break;
                case "5.7":
                    await db.Entry(report).Collection(x => x.Rows57).LoadAsync();
                    break;
            }
        }

        if (report.Notes.Count == 0)
            await db.Entry(report).Collection(x => x.Notes).LoadAsync();
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