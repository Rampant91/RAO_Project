using Avalonia.Threading;
using Client_App.ViewModels;
using Models.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using Models.Forms.Form1;
using Models.DBRealization;
using Models.Classes;
using System.Collections.Generic;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.SourceTransmission;

// Перевод всех источников в форме из РВ в РАО
public class SourceTransmissionAllAsyncCommand : SourceTransmissionBaseAsyncCommand
{
    public SourceTransmissionAllAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel)
    {
        ChangeOrCreateViewModel = changeOrCreateViewModel;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        var formsWithCode41 = SelectedReport[SelectedReport.FormNum_DB].ToList<Form1>()
            .Where(x => string.Equals(x.OperationCode_DB.Trim(), "41", StringComparison.Ordinal))
            .ToList();
        var linesWithCorruptOpDate = formsWithCode41
            .Where(x => !DateOnly.TryParse(x.OperationDate_DB, out _))
            .Select(x => x.NumberInOrder_DB)
            .ToArray();
        if (linesWithCorruptOpDate.Length > 0)
        {
            #region MessageSourceTransmissionFailed

            var suffix1 = linesWithCorruptOpDate.Length == 1 ? "чке" : "ках";
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Перевод источников",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"Некорректно введена дата операции в стро{suffix1} {string.Join(", ", linesWithCorruptOpDate)}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion
             
            return;
        }
        if (formsWithCode41.Count == 0)
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Перевод источников",
                    ContentHeader = "Ошибка",
                    ContentMessage = "В данной форме отсутствуют записи с кодом операции 41.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                })
                .ShowDialog(Desktop.MainWindow));

            #endregion

            return;
        }

        foreach (var f in formsWithCode41)
        {
            var opDate = DateOnly.Parse(f.OperationDate_DB);
            var repInRange = SelectedReports.Report_Collection
                .Where(rep => (f.FormNum_DB == "1.1" && rep.FormNum_DB == "1.5"
                               || f.FormNum_DB == "1.2" && rep.FormNum_DB == "1.6"
                               || f.FormNum_DB == "1.3" && rep.FormNum_DB == "1.6"
                               || f.FormNum_DB == "1.4" && rep.FormNum_DB == "1.6")
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
            if (repInRange.Count > 1)
            {
                #region MessageSourceTransmissionFailed

                await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                        ContentTitle = "Перевод источника в РАО",
                        ContentHeader = "Ошибка",
                        ContentMessage =
                            "У выбранной организации присутствуют отчёты по форме 1.5 с пересекающимися периодами. " +
                            $"{Environment.NewLine}Устраните данное несоответствие перед операцией перевода источника в РАО.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(Desktop.MainWindow));

                #endregion

                return;
            }
        }
        Report repToOpen = new(){ StartPeriod_DB = "01.01.0001" };
        foreach (var form in formsWithCode41)
        {
            var opDate = DateOnly.Parse(form.OperationDate_DB);
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
            switch (repInRange.Count)
            {
                case 1:   // Если есть подходящий отчет, то добавляем форму в него
                {
                    var rep = await ReportsStorage.GetReportAsync(repInRange.First().Id);
                    await AddNewFormToExistingReport(rep, form, db);
                    await db.SaveChangesAsync();
                    if (DateOnly.TryParse(rep.StartPeriod_DB, out var date)
                        && DateOnly.TryParse(repToOpen.StartPeriod_DB, out var maxDate)
                        && date > maxDate)
                    {
                        repToOpen = rep;
                    }
                    break;
                }
                default:    // Если отчета с подходящим периодом нет, создаём новый отчёт и добавляем в него форму 
                {
                    var repId = await CreateReportAndAddNewForm(db, form, opDate);
                    await db.SaveChangesAsync();
                    var report = await ReportsStorage.Api.GetAsync(repId);
                    SelectedReports.Report_Collection.Add(report);
                    if (DateOnly.TryParse(report.StartPeriod_DB, out var date)
                        && DateOnly.TryParse(repToOpen.StartPeriod_DB, out var maxDate)
                        && date > maxDate)
                    {
                        repToOpen = report;
                    }
                    break;
                }
            }
        }
        await CloseWindowAndOpenNew(repToOpen).ConfigureAwait(false);
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