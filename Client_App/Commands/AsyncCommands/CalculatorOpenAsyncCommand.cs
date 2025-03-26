using Client_App.ViewModels;
using Client_App.Views;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

public class CalculatorOpenAsyncCommand : BaseAsyncCommand
{
    public override Task AsyncExecute(object? parameter)
    {
        var dialogWindow = new Calculator
        {
            DataContext = new CalculatorVM()
        };
        dialogWindow.Show();

        return Task.CompletedTask;
    }
}