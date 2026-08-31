using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;
using Client_App.Services.AutoReplace;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using System.Linq;
using System.Reflection;

namespace Client_App.Behaviors.DataGridBehaviors;

/// <summary>
/// Автозамена для форм 1.x на стандартном Avalonia DataGrid (не DataGridGeneric).
/// </summary>
public class DataGridForm1AutoReplaceBehavior : Behavior<DataGrid>
{
    private const RoutingStrategies FocusRouting = RoutingStrategies.Tunnel | RoutingStrategies.Bubble;

    private readonly Form1AutoReplaceUiService _autoReplace = new();
    private Form1? _activeRow;
    private string? _activeColumn;
    private string? _activeOriginalDbValue;
    private Control? _activeEditor;
    private Form1? _lastCommittedRow;
    private string? _lastCommittedColumn;

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is null) return;

        AssociatedObject.AddHandler(InputElement.GotFocusEvent, OnGotFocus, FocusRouting);
        AssociatedObject.AddHandler(InputElement.LostFocusEvent, OnLostFocus, FocusRouting);
        AssociatedObject.CellPointerPressed += OnCellPointerPressed;
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.RemoveHandler(InputElement.GotFocusEvent, OnGotFocus);
            AssociatedObject.RemoveHandler(InputElement.LostFocusEvent, OnLostFocus);
            AssociatedObject.CellPointerPressed -= OnCellPointerPressed;
        }

        base.OnDetaching();
    }

    private void OnCellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
    {
        // Откладываем финализацию: при клике по dropdown AutoCompleteBox-а
        // SelectionChanged должен отработать первым и записать значение в модель.
        Dispatcher.UIThread.Post(FinalizeActiveEdit, DispatcherPriority.Background);
    }

    private void OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        if (AssociatedObject is null || e.Source is not Control control) return;
        if (!TryResolveEditContext(control, out var row, out var column)) return;

        if (_activeRow == row && _activeColumn == column) return;

        FinalizeActiveEdit();
        _activeRow = row;
        _activeColumn = column;
        _activeOriginalDbValue = GetColumnDbValue(row, column);
        _activeEditor = control;
        _lastCommittedRow = null;
        _lastCommittedColumn = null;
    }

    private void OnLostFocus(object? sender, RoutedEventArgs e)
    {
        if (AssociatedObject is null || e.Source is not Control control) return;
        if (!TryResolveEditContext(control, out _, out _)) return;

        if (AssociatedObject.IsKeyboardFocusWithin)
        {
            Dispatcher.UIThread.Post(FinalizeIfFocusLeftEditor, DispatcherPriority.Background);
            return;
        }

        // Откладываем и здесь, чтобы binding успел записать значение.
        Dispatcher.UIThread.Post(FinalizeActiveEdit, DispatcherPriority.Background);
    }

    private void FinalizeIfFocusLeftEditor()
    {
        if (AssociatedObject is null || _activeRow is null || _activeColumn is null) return;
        if (!AssociatedObject.IsKeyboardFocusWithin)
        {
            FinalizeActiveEdit();
            return;
        }

        var focused = TopLevel.GetTopLevel(AssociatedObject!)?.FocusManager?.GetFocusedElement() as Control;
        if (focused is null || !TryResolveEditContext(focused, out var row, out var column))
        {
            FinalizeActiveEdit();
            return;
        }

        if (row != _activeRow || column != _activeColumn)
        {
            FinalizeActiveEdit();
        }
    }

    private void FinalizeActiveEdit()
    {
        if (_activeRow is null || _activeColumn is null) return;

        if (_activeRow == _lastCommittedRow && _activeColumn == _lastCommittedColumn)
        {
            ClearActive();
            return;
        }

        // Не коммитим из редактора вручную — к этому моменту binding
        // (TwoWay на OperationCode.Value) уже записал значение в модель.
        // Читаем актуальное значение из _DB-поля.
        var currentDbValue = GetColumnDbValue(_activeRow, _activeColumn);

        if (currentDbValue != _activeOriginalDbValue)
        {
            _autoReplace.RunIfNeeded(_activeRow, _activeColumn);
        }

        _lastCommittedRow = _activeRow;
        _lastCommittedColumn = _activeColumn;
        ClearActive();
    }

    private void ClearActive()
    {
        _activeRow = null;
        _activeColumn = null;
        _activeOriginalDbValue = null;
        _activeEditor = null;
    }

    private static string GetColumnDbValue(Form1 row, string column)
    {
        var dbProp = row.GetType().GetProperty($"{column}_DB", BindingFlags.Instance | BindingFlags.Public);
        if (dbProp is not null)
        {
            return dbProp.GetValue(row) as string ?? string.Empty;
        }

        var prop = row.GetType().GetProperty(column, BindingFlags.Instance | BindingFlags.Public);
        if (prop?.GetValue(row) is RamAccess<string> ramAccess)
        {
            return ramAccess.Value ?? string.Empty;
        }

        return string.Empty;
    }

    private bool TryResolveEditContext(Control control, out Form1 row, out string column)
    {
        row = null!;
        column = "";

        if (control is not (TextBox or AutoCompleteBox)) return false;

        var cell = control.FindAncestorOfType<DataGridCell>();
        var dataGridRow = cell?.FindAncestorOfType<DataGridRow>();
        if (dataGridRow?.DataContext is not Form1 form1) return false;

        var formNum = GetFormNum();
        var columnIndex = cell is null ? -1 : GetColumnIndex(cell);
        if (!Form1AutoReplaceColumnMaps.TryGetColumnBinding(formNum, columnIndex, out column)) return false;

        row = form1;
        return true;
    }

    private string? GetFormNum()
    {
        if (AssociatedObject is null) return null;
        return Interaction.GetBehaviors(AssociatedObject)
            .OfType<DataGridColumnWidthLoadBehavior>()
            .Select(b => b.FormNum)
            .FirstOrDefault(n => !string.IsNullOrEmpty(n));
    }

    private static int GetColumnIndex(DataGridCell cell)
    {
        var row = cell.FindAncestorOfType<DataGridRow>();
        if (row is null) return -1;

        var cells = row.GetVisualDescendants().OfType<DataGridCell>().ToList();
        return cells.IndexOf(cell);
    }
}
