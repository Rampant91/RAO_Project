using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Interfaces;

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

        var startDate = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
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
            .ShowDialog(Desktop.MainWindow));
        
        #endregion

        if (startDate.Button is null or "Отменить экспорт") return;

        #region MessageAskEndDate

        var endDate = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
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
            .ShowDialog(Desktop.MainWindow));
        
        #endregion

        if (endDate.Button is null or "Отменить экспорт") return;

        var canParseDateRange =
            (DateOnly.TryParse(startDate.Message, out var startDateTime) | string.IsNullOrEmpty(startDate.Message))
            & (DateOnly.TryParse(endDate.Message, out var endDateTime) | string.IsNullOrEmpty(endDate.Message));
        if (endDateTime == DateOnly.MinValue) endDateTime = DateOnly.MaxValue;

        if (!canParseDateRange || startDateTime > endDateTime)
        {
            #region MessageErrorAtParseDate

            await MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(new MessageBoxStandardParams
                {
                    ButtonDefinitions = MessageBox.Avalonia.Enums.ButtonEnum.Ok,
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Экспорт не будет выполнен, поскольку период дат введён некорректно.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                }).ShowDialog(Desktop.MainWindow);

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