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
using Models.Forms.Form1;
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
                    cts.Token.ThrowIfCancellationRequested();

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
                    
                    if (cts.IsCancellationRequested)
                    {
                        break;
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
                    cts.Token.ThrowIfCancellationRequested();

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

                    if (cts.IsCancellationRequested)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            foreach (Form41 form41 in rep.Rows41)
            {
                cts.Token.ThrowIfCancellationRequested();

                var regNo = form41.RegNo_DB == null ? "_____" : form41.RegNo_DB;
                if (regNo == "77994")
                    ;
                progressBarVM.SetProgressBar((int)currentProgress, $"Проверка организации №{regNo}");

                var error = await CheckPresenceOfForm19(form41, cts.Token, secondDB);
                if (error != null)
                    errorList.Add(error);

                error = await CheckComplianceNumOfInventarizationReports(form41, cts.Token);
                if (error != null)
                    errorList.Add(error);

                error = await CheckComplianceNumWithoutInventarizationReports(form41, cts.Token);
                if (error != null)
                    errorList.Add(error);

                error = await CheckComplianceNumOfReports212(form41, cts.Token, secondDB);
                if (error != null)
                    errorList.Add(error);

                error = await CheckPresenceOfForm11(form41, cts);
                if (error != null)
                    errorList.Add(error);

                error = await CheckPresenceOfForm12(form41, cts);
                if (error != null)
                    errorList.Add(error);

                error = await CheckPresenceOfForm13(form41, cts);
                if (error != null)
                    errorList.Add(error);

                error = await CheckPresenceOfForm14(form41, cts);
                if (error != null)
                    errorList.Add(error);

                currentProgress += incProgress;

                if (cts.IsCancellationRequested)
                {
                    break;
                }
            }

            progressBarVM.SetProgressBar(99, $"Нумеруем строки");

            for (int i = 0; i < errorList.Count; i++)
            {
                cts.Token.ThrowIfCancellationRequested();

                errorList[i].Index = i + 1;

                if (cts.IsCancellationRequested)
                {
                    break;
                }
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
        private static async Task<CheckError?> CheckComplianceNumWithoutInventarizationReports(Form41 form41, CancellationToken cancellationToken)
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
                    .CountAsync(cancellationToken);
                }

            }
            catch (OperationCanceledException)
            {
                throw;
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
                        $"Указано: {form41.NumOfFormsWithoutInventarizationInfo_DB},\n" +
                        $"Найдено в базе данных: {count}"
                    };
            else return null;
        }

        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumOfInventarizationReports(Form41 form41, CancellationToken cancellationToken)
        {

            var dbModel = StaticConfiguration.DBModel;
            int count;
            try
            {
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
                    .CountAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
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
                        $"Указано: {form41.NumOfFormsWithInventarizationInfo_DB},\n" +
                        $"Найдено в базе данных: {count}"
                    };
            else return null;
        }


        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumOfReports212(Form41 form41, CancellationToken cancellationToken, DBModel secondDB = null)
        {
            var dbModel = StaticConfiguration.DBModel;
            if (secondDB == null)
                secondDB = dbModel;
            int count;

            try
            {

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
                        .CountAsync(cancellationToken);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
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
        private static async Task<CheckError?> CheckPresenceOfForm19(Form41 form41, CancellationToken cancellationToken, DBModel secondDB = null)
        {
            string regNo = form41.RegNo.Value;
            string year = form41.Report.Year_DB;
            DBModel dbModel = StaticConfiguration.DBModel;
            if (secondDB == null)
                secondDB = dbModel;
            Reports? organization10 = null;
            Reports? organization20 = null;

            try
            {
                organization20 = await secondDB.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == regNo))
                    .Include(x => x.Report_Collection)
                    .ThenInclude(x => x.Rows212).FirstOrDefaultAsync(cancellationToken);

                //Если отчет 2.12 есть, то Выход
                if ((organization20 != null) && organization20.Report_Collection
                    .Any(report =>
                    report.FormNum_DB == "2.12"
                    && report.Year_DB == year))
                    return null;



                    organization10 = await dbModel.ReportsCollectionDbSet
                        .AsNoTracking()
                        .AsSplitQuery()
                        .AsQueryable()
                        .Include(x => x.DBObservable)
                        .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                        .Where(x => x.Master_DB.Rows10.Any(y => y.RegNo_DB == regNo))
                        .Include(x => x.Report_Collection)
                        .ThenInclude(x => x.Rows19).FirstOrDefaultAsync(cancellationToken);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }

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
        private static async Task<CheckError?> CheckPresenceOfForm11(Form41 form41, CancellationTokenSource cts)
        {
            cts.Token.ThrowIfCancellationRequested();

            int year;
            if (!int.TryParse(form41.Report.Year_DB, out year)) return null;


            var organization = organizations10
                .FirstOrDefault(org =>
                org.RegNo == form41.RegNo_DB
                && org.Okpo == form41.Okpo_DB);

            if (organization == null) return null;

            double quantityBalance = 0;
            bool InventarizationFlag = false;
            var dbModel = StaticConfiguration.DBModel;

            List<Report>? reportCollection = null;
            Report? lastInventarizationReport = null;



            try
            {

                reportCollection = dbModel.ReportsCollectionDbSet
                    .AsNoTracking()
                    .Where(reps => reps.Id == organization.Id)
                    .SelectMany(reps => reps.Report_Collection)
                    .Where(rep => rep.FormNum_DB == "1.1")
                    .Include(rep => rep.Rows11)
                    .AsEnumerable() // Переходим к клиентской обработке
                    .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out var startPeriod)
                    && DateTime.TryParse(rep.EndPeriod_DB, out var endPeriod))
                    .OrderBy(rep => DateTime.Parse(rep.StartPeriod_DB))
                    .ThenBy(rep => DateTime.Parse(rep.EndPeriod_DB))
                    .ToList();


                lastInventarizationReport = reportCollection.LastOrDefault(rep =>
                    rep.Rows11.Any(form11 => form11.OperationCode_DB == "10")
                    && DateTime.Parse(rep.EndPeriod_DB).Year < year);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }



            int startIndex = 0;
            if (lastInventarizationReport != null)
            {
                foreach (Form11 row in lastInventarizationReport.Rows11)
                {
                    cts.Token.ThrowIfCancellationRequested();

                    if (row.OperationCode_DB == "10")
                    {
                        quantityBalance += (double)row.Quantity_DB;
                    }

                    if (cts.IsCancellationRequested)
                    {
                        // Можем либо выбросить исключение, либо просто выйти из цикла
                        break;
                    }
                }
                startIndex = reportCollection.IndexOf(lastInventarizationReport);
            }

            try
            {
                for (int i = startIndex; i < reportCollection.Count; i++)
                {
                    var report = reportCollection[i];
                    for (int j = 0; j < report.Rows11.Count; j++)
                    {
                        cts.Token.ThrowIfCancellationRequested();

                        if (DateTime.Parse(report.EndPeriod_DB).Year == year
                            && report.Rows11[j].OperationCode_DB == "10"
                            && !InventarizationFlag)
                        {
                            InventarizationFlag = true;
                        }
                        else if (report.Rows11[j].Quantity_DB is not null
                            && Spravochniks.SignsOperation["1.1"].ContainsKey($"{report.Rows11[j].OperationCode_DB}"))
                        {
                            if (Spravochniks.SignsOperation["1.1"][$"{report.Rows11[j].OperationCode_DB}"] == '+')
                                quantityBalance += (double)report.Rows11[j].Quantity_DB;
                            else if (Spravochniks.SignsOperation["1.1"][$"{report.Rows11[j].OperationCode_DB}"] == '-')
                                quantityBalance -= (double)report.Rows11[j].Quantity_DB;
                        }
                        if (cts.IsCancellationRequested)
                        {
                            // Можем либо выбросить исключение, либо просто выйти из цикла
                            break;
                        }
                    }
                    if (cts.IsCancellationRequested)
                    {
                        // Можем либо выбросить исключение, либо просто выйти из цикла
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!InventarizationFlag && quantityBalance > 0)
                return new CheckError()
                {
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" на балансе присутствуют ЗРИ. " +
                    $"Необходимо предоставить сведения об инвентаризации по форме 1.1\n" +
                    $"Баланс: {quantityBalance}"
                };
            return null;
        }
        private static async Task<CheckError?> CheckPresenceOfForm12(Form41 form41, CancellationTokenSource cts)
        {
            cts.Token.ThrowIfCancellationRequested();

            int year;
            if (!int.TryParse(form41.Report.Year_DB, out year)) return null;


            var organization = organizations10
                .FirstOrDefault(org => 
                org.RegNo == form41.RegNo_DB 
                && org.Okpo == form41.Okpo_DB);

            if (organization == null) return null;

            double massBalance = 0;
            bool InventarizationFlag = false;
            var dbModel = StaticConfiguration.DBModel;


            var reportCollection = dbModel.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.Id == organization.Id)
                .SelectMany(reps => reps.Report_Collection)
                .Where(rep => rep.FormNum_DB == "1.2")
                .Include(rep => rep.Rows12)
                .AsEnumerable() // Переходим к клиентской обработке
                .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out var startPeriod)
                && DateTime.TryParse(rep.EndPeriod_DB, out var endPeriod))
                .OrderBy(rep => DateTime.Parse(rep.StartPeriod_DB))
                .ThenBy(rep => DateTime.Parse(rep.EndPeriod_DB))
                .ToList();

            var lastInventarizationReport = reportCollection.LastOrDefault(rep =>
                rep.Rows12.Any(form12 => form12.OperationCode_DB == "10")
                && DateTime.Parse(rep.EndPeriod_DB).Year < year);


            int startIndex = 0;
            if (lastInventarizationReport != null)
            {
                foreach (Form12 row in lastInventarizationReport.Rows12)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    if (row.OperationCode_DB == "10")
                    {
                        double.TryParse(row.Mass_DB, out var mass);
                        massBalance += mass;
                    }

                    if (cts.IsCancellationRequested)
                    {
                        // Можем либо выбросить исключение, либо просто выйти из цикла
                        break;
                    }

                }
                startIndex = reportCollection.IndexOf(lastInventarizationReport);
            }

            try
            {
                for (int i = startIndex; i < reportCollection.Count; i++)
                {
                    var report = reportCollection[i];
                    for (int j = 0; j < report.Rows12.Count; j++)
                    {
                        cts.Token.ThrowIfCancellationRequested();

                        if (DateTime.Parse(report.EndPeriod_DB).Year == year 
                            && report.Rows12[j].OperationCode_DB == "10" 
                            && !InventarizationFlag)
                        {
                            InventarizationFlag = true;
                        }
                        else if (double.TryParse(report.Rows12[j].Mass_DB, out var mass)
                            && Spravochniks.SignsOperation["1.2"].ContainsKey($"{report.Rows12[j].OperationCode_DB}"))
                        {
                            if (Spravochniks.SignsOperation["1.2"][$"{report.Rows12[j].OperationCode_DB}"] == '+')
                                massBalance += mass;
                            else if (Spravochniks.SignsOperation["1.2"][$"{report.Rows12[j].OperationCode_DB}"] == '-')
                                massBalance -= mass;
                        }

                        if (cts.IsCancellationRequested)
                        {
                            // Можем либо выбросить исключение, либо просто выйти из цикла
                            break;
                        }
                    }
                    if (cts.IsCancellationRequested)
                    {
                        // Можем либо выбросить исключение, либо просто выйти из цикла
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!InventarizationFlag && massBalance > 0)
                return new CheckError()
                {
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" на балансе присутствуют ИОУ. " +
                    $"Необходимо предоставить сведения об инвентаризации по форме 1.2\n" +
                    $"Баланс: {massBalance}"
                };
            return null;
        }

        private static async Task<CheckError?> CheckPresenceOfForm13(Form41 form41, CancellationTokenSource cts)
        {
            cts.Token.ThrowIfCancellationRequested();

            int year;
            if (!int.TryParse(form41.Report.Year_DB, out year)) return null;


            var organization = organizations10
                .FirstOrDefault(org =>
                org.RegNo == form41.RegNo_DB
                && org.Okpo == form41.Okpo_DB);

            if (organization == null) return null;

            double balance = 0;
            bool InventarizationFlag = false;
            var dbModel = StaticConfiguration.DBModel;

            var reportCollection = dbModel.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.Id == organization.Id)
                .SelectMany(reps => reps.Report_Collection)
                .Where(rep => rep.FormNum_DB == "1.3")
                .Include(rep => rep.Rows13)
                .AsEnumerable() // Переходим к клиентской обработке
                .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out var startPeriod)
                && DateTime.TryParse(rep.EndPeriod_DB, out var endPeriod))
                .OrderBy(rep => DateTime.Parse(rep.StartPeriod_DB))
                .ThenBy(rep => DateTime.Parse(rep.EndPeriod_DB))
                .ToList();

            var lastInventarizationReport = reportCollection.LastOrDefault(rep =>
                rep.Rows13.Any(form13 => form13.OperationCode_DB == "10")
                && DateTime.Parse(rep.EndPeriod_DB).Year < year);


            int startIndex = 0;
            if (lastInventarizationReport != null)
            {
                foreach (Form13 row in lastInventarizationReport.Rows13)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    if (row.OperationCode_DB == "10")
                    {
                        balance += 1;
                    }

                    if (cts.IsCancellationRequested)
                    {
                        // Можем либо выбросить исключение, либо просто выйти из цикла
                        break;
                    }
                }
                startIndex = reportCollection.IndexOf(lastInventarizationReport);
            }

            try
            {
                for (int i = startIndex; i < reportCollection.Count; i++)
                {
                    var report = reportCollection[i];
                    for (int j = 0; j < report.Rows13.Count; j++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        if (DateTime.Parse(report.EndPeriod_DB).Year == year
                            && report.Rows13[j].OperationCode_DB == "10"
                            && !InventarizationFlag)
                        {
                            InventarizationFlag = true;
                        }
                        else if (Spravochniks.SignsOperation["1.3"].ContainsKey($"{report.Rows13[j].OperationCode_DB}"))
                        {
                            if (Spravochniks.SignsOperation["1.3"][$"{report.Rows13[j].OperationCode_DB}"] == '+')
                                balance += 1;
                            else if (Spravochniks.SignsOperation["1.3"][$"{report.Rows13[j].OperationCode_DB}"] == '-')
                                balance -= 1;
                        }
                        if (cts.IsCancellationRequested)
                        {
                            // Можем либо выбросить исключение, либо просто выйти из цикла
                            break;
                        }
                    }
                    if (cts.IsCancellationRequested)
                    {
                        // Можем либо выбросить исключение, либо просто выйти из цикла
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!InventarizationFlag && balance > 0)
                return new CheckError()
                {
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" на балансе присутствуют ОРИ. " +
                    $"Необходимо предоставить сведения об инвентаризации по форме 1.3\n" +
                    $"Баланс: {balance}"
                };
            return null;
        }

        private static async Task<CheckError?> CheckPresenceOfForm14(Form41 form41, CancellationTokenSource cts)
        {
            cts.Token.ThrowIfCancellationRequested();

            int year;
            if (!int.TryParse(form41.Report.Year_DB, out year)) return null;

            var organization = organizations10
                .FirstOrDefault(org =>
                org.RegNo == form41.RegNo_DB
                && org.Okpo == form41.Okpo_DB);

            if (organization == null) return null;

            double massBalanceLiquid = 0;   //1
            double massBalanceSolid = 0;    //2
            double massBalanceGas = 0;      //3

            bool InventarizationFlag = false;

            var dbModel = StaticConfiguration.DBModel;

            var reportCollection = dbModel.ReportsCollectionDbSet
                .AsNoTracking()
                .Where(reps => reps.Id == organization.Id)
                .SelectMany(reps => reps.Report_Collection)
                .Where(rep => rep.FormNum_DB == "1.4")
                .Include(rep => rep.Rows14)
                .AsEnumerable() // Переходим к клиентской обработке
                .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out var startPeriod)
                && DateTime.TryParse(rep.EndPeriod_DB, out var endPeriod))
                .OrderBy(rep => DateTime.Parse(rep.StartPeriod_DB))
                .ThenBy(rep => DateTime.Parse(rep.EndPeriod_DB))
                .ToList();

            var lastInventarizationReport = reportCollection.LastOrDefault(rep =>
                rep.Rows14.Any(form14 => form14.OperationCode_DB == "10")
                && DateTime.Parse(rep.EndPeriod_DB).Year < year);


            int startIndex = 0;
            if (lastInventarizationReport != null)
            {
                foreach (Form14 row in lastInventarizationReport.Rows14)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    if (row.OperationCode_DB == "10")
                    {
                        double.TryParse(row.Mass_DB, out var mass);
                        switch(row.AggregateState_DB)
                        {
                            case 1:
                                massBalanceLiquid += mass;
                                break;
                            case 2:
                                massBalanceSolid += mass;
                                break;
                            case 3:
                                massBalanceGas += mass;
                                break;
                        }
                    }

                    if (cts.IsCancellationRequested)
                    {
                        // Можем либо выбросить исключение, либо просто выйти из цикла
                        break;
                    }
                }
                startIndex = reportCollection.IndexOf(lastInventarizationReport);
            }

            try
            {
                for (int i = startIndex; i < reportCollection.Count; i++)
                {
                    var report = reportCollection[i];
                    for (int j = 0; j < report.Rows14.Count; j++)
                    {
                        cts.Token.ThrowIfCancellationRequested();
                        if (DateTime.Parse(report.EndPeriod_DB).Year == year
                            && report.Rows14[j].OperationCode_DB == "10"
                            && !InventarizationFlag)
                        {
                            InventarizationFlag = true;
                        }
                        else if (double.TryParse(report.Rows14[j].Mass_DB, out var mass)
                            && Spravochniks.SignsOperation["1.4"].ContainsKey($"{report.Rows14[j].OperationCode_DB}"))
                        {
                            if (Spravochniks.SignsOperation["1.4"][$"{report.Rows14[j].OperationCode_DB}"] == '+')
                                switch (report.Rows14[j].AggregateState_DB)
                                {
                                    case 1:
                                        massBalanceLiquid += mass;
                                        break;
                                    case 2:
                                        massBalanceSolid += mass;
                                        break;
                                    case 3:
                                        massBalanceGas += mass;
                                        break;
                                }
                            else if (Spravochniks.SignsOperation["1.4"][$"{report.Rows14[j].OperationCode_DB}"] == '-')
                                switch (report.Rows14[j].AggregateState_DB)
                                {
                                    case 1:
                                        massBalanceLiquid -= mass;
                                        break;
                                    case 2:
                                        massBalanceSolid -= mass;
                                        break;
                                    case 3:
                                        massBalanceGas -= mass;
                                        break;
                                }
                        }
                        if (cts.IsCancellationRequested)
                        {
                            // Можем либо выбросить исключение, либо просто выйти из цикла
                            break;
                        }
                    }
                    if (cts.IsCancellationRequested)
                    {
                        // Можем либо выбросить исключение, либо просто выйти из цикла
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!InventarizationFlag && (massBalanceLiquid > 0 || massBalanceSolid > 0 || massBalanceGas > 0))
                return new CheckError()
                {
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" на балансе присутствуют радионуклиды. " +
                    $"Необходимо проинвентаризировать по форме 1.4\n" +
                    $"Баланс жидких РВ: {massBalanceLiquid}\n" +
                    $"Баланс твердых РВ: {massBalanceSolid}\n" +
                    $"Баланс газооборазных РВ: {massBalanceGas}\n"
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
