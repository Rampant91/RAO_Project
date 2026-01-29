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
    public class GenerateForm55AsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
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
                .FirstOrDefault(w => w.Name == "5.5");

            if (owner == null) return;

            var cts = new CancellationTokenSource();



            List<ViacOrganization> loadedList = null;

            #region ShowAskMessages
            if (Report.Rows55.Count != 0)
            {
                if (!await ShowConfirmationMessage(owner)) return;
            }
            if (string.IsNullOrEmpty(year))
            {
                var answer = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskYearMessage(owner));
                Report.Year.Value = answer.ToString();
            }
            Report.Rows55.Clear();

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

            var filteredRows12 = await FilterRows12(repList, progressBarVM, cts);

            GenerateForm55DependOnFilteredRows12(filteredRows12, progressBarVM, cts);

            progressBarVM.SetProgressBar(
            95,
            $"Выставляем номера строк");


            var orderedRows = Report.Rows55.OrderBy(row => row.OperationCode_DB).ToList();
            Report.Rows55.Clear();
            Report.Rows55.AddRange(orderedRows);

            //Выставляем номера строк
            for (int i = 0; i < Report.Rows55.Count; i++)
            {
                Report.Rows55[i].SetOrder(i + 1);
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
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем отчеты 1.2 для обработки ({iteration}/{organizations10IdList.Count})");

            List<Report> repList = new List<Report>();

            foreach (var id in organizations10IdList)
            {
                try
                {
                    cts.Token.ThrowIfCancellationRequested();

                    repList.AddRange(StaticConfiguration.DBModel.ReportCollectionDbSet
                        .Where(rep => rep.Reports.Id == id)
                        .Where(rep => rep.FormNum_DB == "1.2")
                        .Where(rep => rep.StartPeriod_DB.EndsWith(year)
                        || rep.EndPeriod_DB.EndsWith(year))
                        .Include(rep => rep.Rows12));


                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем отчеты 1.2 для обработки ({iteration}/{organizations10IdList.Count})");
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

        private async Task<List<Form12>> FilterRows12 (List<Report> repList, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 30;
            double progressBarIncrement = (double)(50 - progressBarPercent) / repList.Count;
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем строки из отчетов 1.2 для обработки ({iteration}/{repList.Count})");

            List<Form12> rows12List = new List<Form12>();
            foreach (var rep in repList)
            {
                try
                {

                    cts.Token.ThrowIfCancellationRequested();

                    var rows12 = rep.Rows12.Where(row12 => CodeOperationFilter.AllOperationCodes.Contains(row12.OperationCode_DB));

                    if (rep.StartPeriod_DB.EndsWith(year)
                    ^ rep.EndPeriod_DB.EndsWith(year))
                    {
                        rows12 = rows12.Where(row12 => row12.OperationDate_DB.EndsWith(year));
                    }

                    rows12List.AddRange(rows12);

                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Фильтруем строки из отчетов 1.2 для обработки ({iteration}/{repList.Count})");
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
            return rows12List;
        }

        private async Task GenerateForm55DependOnFilteredRows12(List<Form12> filteredRows12, AnyTaskProgressBarVM progressBarVM,CancellationTokenSource cts)
        {
            double progressBarPercent = 50;
            double progressBarIncrement = (double)(95 - progressBarPercent) / filteredRows12.Count;
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Фильтруем строки из отчетов 1.2 для обработки ({iteration}/{filteredRows12.Count})");

            foreach (var row12 in filteredRows12)
            {
                try
                {

                    cts.Token.ThrowIfCancellationRequested();

                    var row55 = Report.Rows55.FirstOrDefault(r =>
                        r.Name_DB == row12.NameIOU_DB
                        && r.OperationCode_DB == row12.OperationCode_DB
                        && r.ProviderOrRecieverOKPO_DB == row12.ProviderOrRecieverOKPO_DB);
                    if (row55 != null)
                    {
                        try
                        {
                            cts.Token.ThrowIfCancellationRequested();

                            row55.Quantity_DB ++;

                            if (double.TryParse(row55.Mass_DB,
                                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                                CultureInfo.CreateSpecificCulture("ru-RU"), 
                                out var value) 
                            && double.TryParse(row12.Mass_DB,
                                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                                CultureInfo.CreateSpecificCulture("ru-RU"), 
                                out var inc))
                            {
                                var sumMass = value + inc;
                                row55.Mass.Value = sumMass.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));
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

                            Report.Rows55.Add(new Form55()
                            {
                                Name_DB = row12.NameIOU_DB,
                                OperationCode_DB = row12.OperationCode_DB,
                                ProviderOrRecieverOKPO_DB = row12.ProviderOrRecieverOKPO_DB,
                                Quantity_DB = 1,
                                Mass_DB = row12.Mass_DB
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
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем строки из отчетов 1.2 для обработки ({iteration}/{filteredRows12.Count})");

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
