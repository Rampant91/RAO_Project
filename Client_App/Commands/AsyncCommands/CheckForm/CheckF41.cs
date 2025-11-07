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

            double currentProgress = 5;
            double incProgress = (100.0 - currentProgress) / rep.Rows41.Count();

            foreach (Form41 form41 in rep.Rows41)
            {
                var regNo = form41.RegNo_DB == null ? "_____" : form41.RegNo_DB;
                progressBarVM.SetProgressBar((int)currentProgress, $"Проверка организации №{regNo}");
                var error = await CheckPresenceOfForm19(form41);
                if (error != null)
                    errorList.Add(error);

                //error = await CheckComplianceNumOfInventarizationReports(form41);
                //if (error != null)
                //    errorList.Add(error);

                error = await CheckComplianceNumWithoutInventarizationReports(form41);
                if (error != null)
                    errorList.Add(error);

                //error = await CheckComplianceNumOfReports212(form41);
                //if (error != null)
                //    errorList.Add(error);

                currentProgress += incProgress;
            }
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
            int count;
            try
            {
                count = await dbModel.ReportsCollectionDbSet
                .AsNoTracking()
                .Include(x => x.DBObservable)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows12)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows14)
                .Where(reps => reps.DBObservable != null
                && reps.Master_DB != null
                && reps.Master_DB.Rows10.Count >0
                && reps.Master_DB.Rows10[0].RegNo_DB == form41.RegNo_DB)
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
            catch( Exception ex)
            {
                throw ex;
            }

            if (count != form41.NumOfFormsWithoutInventarizationInfo_DB)
                    return
                        new CheckError()
                        {
                            FormNum = "form_41",
                            Row = $"{form41.NumberInOrder_DB}",
                            Column = $"-",
                            Value = $"-",
                            Message = $"У организации №{form41.RegNo_DB} указанное количество отчетов 1.1-1.4 без инвентаризации не соответствует имеющимся в базе данных"
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
            var count =  await dbModel.ReportsCollectionDbSet
            .AsNoTracking()
            .AsSplitQuery()
            .AsQueryable()
            .Include(x => x.DBObservable)
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows12)
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
            .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows14)
            .Where(reps => reps.DBObservable != null 
            && reps.Master.RegNoRep != null
            && reps.Master.RegNoRep.Value == form41.RegNo_DB)
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

            if (count != form41.NumOfFormsWithInventarizationInfo_DB)
                return
                    new CheckError()
                    {
                        FormNum = "form_41",
                        Row = $"{form41.NumberInOrder_DB}",
                        Column = $"-",
                        Value = $"-",
                        Message = $"У организации №{form41.RegNo_DB} указанное количество инвентаризационных отчетов 1.1-1.4 не соответствует имеющимся в базе данных"
                    };
            else return null;
        }


        /// <summary>
        /// Проверка соответствия количества отчетов имеющемуся в БД
        /// </summary>
        /// <param name="form41"></param>
        /// <returns></returns>
        private static async Task<CheckError?> CheckComplianceNumOfReports212(Form41 form41)
        {
            var dbModel = StaticConfiguration.DBModel;
            var count = await dbModel.ReportCollectionDbSet
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(report => report.Reports)
                    .Where(report => report.RegNoRep != null && report.RegNoRep.Value == form41.RegNo_DB)
                    .Where(report => report.Year_DB == form41.Report.Year_DB)
                    .Where(report => report.FormNum_DB == "2.12")
                    .CountAsync();

            if (count != form41.NumOfForms212_DB)
                    return
                        new CheckError()
                        {
                            FormNum = "form_41",
                            Row = $"{form41.NumberInOrder_DB}",
                            Column = $"-",
                            Value = $"-",
                            Message = $"У организации №{form41.RegNo_DB} указанное количество отчетов по форме по 2.12 не соответствует имеющимся в базе данных"
                        };
                else return null;
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

                var organization20 = dbModel.ReportsCollectionDbSet
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
                            Message = $"У организации №{regNo} Должен быть отчет по форме 2.12."
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
        private static async Task CancelCommandAndCloseProgressBarWindow(CancellationTokenSource cts, AnyTaskProgressBar? progressBar = null)
        {
            await cts.CancelAsync();
            if (progressBar is not null) await progressBar.CloseAsync();
            cts.Token.ThrowIfCancellationRequested();
        }
    }
}