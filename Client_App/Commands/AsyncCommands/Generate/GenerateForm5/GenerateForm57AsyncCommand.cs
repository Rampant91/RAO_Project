using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using AvaloniaEdit.Utils;
using Client_App.Commands.AsyncCommands.CheckForm;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.BackgroundLoader;
using Client_App.Interfaces.Logger;
using Client_App.Properties;
using Client_App.Properties.UnifiedConfig;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms;
using Models.Forms.Form2;
using Models.Forms.Form4;
using Models.Forms.Form5;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Generate.GenerateForm5;

public class GenerateForm57AsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
{
    #region private Properties
    private Report Report => formVM.Report;

    private Window owner;

    private int year = 0;
    #endregion
    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        try
        {
            await Task.Run(() => AsyncExecute(parameter));
        }
        catch (OperationCanceledException) { }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
        IsExecute = false;
    }

    public override async Task AsyncExecute(object? parameter)
    {

        owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
            .FirstOrDefault(w => w.Name == "5.7");

        if (owner == null) return;

        var cts = new CancellationTokenSource();



        List<ViacOrganization> loadedList = null;

        #region ShowAskMessages
        if (Report.Rows57.Count != 0)
        {
            if (!await ShowConfirmationMessage(owner)) return;
        }

        Report.Rows57.Clear();

        if (await ShowAskDependOnReportOrNotMessage(owner))
        {
            var report = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskReportMessage(owner));

            if (report != null)
                CopyRowsFromReport(report);
        }

        if (Settings.Default.AppLaunchedInNorao)
        {
            loadedList = await ViacManager.GetAllOrganizationsFromVIAC(owner);
        }

        #endregion


        var progressBar = await Dispatcher.UIThread.InvokeAsync(async () => new AnyTaskProgressBar(cts, owner));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;


        progressBarVM.SetProgressBar(5, $"Загрузка организаций");


        var organizations10 = await GetOrganizations10List(StaticConfiguration.DBModel, loadedList);
        var organizations20 = await GetOrganizations20List(StaticConfiguration.DBModel, loadedList);

        foreach(var organization10 in organizations10)
        {
            if (!Report.Rows57.Any(form57 =>
            form57.RegNo_DB == organization10.Master.RegNoRep.Value
            && form57.OKPO_DB == organization10.Master.OkpoRep.Value))
                Report.Rows57.Add(new Form57()
                {
                    RegNo_DB = organization10.Master.RegNoRep.Value,
                    OKPO_DB = organization10.Master.OkpoRep.Value,
                    Name_DB = !string.IsNullOrEmpty(organization10.Master_DB.Rows10[0].ShortJurLico_DB)
                    ? organization10.Master_DB.Rows10[0].ShortJurLico_DB
                    : organization10.Master_DB.Rows10[0].JurLico_DB
                });
        }
        foreach (var organization20 in organizations20)
        {
            if (!Report.Rows57.Any(form57 =>
            form57.RegNo_DB == organization20.Master.RegNoRep.Value
            && form57.OKPO_DB == organization20.Master.OkpoRep.Value))
                Report.Rows57.Add(new Form57()
                {
                    RegNo_DB = organization20.Master.RegNoRep.Value,
                    OKPO_DB = organization20.Master.OkpoRep.Value,
                    Name_DB = !string.IsNullOrEmpty(organization20.Master_DB.Rows20[0].ShortJurLico_DB)
                    ?organization20.Master_DB.Rows20[0].ShortJurLico_DB 
                    :organization20.Master_DB.Rows20[0].JurLico_DB
                });
        }

        await StaticConfiguration.DBModel.SaveChangesAsync();

        //Выставляем номера строк
        for (int i = 0; i < Report.Rows57.Count; i++)
        {
            Report.Rows57[i].SetOrder(i + 1);
        }

        progressBarVM.SetProgressBar(
            100,
            $"Завершаем формирование отчета");

        //Обновляем таблицу
        formVM.UpdateFormList(); 
        formVM.UpdatePageInfo();
        await Dispatcher.UIThread.InvokeAsync(async () => progressBar.Close());
    }

    #region AskMessages
    private async Task<bool> ShowConfirmationMessage(Window owner)
    {
        string answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" },
                ],
                CanResize = true,
                ContentTitle = "Формирование нового отчета",
                ContentMessage = "Все строки будут перезаписаны!\n" +
                "Вы уверены, что хотите продолжить?",
                MinWidth = 300,
                MinHeight = 125,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(owner));

        if (answer == "Да")
            return true;
        else
            return false;
    }
    #endregion

    #region private Functions
    private async Task<bool> ShowAskDependOnReportOrNotMessage(Window owner)
    {
        string answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
            {
                ButtonDefinitions =
                [
                    new ButtonDefinition { Name = "Да" },
                    new ButtonDefinition { Name = "Нет" },
                ],
                CanResize = true,
                ContentTitle = "Формирование нового отчета",
                ContentMessage = "Вы хотите сформировать отчет на основе другого отчета?",
                MinWidth = 300,
                MinHeight = 125,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(owner));

        if (answer == "Да")
            return true;
        else
            return false;
    }
    private void CopyRowsFromReport(Report report)
    {
        var copiedRows = report.Rows57.Select(original => new Form57()
        {
            NumberInOrder_DB = original.NumberInOrder_DB,
            RegNo_DB = original.RegNo_DB,
            OKPO_DB = original.OKPO_DB,
            Name_DB = original.Name_DB,
            Recognizance_DB = original.Recognizance_DB,
            License_DB = original.License_DB,
            Practice_DB = original.Practice_DB,
            Note_DB = original.Note_DB,
            Report = formVM.Report,
        });
        
        Report.Rows57.AddRange(copiedRows);
    }
    private async Task<Report> ShowAskReportMessage(Window owner)
    {
        var dialog = new AskForm57Message(Report);

        Report? report = await dialog.ShowDialog<Report?>(owner);
        return report;
    }
    
    #endregion

    #region Requests
    private async Task<List<Reports>> GetOrganizations10List(DBModel DB, List<ViacOrganization> loadedList = null)
    {
        try
        {
            if (loadedList == null || loadedList.Count == 0)
                return  DB.ReportsCollectionDbSet
                            .AsNoTracking()
                            .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows10)
                            .AsEnumerable()
                            .Where(reports => reports.Master_DB.FormNum_DB == "1.0")
                            .ToList();
            else
                return DB.ReportsCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows10)
                            .AsEnumerable()
                            .Where(reports => reports.Master_DB.FormNum_DB == "1.0")
                            .Where(reports => loadedList.Any(x =>
                                x.RegNo == reports.Master_DB.Rows10[0].RegNo_DB
                                && x.OKPO == reports.Master_DB.Rows10[1].Okpo_DB
                                ||
                                (string.IsNullOrEmpty(reports.Master_DB.Rows10[1].Okpo_DB) 
                                && x.OKPO == reports.Master_DB.Rows10[0].Okpo_DB)))
                            .ToList();
        }
        catch (Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
               .GetMessageBoxCustomWindow(new MessageBoxCustomParams
               {
                   ButtonDefinitions =
                   [
                       new ButtonDefinition { Name = "Ок" },
                   ],
                   CanResize = true,
                   ContentTitle = "Формирование нового отчета",
                   ContentMessage = "Произошла ошибка:\n" +
                   "Не удалось открыть базу данных",
                   MinWidth = 300,
                   MinHeight = 125,
                   WindowStartupLocation = WindowStartupLocation.CenterOwner
               })
               .ShowDialog(owner));
            return new List<Reports>();
        }
    }
    private async Task<List<Reports>> GetOrganizations20List(DBModel DB, List<ViacOrganization> loadedList = null)
    {
        try
        {
            if (loadedList == null || loadedList.Count == 0)
                return DB.ReportsCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows20)
                            .AsEnumerable()
                            .Where(reports => reports.Master_DB.FormNum_DB == "2.0")
                            .ToList();
            else
                return DB.ReportsCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows20)
                            .AsEnumerable()
                            .Where(reports => reports.Master_DB.FormNum_DB == "2.0")
                            .Where(reports => loadedList.Any(x =>
                                x.RegNo == reports.Master_DB.Rows20[0].RegNo_DB
                                && x.OKPO == reports.Master_DB.Rows20[1].Okpo_DB
                                ||
                                (string.IsNullOrEmpty(reports.Master_DB.Rows20[1].Okpo_DB)
                                && x.OKPO == reports.Master_DB.Rows20[0].Okpo_DB)))
                            .ToList();
        }
        catch (Exception ex)
        {
            await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
               .GetMessageBoxCustomWindow(new MessageBoxCustomParams
               {
                   ButtonDefinitions =
                   [
                       new ButtonDefinition { Name = "Ок" },
                   ],
                   CanResize = true,
                   ContentTitle = "Формирование нового отчета",
                   ContentMessage = "Произошла ошибка:\n" +
                   "Не удалось открыть базу данных",
                   MinWidth = 300,
                   MinHeight = 125,
                   WindowStartupLocation = WindowStartupLocation.CenterOwner
               })
               .ShowDialog(owner));
            return new List<Reports>();
        }
    }
    #endregion
}
