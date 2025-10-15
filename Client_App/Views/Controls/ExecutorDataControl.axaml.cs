using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Client_App.Controls;
using System.Diagnostics;
using System.Numerics;

namespace Client_App.Control;

public partial class ExecutorDataControl : UserControl
{

    private readonly ExecutorData _executorData;

    public static readonly StyledProperty<string> FIOProperty =
           AvaloniaProperty.Register<ExecutorDataControl, string>(
               nameof(FIO),
               defaultBindingMode: BindingMode.TwoWay);
    public string FIO
    {
        get => GetValue(FIOProperty);
        set => SetValue(FIOProperty, value);
    }
    public static readonly StyledProperty<string> GradeProperty =
           AvaloniaProperty.Register<ExecutorDataControl, string>(
               nameof(Grade),
               defaultBindingMode: BindingMode.TwoWay);
    public string Grade
    {
        get => GetValue(GradeProperty);
        set => SetValue(GradeProperty, value);
    }
    public static readonly StyledProperty<string> PhoneProperty =
           AvaloniaProperty.Register<ExecutorDataControl, string>(
               nameof(Phone),
               defaultBindingMode: BindingMode.TwoWay);
    public string Phone
    {
        get => GetValue(PhoneProperty);
        set => SetValue(PhoneProperty, value);
    }
    public static readonly StyledProperty<string> EmailProperty =
           AvaloniaProperty.Register<ExecutorDataControl, string>(
               nameof(Email),
               defaultBindingMode: BindingMode.TwoWay);
    public string Email
    {
        get => GetValue(EmailProperty);
        set => SetValue(EmailProperty, value);
    }


    public ExecutorDataControl()
    {
        InitializeComponent();
    }
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

}



// Пока используется только здесь, потом можно вынести в отдельный файл
public class ExecutorData
{
    public string FIO;
    public string Grade;
    public string Phone;
    public string Email;
}