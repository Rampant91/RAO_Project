using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using Client_App.ViewModels.ProgressBar;
using Client_App.Views.ProgressBar;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Client_App.Views.Messages;
using Spravochniki;

namespace Client_App.Commands.AsyncCommands;

public class GenerateForm41AsyncCommand (BaseFormVM formVM) : BaseAsyncCommand
{
    #region private Properties
    private Report Report => formVM.Report;

    private DBModel dbModel = StaticConfiguration.DBModel;
    private DBModel secondDB;
    private Window owner;

    private List<Reports> organizations10;
    private List<Reports> organizations20;
    private string codeSubjectRF;
    private int year = 0;
    #endregion

    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        try
        {
            await Task.Run(() => AsyncExecute(parameter));
        }
        catch (OperationCanceledException) { }
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
            .FirstOrDefault(w => w.Name == "4.1");

        if (owner == null) return;

        var cts = new CancellationTokenSource();

        cts.Token.Register(() => Report.Rows41.Clear());


        #region ShowAskMessages

        if (Report.Rows41.Count > 0)
        {
            if (!await ShowConfirmationMessage(owner)) return;
        }

        Report.Rows41.Clear();

        await new SaveReportAsyncCommand(formVM).AsyncExecute(null);

        if (await ShowAskDependOnReportOrNotMessage(owner))
        {
            var report = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskReportMessage(owner));
            
