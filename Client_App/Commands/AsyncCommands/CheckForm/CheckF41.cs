using Avalonia.Threading;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.ProgressBar;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form4;
using OfficeOpenXml.Drawing.Controls;
using System;
using System.Collections.Concurrent;
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
        public override bool CanExecute(object? parameter) => true;

        #region AsyncExecute
        public override async Task<List<CheckError>> AsyncExecute(object? parameter)
        {
            return await Check_Total(parameter);
        }
        #endregion
        public static async Task<List<CheckError>> Check_Total(object? parameter)
        {
            

            var cts = new CancellationTokenSource();
            List<CheckError> errorList = [];
            var progressBar = await Dispatcher.UIThread.InvokeAsync(() => new AnyTaskProgressBar(cts));
            var progressBarVM = progressBar.AnyTaskProgressBarVM;
            var rep = parameter as Report;
            if (rep is null) await CancelCommandAndCloseProgressBarWindow(cts, progressBar);


            var processedCount = 0;
            var totalCount = rep.Rows41.Count();

            var errorBag = new ConcurrentBag<CheckError>();
            var progressLock = new object();
            var semaphore = new SemaphoreSlim(Environment.ProcessorCount * 2); // Ограничение: 4 параллельных задачи

            var tasks = rep.Rows41.Select(async form41 =>
            {

                await semaphore.WaitAsync();
                try
                {
                    form41.Report = rep;
                    var regNo = form41.RegNo_DB ?? "_____";


                    // Асинхронные проверки
                    var errors = new List<CheckError>();
                    var error1 = await CheckPresenceOfForm19(form41);
                    if (error1 != null) errors.Add(error1);

                    var error2 = await CheckComplianceNumOfInventarizationReports(form41);
                    if (error2 != null) errors.Add(error2);

                    var error3 = await CheckComplianceNumWithoutInventarizationReports(form41);
                    if (error3 != null) errors.Add(error3);

                    var error4 = await CheckComplianceNumOfReports212(form41);
                    if (error4 != null) errors.Add(error4);

                    foreach (var error in errors)
                        errorBag.Add(error);

                    int currentProcessed;
                    lock (progressLock)
                    {
                        processedCount++;
                        currentProcessed = processedCount;
                    }

                    // Вычисляем прогресс на основе количества обработанных элементов
                    var progress = (int)((double)currentProcessed / totalCount * 100);

                    // Обновляем прогресс-бар
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        progressBarVM.SetProgressBar(progress, $"Проверка организации №{regNo} ({currentProcessed}/{totalCount})");
                    });
                }
                finally
                {
                    semaphore.Release();
                }
            });


            await Task.WhenAll(tasks);
            errorList = errorBag.ToList();

            progressBar.Close();

            return errorList;
        }

        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumWithoutInventarizationReports(Form41 form41)
        {
            string regNo = form41.RegNo.Value;
            string year = form41.Report.Year.Value;
            DBModel dbModel = StaticConfiguration.DBModel;

            try
            {
                var numOfFormsWithoutInventarization = 0;
                var organization10 = dbModel.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Where(x => x.Master_DB.Rows10.Any(y => y.RegNo_DB == regNo))
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows12)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows13)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows14)
                    .FirstOrDefault();

                if (organization10 != null)
                {
                    numOfFormsWithoutInventarization = organization10.Report_Collection
                        .Where(report =>
                        report.Year_DB == year
                        && ((report.FormNum_DB == "1.1" && report.Rows11.All(x => x.OperationCode_DB != "10"))
                        || (report.FormNum_DB == "1.2" && report.Rows12.All(x => x.OperationCode_DB != "10"))
                        || (report.FormNum_DB == "1.3" && report.Rows13.All(x => x.OperationCode_DB != "10"))
                        || (report.FormNum_DB == "1.4" && report.Rows14.All(x => x.OperationCode_DB != "10"))
                        ))
                        .Count();
                }

                if (numOfFormsWithoutInventarization != form41.NumOfFormsWithoutInventarizationInfo_DB)
                    return
                        new CheckError()
                        {
                            FormNum = "form_41",
                            Row = $"{form41.NumberInOrder_DB}",
                            Column = $"-",
                            Value = $"-",
                            Message = $"У организации №{regNo} указанное количество отчетов 1.1-1.4 без инвентаризации не соответствует имеющимся в базе данных"
                        };
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumOfInventarizationReports(Form41 form41)
        {
            string regNo = form41.RegNo.Value;
            string year = form41.Report.Year.Value;
            DBModel dbModel = StaticConfiguration.DBModel;

            try
            {
                var numOfFormsWithInventarization = 0;
                var organization10 = dbModel.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Where(x => x.Master_DB.Rows10.Any(y => y.RegNo_DB == regNo))
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows12)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows13)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows14)
                    .FirstOrDefault();

                if (organization10 != null)
                {
                    numOfFormsWithInventarization = organization10.Report_Collection
                        .Where(report =>
                        report.Year_DB == year
                        && ((report.FormNum_DB == "1.1" && report.Rows11.Any(x => x.OperationCode_DB == "10"))
                        || (report.FormNum_DB == "1.2" && report.Rows12.Any(x => x.OperationCode_DB == "10"))
                        || (report.FormNum_DB == "1.3" && report.Rows13.Any(x => x.OperationCode_DB == "10"))
                        || (report.FormNum_DB == "1.4" && report.Rows14.Any(x => x.OperationCode_DB == "10"))
                        ))
                        .Count();
                }

                if (numOfFormsWithInventarization != form41.NumOfFormsWithInventarizationInfo_DB)
                    return
                        new CheckError()
                        {
                            FormNum = "form_41",
                            Row = $"{form41.NumberInOrder_DB}",
                            Column = $"-",
                            Value = $"-",
                            Message = $"У организации №{regNo} указанное количество инвентаризационных отчетов 1.1-1.4 не соответствует имеющимся в базе данных"
                        };
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumOfReports212(Form41 form41)
        {
            string regNo = form41.RegNo.Value;
            string year = form41.Report.Year.Value;
            DBModel dbModel = StaticConfiguration.DBModel;

            try
            {
                var numOfForms212 = 0;
                var organization20 = dbModel.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB == regNo))
                    .Include(x => x.Report_Collection)
                    .ThenInclude(x => x.Rows212).FirstOrDefault();

                if (organization20 != null) 
                {
                    numOfForms212 = organization20.Report_Collection
                        .Where(report =>
                        report.FormNum_DB == "2.12"
                        && report.Year_DB == year)
                        .Count();
                }

                if (numOfForms212 != form41.NumOfForms212_DB)
                    return
                        new CheckError()
                        {
                            FormNum = "form_41",
                            Row = $"{form41.NumberInOrder_DB}",
                            Column = $"-",
                            Value = $"-",
                            Message = $"У организации №{regNo} указанное количество отчетов по форме по 2.12 не соответствует имеющимся в базе данных"
                        };
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Если нет отчета 2.12 – проверяется наличие ф.1.9. Если форма 1.9 есть – ошибка «Должен быть отчет по форме 2.12».
        /// Если нет отчета по 2.12 и 1.9, но в предыдущем отчете есть 2.12 – предупреждение «Проверьте организацию на необходимость представления отчета по ф.2.12
        /// </summary>
        /// <returns></returns>
        private static async Task<CheckError?> CheckPresenceOfForm19(Form41 form41)
        {
            string regNo = form41.RegNo.Value;
            string year = form41.Report.Year_DB;
            DBModel dbModel = StaticConfiguration.DBModel;

            try
            {
                var organization20 = dbModel.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(x => x.DBObservable)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Where(x => x.Master_DB.Rows20.Any(y => y.RegNo_DB != null && y.RegNo_DB == regNo))
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
                            Message = $"У организации №{regNo} Должен быть отчет по форме 2.12."
                        };

                if ((organization20 != null) && organization20.Report_Collection
                    .Any(report =>
                    report.FormNum_DB == "2.12"
                    && int.TryParse(year, out var intYear)
                    && report.Year_DB == (intYear-1).ToString())) //проверка на предыдущий отчет
                    return new CheckError()
                    {
                        FormNum = "form_41",
                        Row = $"{form41.NumberInOrder_DB}",
                        Column = $"-",
                        Value = $"-",
                        Message = $"Проверьте организацию №{regNo} на необходимость представления отчета по форме 2.12"
                    };

                return null ;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private static async Task CancelCommandAndCloseProgressBarWindow(CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null)
        {
            await cts.CancelAsync();
            if (progressBar is not null) await progressBar.CloseAsync();
            cts.Token.ThrowIfCancellationRequested();
        }
    }
}
