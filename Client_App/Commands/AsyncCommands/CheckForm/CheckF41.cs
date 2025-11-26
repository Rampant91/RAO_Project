using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Views.Forms.Forms4;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
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
                            reportsQuery = secondDB.ReportsCollectionDbSet
                             .AsSplitQuery()
                             .AsQueryable()
                             .Include(reps => reps.Master_DB).ThenInclude(rep => rep.Rows20)
                             .Where(reps => reps.Master_DB.FormNum_DB == "2.0");
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

                currentProgress += incProgress;
            }


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
                        && y.CorrectionNumber_DB == 0
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
                        Message = $"У организации №{form41.RegNo_DB} указано {form41.NumOfFormsWithoutInventarizationInfo_DB} отчетов 1.1-1.4 без инвентаризации, когда в БД их {count}"
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
                    && y.CorrectionNumber_DB == 0
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
                        Message = $"У организации №{form41.RegNo_DB} указано {form41.NumOfFormsWithInventarizationInfo_DB} инвентаризационных отчетов 1.1-1.4,когда в БД их {count}"
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
                    .Where(report => report.CorrectionNumber_DB == 0)
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
                        Message = $"У организации №{form41.RegNo_DB} указано {form41.NumOfForms212_DB} количество отчетов по форме по 2.12, когда в БД их {count}"
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
                    ContentMessage = "Не удалось найти годовые отчеты\n" +
                    "Вы хотите указать путь на базу данных с годовыми отчетами?",
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
}
