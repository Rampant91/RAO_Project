﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.Views.ProgressBar;
using FirebirdSql.Data.FirebirdClient;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

//  Экспорт формы в файл .raodb
public class ExportFormAsyncCommand : ExportRaodbBaseAsyncCommand
{
    //public required AnyTaskProgressBar ProgressBar;

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> param) return;
        var cts = new CancellationTokenSource();

        var folderPath = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;
        foreach (var item in param)
        {
            var a = DateTime.Now.Date;
            var aDay = a.Day.ToString();
            var aMonth = a.Month.ToString();
            if (aDay.Length < 2) aDay = $"0{aDay}";
            if (aMonth.Length < 2) aMonth = $"0{aMonth}";
            ((Report)item).ExportDate.Value = $"{aDay}.{aMonth}.{a.Year}";
        }
        var repId = param.First().Id;

        #region ProgressBarInitialization

        await Dispatcher.UIThread.InvokeAsync(() => ProgressBar = new AnyTaskProgressBar(cts));
        var progressBar = ProgressBar;
        var progressBarVM = progressBar.AnyTaskProgressBarVM_DB;
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

        var rep = dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .First(x => x.Id == repId);

        #region Progress = 10

        progressBarVM.ExportName = $"Выгрузка отчёта {rep.Reports.Master_DB.RegNoRep.Value}_" +
                                   $"{rep.Reports.Master_DB.OkpoRep.Value}_" +
                                   $"{rep.FormNum_DB}_" +
                                   $"{rep.StartPeriod_DB}_" +
                                   $"{rep.EndPeriod_DB}";
        loadStatus = "Загрузка отчёта";
        progressBarVM.ValueBar = 10;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        //Нужно переделать с использованием фабрики, но так, чтобы работала асинхронность (почему-то запрос к ДБ через ней выполняется синхронно)
        #region GetReportFromDb
        
        var exportReport = await dbReadOnly.ReportCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Rows10.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows11.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows12.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows13.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows14.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows15.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows16.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows17.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows18.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows19.OrderBy(x => x.NumberInOrder_DB))
            .Include(x => x.Rows20.OrderBy(x => x.NumberInOrder_DB))
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
            .Include(x => x.Notes.OrderBy(x => x.Order))
            .FirstAsync(x => x.Id == repId, cancellationToken: cts.Token); 
        
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

        var reps = ReportsStorage.LocalReports.Reports_Collection
            .FirstOrDefault(t => t.Report_Collection
                .Any(x => x.Id == exportReport.Id));
        if (reps is null) return;

        var fullPathTmp = Path.Combine(BaseVM.TmpDirectory, $"{fileNameTmp}_exp.RAODB");

        Reports orgWithExpForm = new()
        {
            Master = reps.Master,
            Report_Collection = new ObservableCollectionWithItemPropertyChanged<Report>([exportReport])
        };

        var filename = reps.Master_DB.FormNum_DB switch
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
            _ => throw new ArgumentOutOfRangeException()
        };

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
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Выгрузка в Excel",
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

                return;
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
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Выгрузка в Excel",
                        ContentHeader = "Ошибка",
                        ContentMessage = "При копировании файла базы данных из временной папки возникла ошибка." +
                                         $"{Environment.NewLine}Экспорт не выполнен.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    }).ShowDialog(Desktop.MainWindow));

            #endregion
        }

        #region Progress = 100
        
        loadStatus = "Завершение выгрузки";
        progressBarVM.ValueBar = 100;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})"; 
        
        #endregion

        if (!cts.IsCancellationRequested)
        {
            #region ExportCompliteMessage

            var answer = await Dispatcher.UIThread.InvokeAsync(() =>
                MessageBox.Avalonia.MessageBoxManager
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

            if (answer is "Открыть расположение файла")
            {
                Process.Start("explorer", folderPath);
            }
        }

        await Dispatcher.UIThread.InvokeAsync(() => progressBar.Close());
    }

    #region InventoryCheck

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