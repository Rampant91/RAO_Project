using Client_App.ViewModels;
using Models.Collections;
using System.Threading.Tasks;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Костыльная команда, привязывающая изменение параметра автозамены между моделью и представлением.
/// Необходимо уничтожить, после перерисовки интерфейса.
/// </summary>
/// <param name="changeOrCreateViewModel">ViewModel</param>
public class SetAutoReplaceAsyncCommand(ChangeOrCreateVM changeOrCreateViewModel) : BaseAsyncCommand
{
    private Report Storage => changeOrCreateViewModel.Storage;

    public override async Task AsyncExecute(object? parameter)
    {
        changeOrCreateViewModel.IsAutoReplaceEnabled = parameter switch
        {
            true => true,
            false => false,
            _ => changeOrCreateViewModel.IsAutoReplaceEnabled
        };
    }

    public async void Set(object sender, Avalonia.Interactivity.RoutedEventArgs args)
    {
        await AsyncExecute(true);
    }

    public async void UnSet(object sender, Avalonia.Interactivity.RoutedEventArgs args)
    {
        await AsyncExecute(false);
    }
}