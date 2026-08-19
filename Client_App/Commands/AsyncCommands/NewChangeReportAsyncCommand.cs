using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
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
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Открыть окно редактирования выбранного отчета.
/// </summary>
public class NewChangeReportAsyncCommand : BaseAsyncCommand
{
    private readonly FormsTabControlBaseVM _formsTabControlVM;

    public NewChangeReportAsyncCommand(FormsTabControlBaseVM formsTabControlVM)
    {
        _formsTabControlVM = formsTabControlVM;

        formsTabControlVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(FormsTabControlBaseVM.SelectedReport))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _formsTabControlVM.SelectedReport is not null;

    #region AsyncExecute

    /// <summary>
    /// Используется при вызове из других команд
    /// </summary>
    /// <param name="parameter"></param>
    /// <returns></returns>
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter != null)
        {
            Execute(parameter);
        }
    }

    #endregion

    #region Execute

    public override async void Execute(object? parameter)
    {
        await OpenReport(parameter);
    }


    #endregion

    #region OpenReport

    private static async Task OpenReport(object? parameter)
    {
        if (parameter is not Report report)
            return;

        var owner = Desktop.MainWindow as MainWindow;
        owner?.SetReportOpeningOverlay(true);
        try
        {
            await OpenReportCore(report, owner);
        }
        finally
        {
            owner?.SetReportOpeningOverlay(false);
        }
    }

    private static async Task OpenReportCore(Report report, MainWindow? owner)
    {
        var numForm = report.FormNum.Value;
        var dBModel = StaticConfiguration.DBModel;

            var queryWithNotes = dBModel.ReportCollectionDbSet
                .AsSplitQuery()
                .AsQueryable()
                .Include(r => r.Reports)
                .Include(rep => rep.Notes);

            var queryWithOutNotes = dBModel.ReportCollectionDbSet
                .AsSplitQuery()
                .AsQueryable()
                .Include(r => r.Reports);

            switch (numForm)
            {
                case "1.1":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.1",
                        r => new Form_11VM(r), vm => new Form_11(vm));
                    break;
                case "1.2":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.2",
                        r => new Form_12VM(r), vm => new Form_12(vm));
                    break;
                case "1.3":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.3",
                        r => new Form_13VM(r), vm => new Form_13(vm));
                    break;
                case "1.4":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.4",
                        r => new Form_14VM(r), vm => new Form_14(vm));
                    break;
                case "1.5":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.5",
                        r => new Form_15VM(r), vm => new Form_15(vm));
                    break;
                case "1.6":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.6",
                        r => new Form_16VM(r), vm => new Form_16(vm));
                    break;
                case "1.7":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.7",
                        r => new Form_17VM(r), vm => new Form_17(vm));
                    break;
                case "1.8":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.8",
                        r => new Form_18VM(r), vm => new Form_18(vm));
                    break;
                case "1.9":
                    await OpenForm1xPagedAsync(owner, report, dBModel, "1.9",
                        r => new Form_19VM(r), vm => new Form_19(vm));
                    break;
                case "2.1":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows21)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows21 = dbReport.Rows21;
                    var form21VM = new Form_21VM(report);
                    var window = new Form_21(form21VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.2":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows22)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows22 = dbReport.Rows22;
                    var form22VM = new Form_22VM(report);
                    var window = new Form_22(form22VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.3":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows23)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows23 = dbReport.Rows23;
                    var form23VM = new Form_23VM(report);
                    var window = new Form_23(form23VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.4":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows24)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows24 = dbReport.Rows24;
                    var form24VM = new Form_24VM(report);
                    var window = new Form_24(form24VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.5":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows25)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows25 = dbReport.Rows25;
                    var form25VM = new Form_25VM(report);
                    var window = new Form_25(form25VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.6":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows26)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows26 = dbReport.Rows26;
                    var form26VM = new Form_26VM(report);
                    var window = new Form_26(form26VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.7":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows27)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows27 = dbReport.Rows27;
                    var form27VM = new Form_27VM(report);
                    var window = new Form_27(form27VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.8":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows28)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows28 = dbReport.Rows28;
                    var form28VM = new Form_28VM(report);
                    var window = new Form_28(form28VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.9":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows29)
                        .FirstOrDefault(x => x.Id == report.Id);
                
                    report.Rows29 = dbReport.Rows29;
                    var form29VM = new Form_29VM(report);
                    var window = new Form_29(form29VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.10":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows210)
                        .FirstOrDefault(x => x.Id == report.Id);
                
                    report.Rows210 = dbReport.Rows210;
                    var form210VM = new Form_210VM(report);
                    var window = new Form_210(form210VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.11":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows211)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows211 = dbReport.Rows211;
                    var form211VM = new Form_211VM(report);
                    var window = new Form_211(form211VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "2.12":
                {
                    var dbReport = queryWithNotes
                        .Include(rep => rep.Rows212)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows212 = dbReport.Rows212;
                    var form212VM = new Form_212VM(report);
                    var window = new Form_212(form212VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "4.1":
                {
                    var dbReport = queryWithOutNotes
                        .Include(rep => rep.Rows41)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows41 = dbReport.Rows41;
                    var form41VM = new Form_41VM(report);
                    var window = new Form_41(form41VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "5.1":
                {
                    var dbReport = queryWithOutNotes
                        .Include(rep => rep.Rows51)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows51 = dbReport.Rows51;
                    var form51VM = new Form_51VM(report);
                    var window = new Form_51(form51VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "5.2":
                {
                    var dbReport = queryWithOutNotes
                        .Include(rep => rep.Rows52)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows52 = dbReport.Rows52;
                    var form52VM = new Form_52VM(report);
                    var window = new Form_52(form52VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "5.3":
                {
                    var dbReport = queryWithOutNotes
                        .Include(rep => rep.Rows53)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows53 = dbReport.Rows53;
                    var form53VM = new Form_53VM(report);
                    var window = new Form_53(form53VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "5.4":
                {
                    var dbReport = queryWithOutNotes
                        .Include(rep => rep.Rows54)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows54 = dbReport.Rows54;
                    var form54VM = new Form_54VM(report);
                    var window = new Form_54(form54VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "5.5":
                {
                    var dbReport = queryWithOutNotes
                        .Include(rep => rep.Rows55)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows55 = dbReport.Rows55;
                    var form55VM = new Form_55VM(report);
                    var window = new Form_55(form55VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "5.6":
                {
                    var dbReport = queryWithOutNotes
                        .Include(rep => rep.Rows56)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows56 = dbReport.Rows56;
                    var form56VM = new Form_56VM(report);
                    var window = new Form_56(form56VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
                case "5.7":
                {
                    var dbReport = queryWithOutNotes
                        .Include(rep => rep.Rows57)
                        .FirstOrDefault(x => x.Id == report.Id);

                    report.Rows57 = dbReport.Rows57;
                    var form57VM = new Form_57VM(report);
                    var window = new Form_57(form57VM);
                    await ShowFormDialogAsync(window, owner);
                    break;
                }
            }
    }

    private static async Task ShowFormDialogAsync(Window window, MainWindow? owner)
    {
        if (window is IFormDialogHost host)
            await host.ShowFormDialogAsync(owner);
        else if (owner != null)
            await window.ShowDialog(owner);
    }

    private static async Task OpenForm1xPagedAsync<TVm>(
        MainWindow? owner,
        Report report,
        DBModel db,
        string formNum,
        Func<Report, TVm> createVm,
        Func<TVm, Window> createWindow)
        where TVm : BaseFormVM
    {
        // Грид отдаёт AsNoTracking-оболочку: правки/новые строки иначе не в ChangeTracker.
        var tracked = db.ReportCollectionDbSet.Local.FirstOrDefault(r => r.Id == report.Id);
        if (tracked == null)
        {
            tracked = await db.ReportCollectionDbSet
                .AsSplitQuery()
                .Include(r => r.Reports!).ThenInclude(reps => reps.Master_DB).ThenInclude(m => m.Rows10)
                .Include(r => r.Reports!).ThenInclude(reps => reps.Master_DB).ThenInclude(m => m.Rows20)
                .Include(r => r.Notes)
                .FirstOrDefaultAsync(x => x.Id == report.Id);
        }
        else
        {
            // Догружаем навигации, если открыли уже tracked stub без Master/Notes.
            if (tracked.Reports == null || tracked.Notes == null || tracked.Notes.Count == 0)
            {
                var meta = await db.ReportCollectionDbSet
                    .AsSplitQuery()
                    .Include(r => r.Reports!).ThenInclude(reps => reps.Master_DB).ThenInclude(m => m.Rows10)
                    .Include(r => r.Reports!).ThenInclude(reps => reps.Master_DB).ThenInclude(m => m.Rows20)
                    .Include(r => r.Notes)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == report.Id);
                if (meta != null)
                {
                    if (tracked.Reports == null)
                        tracked.Reports = meta.Reports;
                    if (meta.Notes != null)
                        tracked.Notes = meta.Notes;
                }
            }
        }

        if (tracked == null)
            return;

        const int defaultPageSize = 30;
        var total = await FormRowsPageLoader.CountAsync(db, tracked.Id, formNum);
        await FormRowsPageLoader.LoadPageIntoReportAsync(db, tracked, formNum, skip: 0, take: defaultPageSize);

        var vm = createVm(tracked);
        vm.UseDbPaging = true;
        vm.DbTotalRows = total;
        vm.RowCount = defaultPageSize;
        vm.UpdateFormList();
        vm.UpdatePageInfo();

        var window = createWindow(vm);
        await ShowFormDialogAsync(window, owner);
    }

    #endregion

}