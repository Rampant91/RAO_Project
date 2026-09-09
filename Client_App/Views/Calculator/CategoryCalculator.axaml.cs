using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Client_App.ViewModels.Calculator;
using Models.DTO;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace Client_App.Views.Calculator;

public partial class CategoryCalculator : BaseWindow<CategoryCalculatorVM>
{
    public readonly CategoryCalculatorVM VM = null!;

    #region Constructor

    public CategoryCalculator()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public CategoryCalculator(CategoryCalculatorVM vm)
    {
        AvaloniaXamlLoader.Load(this);
        this.AttachDevTools();
        VM = vm;
        DataContext = vm;
        vm.PropertyChanged += OnVmPropertyChanged;
        Opened += (_, _) => SyncPerNuclideActivityColumnVisibility();
        SyncPerNuclideActivityColumnVisibility();
    }

    #endregion

    #region Activity column visibility

    private DataGrid? _selectedRadsDataGrid;
    private DataGridColumn? _perNuclideActivityColumn;
    private int _perNuclideActivityInsertIndex = 2;

    private void OnVmPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is null or nameof(CategoryCalculatorVM.IsSingleActivity))
            SyncPerNuclideActivityColumnVisibility();
    }

    /// <summary>
    /// Avalonia often fails to re-show a DataGridColumn that started (or became) IsVisible=false.
    /// Insert/remove from Columns is reliable; shared "total activity" field still uses IsVisible binding.
    /// </summary>
    private void SyncPerNuclideActivityColumnVisibility()
    {
        if (VM is null)
            return;

        var grid = _selectedRadsDataGrid ??= this.FindControl<DataGrid>("SelectedRadsDataGrid");
        if (grid is null)
            return;

        if (!TryResolvePerNuclideActivityColumn(grid))
            return;

        var column = _perNuclideActivityColumn!;
        var shouldShow = !VM.IsSingleActivity;
        var isInGrid = grid.Columns.Contains(column);

        if (shouldShow)
        {
            if (isInGrid)
            {
                column.IsVisible = true;
                return;
            }

            var index = Math.Clamp(_perNuclideActivityInsertIndex, 0, grid.Columns.Count);
            grid.Columns.Insert(index, column);
            column.IsVisible = true;
            return;
        }

        if (!isInGrid)
            return;

        _perNuclideActivityInsertIndex = Math.Max(0, grid.Columns.IndexOf(column));
        grid.Columns.Remove(column);
    }

    private bool TryResolvePerNuclideActivityColumn(DataGrid grid)
    {
        if (_perNuclideActivityColumn is not null)
            return true;

        if (grid.Columns.Count == 0)
            return false;

        _perNuclideActivityColumn =
            grid.Columns.FirstOrDefault(c => Equals(c.Tag, "PerNuclideActivity"))
            ?? (grid.Columns.Count > 2 ? grid.Columns[2] : null);

        if (_perNuclideActivityColumn is null)
            return false;

        _perNuclideActivityInsertIndex = grid.Columns.IndexOf(_perNuclideActivityColumn);
        return true;
    }

    #endregion

    #region Events

    private void OnAddButtonClicked(object? sender, RoutedEventArgs e)
    {
        var categoryCalculatorVM = (DataContext as CategoryCalculatorVM)!;
        if (categoryCalculatorVM.SelectedDictionaryNuclid is not null 
            && !categoryCalculatorVM.SelectedRadionuclids.Contains(categoryCalculatorVM.SelectedDictionaryNuclid))
        {
            categoryCalculatorVM.SelectedRadionuclids?.Add(categoryCalculatorVM.SelectedDictionaryNuclid);

            categoryCalculatorVM.SelectedRadionuclids 
                = new ObservableCollection<CalculatorRadionuclidDTO>(categoryCalculatorVM.SelectedRadionuclids
                    .OrderBy(x => x.Name));
        }
    }

    private void OnCalculateButtonClicked(object? sender, RoutedEventArgs e)
    {
        VM.CategoryCalculation.Execute(null);
    }

    private void OnClearButtonClicked(object? sender, RoutedEventArgs e)
    {
        var categoryCalculatorVM = (DataContext as CategoryCalculatorVM)!;

        if (categoryCalculatorVM.RadionuclidDictionary != null)
        {
            foreach (var nuclid in categoryCalculatorVM.SelectedRadionuclids)
            {
                categoryCalculatorVM.RadionuclidDictionary
                    .First(x => x.Name == nuclid.Name)
                    .Activity = string.Empty;
            }
        }
        categoryCalculatorVM.SelectedRadionuclids.Clear();
        categoryCalculatorVM.SelectedNuclid = null;
    }

    private void OnCloseButtonClicked(object? sender, RoutedEventArgs e)
    {
        Close();
    }

    private void OnRemoveButtonClicked(object? sender, RoutedEventArgs e)
    {
        var categoryCalculatorVM = (DataContext as CategoryCalculatorVM)!;
        if (categoryCalculatorVM.SelectedNuclid is not null && categoryCalculatorVM.RadionuclidDictionary is not null)
        {
            categoryCalculatorVM.RadionuclidDictionary
                .First(x => x.Name == categoryCalculatorVM.SelectedNuclid.Name)
                .Activity = string.Empty;
            categoryCalculatorVM.SelectedRadionuclids.Remove(categoryCalculatorVM.SelectedNuclid);
            
            if (categoryCalculatorVM.SelectedRadionuclids.Count == 0) categoryCalculatorVM.SelectedNuclid = null;
        }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private protected void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion

    private void InputElement_OnLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            textBox.Text = ExponentialString(textBox.Text);
        }
    }

    #region ExponentialString

    private protected static string ExponentialString(string value)
    {
        var tmp = (value ?? string.Empty)
            .Trim()
            .ToLower()
            .Replace('е', 'e');
        tmp = ReplaceDashes(tmp);
        if (tmp != "прим.")
        {
            tmp = tmp.Replace('.', ',');
        }
        if (tmp is "прим." or "-")
        {
            return tmp;
        }
        var doubleStartsWithBrackets = false;
        if (tmp.StartsWith('(') && tmp.EndsWith(')'))
        {
            doubleStartsWithBrackets = true;
            tmp = tmp
                .TrimStart('(')
                .TrimEnd(')');
        }
        var tmpNumWithoutSign = tmp.StartsWith('+') || tmp.StartsWith('-')
            ? tmp[1..]
            : tmp;
        var sign = tmp.StartsWith('-')
            ? "-"
            : string.Empty;
        if (!tmp.Contains('e')
            && tmpNumWithoutSign.Count(x => x is '+' or '-') == 1)
        {
            tmp = sign + tmpNumWithoutSign.Replace("+", "e+").Replace("-", "e-");
        }
        if (double.TryParse(tmp,
                NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent | NumberStyles.AllowLeadingSign,
                new CultureInfo("ru-RU", useUserOverride: false),
                out var doubleValue))
        {
            tmp = $"{doubleValue:0.######################################################e+00}";
        }
        return doubleStartsWithBrackets
            ? $"({tmp})"
            : tmp;
    }

    private protected static string ReplaceDashes(string value) =>
        value switch
        {
            null => string.Empty,
            _ => DashesRegex().Replace(value, "-")
        };

    #endregion

    #region Regex

    [GeneratedRegex("[-᠆‐‑‒–—―⸺⸻－﹘﹣－]")]
    protected static partial Regex DashesRegex();

    #endregion
}