using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using System;
using System.IO;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Models.Forms;
using System.Linq;

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
                var exportOrg = (Reports)reps;

                var oldReps = await db.ReportsCollectionDbSet.FindAsync(exportOrg.Id);
                if (oldReps != null)
                    db.ReportsCollectionDbSet.Remove(oldReps);
                await db.SaveChangesAsync();

                var newOrg = new Reports
                {
                    Id = exportOrg.Id, Master_DB = exportOrg.Master_DB, Master = exportOrg.Master,
                    DBObservable = exportOrg.DBObservable,
                };
                foreach (var rep in exportOrg.Report_Collection)
                {
                    var repWithoutForms = (Report)rep;
                    var repWithForms = await ReportsStorage.Api.GetAsync(repWithoutForms.Id);
                    newOrg.Report_Collection.Add(repWithForms);
                }
                db.ReportsCollectionDbSet.Add(newOrg);
                await db.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {

        }
    }
}