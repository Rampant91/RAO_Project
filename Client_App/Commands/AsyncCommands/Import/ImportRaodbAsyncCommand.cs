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
using Models.Forms;
using Client_App.Interfaces.Logger;
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
        atLeastOneImportDone = false;

        foreach (var res in answer) // Для каждого импортируемого файла
        {
            if (res == "") continue;
            var file = GetRaoFileName();
            SourceFile = new FileInfo(res);
            SourceFile.CopyTo(file, true);
            var reportsCollection = await GetReportsFromDataBase(file);

            if (!HasMultipleReport)
            {
                HasMultipleReport = reportsCollection.Sum(x => x.Report_Collection.Count) > 1 || answer.Length > 1;
            }

            foreach (var item in reportsCollection) // Для каждой импортируемой организации
            {
                await item.SortAsync();
                await RestoreReportsOrders(item);
                if (item.Master.Rows10.Count != 0)
                {
                    item.Master.Rows10[1].RegNo_DB = item.Master.Rows10[0].RegNo_DB;
                }

                if (item.Master.Rows20.Count != 0)
                {
                    item.Master.Rows20[1].RegNo_DB = item.Master.Rows20[0].RegNo_DB;
                }

                var first11 = GetReports11FromLocalEqual(item);
                var first21 = GetReports21FromLocalEqual(item);
                FillEmptyRegNo(ref first11);
                FillEmptyRegNo(ref first21);
                item.CleanIds();
                ProcessIfNoteOrder0(item);

                ImpRepFormCount = item.Report_Collection.Count;
                BaseRepsOkpo = item.Master.OkpoRep.Value;
                BaseRepsRegNum = item.Master.RegNoRep.Value;
                BaseRepsShortName = item.Master.ShortJurLicoRep.Value;

                if (first11 != null)
                {
                    await ProcessIfHasReports11(first11, item);
                }
                else if (first21 != null)
                {
                    await ProcessIfHasReports21(first21, item);
                }
                else if (first11 == null && first21 == null)
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
                                        $"Будет добавлена новая организация ({BaseRepFormNum}) содержащая {ImpRepFormCount} форм отчетности." +
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
                                        $"Будет добавлена новая организация ({BaseRepFormNum}) содержащая {ImpRepFormCount} форм отчетности." +
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
                        MainWindowVM.LocalReports.Reports_Collection.Add(item);
                        atLeastOneImportDone = true;

                        #region LoggerImport

                        var sortedRepList = item.Report_Collection
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

                switch (item.Master_DB.FormNum_DB)
                {
                    case "1.0":
                        await item.Master_DB.Rows10.QuickSortAsync();
                        break;
                    case "2.0":
                        await item.Master_DB.Rows20.QuickSortAsync();
                        break;
                }
            }
        }

        await MainWindowVM.LocalReports.Reports_Collection.QuickSortAsync();
        await StaticConfiguration.DBModel.SaveChangesAsync();

        var suffix = answer.Length.ToString().EndsWith('1') && !answer.Length.ToString().EndsWith("11")
                ? "а"
                : "ов";
        if (atLeastOneImportDone)
        {
            #region MessageImportDone
            
            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт из файл{suffix} .raodb успешно завершен.",
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
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Импорт из .raodb",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"Импорт из файл{suffix} .raodb был отменен.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow);

            #endregion
        }
    }

    #region FillEmptyRegNo

    private static void FillEmptyRegNo(ref Reports? reps)
    {
        if (reps is null) return;
        if (reps.Master.Rows10.Count >= 2)
        {
            if (reps.Master.Rows10[0].RegNo_DB is "" && reps.Master.Rows10[1].RegNo_DB is not "" && reps.Master.Rows10[0].Okpo_DB is not "")
            {
                reps.Master.Rows10[0].RegNo_DB = reps.Master.Rows10[1].RegNo_DB;
            }
            if (reps.Master.Rows10[1].RegNo_DB is "" && reps.Master.Rows10[0].RegNo_DB is not "" && reps.Master.Rows10[1].Okpo_DB is not "")
            {
                reps.Master.Rows10[1].RegNo_DB = reps.Master.Rows10[0].RegNo_DB;
            }
        }
        if (reps.Master.Rows20.Count >= 2)
        {
            if (reps.Master.Rows20[0].RegNo_DB is "" && reps.Master.Rows20[1].RegNo_DB is not "" && reps.Master.Rows20[0].Okpo_DB is not "")
            {
                reps.Master.Rows20[0].RegNo_DB = reps.Master.Rows20[1].RegNo_DB;
            }
            if (reps.Master.Rows20[1].RegNo_DB is "" && reps.Master.Rows20[0].RegNo_DB is not "" && reps.Master.Rows20[1].Okpo_DB is not "")
            {
                reps.Master.Rows20[1].RegNo_DB = reps.Master.Rows20[0].RegNo_DB;
            }
        }
    }

    #endregion

    #region GetRaoFileName

    private static string GetRaoFileName()
    {
        var count = 0;
        string? file;
        do
        {
            file = Path.Combine(BaseVM.TmpDirectory, $"file_imp_{count++}.raodb");
        } while (File.Exists(file));

        return file;
    }

    #endregion

    #region GetReports11FromLocalEqual

    private static Reports? GetReports11FromLocalEqual(Reports item)
    {
        try
        {
            if (!item.Report_Collection.Any(x => x.FormNum_DB[0].Equals('1')) || item.Master_DB.FormNum_DB is not "1.0")
            {
                return null;
            }

            return MainWindowVM.LocalReports.Reports_Collection10
                       .FirstOrDefault(t =>

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[0].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[1].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[1].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[0].RegNo_DB
                           && item.Master.Rows10[1].Okpo_DB != ""
                           && t.Master.Rows10[1].RegNo_DB == "")

                   ?? MainWindowVM.LocalReports
                       .Reports_Collection10 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
                       .FirstOrDefault(t =>

                           // юр лицо в базе совпадает с обособленным в импорте
                           item.Master.Rows10[1].Okpo_DB != ""
                           && t.Master.Rows10[1].Okpo_DB == ""
                           && item.Master.Rows10[1].Okpo_DB == t.Master.Rows10[0].Okpo_DB
                           && item.Master.Rows10[1].RegNo_DB == t.Master.Rows10[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || item.Master.Rows10[1].Okpo_DB == ""
                           && t.Master.Rows10[1].Okpo_DB != ""
                           && item.Master.Rows10[0].Okpo_DB == t.Master.Rows10[1].Okpo_DB
                           && item.Master.Rows10[0].RegNo_DB == t.Master.Rows10[1].RegNo_DB);
        }
        catch
        {
            return null;
        }
    }

    #endregion

    #region GetReports21FromLocalEqual

    private static Reports? GetReports21FromLocalEqual(Reports item)
    {
        try
        {
            if (!item.Report_Collection.Any(x => x.FormNum_DB[0].Equals('2')) || item.Master_DB.FormNum_DB is not "2.0")
            {
                return null;
            }

            return MainWindowVM.LocalReports.Reports_Collection20
                       .FirstOrDefault(t =>

                           // обособленные пусты и в базе и в импорте, то сверяем головное
                           item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[0].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB == ""

                           // обособленные пусты и в базе и в импорте, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[1].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB == ""

                           // обособленные не пусты, их и сверяем
                           || item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[1].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB != ""

                           // обособленные не пусты, но в базе пуст рег№ юр лица, берем рег№ обособленного
                           || item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[0].RegNo_DB
                           && item.Master.Rows20[1].Okpo_DB != ""
                           && t.Master.Rows20[1].RegNo_DB == "")

                   ?? MainWindowVM.LocalReports.Reports_Collection20 // если null, то ищем сбитый окпо (совпадение юр лица с обособленным)
                       .FirstOrDefault(t =>

                           // юр лицо в базе совпадает с обособленным в импорте
                           item.Master.Rows20[1].Okpo_DB != ""
                           && t.Master.Rows20[1].Okpo_DB == ""
                           && item.Master.Rows20[1].Okpo_DB == t.Master.Rows20[0].Okpo_DB
                           && item.Master.Rows20[1].RegNo_DB == t.Master.Rows20[0].RegNo_DB

                           // юр лицо в импорте совпадает с обособленным в базе
                           || item.Master.Rows20[1].Okpo_DB == ""
                           && t.Master.Rows20[1].Okpo_DB != ""
                           && item.Master.Rows20[0].Okpo_DB == t.Master.Rows20[1].Okpo_DB
                           && item.Master.Rows20[0].RegNo_DB == t.Master.Rows20[1].RegNo_DB);
        }
        catch
        {
            return null;
        }
    }

    #endregion

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

    #region ProcessIfNoteOrder0

    private static void ProcessIfNoteOrder0(Reports item)
    {
        foreach (var key in item.Report_Collection)
        {
            var form = (Report)key;
            foreach (var key1 in form.Notes)
            {
                var note = (Note)key1;
                if (note.Order == 0)
                {
                    note.Order = MainWindowVM.GetNumberInOrder(form.Notes);
                }
            }
        }
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