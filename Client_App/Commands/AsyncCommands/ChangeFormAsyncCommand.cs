using System;
using Avalonia.Controls;
using Client_App.Commands.AsyncCommands.SumRow;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.VisualRealization.Long_Visual;
using Microsoft.EntityFrameworkCore;
using Models.Classes;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Открыть окно редактирования выбранной формы.
/// </summary>
/// <param name="form">Параметр и окно, которое нужно закрыть.</param>
public class ChangeFormAsyncCommand(FormParameter? form = null) : BaseAsyncCommand
{
    #region AsyncExecute
    
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter != null)
        {
            await Execute(parameter);
        }
        else if (form != null)
        {
            await Execute(form.Parameter, form.Window);
        }
    }

    #endregion

    #region Execute

    private async Task Execute(object? parameter, Window? window = null)
    {
        if (window != null)
        {
            window.Closed += WindowClosed;
            window.Close();
        }
        else
        {
            await OpenReport(parameter);
        }
    }

    #endregion

    #region OpenReport

    private static async Task OpenReport(object? parameter)
    {
        if (parameter is ObservableCollectionWithItemPropertyChanged<IKey> param && param.First() is { } obj)
        {
            var t = Desktop.MainWindow as MainWindow;
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);
            var repWithoutRows = (Report)obj;

            await using var db = new DBModel(StaticConfiguration.DBPath);

            #region GetReportFromDB

            var rep = await db.ReportCollectionDbSet
                .AsNoTracking()
                .AsQueryable()
                .AsSplitQuery()
                .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Rows11)
                .Include(x => x.Rows12)
                .Include(x => x.Rows13)
                .Include(x => x.Rows14)
                .Include(x => x.Rows15)
                .Include(x => x.Rows16)
                .Include(x => x.Rows17)
                .Include(x => x.Rows18)
                .Include(x => x.Rows19)
                .Include(x => x.Rows21)
                .Include(x => x.Rows22)
                .Include(x => x.Rows23)
                .Include(x => x.Rows24)
                .Include(x => x.Rows25)
                .Include(x => x.Rows26)
                .Include(x => x.Rows27)
                .Include(x => x.Rows28)
                .Include(x => x.Rows29)
                .Include(x => x.Rows210)
                .Include(x => x.Rows211)
                .Include(x => x.Rows212)
                .Include(x => x.Notes)
                //.Where(x => x.Reports != null && x.Reports.DBObservable != null)
                .FirstAsync(x => x.Id == repWithoutRows.Id);
            rep.Reports = (Reports)t.SelectedReports.First();

            #endregion

            //var tre = ReportsStorage.LocalReports.Reports_Collection
            //    .FirstOrDefault(i => i.Report_Collection.Contains(rep));
            var numForm = rep.FormNum.Value;
            switch (numForm)
            {
                case "1.2":
                {
                    var form12VM = new Form_12VM(rep);
                    var window = new Form_12(form12VM);
                    await window.ShowDialog(t);

                    break;
                }
                case "2.1":
                {
                    var frm = new ChangeOrCreateVM(numForm, rep);
                    Form2_Visual.tmpVM = frm;
                    if (frm.isSum)
                    {
                        //var sumRow = frm.Storage.Rows21.Where(x => x.Sum_DB == true);
                        await new CancelSumRowAsyncCommand(frm).AsyncExecute(null);
                        await new SumRowAsyncCommand(frm).AsyncExecute(null);
                        //var newSumRow = frm.Storage.Rows21.Where(x => x.Sum_DB == true);
                    }

                    break;
                }
                case "2.2":
                {
                    var frm = new ChangeOrCreateVM(numForm, rep);
                    Form2_Visual.tmpVM = frm;
                    if (frm.isSum)
                    {
                        var sumRow = frm.Storage.Rows22
                            .Where(x => x.Sum_DB)
                            .ToList();
                        Dictionary<long, List<string>> dic = new();
                        foreach (var oldR in sumRow)
                        {
                            dic[oldR.NumberInOrder_DB] = new List<string>
                            { oldR.PackQuantity_DB, oldR.VolumeInPack_DB, oldR.MassInPack_DB };
                        }

                        await new CancelSumRowAsyncCommand(frm).AsyncExecute(null);
                        await new SumRowAsyncCommand(frm).AsyncExecute(null);
                        var newSumRow = frm.Storage.Rows22
                            .Where(x => x.Sum_DB)
                            .ToList();

                        foreach (var newR in newSumRow)
                        {
                            var matchDic = dic
                                .Where(oldR => newR.NumberInOrder_DB == oldR.Key)
                                .ToList();
                            foreach (var oldR in matchDic)
                            {
                                newR.PackQuantity_DB = oldR.Value[0];
                                newR.VolumeInPack_DB = oldR.Value[1];
                                newR.MassInPack_DB = oldR.Value[2];
                            }
                        }
                    }

                    break;
                }
                default:
                {
                    ChangeOrCreateVM frm;
                    try
                    {
                        frm = new ChangeOrCreateVM(numForm, rep);
                    }
                    catch(Exception ex)
                    {
                        frm = new ChangeOrCreateVM(numForm, rep.Reports);
                    }
                    Form1_Visual.tmpVM = frm;
                    await MainWindowVM.ShowDialog.Handle(frm);
                    t.SelectedReports = tmp;

                    break;
                }
            }
        }
    }

    #endregion

    #region Events

    private async void WindowClosed(object? sender, System.EventArgs e)
    {
        if (form == null) return;
        await OpenReport(form.Parameter).ConfigureAwait(false);
    }

    #endregion
}