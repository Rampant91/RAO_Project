using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client_App.Views.Controls;

public partial class FormNumButton : UserControl
{
    public static readonly StyledProperty<string> FormNumProperty =
        AvaloniaProperty.Register<FormNumButton, string>(
            nameof(FormNum),
            defaultValue: "");
    public string FormNum
    {
        get => GetValue(FormNumProperty);
        set => SetValue(FormNumProperty, value);
    }
    public FormNumButton()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
