using Avalonia;
using Avalonia.Controls;

namespace Client_App.Behaviors;

/// <summary>Совместимость для App.axaml и views, ещё не переведённых на Client_App.Behaviors.Controls в XAML.</summary>
public class ScrollBlock
{
    public static readonly AttachedProperty<bool> BlockPointerWheelProperty =
        Controls.ScrollBlock.BlockPointerWheelProperty;

    public static bool GetBlockPointerWheel(Control element) =>
        Controls.ScrollBlock.GetBlockPointerWheel(element);

    public static void SetBlockPointerWheel(Control element, bool value) =>
        Controls.ScrollBlock.SetBlockPointerWheel(element, value);
}

/// <summary>Совместимость для кода, ссылающегося на Client_App.Behaviors.BlockPointerWheelBehavior.</summary>
public class BlockPointerWheelBehavior : Controls.BlockPointerWheelBehavior;
