using Avalonia.Controls;
using Client_App.Commands.AsyncCommands.SumRow;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.VisualRealization.Long_Visual;
using Models.Classes;
using Models.Collections;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client_App.Resources;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Открыть окно редактирования выбранной формы.
/// </summary>
/// <param name="formParam">Содержит коллекцию отчётов Parameter (костыль, оттуда мы всегда берём только один отчёт)
/// и окно Window, которое нужно закрыть.</param>
public class ChangeFormAsyncCommand(FormParameter? formParam = null) : BaseAsyncCommand
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
            await Execute();
        }
        else if (formParam != null)
        {
            await Execute(formParam.Window);
        }
    }

    #endregion

    #region Execute

    public override async void Execute(object? parameter)
    {
        await OpenReport(parameter);
    }

    private Task Execute(Window? window = null)
    {
        window.Closed += WindowClosed;
        window.Close();

        return Task.CompletedTask;
    }

    #endregion

    #region OpenReport

    private static async Task OpenReport(object? parameter)
    {
        if (parameter is ObservableCollectionWithItemPropertyChanged<IKey> { Count: > 0 } param && param.First() is { } obj)
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
            switch (numForm)
            {
                case "1.2":
                    {
                        var form12VM = new Form_12VM(frm.Storage);
                        var window = new Form_12(form12VM);
                        await window.ShowDialog(t);
                        break;
                    }
                default:
                    {
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
        if (formParam == null) return;
        await OpenReport(formParam.Parameter).ConfigureAwait(false);
    }

    #endregion
}