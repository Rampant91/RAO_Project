using Client_App.ViewModels;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

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
            case 5:
                mainWindowVM.Forms5TabControlVM.GoToFormNum(formNum);
                break;
            default:
                break;
        }
    }
}