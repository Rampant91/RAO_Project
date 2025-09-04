using Client_App.ViewModels.Forms;
using Models.Classes;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Models.Interfaces;

namespace Client_App.Commands.AsyncCommands.SwitchReport
{
    public class SwitchToPreviousReportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        private Report Report => formVM.Report;
        private Reports Reports => formVM.Reports;
        public override async Task AsyncExecute(object? parameter)
        {
            var index = Reports.Report_Collection.IndexOf(Report);
            Report? newReport = null;
            for (int i = index + 1; i < Reports.Report_Collection.Count ; i++)
            {
                newReport = Reports.Report_Collection[i];
                if (newReport.FormNum.Value == Report.FormNum.Value)
                    break;
            }
            if ((newReport == null) || (newReport.FormNum.Value != Report.FormNum.Value)) return;


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