using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Forms;
using ReactiveUI;
using System.Windows.Input;

namespace Client_App.Views.Controls;

public partial class NumericLeftRight : UserControl
{
    #region Property

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
            FlushFormPagingIfNeeded();
        });

        Increase = ReactiveCommand.Create(() =>
        {
            Value += Increment;
            FlushFormPagingIfNeeded();
        });
        InitializeComponent();
    }

    /// <summary>
    /// Buttons should not wait for typed-page debounce on BaseFormVM.
    /// </summary>
    private void FlushFormPagingIfNeeded()
    {
        if (DataContext is BaseFormVM vm)
            vm.FlushPendingPagingRefresh();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private int CoerceValue(int value)
    {
        var min = Minimum;
        var max = Maximum;
        if (max < min)
            max = min;

        if (value < min)
            return min;
        if (value > max)
            return max;
        return value;
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == MaximumProperty || change.Property == MinimumProperty)
        {
            var coerced = CoerceValue(Value);
            if (coerced != Value)
                SetCurrentValue(ValueProperty, coerced);
        }
    }
}
