using Client_App.ViewModels;
using Client_App.Views;
using Models.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.VisualRealization.Long_Visual;
using Models.Interfaces;
using System.Reactive.Linq;
using Avalonia.Controls;
using Client_App.Commands.AsyncCommands.SumRow;
using Models.Classes;

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
            var rep = (Report)obj;
            //var tre = ReportsStorage.LocalReports.Reports_Collection
            //    .FirstOrDefault(i => i.Report_Collection.Contains(rep));
            var numForm = rep.FormNum.Value;
            var frm = new ChangeOrCreateVM(numForm, rep);
            switch (numForm)
            {
                case "1.1":
                {
                    Form1_Visual.tmpVM = frm;
                    break;
                }
                case "2.1":
                    {
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
            }
            await MainWindowVM.ShowDialog.Handle(frm);
            t.SelectedReports = tmp;
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