            if (report != null)
                CopyRowsFromReport(report);
        }

        formVM.UpdateFormList();
        formVM.UpdatePageInfo();

        codeSubjectRF = Report.Reports.Master_DB.Rows40[0].CodeSubjectRF_DB;

        int intCode;
        while (!int.TryParse(codeSubjectRF, out intCode)
           || !Spravochniks.DictionaryOfSubjectRF.ContainsKey(intCode))
        {
            codeSubjectRF = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskSubjectRFMessage(owner));

        }

        Report.Reports.Master_DB.Rows40[0].CodeSubjectRF_DB = codeSubjectRF;
        Report.Reports.Master_DB.Rows40[0].SubjectRF_DB = Spravochniks.DictionaryOfSubjectRF[intCode];

        if (!int.TryParse(Report.Year.Value, out year))
        {
            year = await Dispatcher.UIThread.InvokeAsync(async () => await ShowAskYearMessage(owner));
            Report.Year.Value = year.ToString();
        }

        #endregion

        var progressBar = await Dispatcher.UIThread.InvokeAsync(async () => new AnyTaskProgressBar(cts, owner));
        var progressBarVM = progressBar.AnyTaskProgressBarVM;

        progressBarVM.SetProgressBar(5, $"Загрузка организаций");

        organizations10 = await GetOrganizationsList("1.0", dbModel, cts.Token);
        organizations20 = await GetOrganizationsList("2.0", dbModel, cts.Token);

        if (organizations20.Count <= 0)
        {
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
                    
                    organizations20 = await GetOrganizationsList("2.0", secondDB, cts.Token);
                }

            }
        }

        if (codeSubjectRF != null)
        {
            progressBarVM.SetProgressBar(7, $"Фильтрация записей по коду субъекта РФ");

            FilterAllByCodeSubjectRF(codeSubjectRF);
        }

        double currentProgress = 14.0;
        double incProgress = (90.0 - currentProgress) / (organizations10.Count() + organizations20.Count());
        foreach (var organization10 in organizations10)
        {
            cts.Token.ThrowIfCancellationRequested();
            progressBarVM.SetProgressBar(
                (int)currentProgress, 
                $"Подсчет форм 1.1 - 1.4 у организации {organization10.Master.Rows10[0].RegNo_DB}");

            var numInventarizationForm = await GetNumOfReportWithInventory(organization10.Id, year.ToString(), dbModel, cts.Token);
            var numWithoutInventarizationForm = await GetNumOfReportWithoutInventory(organization10.Id, year.ToString(), dbModel, cts.Token);
            if (IsRowWithOrganizationExist(organization10))
            {
                //Если запись об организации существует
                UpdateRow(organization10,
                    numInventoryForm: numInventarizationForm,
                    numWithoutInventoryForm: numWithoutInventarizationForm);
            }
            else
            {
                CreateRow(organization10,
                    numInventoryForm: numInventarizationForm,
                    numWithoutInventoryForm: numWithoutInventarizationForm);
            }
            
            currentProgress += incProgress;
        }

        foreach (var organization20 in organizations20)
        {
            progressBarVM.SetProgressBar(
                (int)currentProgress,
                $"Подсчет форм 2.12 у организации {organization20.Master_DB.Rows20[0].RegNo_DB}");

            int numForm212;

            if (secondDB != null)
                numForm212 = await GetNumOfForm212(organization20, year, secondDB, cts.Token);
            else
                numForm212 = await GetNumOfForm212(organization20, year, dbModel, cts.Token);

            if (IsRowWithOrganizationExist(organization20))
                UpdateRow(organization20,
                    numForm212: numForm212);
            else
                CreateRow(organization20,
                    numForm212: numForm212);
        }

        formVM.UpdateFormList();
        formVM.UpdatePageInfo();

        progressBarVM.SetProgressBar(
            90,
            "Сортировка записей по рег. номеру");

        // Сортируем по Рег.Номеру
        var orderedRows = Report.Rows41.OrderBy(row => row.RegNo_DB).ToList();
        Report.Rows41.Clear();
        Report.Rows41.AddRange(orderedRows);


        formVM.UpdateFormList();
        formVM.UpdatePageInfo();

        //Заполняем пробелы РегНомеров 
        FillSpaceByRegNo(progressBarVM, cts);

        progressBarVM.SetProgressBar(
            95,
            $"Выставляем номера строк");

        //Выставляем номера строк
        for (var i = 0; i < Report.Rows41.Count; i++)
        {
            Report.Rows41[i].SetOrder(i + 1);
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
    private static async Task<bool> ShowConfirmationMessage(Window owner)
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
    private static async Task<bool> ShowAskDependOnReportOrNotMessage(Window owner)
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
                ContentMessage = "Вы хотите сформировать отчет на основе другого отчета?",
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

    private async Task<Report?> ShowAskReportMessage(Window owner)
    {
        var dialog = new AskForm41Message(Report);

        var report = await dialog.ShowDialog<Report?>(owner);
        return report;
    }

    private static async Task<int> ShowAskYearMessage(Window owner)
    {
        var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите год, за который хотите сформировать отчет"));

        int? year = await dialog.ShowDialog<int>(owner);

        if (year == null)
            return 0;

        return (int)year;
    }

    private static async Task<bool> ShowAskAllOrOneSubjectRFMessage(Window owner)
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
                ContentMessage = "Вы хотите сформировать отчет по конкретному субъекту Российской Федерации?",
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

    private static async Task<string?> ShowAskSubjectRFMessage(Window owner)
    {
        var dialog = new AskSubjectRFMessage();

        var codeSubjectRF = await dialog.ShowDialog<string?>(owner);
        return codeSubjectRF;
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
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            })
            .ShowDialog(owner));

        if (answer == "Да")
            return true;
        else
            return false;
    }

    #endregion

    #region private Functions
    private void CopyRowsFromReport(Report report)
    {
        var copiedRows = report.Rows41.Select(original => new Form41()
        {
            NumberInOrder_DB = original.NumberInOrder_DB,
            RegNo_DB = original.RegNo_DB,
            Okpo_DB = original.Okpo_DB,
            OrganizationName_DB = original.OrganizationName_DB,
            LicenseOrRegistrationInfo_DB = original.LicenseOrRegistrationInfo_DB,
            Report = formVM.Report,
            Note_DB = original.Note_DB,
        });
        Report.Rows41.AddRange(copiedRows);
    }
    private void FilterAllByCodeSubjectRF(string codeSubjectRF)
    {

        organizations10 = organizations10.FindAll(reports => reports.Master.RegNoRep != null
                    && reports.Master.RegNoRep.Value.StartsWith(codeSubjectRF));

        organizations20 = organizations20.FindAll(reports => reports.Master.RegNoRep != null
                    && reports.Master.RegNoRep.Value.StartsWith(codeSubjectRF));
    }
    private bool IsRowWithOrganizationExist(Reports organization)
    {
        return (organization.Master.RegNoRep != null) &&
                Report.Rows41.Any(form =>
                form.RegNo_DB == organization.Master.RegNoRep.Value
                && form.Okpo_DB == organization.Master.OkpoRep.Value);
    }
    private void UpdateRow(
        Reports organization, 
        int numInventoryForm = -1,        // Необязательный параметр
        int numWithoutInventoryForm = -1, // Необязательный параметр
        int numForm212 = -1)              // Необязательный параметр
    {

        var form41 = Report.Rows41.FirstOrDefault(form41 =>
                form41.RegNo_DB == organization.Master.RegNoRep.Value 
                && form41.Okpo_DB == organization.Master.OkpoRep.Value);

        form41.OrganizationName_DB = organization.Master.ShortJurLicoRep.Value;

        if (numInventoryForm >= 0)
            form41.NumOfFormsWithInventarizationInfo_DB = numInventoryForm;

        if (numWithoutInventoryForm >= 0)
            form41.NumOfFormsWithoutInventarizationInfo_DB = numWithoutInventoryForm;

        if (numForm212 >= 0)
            form41.NumOfForms212_DB = numForm212;
    }

    private void CreateRow(
        Reports organization, 
        int numInventoryForm = 0,        // Необязательный параметр
        int numWithoutInventoryForm = 0, // Необязательный параметр
        int numForm212 = 0)              // Необязательный параметр
    {

        var form41 = new Form41()
        {
            RegNo_DB = organization.Master.RegNoRep == null 
                ? "" 
                : organization.Master.RegNoRep.Value,
            Okpo_DB = organization.Master.OkpoRep == null 
                ? "" 
                : organization.Master.OkpoRep.Value,
            OrganizationName_DB = organization.Master.ShortJurLicoRep == null 
                ? "" 
                : organization.Master.ShortJurLicoRep.Value,
            Report = formVM.Report
        };

        if (numInventoryForm > 0)
            form41.NumOfFormsWithInventarizationInfo_DB = numInventoryForm;

        if (numWithoutInventoryForm > 0)
            form41.NumOfFormsWithoutInventarizationInfo_DB = numWithoutInventoryForm;

        if (numForm212 > 0)
            form41.NumOfForms212_DB = numForm212;


        Report[Report.FormNum_DB].Add(form41);

    }

    // Эта функция создает пустые записи для организаций, не представленных в базе данных или в отчете по форме 4.1, на основе которого генерируется этот отчет
    private void FillSpaceByRegNo(AnyTaskProgressBarVM? progressBarVM, CancellationTokenSource cts)
    {

        var codeSubjectRF = Report.Reports.Master.Rows40[0].CodeSubjectRF_DB;
        int i = 0;
        int regNoIndex = 1;
        while(i < Report.Rows41.Count)
        {
            if (codeSubjectRF != Report.Rows41[i].RegNo_DB.Substring(0, 2))
            {
                i++;
                continue;
            }
            if (!int.TryParse(Report.Rows41[i].RegNo_DB.Substring(2, 3), out var current))
            {
                i++;
                continue;
            }
            // Если текущая организация временная, то переходим к следующей
            if (current / 100 is 8 or 9) return;

            if (regNoIndex < current)
            {
                string regNo = codeSubjectRF;
                if ((regNoIndex) / 100 == 0)
                    regNo += "0";
                if ((regNoIndex) / 10 == 0)
                    regNo += "0";
                regNo += $"{regNoIndex}";
                Report.Rows41.Insert(i, new Form41()
                {
                    RegNo_DB = $"{regNo}",
                    Report = formVM.Report // в форме указываем связь с отчетом, в котором создается эта форма
                });
                regNoIndex++;
                i++;
            }
            else
                break;
        }
        while(i < Report.Rows41.Count)
        {
            cts.Token.ThrowIfCancellationRequested();


            if (codeSubjectRF != Report.Rows41[i].RegNo_DB.Substring(0, 2)) continue;

            if (!int.TryParse(Report.Rows41[i].RegNo_DB.Substring(2, 3), out var current)) continue;

            // Если текущая организация временная, то переходим к следующей
            if (current / 100 is 8 or 9) return;

            //Если текущая запись последняя, то прекращаем выполнение функции
            if (i + 1 >= Report.Rows41.Count) return;

            if (codeSubjectRF != Report.Rows41[i + 1].RegNo_DB.Substring(0, 2)) return;
            
            if (!int.TryParse(Report.Rows41[i + 1].RegNo_DB.Substring(2, 3), out var next)) continue;

            //Если следующая организация временная, то переходим к следующей
            if (next / 100 is 8 or 9) return;
            // Если есть пробел между текущей и следующей организацией, то создаем новую запись между ними

            if (current+1 < next)
            {
                var regNo = codeSubjectRF;
                if ((current + 1) / 100 == 0)
                    regNo += "0";
                if ((current + 1) / 10 == 0)
                    regNo += "0";
                regNo += $"{current + 1}";
                Report.Rows41.Insert(i + 1, new Form41()
                {
                    RegNo_DB = $"{regNo}",
                    Report = formVM.Report // в форме указываем связь с отчетом, в котором создается эта форма
                });
            }
            i++;
        }
    }
    
    #endregion

    #region Requests

    private async Task<List<Reports>> GetOrganizationsList(string formNum, DBModel db, CancellationToken cancellationToken)
    {
        try
        {
            return await db.ReportsCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows10)
                            .Include(reports => reports.Master_DB).ThenInclude(reports => reports.Rows20)
                            .Where(reports => reports.Master_DB.FormNum_DB == formNum)
                            .ToListAsync(cancellationToken);
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
                   "Не удалось открыть базу данных с годовыми отчетами",
                   MinWidth = 300,
                   MinHeight = 125,
                   WindowStartupLocation = WindowStartupLocation.CenterOwner
               })
               .ShowDialog(owner));
            return [];
        }
    }

    private static async Task<int> GetNumOfReportWithInventory(int organizationId, string year, DBModel db, CancellationToken cancellationToken)
    {
        try
        {
            return await db.ReportsCollectionDbSet
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows12)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows14)
                .Where(reps => reps.DBObservable != null && reps.Id == organizationId)
                .SelectMany(x => x.Report_Collection
                    .Where(y => y.EndPeriod_DB.EndsWith(year)
                    &&
                    (
                        (y.FormNum_DB == "1.1" && y.Rows11.Any(form => form.OperationCode_DB == "10"))
                        || (y.FormNum_DB == "1.2" && y.Rows12.Any(form => form.OperationCode_DB == "10"))
                        || (y.FormNum_DB == "1.3" && y.Rows13.Any(form => form.OperationCode_DB == "10"))
                        || (y.FormNum_DB == "1.4" && y.Rows14.Any(form => form.OperationCode_DB == "10"))
                    )))
                .CountAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
    }

    private static async Task<int> GetNumOfReportWithoutInventory(int organizationId, string year, DBModel db, CancellationToken cancellationToken)
    {
        try 
        { 
            return await db.ReportsCollectionDbSet
                .AsSplitQuery()
                .AsQueryable()
                .Include(x => x.DBObservable)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows11)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows12)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows13)
                .Include(reps => reps.Report_Collection).ThenInclude(x => x.Rows14)
                .Where(reps => reps.DBObservable != null && reps.Id == organizationId)
                .SelectMany(x => x.Report_Collection
                    .Where(y => y.EndPeriod_DB.EndsWith(year)
                    &&
                    (
                        (y.FormNum_DB == "1.1" && y.Rows11.All(form => form.OperationCode_DB != "10"))
                        || (y.FormNum_DB == "1.2" && y.Rows12.All(form => form.OperationCode_DB != "10"))
                        || (y.FormNum_DB == "1.3" && y.Rows13.All(form => form.OperationCode_DB != "10"))
                        || (y.FormNum_DB == "1.4" && y.Rows14.All(form => form.OperationCode_DB != "10"))
                    )))
                .CountAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
    }

    private static async Task<int> GetNumOfForm212(Reports organization20, int year, DBModel db, CancellationToken cancellationToken)
    {
        try 
        {
            return await db.ReportCollectionDbSet
                    .AsSplitQuery()
                    .AsQueryable()
                    .Include(report => report.Reports)
                    .Where(report => report.Reports.Id == organization20.Id)
                    .Where(report => report.Year_DB == year.ToString())
                    .Where(report => report.FormNum_DB == "2.12")
                    .CountAsync(cancellationToken);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
    }

    #endregion
}
