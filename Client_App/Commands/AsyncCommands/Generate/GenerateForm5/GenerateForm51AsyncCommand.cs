using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Properties;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Generate.GenerateForm5
{
    public class GenerateForm51AsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        #region private Properties

        private Report Report => formVM.Report;

        private Window owner;

        private string year => formVM.Report.Year_DB;

        

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
                .FirstOrDefault(w => w.Name == "5.1");

            if (owner == null) return;

            var cts = new CancellationTokenSource();



            List<ViacOrganization> loadedList = null;

            #region ShowAskMessages
            if (Report.Rows51.Count != 0)
            {
                if (!await ShowConfirmationMessage(owner)) return;
            }
            if (string.IsNullOrEmpty(year))
            {
                var answer = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskYearMessage(owner));
                Report.Year_DB = answer.ToString();
            }
            Report.Rows51.Clear();

            if (Settings.Default.AppLaunchedInNorao)
            {
                loadedList = await ViacManager.GetAllOrganizationsFromVIAC(owner);
            }

            #endregion

            var progressBar = await Dispatcher.UIThread.InvokeAsync(async () => new AnyTaskProgressBar(cts, owner));
            var progressBarVM = progressBar.AnyTaskProgressBarVM;


            progressBarVM.SetProgressBar(5, $"Загрузка организаций");


            var organizations10 = await GetOrganizations10List(StaticConfiguration.DBModel, loadedList);



            List<Report> repList = new List<Report>();

            progressBarVM.SetProgressBar(15, $"Фильтруем отчеты 1.1 для обработки ({organizations10.Count})");

            foreach (var organization10 in organizations10)
            {
                try
                {
                    repList.AddRange(organization10.Report_Collection.Where(rep =>
                    rep.FormNum_DB == "1.1"
                    && (rep.StartPeriod_DB.EndsWith(year)
                    || rep.EndPeriod_DB.EndsWith(year))));
                }
                catch (Exception ex)
                {
                    Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        CanResize = true,
                        ContentTitle = "Ошибка",
                        ContentMessage = ex.Message,
                        MinWidth = 300,
                        MinHeight = 125,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(owner));
                    throw ex;
                }
            }


            progressBarVM.SetProgressBar(30, $"Фильтруем строки из отчетов 1.1 для обработки ({repList.Count})");

            List<Form11> rows11List = new List<Form11>(); 
            foreach(var rep in repList)
            {
                try
                {
                    var rows11 = rep.Rows11.Where(row11 => CodeOperationFilter.AllOperationCodes.Contains(row11.OperationCode_DB));

                    if (rep.StartPeriod_DB.EndsWith(year)
                    ^ rep.EndPeriod_DB.EndsWith(year))
                    {
                        rows11 = rows11.Where(row11 => row11.OperationDate_DB.EndsWith(year));
                    }

                    rows11List.AddRange(rows11);
                }
                catch( Exception ex )
                {
                    Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        CanResize = true,
                        ContentTitle = "Ошибка",
                        ContentMessage = ex.Message,
                        MinWidth = 300,
                        MinHeight = 125,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(owner));
                    throw ex;
                }
            }


            progressBarVM.SetProgressBar(50, $"Обрабатываем строки из отчетов 1.1 для обработки ({rows11List.Count})");

            foreach (var row11 in rows11List)
            {
                try
                {
                    var row51 = Report.Rows51.FirstOrDefault(r =>
                        r.OperationCode_DB == row11.OperationCode_DB
                        && r.Category_DB == row11.Category_DB
                        && r.Radionuclids_DB == row11.Radionuclids_DB
                        && r.ProviderOrRecieverOKPO_DB == row11.ProviderOrRecieverOKPO_DB);
                    if (row51 != null)
                    {
                        try
                        {
                            row51.Quantity_DB += row11.Quantity_DB;
                            if (double.TryParse(row51.Activity_DB, out var value) && double.TryParse(row11.Activity_DB, out var inc))
                            {
                                var sumActivity = value + inc;
                                row51.Activity_DB = sumActivity.ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                CanResize = true,
                                ContentTitle = "Ошибка",
                                ContentMessage = ex.Message,
                                MinWidth = 300,
                                MinHeight = 125,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(owner));
                            throw ex;
                        }
                    }
                    else
                    {
                        try
                        {


                            Report.Rows51.Add(new Form51()
                            {
                                OperationCode_DB = row11.OperationCode_DB,
                                Category_DB = row11.Category_DB,
                                Radionuclids_DB = row11.Radionuclids_DB,
                                Quantity_DB = row11.Quantity_DB,
                                Activity_DB = row11.Activity_DB,
                                ProviderOrRecieverOKPO_DB = row11.ProviderOrRecieverOKPO_DB
                            });
                        }
                        catch (Exception ex)
                        {
                            Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                            .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                            {
                                CanResize = true,
                                ContentTitle = "Ошибка",
                                ContentMessage = ex.Message,
                                MinWidth = 300,
                                MinHeight = 125,
                                WindowStartupLocation = WindowStartupLocation.CenterOwner
                            })
                            .ShowDialog(owner));
                            throw ex;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxCustomWindow(new MessageBoxCustomParams
                    {
                        CanResize = true,
                        ContentTitle = "Ошибка",
                        ContentMessage = ex.Message,
                                MinWidth = 300,
                        MinHeight = 125,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    })
                    .ShowDialog(owner));
                    throw ex;
                }
            }
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
        private async Task<int> ShowAskYearMessage(Window owner)
        {
            var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите год, за который хотите сформировать отчет"));

            int? year = await dialog.ShowDialog<int>(owner);

            if (year == null)
                return 0;

            return (int)year;
        }
        #endregion

        #region Requests

        private async Task<List<Reports>> GetOrganizations10List(DBModel DB, List<ViacOrganization> loadedList = null)
        {
            try
            {
                if (loadedList == null || loadedList.Count == 0)
                    return await DB.ReportsCollectionDbSet
                        .AsSplitQuery()
                        .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows10)
                        .Include(reports => reports.Report_Collection).ThenInclude(rep => rep.Rows11)
                        .Where(reports => reports.Master_DB.FormNum_DB == "1.0")
                        .ToListAsync();
                else
                    return await DB.ReportsCollectionDbSet
                        .AsSplitQuery()
                        .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows10)
                        .Include(reports => reports.Report_Collection).ThenInclude(rep => rep.Rows11)
                        .Where(reports => loadedList.Any(x =>
                        x.RegNo == reports.Master_DB.Rows10[0].RegNo_DB
                        && x.OKPO == reports.Master_DB.Rows10[1].Okpo_DB
                        ||
                        (string.IsNullOrEmpty(reports.Master_DB.Rows10[1].Okpo_DB)
                        && x.OKPO == reports.Master_DB.Rows10[0].Okpo_DB)))
                        .ToListAsync();
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
                       $"{ex.Message}",
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
}
