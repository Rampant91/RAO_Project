using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using Client_App.Resources.CustomComparers;
using System;
using System.Collections;

namespace Client_App.Behaviors.DataGridBehaviors;

public class DataGridSortingBehavior : Behavior<DataGrid>
{
    private DataGridColumn? _lastSortedColumn;
    private bool _isSortedAscending = true;

    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject != null)
        {
            AssociatedObject.Sorting += OnSorting;
        }
    }

    protected override void OnDetaching()
    {
        if (AssociatedObject != null)
        {
            AssociatedObject.Sorting -= OnSorting;
        }
        base.OnDetaching();
    }

    private void OnSorting(object sender, DataGridColumnEventArgs e)
    {
        if (e.Column == null)
            return;

        e.Handled = true;

        // ���� ��� ������ ���������� � �� ��������� �� �������, �� ��������� �� ������� �� ��������
        if (_lastSortedColumn is null && e.Column.SortMemberPath is "Index")
        {
            _lastSortedColumn = e.Column;
            _isSortedAscending = false;
        }
        // ���� � ��������� ��� ����������� ��� ������� - ��������� � �������� �����������
        else if (_lastSortedColumn == e.Column)
        {
            _isSortedAscending = !_isSortedAscending;
        }
        // ��������� �� ����������� ��������� �������
        else
        {
            _lastSortedColumn = e.Column;
            _isSortedAscending = true;
        }

        if (AssociatedObject?.ItemsSource is not IList { Count: > 0 } items)
            return;

        var array = new object[items.Count];
        items.CopyTo(array, 0);

        // Use custom sorter for columns with SortMemberPath
        var comparer = new CustomCheckFormWindowComparer(e.Column.SortMemberPath, _isSortedAscending);
        Array.Sort(array, comparer);

        // Update the items source
        AssociatedObject.ItemsSource = array;
    }
}