using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Resources;
using Client_App.ViewModels;
using FirebirdSql.Data.FirebirdClient;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

//  Экспорт всех организаций организации в отдельные файлы .raodb
internal class ExportAllReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        string? answer;
        if (MainWindowVM.LocalReports.Reports_Collection.Count > 10)
        {
            #region ExportDoneMessage

            answer = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions = new[]
                    {
                        new ButtonDefinition { Name = "Ок", IsDefault = true },
                        new ButtonDefinition { Name = "Отменить выгрузку", IsCancel = true }
                    },
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Текущая база содержит {MainWindowVM.LocalReports.Reports_Collection.Count} форм организаций," +
                        $"{Environment.NewLine}выгрузка займет примерно {MainWindowVM.LocalReports.Reports_Collection.Count / 20} минут",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow);

            #endregion

            if (answer is "Отменить выгрузку") return;
        }
        var folderPath = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;

        var po = new ParallelOptions();
        Parallel.ForEach(MainWindowVM.LocalReports.Reports_Collection, async exportOrg =>
        {
            await Task.Run(async () =>
            {
                var dt = DateTime.Now;
                var fileNameTmp = $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}_{dt.Millisecond}";
                await StaticConfiguration.DBModel.SaveChangesAsync();
                var fullPathTmp = Path.Combine(BaseVM.TmpDirectory, $"{fileNameTmp}.RAODB");
                var filename = $"{StaticStringMethods.RemoveForbiddenChars(exportOrg.Master.RegNoRep.Value)}" +
                               $"_{StaticStringMethods.RemoveForbiddenChars(exportOrg.Master.OkpoRep.Value)}" +
                               $"_{exportOrg.Master.FormNum_DB}" +
                               $"_{BaseVM.Version}";

                var fullPath = Path.Combine(folderPath, $"{filename}.RAODB");
                DBModel db = new(fullPathTmp);
                try
                {
                    await db.Database.MigrateAsync();
                    await db.ReportsCollectionDbSet.AddAsync(exportOrg);
                    await db.SaveChangesAsync();

                    var t = db.Database.GetDbConnection() as FbConnection;
                    await t.CloseAsync();
                    await t.DisposeAsync();

                    await db.Database.CloseConnectionAsync();
                    await db.DisposeAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return;
                }

                try
                {
                    while (File.Exists(fullPath)) // insert index if file already exist
                    {
                        var matches = Regex.Matches(fullPath, @"(.+)#(\d+)(?=\.RAODB)");
                        if (matches.Count > 0)
                        {
                            foreach (Match match in matches)
                            {
                                if (!int.TryParse(match.Groups[2].Value, out var index)) return;
                                fullPath = match.Groups[1].Value + $"#{index + 1}.RAODB";
                            }
                        }
                        else
                        {
                            fullPath = fullPath.TrimEnd(".RAODB".ToCharArray()) + "#1.RAODB";
                        }
                    }
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
                                                 $"{Environment.NewLine}Экспорт не выполнен." +
                                                 $"{Environment.NewLine}{e.Message}",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen
                            })
                            .ShowDialog(Desktop.MainWindow));

                    #endregion
                }
            });
        });

        #region ExportDoneMessage

        answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Открыть расположение файлов" }
                },
                ContentTitle = "Выгрузка",
                ContentHeader = "Уведомление",
                ContentMessage = "Выгрузка всех организаций в отдельные" +
                                 $"{Environment.NewLine}файлы .raodb завершена.",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            })
            .ShowDialog(Desktop.MainWindow);

        #endregion

        if (answer is "Открыть расположение файлов")
        {
            Process.Start("explorer", folderPath);
        }
    }
}