using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.CheckForm;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form1;
using Models.Forms.Form4;
using Spravochniki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Models.Helpers;

namespace Client_App.Commands.AsyncCommands.CheckForm;

public abstract class CheckF41 : CheckBase
{
    static List<Organization> organizations10 = [];
    static List<Organization> organizations20 = [];
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
        var incProgress = (100.0 - currentProgress) / rep!.Rows41.Count;

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
                    (form41.RegNo_DB == reports.Master_DB.Rows10[0].RegNo_DB
                    || form41.RegNo_DB == reports.Master_DB.Rows10[1].RegNo_DB )
                    && (form41.Okpo_DB == reports.Master_DB.Rows10[0].Okpo_DB
                    || form41.Okpo_DB == reports.Master_DB.Rows10[1].Okpo_DB)))
                {
                    var okpo = DashStringHelper.IsNullOrEmptyOrDash(reports.Master_DB.Rows10[1].Okpo_DB)
                        ? reports.Master_DB.Rows10[0].Okpo_DB
                        : reports.Master_DB.Rows10[1].Okpo_DB;

                    string regNo;
                    if ((reports.Master_DB.Rows10[1].RegNo.Value != "" || DashStringHelper.IsDash(reports.Master_DB.Rows10[1].Okpo_DB)) && reports.Master_DB.Rows10[1].Okpo.Value != "")
                        regNo = reports.Master_DB.Rows10[1].RegNo_DB;
                    else
                        regNo = reports.Master_DB.Rows10[0].RegNo_DB;

                    organizations10.Add(new Organization()
                    {
                        Id = reports.Id,
                        RegNo = regNo,
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

            if (!reportsQuery.Any())
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
                                    ContentMessage = "Произошла ошибка:" +
                                                     $"{Environment.NewLine}Не удалось открыть базу данных с годовыми отчетами",
                                    MinWidth = 300,
                                    MinHeight = 125,
                                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                                    Topmost = true,
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
                     (form41.RegNo_DB == reports.Master_DB.Rows20[0].RegNo_DB
                     || form41.RegNo_DB == reports.Master_DB.Rows20[1].RegNo_DB)
                     && (form41.Okpo_DB == reports.Master_DB.Rows20[0].Okpo_DB
                     || form41.Okpo_DB == reports.Master_DB.Rows20[1].Okpo_DB)))
                {
                    var okpo = DashStringHelper.IsNullOrEmptyOrDash(reports.Master_DB.Rows20[1].Okpo_DB)
                        ? reports.Master_DB.Rows20[0].Okpo_DB
                        : reports.Master_DB.Rows20[1].Okpo_DB;

                    string regNo;
                    if ((reports.Master_DB.Rows20[1].RegNo.Value != "" || DashStringHelper.IsDash(reports.Master_DB.Rows20[1].Okpo_DB)) && reports.Master_DB.Rows20[1].Okpo.Value != "")
                        regNo = reports.Master_DB.Rows20[1].RegNo_DB;
                    else
                        regNo = reports.Master_DB.Rows20[0].RegNo_DB;

                    organizations20.Add(new Organization()
                    {
                        Id = reports.Id,
                        RegNo = regNo,
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

        foreach (var key in rep.Rows41)
        {
            var form41 = (Form41)key;
            cts.Token.ThrowIfCancellationRequested();

            var regNo = form41.RegNo_DB ?? "_____";

            progressBarVM.SetProgressBar((int)currentProgress, $"Проверка организации №{regNo}");

            var error = await CheckPresenceOfForm19(form41, cts.Token, secondDB);
            if (error != null)
                errorList.Add(error);

            error = await CheckComplianceNumOfInventoryReports(form41, cts.Token);
            if (error != null)
                errorList.Add(error);

            error = await CheckComplianceNumWithoutInventoryReports(form41, cts.Token);
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

        progressBarVM.SetProgressBar(99, "Нумеруем строки");

        for (var i = 0; i < errorList.Count; i++)
        {
            cts.Token.ThrowIfCancellationRequested();

            errorList[i].Index = i + 1;

            if (cts.IsCancellationRequested)
            {
                break;
            }
        }
        progressBarVM.SetProgressBar(100, "Проверка выполнена успешно");
        //Освобождаем память
        if (secondDB != null)
            await secondDB.DisposeAsync();

        await progressBar.CloseAsync();
        return errorList;
    }

    /// <summary>
    /// Проверка соответствия количества отчетов имеющемуся в БД
    /// </summary>
    /// <param name="form41">Форма 4.1.</param>
    /// <param name="token">Токен.</param>
    /// <returns></returns>
    private static async Task<CheckError?> CheckComplianceNumWithoutInventoryReports(Form41 form41, CancellationToken token)
    {
        var dbModel = StaticConfiguration.DBModel;
        var count = 0;
        try
        {
            var organization = organizations10.FirstOrDefault(org =>
                org.RegNo == form41.RegNo_DB
                && (org.Okpo == form41.Okpo_DB));

            if (organization != null)
            {
                var reportYearText = form41.Report.Year_DB?.ToString() ?? "";
                count = await dbModel.ReportsCollectionDbSet
                    .AsNoTracking()
                    .Include(x => x.DBObservable)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows12)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
                    .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows14)
                    .Where(reps => reps.DBObservable != null && reps.Id == organization.Id)
                    .SelectMany(x => x.Report_Collection
                        .Where(y => y.EndPeriod_DB.EndsWith(reportYearText)
                                    &&
                                    (
                                        y.FormNum_DB == "1.1" && y.Rows11.All(form => form.OperationCode_DB != "10")
                                        || y.FormNum_DB == "1.2" && y.Rows12.All(form => form.OperationCode_DB != "10")
                                        || y.FormNum_DB == "1.3" && y.Rows13.All(form => form.OperationCode_DB != "10")
                                        || y.FormNum_DB == "1.4" && y.Rows14.All(form => form.OperationCode_DB != "10")
                                    )))
                    .CountAsync(token);
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
                    Column = "7",
                    RegNo = form41.RegNo_DB,
                    Okpo = form41.Okpo_DB,
                    Value = $"{form41.NumOfFormsWithoutInventarizationInfo_DB}",
                    DbValue = $"{count}",
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\"\n" +
                    $"Наименование -\"{form41.OrganizationName_DB}\"\n" +
                    $"Не совпадает количество отчётов 1.1 - 1.4 без инвентаризации:" +
                              $"{Environment.NewLine}Указано: {form41.NumOfFormsWithoutInventarizationInfo_DB}," +
                              $"{Environment.NewLine}Найдено в базе данных: {count}"
                };
        else return null;
    }

    /// <summary>
    /// Проверка соответствия количества отчетов имеющемуся в БД
    /// </summary>
    /// <param name="form41">Форма 4.1.</param>
    /// <param name="token">Токен.</param>
    /// <returns></returns>
    private static async Task<CheckError?> CheckComplianceNumOfInventoryReports(Form41 form41, CancellationToken token)
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
                var year = form41.Report!.Year_DB?.ToString() ?? "";

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
                        .Where(y => y.EndPeriod_DB.EndsWith(year) 
                                    && 
                                    (
                                        y.FormNum_DB == "1.1" && y.Rows11.Any(form => form.OperationCode_DB == "10")
                                        || y.FormNum_DB == "1.2" && y.Rows12.Any(form => form.OperationCode_DB == "10")
                                        || y.FormNum_DB == "1.3" && y.Rows13.Any(form => form.OperationCode_DB == "10")
                                        || y.FormNum_DB == "1.4" && y.Rows14.Any(form => form.OperationCode_DB == "10")
                                    )))
                    .CountAsync(token);
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
                new CheckError
                {
                    FormNum = "form_41",
                    Row = $"{form41.NumberInOrder_DB}",
                    Column = "6",
                    RegNo = form41.RegNo_DB,
                    Okpo = form41.Okpo_DB,
                    Value = $"{form41.NumOfFormsWithInventarizationInfo_DB}",
                    DbValue = $"{count}",
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" \n" +
                              $"Наименование -\"{form41.OrganizationName_DB}\"\n" +
                              $"Не совпадает количество инвентаризационных отчётов 1.1-1.4:" + 
                              $"{Environment.NewLine}Указано: {form41.NumOfFormsWithInventarizationInfo_DB}," + 
                              $"{Environment.NewLine}Найдено в базе данных: {count}"
                };
        else return null;
    }


    /// <summary>
    /// Проверка соответствия количества отчетов имеющемуся в БД
    /// </summary>
    /// <param name="form41"></param>
    /// <returns></returns>
    private static async Task<CheckError?> CheckComplianceNumOfReports212(Form41 form41, CancellationToken cancellationToken, DBModel? secondDB = null)
    {
        var dbModel = StaticConfiguration.DBModel;
        secondDB ??= dbModel;
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
                new CheckError
                {
                    FormNum = "form_41",
                    Row = $"{form41.NumberInOrder_DB}",
                    Column = $"8",
                    RegNo = form41.RegNo_DB,
                    Okpo = form41.Okpo_DB,
                    Value = $"{form41.NumOfForms212_DB}",
                    DbValue = $"{count}",
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\"\n" +
                    $"Наименование -\"{form41.OrganizationName_DB}\"\n" +
                    $"Не совпадает количество отчётов 2.12:" +
                              $"{Environment.NewLine}Указано: {form41.NumOfForms212_DB}," +
                              $"{Environment.NewLine}Найдено в базе данных: {count}"
                };
        else return null;
    }
    /// <summary>
    /// Если нет отчета 2.12 – проверяется наличие ф.1.9. Если форма 1.9 есть – ошибка «Должен быть отчет по форме 2.12».
    /// Если нет отчета по 2.12 и 1.9, но в предыдущем отчете есть 2.12 – предупреждение «Проверьте организацию на необходимость представления отчета по ф.2.12
    /// </summary>
    /// <returns></returns>
    private static async Task<CheckError?> CheckPresenceOfForm19(Form41 form41, CancellationToken cancellationToken, DBModel? secondDB = null)
    {
        var regNo = form41.RegNo.Value;
        var year = form41.Report.Year_DB;
        var dbModel = StaticConfiguration.DBModel;
        secondDB ??= dbModel;
        Reports? organization10;
        Reports? organization20;

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
            if ((organization20 != null) && organization20.Report_Collection.Any(report => 
                    report.FormNum_DB == "2.12" && report.Year_DB == year))
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
                    && dateOnly.Year == year))
            return
                new CheckError
                {
                    FormNum = "form_41",
                    Row = $"{form41.NumberInOrder_DB}",
                    RegNo = form41.RegNo_DB,
                    Okpo = form41.Okpo_DB,
                    Column = "-",
                    Value = "-",
                    Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" \n" +
                    $"Наименование -\"{form41.OrganizationName_DB}\"\n" +
                    $"Должен быть отчет по форме 2.12, потому что у нее есть отчет по форме 1.9"
                };

        if ((organization20 != null) && year is { } reportYear && organization20.Report_Collection.Any(report => 
                report.FormNum_DB == "2.12" 
                && report.Year_DB == reportYear - 1)) //проверка на предыдущий отчет
            return new CheckError
            {
                FormNum = "form_41",
                Row = $"{form41.NumberInOrder_DB}",
                Column = "-",
                RegNo = form41.RegNo_DB,
                Okpo = form41.Okpo_DB,
                Value = "-",
                Message = $"Проверьте организацию №{regNo} на необходимость представления отчета по форме 2.12"
            };

        return null;

    }
    private static async Task<CheckError?> CheckPresenceOfForm11(Form41 form41, CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        if (!form41.Report.Year_DB.HasValue) return null;
        var year = form41.Report.Year_DB.Value;


        var organization = organizations10.FirstOrDefault(org => 
            org.RegNo == form41.RegNo_DB && org.Okpo == form41.Okpo_DB);

        if (organization == null) return null;

        var quantityBalance = 0;
        var inventoryFlag = false;
        var dbModel = StaticConfiguration.DBModel;

        //List<Report>? reportCollection = null;
        //Report? lastInventoryReport = null;

        var reportCollection = dbModel.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == organization.Id)
            .SelectMany(reps => reps.Report_Collection)
            .Where(rep => rep.FormNum_DB == "1.1")
            .Include(rep => rep.Rows11)
            .AsEnumerable() // Переходим к клиентской обработке
            .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out _)
                          && DateTime.TryParse(rep.EndPeriod_DB, out _))
            .OrderBy(rep => DateTime.Parse(rep.StartPeriod_DB))
            .ThenBy(rep => DateTime.Parse(rep.EndPeriod_DB))
            .ToList();

        var lastInventoryReport = reportCollection.LastOrDefault(rep =>
            rep.Rows11.Any(form11 => form11.OperationCode_DB == "10") 
            && DateTime.Parse(rep.EndPeriod_DB).Year < year);

        var startIndex = 0;
        if (lastInventoryReport != null)
        {
            foreach (var key in lastInventoryReport.Rows11)
            {
                var row = (Form11)key;
                cts.Token.ThrowIfCancellationRequested();

                if (row.OperationCode_DB == "10")
                {
                    quantityBalance += row.Quantity_DB ?? 1;
                }

                if (cts.IsCancellationRequested)
                {
                    // Можем либо выбросить исключение, либо просто выйти из цикла
                    break;
                }
            }
            startIndex = reportCollection.IndexOf(lastInventoryReport);
        }

        try
        {
            for (var i = startIndex; i < reportCollection.Count; i++)
            {
                var report = reportCollection[i];
                for (var j = 0; j < report.Rows11.Count; j++)
                {
                    cts.Token.ThrowIfCancellationRequested();

                    if (DateTime.Parse(report.EndPeriod_DB).Year == year
                        && report.Rows11[j].OperationCode_DB == "10"
                        && !inventoryFlag)
                    {
                        inventoryFlag = true;
                    }
                    else if (report.Rows11[j].Quantity_DB is not null
                             && Spravochniks.SignsOperation["1.1"].ContainsKey($"{report.Rows11[j].OperationCode_DB}"))
                    {
                        switch (Spravochniks.SignsOperation["1.1"][$"{report.Rows11[j].OperationCode_DB}"])
                        {
                            case '+':
                                quantityBalance += report.Rows11[j].Quantity_DB != null
                                    ? (int)report.Rows11[j].Quantity_DB!
                                    : 1;
                                break;
                            case '-':
                                quantityBalance -= report.Rows11[j].Quantity_DB != null
                                    ? (int)report.Rows11[j].Quantity_DB!
                                    : 1;
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

        if (!inventoryFlag && quantityBalance > 0)
            return new CheckError()
            {
                Row = $"{form41.NumberInOrder_DB}",
                RegNo = form41.RegNo_DB,
                Okpo = form41.Okpo_DB,
                DbValue = $"{quantityBalance}",
                Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\" \n" +
                          $"Наименование -\"{form41.OrganizationName_DB}\"\n" +
                          $"На балансе присутствуют ЗРИ. " +
                          $"Необходимо предоставить сведения об инвентаризации по форме 1.1" +
                          $"{Environment.NewLine}Баланс: {quantityBalance}"
            };
        return null;
    }
    private static async Task<CheckError?> CheckPresenceOfForm12(Form41 form41, CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        if (!form41.Report.Year_DB.HasValue) return null;
        var year = form41.Report.Year_DB.Value;

        var organization = organizations10.FirstOrDefault(org => 
            org.RegNo == form41.RegNo_DB && org.Okpo == form41.Okpo_DB);

        if (organization == null) return null;

        var balance = 0;
        var inventoryFlag = false;
        var dbModel = StaticConfiguration.DBModel;

        var reportCollection = dbModel.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == organization.Id)
            .SelectMany(reps => reps.Report_Collection)
            .Where(rep => rep.FormNum_DB == "1.2")
            .Include(rep => rep.Rows12)
            .AsEnumerable() // Переходим к клиентской обработке
            .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out _)
                          && DateTime.TryParse(rep.EndPeriod_DB, out _))
            .OrderBy(rep => DateTime.Parse(rep.StartPeriod_DB))
            .ThenBy(rep => DateTime.Parse(rep.EndPeriod_DB))
            .ToList();

        var lastInventoryReport = reportCollection.LastOrDefault(rep =>
            rep.Rows12.Any(form12 => form12.OperationCode_DB == "10")
            && DateTime.Parse(rep.EndPeriod_DB).Year < year);

        var startIndex = 0;
        if (lastInventoryReport != null)
        {
            foreach (var key in lastInventoryReport.Rows12)
            {
                var row = (Form12)key;
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
            startIndex = reportCollection.IndexOf(lastInventoryReport);
        }

        try
        {
            for (var i = startIndex; i < reportCollection.Count; i++)
            {
                var report = reportCollection[i];
                for (var j = 0; j < report.Rows12.Count; j++)
                {
                    cts.Token.ThrowIfCancellationRequested();

                    if (DateTime.Parse(report.EndPeriod_DB).Year == year 
                        && report.Rows12[j].OperationCode_DB == "10" 
                        && !inventoryFlag)
                    {
                        inventoryFlag = true;
                    }
                    else if (Spravochniks.SignsOperation["1.2"].ContainsKey($"{report.Rows12[j].OperationCode_DB}"))
                    {
                        switch (Spravochniks.SignsOperation["1.2"][$"{report.Rows12[j].OperationCode_DB}"])
                        {
                            case '+':
                                balance += 1;
                                break;
                            case '-':
                                balance -= 1;
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

        if (!inventoryFlag && balance > 0)
            return new CheckError()
            {
                Row = $"{form41.NumberInOrder_DB}",
                RegNo = form41.RegNo_DB,
                Okpo = form41.Okpo_DB,
                DbValue = $"{balance}",
                Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\"\n" +
                          $"Наименование -\"{form41.OrganizationName_DB}\"\n" +
                          $"На балансе присутствуют ИОУ. " +
                          $"Необходимо предоставить сведения об инвентаризации по форме 1.2" +
                          $"{Environment.NewLine}Баланс: {balance}"
            };
        return null;
    }

    private static async Task<CheckError?> CheckPresenceOfForm13(Form41 form41, CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        if (!form41.Report.Year_DB.HasValue) return null;
        var year = form41.Report.Year_DB.Value;

        var organization = organizations10.FirstOrDefault(org =>
                org.RegNo == form41.RegNo_DB && org.Okpo == form41.Okpo_DB);

        if (organization == null) return null;

        double balance = 0;
        var inventoryFlag = false;
        var dbModel = StaticConfiguration.DBModel;

        var reportCollection = dbModel.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == organization.Id)
            .SelectMany(reps => reps.Report_Collection)
            .Where(rep => rep.FormNum_DB == "1.3")
            .Include(rep => rep.Rows13)
            .AsEnumerable() // Переходим к клиентской обработке
            .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out _)
                          && DateTime.TryParse(rep.EndPeriod_DB, out _))
            .OrderBy(rep => DateTime.Parse(rep.StartPeriod_DB))
            .ThenBy(rep => DateTime.Parse(rep.EndPeriod_DB))
            .ToList();

        var lastInventoryReport = reportCollection.LastOrDefault(rep =>
            rep.Rows13.Any(form13 => form13.OperationCode_DB == "10")
            && DateTime.Parse(rep.EndPeriod_DB).Year < year);

        var startIndex = 0;
        if (lastInventoryReport != null)
        {
            foreach (var key in lastInventoryReport.Rows13)
            {
                var row = (Form13)key;
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
            startIndex = reportCollection.IndexOf(lastInventoryReport);
        }

        try
        {
            for (var i = startIndex; i < reportCollection.Count; i++)
            {
                var report = reportCollection[i];
                for (var j = 0; j < report.Rows13.Count; j++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    if (DateTime.Parse(report.EndPeriod_DB).Year == year
                        && report.Rows13[j].OperationCode_DB == "10"
                        && !inventoryFlag)
                    {
                        inventoryFlag = true;
                    }
                    else if (Spravochniks.SignsOperation["1.3"].ContainsKey($"{report.Rows13[j].OperationCode_DB}"))
                    {
                        switch (Spravochniks.SignsOperation["1.3"][$"{report.Rows13[j].OperationCode_DB}"])
                        {
                            case '+':
                                balance += 1;
                                break;
                            case '-':
                                balance -= 1;
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

        if (!inventoryFlag && balance > 0)
            return new CheckError
            {
                Row = $"{form41.NumberInOrder_DB}",
                RegNo = form41.RegNo_DB,
                Okpo = form41.Okpo_DB,
                DbValue = $"{balance}",
                Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\"\n" +
                          $"Наименование -\"{form41.OrganizationName_DB}\"\n" +
                          $"На балансе присутствуют ОРИ. " +
                          $"Необходимо предоставить сведения об инвентаризации по форме 1.3\n" +
                          $"Баланс: {balance}"
            };
        return null;
    }

    private static async Task<CheckError?> CheckPresenceOfForm14(Form41 form41, CancellationTokenSource cts)
    {
        cts.Token.ThrowIfCancellationRequested();

        if (!form41.Report.Year_DB.HasValue) return null;
        var year = form41.Report.Year_DB.Value;

        var organization = organizations10.FirstOrDefault(org =>
                org.RegNo == form41.RegNo_DB && org.Okpo == form41.Okpo_DB);

        if (organization == null) return null;

        double massBalanceLiquid = 0;   //1
        double massBalanceSolid = 0;    //2
        double massBalanceGas = 0;      //3

        var inventoryFlag = false;

        var dbModel = StaticConfiguration.DBModel;

        var reportCollection = dbModel.ReportsCollectionDbSet
            .AsNoTracking()
            .Where(reps => reps.Id == organization.Id)
            .SelectMany(reps => reps.Report_Collection)
            .Where(rep => rep.FormNum_DB == "1.4")
            .Include(rep => rep.Rows14)
            .AsEnumerable() // Переходим к клиентской обработке
            .Where(rep => DateTime.TryParse(rep.StartPeriod_DB, out _)
                          && DateTime.TryParse(rep.EndPeriod_DB, out _))
            .OrderBy(rep => DateTime.Parse(rep.StartPeriod_DB))
            .ThenBy(rep => DateTime.Parse(rep.EndPeriod_DB))
            .ToList();

        var lastInventoryReport = reportCollection.LastOrDefault(rep =>
            rep.Rows14.Any(form14 => form14.OperationCode_DB == "10")
            && DateTime.Parse(rep.EndPeriod_DB).Year < year);

        var startIndex = 0;
        if (lastInventoryReport != null)
        {
            foreach (var key in lastInventoryReport.Rows14)
            {
                var row = (Form14)key;
                cts.Token.ThrowIfCancellationRequested();
                if (row.OperationCode_DB == "10")
                {
                    if (double.TryParse(row.Mass_DB, out var mass))
                        switch (row.AggregateState_DB)
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
            startIndex = reportCollection.IndexOf(lastInventoryReport);
        }

        try
        {
            for (var i = startIndex; i < reportCollection.Count; i++)
            {
                var report = reportCollection[i];
                for (var j = 0; j < report.Rows14.Count; j++)
                {
                    cts.Token.ThrowIfCancellationRequested();
                    if (DateTime.Parse(report.EndPeriod_DB).Year == year
                        && report.Rows14[j].OperationCode_DB == "10"
                        && !inventoryFlag)
                    {
                        inventoryFlag = true;
                    }
                    else if (double.TryParse(report.Rows14[j].Mass_DB, out var mass)
                             && Spravochniks.SignsOperation["1.4"].ContainsKey($"{report.Rows14[j].OperationCode_DB}"))
                    {
                        

                        switch (Spravochniks.SignsOperation["1.4"][$"{report.Rows14[j].OperationCode_DB}"])
                        {
                            case '+':
                            {
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

                                break;
                            }
                            case '-':
                            {
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

                                break;
                            }
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
        massBalanceLiquid = Math.Round(massBalanceLiquid, 10);
        massBalanceSolid = Math.Round(massBalanceSolid, 10);
        massBalanceGas = Math.Round(massBalanceGas, 10);

        if (!inventoryFlag && (massBalanceLiquid > 0 || massBalanceSolid > 0 || massBalanceGas > 0))
            return new CheckError
            {
                Row = $"{form41.NumberInOrder_DB}",
                RegNo = form41.RegNo_DB,
                Okpo = form41.Okpo_DB,
                DbValue = $"{massBalanceLiquid}" +
                          $"{Environment.NewLine}{massBalanceSolid}" +
                          $"{Environment.NewLine}{massBalanceGas}",
                Message = $"У организации Рег№-\"{form41.RegNo_DB}\" ОКПО-\"{form41.Okpo_DB}\"\n" +
                          $"Наименование -\"{form41.OrganizationName_DB}\"\n" +
                          $"На балансе присутствуют радионуклиды. " +
                          $"Необходимо проинвентаризировать по форме 1.4" +
                          $"{Environment.NewLine}Баланс жидких РВ: {massBalanceLiquid}" +
                          $"{Environment.NewLine}Баланс твердых РВ: {massBalanceSolid}" +
                          $"{Environment.NewLine}Баланс газообразных РВ: {massBalanceGas}{Environment.NewLine}"
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
        var answer = await Dispatcher.UIThread.InvokeAsync(() => MessageBox.Avalonia.MessageBoxManager
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
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Topmost = true,
            })
            .ShowDialog(owner));

        return answer == "Да";
    }

        
}

internal class Organization
{
    public int Id { get; set; }

    public string RegNo { get; set; }

    public string Okpo { get; set; }
}

//class OperationCodeAndMass
//{
//    public string OperationCode { get; set; }
//    public int Mass { get; set; }
//}