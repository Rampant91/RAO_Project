using System;
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
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Экспорт организации в файл .RAODB
/// </summary>
public class ExportReportsAsyncCommand : ExportRaodbBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var cts = new CancellationTokenSource();
        var folderPath = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;
        var dt = DateTime.Now;
        Reports exportOrg;
        string fileNameTmp;

        switch (parameter)
        {
            case ObservableCollectionWithItemPropertyChanged<IKey> param:
            {
                var reps = (Reports)param.First();
                var aDay = dt.Day.ToString();
                var aMonth = dt.Month.ToString();
                if (aDay.Length < 2) aDay = $"0{aDay}";
                if (aMonth.Length < 2) aMonth = $"0{aMonth}";
                foreach (var key in reps.Report_Collection)
                {
                    var rep = (Report)key;
                    rep.ExportDate.Value = $"{aDay}.{aMonth}.{dt.Year}";
                }
                fileNameTmp = $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
                exportOrg = (Reports)param.First();
                await StaticConfiguration.DBModel.SaveChangesAsync(cts.Token);
                break;
            }
            case Reports reps:
            {
                fileNameTmp = $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
                exportOrg = reps;
                exportOrg.Master.ExportDate.Value = dt.Date.ToShortDateString();
                await StaticConfiguration.DBModel.SaveChangesAsync(cts.Token);
                break;
            }
            default: return;
        }
        var repsId = exportOrg.Id;

        #region ProgressBarInitialization

        await Dispatcher.UIThread.InvokeAsync(() => ProgressBar = new AnyTaskProgressBar(cts));
        var progressBar = ProgressBar;
        var progressBarVM = progressBar.AnyTaskProgressBarVM;
        progressBarVM.ExportType = "Экспорт_RAODB";
        progressBarVM.ExportName = $"Выгрузка организации";
        progressBarVM.ValueBar = 5;
        var loadStatus = "Создание временной БД";
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        var dbReadOnlyPath = CreateTempDataBase();

        await using var dbReadOnly = new DBModel(dbReadOnlyPath);

        #region Progress = 10

        progressBarVM.ExportName = $"Выгрузка организации {exportOrg.Master_DB.RegNoRep.Value}_{exportOrg.Master_DB.OkpoRep.Value}";
        loadStatus = "Загрузка данных организации";
        progressBarVM.ValueBar = 10;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        #region GetReportsWithoutRows

        var repsFull = await dbReadOnly.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
            .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
            .Include(reports => reports.Report_Collection)
            .FirstAsync(x => x.Id == repsId, cancellationToken: cts.Token); 
        
        #endregion

        var repsReportIds = parameter switch
        {
            Reports => exportOrg.Report_Collection.Select(x => x.Id).ToArray(),
            _ => await dbReadOnly.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.Report_Collection)
                .Where(x => x.Id == repsId)
                .SelectMany(x => x.Report_Collection
                    .OrderBy(x => x.FormNum_DB)
                    .ThenBy(x => x.StartPeriod_DB)
                    .Select(x => x.Id))
                .ToArrayAsync(cancellationToken: cts.Token)
        };
        
        #region Progress = 15
        
        loadStatus = "Загрузка отчётов";
        progressBarVM.ValueBar = 15;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})"; 
        
        #endregion

        double progressBarDoubleValue = progressBarVM.ValueBar;
        repsFull.Report_Collection.Clear();
        foreach (var repId in repsReportIds)
        {
            var tmpRep = await dbReadOnly.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .FirstAsync(x => x.Id == repId, cancellationToken: cts.Token);

            loadStatus = $"Загрузка отчёта {tmpRep.FormNum_DB}_{tmpRep.StartPeriod_DB}_{tmpRep.EndPeriod_DB}";
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

            //TODO
            //Нужно переделать с использованием фабрики, но так, чтобы работала асинхронность (почему-то запрос к ДБ в ней выполняется синхронно)
            #region GetReportWithRows

            var rep = await dbReadOnly.ReportCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
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
                .Include(x => x.Notes.OrderBy(x => x.Order))
                .FirstAsync(x => x.Id == repId, cancellationToken: cts.Token);

            #endregion

            rep.ExportDate.Value = DateTime.Now.ToShortDateString();
            repsFull.Report_Collection.Add(rep);

            progressBarDoubleValue += (double)35 / repsReportIds.Length;
            progressBarVM.ValueBar = (int)Math.Floor(progressBarDoubleValue);
            progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";
        }

        var fullPathTmp = Path.Combine(BaseVM.TmpDirectory, $"{fileNameTmp}_exp.RAODB");
        var filename = $"{StaticStringMethods.RemoveForbiddenChars(exportOrg.Master.RegNoRep.Value)}" +
                       $"_{StaticStringMethods.RemoveForbiddenChars(exportOrg.Master.OkpoRep.Value)}" +
                       $"_{exportOrg.Master.FormNum_DB[0]}.x" +
                       $"_{Assembly.GetExecutingAssembly().GetName().Version}";

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
                            ContentTitle = "Выгрузка",
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

        #region Progress = 50

        loadStatus = "Создание базы данных";
        progressBarVM.ValueBar = 50;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        await tempDb.Database.MigrateAsync(cancellationToken: cts.Token);

        #region Progress = 60

        loadStatus = "Добавление коллекций организации";
        progressBarVM.ValueBar = 60;
        progressBarVM.LoadStatus = $"{progressBarVM.ValueBar}% ({loadStatus})";

        #endregion

        await tempDb.ReportsCollectionDbSet.AddAsync(repsFull, cts.Token);
        if (!tempDb.DBObservableDbSet.Any())
        {
            tempDb.DBObservableDbSet.Add(new DBObservable());
            tempDb.DBObservableDbSet.Local.First().Reports_Collection.AddRange(tempDb.ReportsCollectionDbSet.Local);
        }

        #region Progress = 70

        loadStatus = "Сохранение данных";
        progressBarVM.ValueBar = 70;
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
            File.Delete(dbReadOnlyPath);
        }
        catch (Exception ex)
        {
            #region FailedCopyFromTempMessage

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка",
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
            #region ExportDoneMessage

            var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Открыть расположение файла" }
                    ],
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Экспорт завершен. Файл экспорта организации ({exportOrg.Master.FormNum_DB[0]}.x) сохранен по пути:" +
                        $"{Environment.NewLine}{fullPath}" +
                        $"{Environment.NewLine}" +
                        $"{Environment.NewLine}Регистрационный номер - {exportOrg.Master.RegNoRep.Value}" +
                        $"{Environment.NewLine}ОКПО - {exportOrg.Master.OkpoRep.Value}" +
                        $"{Environment.NewLine}Сокращенное наименование - {exportOrg.Master.ShortJurLicoRep.Value}",
                    MinWidth = 400,
                    MinHeight = 150,
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
}