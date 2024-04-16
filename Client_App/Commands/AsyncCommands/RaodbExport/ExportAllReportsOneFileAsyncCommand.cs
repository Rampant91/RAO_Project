using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using System.Threading;

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

        foreach (var reps in ReportsStorage.LocalReports.Reports_Collection)
        {
            var exportOrg = (Reports)reps;
            var oldReps = await db.ReportsCollectionDbSet.FindAsync(exportOrg.Id);
            if (oldReps != null) db.ReportsCollectionDbSet.Remove(oldReps);
            await db.SaveChangesAsync();
            foreach (var rep in exportOrg.Report_Collection)
            {
                var exportRep = (Report)rep;
                var report = await ReportsStorage.Api.GetAsync(exportRep.Id);
                exportOrg.Report_Collection.Replace(exportRep, report);
            }
            db.ReportsCollectionDbSet.Add(exportOrg);
            await db.SaveChangesAsync();
        }
    }
}