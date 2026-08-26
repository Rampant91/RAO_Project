using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Client_App.Behaviors.WindowSizing;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Views;

public interface IFormDialogHost
{
    Task ShowFormDialogAsync(Window? owner);
}

public abstract class BaseWindow<T> : ReactiveWindow<BaseVM>, IFormDialogHost where T : class
{
    public T? VM => DataContext as T;
    public WindowState OwnerPrevState;

    protected virtual bool IsFullScreenWindow => false;

    /// <summary>Некоторые окна (прогрессбар) показываются сразу без fade-in.</summary>
    protected virtual bool RevealOnOpen => true;

    private bool _revealOnOpenAttached;
    private bool _openedPositionFallbackAttached;
    private Window? _positionOwnerHint;

    /// <summary>
    /// Задаёт финальный размер/состояние до показа окна, чтобы избежать «прыжка» layout.
    /// </summary>
    public void PrepareBeforeShow(Window? owner = null)
    {
        if (owner != null)
        {
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        if (IsFullScreenWindow)
        {
            WindowState = WindowState.Maximized;
            return;
        }

        if (WindowState is WindowState.Maximized or WindowState.FullScreen)
        {
            return;
        }

        if (Width > 0 && !double.IsNaN(Width) && Height > 0 && !double.IsNaN(Height))
        {
            return;
        }

        WindowScreenSizeBehavior.RefreshWindowSize(this);

        if (Width > 0 && !double.IsNaN(Width) && Height > 0 && !double.IsNaN(Height))
        {
            return;
        }

        if (Width <= 0 || double.IsNaN(Width))
        {
            Width = MinWidth > 0 ? MinWidth : 800;
        }

        if (Height <= 0 || double.IsNaN(Height))
        {
            Height = MinHeight > 0 ? MinHeight : 600;
        }
    }

    public async Task ShowFormDialogAsync(Window? owner)
    {
        PrepareBeforeShow(owner);
        AttachFullscreenFallback();

        if (!IsFullScreenWindow)
        {
            PositionWindowOnOwnerScreen(owner ?? GetOwnerWindow());
            AttachOpenedPositionFallback(owner);
        }

        AttachRevealOnOpen();

        if (owner is MainWindow mainWindow)
        {
            mainWindow.SetReportOpeningOverlay(false);
        }

        if (owner != null)
        {
            await ShowDialog(owner);
        }
    }

    /// <summary>
    /// Показ с предварительным размером и центрированием (до Show, плюс fallback в Opened для Linux).
    /// </summary>
    protected void ShowCentered(Window? owner = null)
    {
        var ownerWindow = owner ?? GetOwnerWindow();
        PrepareBeforeShow(ownerWindow);
        AttachFullscreenFallback();

        if (!IsFullScreenWindow)
        {
            PositionWindowOnOwnerScreen(ownerWindow);
            AttachOpenedPositionFallback(ownerWindow);
        }

        AttachRevealOnOpen();
        base.Show();
    }

    public override async void Show()
    {
        ShowCentered();
    }

    protected virtual void PositionWindowOnOwnerScreen(Window? ownerWindow = null)
    {
        try
        {
            if (!WindowCenterPlacement.TryCenterOnScreen(this, ownerWindow ?? GetOwnerWindow()))
            {
                return;
            }
        }
        catch (Exception ex)
        {
            var msg = $"{Environment.NewLine}Message: {ex.Message}" +
                      $"{Environment.NewLine}StackTrace: {ex.StackTrace}";
            ServiceExtension.LoggerManager.Error(msg);
        }
    }

    /// <summary>
    /// На Linux/Wayland Position до Show() часто игнорируется — повторяем центрирование в Opened.
    /// </summary>
    protected void AttachOpenedPositionFallback(Window? ownerHint)
    {
        if (_openedPositionFallbackAttached || IsFullScreenWindow)
        {
            return;
        }

        _openedPositionFallbackAttached = true;
        _positionOwnerHint = ownerHint;
        Opened += OnRepositionAfterOpen;
    }

    private void OnRepositionAfterOpen(object? sender, EventArgs e)
    {
        Opened -= OnRepositionAfterOpen;
        _openedPositionFallbackAttached = false;

        if (IsFullScreenWindow)
        {
            return;
        }

        if (OperatingSystem.IsLinux() || WindowCenterPlacement.IsLikelyUnpositioned(Position))
        {
            PositionWindowOnOwnerScreen(_positionOwnerHint);
        }
    }

    private void AttachRevealOnOpen()
    {
        if (!RevealOnOpen)
        {
            Opacity = 1;
            return;
        }

        if (_revealOnOpenAttached)
        {
            return;
        }

        _revealOnOpenAttached = true;
        Opacity = 0;
        Opened += OnRevealAfterOpen;
    }

    /// <summary>
    /// На части Linux/Wayland WM Maximized до Show() не применяется — дублируем в Opened.
    /// </summary>
    private void AttachFullscreenFallback()
    {
        if (!IsFullScreenWindow)
        {
            return;
        }

        Opened += OnEnsureMaximizedOnOpen;
    }

    private void OnEnsureMaximizedOnOpen(object? sender, EventArgs e)
    {
        Opened -= OnEnsureMaximizedOnOpen;
        if (WindowState != WindowState.Maximized)
        {
            WindowState = WindowState.Maximized;
        }
    }

    private void OnRevealAfterOpen(object? sender, EventArgs e)
    {
        Opened -= OnRevealAfterOpen;
        Opacity = 1;
    }

    private Window? GetOwnerWindow()
    {
        var appLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var windows = appLifetime?.Windows;
        if (windows == null)
        {
            return null;
        }

        var candidateWindows = new List<Window>();
        foreach (var window in windows)
        {
            if (window != this && window.WindowState != WindowState.Minimized)
            {
                candidateWindows.Add(window);
            }
        }

        if (candidateWindows.Count > 0)
        {
            var notMainWindowCandidates = candidateWindows
                .Where(x => x.Name != "MainWindow")
                .ToList();

            return notMainWindowCandidates.Count > 0
                ? notMainWindowCandidates.LastOrDefault()
                : candidateWindows.LastOrDefault();
        }

        var mainWindow = appLifetime.MainWindow;
        if (mainWindow != null && mainWindow != this)
        {
            return mainWindow;
        }

        return null;
    }
}
