using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using FirebirdSql.Data.FirebirdClient;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Экспорт всех организаций организации в один файл .RAODB (отключён)
/// </summary>
public partial class ExportAllReportsOneFileAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        string answer;
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
        var newDbFolder = await new OpenFolderDialog().ShowAsync(Desktop.MainWindow);
        if (string.IsNullOrEmpty(newDbFolder)) return;
        var newDbPath = Path.Combine(newDbFolder, "Local_0.RAODB");

        await using var db = new DBModel(newDbPath);
        await db.Database.MigrateAsync();
        db.DBObservableDbSet.Add(ReportsStorage.LocalReports);
        await db.SaveChangesAsync();

        try
        {
            foreach (var reps in ReportsStorage.LocalReports.Reports_Collection)
            {
                var newReports = (Reports)reps;
                //List<Report> repWithFormsList = [];
                var masterReport = db.ReportCollectionDbSet.First(x => x.Id == newReports.Master_DBId);
                foreach (var key1 in newReports.Report_Collection)
                {
                    var rep = (Report)key1;
                    var repWithForms = await ReportsStorage.Api.GetAsync(rep.Id);
                    //repWithFormsList.Add(repWithForms);
                    newReports.Report_Collection.Replace(rep, repWithForms);
                }
                //newReports.Report_Collection.Clear();
                //newReports.Report_Collection.AddRangeNoChange(repWithFormsList);
               

                var existingReports = await db.ReportsCollectionDbSet
                    .Where(x => x.Id == newReports.Id)
                    .Include(reports => reports.Report_Collection)
                    .Include(reports => reports.Master_DB)
                    .SingleOrDefaultAsync();

                foreach (var rep in existingReports.Report_Collection)
                {
                    var report = (Report)rep;
                    db.Entry(report).State = EntityState.Deleted;
                }
                db.Entry(existingReports.Master_DB).State = EntityState.Deleted;
                db.Entry(existingReports).State = EntityState.Deleted;

                await db.SaveChangesAsync();

                newReports.Master_DB = masterReport;
                db.ReportsCollectionDbSet.Add(newReports);

                await db.SaveChangesAsync();
            }
            var t = db.Database.GetDbConnection() as FbConnection;
            await t.CloseAsync();
            await t.DisposeAsync();

            await db.Database.CloseConnectionAsync();
            await db.DisposeAsync();
        }
        catch (Exception ex)
        {

        }

        #region ExportDoneMessage

        answer = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Открыть расположение файла" }
                ],
                ContentTitle = "Выгрузка",
                ContentHeader = "Уведомление",
                ContentMessage = "Выгрузка всех организаций в отдельный" +
                                 $"{Environment.NewLine}файл .raodb завершена.",
                MinWidth = 400,
                MinHeight = 150,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            })
            .ShowDialog(Desktop.MainWindow);

        #endregion

        if (answer is "Открыть расположение файлов")
        {
            Process.Start("explorer", newDbFolder);
        }
    }
}