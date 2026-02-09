using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.Properties;
using Client_App.Resources.CustomComparers.SnkComparers;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.Messages;
using Client_App.Views.ProgressBar;
using CommunityToolkit.Mvvm.DependencyInjection;
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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.Generate.GenerateForm5
{
    public class GenerateForm54AsyncCommand(BaseFormVM formVM) : BaseGenerateForm5
    {
        #region private Properties

        private Report Report => formVM.Report;

        private Window owner;

        private string year => formVM.Report.Year_DB;

        SnkRadionuclidsEqualityComparer comparer = new SnkRadionuclidsEqualityComparer();

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
                .FirstOrDefault(w => w.Name == "5.4");

            if (owner == null) return;

            var cts = new CancellationTokenSource();



            List<ViacOrganization> loadedList = null;

            #region ShowAskMessages
            if (Report.Rows54.Count != 0)
            {
                if (!await ShowConfirmationMessage(owner)) return;
            }
            if (string.IsNullOrEmpty(year))
            {
                var answer = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskYearMessage(owner));
                Report.Year.Value = answer.ToString();
            }

            Report.Rows54.Clear();
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

            LoadReportDictionary(organizations10IdList, out var rep13Dictionary,out var rep14Dictionary, progressBarVM, cts);


            GenerateForm54DependOnForm13(rep13Dictionary, progressBarVM, cts);
            GenerateForm54DependOnForm14(rep14Dictionary, progressBarVM, cts);

            progressBarVM.SetProgressBar(
            95,
            $"Выставляем номера строк");


            //var orderedRows = Report.Rows54.OrderBy(row => row.OperationCode_DB).ToList();
            //Report.Rows54.Clear();
            //Report.Rows54.AddRange(orderedRows);


            //Выставляем номера строк
            for (int i = 0; i < Report.Rows54.Count; i++)
            {
                Report.Rows54[i].SetOrder(i + 1);
            }

            progressBarVM.SetProgressBar(
                100,
                $"Завершаем формирование отчета");


            //Обновляем таблицу
            formVM.UpdateFormList();
            formVM.UpdatePageInfo();
            await Dispatcher.UIThread.InvokeAsync(async () => progressBar.Close());
        }

        #region Requests

       
        private void LoadReportDictionary(List<int> organizations10IdList, out Dictionary<int, List<Report>> rep13Dictionary, out Dictionary<int, List<Report>> rep14Dictionary, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 15;
            double progressBarIncrement = (double)(70 - progressBarPercent) / organizations10IdList.Count();
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем отчеты 1.3, 1.4 для обработки ({iteration}/{organizations10IdList.Count})");
            rep13Dictionary =new Dictionary<int, List<Report>> (); 
            rep14Dictionary = new Dictionary<int, List<Report>>();

            foreach (var id in organizations10IdList)
            {
                try
                {
                    cts.Token.ThrowIfCancellationRequested();

                    var reportList = StaticConfiguration.DBModel.ReportCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Where(rep => rep.Reports.Id == id)
                        .Where(rep => rep.FormNum_DB == "1.3" || rep.FormNum_DB == "1.4")
                        .Include(rep => rep.Rows13)
                        .Include(rep => rep.Rows14)
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

                    var report13List = reportList.FindAll(rep => rep.FormNum_DB == "1.3");
                    var report14List = reportList.FindAll(rep => rep.FormNum_DB == "1.4");

                    

                    var inventarization13 = report13List.LastOrDefault(report => 
                        report.Rows13.Any(row => 
                            row.OperationCode_DB == "10" ));

                    var inventarization14 = report14List.LastOrDefault(report =>
                        report.Rows14.Any(row =>
                            row.OperationCode_DB == "10"));



                    var i = report13List.IndexOf(inventarization13);

                    if(report13List.Skip(i).Count() > 0)
                        rep13Dictionary.Add(id, new List<Report>(report13List.Skip(i)));


                    i = report14List.IndexOf(inventarization14);

                    if (report14List.Skip(i).Count() > 0)
                        rep14Dictionary.Add(id, new List<Report>(report14List.Skip(i)));

                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем отчеты 1.3, 1.4 для обработки ({iteration}/{organizations10IdList.Count})");
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
        private async Task GenerateForm54DependOnForm13(Dictionary<int, List<Report>> reportDictionary, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 70;
            double progressBarIncrement = (double)(85 - progressBarPercent) / reportDictionary.Count();
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем отчеты 1.3 для обработки ({iteration}/{reportDictionary.Count})");
            try
            {

                foreach (var item in reportDictionary)
                {
                    var reportList = item.Value;

                    var inventarizationDate = ProcessInventoryReport13(reportList[0]);
                    int.TryParse(year, out var yearIntValue);
                    var endOfTheYear = new DateOnly(day: 31, month: 12, year: yearIntValue);

                    for (int i = 1; i < reportList.Count; i++)
                    {
                        foreach (Form13 row13 in reportList[i].Rows13)
                        {

                            if (!(DateOnly.TryParse(row13.OperationDate_DB, out var operationDate)
                                && inventarizationDate < operationDate
                                && operationDate <= endOfTheYear)) 
                                continue;

                            var matchingForm = FindMatchingForm54(row13);
                            if (matchingForm == null) continue;

                            //В только что импортированных отчетах может встречаться 
                            //некоректная запись экспоненциального формата,
                            //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                            row13.Activity_DB = Form.ConvertStringToExponentialFormat(row13.Activity_DB);

                            if (CodeOperationFilter.PlusOperationsForm54.Any(x => x == row13.OperationCode_DB))
                            {
                                matchingForm.Quantity_DB += 1;

                                matchingForm.Activity.Value = SummarizeExponentionalStrings(matchingForm.Activity.Value, row13.Activity.Value);


                                AddRadionuclids(matchingForm, row13.Radionuclids_DB);
                            }
                            else if (CodeOperationFilter.MinusOperationsForm54.Any(x => x == row13.OperationCode_DB))
                            {
                                matchingForm.Quantity_DB -= 1;

                                matchingForm.Activity.Value = SubtractExponentionalStrings(matchingForm.Activity.Value, row13.Activity.Value);

                                RemoveRadionuclids(matchingForm, row13.Radionuclids_DB);
                            }
                        }
                    }
                }
                progressBarPercent += progressBarIncrement;
                iteration++;
                progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем отчеты 1.3 для обработки ({iteration}/{reportDictionary.Count})");

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

        private async Task GenerateForm54DependOnForm14(Dictionary<int, List<Report>> reportDictionary, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 85;
            double progressBarIncrement = (double)(95 - progressBarPercent) / reportDictionary.Count();
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем отчеты 1.4 для обработки ({iteration}/{reportDictionary.Count})");
            try
            {

                foreach (var item in reportDictionary)
                {
                    var reportList = item.Value;

                    var inventarizationDate = ProcessInventoryReport14(reportList[0]);
                    int.TryParse(year, out var yearIntValue);
                    var endOfTheYear = new DateOnly(day: 31, month: 12, year: yearIntValue);

                    for (int i = 1; i < reportList.Count; i++)
                    {
                        foreach (Form14 row14 in reportList[i].Rows14)
                        {
                            if (!(DateOnly.TryParse(row14.OperationDate_DB, out var operationDate)
                                && inventarizationDate < operationDate
                                && operationDate <= endOfTheYear)) 
                                continue;

                            var matchingForm = FindMatchingForm54(row14);
                            if (matchingForm == null) continue;

                            //В только что импортированных отчетах может встречаться 
                            //некоректная запись экспоненциального формата,
                            //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                            row14.Activity_DB = Form.ConvertStringToExponentialFormat(row14.Activity_DB);
                            row14.Mass_DB = Form.ConvertStringToExponentialFormat(row14.Mass_DB);
                            row14.Volume_DB = Form.ConvertStringToExponentialFormat(row14.Volume_DB);

                            if (CodeOperationFilter.PlusOperationsForm54.Any(x => x == row14.OperationCode_DB))
                            {
                                matchingForm.Activity.Value = SummarizeExponentionalStrings(matchingForm.Activity.Value, row14.Activity.Value);
                                matchingForm.Mass.Value = SummarizeExponentionalStrings(matchingForm.Mass.Value, row14.Mass.Value);
                                matchingForm.Volume.Value = SummarizeExponentionalStrings(matchingForm.Volume.Value, row14.Volume.Value);

                                AddRadionuclids(matchingForm, row14.Radionuclids_DB);
                            }
                            else if (CodeOperationFilter.MinusOperationsForm54.Any(x => x == row14.OperationCode_DB))
                            {
                                matchingForm.Activity.Value = SubtractExponentionalStrings(matchingForm.Activity.Value, row14.Activity.Value);
                                matchingForm.Mass.Value = SubtractExponentionalStrings(matchingForm.Mass.Value, row14.Mass.Value);
                                matchingForm.Volume.Value = SubtractExponentionalStrings(matchingForm.Volume.Value, row14.Volume.Value);

                                RemoveRadionuclids(matchingForm, row14.Radionuclids_DB);
                            }
                        }
                    }
                }
                progressBarPercent += progressBarIncrement;
                iteration++;
                progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем отчеты 1.4 для обработки ({iteration}/{reportDictionary.Count})");
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

        #endregion

        private DateOnly ProcessInventoryReport13(Report rep)
        {
            var rows13 = rep.Rows13;
            DateOnly inventarizationDate = DateOnly.MinValue;
            foreach (Form13 row13 in rows13)
            {
                if (row13.OperationCode_DB != "10") continue;

                if (DateOnly.TryParse(row13.OperationDate_DB, out var dateOnly)
                && dateOnly > inventarizationDate)
                    inventarizationDate = dateOnly;

                var matchingForm = FindMatchingForm54(row13);

                //В только что импортированных отчетах может встречаться 
                //некоректная запись экспоненциального формата,
                //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                row13.Activity_DB = Form.ConvertStringToExponentialFormat(row13.Activity_DB);

                if (matchingForm != null)
                {
                    matchingForm.Quantity_DB += 1;

                    matchingForm.Activity.Value = SummarizeExponentionalStrings(matchingForm.Activity.Value, row13.Activity.Value);

                    AddRadionuclids(matchingForm, row13.Radionuclids_DB);
                }
                else
                {
                    Report.Rows54.Add(new Form54()
                    {
                        TypeORI_DB = "1",
                        VarietyORI_DB = null,
                        AggregateState_DB = 2,
                        Radionuclids_DB = row13.Radionuclids_DB,
                        Quantity_DB = 1,
                        Activity_DB = row13.Activity_DB
                    });
                }

            }
            return inventarizationDate;
        }

        private DateOnly ProcessInventoryReport14(Report rep)
        {
            var rows14 = rep.Rows14;
            DateOnly inventarizationDate = DateOnly.MinValue;
            foreach (Form14 row14 in rows14)
            {
                if (row14.OperationCode_DB != "10") continue;

                if (DateOnly.TryParse(row14.OperationDate_DB, out var dateOnly)
                && dateOnly > inventarizationDate)
                    inventarizationDate = dateOnly;

                var matchingForm = FindMatchingForm54(row14);

                //В только что импортированных отчетах может встречаться 
                //некоректная запись экспоненциального формата,
                //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                row14.Activity_DB = Form.ConvertStringToExponentialFormat(row14.Activity_DB);
                row14.Mass_DB = Form.ConvertStringToExponentialFormat(row14.Mass_DB);
                row14.Volume_DB = Form.ConvertStringToExponentialFormat(row14.Volume_DB);

                if (matchingForm != null)
                {

                    matchingForm.Activity.Value = SummarizeExponentionalStrings(matchingForm.Activity.Value, row14.Activity.Value);
                    matchingForm.Mass.Value = SummarizeExponentionalStrings(matchingForm.Mass.Value, row14.Mass.Value);
                    matchingForm.Volume.Value = SummarizeExponentionalStrings(matchingForm.Volume.Value, row14.Volume.Value);

                    AddRadionuclids(matchingForm, row14.Radionuclids_DB);
                }
                else
                {
                    Report.Rows54.Add(new Form54()
                    {
                        TypeORI_DB = "2",
                        VarietyORI_DB = row14.Sort_DB,
                        AggregateState_DB = row14.AggregateState_DB,
                        Radionuclids_DB = row14.Radionuclids_DB,
                        Activity_DB = row14.Activity_DB,
                        Mass_DB = row14.Mass_DB,
                        Volume_DB = row14.Volume_DB,
                    });
                }

            }
            return inventarizationDate;
        }

        private void AddRadionuclids(Form54 row54, string addedString)
        {
            var radionuclids = row54.Radionuclids_DB.Split(';').ToList();
            for (int i = 0; i < radionuclids.Count; i++)
            {
                radionuclids[i] = radionuclids[i].Trim();
            }

            var addedRadionuclids = addedString.Split(';');

            for (int i = 0; i < addedRadionuclids.Length; i++)
            {
                addedRadionuclids[i] = addedRadionuclids[i].Trim();
                if (!radionuclids.Any(rad => comparer.Equals(rad, addedRadionuclids[i])))
                {
                    radionuclids.Add(addedRadionuclids[i]);
                }
            }
            if (radionuclids.Count > 0)
            {
                var result = radionuclids[0];
                for (int i = 1; i < radionuclids.Count; i++)
                {
                    result = result + "; " + radionuclids[i];
                }
                row54.Radionuclids_DB = result;
            }
        }
        private void RemoveRadionuclids(Form54 row54, string removedString)
        {
            var radionuclids = row54.Radionuclids_DB.Split(';').ToList();
            for (int i = 0; i < radionuclids.Count; i++)
            {
                radionuclids[i] = radionuclids[i].Trim();
            }

            var removedRadionuclids = removedString.Split(';');

            for (int i = 0; i < removedRadionuclids.Length; i++)
            {
                removedRadionuclids[i] = removedRadionuclids[i].Trim();
                if (radionuclids.Any(rad => comparer.Equals(rad, removedRadionuclids[i])))
                {
                    radionuclids.RemoveAll(rad => comparer.Equals(rad, removedRadionuclids[i]));
                }
            }
            if (radionuclids.Count > 0)
            {
                var result = radionuclids[0];
                for (int i = 1; i < radionuclids.Count; i++)
                {
                    result = result + "; " + radionuclids[i];
                }
                row54.Radionuclids_DB = result;
            }
        }
        private Form54? FindMatchingForm54(Form form)
        {
            switch (form.FormNum_DB)
            {
                case "1.3":
                    return Report.Rows54.FirstOrDefault(row54 =>
                    row54.TypeORI_DB == "1"
                    && row54.VarietyORI_DB == null
                    && row54.AggregateState_DB == 2);
                case "1.4":
                    var row14 = form as Form14;
                    return Report.Rows54.FirstOrDefault(row54 =>
                    row54.TypeORI_DB == "2"
                    && row54.VarietyORI_DB == row14.Sort_DB
                    && row54.AggregateState_DB == row14.AggregateState_DB);
                    break;
                default:
                    return null;
            }
            
        }
    }
}
