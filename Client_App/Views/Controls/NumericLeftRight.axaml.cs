using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
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

    private TextBox? _valueTextBox;

    public NumericLeftRight()
    {
        Decrease = ReactiveCommand.Create(() =>
        {
            Value -= Increment;
            SyncTextBoxToValue();
            FlushFormPagingIfNeeded();
        });

        Increase = ReactiveCommand.Create(() =>
        {
            Value += Increment;
            SyncTextBoxToValue();
            FlushFormPagingIfNeeded();
        });
        InitializeComponent();
        AttachedToVisualTree += OnAttachedToVisualTree;
        DetachedFromVisualTree += OnDetachedFromVisualTree;
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

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        _valueTextBox = this.FindControl<TextBox>("ValueTextBox");
        if (_valueTextBox is null)
            return;

        _valueTextBox.LostFocus -= OnValueTextBoxLostFocus;
        _valueTextBox.LostFocus += OnValueTextBoxLostFocus;
        _valueTextBox.KeyDown -= OnValueTextBoxKeyDown;
        _valueTextBox.KeyDown += OnValueTextBoxKeyDown;
        SyncTextBoxToValue();
    }

    private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (_valueTextBox is null)
            return;

        _valueTextBox.LostFocus -= OnValueTextBoxLostFocus;
        _valueTextBox.KeyDown -= OnValueTextBoxKeyDown;
        _valueTextBox = null;
    }

    private void OnValueTextBoxLostFocus(object? sender, RoutedEventArgs e) =>
        CommitDisplayToValue();

    private void OnValueTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
            return;

        CommitDisplayToValue();
        FlushFormPagingIfNeeded();
        e.Handled = true;
    }

    /// <summary>
    /// Applies typed text (with clamp) and forces the box to show the page actually in use.
    /// </summary>
    private void CommitDisplayToValue()
    {
        if (_valueTextBox is null)
            return;

        if (int.TryParse(_valueTextBox.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed))
            Value = parsed;
        else
            SetCurrentValue(ValueProperty, CoerceValue(Value));

        SyncTextBoxToValue();
    }

    /// <summary>
    /// Keeps the editor text aligned with <see cref="Value"/> after clamp / filter / blur.
    /// </summary>
    private void SyncTextBoxToValue()
    {
        if (_valueTextBox is null)
            return;

        var text = Value.ToString(CultureInfo.InvariantCulture);
        if (_valueTextBox.Text != text)
            _valueTextBox.Text = text;

        DataValidationErrors.ClearErrors(_valueTextBox);
    }

    private int CoerceValue(int value)
    {
        var min = Minimum;
        var max = Maximum;
        if (max < min)
            max = min;

        // TotalPages can be 0 while lists load; keep a usable upper bound for the editor.
        if (max < 1 && min <= 1)
            max = 1;

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

            // Filter / form switch changed TotalPages — show the page that is actually active.
            SyncTextBoxToValue();
        }
        else if (change.Property == ValueProperty
                 && _valueTextBox is { IsFocused: false })
        {
            SyncTextBoxToValue();
        }
    }
}
