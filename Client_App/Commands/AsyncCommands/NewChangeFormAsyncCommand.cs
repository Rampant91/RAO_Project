using Avalonia.Controls;
using Client_App.Commands.AsyncCommands.SumRow;
using Client_App.Resources;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.Views;
using Client_App.Views.Forms.Forms1;
using Client_App.Views.Forms.Forms4;
using Client_App.VisualRealization.Long_Visual;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Открыть окно редактирования выбранной формы.
/// </summary>
/// <param name="formParam">Содержит коллекцию отчётов Parameter (костыль, оттуда мы всегда берём только один отчёт)
/// и окно Window, которое нужно закрыть.</param>
public class NewChangeFormAsyncCommand(FormParameter? formParam = null) : BaseAsyncCommand
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
        if (parameter is Report report)
        {

            var t = Desktop.MainWindow as MainWindow;
            var numForm = report.FormNum.Value;
            report = await ReportsStorage.GetReportAsync(report.Id);
            switch (numForm)
            {
                case "1.1":
                    {
                        var form11VM = new Form_11VM(report);
                        var window = new Form_11(form11VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.2":
                    {
                        var form12VM = new Form_12VM(report);
                        var window = new Form_12(form12VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.3":
                    {
                        var form13VM = new Form_13VM(report);
                        var window = new Form_13(form13VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.4":
                    {
                        var form14VM = new Form_14VM(report);
                        var window = new Form_14(form14VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.5":
                    {
                        var form15VM = new Form_15VM(report);
                        var window = new Form_15(form15VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.6":
                    {
                        var form16VM = new Form_16VM(report);
                        var window = new Form_16(form16VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.7":
                    {
                        var form17VM = new Form_17VM(report);
                        var window = new Form_17(form17VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.8":
                    {
                        var form18VM = new Form_18VM(report);
                        var window = new Form_18(form18VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "1.9":
                    {
                        var form19VM = new Form_19VM(report);
                        var window = new Form_19(form19VM);
                        await window.ShowDialog(t);
                        break;
                    }
                case "4.1":
                    {
                        var form41VM = new Form_41VM(report);
                        var window = new Form_41(form41VM);
                        await window.ShowDialog(t);
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