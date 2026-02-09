using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
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
    public class GenerateForm53AsyncCommand(BaseFormVM formVM) : BaseGenerateForm5
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
            catch (OperationCanceledException) { throw; }
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
                .FirstOrDefault(w => w.Name == "5.3");

            if (owner == null) return;

            var cts = new CancellationTokenSource();



            List<ViacOrganization> loadedList = null;

            #region ShowAskMessages
            if (Report.Rows53.Count != 0)
            {
                if (!await ShowConfirmationMessage(owner)) return;
            }
            if (string.IsNullOrEmpty(year))
            {
                var answer = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskYearMessage(owner));
                Report.Year.Value = answer.ToString();
            }

            Report.Rows53.Clear();
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

            var repList = await LoadReportList(organizations10IdList, progressBarVM, cts);
            var repList13 = repList.FindAll(rep => rep.FormNum_DB == "1.3");
            var repList14 = repList.FindAll(rep => rep.FormNum_DB == "1.4");

            var filteredRows13 = await FilterRows13(repList13, progressBarVM, cts);
            var filteredRows14 = await FilterRows14(repList14, progressBarVM, cts);

            GenerateForm53DependOnFilteredRows13(filteredRows13, progressBarVM, cts);
            GenerateForm53DependOnFilteredRows14(filteredRows14, progressBarVM, cts);

            progressBarVM.SetProgressBar(
            95,
            $"Выставляем номера строк");

            var orderedRows = Report.Rows53.OrderBy(row => row.OperationCode_DB).ToList();
            Report.Rows53.Clear();
            Report.Rows53.AddRange(orderedRows);

            //Выставляем номера строк
            for (int i = 0; i < Report.Rows53.Count; i++)
            {
                Report.Rows53[i].SetOrder(i + 1);
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
        
        private async Task<List<Report>> LoadReportList(List<int> organizations10IdList, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 15;
            double progressBarIncrement = (double)(70 - progressBarPercent) / organizations10IdList.Count();
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Загружаем отчеты 1.3, 1.4 для обработки ({iteration}/{organizations10IdList.Count})");

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
                        .Where(rep => rep.FormNum_DB == "1.3" || rep.FormNum_DB == "1.4")
                        .Where(rep => rep.StartPeriod_DB.EndsWith(year)
                        || rep.EndPeriod_DB.EndsWith(year))
                        .Include(rep => rep.Rows13)
                        .Include(rep => rep.Rows14));


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
            return repList;
        }

        private async Task<List<Form13>> FilterRows13(List<Report> repList, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 70;
            double progressBarIncrement = (double)(80 - progressBarPercent) / repList.Count;
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Фильтруем строки из отчетов 1.3 для обработки ({iteration}/{repList.Count})");

            List<Form13> rows13List = new List<Form13>();
            foreach (var rep in repList)
            {
                try
                {

                    cts.Token.ThrowIfCancellationRequested();

                    var rows13 = rep.Rows13.Where(row13 => 
                        CodeOperationFilter.PlusOperationsForm53.Contains(row13.OperationCode_DB)
                        || CodeOperationFilter.MinusOperationsForm53.Contains(row13.OperationCode_DB));

                    if (rep.StartPeriod_DB.EndsWith(year)
                    ^ rep.EndPeriod_DB.EndsWith(year))
                    {
                        rows13 = rows13.Where(row13 => row13.OperationDate_DB.EndsWith(year));
                    }

                    rows13List.AddRange(rows13);

                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Фильтруем строки из отчетов 1.3 для обработки ({iteration}/{repList.Count})");
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
            return rows13List;
        }

        private async Task<List<Form14>> FilterRows14(List<Report> repList, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 80;
            double progressBarIncrement = (double)(90 - progressBarPercent) / repList.Count;
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Фильтруем строки из отчетов 1.4 для обработки ({iteration}/{repList.Count})");

            List<Form14> rows14List = new List<Form14>();
            foreach (var rep in repList)
            {
                try
                {

                    cts.Token.ThrowIfCancellationRequested();

                    var rows14 = rep.Rows14.Where(row14 => 
                        CodeOperationFilter.PlusOperationsForm53.Contains(row14.OperationCode_DB)
                        || CodeOperationFilter.MinusOperationsForm53.Contains(row14.OperationCode_DB));

                    if (rep.StartPeriod_DB.EndsWith(year)
                    ^ rep.EndPeriod_DB.EndsWith(year))
                    {
                        rows14 = rows14.Where(row14 => row14.OperationDate_DB.EndsWith(year));
                    }

                    rows14List.AddRange(rows14);

                    progressBarPercent += progressBarIncrement;
                    iteration++;
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Фильтруем строки из отчетов 1.4 для обработки ({iteration}/{repList.Count})");
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
            return rows14List;
        }

        private async Task GenerateForm53DependOnFilteredRows13(List<Form13> filteredRows13, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 90;
            double progressBarIncrement = (double)(95 - progressBarPercent) / filteredRows13.Count;
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем строки из отчетов 1.3 для обработки ({iteration}/{filteredRows13.Count})");

            foreach (var row13 in filteredRows13)
            {
                try
                {
                    //В только что импортированных отчетах может встречаться 
                    //некоректная запись экспоненциального формата,
                    //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                    row13.Activity_DB = Form.ConvertStringToExponentialFormat(row13.Activity_DB);


                    cts.Token.ThrowIfCancellationRequested();

                    var row53 = Report.Rows53.FirstOrDefault(r =>
                        r.OperationCode_DB == row13.OperationCode_DB
                        && r.TypeORI_DB == "1"
                        && r.VarietyORI_DB == null
                        && r.AggregateState_DB == 2
                        && r.ProviderOrRecieverOKPO_DB == row13.ProviderOrRecieverOKPO_DB);
                    if (row53 != null)
                    {
                        try
                        {
                            cts.Token.ThrowIfCancellationRequested();


                            row53.Activity.Value = SummarizeExponentionalStrings(row53.Activity.Value, row13.Activity.Value);

                            row53.Quantity_DB += 1;

                            AddRadionuclids(row53, row13.Radionuclids_DB);
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

                            Report.Rows53.Add(new Form53()
                            {
                                OperationCode_DB = row13.OperationCode_DB,
                                TypeORI_DB = "1",
                                VarietyORI_DB = null,
                                AggregateState_DB = 2,
                                ProviderOrRecieverOKPO_DB = row13.ProviderOrRecieverOKPO_DB,
                                Activity_DB = row13.Activity_DB,
                                Quantity_DB = 1,
                                Radionuclids_DB = row13.Radionuclids_DB,
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
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем строки из отчетов 1.3 для обработки ({iteration}/{filteredRows13.Count})");

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

        private async Task GenerateForm53DependOnFilteredRows14(List<Form14> filteredRows14, AnyTaskProgressBarVM progressBarVM, CancellationTokenSource cts)
        {
            double progressBarPercent = 82;
            double progressBarIncrement = (double)(95 - progressBarPercent) / filteredRows14.Count;
            int iteration = 0;
            progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем строки из отчетов 1.4 для обработки ({iteration}/{filteredRows14.Count})");

            foreach (var row14 in filteredRows14)
            {
                try
                {

                    //В только что импортированных отчетах может встречаться 
                    //некоректная запись экспоненциального формата,
                    //поэтому на всякий случай прогоняем через Exponentional_ValueChanged
                    row14.Activity_DB = Form.ConvertStringToExponentialFormat(row14.Activity_DB);
                    row14.Mass_DB = Form.ConvertStringToExponentialFormat(row14.Mass_DB);
                    row14.Volume_DB = Form.ConvertStringToExponentialFormat(row14.Volume_DB);

                    cts.Token.ThrowIfCancellationRequested();

                    var row53 = Report.Rows53.FirstOrDefault(r =>
                        r.OperationCode_DB == row14.OperationCode_DB
                        && r.TypeORI_DB == "2"
                        && r.VarietyORI_DB == row14.Sort_DB
                        && r.AggregateState_DB == row14.AggregateState_DB
                        && r.ProviderOrRecieverOKPO_DB == row14.ProviderOrRecieverOKPO_DB);
                    if (row53 != null)
                    {
                        try
                        {
                            cts.Token.ThrowIfCancellationRequested();


                            row53.Activity.Value = SummarizeExponentionalStrings(row53.Activity.Value, row14.Activity.Value);

                            row53.Mass.Value = SummarizeExponentionalStrings(row53.Mass.Value, row14.Mass.Value);

                            row53.Volume.Value = SummarizeExponentionalStrings(row53.Volume.Value, row14.Volume.Value);

                            AddRadionuclids(row53, row14.Radionuclids_DB);

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

                            Report.Rows53.Add(new Form53()
                            {
                                OperationCode_DB = row14.OperationCode_DB,
                                TypeORI_DB = "2",
                                VarietyORI_DB = row14.Sort_DB,
                                AggregateState_DB = row14.AggregateState_DB,
                                ProviderOrRecieverOKPO_DB = row14.ProviderOrRecieverOKPO_DB,
                                Activity_DB = row14.Activity_DB,
                                Mass_DB = row14.Mass_DB,
                                Volume_DB = row14.Volume_DB,
                                Radionuclids_DB = row14.Radionuclids_DB,
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
                    progressBarVM.SetProgressBar((int)progressBarPercent, $"Обрабатываем строки из отчетов 1.4 для обработки ({iteration}/{filteredRows14.Count})");

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

        private void AddRadionuclids(Form53 row53, string addedString)
        {
            var radionuclids = row53.Radionuclids_DB.Split(';').ToList();
            for (int i = 0; i < radionuclids.Count; i++)
            {
                radionuclids[i] = radionuclids[i].Trim();
            }

            var addedRadionuclids = addedString.Split(';');

            for (int i = 0; i < addedRadionuclids.Length; i++)
            {
                addedRadionuclids[i] = addedRadionuclids[i].Trim();
                if (!radionuclids.Any(rad => rad == addedRadionuclids[i]))
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
                row53.Radionuclids_DB = result;
            }
        }

        #endregion
    }
}
