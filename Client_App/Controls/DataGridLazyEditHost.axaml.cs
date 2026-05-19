using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Models.Forms.Form1;
using System;
using System.Linq;

namespace Client_App.Controls;

/// <summary>
/// Ячейка DataGrid: по умолчанию только TextBlock, редактор создаётся по клику.
/// </summary>
public partial class DataGridLazyEditHost : UserControl
{
    private Border? _displayBorder;
    private ContentPresenter? _editPresenter;
    private Control? _editRoot;
    private Point? _pressPoint;
    private bool _isPointerPressedOnDisplay;
    private object? _editDataContext;
    private TextBox? _editTextBox;

    public static readonly StyledProperty<string?> DisplayTextProperty =
        AvaloniaProperty.Register<DataGridLazyEditHost, string?>(nameof(DisplayText));

    public static readonly StyledProperty<IDataTemplate?> EditTemplateProperty =
        AvaloniaProperty.Register<DataGridLazyEditHost, IDataTemplate?>(nameof(EditTemplate));

    public static readonly StyledProperty<bool> IsInEditModeProperty =
        AvaloniaProperty.Register<DataGridLazyEditHost, bool>(nameof(IsInEditMode), defaultValue: false);

    public string? DisplayText
    {
        get => GetValue(DisplayTextProperty);
        set => SetValue(DisplayTextProperty, value);
    }

    public IDataTemplate? EditTemplate
    {
        get => GetValue(EditTemplateProperty);
        set => SetValue(EditTemplateProperty, value);
    }

    public bool IsInEditMode
    {
        get => GetValue(IsInEditModeProperty);
        private set => SetValue(IsInEditModeProperty, value);
    }

    public DataGridLazyEditHost()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
        _displayBorder = this.FindControl<Border>("DisplayBorder");
        _editPresenter = this.FindControl<ContentPresenter>("EditPresenter");

        if (_displayBorder != null)
        {
            _displayBorder.PointerPressed += OnDisplayPointerPressed;
            _displayBorder.PointerReleased += OnDisplayPointerReleased;
        }

        if (_editPresenter != null)
            _editPresenter.AddHandler(InputElement.PointerReleasedEvent, OnEditPointerReleased, RoutingStrategies.Tunnel);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        ExitEditMode();
        base.OnDetachedFromVisualTree(e);
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        if (IsInEditMode)
        {
            if (DataContext == null)
            {
                base.OnDataContextChanged(e);
                return;
            }

            if (!IsSameRowContext(_editDataContext, DataContext))
                ExitEditMode();
        }

