using Avalonia.Controls;
using Client_App.Commands.AsyncCommands.SumRow;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.Views;
using Client_App.Views.Forms.Forms2;
using Client_App.VisualRealization.Long_Visual;
using Models.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Resources;

/// <summary>
/// Открытие форм 2.1–2.12 в новых окнах и подготовка данных (суммирование строк).
/// </summary>
public static class Form2NewInterfaceOpener
{
    private static readonly HashSet<string> SupportedForms =
    [
        "2.1", "2.2", "2.3", "2.4", "2.5", "2.6", "2.7", "2.8", "2.9", "2.10", "2.11", "2.12"
    ];

    public static bool IsSupportedForm(string? formNum) =>
        formNum is not null && SupportedForms.Contains(formNum);

    public static async Task PrepareSumRowsAsync(ChangeOrCreateVM changeOrCreateVM, string numForm)
    {
        switch (numForm)
        {
            case "2.1":
                Form2_Visual.tmpVM = changeOrCreateVM;
                if (changeOrCreateVM.isSum)
                {
                    await new CancelSumRowAsyncCommand(changeOrCreateVM).AsyncExecute(null);
                    await new SumRowAsyncCommand(changeOrCreateVM).AsyncExecute(null);
                }

                break;

            case "2.2":
                Form2_Visual.tmpVM = changeOrCreateVM;
                if (changeOrCreateVM.isSum)
                {
                    var sumRow = changeOrCreateVM.Storage.Rows22
                        .Where(x => x.Sum_DB)
                        .ToList();
                    var dic = new Dictionary<long, List<string>>();
                    foreach (var oldR in sumRow)
                    {
                        dic[oldR.NumberInOrder_DB] = [oldR.PackQuantity_DB, oldR.VolumeInPack_DB, oldR.MassInPack_DB];
                    }

                    await new CancelSumRowAsyncCommand(changeOrCreateVM).AsyncExecute(null);
                    await new SumRowAsyncCommand(changeOrCreateVM).AsyncExecute(null);

                    var newSumRow = changeOrCreateVM.Storage.Rows22
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

    public static async Task ShowDialogAsync(string numForm, Report report, MainWindow? mainWindow)
    {
        if (mainWindow is null) return;

        Window window = numForm switch
        {
            "2.1" => new Form_21(new Form_21VM(report)),
            "2.2" => new Form_22(new Form_22VM(report)),
            "2.3" => new Form_23(new Form_23VM(report)),
            "2.4" => new Form_24(new Form_24VM(report)),
            "2.5" => new Form_25(new Form_25VM(report)),
            "2.6" => new Form_26(new Form_26VM(report)),
            "2.7" => new Form_27(new Form_27VM(report)),
            "2.8" => new Form_28(new Form_28VM(report)),
            "2.9" => new Form_29(new Form_29VM(report)),
            "2.10" => new Form_210(new Form_210VM(report)),
            "2.11" => new Form_211(new Form_211VM(report)),
            "2.12" => new Form_212(new Form_212VM(report)),
            _ => throw new System.ArgumentOutOfRangeException(nameof(numForm), numForm, null)
        };

        if (window.DataContext is BaseFormVM formVm)
            formVm.BeginContentLoading("\u041e\u0442\u043a\u0440\u044b\u0442\u0438\u0435 \u043e\u0442\u0447\u0451\u0442\u0430\u2026");

        if (window is IFormOwnerStateWindow ownerState)
            ownerState.OwnerPrevState = mainWindow.WindowState;

        mainWindow.WindowState = WindowState.Minimized;
        mainWindow.SetReportOpeningOverlay(true);

        if (window is IFormDialogHost dialogHost)
            await dialogHost.ShowFormDialogAsync(mainWindow);
        else
            await window.ShowDialog(mainWindow);
    }
}
