using Avalonia;
using Avalonia.Controls;

namespace Client_App.Behaviors;

/// <summary>Совместимость для App.axaml и views, ещё не переведённых на Client_App.Behaviors.Input в XAML.</summary>
public static class TextBoxAutoTrim
{
    public static readonly AttachedProperty<bool> IsEnabledProperty =
        Input.TextBoxAutoTrim.IsEnabledProperty;

    public static bool GetIsEnabled(TextBox textBox) =>
        Input.TextBoxAutoTrim.GetIsEnabled(textBox);

    public static void SetIsEnabled(TextBox textBox, bool value) =>
        Input.TextBoxAutoTrim.SetIsEnabled(textBox, value);
}
