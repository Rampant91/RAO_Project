using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Behaviors;
using Client_App.ViewModels.Forms;
using Client_App.ViewModels.Forms.Forms4;
using Client_App.ViewModels.Messages;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands
{
    public class GenerateForm41AsyncCommand (BaseFormVM formVM) : BaseAsyncCommand
    {
        

        public override async Task AsyncExecute(object? parameter)
        {
            var owner = (Application.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Windows
                .FirstOrDefault(w => w.IsActive);

            if (owner == null) return;

            if (!await ShowConfirmationMessage(owner)) return;

            Report? report = null;
            if (await ShowAskDependOnReportOrNotMessage(owner)) 
                report = await ShowAskReportMessage(owner);

            int? year = null;
            if (formVM.Report.Year.Value == "" || formVM.Report.Year.Value == null)
            {
                year = await ShowAskYearMessage(owner);
                formVM.Report.Year.Value = year.ToString();
            }

            string? subjectRF = null;
            if (await ShowAskAllOrOneSubjectRFMessage(owner))
                subjectRF = await ShowAskSubjectRFMessage(owner);
            

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

        private async Task<int?> ShowAskYearMessage(Window owner)
        {
            var dialog = new AskIntMessageWindow(new AskIntMessageVM("Введите год, за который хотите сформировать отчет"));

            int? year = await dialog.ShowDialog<int?>(owner);
            return year;
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

            string? subjectRF = await dialog.ShowDialog<string?>(owner);
            return subjectRF;
        }
    }
}
