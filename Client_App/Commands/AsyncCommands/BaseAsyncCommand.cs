using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.SyncCommands;
using Client_App.Interfaces.Logger;

namespace Client_App.Commands.AsyncCommands;

/// <summary>
/// Базовый класс async команды.
/// </summary>
public abstract class BaseAsyncCommand : BaseCommand
{
    private protected static readonly IClassicDesktopStyleApplicationLifetime Desktop =
        (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)!;

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
        try
        {
            await AsyncExecute(parameter);
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                       $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Warning(msg);
        }
        IsExecute = false;
    }

    public abstract Task AsyncExecute(object? parameter);
}