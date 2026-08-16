using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.ViewModels.Forms.Forms5;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms2;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.Forms.Forms5;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Изменить Формы организации (1.0, 2.0, 4.0, 5.0).
/// </summary>
public class NewChangeReportsAsyncCommand : BaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public NewChangeReportsAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
    {
        _formsTabControlVM = formsTabControlVM;

        formsTabControlVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _formsTabControlVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        var mainWindow = (Desktop.MainWindow as MainWindow)!;
        var mainWindowVM = (mainWindow.DataContext as MainWindowVM)!;

        if (mainWindowVM.SelectedReports is null) return;

        var report = mainWindowVM.SelectedReports.Master;
        var formNum = report.FormNum.Value;
        var refreshOrgListAfterTitle = false;

        switch (formNum)
        {
            case "1.0":
            {
                await EnsureForm10RowsLoadedAsync(report);
                var titleBefore = SnapshotForm10Title(report);
                var form10VM = new Form_10VM(formNum, report)
                {
                    IsSeparateDivision = !string.IsNullOrWhiteSpace(report.Rows10[1].Okpo.Value)
                };
                var window = new Form_10(form10VM) { DataContext = form10VM };
                await window.ShowDialog(mainWindow);

                // Не InvalidateOrgKeys / не InvalidateOrg: полный rebuild ключей и сброс report-кэша
                // давали заметный лаг на UI до выбора следующей org.
                var titleAfter = SnapshotForm10Title(report);
                if (titleBefore != titleAfter)
                {
                    MainWindowListQuery.UpsertOrgKeyForm10FromMaster(
                        mainWindowVM.SelectedReports.Id, report);
                    mainWindowVM.Forms1TabControlVM.RefreshOrgListAfterTitleChange();
                }

                refreshOrgListAfterTitle = true;
                break;
            }
            case "2.0":
            {
                var form20VM = new Form_20VM(formNum, report)
                {
                    IsSeparateDivision = !string.IsNullOrWhiteSpace(report.Rows20[1].Okpo.Value)
                };
                var window = new Form_20(form20VM) { DataContext = form20VM };
                await window.ShowDialog(mainWindow);
                break;
            }
            case "4.0":
            {
                var form40VM = new Form_40VM(formNum, report);
                var window = new Form_40(form40VM) { DataContext = form40VM };
                await window.ShowDialog(mainWindow);
                break;
            }
            case "5.0":
            {
                var form50VM = new Form_50VM(formNum, report);
                var window = new Form_50(form50VM) { DataContext = form50VM };
                await window.ShowDialog(mainWindow);
                break;
            }
        }

        // Для 1.0 список org уже обновлён точечно (или не менялся) — без Sync через getter ReportsCollection.
        if (!refreshOrgListAfterTitle)
            mainWindowVM.UpdateReportsCollection();
    }

    private static Form10TitleSelector.TitleFields SnapshotForm10Title(Report master)
    {
        var rows = master.Rows10.OrderBy(r => r.NumberInOrder_DB).ToList();
        var r0 = rows.ElementAtOrDefault(0);
        var r1 = rows.ElementAtOrDefault(1);
        return Form10TitleSelector.Pick(
            r0?.RegNo_DB, r0?.Okpo_DB, r0?.ShortJurLico_DB,
            r1?.RegNo_DB, r1?.Okpo_DB, r1?.ShortJurLico_DB);
    }

    /// <summary>
    /// Страница грида уже Include'ит Rows10; если сущности detached/неполные — догружаем из БД.
    /// </summary>
    private static async Task EnsureForm10RowsLoadedAsync(Report master)
    {
        if (master.Rows10.Count >= 2)
            return;

        await using var db = new DBModel(StaticConfiguration.DBPath);
        var rows = await db.form_10
            .AsNoTracking()
            .Where(f => f.ReportId == master.Id)
            .OrderBy(f => f.NumberInOrder_DB)
            .ToListAsync();

        master.Rows10.Clear();
        foreach (var row in rows)
            master.Rows10.Add(row);

        // Гарантируем две строки для UI (как ProcessDataBaseFillEmpty).
        while (master.Rows10.Count < 2)
        {
            var empty = (Form10)FormCreator.Create("1.0");
            empty.NumberInOrder_DB = (short)(master.Rows10.Count + 1);
            master.Rows10.Add(empty);
        }
    }
}
