 using Avalonia.Controls;
using Client_App.Commands.AsyncCommands.SumRow;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.VisualRealization.Long_Visual;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client_App.Resources;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms2;

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
        var db = StaticConfiguration.DBModel;
        var modifiedEntities = db.ChangeTracker.Entries()
            .Where(x => x.State != EntityState.Unchanged);

        if (parameter is ObservableCollectionWithItemPropertyChanged<IKey> { Count: > 0 } param && param.First() is { } obj)
        {
            var mainWindow = Desktop.MainWindow as MainWindow;
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(mainWindow.SelectedReports);
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

            switch (numForm)
            {
                case "1.1":
                {
                    var form11VM = new Form_11VM(frm.Storage);
                    var form11Window = new Form_11(form11VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await form11Window.ShowDialog(mainWindow);
                    break;
                }
                case "1.2":
                {
                    var form12VM = new Form_12VM(frm.Storage);
                    var window = new Form_12(form12VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
                case "1.3":
                {
                    var form13VM = new Form_13VM(frm.Storage);
                    var window = new Form_13(form13VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
                case "1.4":
                {
                    var form14VM = new Form_14VM(frm.Storage);
                    var window = new Form_14(form14VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
                case "1.5":
                {
                    var form15VM = new Form_15VM(frm.Storage);
                    var window = new Form_15(form15VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
                case "1.6":
                {
                    var form16VM = new Form_16VM(frm.Storage);
                    var window = new Form_16(form16VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
                case "1.7":
                {
                    var form17VM = new Form_17VM(frm.Storage);
                    var window = new Form_17(form17VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
                case "1.8":
                {
                    var form18VM = new Form_18VM(frm.Storage);
                    var window = new Form_18(form18VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                }
                case "1.9":
                {
                    var form19VM = new Form_19VM(frm.Storage);
                    var window = new Form_19(form19VM) { OwnerPrevState = mainWindow.WindowState };
                    mainWindow.WindowState = WindowState.Minimized;
                    await window.ShowDialog(mainWindow);
                    break;
                    }
                case "2.1":
                    {
                        var form21VM = new Form_21VM(frm.Storage);
                        var window = new Form_21(form21VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.2":
                    {
                        var form22VM = new Form_22VM(frm.Storage);
                        var window = new Form_22(form22VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.3":
                    {
                        var form23VM = new Form_23VM(frm.Storage);
                        var window = new Form_23(form23VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.4":
                    {
                        var form24VM = new Form_24VM(frm.Storage);
                        var window = new Form_24(form24VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.5":
                    {
                        var form25VM = new Form_25VM(frm.Storage);
                        var window = new Form_25(form25VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.6":
                    {
                        var form26VM = new Form_26VM(frm.Storage);
                        var window = new Form_26(form26VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.7":
                    {
                        var form27VM = new Form_27VM(frm.Storage);
                        var window = new Form_27(form27VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.8":
                    {
                        var form28VM = new Form_28VM(frm.Storage);
                        var window = new Form_28(form28VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.9":
                    {
                        var form29VM = new Form_29VM(frm.Storage);
                        var window = new Form_29(form29VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.10":
                    {
                        var form210VM = new Form_210VM(frm.Storage);
                        var window = new Form_210(form210VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.11":
                    {
                        var form211VM = new Form_211VM(frm.Storage);
                        var window = new Form_211(form211VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "2.12":
                    {
                        var form212VM = new Form_212VM(frm.Storage);
                        var window = new Form_212(form212VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                case "4.1":
                    {
                        var form41VM = new Form_41VM(frm.Storage);
                        var window = new Form_41(form41VM) { OwnerPrevState = mainWindow.WindowState };
                        mainWindow.WindowState = WindowState.Minimized;
                        await window.ShowDialog(mainWindow);
                        break;
                    }
                default:
                {
                    await MainWindowVM.ShowDialog.Handle(frm);
                    mainWindow.SelectedReports = tmp;
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