using System.Threading.Tasks;
using Client_App.ViewModels.Calculator;
using Client_App.Views.Calculator;

namespace Client_App.Commands.AsyncCommands;

public class CalculatorOpenAsyncCommand : BaseAsyncCommand
{
    public override Task AsyncExecute(object? parameter)
    {
        var dialogWindow = new ActivityCalculator
        {
            DataContext = new ActivityCalculatorVM()
        };
        dialogWindow.Show();

        return Task.CompletedTask;
    }
}