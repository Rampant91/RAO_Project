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
using Client_App.ViewModels.Forms.Forms4;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.Forms.Forms2;
using Client_App.ViewModels.Forms.Forms2;
using Client_App.Views.Forms.Forms5;
using Client_App.ViewModels.Forms.Forms5;

namespace Client_App.Commands.AsyncCommands.Add;

/// <summary>
/// Создать и открыть новое окно формы для выбранной организации.
/// </summary>
public class NewAddFormAsyncCommand : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is string param)
        {
            var mainWindow = Desktop.MainWindow as MainWindow;
            var mainWindowVM = mainWindow.DataContext as MainWindowVM;
            if (mainWindowVM.SelectedReports is null)
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

            var selectedReports = mainWindowVM.SelectedReports;
            if (selectedReports?.Master.FormNum_DB.Split(".")[0] == param.Split(".")[0])
            {
                switch (param)
                {
                    case "1.1":
                    {
                        var form11Window = new Form_11(new Form_11VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.2":
                    {
                        var form11Window = new Form_12(new Form_12VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.3":
                    {
                        var form11Window = new Form_13(new Form_13VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.4":
                    {
                        var form11Window = new Form_14(new Form_14VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.5":
                    {
                        var form11Window = new Form_15(new Form_15VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.6":
                    {
                        var form11Window = new Form_16(new Form_16VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.7":
                    {
                        var form11Window = new Form_17(new Form_17VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.8":
                    {
                        var form11Window = new Form_18(new Form_18VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                    }
                    case "1.9":
                    {
                        var form11Window = new Form_19(new Form_19VM(selectedReports));
                        await form11Window.ShowDialog(mainWindow);
                        await selectedReports.Report_Collection.QuickSortAsync();
                        break;
                        }
                    case "2.1":
                        {
                            var form21Window = new Form_21(new Form_21VM(selectedReports));
                            await form21Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.2":
                        {
                            var form22Window = new Form_22(new Form_22VM(selectedReports));
                            await form22Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.3":
                        {
                            var form23Window = new Form_23(new Form_23VM(selectedReports));
                            await form23Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.4":
                        {
                            var form24Window = new Form_24(new Form_24VM(selectedReports));
                            await form24Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.5":
                        {
                            var form25Window = new Form_25(new Form_25VM(selectedReports));
                            await form25Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.6":
                        {
                            var form26Window = new Form_26(new Form_26VM(selectedReports));
                            await form26Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.7":
                        {
                            var form27Window = new Form_27(new Form_27VM(selectedReports));
                            await form27Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.8":
                        {
                            var form28Window = new Form_28(new Form_28VM(selectedReports));
                            await form28Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.9":
                        {
                            var form29Window = new Form_29(new Form_29VM(selectedReports));
                            await form29Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.10":
                        {
                            var form210Window = new Form_210(new Form_210VM(selectedReports));
                            await form210Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.11":
                        {
                            var form211Window = new Form_211(new Form_211VM(selectedReports));
                            await form211Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "2.12":
                        {
                            var form212Window = new Form_212(new Form_212VM(selectedReports));
                            await form212Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "4.1":
                        {
                            var form41Window = new Form_41(new Form_41VM(selectedReports));
                            await form41Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "5.1":
                        {
                            var form51Window = new Form_51(new Form_51VM(selectedReports));
                            await form51Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "5.2":
                        {
                            var form52Window = new Form_52(new Form_52VM(selectedReports));
                            await form52Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "5.3":
                        {
                            var form53Window = new Form_53(new Form_53VM(selectedReports));
                            await form53Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "5.4":
                        {
                            var form54Window = new Form_54(new Form_54VM(selectedReports));
                            await form54Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "5.5":
                        {
                            var form55Window = new Form_55(new Form_55VM(selectedReports));
                            await form55Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "5.6":
                        {
                            var form56Window = new Form_56(new Form_56VM(selectedReports));
                            await form56Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                    case "5.7":
                        {
                            var form57Window = new Form_57(new Form_57VM(selectedReports));
                            await form57Window.ShowDialog(mainWindow);
                            await selectedReports.Report_Collection.QuickSortAsync();
                            break;
                        }
                }
                mainWindowVM.UpdateReportCollection();
                mainWindowVM.UpdateFormsPageInfo();
                mainWindowVM.UpdateTotalReportCount();

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