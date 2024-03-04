using System;
using System.Collections.Generic;
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
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

//  Экспорт организации в файл .raodb
internal class ExportReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        var folderPath = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;
        var dt = DateTime.Now;
        Reports exportOrg;
        string fileNameTmp;
        await using var db = new DBModel(StaticConfiguration.DBPath);
        switch (parameter)
        {
            case ObservableCollectionWithItemPropertyChanged<IKey> param:
                foreach (var item in param)
                {
                    ((Reports)item).Master.ExportDate.Value = dt.Date.ToShortDateString();
                }
                fileNameTmp = $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
                exportOrg = (Reports)param.First();
                await db.SaveChangesAsync();
                break;
            case Reports reps:
                fileNameTmp = $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}";
                exportOrg = reps;
                exportOrg.Master.ExportDate.Value = dt.Date.ToShortDateString();
                await db.SaveChangesAsync();
                break;
            default:
                return;
        }

        List<Report> repList = [];
        foreach (var key in exportOrg.Report_Collection)
        {
            var rep = (Report)key;
            repList.Add(await ReportsStorage.Api.GetAsync(rep.Id));
        }
        exportOrg.Report_Collection.Clear();
        exportOrg.Report_Collection.AddRangeNoChange(repList);

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

        await Task.Run(async () =>
        {
            await using var tempDb = new DBModel(fullPathTmp);
            try
            {
                await tempDb.Database.MigrateAsync();
                await tempDb.ReportsCollectionDbSet.AddAsync(exportOrg);
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
        });

        #region ExportDoneMessage

        var answer = await MessageBox.Avalonia.MessageBoxManager
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
            }).ShowDialog(Desktop.MainWindow);

        #endregion

        if (answer is "Открыть расположение файла")
        {
            Process.Start("explorer", folderPath);
        }
    }
}