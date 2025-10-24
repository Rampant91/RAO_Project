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
    //TODO отрефакторить
    public class GenerateForm41AsyncCommand (BaseFormVM formVM) : BaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);

            if (owner == null) return;

            if (!await ShowConfirmationMessage(owner)) return;

            formVM.Report.Rows41.Clear();

            Report? report = null;
            if (await ShowAskDependOnReportOrNotMessage(owner))
            {
                report = await ShowAskReportMessage(owner);
                if (report!=null)
                    formVM.Report.Rows41.AddRange(report.Rows41);
            }

            formVM.UpdateFormList();
            formVM.UpdatePageInfo();

            string? codeSubjectRF = null;
            if (await ShowAskAllOrOneSubjectRFMessage(owner))
                codeSubjectRF = await ShowAskSubjectRFMessage(owner);


            var dbModel = StaticConfiguration.DBModel;
            var organizations10 = dbModel.ReportsCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(reports => reports.Master_DB)
                            .Where(reports => 
                            reports.Master_DB.FormNum_DB == "1.0")
                            .ToList();
            var organizations20 = dbModel.ReportsCollectionDbSet
                            .AsSplitQuery()
                            .AsQueryable()
                            .Include(reports => reports.Master_DB)
                            .Where(reports =>
                            reports.Master_DB.FormNum_DB == "2.0")
                            .ToList();

            if (codeSubjectRF != null)
            {
                List<Form41> collection = formVM.Report.Rows41
                    .Where(form41 => form41.RegNo_DB.StartsWith(codeSubjectRF)).ToList();

                formVM.Report.Rows41.Clear();
                formVM.Report.Rows41.AddRange(collection);



                organizations10 = organizations10.FindAll(reports => reports.Master.RegNoRep != null
                            && reports.Master.RegNoRep.Value.StartsWith(codeSubjectRF));

                organizations20 = organizations20.FindAll(reports => reports.Master.RegNoRep != null
                            && reports.Master.RegNoRep.Value.StartsWith(codeSubjectRF));
            }

            int year =0;
            if (!int.TryParse(formVM.Report.Year.Value, out year))
            {
                year = await ShowAskYearMessage(owner);
                formVM.Report.Year.Value = year.ToString();
            }



            foreach (var organization in organizations10)
            {
                int numInventarizationForm = dbModel.ReportCollectionDbSet
                    .AsSplitQuery()
                    .Include(report =>report.Reports)
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

                int numWithoutInventarizationForm = dbModel.ReportCollectionDbSet
                    .AsSplitQuery()
                    .AsQueryable()
                    .Where(report => report.Reports.Id == organization.Id)
                    .Where(report =>
                        report.EndPeriod_DB.EndsWith(year.ToString()) ||
                        report.Year_DB == year.ToString()
                    )
                    .Count();
                numWithoutInventarizationForm -= numInventarizationForm;

                

                formVM.Report.Rows41.Add(new Form41()
                {
                    RegNo_DB = organization.Master.RegNoRep == null ? "" : organization.Master.RegNoRep.Value,
                    Okpo_DB = organization.Master.OkpoRep == null ? "" : organization.Master.OkpoRep.Value,
                    OrganizationName_DB = organization.Master.ShortJurLicoRep == null ? "" : organization.Master.ShortJurLicoRep.Value,
                    NumOfFormsWithInventarizationInfo_DB = numInventarizationForm,
                    NumOfFormsWithoutInventarizationInfo_DB = numWithoutInventarizationForm,
                });

            }

            foreach (var organization in organizations20)
            {
                int numForm212 = dbModel.ReportCollectionDbSet
                        .AsSplitQuery()
                        .AsQueryable()
                        .Include(report => report.Reports)
                        .Where(report => report.Reports.Id == organization.Id)
                        .Where(report => report.FormNum_DB == "2.12")
                        .Count();

                var form41 = formVM.Report.Rows41.
                    FirstOrDefault(form => 
                    form.RegNo_DB == organization.Master.RegNoRep.Value);

                if (form41 != null)
                {
                    form41.NumOfForms212.Value = numForm212;
                }
                else
                {
                    formVM.Report.Rows41.Add(new Form41()
                    {
                        RegNo_DB = organization.Master.RegNoRep == null ? "" : organization.Master.RegNoRep.Value,
                        Okpo_DB = organization.Master.OkpoRep == null ? "" : organization.Master.OkpoRep.Value,
                        OrganizationName_DB = organization.Master.ShortJurLicoRep == null ? "" : organization.Master.ShortJurLicoRep.Value,
                        NumOfFormsWithInventarizationInfo_DB = 0,
                        NumOfFormsWithoutInventarizationInfo_DB = 0,
                        NumOfForms212_DB = numForm212
                    });
                }

            }

            for (int i = 0; i < formVM.Report.Rows41.Count; i++)
                formVM.Report.Rows41[i].NumberInOrder_DB = i + 1;

            formVM.UpdateFormList();
            formVM.UpdatePageInfo();


        }

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
            var dialog = new AskForm41Message(formVM.Report);

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
    }
}
