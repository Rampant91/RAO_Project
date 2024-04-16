using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
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
internal class ExportFormAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> param) return;
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

        var dt = DateTime.Now;
        var fileNameTmp = $"Report_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
        var exportForm = (Report)param.First();
        exportForm = await ReportsStorage.Api.GetAsync(exportForm.Id);

        var dtDay = dt.Day.ToString();
        var dtMonth = dt.Month.ToString();
        if (dtDay.Length < 2) dtDay = $"0{dtDay}";
        if (dtMonth.Length < 2) dtMonth = $"0{dtMonth}";
        exportForm.ExportDate.Value = $"{dtDay}.{dtMonth}.{dt.Year}";

        await using var db = new DBModel(StaticConfiguration.DBPath);
        await db.SaveChangesAsync();

        var reps = ReportsStorage.LocalReports.Reports_Collection
            .FirstOrDefault(t => t.Report_Collection
                .Any(x => x.Id == exportForm.Id));
        if (reps is null) return;

        var fullPathTmp = Path.Combine(BaseVM.TmpDirectory, $"{fileNameTmp}_exp.RAODB");

        Reports orgWithExpForm = new()
        {
            Master = reps.Master
        };
        orgWithExpForm.Report_Collection.Add(exportForm);

        var filename = reps.Master_DB.FormNum_DB switch
        {
            "1.0" =>
                StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.RegNoRep.Value) +
                $"_{StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.OkpoRep.Value)}" +
                $"_{exportForm.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(exportForm.StartPeriod_DB)}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(exportForm.EndPeriod_DB)}" +
                $"_{exportForm.CorrectionNumber_DB}" +
                $"_{Assembly.GetExecutingAssembly().GetName().Version}",

            "2.0" when orgWithExpForm.Master.Rows20.Count > 0 =>
                StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.RegNoRep.Value) +
                $"_{StaticStringMethods.RemoveForbiddenChars(orgWithExpForm.Master.OkpoRep.Value)}" +
                $"_{exportForm.FormNum_DB}" +
                $"_{StaticStringMethods.RemoveForbiddenChars(exportForm.Year_DB)}" +
                $"_{exportForm.CorrectionNumber_DB}" +
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

        await Task.Run(async () =>
        {
            await using var tempDb = new DBModel(fullPathTmp);
            try
            {
                await tempDb.Database.MigrateAsync();
                await tempDb.ReportsCollectionDbSet.AddAsync(orgWithExpForm);
                if (!tempDb.DBObservableDbSet.Any())
                {
                    tempDb.DBObservableDbSet.Add(new DBObservable());
                    tempDb.DBObservableDbSet.Local.First().Reports_Collection.AddRange(tempDb.ReportsCollectionDbSet.Local);
                }
                await tempDb.SaveChangesAsync();

                var t = tempDb.Database.GetDbConnection() as FbConnection;
                await t.CloseAsync();
                await t.DisposeAsync();

                await tempDb.Database.CloseConnectionAsync();
                await tempDb.DisposeAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }

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
        });

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
                        $"{Environment.NewLine}Номер формы - {exportForm.FormNum_DB}" +
                        $"{Environment.NewLine}Начало отчетного периода - {exportForm.StartPeriod_DB}" +
                        $"{Environment.NewLine}Конец отчетного периода - {exportForm.EndPeriod_DB}" +
                        $"{Environment.NewLine}Дата выгрузки - {exportForm.ExportDate_DB}" +
                        $"{Environment.NewLine}Номер корректировки - {exportForm.CorrectionNumber_DB}" +
                        $"{Environment.NewLine}Количество строк - {exportForm.Rows.Count}{InventoryCheck(exportForm)}",
                    MinWidth = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is "Открыть расположение файла")
        {
            Process.Start("explorer", folderPath);
        }
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
