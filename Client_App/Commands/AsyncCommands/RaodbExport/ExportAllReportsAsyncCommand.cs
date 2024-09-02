using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Models.Collections;
using Models.DBRealization;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

//  Экспорт всех организаций организации в отдельные файлы .raodb
internal partial class ExportAllReportsAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        string? answer;
        if (ReportsStorage.LocalReports.Reports_Collection.Count > 10)
        {
            #region ExportDoneMessage

            answer = await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                {
                    ButtonDefinitions =
                    [
                        new ButtonDefinition { Name = "Да", IsDefault = true },
                        new ButtonDefinition { Name = "Отменить выгрузку", IsCancel = true }
                    ],
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage =
                        $"Текущая база содержит {ReportsStorage.LocalReports.Reports_Collection.Count} форм организаций," +
                        $"{Environment.NewLine}выгрузка может занять длительный период времени. Продолжить операцию?",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                }).ShowDialog(Desktop.MainWindow);

            #endregion

            if (answer is "Отменить выгрузку") return;
        }
        var folderPath = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (string.IsNullOrEmpty(folderPath)) return;

        Parallel.ForEach(ReportsStorage.LocalReports.Reports_Collection, async exportOrg =>
        {
            List<Report> repInRangeWithForms = [];
            foreach (var key1 in exportOrg.Report_Collection)
            {
                var rep = (Report)key1;
                repInRangeWithForms.Add(await ReportsStorage.Api.GetAsync(rep.Id));
            }

            exportOrg.Report_Collection.Clear();
            exportOrg.Report_Collection.AddRangeNoChange(repInRangeWithForms);

            await Task.Run(async () =>
            {
                var dt = DateTime.Now;
                var fileNameTmp =
                    $"Reports_{dt.Year}_{dt.Month}_{dt.Day}_{dt.Hour}_{dt.Minute}_{dt.Second}_{dt.Millisecond}";
                //await StaticConfiguration.DBModel.SaveChangesAsync();
                var fullPathTmp = Path.Combine(BaseVM.TmpDirectory, $"{fileNameTmp}.RAODB");
                var filename = $"{StaticStringMethods.RemoveForbiddenChars(exportOrg.Master.RegNoRep.Value)}" +
                               $"_{StaticStringMethods.RemoveForbiddenChars(exportOrg.Master.OkpoRep.Value)}" +
                               $"_{exportOrg.Master.FormNum_DB[0]}.x" +
                               $"_{Assembly.GetExecutingAssembly().GetName().Version}";

                var fullPath = Path.Combine(folderPath, $"{filename}.RAODB");
                var db = new DBModel(fullPathTmp);
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
                        var matches = RaodbFileNameRegex().Matches(fullPath);
                        if (matches.Count > 0)
                        {
                            foreach (var match in matches.Cast<Match>())
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
                                ContentMessage =
                                    "При копировании файла базы данных из временной папки возникла ошибка." +
                                    $"{Environment.NewLine}Экспорт не выполнен." +
                                    $"{Environment.NewLine}{e.Message}",
                                MinWidth = 400,
                                MinHeight = 150,
                                WindowStartupLocation = WindowStartupLocation.CenterScreen
                            })
                            .ShowDialog(Desktop.MainWindow));

                    #endregion

                    return;
                }

                #region ExportDoneMessage

                answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Ок", IsDefault = true },
                            new ButtonDefinition { Name = "Открыть расположение файлов" }
                        ],
                        ContentTitle = "Выгрузка",
                        ContentHeader = "Уведомление",
                        ContentMessage = "Выгрузка всех организаций в отдельные" +
                                         $"{Environment.NewLine}файлы .raodb завершена.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                if (answer is "Открыть расположение файлов")
                {
                    Process.Start("explorer", folderPath);
                }
            }).ConfigureAwait(false);
        });
    }

    [GeneratedRegex(@"(.+)#(\d+)(?=\.RAODB)")]
    private static partial Regex RaodbFileNameRegex();
}