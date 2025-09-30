using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Resources;

namespace Client_App.Commands.AsyncCommands.SwitchReport;

//Потом нужно будет сделать универсальную SwitchingSelectedReportCommand
public class SwitchToPreviousReportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    private Report Report => formVM.Report;
    private Reports Reports => formVM.Reports;

    public override async Task AsyncExecute(object? parameter)
    {
        // Проверяем изменения и предлагаем сохранить
        var shouldContinue = await CheckForChangesAndSave();
        if (!shouldContinue) return;

        // Дальше переключаемся на другую форму
        var index = Reports.Report_Collection.IndexOf(Report);
        Report? newReport = null;
            
        for (int i = index + 1; i < Reports.Report_Collection.Count; i++)
        {
            newReport = Reports.Report_Collection[i];
            if (newReport.FormNum.Value == Report.FormNum.Value)
                break;
        }
        if (newReport == null) return;
        var window = Desktop.Windows.First(x => x.Name == formVM.FormType);
        var windowParam = new FormParameter()
        {
            Parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { newReport }),
            Window = window
        };
        await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
    }

    private async Task<bool> CheckForChangesAndSave()
    {
        var desktop = (IClassicDesktopStyleApplicationLifetime)Avalonia.Application.Current.ApplicationLifetime;
        var dbm = StaticConfiguration.DBModel;

        try
        {
            if (!dbm.ChangeTracker.HasChanges())
            {
                return true;
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
            return true;
        }

        // Показываем диалог сохранения изменений
        var res = await MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" }
                ],
                ContentTitle = "Сохранение изменений",
                ContentHeader = "Уведомление",
                ContentMessage = $"Сохранить форму {formVM.FormType}?",
                MinWidth = 400,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(desktop.MainWindow);

        switch (res)
        {
            case "Да":
                await dbm.SaveChangesAsync();
                await new SaveReportAsyncCommand(formVM).AsyncExecute(null);
                return true;

            case "Нет":
                dbm.Restore();
                await dbm.SaveChangesAsync();
                return true;

            default:
                return false; // Отмена переключения
        }
    }
}