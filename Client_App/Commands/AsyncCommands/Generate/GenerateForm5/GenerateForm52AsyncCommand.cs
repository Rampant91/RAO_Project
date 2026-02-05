using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.Properties;
using Client_App.Resources.CustomComparers.SnkComparers;
using Client_App.Resources.CustomComparers.SnkComparers;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using DynamicData;
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
    public class GenerateForm52AsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        #region private Properties

        private Report Report => formVM.Report;

        private Window owner;

        private string year => formVM.Report.Year_DB;

        CustomSnkRadionuclidsEqualityComparer comparer = new CustomSnkRadionuclidsEqualityComparer();


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
                .FirstOrDefault(w => w.Name == "5.2");

            if (owner == null) return;

            var cts = new CancellationTokenSource();



            List<ViacOrganization> loadedList = null;

            #region ShowAskMessages
            if (Report.Rows52.Count != 0)
            {
                if (!await ShowConfirmationMessage(owner)) return;
            }
            if (string.IsNullOrEmpty(year))
            {
                var answer = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskYearMessage(owner));
                Report.Year.Value = answer.ToString();
            }

            Report.Rows52.Clear();
            await new SaveReportAsyncCommand(formVM).AsyncExecute(null);

            if (Settings.Default.AppLaunchedInNorao)
            {
                loadedList = await ViacManager.GetAllOrganizationsFromVIAC(owner);
            }

            #endregion

            var progressBar = await Dispatcher.UIThread.InvokeAsync(async () => new AnyTaskProgressBar(cts, owner));
            var progressBarVM = progressBar.AnyTaskProgressBarVM;


            progressBarVM.SetProgressBar(5, $"Загрузка организаций");


            var organizations10IdList = await GetOrganizations10List(StaticConfiguration.DBModel, cts.Token, loadedList);

            var repDictionary = await LoadReportDictionary(organizations10IdList, progressBarVM, cts);

            GenerateForm52(repDictionary, progressBarVM, cts);

            progressBarVM.SetProgressBar(
            95,
            $"Выставляем номера строк");


            //var orderedRows = Report.Rows52.OrderBy(row => row.OperationCode_DB).ToList();
            //Report.Rows52.Clear();
            //Report.Rows52.AddRange(orderedRows);


            //Выставляем номера строк
            for (int i = 0; i < Report.Rows52.Count; i++)
            {
                Report.Rows52[i].SetOrder(i + 1);
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

        private async Task<Dictionary<int, List<Report>>> LoadReportDictionary(List<int> organizations10IdList, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 15;
            double progressBarIncrement = (double)(75 - progressBarPercent) / organizations10IdList.Count();
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем отчеты 1.1 для обработки ({iteration}/{organizations10IdList.Count})");

            var result = new Dictionary<int, List<Report>>();

            foreach (var id in organizations10IdList)
            {
                try
                {
                    cts.Token.ThrowIfCancellationRequested();

                    var reportList = StaticConfiguration.DBModel.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(rep => rep.Reports.Id == id)
                        .Where(rep => rep.FormNum_DB == "1.1")
                        .Include(rep => rep.Rows11)
                        .ToList()
                        .Select(rep => new
                        {
                            Report = rep,
                            CanParseStart = DateOnly.TryParse(rep.StartPeriod_DB, out var startDate),
                            CanParseEnd = DateOnly.TryParse(rep.EndPeriod_DB, out var endDate),
                            StartDate = startDate,
                            EndDate = endDate,
                            StartPeriod = rep.StartPeriod_DB,
                            EndPeriod = rep.EndPeriod_DB
                        })
                        .Where(x => x.CanParseStart && x.CanParseEnd)
                        .OrderBy(x => x.EndDate)
                        .Select(x => x.Report)
                        .ToList();

                    var inventarization = reportList.LastOrDefault(report => 
                        report.Rows11.Any(row => 
                            row.OperationCode_DB == "10"));

                    var i = reportList.IndexOf(inventarization);

                    if(reportList.Skip(i).Count() > 0)
                        result.Add(id, new List<Report>(reportList.Skip(i)));

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
            return result;
        }
        private async Task GenerateForm52(Dictionary<int, List<Report>> reportDictionary, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {

            double progressBarPercent = 75;
            double progressBarIncrement = (double)(95 - progressBarPercent) / reportDictionary.Count();
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем отчеты 1.1 для обработки ({iteration}/{reportDictionary.Count})");
            try
            {

                foreach (var item in reportDictionary)
                {
                    var reportList = item.Value;

                    var inventarizationDate = ProcessInventoryReport(reportList[0]);
                    int.TryParse(year, out var yearIntValue);
                    var endOfTheYear = new DateOnly(day: 31, month:12, year: yearIntValue);
                    for(int i = 1; i < reportList.Count; i++)
                    {
                        foreach (Form11 row11 in reportList[i].Rows11)
                        {
                            if (!(DateOnly.TryParse(row11.OperationDate_DB, out var operationDate)
                                && inventarizationDate < operationDate
                                && operationDate <= endOfTheYear))
                                continue;

                            var matchingForm = FindMatchingForm52(row11);
                            if (matchingForm == null) continue;

                            //В только что импортированных отчетах может встречаться 
                            //некоректная запись экспоненциального формата,
                            //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                            row11.Activity_DB = Form.ConvertStringToExponentialFormat(row11.Activity_DB);

                            if (CodeOperationFilter.PlusOperationsForm52.Any(x => x == row11.OperationCode_DB))
                            {
                                matchingForm.Quantity_DB += row11.Quantity_DB;


                                if (double.TryParse(matchingForm.Activity_DB,
                                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                                    CultureInfo.CreateSpecificCulture("ru-RU"),
                                    out var value)
                                && double.TryParse(row11.Activity_DB,
                                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                                    CultureInfo.CreateSpecificCulture("ru-RU"),
                                    out var inc))
                                {
                                    var sumActivity = value + inc;
                                    matchingForm.Activity.Value = sumActivity.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));
                                }
                            }
                            else if (CodeOperationFilter.MinusOperationsForm52.Any(x => x == row11.OperationCode_DB))
                            {
                                matchingForm.Quantity_DB -= row11.Quantity_DB;

                                if (double.TryParse(matchingForm.Activity_DB,
                                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                                    CultureInfo.CreateSpecificCulture("ru-RU"),
                                    out var value)
                                && double.TryParse(row11.Activity_DB,
                                    NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                                    CultureInfo.CreateSpecificCulture("ru-RU"),
                                    out var dec))
                                {
                                    var sumActivity = value - dec;
                                    matchingForm.Activity.Value = sumActivity.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));
                                }
                            }
                        }
                    }

                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем строки из отчетов 1.1 для обработки ({iteration}/{reportDictionary.Count})");
                }
            } catch (Exception ex)
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

        #endregion

        private DateOnly ProcessInventoryReport(Report rep)
        {
            var rows11 = rep.Rows11;
            DateOnly inventarizationDate = DateOnly.MinValue;
            foreach (Form11 row11 in rows11)
            {
                if (row11.OperationCode_DB != "10") continue;

                if (DateOnly.TryParse(row11.OperationDate_DB, out var dateOnly)
                && dateOnly > inventarizationDate)
                    inventarizationDate = dateOnly;

                //В только что импортированных отчетах может встречаться 
                //некоректная запись экспоненциального формата,
                //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                row11.Activity_DB = Form.ConvertStringToExponentialFormat(row11.Activity_DB);

                var matchingForm = FindMatchingForm52(row11);

                if (matchingForm != null)
                {
                    matchingForm.Quantity_DB += row11.Quantity_DB;


                    if (double.TryParse(matchingForm.Activity_DB,
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                        CultureInfo.CreateSpecificCulture("ru-RU"),
                        out var value)
                    && double.TryParse(row11.Activity_DB,
                        NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                        CultureInfo.CreateSpecificCulture("ru-RU"),
                        out var inc))
                    {
                        var sumActivity = value + inc;
                        matchingForm.Activity.Value = sumActivity.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));
                    }
                }
                else
                {
                    Report.Rows52.Add(new Form52()
                    {
                        Category_DB = row11.Category_DB,
                        Radionuclids_DB = row11.Radionuclids_DB,
                        Quantity_DB = row11.Quantity_DB,
                        Activity_DB = row11.Activity_DB
                    });
                }

            }

            return inventarizationDate;
        }

        private Form52? FindMatchingForm52(Form11 row11)
        {
            return Report.Rows52.FirstOrDefault(row52 =>
            row52.Category_DB == row11.Category_DB
            && comparer.Equals(row52.Radionuclids_DB, row11.Radionuclids_DB));
        }
    }
}
