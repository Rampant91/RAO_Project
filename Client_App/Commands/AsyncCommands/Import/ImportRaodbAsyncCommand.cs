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
using Client_App.ViewModels;

namespace Client_App.Commands.AsyncCommands.Import;

/// <summary>
/// Импорт -> Из RAODB
/// </summary>
public class ImportRaodbAsyncCommand : ImportBaseAsyncCommand
{
    #region AsyncExecute

    public override async Task AsyncExecute(object? parameter)
    {
        var answer = await InitializeImportProcess();
        if (answer is null) return;

        var countReadFiles = answer.Length;
        var impReportsList = new List<Reports>();

        //Для каждого импортируемого файла
        foreach (var path in answer)
        {
            if (path == "") continue;
            
            var reportsCollection = await ProcessImportFile(path);
            if (reportsCollection is null) 
            {
                countReadFiles--;
                continue;
            }

            if (!HasMultipleReport)
            {
                HasMultipleReport = reportsCollection.Sum(x => x.Report_Collection.Count) > 1 || answer.Length > 1;
            }

            await ProcessReportsCollection(reportsCollection, impReportsList);
        }

        await FinalizeImportProcess(answer, countReadFiles, impReportsList);
    } 

    #endregion

    #region Methods

    /// <summary>
    /// Инициализирует процесс импорта: сбрасывает флаги и получает список файлов для импорта
    /// </summary>
    /// <returns>Массив путей к выбранным файлам или null если отмена</returns>
    private async Task<string[]?> InitializeImportProcess()
    {
        RepsWhereTitleFormCheckIsCancel.Clear();
        IsFirstLogLine = true;
        CurrentLogLine = 1;
        string[] extensions = ["raodb", "RAODB"];
        var answer = await GetSelectedFilesFromDialog("RAODB", extensions);
        if (answer is null) return null;
        
        SkipNewOrg = false;
        SkipInter = false;
        SkipLess = false;
        SkipNew = false;
        SkipReplace = false;
        HasMultipleReport = false;
        AtLeastOneImportDone = false;
        
        return answer;
    }

    /// <summary>
    /// Обрабатывает один файл импорта: копирует во временную папку и пытается прочитать данные
    /// </summary>
    /// <param name="path">Путь к исходному файлу</param>
    /// <returns>Коллекция отчетов или null если файл поврежден</returns>
    private async Task<List<Reports>?> ProcessImportFile(string path)
    {
        var count = 0;
        string? tmpFile;
        do
        {
            tmpFile = Path.Combine(BaseVM.TmpDirectory, $"file_imp_{count++}.raodb");
        } while (File.Exists(tmpFile));

        TmpImpFilePath = tmpFile;
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
            await ShowFileCorruptedMessage(path);
            return null;
        }

