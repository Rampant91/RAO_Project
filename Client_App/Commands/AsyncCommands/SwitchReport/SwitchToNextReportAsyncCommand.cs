using Client_App.ViewModels.Forms;
using Microsoft.EntityFrameworkCore.Update.Internal;
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
    public class SwitchToNextReportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        private Report Report => formVM.Report; 
        private Reports Reports => formVM.Reports;
        public override async Task AsyncExecute(object? parameter)
        {
            var index = Reports.Report_Collection.IndexOf(Report);
            Report? newReport = null;
            for (int i = index-1; i >= 0; i--)
            {
                if (Reports.Report_Collection[i].FormNum.Value == Report.FormNum.Value)
                {
                    newReport = Reports.Report_Collection[i];
                    break;
                }
            }
            if (newReport==null) return;


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
