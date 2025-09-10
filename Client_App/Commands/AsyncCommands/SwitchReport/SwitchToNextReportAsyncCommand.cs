using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels.Forms;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBox.Avalonia.Models;
using Microsoft.EntityFrameworkCore.Update.Internal;
using Models.Classes;
using Models.Collections;
using Models.DBRealization;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.SwitchReport
{
    //Потом нужно будет сделать униварсальную SwitchingSelectedReportCommand
    public class SwitchToNextReportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        private Report Report => formVM.Report;
        private Reports Reports => formVM.Reports;

        public override async Task AsyncExecute(object? parameter)
        {
            // Проверяем изменения и предлагаем сохранить
            var shouldContinue = await new CheckForChangesAndSaveCommand(formVM).AsyncExecute(null);
            if (!shouldContinue) return;

            // Дальше переключаемся на другую форму
            var index = Reports.Report_Collection.IndexOf(Report);
            Report? newReport = null;
            for (int i = index - 1; i >= 0; i--)
            {
                if (Reports.Report_Collection[i].FormNum.Value == Report.FormNum.Value)
                {
                    newReport = Reports.Report_Collection[i];
                    break;
                }
            }
            if (newReport == null) return;

            var window = Desktop.Windows.First(x => x.Name == formVM.FormType);
            var windowParam = new FormParameter()
            {
                Parameter = new ObservableCollectionWithItemPropertyChanged<IKey>(new List<Report> { newReport }),
                Window = window
            };
            await new ChangeFormAsyncCommand(windowParam).AsyncExecute(null).ConfigureAwait(false);
            
        }
    }
}