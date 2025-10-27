using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Behaviors;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Messages;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using Models.Forms.Form4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands
{
    public class GenerateForm41AsyncCommand (BaseFormVM formVM) : BaseAsyncCommand
    {
        #region private Properties
        private Report Report => formVM.Report;

        private DBModel dbModel = StaticConfiguration.DBModel;

        private List<Reports> organizations10;
        private List<Reports> organizations20;
        private string codeSubjectRF;
        private int year = 0;
        #endregion

        public override async Task AsyncExecute(object? parameter)
        {
            var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);


            if (owner == null) return;

            #region ShowAskMessages
            if (!await ShowConfirmationMessage(owner)) return;

            Report.Rows41.Clear();

            if (await ShowAskDependOnReportOrNotMessage(owner))
            {
                var report =  await ShowAskReportMessage(owner);
                if (report != null)
                    CopyRowsFromReport(report);
            }

            formVM.UpdateFormList();
            formVM.UpdatePageInfo();

            if (await ShowAskAllOrOneSubjectRFMessage(owner))
                codeSubjectRF = await ShowAskSubjectRFMessage(owner);

            if (!int.TryParse(Report.Year.Value, out year))
            {
                year = await ShowAskYearMessage(owner);
                Report.Year.Value = year.ToString();
            }
            #endregion 

            organizations10 = GetOrganizationsList("1.0");
            organizations20 = GetOrganizationsList("2.0"); 

            if (codeSubjectRF != null)
            {
                FilterAllByCodeSubjectRF(codeSubjectRF);
            }

            foreach (var organization10 in organizations10)
            {
                int numInventarizationForm = GetNumOfReportWithInventarization(organization10, year);
                int numWithoutInventarizationForm = GetNumOfReportWithoutInventarization(organization10, year);

                //Если запись об организации существует
                if (IsRowWithOrganizationExist(organization10))
                    UpdateRow(organization10,
                        numInventarizationForm : numInventarizationForm,
                        numWithoutInventarizationForm : numWithoutInventarizationForm);
                else
                    CreateRow(organization10,
                        numInventarizationForm: numInventarizationForm,
                        numWithoutInventarizationForm: numWithoutInventarizationForm);
            }

            foreach (var organization20 in organizations20)
            {
                int numForm212 = GetNumOfForm212(organization20, year);


                if (IsRowWithOrganizationExist(organization20))
                    UpdateRow(organization20, 
                        numForm212: numForm212);
                else
                    CreateRow(organization20, 
                        numForm212: numForm212);

            }

            //Выставляем номера строк
            for (int i = 0; i < Report.Rows41.Count; i++)
                Report.Rows41[i].NumberInOrder_DB = i + 1;

            //Обновляем таблицу
            formVM.UpdateFormList();
            formVM.UpdatePageInfo();


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
        private async Task<bool> ShowAskDependOnReportOrNotMessage(Window owner)
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
        private async Task<Report> ShowAskReportMessage(Window owner)
        {
            var dialog = new AskForm41Message(Report);

            Report? report = await dialog.ShowDialog<Report?>(owner);
            return report;
        }
        private async Task<int> ShowAskYearMessage(Window owner)
        {
            var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите год, за который хотите сформировать отчет"));

            int? year = await dialog.ShowDialog<int>(owner);

            if (year == null)
                return 0;

            return (int)year;
        }
        private async Task<bool> ShowAskAllOrOneSubjectRFMessage(Window owner)
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
        private async Task<string?> ShowAskSubjectRFMessage(Window owner)
        {
            var dialog = new AskSubjectRFMessage();

            string? codeSubjectRF = await dialog.ShowDialog<string?>(owner);
            return codeSubjectRF;
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

                Note_DB = original.Note_DB,
            });
            Report.Rows41.AddRange(copiedRows);
        }
        private void FilterAllByCodeSubjectRF(string codeSubjectRF)
        {
            var collection = Report.Rows41.ToList().FindAll(form41 => form41.RegNo_DB.StartsWith(codeSubjectRF)); //Фильтруем строчки

            Report.Rows41.Clear();
            Report.Rows41.AddRange(collection);

            organizations10 = organizations10.FindAll(reports => reports.Master.RegNoRep != null
                        && reports.Master.RegNoRep.Value.StartsWith(codeSubjectRF));

            organizations20 = organizations20.FindAll(reports => reports.Master.RegNoRep != null
                        && reports.Master.RegNoRep.Value.StartsWith(codeSubjectRF));
        }
        private bool IsRowWithOrganizationExist(Reports organization )
        {
            return (organization.Master.RegNoRep != null) &&
                    Report.Rows41.Any(form =>
                    form.RegNo_DB == organization.Master.RegNoRep.Value);
        }
        private void UpdateRow(
            Reports organization, 
            int numInventarizationForm = -1,        // Необязательный параметр
            int numWithoutInventarizationForm = -1, // Необязательный параметр
            int numForm212 = -1)                    // Необязательный параметр
        {

            var form41 = Report.Rows41.FirstOrDefault(form41 =>
                    form41.RegNo_DB == organization.Master.RegNoRep.Value);

            form41.Okpo_DB = organization.Master.OkpoRep.Value;
            form41.OrganizationName_DB = organization.Master.ShortJurLicoRep.Value;

            if (numInventarizationForm >= 0)
                form41.NumOfFormsWithInventarizationInfo_DB = numInventarizationForm;

            if (numWithoutInventarizationForm >= 0)
                form41.NumOfFormsWithoutInventarizationInfo_DB = numWithoutInventarizationForm;

            if (numForm212 >= 0)
                form41.NumOfForms212_DB = numForm212;
        }

        private void CreateRow(
            Reports organization, 
            int numInventarizationForm = -1,        // Необязательный параметр
            int numWithoutInventarizationForm = -1, // Необязательный параметр
            int numForm212 = -1)                    // Необязательный параметр
        {
            var form41 = new Form41()
            {
                RegNo_DB = organization.Master.RegNoRep == null ? "" : organization.Master.RegNoRep.Value,
                Okpo_DB = organization.Master.OkpoRep == null ? "" : organization.Master.OkpoRep.Value,
                OrganizationName_DB = organization.Master.ShortJurLicoRep == null ? "" : organization.Master.ShortJurLicoRep.Value
            };

            if (numInventarizationForm >= 0)
                form41.NumOfFormsWithInventarizationInfo_DB = numInventarizationForm;

            if (numWithoutInventarizationForm >= 0)
                form41.NumOfFormsWithoutInventarizationInfo_DB = numWithoutInventarizationForm;

            if (numForm212 >= 0)
                form41.NumOfForms212_DB = numForm212;

            Report.Rows41.Add(form41);

        }

        #endregion

        #region Requests
        private List<Reports> GetOrganizationsList(string formNum)
        {
            return dbModel.ReportsCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(reports => reports.Master_DB)
                            .Where(reports => reports.Master_DB.FormNum_DB == formNum)
                            .ToList();
        }
        private int GetNumOfReportWithInventarization(Reports organization, int year)
        {
            return dbModel.ReportCollectionDbSet
                    .AsSplitQuery()
                    .Include(report => report.Reports)
                    .Where(report => report.Reports.Id == organization.Id)
                    .Where(report =>
                        report.EndPeriod_DB.EndsWith(year.ToString()) ||
                        report.Year_DB == year.ToString()
                    )
                    .Where(report =>
                        report.Rows11.Any(row => row.OperationCode_DB == "10") ||
                        report.Rows12.Any(row => row.OperationCode_DB == "10") ||
                        report.Rows13.Any(row => row.OperationCode_DB == "10") ||
                        report.Rows14.Any(row => row.OperationCode_DB == "10")
                    )
                    .Count();
        }
        private int GetNumOfReportWithoutInventarization(Reports organization10, int year)
        {
            return dbModel.ReportCollectionDbSet
                    .AsSplitQuery()
                    .Include(report => report.Reports)
                    .Where(report => report.Reports.Id == organization10.Id)
                    .Where(report => report.EndPeriod_DB.EndsWith(year.ToString()))
                    .Where(report =>
                        report.Rows11.All(row => row.OperationCode_DB != "10") &&
                        report.Rows12.All(row => row.OperationCode_DB != "10") &&
                        report.Rows13.All(row => row.OperationCode_DB != "10") &&
                        report.Rows14.All(row => row.OperationCode_DB != "10")
                    )
                    .Count();
        }
        private int GetNumOfForm212(Reports organization20, int year)
        {
            return dbModel.ReportCollectionDbSet
                        .AsSplitQuery()
                        .AsQueryable()
                        .Include(report => report.Reports)
                        .Where(report => report.Reports.Id == organization20.Id)
                        .Where(report => report.Year_DB == year.ToString())
                        .Where(report => report.FormNum_DB == "2.12")
                        .Count();
        }
        #endregion
    }
}
