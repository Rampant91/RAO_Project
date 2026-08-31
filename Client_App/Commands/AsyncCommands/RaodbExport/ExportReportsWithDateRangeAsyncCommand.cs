using MsBox.Avalonia;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Threading;
using Client_App.Services.DataAccess;
using Client_App.ViewModels;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;

using Client_App.Resources;
using MsBox.Avalonia.Enums;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

/// <summary>
/// Экспорт организации в файл .RAODB с указанием диапазона дат выгружаемых форм
/// </summary>
public class ExportReportsWithDateRangeAsyncCommand : ExportRaodbBaseAsyncCommand
{
    private readonly MainWindowVM _mainWindowVM;

    public ExportReportsWithDateRangeAsyncCommand(MainWindowVM mainWindowVM)
    {
        _mainWindowVM = mainWindowVM;

        // Подписываемся на изменение SelectedReports для обновления CanExecute
        mainWindowVM.PropertyChanged += (sender, e) =>
        {
            if (e.PropertyName == nameof(MainWindowVM.SelectedReports))
            {
                OnCanExecuteChanged();
            }
        };
    }

    public override bool CanExecute(object? parameter) => _mainWindowVM.SelectedReports is not null;

    public override async Task AsyncExecute(object? parameter)
    {
        Reports reports;
        switch (parameter)
        {
            case ObservableCollectionWithItemPropertyChanged<IKey> param:
                {
                    reports = (Reports)param.First();
                    break;
                }
            case Reports reps:
                {
                    reports = reps;
                    break;
                }
            default: return;
        }

        #region MessageAskStartDate

        var startDate = await Dispatcher.UIThread.InvokeAsync(async () =>
            await Avalonia11Compat.ShowInputDialogAsync(new MessageBoxCustomParams
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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
                InputParams = new InputParams()
            }, Desktop.MainWindow));
        
        #endregion

        if (startDate.Button is null or "Отменить экспорт") return;

        #region MessageAskEndDate

        var endDate = await Dispatcher.UIThread.InvokeAsync(async () =>
            await Avalonia11Compat.ShowInputDialogAsync(new MessageBoxCustomParams
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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
                InputParams = new InputParams()
            }, Desktop.MainWindow));
        
        #endregion

        if (endDate.Button is null or "Отменить экспорт") return;

        var canParseDateRange =
            (DateOnly.TryParse(startDate.Message, out var startDateTime) | string.IsNullOrEmpty(startDate.Message))
            & (DateOnly.TryParse(endDate.Message, out var endDateTime) | string.IsNullOrEmpty(endDate.Message));
        if (endDateTime == DateOnly.MinValue) endDateTime = DateOnly.MaxValue;

        if (!canParseDateRange || startDateTime > endDateTime)
        {
            #region MessageErrorAtParseDate

            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Экспорт не будет выполнен, поскольку период дат введён некорректно.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));

            #endregion

            return;
        }

        var orgFromDb = await OrgReportsQuery.LoadOrgWithReportShellsAsync(
            StaticConfiguration.DBModel, reports.Id, CancellationToken.None);
        if (orgFromDb is null)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBoxManager
                .GetMessageBoxStandard(new MessageBoxStandardParams
                {
                    ButtonDefinitions = ButtonEnum.Ok,
                    ContentTitle = "Выгрузка",
                    ContentHeader = "Уведомление",
                    ContentMessage = "Экспорт не будет выполнен: организация не найдена в базе.",
                    MinWidth = 400,
                    MinHeight = 150,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Topmost = true,
                }).ShowWindowDialogAsync(Desktop.MainWindow));
            return;
        }

        var repInRange = orgFromDb.Report_Collection
            .Where(rep => (DateOnly.TryParse(rep.StartPeriod_DB, out var repStartDateTime)
                          && DateOnly.TryParse(rep.EndPeriod_DB, out var repEndDateTime)
                          && startDateTime <= repEndDateTime && endDateTime >= repStartDateTime)
                          ||
                          (rep.Year_DB is { } year
                          && startDateTime.Year <= year && year <= endDateTime.Year))
            .ToArray();

        Reports exportOrg = new() { Master = orgFromDb.Master, Id = orgFromDb.Id };
        exportOrg.Report_Collection.AddRangeNoChange(repInRange);

        ICommand ExportReports = new ExportReportsAsyncCommand(_mainWindowVM);
        if (ExportReports.CanExecute(null))
        {
            ExportReports.Execute(exportOrg);
        }
    
    }
}