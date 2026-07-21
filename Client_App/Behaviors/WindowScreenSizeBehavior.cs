using Avalonia;
using Avalonia.Controls;

namespace Client_App.Behaviors;

/// <summary>Совместимость для views, ещё не переведённых на Client_App.Behaviors.WindowSizing в XAML.</summary>
public static class WindowScreenSizeBehavior
{
    public static readonly AttachedProperty<double> WidthRatioProperty =
        WindowSizing.WindowScreenSizeBehavior.WidthRatioProperty;

    public static readonly AttachedProperty<double> HeightRatioProperty =
        WindowSizing.WindowScreenSizeBehavior.HeightRatioProperty;

    public static double GetWidthRatio(Window element) =>
        WindowSizing.WindowScreenSizeBehavior.GetWidthRatio(element);

    public static void SetWidthRatio(Window element, double value) =>
        WindowSizing.WindowScreenSizeBehavior.SetWidthRatio(element, value);

    public static double GetHeightRatio(Window element) =>
        WindowSizing.WindowScreenSizeBehavior.GetHeightRatio(element);

    public static void SetHeightRatio(Window element, double value) =>
        WindowSizing.WindowScreenSizeBehavior.SetHeightRatio(element, value);
}
