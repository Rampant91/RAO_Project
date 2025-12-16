using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms.Forms1;
using Client_App.Views;
using Client_App.VisualRealization.Long_Visual;
using MessageBox.Avalonia.DTO;
using Models.Collections;
using Models.Interfaces;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Client_App.Views.Forms.Forms1;

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
            var mainWindow = Desktop.MainWindow as MainWindow;
            var mainWindowVM = mainWindow.DataContext as MainWindowVM;

            Reports? selectedReports;

            if (mainWindowVM.SelectedReports is not null)
            {
                selectedReports = mainWindowVM.SelectedReports;     //новая реализация
            }
            else
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



            if (selectedReports?.Master.FormNum_DB.Split(".")[0] == param.Split(".")[0])
            {

                ChangeOrCreateVM frm = new(param, selectedReports);

                
                switch (param)
                {
                    case "1.1":
                    {
                        var form11Window = new Form_11(new Form_11VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.2":
                    {
                        var form11Window = new Form_12(new Form_12VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.3":
                    {
                        var form11Window = new Form_13(new Form_13VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.4":
                    {
                        var form11Window = new Form_14(new Form_14VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                            await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.5":
                    {
                        var form11Window = new Form_15(new Form_15VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.6":
                    {
                        var form11Window = new Form_16(new Form_16VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.7":
                    {
                        var form11Window = new Form_17(new Form_17VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.8":
                    {
                        var form11Window = new Form_18(new Form_18VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.9":
                    {
                        var form11Window = new Form_19(new Form_19VM(selectedReports));
                        mainWindow.WindowState = WindowState.Minimized;
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    default:
                    {
                        if (param.Split(".")[0] is "2")
                        {
                            Form2_Visual.tmpVM = param switch
                            {
                                "2.1" or "2.2" => frm,
                                _ => Form2_Visual.tmpVM
                            };
                            await MainWindowVM.ShowDialog.Handle(frm);
                            await selectedReports.Report_Collection.QuickSortAsync();
                        }

                        break;
                    }

                }

                /*
                if(param == "1.2")
                {
                    Form_12 form12 = new Form_12();
                    form12.ShowDialog(t);
                }
                else */
                //if (param.Split(".")[0] is "1")
                //{
                //    Form1_Visual.tmpVM = frm;
                //    await MainWindowVM.ShowDialog.Handle(frm);
                //    t.SelectedReports = tmp;
                //    await y.Report_Collection.QuickSortAsync();
                //}

            }
        }
    }
}