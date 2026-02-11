using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Windows.Input;

namespace Client_App.Views.Controls;

public partial class NumericLeftRight : UserControl
{
    #region Property
    // Определение StyledProperty для Value
    public static readonly StyledProperty<int> ValueProperty =
        AvaloniaProperty.Register<NumericLeftRight, int>(
            nameof(Value),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay);

    public int Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, CoerceValue(value));
    }

    // Свойство Minimum
    public static readonly StyledProperty<int> MinimumProperty =
        AvaloniaProperty.Register<NumericLeftRight, int>(
            nameof(Minimum),
            defaultValue: int.MinValue,
            defaultBindingMode: BindingMode.TwoWay);

    public int Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    // Свойство Maximum
    public static readonly StyledProperty<int> MaximumProperty =
        AvaloniaProperty.Register<NumericLeftRight, int>(
            nameof(Maximum),
            defaultValue: int.MaxValue,
            defaultBindingMode: BindingMode.TwoWay);

    public int Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    // Свойство Increment
    public static readonly StyledProperty<int> IncrementProperty =
        AvaloniaProperty.Register<NumericLeftRight, int>(
            nameof(Increment),
            defaultValue: 1);

    public int Increment
    {
        get => GetValue(IncrementProperty);
        set => SetValue(IncrementProperty, value);
    }

    #endregion

    #region Commands
    public ICommand Decrease { get; set; }
    public ICommand Increase { get; set; }
        
    #endregion

    public NumericLeftRight()
    {
        Decrease = ReactiveCommand.Create(() =>
        {
            Value -= Increment;
        });

        Increase = ReactiveCommand.Create(() =>
        {
            Value += Increment;
        });
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

    }
    // Метод для ограничения значения в пределах Minimum/Maximum
    private int CoerceValue(int value)
    {
        if (value < Minimum)
            return Minimum;
        if (value > Maximum)
            return Maximum;
        return value;
    }
}