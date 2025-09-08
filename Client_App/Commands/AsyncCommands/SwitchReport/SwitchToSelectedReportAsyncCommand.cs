using Client_App.ViewModels.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands.SwitchReport
{
    internal class SwitchToSelectedReportAsyncCommand(BaseFormVM formVM) : BaseAsyncCommand
    {
        // Пока не используется
        public override async Task AsyncExecute(object? parameter)
        {
        }
    }
}