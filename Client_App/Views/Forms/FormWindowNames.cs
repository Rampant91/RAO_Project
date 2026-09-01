using Avalonia;
using Avalonia.Controls;
using System;
using System.Linq;

namespace Client_App.Views.Forms;

/// <summary>
/// Lookup key for open form windows. Do not set <see cref="StyledElement.Name"/> in Avalonia 11
/// (invalid identifiers like 1.0, and Name cannot change after styling).
/// Do not call <c>AvaloniaXamlLoader.Load</c> from this helper: Avalonia 11 rewrites
/// <c>Load(this)</c> only inside the x:Class type; a helper always hits the stub that throws
/// "No precompiled XAML found".
/// </summary>
public static class FormWindowNames
{
    public static readonly AttachedProperty<string?> WindowKeyProperty =
        AvaloniaProperty.RegisterAttached<Control, string?>("WindowKey", typeof(FormWindowNames));

    public static void SetWindowKey(Control control, string? value) => control.SetValue(WindowKeyProperty, value);
    public static string? GetWindowKey(Control control) => control.GetValue(WindowKeyProperty);

    public static string ToWindowName(string formNum)
        => "Form_" + formNum.Replace(".", string.Empty, StringComparison.Ordinal);

    public static Window? FindOpenFormWindow(string formNum)
    {
        var windowName = ToWindowName(formNum);
        var desktop = Avalonia.Application.Current?.ApplicationLifetime
            as Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime;

        return desktop?.Windows.FirstOrDefault(w => MatchesWindowKey(w, windowName));
    }

    private static bool MatchesWindowKey(Window window, string windowName)
    {
        if (string.Equals(GetWindowKey(window), windowName, StringComparison.Ordinal))
            return true;

        return string.Equals(window.Name, windowName, StringComparison.Ordinal);
    }
}
