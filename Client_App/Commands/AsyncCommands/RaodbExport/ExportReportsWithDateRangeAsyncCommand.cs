using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using Models.Collections;
using Models.Interfaces;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Экспорт организации в файл .RAODB с указанием диапазона дат выгружаемых форм
/// </summary>
public class ExportReportsWithDateRangeAsyncCommand : ExportRaodbBaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> param) return;

        #region MessageAskStartDate

        var startDate = await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отменить экспорт", IsCancel = true }
                ],
                ContentTitle = "Выгрузка",
                ShowInCenter = true,
                ContentMessage =
                    "Введите дату начала периода. Если оставить поле пустым," +
                    $"{Environment.NewLine}то при выгрузке форм организации не будет ограничения по дате начала периода.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowWindowDialogAsync(Desktop.MainWindow));
        
        #endregion

        if (startDate is "Отменить экспорт") return;

        #region MessageAskEndDate

        var endDate = await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustom(new MessageBoxCustomParams()
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отменить экспорт", IsCancel = true }
                ],
                ContentTitle = "Выгрузка",
                ContentMessage =
                    "Введите дату конца периода. Если оставить поле пустым," +
                    $"{Environment.NewLine}то при выгрузке форм организации не будет ограничения по дате конца периода.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowWindowDialogAsync(Desktop.MainWindow));
        
        #endregion

        if (endDate is "Отменить экспорт") return;

        var canParseDateRange =
            (DateOnly.TryParse(startDate, out var startDateTime) | string.IsNullOrEmpty(startDate))
            & (DateOnly.TryParse(endDate, out var endDateTime) | string.IsNullOrEmpty(endDate));
        if (endDateTime == DateOnly.MinValue) endDateTime = DateOnly.MaxValue;

        if (!canParseDateRange || startDateTime > endDateTime)
        {
            #region MessageErrorAtParseDate

            await Dispatcher.UIThread.InvokeAsync(() => MsBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MsBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Экспорт не будет выполнен, поскольку период дат введён некорректно.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion

            return;
        }

        var org = (Reports)param.First();
        var repInRange = org.Report_Collection
            .Where(rep => DateOnly.TryParse(rep.StartPeriod_DB, out var repStartDateTime)
                          && DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDateTime)
                          && startDateTime <= repEndDateTime && endDateTime >= repStartDateTime)
            .ToArray();

        Reports exportOrg = new() { Master = org.Master, Id = org.Id };
        exportOrg.Report_Collection.AddRangeNoChange(repInRange);

        if (MainWindowVM.ExportReports.CanExecute(null))
        {
            MainWindowVM.ExportReports.Execute(exportOrg);
        }
    }
}