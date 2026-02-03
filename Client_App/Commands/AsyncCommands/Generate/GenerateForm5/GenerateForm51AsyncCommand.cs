using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Interfaces.Logger;
using Client_App.Properties;
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
using Models.Forms.Form1;
using Models.Forms.Form5;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            catch (OperationCanceledException) { throw ; }
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
                Report.Year.Value = answer.ToString();
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


            var organizations10IdList = await GetOrganizations10List(StaticConfiguration.DBModel, cts.Token, loadedList);

            var repList = await LoadReportList(organizations10IdList, progressBarVM, cts);

            var filteredRows11 = await FilterRows11(repList, progressBarVM, cts);

            GenerateForm51DependOnFilteredRows11(filteredRows11, progressBarVM, cts);

            progressBarVM.SetProgressBar(
            95,
            $"Выставляем номера строк");


            var orderedRows = Report.Rows51.OrderBy(row => row.OperationCode_DB).ToList();
            Report.Rows51.Clear();
            Report.Rows51.AddRange(orderedRows);


            //Выставляем номера строк
            for (int i = 0; i < Report.Rows51.Count; i++)
            {
                Report.Rows51[i].SetOrder(i + 1);
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
        private async Task<List<int>> GetOrganizations10List(DBModel DB, CancellationToken cancellationToken, List<ViacOrganization> loadedList = null)
        {
            try
            {

                cancellationToken.ThrowIfCancellationRequested();

                if (loadedList == null || loadedList.Count == 0)
                    return await DB.ReportsCollectionDbSet
                        .AsSplitQuery()
                        .AsNoTracking()
                        .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows10)
                        .Where(reports => reports.Master_DB.FormNum_DB == "1.0")
                        .Select(reports => reports.Id)
                        .ToListAsync(cancellationToken);
                else
                    return DB.ReportsCollectionDbSet
                        .AsSplitQuery()
                        .AsNoTracking()
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Where(x => x.Master_DB.FormNum_DB == "1.0")
                        .AsEnumerable()
                        .Where(reports => loadedList.Any(x => 
                        x.RegNo == reports.Master_DB.Rows10[0].RegNo_DB
                        && (x.OKPO == reports.Master_DB.Rows10[1].Okpo_DB
                        ||
                        (string.IsNullOrEmpty(reports.Master_DB.Rows10[1].Okpo_DB)
                        && x.OKPO == reports.Master_DB.Rows10[0].Okpo_DB))))
                        .Select(reports => reports.Id)
                        .ToList();
            }
            catch (OperationCanceledException)
            {
                throw;
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
                return new List<int>();
            }
        }
        private async Task<List<Report>> LoadReportList(List<int> organizations10IdList, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 15;
            double progressBarIncrement = (double)(30 - progressBarPercent) / organizations10IdList.Count();
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем отчеты 1.1 для обработки ({iteration}/{organizations10IdList.Count})");

            List<Report> repList = new List<Report>();

            foreach (var id in organizations10IdList)
            {
                try
                {
                    cts.Token.ThrowIfCancellationRequested();

                    repList.AddRange(StaticConfiguration.DBModel.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(rep => rep.Reports.Id == id)
                        .Where(rep => rep.FormNum_DB == "1.1")
                        .Where(rep => rep.StartPeriod_DB.EndsWith(year)
                        || rep.EndPeriod_DB.EndsWith(year))
                        .Include(rep => rep.Rows11));


                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем отчеты 1.1 для обработки ({iteration}/{organizations10IdList.Count})");
                }
                catch (OperationCanceledException)
                {
                    throw;
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
            return repList;
        }

        private async Task<List<Form11>> FilterRows11 (List<Report> repList, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 80;
            double progressBarIncrement = (double)(90 - progressBarPercent) / repList.Count;
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем строки из отчетов 1.1 для обработки ({iteration}/{repList.Count})");

            List<Form11> rows11List = new List<Form11>();
            foreach (var rep in repList)
            {
                try
                {

                    cts.Token.ThrowIfCancellationRequested();

                    var rows11 = rep.Rows11.Where(row11 => 
                        CodeOperationFilter.PlusOperationsForm51.Contains(row11.OperationCode_DB) 
                        || CodeOperationFilter.MinusOperationsForm51.Contains(row11.OperationCode_DB));

                    if (rep.StartPeriod_DB.EndsWith(year)
                    ^ rep.EndPeriod_DB.EndsWith(year))
                    {
                        rows11 = rows11.Where(row11 => row11.OperationDate_DB.EndsWith(year));
                    }

                    rows11List.AddRange(rows11);

                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Фильтруем строки из отчетов 1.1 для обработки ({iteration}/{repList.Count})");
                }
                catch (OperationCanceledException)
                {
                    throw;
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
            return rows11List;
        }

        private async Task GenerateForm51DependOnFilteredRows11(List<Form11> filteredRows11, AnyTaskProgressBarVM progressBarVM,CancellationTokenSource cts)
        {
            double progressBarPercent = 90;
            double progressBarIncrement = (double)(95 - progressBarPercent) / filteredRows11.Count;
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Фильтруем строки из отчетов 1.1 для обработки ({iteration}/{filteredRows11.Count})");

            foreach (var row11 in filteredRows11)
            {
                try
                {


                    //В только что импортированных отчетах может встречаться 
                    //некоректная запись экспоненциального формата,
                    //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                    row11.Activity_DB = Form.ConvertStringToExponentialFormat(row11.Activity_DB);

                    cts.Token.ThrowIfCancellationRequested();

                    var row51 = Report.Rows51.FirstOrDefault(r =>
                        r.OperationCode_DB == row11.OperationCode_DB
                        && r.Category_DB == row11.Category_DB
                        && r.Radionuclids_DB == row11.Radionuclids_DB
                        && r.ProviderOrRecieverOKPO_DB == row11.ProviderOrRecieverOKPO_DB);
                    if (row51 != null)
                    {
                        try
                        {
                            cts.Token.ThrowIfCancellationRequested();

                            row51.Quantity_DB += row11.Quantity_DB;

                            if (double.TryParse(row51.Activity_DB,
                                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                                CultureInfo.CreateSpecificCulture("ru-RU"), 
                                out var value) 
                            && double.TryParse(row11.Activity_DB,
                                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                                CultureInfo.CreateSpecificCulture("ru-RU"), 
                                out var inc))
                            {
                                var sumActivity = value + inc;
                                row51.Activity.Value = sumActivity.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            throw;
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

                            cts.Token.ThrowIfCancellationRequested();

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
                        catch (OperationCanceledException)
                        {
                            throw;
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
                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем строки из отчетов 1.1 для обработки ({iteration}/{filteredRows11.Count})");

                }
                catch (OperationCanceledException)
                {
                    throw;
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

        #endregion
    }
}
