using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.DBRealization;
using System;
using System.IO;
using System.Threading.Tasks;
using Client_App.ViewModels;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using System.Collections.Generic;
using System.Linq;
using Client_App.Interfaces.Logger;
using MessageBox.Avalonia.Enums;
using Models.DTO;
using static Client_App.Resources.StaticStringMethods;

namespace Client_App.Commands.AsyncCommands.Import;

//  Импорт -> Из RAODB
internal class ImportRaodbAsyncCommand : ImportBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        string[] extensions = { "raodb", "RAODB" };
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
        foreach (var path in answer) // Для каждого импортируемого файла
        {
            if (path == "") continue;
            TmpImpFilePath = GetRaoFileName();
            SourceFile = new FileInfo(path);
            SourceFile.CopyTo(TmpImpFilePath, true);
            var reportsCollection = await GetReportsFromDataBase(TmpImpFilePath);
            if (reportsCollection.Count == 0)
            {
                #region MessageFailedToReadFile

                await MessageBox.Avalonia.MessageBoxManager
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
                            .ShowDialog(Desktop.MainWindow); 

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
                await impReps.SortAsync();
                await RestoreReportsOrders(impReps);
                if (impReps.Master.Rows10.Count != 0)
                {
                    impReps.Master.Rows10[1].RegNo_DB = impReps.Master.Rows10[0].RegNo_DB;
                }

                if (impReps.Master.Rows20.Count != 0)
                {
                    impReps.Master.Rows20[1].RegNo_DB = impReps.Master.Rows20[0].RegNo_DB;
                }

                var baseReps11 = GetReports11FromLocalEqual(impReps);
                var baseReps21 = GetReports21FromLocalEqual(impReps);
                FillEmptyRegNo(ref baseReps11);
                FillEmptyRegNo(ref baseReps21);
                impReps.CleanIds();
                ProcessIfNoteOrder0(impReps);

                ImpRepFormCount = impReps.Report_Collection.Count;
                ImpRepFormNum = impReps.Master.FormNum_DB;
                BaseRepsOkpo = impReps.Master.OkpoRep.Value;
                BaseRepsRegNum = impReps.Master.RegNoRep.Value;
                BaseRepsShortName = impReps.Master.ShortJurLicoRep.Value;

                if (baseReps11 != null)
                {
                    var baseReps11Id = baseReps11.Id;
                    baseReps11 = StaticConfiguration.DBModel.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(reps => reps.Id == baseReps11Id)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows10)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows12)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows13)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows14)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows15)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows16)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows17)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows18)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows19)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .First();
                    await ProcessIfHasReports11(baseReps11, impReps);
                }
                else if (baseReps21 != null)
                {
                    var baseReps21Id = baseReps21.Id;
                    baseReps21 = StaticConfiguration.DBModel.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Where(reps => reps.Id == baseReps21Id)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows20)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows21)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows22)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows23)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows24)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows25)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows26)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows27)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows28)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows29)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows210)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows211)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Rows212)
                        .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                        .First();
                    await ProcessIfHasReports21(baseReps21, impReps);
                }
                else if (baseReps11 == null && baseReps21 == null)
                {
                    #region AddNewOrg

                    var an = "Добавить";
                    if (!SkipNewOrg)
                    {
                        if (answer.Length > 1 || reportsCollection.Count > 1)
                        {
                            #region MessageNewOrg

                            an = await MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions = new[]
                                    {
                                        new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                        new ButtonDefinition { Name = "Да для всех" },
                                        new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                    },
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
                                .ShowDialog(Desktop.MainWindow);

                            #endregion

                            if (an is "Да для всех") SkipNewOrg = true;
                        }
                        else
                        {
                            #region MessageNewOrg

                            an = await MessageBox.Avalonia.MessageBoxManager
                                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                                {
                                    ButtonDefinitions = new[]
                                    {
                                        new ButtonDefinition { Name = "Добавить", IsDefault = true },
                                        new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                                    },
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
                                .ShowDialog(Desktop.MainWindow);

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
                                            .ThenBy(x => StringReverse(x.StartPeriod_DB))
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
                }
            }
        }

        await ReportsStorage.LocalReports.Reports_Collection.QuickSortAsync();
        await StaticConfiguration.DBModel.SaveChangesAsync();

        var suffix = answer.Length.ToString().EndsWith('1') && !answer.Length.ToString().EndsWith("11")
                ? "а"
                : "ов";
        if (AtLeastOneImportDone)
        {
            #region MessageImportDone
            
            await MessageBox.Avalonia.MessageBoxManager
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
                .ShowDialog(Desktop.MainWindow);

            #endregion
        }
        else
        {
            #region MessageImportCancel

            await MessageBox.Avalonia.MessageBoxManager
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
                .ShowDialog(Desktop.MainWindow);

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
        await MainWindowVM.ProcessDataBaseFillEmpty(db);

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