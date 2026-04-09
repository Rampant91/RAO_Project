using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Resources;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Commands.AsyncCommands.Save;

namespace Client_App.Commands.AsyncCommands.SourceTransmission;

// Перевод источника из РВ в РАО
public class NewSourceTransmissionAsyncCommand : NewSourceTransmissionBaseAsyncCommand
{
    #region Constructor

    public NewSourceTransmissionAsyncCommand(BaseFormVM formVM)
    {
        FormVM = formVM;
    }

    #endregion

    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is null) return;

        var form = (Form1)parameter;

        var formWindow = Desktop.Windows.FirstOrDefault(x => x.Name == form.FormNum_DB);
        var desktop = (IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current?.ApplicationLifetime!;
        var activeWindow = formWindow ?? desktop.MainWindow;

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
                .ShowDialog(activeWindow));

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
                .ShowDialog(activeWindow));

            #endregion

            return;
        }
        try
        {
            if (StaticConfiguration.DBModel.ChangeTracker.HasChanges())
            {
                #region MessageSaveChanges

                var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        ButtonDefinitions =
                        [
                            new ButtonDefinition { Name = "Да" },
                            new ButtonDefinition { Name = "Отмена" }
                        ],
                        ContentTitle = "Сохранение изменений",
                        ContentHeader = "Уведомление",
                        ContentMessage = $"Обнаружены изменения." +
                                         $"{Environment.NewLine}Сохранить форму {FormVM.FormType} перед переводом РВ в РАО?",
                        MinWidth = 400,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(activeWindow));

                #endregion

                var dbm = StaticConfiguration.DBModel;
                switch (res)
                {
                    case "Да":
                    {
                        await dbm.SaveChangesAsync();
                        await new SaveReportAsyncCommand(FormVM).AsyncExecute(null);

                        break;
                    }
                    case null or "Отмена":
                    default:
                    {
                        return;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
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
        var repFormNum = form.FormNum_DB switch
        {
            "1.1" => "1.5",
            _ => "1.6"
        };
        switch (repInRange.Count)
        {
            case > 1:   // У организации по ошибке есть несколько отчётов с нужным периодом
            {
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
                    .ShowDialog(activeWindow));

                #endregion

                return;
            }
            case 1:   // Если есть подходящий отчет, то добавляем форму в него
            {
                var rep = repInRange.First();
                var formIsAdded = await AddNewFormToExistingReport(rep, form, db);

                if (!formIsAdded)
                {
                    #region MessageSourceTransmissionFailed

                    await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                        {
                            ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                            ContentTitle = "Перевод источника в РАО",
                            ContentHeader = "Уведомление",
                            ContentMessage = $"Строчка не была переведена в РАО, в связи с тем, что в форме {repFormNum}" +
                                             $"{Environment.NewLine}уже присутствует данная строчка с кодом операции 41." +
                                             $"{Environment.NewLine}Проверьте правильность заполнения форм {form.FormNum_DB} и {repFormNum}.",
                            CanResize = true,
                            MinWidth = 450,
                            MinHeight = 175,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        })
                        .ShowDialog(activeWindow));

                    return;

                    #endregion
                }

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
                        .ShowDialog(activeWindow));

                    #endregion

                    if (res is "Да") report.CorrectionNumber_DB++;
                    
                }

                await db.SaveChangesAsync();

                await CloseWindowAndOpenNew(report);

                break;
            }
            default:    // Если отчета с подходящим периодом нет, создаём новый отчёт и добавляем в него форму 
            {
                var rep = await CreateReportAndAddNewForm(db, form, opDate);
                await db.SaveChangesAsync();
                var report = await ReportsStorage.Api.GetAsync(rep.Id);
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
        var vm = (BaseFormVM)window.DataContext;
        vm.SkipChangeTacking = true;
        var windowParam = new FormParameter
        {
            Parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { rep } ),
            Window = window
        };
        await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
    }

    #endregion
}