using Client_App.ViewModels;
using Client_App.Views;
using System.Threading.Tasks;

namespace Client_App.Service
{
    public class DialogService : IDialogService
    {
        public Task ShowDialogAsync() // При вызове метода, создаем и выводим окно
        {
            var dialogWindow = new Calculator
            {
                DataContext = new CalculatorVM()
            };

            dialogWindow.Show();

            return Task.CompletedTask;
        }
    }
}