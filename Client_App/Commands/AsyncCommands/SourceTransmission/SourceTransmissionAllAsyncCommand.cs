using MsBox.Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.Services.DataAccess;
using Client_App.ViewModels.Forms;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MsBox.Avalonia.Enums;
namespace Client_App.Commands.AsyncCommands.SourceTransmission;

// Перевод всех источников в форме из РВ в РАО
public class SourceTransmissionAllAsyncCommand : SourceTransmissionBaseAsyncCommand
{
    public SourceTransmissionAllAsyncCommand(BaseFormVM formVM)
    {
        FormVM = formVM;
    }

    public override async Task AsyncExecute(object? parameter)
    {
        var formWindow = Desktop.Windows.FirstOrDefault(x => x.Name == FormVM.FormType);
        var desktop = (IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current?.ApplicationLifetime!;
        var activeWindow = formWindow ?? desktop.MainWindow;

        var formsWithCode41 = await LoadCode41FormsAsync();
        var linesWithCorruptOpDate = formsWithCode41
            .Where(x => !DateOnly.TryParse(x.OperationDate_DB, out _))
            .Select(x => x.NumberInOrder_DB)
            .ToArray();
        if (linesWithCorruptOpDate.Length > 0)
        {
            #region MessageSourceTransmissionFailed

            var suffix1 = linesWithCorruptOpDate.Length == 1 ? "чке" : "ках";
            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Перевод источников",
                    ContentHeader = "Ошибка",
                    ContentMessage = $"Некорректно введена дата операции в стро{suffix1} {string.Join(", ", linesWithCorruptOpDate)}.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(activeWindow));

            #endregion
             
            return;
        }
        if (formsWithCode41.Count == 0)
        {
            #region MessageSourceTransmissionFailed

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Перевод источников",
                    ContentHeader = "Ошибка",
                    ContentMessage = "В данной форме отсутствуют записи с кодом операции 41.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(activeWindow));

            #endregion

            return;
        }

        try
        {
            if (StaticConfiguration.DBModel.ChangeTracker.HasChanges())
            {
                #region MessageSaveChanges

                var res = await Dispatcher.UIThread.InvokeAsync(async () => await MessageBoxManager
                    .GetMessageBoxCustom(new MessageBoxCustomParams
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
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).ShowWindowDialogAsync(activeWindow));

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

        foreach (var f in formsWithCode41)
        {
            var opDate = DateOnly.Parse(f.OperationDate_DB);
            var repInRange = FindTargetReportsInRange(f.FormNum_DB, opDate);
            if (repInRange.Count > 1)
            {
                #region MessageSourceTransmissionFailed

                var formNumInMessage = f.FormNum_DB is "1.1" 
                    ? "1.5" 
                    : "1.6";

                await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                    .GetMessageBoxStandard(new MessageBoxStandardParams
                    {
                        ButtonDefinitions = ButtonEnum.Ok,
                        ContentTitle = "Перевод источника в РАО",
                        ContentHeader = "Ошибка",
                        ContentMessage =
                            $"У выбранной организации присутствуют отчёты по форме {formNumInMessage} с пересекающимися периодами. " +
                            $"{Environment.NewLine}Устраните данное несоответствие перед операцией перевода источника в РАО.",
                        MinWidth = 400,
                        MinHeight = 150,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Topmost = true,
                    }).ShowWindowDialogAsync(activeWindow));

                #endregion

                return;
            }
        }
        Report repToOpen = new(){ StartPeriod_DB = "01.01.0001" };
        var countAddedForm = 0;
        foreach (var form in formsWithCode41)
        {
            var opDate = DateOnly.Parse(form.OperationDate_DB);
            // Report_Collection у org из грида часто пуст (AsNoTracking stub) — ищем периоды в БД.
            var shells = OrgReportsQuery.LoadReportShells(
                StaticConfiguration.DBModel, SelectedReports.Id);
            var repInRange = shells
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
                    rep.Reports ??= SelectedReports;
                    var formIsAdded = await AddNewFormToExistingReport(rep, form, db);
                    if (formIsAdded) countAddedForm++;
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
                    var rep = await CreateReportAndAddNewForm(db, form, opDate);
                    countAddedForm++;
                    await db.SaveChangesAsync();
                    var report = await ReportsStorage.GetReportAsync(rep.Id);
                    report.Reports ??= SelectedReports;
                    Forms1WarmCache.Instance.InvalidateOrg(SelectedReports.Id);
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

        if (countAddedForm != formsWithCode41.Count)
        {
            #region MessageSourceTransmissionFailed

            var repFormNum = SelectedReport.FormNum_DB switch
            {
                "1.1" => "1.5",
                _ => "1.6"
            };

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Перевод источника в РАО",
                    ContentHeader = "Уведомление",
                    ContentMessage = $"В связи с наличием строчек дубликатов, " +
                                     $"{Environment.NewLine}было переведено {countAddedForm} строчек форм из {formsWithCode41.Count}. " +
                                     $"{Environment.NewLine}Проверьте правильность заполнения форм {SelectedReport.FormNum_DB} и {repFormNum}",
                    CanResize = true,
                    MinWidth = 450,
                    MinHeight = 175,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(activeWindow));

            #endregion
        }

        if (countAddedForm > 0)
        {
            repToOpen.Reports ??= SelectedReports;
            await CloseWindowAndOpenNew(repToOpen).ConfigureAwait(false);
        }
    }

    private async Task<List<Form1>> LoadCode41FormsAsync()
    {
        var db = StaticConfiguration.DBModel;
        var reportId = SelectedReport.Id;
        var formNum = SelectedReport.FormNum_DB;
        FormRowMutationService.CollectPending(db, reportId, formNum,
            out var added, out var tracked, out var exclude);

        List<(int Id, string Code)> codes;
        await using (var snap = new DBModel(StaticConfiguration.DBPath))
        {
            codes = await FormRowsPageLoader.LoadIdAndOperationCodesAsync(snap, reportId, formNum)
                .ConfigureAwait(false);
        }

        var matchIds = new List<int>();
        foreach (var (id, code) in codes)
        {
            if (exclude.Contains(id))
                continue;
            if (string.Equals(code.Trim(), "41", StringComparison.Ordinal))
                matchIds.Add(id);
        }

        var idsToLoad = matchIds.Where(id => !tracked.ContainsKey(id)).ToList();
        var loaded = new Dictionary<int, Form>();
        if (idsToLoad.Count > 0)
        {
            await using var snap = new DBModel(StaticConfiguration.DBPath);
            var forms = await FormRowsPageLoader.LoadByIdsAsync(snap, formNum, idsToLoad)
                .ConfigureAwait(false);
            foreach (var form in forms)
                loaded[form.Id] = form;
        }

        var result = new List<Form1>();
        foreach (var id in matchIds)
        {
            Form? form = null;
            if (tracked.TryGetValue(id, out var keep))
                form = keep;
            else if (loaded.TryGetValue(id, out var fromDb))
                form = fromDb;
            if (form is Form1 f1)
                result.Add(f1);
        }

        foreach (var row in added)
        {
            if (row is Form1 f1 &&
                string.Equals(f1.OperationCode_DB?.Trim(), "41", StringComparison.Ordinal))
                result.Add(f1);
        }

        return result;
    }
}