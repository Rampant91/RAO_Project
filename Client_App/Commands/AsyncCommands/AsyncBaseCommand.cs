using System.Threading.Tasks;
using Client_App.Commands.SyncCommands;

namespace Client_App.Commands.AsyncCommands;

public abstract class AsyncBaseCommand : BaseCommand
{
    private bool _isExecute;

    public bool IsExecute
    {
        get => _isExecute;
        set
        {
            _isExecute = value;
            OnCanExecuteChanged();
        }
    }

    public override bool CanExecute(object? parameter)
    {
        return !_isExecute;
    }

    public override async void Execute(object? parameter)
    {
        IsExecute = true;
        await AsyncExecute(parameter);
        IsExecute = false;
    }

    public abstract Task AsyncExecute(object? parameter);
}