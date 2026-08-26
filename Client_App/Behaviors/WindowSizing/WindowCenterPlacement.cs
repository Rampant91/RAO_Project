using Avalonia;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Client_App.Behaviors.WindowSizing;

/// <summary>
/// Центрирование окна на экране owner / текущего MainWindow (см. <see cref="WindowScreenContext"/>).
/// </summary>
internal static class WindowCenterPlacement
{
    internal static Screen? ResolveOwnerScreen(Window window, Window? ownerWindow) =>
        WindowScreenContext.ResolveTargetScreen(window, ownerWindow);

    internal static bool TryCenterOnScreen(Window window, Window? ownerWindow) =>
        WindowScreenContext.TryCenterOnScreen(window, ownerWindow);

    internal static bool IsLikelyUnpositioned(PixelPoint position) =>
        WindowScreenContext.IsLikelyUnpositioned(position);
}
