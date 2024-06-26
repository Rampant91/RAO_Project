﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using Client_App.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.RaodbExport;

// Экспорт организации в файл .raodb с указанием диапазона дат выгружаемых форм
internal class ExportReportsWithDateRangeAsyncCommand(MainWindowVM mainWindowViewModel) : BaseAsyncCommand
{
    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not ObservableCollectionWithItemPropertyChanged<IKey> param) return;

        #region MessageAskStartDate
        var startDate = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отменить экспорт", IsCancel = true }
                },
                ContentTitle = "Выгрузка",
                ShowInCenter = true,
                ContentMessage =
                    "Введите дату начала периода. Если оставить поле пустым," +
                    $"{Environment.NewLine}то при выгрузке форм организации не будет ограничения по дате начала периода.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow);
        #endregion

        if (startDate.Button is null or "Отменить экспорт") return;

        #region MessageAskEndDate
        var endDate = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxInputWindow(new MessageBoxInputParams
            {
                ButtonDefinitions = new[]
                {
                    new ButtonDefinition { Name = "Ок", IsDefault = true },
                    new ButtonDefinition { Name = "Отменить экспорт", IsCancel = true }
                },
                ContentTitle = "Выгрузка",
                ContentMessage =
                    "Введите дату конца периода. Если оставить поле пустым," +
                    $"{Environment.NewLine}то при выгрузке форм организации не будет ограничения по дате конца периода.",
                MinWidth = 600,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(Desktop.MainWindow);
        #endregion

        if (endDate.Button is null or "Отменить экспорт") return;

        var canParseDateRange =
            (DateTime.TryParse(startDate.Message, out var startDateTime) | string.IsNullOrEmpty(startDate.Message))
            & (DateTime.TryParse(endDate.Message, out var endDateTime) | string.IsNullOrEmpty(endDate.Message));
        if (endDateTime == DateTime.MinValue) endDateTime = DateTime.MaxValue;

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
            .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out var repStartDateTime)
                          && DateTime.TryParse(rep.EndPeriod_DB, out var repEndDateTime)
                          && startDateTime <= repEndDateTime && endDateTime >= repStartDateTime)
            .ToArray();
        //List<Report> repInRangeWithForms = [];
        //foreach (var rep in repInRange)
        //{
        //    repInRangeWithForms.Add(await ReportsStorage.Api.GetAsync(rep.Id));
        //}
        Reports exportOrg = new() { Master = org.Master };
        exportOrg.Report_Collection.AddRangeNoChange(repInRange);

        if (mainWindowViewModel.ExportReports.CanExecute(null))
            mainWindowViewModel.ExportReports.Execute(exportOrg);
    }
}