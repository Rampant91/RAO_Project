using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Interfaces.Logger.EnumLogger;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Delete;

/// <summary>
/// Удалить выбранную организацию.
/// </summary>
public class DeleteReportsAsyncCommand : BaseAsyncCommand
{
    private readonly MainWindowVM _mainWindowVM;

    public DeleteReportsAsyncCommand(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;

        mainWindowVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(MainWindowVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _mainWindowVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        #region MessageDeleteReports

        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                ],
                ContentTitle = "Уведомление",
                ContentHeader = "Уведомление",
                ContentMessage = "Вы действительно хотите удалить организацию?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            })
            .ShowDialog(Desktop.MainWindow));

        #endregion

        if (answer is not "Да") return;

        try
        {
            Reports reps;
            if (parameter is IEnumerable enumerable)
                reps = enumerable!.Cast<Reports>().First();
            else if (parameter is Reports reports)
                reps = reports;
            else return;

            var masterRep = reps.Master_DB;

            var db = StaticConfiguration.DBModel;

            var reportIds = await OrgReportsQuery.GetReportIdsAsync(db, reps.Id);
            foreach (var batch in FirebirdInClause.Chunk(reportIds))
            {
                var toRemove = db.ReportCollectionDbSet
                    .Where(r => batch.Contains(r.Id))
                    .ToList();
                db.ReportCollectionDbSet.RemoveRange(toRemove);
            }

            db.ReportCollectionDbSet.Remove(masterRep);
            
            db.ReportsCollectionDbSet.Remove(reps);
            await db.SaveChangesAsync();

            Forms1WarmCache.Instance.InvalidateOrg(reps.Id);
            Forms1WarmCache.Instance.InvalidateOrgPages();

            await ProcessDataBaseFillEmpty(db);

            var mainWindow = (Desktop.MainWindow as MainWindow)!;
            var mainWindowVM = (mainWindow.DataContext as MainWindowVM)!;
            mainWindowVM.UpdateReportsCollection();
            mainWindowVM.UpdateOrgsPageInfo();
            mainWindowVM.UpdateTotalReportCount();
            mainWindowVM.UpdateTotalReportsCount();
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg, ErrorCodeLogger.DataBase);
        }

        //await Local_Reports.Reports_Collection.QuickSortAsync();
    
    }

    private static async Task ProcessDataBaseFillEmpty(DataContext dbm)
    {
        if (!dbm.DBObservableDbSet.Any()) dbm.DBObservableDbSet.Add(new DBObservable());
        foreach (var item in dbm.DBObservableDbSet)
        {
            foreach (var key in item.Reports_Collection)
            {
                var it = (Reports)key;
                if (it.Master_DB is null) continue;
                if (it.Master_DB.FormNum_DB == "") continue;
                if (it.Master_DB.Rows10.Count == 0)
                {
                    var ty1 = (Form10)FormCreator.Create("1.0");
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form10)FormCreator.Create("1.0");
                    ty2.NumberInOrder_DB = 2;
                    it.Master_DB.Rows10.Add(ty1);
                    it.Master_DB.Rows10.Add(ty2);
                }

                if (it.Master_DB.Rows20.Count == 0)
                {
                    var ty1 = (Form20)FormCreator.Create("2.0");
                    ty1.NumberInOrder_DB = 1;
                    var ty2 = (Form20)FormCreator.Create("2.0");
                    ty2.NumberInOrder_DB = 2;
                    it.Master_DB.Rows20.Add(ty1);
                    it.Master_DB.Rows20.Add(ty2);
                }

                it.Master_DB.Rows10.Sorted = false;
                it.Master_DB.Rows20.Sorted = false;
                await it.Master_DB.Rows10.QuickSortAsync();
                await it.Master_DB.Rows20.QuickSortAsync();
            }
        }
    }
}