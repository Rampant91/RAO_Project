using Avalonia.Controls;
using Client_App.Commands.AsyncCommands.SumRow;
using Client_App.Resources;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms4;
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
                            .Include(rep => rep.Rows11)
                            .Include(rep => rep.Reports)
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
                            .Include(rep => rep.Rows12)
                            .Include(rep => rep.Reports)
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
                            .Include(rep => rep.Rows13)
                            .Include(rep => rep.Reports)
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
                            .Include(rep => rep.Rows14)
                            .Include(rep => rep.Reports)
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
                            .Include(rep => rep.Rows15)
                            .Include(rep => rep.Reports)
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
                            .Include(rep => rep.Rows16)
                            .Include(rep => rep.Reports)
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
                            .Include(rep => rep.Rows17)
                            .Include(rep => rep.Reports)
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
                            .Include(rep => rep.Rows18)
                            .Include(rep => rep.Reports)
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
                            .Include(rep => rep.Rows19)
                            .Include(rep => rep.Reports)
                            .FirstOrDefault(x => x.Id == report.Id);
                        report.Rows19 = dbReport.Rows19;
                        var form19VM = new Form_19VM(report);
                        var window = new Form_19(form19VM);
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
            }

        }
    }

    #endregion

}