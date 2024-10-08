﻿using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Client_App.Commands.SyncCommands;

namespace Client_App.Commands.AsyncCommands;

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
        catch (Exception e)
        {
            // ignored
        }
        IsExecute = false;
    }

    public abstract Task AsyncExecute(object? parameter);
}