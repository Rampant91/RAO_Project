using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form4;
using OfficeOpenXml.Drawing.Controls;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.CheckForm
{
    public class CheckF41 : CheckBase
    {
        static List<Organization> organizations10 = new List<Organization>();
        static List<Organization> organizations20 = new List<Organization>();
        public override bool CanExecute(object? parameter) => true;

        #region AsyncExecute
        public override async Task<List<CheckError>> AsyncExecute(object? parameter)
        {
            return await Check_Total(parameter);
        }
        #endregion
        public static async Task<List<CheckError>> Check_Total(object? parameter)
        {

            DBModel secondDB = null; // Пользователь может указать другую БД, в которой хранятся годовые отчеты (2.X)
            var cts = new CancellationTokenSource();
            List<CheckError> errorList = [];
            var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
            var progressBarVM = progressBar.AnyTaskProgressBarVM;
            var rep = parameter as Report;

            organizations10.Clear();
            organizations20.Clear();

            if (rep is null) await CancelCommandAndCloseProgressBarWindow(cts, progressBar);

            double currentProgress = 5;
            double incProgress = (100.0 - currentProgress) / rep.Rows41.Count();

            var dbModel = StaticConfiguration.DBModel;

            var formList = rep.Rows41.ToList();
            try
            {
                var reportsQuery = dbModel.ReportsCollectionDbSet
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows10)
                    .Where(reps => reps.Master_DB.FormNum_DB == "1.0");


                foreach (var reports in reportsQuery)
                {
                    if (formList.Any(form41 =>
                    form41.RegNo_DB == reports.Master_DB.Rows10[0].RegNo_DB
                    && (form41.Okpo_DB == reports.Master_DB.Rows10[0].Okpo_DB
                    || form41.Okpo_DB == reports.Master_DB.Rows10[1].Okpo_DB)))
                    {
                        string okpo;
                        if (reports.Master_DB.Rows10[1].Okpo_DB is "" or "-" or null)
                            okpo = reports.Master_DB.Rows10[0].Okpo_DB;
                        else
                            okpo = reports.Master_DB.Rows10[1].Okpo_DB;

                        organizations10.Add(new Organization()
                            {
                                Id = reports.Id,
                                RegNo = reports.Master_DB.Rows10[0].RegNo_DB,
                                Okpo = okpo,
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            try
            {
                var reportsQuery = dbModel.ReportsCollectionDbSet
                     .AsSplitQuery()
                     .AsQueryable()
                     .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows20)
                     .Where(reps => reps.Master_DB.FormNum_DB == "2.0");

                if (reportsQuery.Count() == 0)
                {
                    var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                        .FirstOrDefault(w => w.IsActive);
                    if (await ShowAskSecondDB(owner))
                    {
                        var dialog = new OpenFileDialog()
                        {
                            AllowMultiple = false,
                        };
                        var path = await dialog.ShowAsync(owner);
                        if (path != null)
                        {
                            secondDB = new DBModel(path[0]);
                            try
                            {
                                reportsQuery = secondDB.ReportsCollectionDbSet
                                 .AsSplitQuery()
                                 .AsQueryable()
                                 .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows20)
                                 .Where(reps => reps.Master_DB.FormNum_DB == "2.0");

                                reportsQuery.Count();//Небольшой запрос, чтобы проверить подключение к БД
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
                                       "Не удалось открыть базу данных с годовыми отчетами",
                                       MinWidth = 300,
                                       MinHeight = 125,
                                       WindowStartupLocation = WindowStartupLocation.CenterOwner
                                   })
                                   .ShowDialog(owner));
                            }
                        }
                    }
                }

                foreach (var reports in reportsQuery)
                {
                    if (formList.Any(form41 =>
                    form41.RegNo_DB == reports.Master_DB.Rows20[0].RegNo_DB
                    && (form41.Okpo_DB == reports.Master_DB.Rows20[0].Okpo_DB
                    || form41.Okpo_DB == reports.Master_DB.Rows20[1].Okpo_DB)))
                    {
                        string okpo;
                        if (reports.Master_DB.Rows20[1].Okpo_DB is "" or "-" or null)
                            okpo = reports.Master_DB.Rows20[0].Okpo_DB;
                        else
                            okpo = reports.Master_DB.Rows20[1].Okpo_DB;

                        organizations20.Add(new Organization()
                        {
                            Id = reports.Id,
                            RegNo = reports.Master_DB.Rows20[0].RegNo_DB,
                            Okpo = okpo,
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (Form41 form41 in rep.Rows41)
            {
                var regNo = form41.RegNo_DB == null ? "_____" : form41.RegNo_DB;
                progressBarVM.SetProgressBar((int)currentProgress, $"Проверка организации №{regNo}");

                var error = await CheckPresenceOfForm19(form41, secondDB);
                if (error != null)
                    errorList.Add(error);

                error = await CheckComplianceNumOfInventarizationReports(form41);
                if (error != null)
                    errorList.Add(error);

                error = await CheckComplianceNumWithoutInventarizationReports(form41);
                if (error != null)
                    errorList.Add(error);

                error = await CheckComplianceNumOfReports212(form41, secondDB);
                if (error != null)
                    errorList.Add(error);

                error = await CheckPresenceOfDepletedUraniumProducts(form41);
                if (error != null)
                    errorList.Add(error);

                currentProgress += incProgress;
            }


            #if DEBUG
            foreach (var organization in organizations10)
            {
                var massBalance12 = await CountMassBalanceForm12(organization.Id);
                var massBalance14 = await CountMassBalanceForm14(organization.Id);
                errorList.Add(new CheckError()
                {
                    Message =
                    $"РегНомер: {organization.RegNo}\n" +
                    $"ОКПО: {organization.Okpo}\n" +
                    $"Уран = {massBalance12}\n" +
                    $"Жидкое РВ  = {massBalance14.Item1}\n" +
                    $"Твердое РВ = {massBalance14.Item2}\n" +
                    $"Газовое РВ = {massBalance14.Item3}\n"
                });

            }
            #endif

            for (int i = 0; i < errorList.Count; i++)
            {
                errorList[i].Index = i + 1;
            }
            progressBarVM.SetProgressBar(100, $"Проверка выполнена успешно");
            //Освобождаем память
            if (secondDB != null)
                secondDB.Dispose();

            await progressBar.CloseAsync();
            return errorList;
        }

        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumWithoutInventarizationReports(Form41 form41)
        {
            var dbModel = StaticConfiguration.DBModel;
            int count = 0;
            try
            {
                var organization = organizations10.FirstOrDefault(org =>
                    org.RegNo == form41.RegNo_DB
                    && (org.Okpo == form41.Okpo_DB));




                if (organization != null)
                {
                    count = await dbModel.ReportsCollectionDbSet
                    .AsNoTracking()
                    .Include(x => x.DBObservable)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows12)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows14)
                    .Where(reps => reps.DBObservable != null && reps.Id == organization.Id)
                    .SelectMany(x => x.Report_Collection
                        .Where(y => y.EndPeriod_DB.EndsWith(form41.Report.Year_DB)
                        &&
                        (
                            y.FormNum_DB == "1.1" && y.Rows11.All(form => form.OperationCode_DB != "10")
                            || y.FormNum_DB == "1.2" && y.Rows12.All(form => form.OperationCode_DB != "10")
                            || y.FormNum_DB == "1.3" && y.Rows13.All(form => form.OperationCode_DB != "10")
                            || y.FormNum_DB == "1.4" && y.Rows14.All(form => form.OperationCode_DB != "10")
                        )))
                    .CountAsync();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (count != form41.NumOfFormsWithoutInventarizationInfo_DB)
                return
                    new CheckError()
                    {
                        FormNum = "form_41",
                        Row = $"{form41.NumberInOrder_DB}",
                        Column = $"7",
                        Value = $"{form41.NumOfFormsWithoutInventarizationInfo_DB}",
                        Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" не совпадает количество отчётов 1.1 - 1.4 без инвентаризации:\n" +
                        $"Указано: {form41.NumOfForms212_DB},\n" +
                        $"Найдено в базе данных: {count}"
                    };
            else return null;
        }

        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumOfInventarizationReports(Form41 form41)
        {
            var dbModel = StaticConfiguration.DBModel;
            int count;
            var organization = organizations10.FirstOrDefault(org =>
                    org.RegNo == form41.RegNo_DB
                    && (org.Okpo == form41.Okpo_DB));

            if (organization == null)
            {
                count = 0;
            }
            else
            {
                count = await dbModel.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows12)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows14)
                .Where(reps => reps.DBObservable != null && reps.Id == organization.Id)
                .SelectMany(x => x.Report_Collection
                    .Where(y => y.EndPeriod_DB.EndsWith(form41.Report.Year_DB)
                    &&
                    (
                        y.FormNum_DB == "1.1" && y.Rows11.Any(form => form.OperationCode_DB == "10")
                        || y.FormNum_DB == "1.2" && y.Rows12.Any(form => form.OperationCode_DB == "10")
                        || y.FormNum_DB == "1.3" && y.Rows13.Any(form => form.OperationCode_DB == "10")
                        || y.FormNum_DB == "1.4" && y.Rows14.Any(form => form.OperationCode_DB == "10")
                    )))
                .CountAsync();
            }

            if (count != form41.NumOfFormsWithInventarizationInfo_DB)
                return
                    new CheckError()
                    {
                        FormNum = "form_41",
                        Row = $"{form41.NumberInOrder_DB}",
                        Column = $"6",
                        Value = $"{form41.NumOfFormsWithInventarizationInfo_DB}",
                        Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" не совпадает количество инвентаризационных отчётов 1.1-1.4:\n" +
                        $"Указано: {form41.NumOfForms212_DB},\n" +
                        $"Найдено в базе данных: {count}"
                    };
            else return null;
        }


        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumOfReports212(Form41 form41, DBModel secondDB = null)
        {
            var dbModel = StaticConfiguration.DBModel;
            if (secondDB == null)
                secondDB = dbModel;
            int count;

            var organization = organizations20.FirstOrDefault(org =>
                    org.RegNo == form41.RegNo_DB
                    && (org.Okpo == form41.Okpo_DB));

            if (organization == null)
            {
                count = 0;
            }
            else
            {
                count = await secondDB.ReportCollectionDbSet
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(report => report.Reports)
                    .Where(report => report.Reports.Id == organization.Id)
                    .Where(report => report.Year_DB == form41.Report.Year_DB)
                    .Where(report => report.FormNum_DB == "2.12")
                    .CountAsync();
            }

            if (count != form41.NumOfForms212_DB)
                return
                    new CheckError()
                    {
                        FormNum = "form_41",
                        Row = $"{form41.NumberInOrder_DB}",
                        Column = $"8",
                        Value = $"{form41.NumOfForms212_DB}",
                        Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" не совпадает количество отчётов 2.12:\n" +
                        $"Указано: {form41.NumOfForms212_DB},\n" +
                        $"Найдено в базе данных: {count}"
                    };
            else return null;
        }
        /// <summary>
        /// Если нет отчета 2.12 – проверяется наличие ф.1.9. Если форма 1.9 есть – ошибка «Должен быть отчет по форме 2.12».
        /// Если нет отчета по 2.12 и 1.9, но в предыдущем отчете есть 2.12 – предупреждение «Проверьте организацию на необходимость представления отчета по ф.2.12
        /// </summary>
        /// <returns></returns>
        private static async Task<CheckError?> CheckPresenceOfForm19(Form41 form41, DBModel secondDB = null)
        {
            string regNo = form41.RegNo.Value;
            string year = form41.Report.Year_DB;
            DBModel dbModel = StaticConfiguration.DBModel;
            if (secondDB == null)
                secondDB = dbModel;

            var organization20 = secondDB.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == regNo))
                .Include(x => x.Report_Collection)
                .ThenInclude(x => x.Rows212).FirstOrDefault();

            //Если отчет 2.12 есть, то Выход
            if ((organization20 != null) && organization20.Report_Collection
                .Any(report =>
                report.FormNum_DB == "2.12"
                && report.Year_DB == year))
                return null;



            var organization10 = dbModel.ReportsCollectionDbSet
                .AsNoTracking()
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Where(x => x.Master_DB.Rows10.Any(y => y.RegNo_DB == regNo))
                .Include(x => x.Report_Collection)
                .ThenInclude(x => x.Rows19).FirstOrDefault();


            //Если есть отчеты 1.9, то Выход с ошибкой
            if ((organization10 != null) && organization10.Report_Collection
                .Any(report =>
                report.FormNum_DB == "1.9"
                && DateOnly.TryParse(report.EndPeriod_DB, out var dateOnly)
                && dateOnly.Year.ToString() == year))
                return
                    new CheckError()
                    {
                        FormNum = "form_41",
                        Row = $"{form41.NumberInOrder_DB}",
                        Column = $"-",
                        Value = $"-",
                        Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" должен быть отчет по форме 2.12, потому что у нее есть отчет по форме 1.9"
                    };

            if ((organization20 != null) && organization20.Report_Collection
                .Any(report =>
                report.FormNum_DB == "2.12"
                && int.TryParse(year, out var intYear)
                && report.Year_DB == (intYear - 1).ToString())) //проверка на предыдущий отчет
                return new CheckError()
                {
                    FormNum = "form_41",
                    Row = $"{form41.NumberInOrder_DB}",
                    Column = $"-",
                    Value = $"-",
                    Message = $"Проверьте организацию №{regNo} на необходимость представления отчета по форме 2.12"
                };

            return null;

        }
        private static async Task<CheckError?> CheckPresenceOfDepletedUraniumProducts(Form41 form41)
        {
            if (form41.NumOfFormsWithInventarizationInfo_DB <= 0) return null;

            var organization = organizations10
                .FirstOrDefault(org => 
                org.RegNo == form41.RegNo_DB 
                && org.Okpo == form41.Okpo_DB);

            if (organization == null) return null;

            if (await CountMassBalanceForm12(organization.Id) > 0)
                return new CheckError()
                {
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" на балансе присутствуют изделия из обедненного урана"
                };
            return null;
        }
        private static async Task CancelCommandAndCloseProgressBarWindow(CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null)
        {
            await cts.CancelAsync();
            if (progressBar is not null) await progressBar.CloseAsync();
            cts.Token.ThrowIfCancellationRequested();
        }

        private static async Task<bool> ShowAskSecondDB(Window owner)
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
                    ContentMessage = "Хотите указать путь к базе данных с годовыми отчетами по форме 2.12?",
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

        private static async Task<double> CountMassBalanceForm12 (int reportsID)
        {
            double massBalance = 0;
            bool InventarizationFlag = false;
            var dbModel = StaticConfiguration.DBModel;
            var reports = dbModel.ReportsCollectionDbSet
                .Include(reps => reps.Report_Collection)
                .ThenInclude(rep => rep.Rows12)
                .FirstOrDefault(reps => reps.Id == reportsID);
            var reportCollection = reports.Report_Collection.ToList()
                .FindAll(rep => rep.FormNum_DB == "1.2" 
                && DateTime.TryParse(rep.StartPeriod_DB, out var date)
                && DateTime.TryParse(rep.EndPeriod_DB, out date));

            try
            {


                for (int i = reportCollection.Count - 1; i >= 0; i--)
                {
                    var report = reportCollection[i];
                    for (int j = 0; j < report.Rows12.Count; j++)
                    {
                        if (!InventarizationFlag)
                        {
                            if (report.Rows12[j].OperationCode_DB == "10")
                            {
                                InventarizationFlag = true;
                                double.TryParse(report.Rows12[j].Mass_DB, out massBalance);
                            }
                        }
                        else
                        {
                            if (double.TryParse(report.Rows12[j].Mass_DB, out var mass)
                                && Spravochniks.SignsOperation["1.2"].ContainsKey($"{report.Rows12[j].OperationCode_DB}"))
                            {
                                if (Spravochniks.SignsOperation["1.2"][$"{report.Rows12[j].OperationCode_DB}"] == '+')
                                    massBalance += mass;
                                else if (Spravochniks.SignsOperation["1.2"][$"{report.Rows12[j].OperationCode_DB}"] == '-')
                                    massBalance -= mass;
                            }
                        }

                    }
                }
            } catch (Exception ex)
            {
                throw ex;
            }
            return massBalance;
        }
        private static async Task<Tuple<double,double,double>> CountMassBalanceForm14(int reportsID)
        {
            double[] massBalances = new double[3];
            bool[] inventarizationFlags = new bool[3];

            var dbModel = StaticConfiguration.DBModel;
            var reports = dbModel.ReportsCollectionDbSet
                .Include(reps => reps.Report_Collection)
                .ThenInclude(rep => rep.Rows14)
                .FirstOrDefault(reps => reps.Id == reportsID);
            var reportCollection = reports.Report_Collection.ToList().FindAll(rep => rep.FormNum_DB == "1.4");


            for (int i = reportCollection.Count - 1; i >= 0; i--)
            {
                var report = reportCollection[i];
                for (int j = 0; j < report.Rows14.Count; j++)
                {
                    byte aggregateState = report.Rows14[j].AggregateState_DB ?? 0; 

                    if (( aggregateState < 1) || (aggregateState > 3)) continue;  // значение AggregateState_DB может быть только 1,2,3

                    if (!double.TryParse(report.Rows14[j].Mass_DB, out var mass)) continue;

                    if (!inventarizationFlags[aggregateState - 1])
                    {
                        if (report.Rows14[j].OperationCode_DB == "10")
                        {
                            inventarizationFlags[aggregateState - 1] = true;
                            massBalances[aggregateState - 1] = mass;
                        }
                    }
                    else
                    {
                        if (Spravochniks.SignsOperation["1.4"][$"{report.Rows14[j].OperationCode_DB}"] == '+')
                        massBalances[aggregateState - 1] += mass;
                        if (Spravochniks.SignsOperation["1.4"][$"{report.Rows14[j].OperationCode_DB}"] == '-')
                            massBalances[aggregateState - 1] -= mass;
                    }
                }
            }
            return new Tuple<double, double,double>(massBalances[0], massBalances[1], massBalances[2]);
        }
    }
    class Organization
    {
        public int Id { get; set; }

        public string RegNo { get; set; }

        public string Okpo { get; set; }
    }

    class OperationCodeAndMass
    {
        public string OperationCode { get; set; }
        public int Mass { get; set; }
    }
}