        base.OnDataContextChanged(e);
    }

    private static bool IsSameRowContext(object? previous, object? current)
    {
        if (ReferenceEquals(previous, current))
            return true;
        if (previous is Form11 prev && current is Form11 cur)
            return prev.NumberInOrder_DB == cur.NumberInOrder_DB;
        return false;
    }

    private void OnDisplayPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (IsInEditMode)
            return;

        var point = e.GetCurrentPoint(this);
        if (!point.Properties.IsLeftButtonPressed)
            return;

        _isPointerPressedOnDisplay = true;
        _pressPoint = point.Position;
    }

    private void OnDisplayPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!_isPointerPressedOnDisplay || IsInEditMode)
            return;

        _isPointerPressedOnDisplay = false;

        if (e.InitialPressMouseButton != MouseButton.Left)
            return;

        if (_pressPoint is { } pressPoint)
        {
            var releasePoint = e.GetCurrentPoint(this).Position;
            var dx = releasePoint.X - pressPoint.X;
            var dy = releasePoint.Y - pressPoint.Y;
            if (dx * dx + dy * dy > 16)
                return;
        }

        EnterEditMode(_pressPoint);
    }

    public void EnterEditMode(Point? clickPositionInHost = null)
    {
        if (IsInEditMode || EditTemplate == null || _editPresenter == null || _displayBorder == null)
            return;

        _editRoot = EditTemplate.Build(DataContext) as Control;
        if (_editRoot == null)
            return;

        _editPresenter.Content = _editRoot;
        _editPresenter.IsVisible = true;
        _displayBorder.IsVisible = false;
        IsInEditMode = true;
        _editDataContext = DataContext;
        AttachEditorHandlers();

        Dispatcher.UIThread.Post(
            () => FocusEditContent(clickPositionInHost),
            DispatcherPriority.Loaded);
    }

    private void AttachEditorHandlers()
    {
        DetachEditorHandlers();
        if (_editRoot == null)
            return;

        _editTextBox = _editRoot.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        if (_editTextBox == null)
            return;

        _editTextBox.SetValue(InputElement.IsHitTestVisibleProperty, true);
        _editTextBox.AddHandler(InputElement.PointerPressedEvent, OnEditorTextBoxPointerPressed, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        _editTextBox.AddHandler(InputElement.PointerReleasedEvent, OnEditorTextBoxPointerReleased, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
    }

    private void DetachEditorHandlers()
    {
        if (_editTextBox == null)
            return;

        _editTextBox.RemoveHandler(InputElement.PointerPressedEvent, OnEditorTextBoxPointerPressed);
        _editTextBox.RemoveHandler(InputElement.PointerReleasedEvent, OnEditorTextBoxPointerReleased);
        _editTextBox = null;
    }

    private void OnEditorTextBoxPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsInEditMode || sender is not TextBox textBox)
            return;

        textBox.SetValue(InputElement.IsHitTestVisibleProperty, true);
        textBox.Focus();
        e.Handled = false;
    }

    private void OnEditorTextBoxPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!IsInEditMode || sender is not TextBox textBox)
            return;

        if (e.InitialPressMouseButton != MouseButton.Left)
            return;

        textBox.Focus();
        var point = e.GetCurrentPoint(textBox);
        TrySetCaretFromPoint(textBox, point.Position);
        e.Handled = false;
    }

    private void FocusEditContent(Point? clickPositionInHost)
    {
        if (_editRoot == null)
            return;

        var textBox = _editTextBox ?? _editRoot.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        if (textBox != null)
        {
            textBox.SetValue(InputElement.IsHitTestVisibleProperty, true);
            textBox.Focus();
            if (clickPositionInHost is Point p)
                TrySetCaretFromPoint(textBox, p);
            return;
        }

        var autoComplete = _editRoot.GetVisualDescendants().OfType<global::Avalonia.Controls.AutoCompleteBox>().FirstOrDefault();
        if (autoComplete != null)
        {
            autoComplete.Focus();
            return;
        }

        if (_editRoot.Focusable)
            _editRoot.Focus();
        else
            _editRoot.GetVisualDescendants().OfType<InputElement>().FirstOrDefault(x => x.Focusable)?.Focus();
    }

    private void OnEditPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!IsInEditMode || _editRoot == null)
            return;

        if (e.InitialPressMouseButton != MouseButton.Left)
            return;

        var textBox = _editRoot.GetVisualDescendants().OfType<TextBox>().FirstOrDefault();
        if (textBox == null)
            return;

        var point = e.GetCurrentPoint(textBox);
        if (!textBox.Bounds.Contains(point.Position))
            return;

        textBox.Focus();
        TrySetCaretFromPoint(textBox, point.Position);
    }

    internal static void TrySetCaretFromPoint(TextBox textBox, Point pointInTextBox)
    {
        var text = textBox.Text ?? string.Empty;
        if (text.Length == 0)
            return;

        try
        {
            var width = textBox.Bounds.Width;
            if (width <= 0)
                return;

            var ratio = Math.Clamp(pointInTextBox.X / width, 0, 1);
            var index = (int)Math.Round(ratio * text.Length);
            textBox.CaretIndex = index;
            textBox.SelectionStart = index;
            textBox.SelectionEnd = index;
        }
        catch
        {
            // optional
        }
    }

    public void ExitEditMode()
    {
        if (!IsInEditMode || _editPresenter == null || _displayBorder == null)
            return;

        DetachEditorHandlers();
        _editPresenter.Content = null;
        _editPresenter.IsVisible = false;
        _editRoot = null;
        _displayBorder.IsVisible = true;
        IsInEditMode = false;
        _editDataContext = null;
        _isPointerPressedOnDisplay = false;
        _pressPoint = null;
    }

    public bool HasOpenOverlay()
    {
        if (_editRoot == null)
            return false;

        if (_editRoot.GetVisualDescendants().OfType<Popup>().Any(p => p.IsOpen))
            return true;

        foreach (var picker in _editRoot.GetVisualDescendants().OfType<CalendarDatePicker>())
        {
            if (picker.IsDropDownOpen)
                return true;
        }

        foreach (var autoComplete in _editRoot.GetVisualDescendants().OfType<global::Avalonia.Controls.AutoCompleteBox>())
        {
            if (autoComplete.IsDropDownOpen)
                return true;
        }

        return false;
    }
}