        return reportsCollection;
    }

    /// <summary>
    /// Показывает сообщение о поврежденном файле
    /// </summary>
    /// <param name="path">Путь к поврежденному файлу</param>
    private static async Task ShowFileCorruptedMessage(string path)
    {
        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = "Импорт из .raodb",
                ContentHeader = "Ошибка",
                ContentMessage = $"Не удалось прочесть файл {path}," +
                                 $"{Environment.NewLine}файл поврежден или не содержит данных.",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));
    }

    /// <summary>
    /// Обрабатывает коллекцию отчетов из одного файла
    /// </summary>
    /// <param name="reportsCollection">Коллекция отчетов для обработки</param>
    /// <param name="impReportsList">Общий список импортированных отчетов</param>
    private async Task ProcessReportsCollection(List<Reports> reportsCollection, List<Reports> impReportsList)
    {
        foreach (var impReps in reportsCollection)
        {
            await ProcessSingleReport(impReps);
            impReportsList.Add(impReps);
        }

        // Сохранение для предотвращения дубликатов при импорте нескольких файлов
        try
        {
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Логирование ошибки при необходимости
        }
    }

    /// <summary>
    /// Обрабатывает один отчет (организацию) из коллекции
    /// </summary>
    /// <param name="impReps">Отчет для обработки</param>
    private async Task ProcessSingleReport(Reports impReps)
    {
        var dateTime = DateTime.Now;

        if (impReps.Master_DB.FormNum_DB is not ("1.0" or "2.0")) return;

        await impReps.SortAsync();
        await RestoreReportsOrders(impReps);
        UpdateReportDates(impReps, dateTime);

        var baseReps11 = await GetReports11FromDbEqualAsync(impReps);
        var baseReps21 = await GetReports21FromDbEqualAsync(impReps);

        FillEmptyRegNo(ref baseReps11);
        FillEmptyRegNo(ref baseReps21);
        impReps.CleanIds();
        ProcessIfNoteOrder0(impReps);

        SetImportReportInfo(impReps);
        UpdateAllReportChangeDate(impReps, dateTime);

        var impRepsReportList = impReps.Report_Collection.ToList();
        if (baseReps11 != null)
        {
            await ProcessIfHasReports11(baseReps11, impReps, impRepsReportList);
        }
        else if (baseReps21 != null)
        {
            await ProcessIfHasReports21(baseReps21, impReps, impRepsReportList);
        }
        else
        {
            await AddNewOrganization(impReps);
        }

        await SortReportRows(impReps);
    }

    /// <summary>
    /// Обновляет даты отчетов и регистрационные номера
    /// </summary>
    /// <param name="impReps">Отчет для обновления</param>
    /// <param name="dateTime">Текущая дата</param>
    private static void UpdateReportDates(Reports impReps, DateTime dateTime)
    {
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
    }

    /// <summary>
    /// Устанавливает информацию об импортируемом отчете
    /// </summary>
    /// <param name="impReps">Отчет для установки информации</param>
    private void SetImportReportInfo(Reports impReps)
    {
        ImpRepFormCount = impReps.Report_Collection.Count;
        ImpRepFormNum = impReps.Master.FormNum_DB;
        BaseRepsOkpo = impReps.Master.OkpoRep?.Value ?? "";
        BaseRepsRegNum = impReps.Master.RegNoRep?.Value ?? "";
        BaseRepsShortName = impReps.Master.ShortJurLicoRep.Value;
    }

    /// <summary>
    /// Обновляет дату изменения для всех отчетов в коллекции
    /// </summary>
    /// <param name="impReps">Коллекция отчетов</param>
    /// <param name="dateTime">Текущая дата</param>
    private static void UpdateAllReportChangeDate(Reports impReps, DateTime dateTime)
    {
        impReps.Report_Collection.ToList<Report>().ForEach(x => x.ReportChangedDate = dateTime);

        foreach (var key in impReps.Report_Collection)
        {
            var report = (Report)key;
            report.ReportChangedDate = dateTime;
        }
    }

    /// <summary>
    /// Сортирует строки отчета в зависимости от типа формы
    /// </summary>
    /// <param name="impReps">Отчет для сортировки</param>
    private static async Task SortReportRows(Reports impReps)
    {
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

    /// <summary>
    /// Добавляет новую организацию в базу данных
    /// </summary>
    /// <param name="impReps">Отчет новой организации</param>
    private async Task AddNewOrganization(Reports impReps)
    {
        var userChoice = await GetNewOrganizationUserChoice();
        
        if (userChoice is "Добавить" or "Да для всех")
        {
            var db = StaticConfiguration.DBModel;
            var dbObservable = db.DBObservableDbSet.Local.FirstOrDefault() 
                               ?? await db.DBObservableDbSet.FirstAsync();

            impReps.DBObservable = dbObservable;
            db.ReportsCollectionDbSet.Add(impReps);

            AtLeastOneImportDone = true;
            await LogImportedReports(impReps);
        }
    }

    /// <summary>
    /// Получает выбор пользователя о добавлении новой организации
    /// </summary>
    /// <returns>Выбор пользователя</returns>
    private async Task<string> GetNewOrganizationUserChoice()
    {
        var an = "Добавить";
        if (SkipNewOrg) return an;

        var isMultipleFiles = false; // Будет определено в контексте вызова
            
        an = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = isMultipleFiles
                    ? [
                        new ButtonDefinition { Name = "Добавить", IsDefault = true },
                        new ButtonDefinition { Name = "Да для всех" },
                        new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                    ]
                    : [
                        new ButtonDefinition { Name = "Добавить", IsDefault = true },
                        new ButtonDefinition { Name = "Отменить импорт", IsCancel = true }
                    ],
                ContentTitle = "Импорт из .raodb",
                ContentHeader = "Уведомление",
                ContentMessage = GetNewOrgMessage(isMultipleFiles),
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));

        if (an is "Да для всех") SkipNewOrg = true;

        return an;
    }

    /// <summary>
    /// Формирует сообщение о добавлении новой организации
    /// </summary>
    /// <param name="isMultipleFiles">Множественный импорт</param>
    /// <returns>Текст сообщения</returns>
    private string GetNewOrgMessage(bool isMultipleFiles)
    {
        var baseMessage = $"Будет добавлена новая организация ({ImpRepFormNum}) содержащая {ImpRepFormCount} форм отчетности."
                         + $"{Environment.NewLine}"
                         + $"{Environment.NewLine}Регистрационный номер - {BaseRepsRegNum}"
                         + $"{Environment.NewLine}ОКПО - {BaseRepsOkpo}"
                         + $"{Environment.NewLine}Сокращенное наименование - {BaseRepsShortName}";
        
        if (isMultipleFiles)
        {
            baseMessage += $"{Environment.NewLine}"
                          + $"{Environment.NewLine}Кнопка \"Да для всех\" позволяет без уведомлений "
                          + $"{Environment.NewLine}импортировать все новые организации.";
        }
        
        return baseMessage;
    }

    /// <summary>
    /// Логирует импортированные отчеты
    /// </summary>
    /// <param name="impReps">Импортированные отчеты</param>
    private async Task LogImportedReports(Reports impReps)
    {
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
                Act = Act,
                CorNum = ImpRepCorNum,
                CurrentLogLine = CurrentLogLine,
                EndPeriod = ImpRepEndPeriod,
                FormCount = ImpRepFormCount,
                FormNum = ImpRepFormNum,
                StartPeriod = ImpRepStartPeriod,
                Okpo = BaseRepsOkpo,
                OperationDate = OperationDate,
                RegNum = BaseRepsRegNum,
                ShortName = BaseRepsShortName,
                SourceFileFullPath = SourceFile!.FullName,
                Year = ImpRepYear
            };
            ServiceExtension.LoggerManager.Import(LoggerImportDTO);
            IsFirstLogLine = false;
            CurrentLogLine++;
        }
    }

    /// <summary>
    /// Завершает процесс импорта: сохраняет изменения, обновляет UI и показывает результат
    /// </summary>
    /// <param name="answer">Массив путей к файлам</param>
    /// <param name="countReadFiles">Количество успешно обработанных файлов</param>
    /// <param name="impReportsList">Список импортированных отчетов</param>
    private async Task FinalizeImportProcess(string[] answer, int countReadFiles, List<Reports> impReportsList)
    {
        try
        {
            await StaticConfiguration.DBModel.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Логирование ошибки при необходимости
        }

        await SortReportsCollectionAsync();
        await SetDataGridPage(impReportsList);

        var suffix = answer.Length.ToString().EndsWith('1') && !answer.Length.ToString().EndsWith("11") 
            ? "а"
            : "ов";

        await ShowImportResultMessage(answer.Length, countReadFiles, suffix, AtLeastOneImportDone);
    }

    /// <summary>
    /// Показывает сообщение о результате импорта
    /// </summary>
    /// <param name="totalFiles">Общее количество файлов</param>
    /// <param name="successFiles">Количество успешно импортированных файлов</param>
    /// <param name="suffix">Суффикс для склонения слова "файл"</param>
    /// <param name="hasSuccess">Был ли хотя бы один успешный импорт</param>
    private static async Task ShowImportResultMessage(int totalFiles, int successFiles, string suffix, bool hasSuccess)
    {
        var message = hasSuccess
            ? $"Импорт {successFiles} из {totalFiles} файл{suffix} .raodb успешно завершен."
            : $"Импорт из {totalFiles} файл{suffix} .raodb был отменен.";

        await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(new MessageBoxStandardParams
            {
                ButtonDefinitions = ButtonEnum.Ok,
                ContentTitle = "Импорт из .raodb",
                ContentHeader = "Уведомление",
                ContentMessage = message,
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow));
    }

    #endregion

    #region GetReportsFromDataBase

    private static async Task<List<Reports>> GetReportsFromDataBase(string file)
    {
        await using DBModel db = new(file);

        #region Test Version

        //var t = await db.Database.GetPendingMigrationsAsync();
        //var a = db.Database.GetMigrations();
        //var b = await db.Database.GetAppliedMigrationsAsync();

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