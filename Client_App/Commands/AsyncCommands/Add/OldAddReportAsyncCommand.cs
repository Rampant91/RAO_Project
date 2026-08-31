using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.Views;
using Client_App.VisualRealization.Long_Visual;
using MsBox.Avalonia.Dto;
using Models.Collections;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Создать и открыть новое окно с отчётом по форме 2.x для выбранной организации.
/// Для 1.x, 4.1 и 5.x используется новая команда, эту удалим когда обновим интерфейс форм 2.
/// </summary>
public class OldAddReportAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is string param)
        {
            var mainWindow = Desktop.MainWindow as MainWindow;
            var mainWindowVM = mainWindow?.DataContext as MainWindowVM;

            Reports? selectedReports;

            if (mainWindowVM?.SelectedReports is not null)
            {
                selectedReports = mainWindowVM.SelectedReports;     //новая реализация
            }
            else
            {
                #region MessageFailedToOpenForm

                await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = $"Создание формы {param}",
                        ContentHeader = "Ошибка",
                        ContentMessage =
                            $"Не удалось создать форму {param}, поскольку не выбрана организация. Перед созданием формы убедитесь,"
                            + $"{Environment.NewLine}что в списке организаций имеется выбранная организация (подсвечивается голубым цветом).",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    }).ShowWindowDialogAsync(Desktop.MainWindow));
                #endregion

                return;
            }

            if (selectedReports?.Master.FormNum_DB.Split('.')[0] == param.Split('.')[0])
            {

                ChangeOrCreateVM frm = new(param, selectedReports);

                if (param.Split('.')[0] is "2")
                {
                    Form2_Visual.tmpVM = param switch
                    {
                        "2.1" or "2.2" => frm,
                        _ => Form2_Visual.tmpVM
                    };
                    await MainWindowVM.ShowDialog.Handle(frm);
                    await selectedReports.Report_Collection.QuickSortAsync();
                }

                mainWindowVM.UpdateReportCollection();
                mainWindowVM.UpdateFormsPageInfo();
                mainWindowVM.UpdateTotalReportCount();
            }
        }
    }
}
