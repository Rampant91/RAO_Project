using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Client_App.Commands.AsyncCommands.Save;
using Client_App.ViewModels;
using Client_App.ViewModels.MainWindowTabs;
using Client_App.Views;
using Models.Collections;
using Models.DBRealization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands
{
    internal class GoToFormNumAsyncCommand(MainWindowVM mainWindowVM) : BaseAsyncCommand
    {
        public override async Task AsyncExecute(object? parameter)
        {
            if (parameter is not string formNum) return;
            switch (mainWindowVM.SelectedReportType)
            {
                case 1:
                    mainWindowVM.Forms1TabControlVM.GoToFormNum(formNum);
                    break;
                case 2:
                    mainWindowVM.Forms2TabControlVM.GoToFormNum(formNum);
                    break;
                case 4:
                    mainWindowVM.Forms4TabControlVM.GoToFormNum(formNum);
                    break;
                default:
                    break;
            }
        }
    }
}
