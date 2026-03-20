using Client_App.ViewModels;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

internal class SetWhiteListNumAsyncCommand : BaseAsyncCommand
{
    private readonly MainWindowVM _mainWindowVM;
    public SetWhiteListNumAsyncCommand(MainWindowVM mainWindowVM) : base()
    {
        _mainWindowVM = mainWindowVM;
    }


    public override async Task AsyncExecute(object? parameter)
    {
        if (parameter is not string formNum) return;
        switch (_mainWindowVM.SelectedReportType)
        {
            case 1:
                _mainWindowVM.Forms1TabControlVM.SetWhiteList(formNum);
                break;
            case 2:
                _mainWindowVM.Forms2TabControlVM.SetWhiteList(formNum);
                break;
            //case 4:
            //    _mainWindowVM.Forms4TabControlVM.SetWhiteList(formNum);
            //    break;
            case 5:
                _mainWindowVM.Forms5TabControlVM.SetWhiteList(formNum);
                break;
            default:
                break;
        }
    }
}