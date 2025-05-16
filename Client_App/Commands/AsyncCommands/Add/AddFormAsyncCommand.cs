using Avalonia.Controls;
using Client_App.ViewModels;
using Client_App.Views;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client_App.VisualRealization.Long_Visual;
using Models.Interfaces;
using Avalonia.Threading;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Создать и открыть новое окно формы для выбранной организации.
/// </summary>
public class AddFormAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is string param)
        {
            var t = Desktop.MainWindow as MainWindow;
            if (t?.SelectedReports is null
                || !t.SelectedReports.Any()
                || ((Reports)t.SelectedReports.First()).Master.FormNum_DB[0] != param[0])
            {
                #region MessageFailedToOpenForm

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = $"Создание формы {param}",
                        ContentHeader = "Ошибка",
                        ContentMessage =
                            $"Не удалось создать форму {param}, поскольку не выбрана организация. Перед созданием формы убедитесь,"
                            + $"{Environment.NewLine}что в списке организаций имеется выбранная организация (подсвечивается голубым цветом).",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                return;
            }

            var y = t.SelectedReports.First() as Reports;
            if (y?.Master.FormNum_DB.Split(".")[0] == param.Split(".")[0])
            {
                var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>(t.SelectedReports);

                ChangeOrCreateVM frm = new(param, y);
                Form2_Visual.tmpVM = param switch
                {
                    "2.1" or "2.2" => frm,
                    _ => Form2_Visual.tmpVM
                };
                await MainWindowVM.ShowDialog.Handle(frm);
                t.SelectedReports = tmp;
                await y.Report_Collection.QuickSortAsync();
            }
        }
    }
}