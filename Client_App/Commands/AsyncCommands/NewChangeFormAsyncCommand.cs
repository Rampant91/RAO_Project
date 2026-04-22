using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.ViewModels.Forms.Forms5;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms2;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.Forms.Forms5;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Открыть окно редактирования выбранной формы.
/// </summary>
public class NewChangeFormAsyncCommand : BaseAsyncCommand
{
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
        if (parameter is Report report)
        {

            var t = Desktop.MainWindow as MainWindow;

            var numForm = report.FormNum.Value;
            var dBModel = StaticConfiguration.DBModel;
            
            switch (numForm)
            {
                case "1.1":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows11)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows11 = dbReport.Rows11;
                        var form11VM = new Form_11VM(report);
                        var window = new Form_11(form11VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.2":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows12)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows12 = dbReport.Rows12;
                        var form12VM = new Form_12VM(report);
                        var window = new Form_12(form12VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.3":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows13)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows13 = dbReport.Rows13;
                        var form13VM = new Form_13VM(report);
                        var window = new Form_13(form13VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.4":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows14)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows14 = dbReport.Rows14;
                        var form14VM = new Form_14VM(report);
                        var window = new Form_14(form14VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.5":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows15)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows15 = dbReport.Rows15;
                        var form15VM = new Form_15VM(report);
                        var window = new Form_15(form15VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.6":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows16)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows16 = dbReport.Rows16;
                        var form16VM = new Form_16VM(report);
                        var window = new Form_16(form16VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.7":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows17)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows17 = dbReport.Rows17;
                        var form17VM = new Form_17VM(report);
                        var window = new Form_17(form17VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.8":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows18)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows18 = dbReport.Rows18;
                        var form18VM = new Form_18VM(report);
                        var window = new Form_18(form18VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.9":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows19)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows19 = dbReport.Rows19;
                        var form19VM = new Form_19VM(report);
                        var window = new Form_19(form19VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.1":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows21)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows21 = dbReport.Rows21;
                        var form21VM = new Form_21VM(report);
                        var window = new Form_21(form21VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.2":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows22)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows22 = dbReport.Rows22;
                        var form22VM = new Form_22VM(report);
                        var window = new Form_22(form22VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.3":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows23)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows23 = dbReport.Rows23;
                        var form23VM = new Form_23VM(report);
                        var window = new Form_23(form23VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.4":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows24)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows24 = dbReport.Rows24;
                        var form24VM = new Form_24VM(report);
                        var window = new Form_24(form24VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.5":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows25)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows25 = dbReport.Rows25;
                        var form25VM = new Form_25VM(report);
                        var window = new Form_25(form25VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.6":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows26)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows26 = dbReport.Rows26;
                        var form26VM = new Form_26VM(report);
                        var window = new Form_26(form26VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.7":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows27)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows27 = dbReport.Rows27;
                        var form27VM = new Form_27VM(report);
                        var window = new Form_27(form27VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.8":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows28)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows28 = dbReport.Rows28;
                        var form28VM = new Form_28VM(report);
                        var window = new Form_28(form28VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.9":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows29)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows29 = dbReport.Rows29;
                        var form29VM = new Form_29VM(report);
                        var window = new Form_29(form29VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.10":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows210)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows210 = dbReport.Rows210;
                        var form210VM = new Form_210VM(report);
                        var window = new Form_210(form210VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.11":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows211)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows211 = dbReport.Rows211;
                        var form211VM = new Form_211VM(report);
                        var window = new Form_211(form211VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "2.12":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Reports)
                            .Include(rep => rep.Rows212)
                            .Include(rep => rep.Notes)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows212 = dbReport.Rows212;
                        var form212VM = new Form_212VM(report);
                        var window = new Form_212(form212VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "4.1":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows41)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows41 = dbReport.Rows41;
                        var form41VM = new Form_41VM(report);
                        var window = new Form_41(form41VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "5.1":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows51)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows51 = dbReport.Rows51;
                        var form51VM = new Form_51VM(report);
                        var window = new Form_51(form51VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "5.2":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows52)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows52 = dbReport.Rows52;
                        var form52VM = new Form_52VM(report);
                        var window = new Form_52(form52VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "5.3":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows53)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows53 = dbReport.Rows53;
                        var form53VM = new Form_53VM(report);
                        var window = new Form_53(form53VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "5.4":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows54)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows54 = dbReport.Rows54;
                        var form54VM = new Form_54VM(report);
                        var window = new Form_54(form54VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "5.5":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows55)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows55 = dbReport.Rows55;
                        var form55VM = new Form_55VM(report);
                        var window = new Form_55(form55VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "5.6":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows56)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows56 = dbReport.Rows56;
                        var form56VM = new Form_56VM(report);
                        var window = new Form_56(form56VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "5.7":
                    {
                        var dbReport = dBModel.ReportCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(rep => rep.Rows57)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows57 = dbReport.Rows57;
                        var form57VM = new Form_57VM(report);
                        var window = new Form_57(form57VM);
                        await window.ShowDialog(t);
                        break;
                    }
            }

        }
    }

    #endregion

}