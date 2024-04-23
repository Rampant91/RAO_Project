using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FirebirdSql.Data.FirebirdClient;
using SkiaSharp;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

//  Экспорт всех организаций организации в один файл .raodb
public partial class ExportAllReportsOneFileAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (ReportsStorage.LocalReports.Reports_Collection.Count > 10)
        {
            #region ExportDoneMessage

            var answer = await MessageBox.Avalonia.MessageBoxManager
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
                List<Report> repWithFormsList = [];
                foreach (var key1 in newReports.Report_Collection)
                {
                    var rep = (Report)key1;
                    var repWithForms = await ReportsStorage.Api.GetAsync(rep.Id);
                    repWithFormsList.Add(repWithForms);
                }
                newReports.Report_Collection.Clear();
                newReports.Report_Collection.AddRangeNoChange(repWithFormsList);
               

                var existingReports = await db.ReportsCollectionDbSet
                    .Where(x => x.Id == newReports.Id)
                    .Include(x => x.Report_Collection)
                    .Include(reports => reports.Master_DB)
                    .SingleOrDefaultAsync();
                newReports.Master_DB = existingReports.Master_DB;

                foreach (var rep in existingReports.Report_Collection)
                {
                    var report = (Report)rep;
                    db.Entry(report).State = EntityState.Deleted;
                }
                db.Entry(existingReports.Master_DB).State = EntityState.Deleted;
                db.Entry(existingReports).State = EntityState.Deleted;

                await db.SaveChangesAsync();

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
    }
}