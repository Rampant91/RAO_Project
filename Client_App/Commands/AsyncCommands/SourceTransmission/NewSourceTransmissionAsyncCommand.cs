using Client_App.ViewModels;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using MessageBox.Avalonia.DTO;
using Models.Classes;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Interfaces;
using MessageBox.Avalonia.Models;
using Client_App.ViewModels.Forms.Forms1;

namespace Client_App.Commands.AsyncCommands.SourceTransmission;

// Перевод источника из РВ в РАО
public class NewSourceTransmissionAsyncCommand : NewSourceTransmissionBaseAsyncCommand
{
    #region Constructor

    public NewSourceTransmissionAsyncCommand(Form_12VM formVM)
    {
        FormVM = formVM;
    }

    #endregion

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;
        var form = ((IKeyCollection)parameter).Get<Form1>(0);
        if (!string.Equals(form.OperationCode_DB.Trim(), "41", StringComparison.Ordinal))
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Передача источника",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Перевод источника в РАО осуществляется кодом операции 41",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }
        if (!DateOnly.TryParse(form.OperationDate_DB, out var opDate))
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Передача источника",
                    ContentHeader = "Ошибка",
                    ContentMessage = "Некорректно введена дата операции",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        var repInRange = SelectedReports.Report_Collection
            .Where(rep => (form.FormNum_DB == "1.1" && rep.FormNum_DB == "1.5"
                           || form.FormNum_DB == "1.2" && rep.FormNum_DB == "1.6"
                           || form.FormNum_DB == "1.3" && rep.FormNum_DB == "1.6"
                           || form.FormNum_DB == "1.4" && rep.FormNum_DB == "1.6")
                          && (DateOnly.TryParse(rep.StartPeriod_DB, out var repStartDate) 
                              && DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDate)
                              && opDate > repStartDate && opDate <= repEndDate
                              || DateOnly.TryParse(rep.StartPeriod_DB, out repStartDate) 
                              && !DateOnly.TryParse(rep.EndPeriod_DB, out _)
                              && opDate > repStartDate))
            .OrderBy(x => x.EndPeriod_DB)
            .ToList();
        if (repInRange.Count == 2
            && !DateOnly.TryParse(repInRange[0].EndPeriod_DB, out _)
            && DateOnly.TryParse(repInRange[1].EndPeriod_DB, out _))
        {
            repInRange.Remove(repInRange[0]);
        }

        await using var db = new DBModel(StaticConfiguration.DBPath);
        var formIsAdded = false;
        switch (repInRange.Count)
        {
            case > 1:   // У организации по ошибке есть несколько отчётов с нужным периодом
            {
                var repFormNum = form.FormNum_DB switch
                {
                    "1.1" => "1.5",
                    _ => "1.6"
                };

                #region MessageSourceTransmissionFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Перевод источника в РАО",
                        ContentHeader = "Ошибка",
                        ContentMessage = $"У выбранной организации присутствуют отчёты по форме {repFormNum} с пересекающимися периодами. " +
                                         $"{Environment.NewLine}Устраните данное несоответствие перед операцией перевода РВ в РАО.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                return;
            }
            case 1:   // Если есть подходящий отчет, то добавляем форму в него
            {
                var rep = repInRange.First();
                formIsAdded = await AddNewFormToExistingReport(rep, form, db);
                var report = await ReportsStorage.GetReportAsync(rep.Id);
                if (report.ExportDate_DB != "")
                {
                    var appropriateFormNum = form.FormNum_DB is "1.1"
                        ? "1.5"
                        : "1.6";

                    #region ChangeCorrectionNumber

                    var res = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                        {
                            ButtonDefinitions =
                            [
                                new ButtonDefinition { Name = "Да" },
                            new ButtonDefinition { Name = "Нет" }
                            ],
                            ContentTitle = "Перевод источника в РАО",
                            ContentHeader = "Уведомление",
                            ContentMessage = $"Изменить номер корректировки в форме {appropriateFormNum} " +
                                             $"{Environment.NewLine}с соответствующим периодом?",
                            MinWidth = 400,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(Desktop.MainWindow));

                    #endregion

                    if (res is "Да") report.CorrectionNumber_DB++;
                }
                await db.SaveChangesAsync();
                await CloseWindowAndOpenNew(report);

                #region MessageSourceTransmissionFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Перевод источника в РАО",
                        ContentHeader = "Уведомление",
                        ContentMessage = $"Строчка не была переведена в РАО, в связи с наличием строчек дубликатов. " +
                                         $"{Environment.NewLine}Проверьте правильность заполнения формы {SelectedReport.FormNum_DB}",
                        CanResize = true,
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                break;
            }
            default:    // Если отчета с подходящим периодом нет, создаём новый отчёт и добавляем в него форму 
            {
                var repId = await CreateReportAndAddNewForm(db, form, opDate);
                formIsAdded = true;
                await db.SaveChangesAsync();
                var report = await ReportsStorage.Api.GetAsync(repId);
                SelectedReports.Report_Collection.Add(report);
                await CloseWindowAndOpenNew(report);
                break;
            }
        }
    }

    #region CloseWindowAndOpenNew

    private static async Task CloseWindowAndOpenNew(Report rep)
    {
        var window = Desktop.Windows.First(x => x.Name is "1.1" or "1.2" or "1.3" or "1.4");
        var windowParam = new FormParameter()
        {
            Parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { rep }),
            Window = window
        };
        await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
    }

    #endregion
}