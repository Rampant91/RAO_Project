using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.ReactiveUI;
using Avalonia.Threading;
using Client_App.Behaviors.WindowSizing;
using Client_App.Interfaces.Logger;
using Client_App.ViewModels;
using Client_App.ViewModels.Forms;
using Client_App.Views.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Client_App.Views;

public interface IFormDialogHost
{
    Task ShowFormDialogAsync(Window? owner);
}

/// <summary>
/// Состояние MainWindow до открытия формы (для восстановления при закрытии / смене отчёта).
/// </summary>
public interface IFormOwnerStateWindow
{
    WindowState OwnerPrevState { get; set; }
}

public abstract class BaseWindow<T> : ReactiveWindow<BaseVM>, IFormDialogHost, IFormOwnerStateWindow where T : class
{
    public T? VM => DataContext as T;
    public WindowState OwnerPrevState { get; set; }

    protected virtual bool IsFullScreenWindow => false;

    /// <summary>Некоторые окна (прогрессбар) показываются сразу без fade-in.</summary>
    protected virtual bool RevealOnOpen => true;

    private bool _revealOnOpenAttached;
    private bool _openedPositionFallbackAttached;
    private bool _fullscreenFallbackAttached;
    private bool _formContentLoadingOverlayAttached;
    private Window? _positionOwnerHint;

    protected BaseWindow()
    {
        Opened += OnOpenedAttachFormContentLoadingOverlay;
    }

    private void OnOpenedAttachFormContentLoadingOverlay(object? sender, EventArgs e)
    {
        Opened -= OnOpenedAttachFormContentLoadingOverlay;
        TryAttachFormContentLoadingOverlay();
    }

    /// <summary>
    /// Overlay «Загрузка…» для <see cref="BaseFormVM"/>.
    /// Добавляем в существующий корневой Grid/Panel — без замены Content,
    /// иначе ломается NameScope и ElementName-синхронизация многоуровневой шапки.
    /// </summary>
    private void TryAttachFormContentLoadingOverlay()
    {
        if (_formContentLoadingOverlayAttached)
            return;
        if (DataContext is not BaseFormVM)
            return;
        if (Content is not Control existingContent)
            return;

        var overlay = new DataGridLoadingOverlay
        {
            ZIndex = 1000,
            IsVisible = false,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            IsHitTestVisible = true,
        };
        overlay.Bind(IsVisibleProperty, new Binding(nameof(BaseFormVM.IsContentLoading)));

        if (existingContent is Grid rootGrid)
        {
            var rowSpan = Math.Max(1, rootGrid.RowDefinitions.Count);
            var colSpan = Math.Max(1, rootGrid.ColumnDefinitions.Count);
            Grid.SetRow(overlay, 0);
            Grid.SetColumn(overlay, 0);
            Grid.SetRowSpan(overlay, rowSpan);
            Grid.SetColumnSpan(overlay, colSpan);
            rootGrid.Children.Add(overlay);
            _formContentLoadingOverlayAttached = true;
            return;
        }

        if (existingContent is Panel rootPanel)
        {
            rootPanel.Children.Add(overlay);
            _formContentLoadingOverlayAttached = true;
        }
    }

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

        WindowScreenSizeBehavior.RefreshWindowSize(this, owner);

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
            PositionWindowOnOwnerScreen(owner ?? GetPreferredOwnerWindow());
            AttachOpenedPositionFallback(owner);
        }

        AttachRevealOnOpen(owner);

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
        var ownerWindow = owner ?? GetPreferredOwnerWindow();
        PrepareBeforeShow(ownerWindow);
        AttachFullscreenFallback();

        if (!IsFullScreenWindow)
        {
            PositionWindowOnOwnerScreen(ownerWindow);
            AttachOpenedPositionFallback(ownerWindow);
        }

        AttachRevealOnOpen(ownerWindow);
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
            var owner = ownerWindow ?? GetPreferredOwnerWindow();
            WindowCenterPlacement.TryCenterOnScreen(this, owner);
            WindowScreenContext.TryEnsureVisibleOnScreen(this, owner);
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

        // Всегда: шапка не должна оказаться за верхним краем working area (Form_10 и др.).
        WindowScreenContext.TryEnsureVisibleOnScreen(this, _positionOwnerHint);
    }

    private void AttachRevealOnOpen(Window? ownerHint)
    {
        if (!RevealOnOpen)
        {
            Opacity = 1;
            ClearReportOpeningOverlay(ownerHint);
            return;
        }

        if (_revealOnOpenAttached)
        {
            return;
        }

        _revealOnOpenAttached = true;
        _positionOwnerHint ??= ownerHint;
        Opacity = 0;
        Opened += OnRevealAfterOpen;
    }

    /// <summary>
    /// На части Linux/Wayland WM Maximized до Show() не применяется — дублируем в Opened.
    /// </summary>
    private void AttachFullscreenFallback()
    {
        if (!IsFullScreenWindow || _fullscreenFallbackAttached)
        {
            return;
        }

        _fullscreenFallbackAttached = true;
        Opened += OnEnsureMaximizedOnOpen;
    }

    private void OnEnsureMaximizedOnOpen(object? sender, EventArgs e)
    {
        Opened -= OnEnsureMaximizedOnOpen;
        _fullscreenFallbackAttached = false;
        if (WindowState != WindowState.Maximized)
        {
            WindowState = WindowState.Maximized;
        }
    }

    private void OnRevealAfterOpen(object? sender, EventArgs e)
    {
        Opened -= OnRevealAfterOpen;

        // Пока Opacity=0: дожимаем геометрию (Linux), затем показываем кадр после layout.
        if (IsFullScreenWindow && WindowState != WindowState.Maximized)
        {
            WindowState = WindowState.Maximized;
        }
        else if (!IsFullScreenWindow)
        {
            if (OperatingSystem.IsLinux() || WindowCenterPlacement.IsLikelyUnpositioned(Position))
            {
                PositionWindowOnOwnerScreen(_positionOwnerHint);
            }

            WindowScreenContext.TryEnsureVisibleOnScreen(this, _positionOwnerHint);
        }

        var ownerHint = _positionOwnerHint;
        // ContextIdle: дать гриду/layout первый кадр до снятия overlay (слабые ПК).
        Dispatcher.UIThread.Post(() =>
        {
            if (!IsFullScreenWindow)
            {
                WindowScreenContext.TryEnsureVisibleOnScreen(this, ownerHint);
            }

            Opacity = 1;
            ClearReportOpeningOverlay(ownerHint);
        }, DispatcherPriority.ContextIdle);
    }

    private static void ClearReportOpeningOverlay(Window? ownerHint)
    {
        if (ownerHint is MainWindow mainWindow)
        {
            mainWindow.SetReportOpeningOverlay(false);
            return;
        }

        var appLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        if (appLifetime?.MainWindow is MainWindow mw)
        {
            mw.SetReportOpeningOverlay(false);
        }
    }

    /// <summary>
    /// Предпочтительный owner для экрана: MainWindow (текущий монитор), иначе последнее видимое окно.
    /// </summary>
    private Window? GetPreferredOwnerWindow()
    {
        var appLifetime = Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime;
        var mainWindow = appLifetime?.MainWindow;
        if (mainWindow != null && mainWindow != this && mainWindow.WindowState != WindowState.Minimized)
        {
            return mainWindow;
        }

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

        return candidateWindows.LastOrDefault();
    }
}
