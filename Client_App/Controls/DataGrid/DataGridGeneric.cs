using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Models.Attributes;
using Models.Collections;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using Client_App.VisualRealization.Converters;
using Microsoft.EntityFrameworkCore;
using Models.DBRealization;
using Models.Forms;

namespace Client_App.Controls.DataGrid;

#region DataGridEnums

public enum ChooseMode
{
    Cell = 0,
    Line
}

public enum MultilineMode
{
    Multi = 0,
    Single
}

#endregion

public class DataGrid<T> : UserControl, IDataGrid where T : class, IKey, IDataGridColumn, new()
{
    #region Items

    public static readonly DirectProperty<DataGrid<T>, IKeyCollection> ItemsProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, IKeyCollection>(
            nameof(Items),
            o => o.Items,
            (o, v) => o.Items = v, defaultBindingMode: BindingMode.TwoWay);

    private IKeyCollection _items;

    public IKeyCollection Items
    {
        get => _items;
        set
        {
            if (value != null)
            {
                if (_items != value)
                {
                    NowPage = "1";
                }
                SetAndRaise(ItemsProperty, ref _items, value);
                UpdateCells();
                SetSelectedControls();
            }
        }
    }

    #endregion

    #region SelectedItems

    public static readonly DirectProperty<DataGrid<T>, IKeyCollection> SelectedItemsProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, IKeyCollection>(
            nameof(SelectedItems),
            o => o.SelectedItems,
            (o, v) => o.SelectedItems = v);
    private IKeyCollection _selecteditems =
        new ObservableCollectionWithItemPropertyChanged<IKey>();
    public IKeyCollection SelectedItems
    {
        get => _selecteditems;
        set
        {
            //if (value != null && value.Count != 0)    Убирает сброс выбранной организации при перелистывании страниц и поиске
            {
                SetAndRaise(SelectedItemsProperty, ref _selecteditems, value);
                ReportStringCount = "0";
                if (Sum) SumColumn = "0";
            }
        }
    }

    #endregion

    #region SelectedCells

    public static readonly DirectProperty<DataGrid<T>, IList<Control>> SelectedCellsProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, IList<Control>>(
            nameof(SelectedCells),
            o => o.SelectedCells,
            (o, v) => o.SelectedCells = v);

    private IList<Control> _selectedCells = new List<Control>();
    public IList<Control> SelectedCells
    {
        get => _selectedCells;
        set
        {
            if (value != null)
            {
                SetAndRaise(SelectedCellsProperty, ref _selectedCells, value);
            }
            UpdateCells();
        }
    }

    #endregion

    #region Type

    public static readonly DirectProperty<DataGrid<T>, string> TypeProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(Type),
            o => o.Type,
            (o, v) => o.Type = v);
    public string Type
    {
        get => typeof(T).Name;
        set { }
    }

    #endregion

    #region CommentСhangeable

    public static readonly DirectProperty<DataGrid<T>, bool> CommentСhangeableProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(CommentСhangeable),
            o => o.CommentСhangeable,
            (o, v) => o.CommentСhangeable = v);

    private bool _CommentСhangeable;
    public bool CommentСhangeable
    {
        get => _CommentСhangeable;
        set
        {
            SetAndRaise(CommentСhangeableProperty, ref _CommentСhangeable, value);
            Init();
        }
    }

    #endregion

    #region Comment

    public static readonly DirectProperty<DataGrid<T>, string> CommentProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(Comment),
            o => o.Comment,
            (o, v) => o.Comment = v);
    private string _Comment = "";
    public string Comment
    {
        get => _Comment;
        set
        {
            if (value != null && value != _Comment)
            {
                SetAndRaise(CommentProperty, ref _Comment, value);
                Init();
            }
        }
    }

    #endregion

    #region Search

    public static readonly DirectProperty<DataGrid<T>, bool> SearchProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(Search),
            o => o.Search,
            (o, v) => o.Search = v);

    private bool _Search;
    public bool Search
    {
        get => _Search;
        set
        {
            if (value != null)
            {
                SetAndRaise(SearchProperty, ref _Search, value);
                Init();
            }
        }
    }

    #endregion

    #region Sum

    public static readonly DirectProperty<DataGrid<T>, bool> SumProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(Sum),
            o => o.Sum,
            (o, v) => o.Sum = v);

    private bool _Sum;
    public bool Sum
    {
        get => _Sum;
        set
        {
            SetAndRaise(SumProperty, ref _Sum, value);
            Init();
        }
    }

    #endregion

    #region ShowAllReport

    public static readonly DirectProperty<DataGrid<T>, bool> ShowAllReportProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(ShowAllReport),
            o => o.ShowAllReport,
            (o, v) => o.ShowAllReport = v);

    private bool _ShowAllReport;
    public bool ShowAllReport
    {
        get => _ShowAllReport;
        set
        {
            SetAndRaise(ShowAllReportProperty, ref _ShowAllReport, value);
            //Init();
        }
    }

    #endregion

    #region SumColumn

    public static readonly DirectProperty<DataGrid<T>, string> SumColumnProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(SumColumn),
            o => o.SumColumn,
            (o, v) => o.SumColumn = v);

    private string _SumColumn = "0";
    public string SumColumn
    {
        get => _SumColumn;
        set
        {
            var s = GetSum();
            SetAndRaise(SumColumnProperty, ref _SumColumn, s);
        }
    }

    private string GetSum()
    {
        var answ = new object[3];
        answ[0] = SelectedItems;
        answ[1] = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
        answ[2] = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
        var minColumn = Convert.ToInt32(answ[1]) + 1;
        var maxColumn = Convert.ToInt32(answ[2]) + 1;
        var _s = 0.0;
        if (minColumn == maxColumn && answ[0] is IKeyCollection collection)
        {
            foreach (var item in collection.GetEnumerable().OrderBy(x => x.Order))
            {
                var props = item.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var attr = (FormPropertyAttribute)prop.GetCustomAttributes(typeof(FormPropertyAttribute), false).FirstOrDefault();
                    if (attr == null) continue;
                    try
                    {
                        var columnNum = Convert.ToInt32(attr.Number);
                        if (columnNum >= minColumn && columnNum <= maxColumn)
                        {
                            var midValue = prop.GetMethod.Invoke(item, null);
                            var _value = midValue.GetType().GetProperty("Value").GetMethod.Invoke(midValue, null);
                            if (_value != null && _value != " ")
                            {
                                try
                                {
                                    _value = _value.ToString().Replace("е", "e").Replace("Е", "E").Replace(".", ",");
                                    _s += Convert.ToDouble(_value);
                                    var stackPanel = (StackPanel)((StackPanel)((Border)((Grid)((Panel)Content).Children[0]).Children[2]).Child).Children[0];
                                    stackPanel.Children[0].IsVisible = true;
                                    stackPanel.Children[1].IsVisible = true;
                                }
                                catch
                                {
                                    var stackPanel = (StackPanel)((StackPanel)((Border)((Grid)((Panel)Content).Children[0]).Children[2]).Child).Children[0];
                                    stackPanel.Children[0].IsVisible = false;
                                    stackPanel.Children[1].IsVisible = false;
                                    return null;
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
        }
        else
        {
            var stackPanel = (StackPanel)((StackPanel)((Border)((Grid)((Panel)Content).Children[0]).Children[2]).Child).Children[0];
            stackPanel.Children[0].IsVisible = false;
            stackPanel.Children[1].IsVisible = false;
            return null;
        }
        return _s.ToString();
    }

    #endregion

    #region ChooseMode

    public static readonly StyledProperty<ChooseMode> ChooseModeProperty =
        AvaloniaProperty.Register<DataGrid<T>, ChooseMode>(nameof(ChooseMode));

    public ChooseMode ChooseMode
    {
        get => GetValue(ChooseModeProperty);
        set => SetValue(ChooseModeProperty, value);
    }

    #endregion

    #region MultilineMode

    public static readonly StyledProperty<MultilineMode> MultilineModeProperty =
        AvaloniaProperty.Register<DataGrid<T>, MultilineMode>(nameof(MultilineMode));
    public MultilineMode MultilineMode
    {
        get => GetValue(MultilineModeProperty);
        set => SetValue(MultilineModeProperty, value);
    }

    #endregion

    #region ChooseColor
    public static readonly StyledProperty<Brush> ChooseColorProperty =
        AvaloniaProperty.Register<DataGrid<T>, Brush>(nameof(ChooseColor));
    public Brush ChooseColor
    {
        get => GetValue(ChooseColorProperty);
        set => SetValue(ChooseColorProperty, value);
    }
    #endregion

    #region Pagination

    public static readonly DirectProperty<DataGrid<T>, bool> PaginationProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(Pagination),
            o => o.Pagination,
            (o, v) => o.Pagination = v);

    private bool _pagination = true;
    public bool Pagination
    {
        get => _pagination;
        set
        {
            SetAndRaise(PaginationProperty, ref _pagination, value);
            UpdateCells();
        }
    }

    #endregion

    #region IsReadable

    public static readonly DirectProperty<DataGrid<T>, bool> IsReadableProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(IsReadable),
            o => o.IsReadable,
            (o, v) => o.IsReadable = v);

    private bool _IsReadable;
    public bool IsReadable
    {
        get => _IsReadable;
        set
        {
            SetAndRaise(IsReadableProperty, ref _IsReadable, value);
            Init();
        }
    }

    #endregion

    #region IsAutoSizable

    public static readonly DirectProperty<DataGrid<T>, bool> IsAutoSizableProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(IsAutoSizable),
            o => o.IsAutoSizable,
            (o, v) => o.IsAutoSizable = v);

    private bool _IsAutoSizable;
    public bool IsAutoSizable
    {
        get => _IsAutoSizable;
        set
        {
            SetAndRaise(IsAutoSizableProperty, ref _IsAutoSizable, value);
            Init();
        }
    }

    #endregion

    #region IsReadableSum

    public static readonly DirectProperty<DataGrid<T>, bool> IsReadableSumProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(IsReadableSum),
            o => o.IsReadableSum,
            (o, v) => o.IsReadableSum = v);

    private bool _IsReadableSum;
    public bool IsReadableSum
    {
        get => _IsReadableSum;
        set
        {
            SetAndRaise(IsReadableSumProperty, ref _IsReadableSum, value);
            Init();
        }
    }

    #endregion

    #region IsColumnResize

    public static readonly DirectProperty<DataGrid<T>, bool> IsColumnResizeProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
            nameof(IsColumnResize),
            o => o.IsColumnResize,
            (o, v) => o.IsColumnResize = v);

    private bool _IsColumnResize = true;
    public bool IsColumnResize
    {
        get => _IsColumnResize;
        set
        {
            SetAndRaise(IsColumnResizeProperty, ref _IsColumnResize, value);
            Init();
        }
    }

    #endregion

    #region PageSize

    public static readonly DirectProperty<DataGrid<T>, int> PageSizeProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, int>(
            nameof(PageSize),
            o => o.PageSize,
            (o, v) => o.PageSize = v);

    private int _pageSize = 30;
    public int PageSize
    {
        get => _pageSize;
        set
        {
            SetAndRaise(PageSizeProperty, ref _pageSize, value);
            Init();
        }
    }

    #endregion

    #region FixedContent

    public static readonly DirectProperty<DataGrid<T>, Thickness> FixedContentProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, Thickness>(
            nameof(FixedContent),
            o => o.FixedContent,
            (o, v) => o.FixedContent = v);

    private Thickness _FixedContent = Thickness.Parse("5,0,0,0");
    public Thickness FixedContent
    {
        get => _FixedContent;
        set => SetAndRaise(FixedContentProperty, ref _FixedContent, value);
    }

    #endregion

    #region ScrollLeftRight

    public static readonly DirectProperty<DataGrid<T>, int> ScrollLeftRightProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, int>(
            nameof(ScrollLeftRight),
            o => o.ScrollLeftRight,
            (o, v) => o.ScrollLeftRight = v);

    private int _ScrollLeftRight;
    public int ScrollLeftRight
    {
        get => _ScrollLeftRight;
        set => SetAndRaise(ScrollLeftRightProperty, ref _ScrollLeftRight, value);
    }

    #endregion

    #region Count

    #region ItemsCount

    public static readonly DirectProperty<DataGrid<T>, string> ItemsCountProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(ItemsCount),
            o => o.ItemsCount,
            (o, v) => o.ItemsCount = v);

    private string _ItemsCount = "0";
    public string ItemsCount
    {
        get => _ItemsCount;
        set
        {
            if (Items != null)
            {
                var searchText = Regex.Replace(SearchText.ToLower(), "[-.?!)(,: ]", "");
                var val = searchText == "" 
                    ? Items.Count 
                    : _itemsWithSearch != null 
                        ? _itemsWithSearch.Count 
                        : 0;
                SetAndRaise(ItemsCountProperty, ref _ItemsCount, val.ToString());
            }
        }
    }

    #endregion

    #region PageCount

    public static readonly DirectProperty<DataGrid<T>, string> PageCountProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(PageCount),
            o => o.PageCount,
            (o, v) => o.PageCount = v);

    private string _PageCount = "0";
    public string PageCount
    {
        get => _PageCount;
        set
        {
            var pageCount = Items != null
                ? SearchText == ""
                    ? Items.Count % PageSize == 0
                        ? Items.Count / PageSize
                        : Items.Count / PageSize + 1
                    : _itemsWithSearch != null
                        ? _itemsWithSearch.Count == 0
                            ? 0
                            : _itemsWithSearch.Count <= PageSize
                                ? 1
                                : _itemsWithSearch.Count % PageSize == 0
                                    ? _itemsWithSearch.Count / PageSize
                                    : _itemsWithSearch.Count / PageSize + 1
                        : Items.Count % PageSize == 0
                            ? Items.Count / PageSize
                            : Items.Count / PageSize + 1
                : 0;
            SetAndRaise(PageCountProperty, ref _PageCount, pageCount.ToString());
        }
    }

    #endregion

    #region ReportCount

    public static readonly DirectProperty<DataGrid<T>, string> ReportCountProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(ReportCount),
            o => o.ReportCount,
            (o, v) => o.ReportCount = v);

    private string _ReportCount = "0";
    public string ReportCount
    {
        get => _ReportCount;
        set
        {
            if (Items != null)
            {
                var countR = 0;
                var searchText = Regex.Replace(SearchText.ToLower(), "[-.?!)(,: ]", "");
                foreach (var item in searchText != "" && _itemsWithSearch != null ? _itemsWithSearch : Items)
                {
                    var reps = (Reports)item;
                    countR += reps.Report_Collection.Count;
                }
                SetAndRaise(ReportCountProperty, ref _ReportCount, countR.ToString());
            }
        }
    }

    #endregion

    #region ReportStringCount

    public static readonly DirectProperty<DataGrid<T>, string> ReportStringCountProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(ReportStringCount),
            o => o.ReportStringCount,
            (o, v) => o.ReportStringCount = v);

    private string _ReportStringCount = "0";
    public string ReportStringCount
    {
        get => _ReportStringCount;
        set
        {
            if (SelectedItems is null || SelectedItems.Count == 0)
            {
                if (ReportStringCountProperty is not "0")
                {
                    SetAndRaise(ReportStringCountProperty, ref _ReportStringCount, "0");
                }
                return;
            }

            var rep = SelectedItems.Get<Report>(0);
            if (rep is null) return;
            var countR = ReportsStorage.GetReportRowsCount(rep);
            SetAndRaise(ReportStringCountProperty, ref _ReportStringCount, countR.ToString());
        }
    }

    #endregion

    #endregion

    #region NowPage

    private static readonly DirectProperty<DataGrid<T>, string> NowPageProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(NowPage),
            o => o.NowPage,
            (o, v) => o.NowPage = v, defaultBindingMode: BindingMode.TwoWay);

    private string _nowPage = "1";
    public string NowPage
    {
        get => _nowPage;
        set
        {
            try
            {
                var val = Convert.ToInt32(value);
                if (val == null) return;
                var searchText = Regex.Replace(SearchText.ToLower(), "[-.?!)(,: ]", "");
                var maxPage = searchText == ""
                    ? Items != null
                        ? Items.Count % PageSize == 0
                            ? Items.Count / PageSize
                            : Items.Count / PageSize + 1
                        : 1
                    : _itemsWithSearch != null
                        ? _itemsWithSearch.Count == 0
                            ? 1
                            : _itemsWithSearch.Count % PageSize == 0
                                ? _itemsWithSearch.Count / PageSize
                                : _itemsWithSearch.Count / PageSize + 1
                        : 1;
                if (maxPage == 0) maxPage = 1;
                if (val.ToString() == _nowPage) return;
                if (val <= maxPage && val >= 1)
                {
                    SetAndRaise(NowPageProperty, ref _nowPage, value);
                    UpdateCells();
                }
                else
                {
                    if (val > maxPage)
                    {
                        if (_nowPage != maxPage.ToString())
                        {
                            SetAndRaise(NowPageProperty, ref _nowPage, maxPage.ToString());
                            UpdateCells();
                        }
                    }
                    if (val < 1 && _nowPage != "1")
                    {
                        SetAndRaise(NowPageProperty, ref _nowPage, "1");
                        UpdateCells();
                    }
                }
            }
            catch
            {
                // ignored
            }
        }
    }

    private void NowPageDown(object sender, RoutedEventArgs args)
    {
        NowPage = (Convert.ToInt32(NowPage) - 1).ToString();
    }

    private void NowPageUp(object sender, RoutedEventArgs args)
    {
        NowPage = (Convert.ToInt32(NowPage) + 1).ToString();
    }

    #endregion

    #region CommandsList

    private static readonly DirectProperty<DataGrid<T>, ObservableCollection<KeyCommand>> CommandsListProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, ObservableCollection<KeyCommand>>(
            nameof(CommandsList),
            o => o.CommandsList,
            (o, v) => o.CommandsList = v);

    private ObservableCollection<KeyCommand> _CommandsList = new();
    public ObservableCollection<KeyCommand> CommandsList
    {
        get => _CommandsList;
        set => SetAndRaise(CommandsListProperty, ref _CommandsList, value);
    }
    private void CommandListChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        if (args.NewItems.Cast<KeyCommand>().Any(item => item.IsContextMenuCommand))
        {
            MakeContextMenu();
        }
    }
    private object GetParamByParamName(KeyCommand param)
    {
        switch (param.ParamName)
        {
            case "SelectedItems":
                return SelectedItems;
            case "SelectedCells":
                return SelectedCells;
            case "FormType":
                return SelectedCells;
            case "Copy":
                {
                    var answ = new object[3];
                    answ[0] = SelectedItems;
                    answ[1] = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
                    answ[2] = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
                    return answ;
                }
            case "Paste" or "Del":
                {
                    var answ = new object[3];
                    answ[0] = SelectedItems;
                    answ[1] = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
                    answ[2] = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
                    return answ;
                }
            case "SelectAll":
                {
                    var maxRow = PageSize;
                    var maxColumn = Rows[0].Children.Count;
                    FirstPressedItem[0] = 0;
                    FirstPressedItem[1] = 0;
                    LastPressedItem[0] = maxRow - 1;
                    LastPressedItem[1] = maxColumn - 1;
                    SetSelectedControls();
                    ObservableCollectionWithItemPropertyChanged<IKey> lst = new();
                    foreach (var item in Items)
                    {
                        lst.Add(item);
                    }
                    SelectedItems = lst;
                    break;
                }
        }

        if (string.IsNullOrEmpty(param.ParamName))
        {
            return param.Param;
        }
        if (param.ParamName is "1.0" or "2.0")
        {
            return param.ParamName;
        }
        return null;
    }

    private void CommandTapped(object sender, RoutedEventArgs args)
    {
        if (sender == null) return;
        var selectItem = (string)((MenuItem)sender).Header;
        if (selectItem == null) return;
        var rt = CommandsList
            .Where(item => item.IsContextMenuCommand && item.ContextMenuText.Contains(selectItem));
        foreach (var item in rt)
        {
            item.DoCommand(GetParamByParamName(item));
            if (item.ContextMenuText[0].Contains("Удалить форму"))
            {
                SelectedItems = null;
            }
            if (item.IsUpdateCells)
            {
                UpdateCells();
            }
        }
    }

    #endregion

    #region Columns
    private DataGridColumns _Columns;
    private DataGridColumns Columns
    {
        get
        {
            if (_Columns == null)
            {
                var t = new T();
                var tmp = Name switch
                {
                    "Form1AllDataGrid_" => "1.0",
                    "Form2AllDataGrid_" => "2.0",
                    _ => ""
                };
                _Columns = t.GetColumnStructure(tmp);
            }
            return _Columns;
        }
        set
        {
            if (_Columns != value)
            {
                _Columns = value;
                Init();
            }
        }
    }
    #endregion

    private IKeyCollection _itemsWithSearch;

    #region SearchText
    public static readonly DirectProperty<DataGrid<T>, string> SearchTextProperty =
        AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
            nameof(SearchText),
            o => o.SearchText,
            (o, v) => o.SearchText = v);
    private string _SearchText = "";
    public string SearchText
    {
        get => _SearchText;
        set
        {
            if (value != null && value != _SearchText)
            {
                SetAndRaise(SearchTextProperty, ref _SearchText, value);
                UpdateCells();
            }
        }
    }
    #endregion

    private List<DataGridRow> Rows { get; } = new();

    private StackPanel HeaderStackPanel { get; set; }
    private StackPanel CenterStackPanel { get; set; }

    protected DataGrid(string name = "")
    {
        this.Name = name;
        Focusable = true;
        FirstPressedItem[0] = -1;
        FirstPressedItem[1] = -1;
        LastPressedItem[0] = -1;
        LastPressedItem[1] = -1;

        AddHandler(PointerPressedEvent, MousePressed, handledEventsToo: true);
        AddHandler(PointerMovedEvent, MouseMoved, handledEventsToo: true);
        AddHandler(PointerReleasedEvent, MouseReleased, handledEventsToo: true);
        AddHandler(DoubleTappedEvent, MouseDoublePressed, handledEventsToo: true);
        AddHandler(KeyDownEvent, OnDataGridKeyDown, handledEventsToo: true);

        _CommandsList.CollectionChanged += CommandListChanged;
    }

    #region SetSelectedControls
    private void SetSelectedControls()
    {
        if (Items == null || Items.Count == 0) return;
        if (ChooseMode == ChooseMode.Cell)
        {
            if (MultilineMode == MultilineMode.Multi) SetSelectedControls_CellMulti();
            if (MultilineMode == MultilineMode.Single) SetSelectedControls_CellSingle();
        }

        if (ChooseMode == ChooseMode.Line)
        {
            if (MultilineMode == MultilineMode.Multi) SetSelectedControls_LineMulti();
            if (MultilineMode == MultilineMode.Single) SetSelectedControls_LineSingle();
        }
    }

    private void SetSelectedControls_LineSingle()
    {
        var row = LastPressedItem[0];
        if (row != -1)
        {
            var tmp1 = Rows.SelectMany(x => x.Children).Where(item => ((Cell)item).Row != row);

            foreach (var control in tmp1)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)Background;
            }
            SelectedCells.Clear();
            ObservableCollectionWithItemPropertyChanged<IKey> tmpSelectedItems = new();
            var tmp2 = Rows
                .Where(x => x.IsVisible)
                .SelectMany(x => x.Children)
                .Where(item => ((Cell)item).Row == row);
            foreach (var control in tmp2)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)ChooseColor;
                SelectedCells.Add(item);
                tmpSelectedItems.Add((T)item.DataContext);
            }
            //tmpSelectedItems.Add((T)((Cell)tmp2.FirstOrDefault()).DataContext);
            SelectedItems = tmpSelectedItems;
        }
        else
        {
            var tmp1 = Rows.SelectMany(x => x.Children);
            foreach (var control in tmp1)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)Background;
            }
            SelectedCells.Clear();
            SelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();
        }
    }

    //Not Done
    private void SetSelectedControls_CellSingle()
    {
        var row = LastPressedItem[0];
        var column = LastPressedItem[1];

        if (row != -1 && column != -1)
        {

            var tmp1 = Rows
                .Where(item => ((Cell)item.Children.FirstOrDefault()).Row != row 
                               && ((Cell)item.Children.FirstOrDefault()).Column != column);
            foreach (var item in tmp1)
            {
                item.ChooseColor = (SolidColorBrush)Background;
            }

            SelectedCells.Clear();

            ObservableCollectionWithItemPropertyChanged<IKey> tmpSelectedItems = new();

            var tmp2 = Rows
                .Where(item => ((Cell)item.Children.FirstOrDefault()).Row == row
                               && ((Cell)item.Children.FirstOrDefault()).Column == column);
            foreach (var item in tmp2)
            {
                item.ChooseColor = (SolidColorBrush)ChooseColor;
                SelectedCells.Add(item);
                tmpSelectedItems.Add((T)item.DataContext);
            }
            SelectedItems = tmpSelectedItems;
        }
        else
        {
            var tmp1 = Rows.SelectMany(x => x.Children);

            foreach (var control in tmp1)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)Background;
            }
            SelectedCells.Clear();
            SelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();
        }
    }

    private void SetSelectedControls_LineMulti()
    {
        var minRow = Math.Min(FirstPressedItem[0], LastPressedItem[0]);
        var maxRow = Math.Max(FirstPressedItem[0], LastPressedItem[0]);
        if (minRow != -1 && maxRow != -1)
        {
            var tmp1 = Rows
                .SelectMany(x => x.Children)
                .Where(item => !(((Cell)item).Row >= minRow && ((Cell)item).Row <= maxRow));
            foreach (var control in tmp1)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)Background;
            }

            SelectedCells.Clear();
            ObservableCollectionWithItemPropertyChanged<IKey> tmpSelectedItems = new();
            var tmp2 = Rows
                .SelectMany(x => x.Children)
                .Where(item => ((Cell)item).Row >= minRow && ((Cell)item).Row <= maxRow);
            foreach (var control in tmp2)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)ChooseColor;
                SelectedCells.Add(item);
                tmpSelectedItems.Add((T)item.DataContext);
            }
            SelectedItems = tmpSelectedItems;
        }
        else
        {
            var tmp1 = Rows.SelectMany(x => x.Children);
            foreach (var control in tmp1)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)Background;
            }
            SelectedCells.Clear();
            SelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();
        }
    }

    private void SetSelectedControls_CellMulti()
    {
        var minRow = Math.Min(FirstPressedItem[0], LastPressedItem[0]);
        var maxRow = Math.Max(FirstPressedItem[0], LastPressedItem[0]);
        var minColumn = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
        var maxColumn = Math.Max(FirstPressedItem[1], LastPressedItem[1]);

        if (minRow != -1 && maxRow != -1 && minColumn != -1 && maxColumn != -1)
        {
            var tmp1 = Rows
                .SelectMany(x => x.Children)
                .Where(item => !(((Cell)item).Row >= minRow 
                                 && ((Cell)item).Row <= maxRow 
                                 && ((Cell)item).Column >= minColumn 
                                 && ((Cell)item).Column <= maxColumn));
            foreach (var control in tmp1)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)Background;
            }
            SelectedCells.Clear();
            Dictionary<long, ObservableCollectionWithItemPropertyChanged<IKey>> tmpSelectedItems = new();
            var tmp2 = Rows
                .SelectMany(x => x.Children)
                .Where(item => ((Cell)item).Row >= minRow
                               && ((Cell)item).Row <= maxRow
                               && ((Cell)item).Column >= minColumn
                               && ((Cell)item).Column <= maxColumn);
            foreach (var control in tmp2)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)ChooseColor;
                SelectedCells.Add(item);
                if (item.DataContext is T dataContext)
                {
                    if (!tmpSelectedItems.ContainsKey(dataContext.Order))
                    {
                        tmpSelectedItems.Add(dataContext.Order, new ObservableCollectionWithItemPropertyChanged<IKey>());
                    }
                }
                else
                {
                    break;
                }
                tmpSelectedItems[((T)item.DataContext).Order].Add((T)item.DataContext);
            }
            var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>();
            foreach (var value in tmpSelectedItems
                         .Select(item => item.Value.FirstOrDefault())
                         .Where(value => value != null))
            {
                tmp.Add(value);
            }
            SelectedItems = tmp;
        }
        else
        {
            var tmp1 = Rows.SelectMany(x => x.Children);
            foreach (var control in tmp1)
            {
                var item = (Cell)control;
                item.ChooseColor = (SolidColorBrush)Background;
            }
            SelectedCells.Clear();
            SelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();
        }
    }
    #endregion
    
    //Всё что касается работы с мышью
    #region DataGridPoiter

    private int[] FirstPressedItem { get; } = new int[2];
    private int[] LastPressedItem { get; set; } = new int[2];

    #region SetFirstPressed

    private bool SetFirstPressed(IReadOnlyList<int> first)
    {
        if (FirstPressedItem[0] == first[0] && FirstPressedItem[1] == first[1]) return false;
        FirstPressedItem[0] = first[0];
        FirstPressedItem[1] = first[1];
        return true;
    }

    #endregion

    #region SetLastPressed

    private bool SetLastPressed(IReadOnlyList<int> last)
    {
        if (LastPressedItem[0] == last[0] && LastPressedItem[1] == last[1]) return false;
        LastPressedItem[0] = last[0];
        LastPressedItem[1] = last[1];
        return true;
    }

    #endregion

    #region FindMousePressed

    private int[] FindMousePress(IReadOnlyList<double> mouse)
    {
        var tmp = new int[2];
        var sumy = 0.0;
        var flag = false;
        var doFlag = false;
        foreach (var item in Rows)
        {
            sumy += item.Bounds.Height;
            if (!(mouse[0] <= sumy)) continue;
            if (mouse[0] >= 0)
            {
                var sumx = 0.0;
                foreach (var it in item.Children.Cast<Cell?>())
                {
                    sumx += it.Bounds.Width + 2;    //плюс 2 добавил опытным путем, чтобы выделялась правильная ячейка
                    if (!(mouse[1] <= sumx)) continue;
                    if (mouse[1] >= 0)
                    {
                        tmp[0] = it.Row;
                        tmp[1] = it.Column;
                        flag = true;
                        doFlag = true;
                        break;
                    }
                    flag = true;
                    break;
                }
                if (flag) break;
            }
            else break;
        }
        if (!doFlag)
        {
            tmp[0] = -1;
            tmp[1] = -1;
        }
        return tmp;
    }

    #endregion

    #region MousePressed
    
    private void MousePressed(object sender, PointerPressedEventArgs args)
    {
        var paramKey = args.GetCurrentPoint(this).Properties.PointerUpdateKind;
        var paramPos = args.GetCurrentPoint(CenterStackPanel).Position;
        var doSetItemFlag = false;

        if (paramKey is not (PointerUpdateKind.LeftButtonPressed or PointerUpdateKind.RightButtonPressed)) return;
        var paramRowColumn = FindMousePress(new[] { paramPos.Y, paramPos.X });
        if (paramKey == PointerUpdateKind.RightButtonPressed)
        {
            var minRow = Math.Min(FirstPressedItem[0], LastPressedItem[0]);
            var maxRow = Math.Max(FirstPressedItem[0], LastPressedItem[0]);
            var minColumn = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
            var maxColumn = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
            if (paramRowColumn[0] <= minRow || paramRowColumn[0] >= maxRow)
            {
                if (ChooseMode != ChooseMode.Line)
                {
                    if (paramRowColumn[0] == minRow || paramRowColumn[0] == maxRow)
                    {
                        if (paramRowColumn[1] < minColumn || paramRowColumn[1] > maxColumn)
                        {
                            doSetItemFlag = SetFirstPressed(paramRowColumn);
                            if (doSetItemFlag)
                            {
                                LastPressedItem[0] = FirstPressedItem[0];
                                LastPressedItem[1] = FirstPressedItem[1];
                            }
                            doSetItemFlag = doSetItemFlag || SetLastPressed(paramRowColumn);
                        }
                    }
                    else
                    {
                        doSetItemFlag = SetFirstPressed(paramRowColumn);
                        if (doSetItemFlag)
                        {
                            LastPressedItem[0] = FirstPressedItem[0];
                            LastPressedItem[1] = FirstPressedItem[1];
                        }
                        doSetItemFlag = doSetItemFlag || SetLastPressed(paramRowColumn);
                    }
                }
                else
                {
                    doSetItemFlag = SetFirstPressed(paramRowColumn);
                    if (doSetItemFlag)
                    {
                        LastPressedItem[0] = FirstPressedItem[0];
                        LastPressedItem[1] = FirstPressedItem[1];
                    }
                    doSetItemFlag = doSetItemFlag || SetLastPressed(paramRowColumn);
                }
            }
            if (FirstPressedItem[0] != -1)
            {
                ContextMenu.Close();
                
                var tmp1 = (Cell)Rows
                    .SelectMany(x => x.Children)
                    .FirstOrDefault(item => ((Cell)item).Row == paramRowColumn[0]
                                            && ((Cell)item).Column == paramRowColumn[1])!;
                ContextMenu.PlacementTarget = tmp1;
                ContextMenu.Open();
            }
        }
        else
        {
            ContextMenu.Close();
            doSetItemFlag = SetFirstPressed(paramRowColumn);
            if (doSetItemFlag)
            {
                LastPressedItem[0] = FirstPressedItem[0];
                LastPressedItem[1] = FirstPressedItem[1];
            }
            doSetItemFlag = doSetItemFlag || SetLastPressed(paramRowColumn);
        }

        if (!doSetItemFlag) return;
        {
            SetSelectedControls();
            if (paramKey != PointerUpdateKind.LeftButtonPressed) return;
            var item = (Cell)SelectedCells.FirstOrDefault();
            if (item?.Control is TextBox ctrl)
            {
                ctrl.Focus();
                ctrl.SelectAll();
                var num = 0;
                if (ctrl.Text != null)
                {
                    num = ctrl.Text.Length;
                }
                ctrl.CaretIndex = num - 1;
            }
        }
    }

    #endregion

    #region MouseDoublePressed
    
    private void MouseDoublePressed(object sender, EventArgs args)
    {
        if (FirstPressedItem[0] == -1) return;
        var commands = _CommandsList
            .Where(item => item.IsDoubleTappedCommand)
            .ToList();
        foreach (var item in commands)
        {
            item.DoCommand(GetParamByParamName(item));
        }
    }

    #endregion

    #region MouseReleased
    
    private void MouseReleased(object sender, PointerReleasedEventArgs args)
    {
        var paramKey = args.GetCurrentPoint(this).Properties.PointerUpdateKind;
        var paramPos = args.GetCurrentPoint(CenterStackPanel).Position;

        if (paramKey != PointerUpdateKind.LeftButtonReleased) return;
        var paramRowColumn = FindMousePress(new[] { paramPos.Y, paramPos.X });
        if (LastPressedItem[0] != paramRowColumn[0] || LastPressedItem[1] != paramRowColumn[1])
        {
            LastPressedItem = paramRowColumn;
            ScrollLeftRight = 0;
            SetSelectedControls();
        }
    }

    #endregion

    #region MouseMoved
    
    private void MouseMoved(object sender, PointerEventArgs args)
    {
        var paramKey = args.GetCurrentPoint(this).Properties;
        var paramPos = args.GetCurrentPoint(CenterStackPanel).Position;

        if (!paramKey.IsLeftButtonPressed) return;
        var paramRowColumn = FindMousePress(new[] { paramPos.Y, paramPos.X });
        if (LastPressedItem[0] == paramRowColumn[0] && LastPressedItem[1] == paramRowColumn[1]) return;
        var pr = ((Panel)Content).Bounds.Width;
        if (LastPressedItem[1] < paramRowColumn[1] && paramPos.X > pr / 4)
        {
            ScrollLeftRight += 100;
        }
        if (LastPressedItem[1] > paramRowColumn[1] && (paramPos.X < pr - 700 || paramPos.X < pr / 4))
        {
            ScrollLeftRight -= 50;
        }
        LastPressedItem = paramRowColumn;
        SetSelectedControls();
        //else
        //{
        //var paramRowColumn = FindMousePress(new double[] { paramPos.Y, paramPos.X });
        //if (paramPos.X > 100)
        //{
        //    FixedContentN += 20;
        //}
        //if (paramPos.X < 0)
        //{
        //    FixedContentN -= 20;
        //}
        //}
    }

    #endregion

    #endregion

    #region UpdateCells

    private void UpdateCells()
    {
        var count = 0;
        var num = Convert.ToInt32(_nowPage);
        var offset = (num - 1) * PageSize;
        var offsetMax = num * PageSize;
        if (Items != null)
        {
            var tmpColl = new ObservableCollectionWithItemPropertyChanged<IKey>(Items.GetEnumerable());
            if (Search)
            {
                var tmp2Coll = new ObservableCollectionWithItemPropertyChanged<IKey>();
                var searchText = ((TextBox)
                    ((Panel)
                        ((Border)
                            ((Grid)
                                ((Panel)
                                    Content).
                                Children[0]).
                            Children[0]).
                        Child).
                    Children[0]).Text;
                if (!string.IsNullOrEmpty(searchText))
                {
                    num = Convert.ToInt32(_nowPage);
                    offset = (num - 1) * PageSize;
                    offsetMax = num * PageSize;
                    searchText = Regex.Replace(searchText.ToLower(), "[-.?!)(,: ]", "");
                    if (searchText != "")
                    {
                        foreach (var it in tmpColl)
                        {
                            var rowsText = ((Reports)it).Master_DB.OkpoRep.Value +
                                           ((Reports)it).Master_DB.ShortJurLicoRep.Value +
                                           ((Reports)it).Master_DB.RegNoRep.Value;
                            rowsText = rowsText.ToLower();
                            rowsText = Regex.Replace(rowsText, "[-.?!)(,: ]", "");
                            if (rowsText.Contains(searchText))
                            {
                                tmp2Coll.Add(it);
                            }
                        }
                        if (int.Parse(NowPage) > (tmp2Coll.Count / PageSize + 1))
                        {
                            SetAndRaise(NowPageProperty, ref _nowPage, "1");
                            offsetMax = 5;
                            UpdateCells();
                        }
                        tmpColl = tmp2Coll;
                        _itemsWithSearch = tmp2Coll;
                    }
                }
            }
            if (tmpColl.Count != 0)
            {
                for (var i = offset; i < offsetMax; i++)
                {
                    if (count < PageSize && i < tmpColl.Count)
                    {
                        Rows[count].DataContext = null; // правит баг с записью данных в пустые ячейки на первых страницах
                        Rows[count].DataContext = tmpColl.Get<T>(i);
                        Rows[count].IsVisible = true;
                        count++;
                    }
                    else break;
                }
            }
            if (tmpColl.Count < offsetMax)
            {
                for (var i = tmpColl.Count; i < offsetMax; i++)
                {
                    try
                    {
                        if (Rows[i - offset].IsVisible)
                        {
                            Rows[i - offset].IsVisible = false;
                        }
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
            var t = typeof(T)
                .FindInterfaces((x, y) => x.ToString() == y.ToString(), typeof(IBaseColor).FullName);
            if (t.Length != 0)
            {
                for (var i = 0; i < PageSize; i++)
                {
                    if (Rows[i].DataContext is not IBaseColor baseColor) continue;
                    var _t = baseColor;
                    if (_t == null) continue;
                    var tmp2 = Rows
                        .SelectMany(x => x.Children)
                        .Where(item => ((Cell)item).Row == i)
                        .ToList();
                    var index = (int)_t.BaseColor;
                    var color = IBaseColor.ColorTypeList[index];
                    var solidColorBrush = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));

                    foreach (var item in tmp2.Cast<Cell?>())
                    {
                        item.Background = solidColorBrush;
                    }
                }
            }
        }

        if (ShowAllReport)
        {
            ReportCount = "0";
        }
        //NowPage = "0";
        PageCount = "0";
        ItemsCount = "0";

    }
    #endregion

    #region KeyDown

    private void OnDataGridKeyDown(object sender, KeyEventArgs args)
    {
        switch (args.Key)
        {
            case Key.Left:
            {
                LastPressedItem[1] = LastPressedItem[1] == 1 
                    ? LastPressedItem[1] 
                    : LastPressedItem[1] - 1;
                if (args.KeyModifiers != KeyModifiers.Shift)
                {
                    FirstPressedItem[0] = LastPressedItem[0];
                    FirstPressedItem[1] = LastPressedItem[1];
                }
                SetSelectedControls();
                var item = (Cell)SelectedCells.FirstOrDefault();
                if (item is { Control: TextBox ctrl })
                {
                    ctrl.Focus();
                    ctrl.SelectAll();
                    var num = 0;
                    if (ctrl.Text != null)
                    {
                        num = ctrl.Text.Length;
                    }
                    ctrl.CaretIndex = num - 1;
                }

                break;
            }
            case Key.Right:
            {
                if (LastPressedItem[1] != Rows[0].Children.Count - 1)
                    LastPressedItem[1]++;
                if (args.KeyModifiers != KeyModifiers.Shift)
                {
                    FirstPressedItem[0] = LastPressedItem[0];
                    FirstPressedItem[1] = LastPressedItem[1];
                }
                SetSelectedControls();
                var item = (Cell)SelectedCells.FirstOrDefault();
                if (item is { Control: TextBox ctrl })
                {
                    ctrl.Focus();
                    ctrl.SelectAll();
                    var num = 0;
                    if (ctrl.Text != null)
                    {
                        num = ctrl.Text.Length;
                    }
                    ctrl.CaretIndex = num - 1;
                }

                break;
            }
            case Key.Down:
            {
                LastPressedItem[0] = LastPressedItem[0] == Rows.Count - 1 || LastPressedItem[0] == Items.Count - 1 ? LastPressedItem[0] : LastPressedItem[0] + 1;
                if (args.KeyModifiers != KeyModifiers.Shift)
                {
                    FirstPressedItem[0] = LastPressedItem[0];
                    FirstPressedItem[1] = LastPressedItem[1];
                }
                SetSelectedControls();
                var item = (Cell)SelectedCells.FirstOrDefault();
                if (item is { Control: TextBox ctrl })
                {
                    ctrl.Focus();
                    ctrl.SelectAll();
                    var num = 0;
                    if (ctrl.Text != null)
                    {
                        num = ctrl.Text.Length;
                    }
                    ctrl.CaretIndex = num - 1;
                }

                break;
            }
            case Key.Up:
            {
                LastPressedItem[0] = LastPressedItem[0] == 0 ? LastPressedItem[0] : LastPressedItem[0] - 1;
                if (args.KeyModifiers != KeyModifiers.Shift)
                {
                    FirstPressedItem[0] = LastPressedItem[0];
                    FirstPressedItem[1] = LastPressedItem[1];
                }
                SetSelectedControls();
                var item = (Cell)SelectedCells.FirstOrDefault();
                if (item is { Control: TextBox ctrl })
                {
                    ctrl.Focus();
                    ctrl.SelectAll();
                    var num = 0;
                    if (ctrl.Text != null)
                    {
                        num = ctrl.Text.Length;
                    }
                    ctrl.CaretIndex = num - 1;
                }

                break;
            }
        }

        var rt = CommandsList.Where(item => item.Key == args.Key && item.KeyModifiers == args.KeyModifiers);
        if (IsReadableSum)
        {
            rt = rt.Where(item => item.Key == args.Key && item.Key is Key.A or Key.C && item.KeyModifiers == args.KeyModifiers);
        }

        foreach (var item in rt)
        {
            item.DoCommand(GetParamByParamName(item));
        }

    }

    #endregion

    #region Init

    private void ChooseAllRow(object sender, RoutedEventArgs args)
    {
        FirstPressedItem[1] = 0;
        LastPressedItem[1] = Rows[0].Children.Count;
        SetSelectedControls();
    }

    protected void Init()
    {
        MakeAll();
        MakeHeaderRows();
        MakeCenterRows();
        UpdateCells();
        MakeContextMenu();
    }

    private void MakeContextMenu()
    {
        var lst = CommandsList
            .Where(item => item.IsContextMenuCommand)
            .GroupBy(item => item.ContextMenuText[0]);
        if (IsReadableSum)
            lst = lst.Where(item => item.First().Key is Key.A or Key.C);
        ContextMenu menu = new();
        List<MenuItem> lr = new();
        foreach (var item in lst)
        {
            switch (item.Count())
            {
                case 1:
                {
                    var tmp = new MenuItem { Header = item.First().ContextMenuText[0] };
                    tmp.Tapped += CommandTapped;
                    lr.Add(tmp);
                    break;
                }
                case 2:
                {
                    List<MenuItem> inlr = new();
                    foreach (var it in item)
                    {
                        var tmp = new MenuItem { Header = it.ContextMenuText[1] };
                        tmp.Tapped += CommandTapped;
                        inlr.Add(tmp);
                    }
                    lr.Add(new MenuItem { Header = item.Key, Items = inlr });
                    break;
                }
            }
        }
        menu.Items = lr;
        ContextMenu = menu;
    }

    readonly List<ColumnDefinition> HeadersColumns = new();
    readonly int GridSplitterSize = 2;
    private void MakeHeaderInner(DataGridColumns ls)
    {

        if (ls == null) return;

        var level = ls.Level;

        Width = !IsAutoSizable 
            ? ls.SizeCol 
            : double.NaN;
        HeadersColumns.Clear();
        var tre = ls.GetLevel(level - 1);
        for (var i = level - 1; i >= 1; i--)
        {
            Grid headerRow = new();
            var count = 0;
            foreach (var item in tre)
            {
                Binding b = new()
                {
                    Source = item,
                    Path = "GridLength",
                    Mode = BindingMode.TwoWay,
                    Converter = new stringToGridLength_Converter()
                };
                var column = new ColumnDefinition()
                {
                    [!ColumnDefinition.WidthProperty] = b
                };
                headerRow.ColumnDefinitions.Add(column);

                Cell cell = new()
                {
                    [Grid.ColumnProperty] = count,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Height = i == 2 ? 40 : 25,
                    BorderColor = new SolidColorBrush(Color.Parse("Gray")),
                    Background = new SolidColorBrush(Color.Parse("White"))
                };

                TextBlock textBlock = new()
                {
                    TextWrapping = TextWrapping.Wrap,
                    Text = item.name.Contains("null") ? "" : item.name,
                    TextAlignment = TextAlignment.Center,
                    FontSize = 12,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Stretch
                };

                cell.Control = textBlock;

                headerRow.Children.Add(cell);
                if (count + 1 < tre.Count * 2 - 1)
                {
                    headerRow.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Parse(GridSplitterSize.ToString())
                    });
                    if (i == 1 && IsColumnResize)
                    {
                        headerRow.Children.Add(new GridSplitter()
                        {
                            [Grid.ColumnProperty] = count + 1,
                            ResizeDirection = GridResizeDirection.Columns,
                            Background = new SolidColorBrush(Color.Parse("Gray")),
                            ResizeBehavior = GridResizeBehavior.PreviousAndNext
                        });
                        //HeaderRow.Children.Add(new GridSplitter()
                        //{
                        //    [Grid.ColumnProperty] = count + 2,
                        //    ResizeDirection = GridResizeDirection.Columns,
                        //    Background = new SolidColorBrush(Color.Parse("Gray")),
                        //    ResizeBehavior = GridResizeBehavior.PreviousAndCurrent
                        //});
                    }
                    count += 2;
                }
                else
                {
                    count++;
                }
                if (i == 1)
                {
                    HeadersColumns.Add(column);
                }
            }
            HeaderStackPanel.Children.Add(headerRow);
            if (i - 1 >= 1)
            {
                tre = ls.GetLevel(i - 1);
            }
            else
                break;
        }
    }

    private void MakeCenterInner(DataGridColumns ls)
    {
        if (ls == null) return;

        Rows.Clear();
        var lst = ls.GetLevel(1);

        for (var i = 0; i < PageSize; i++)
        {
            var column = 0;
            var count = 0;
            DataGridRow rowStackPanel = new() { Row = i };

            foreach (var item in lst)
            {
                Binding b = new()
                {
                    Source = HeadersColumns[column],
                    Path = nameof(ColumnDefinition.Width)
                };
                var columnq = new ColumnDefinition { [!ColumnDefinition.WidthProperty] = b };
                rowStackPanel.ColumnDefinitions.Add(columnq);

                Control textBox;
                Cell cell = new()
                {
                    [Grid.ColumnProperty] = count,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    Row = i,
                    Column = column,
                    BorderColor = new SolidColorBrush(Color.Parse("Gray")),
                    Background = new SolidColorBrush(Color.Parse("White"))
                };
                if (item.ChooseLine)
                {
                    cell.Tapped += ChooseAllRow;
                }
                if (IsReadable || item.Blocked || IsReadableSum)
                {
                    if (Sum || CommentСhangeable)
                    {
                        var f22 = item.Binding is "PackQuantity" or "VolumeInPack" or "MassInPack" or "Comments";
                        if (f22)
                        {
                            textBox = new TextBox()
                            {
                                [!DataContextProperty] = new Binding(item.Binding),
                                [!TextBox.TextProperty] = new Binding("Value"),
                                [!BackgroundProperty] = cell[!Cell.ChooseColorProperty],
                            };
                            ((TextBox)textBox).TextAlignment = TextAlignment.Left;
                            textBox.VerticalAlignment = VerticalAlignment.Stretch;
                            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                            textBox.ContextMenu = new ContextMenu { Width = 0, Height = 0 };
                            if (item.IsTextWrapping)
                            {
                                ((TextBox)textBox).TextWrapping = TextWrapping.Wrap;
                                ((TextBox)textBox).AcceptsReturn = true;
                            }
                        }
                        else
                        {
                            textBox = new TextBlock()
                            {
                                [!DataContextProperty] = new Binding(item.Binding),
                                [!TextBlock.TextProperty] = new Binding("Value"),
                                [!BackgroundProperty] = cell[!Cell.ChooseColorProperty]
                            };

                            if (item.Blocked)
                            {
                                textBox[!BackgroundProperty] = cell[!Cell.ChooseColorProperty];
                            }
                            ((TextBlock)textBox).TextAlignment = TextAlignment.Center;
                            textBox.VerticalAlignment = VerticalAlignment.Center;
                            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                            ((TextBlock)textBox).Padding = new Thickness(0, 5, 0, 5);
                            textBox.Height = 30;
                            textBox.ContextMenu = new ContextMenu { Width = 0, Height = 0 };
                        }
                    }
                    else
                    {
                        textBox = new TextBlock()
                        {
                            [!DataContextProperty] = new Binding(item.Binding),
                            [!TextBlock.TextProperty] = new Binding("Value"),
                            [!BackgroundProperty] = cell[!Cell.ChooseColorProperty]
                        };

                        if (item.Blocked)
                        {
                            textBox[!BackgroundProperty] = cell[!Cell.ChooseColorProperty];
                        }
                        ((TextBlock)textBox).TextAlignment = TextAlignment.Center;
                        textBox.VerticalAlignment = VerticalAlignment.Center;
                        textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                        ((TextBlock)textBox).Padding = new Thickness(0, 5, 0, 5);
                        textBox.Height = 30;
                        textBox.ContextMenu = new ContextMenu { Width = 0, Height = 0 };
                    }
                }
                else
                {
                    textBox = new TextBox()
                    {
                        [!DataContextProperty] = new Binding(item.Binding),
                        [!TextBox.TextProperty] = new Binding("Value"),
                        [!BackgroundProperty] = cell[!Cell.ChooseColorProperty],
                    };
                    ((TextBox)textBox).TextAlignment = TextAlignment.Left;
                    textBox.VerticalAlignment = VerticalAlignment.Stretch;
                    textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    textBox.ContextMenu = new ContextMenu { Width = 0, Height = 0 };
                    if (item.IsTextWrapping)
                    {
                        ((TextBox)textBox).TextWrapping = TextWrapping.Wrap;
                        ((TextBox)textBox).AcceptsReturn = true;
                    }
                }

                cell.Control = textBox;

                rowStackPanel.Children.Add(cell);
                if (count + 1 < lst.Count * 2 - 1)
                {
                    rowStackPanel.ColumnDefinitions.Add(new ColumnDefinition
                    {
                        Width = GridLength.Parse(GridSplitterSize.ToString())
                    });
                    count += 2;
                }
                else
                {
                    count++;
                }
                column++;
            }
            rowStackPanel.IsVisible = false;
            CenterStackPanel.Children.Add(rowStackPanel);
            Rows.Add(rowStackPanel);
        }
    }

    private void MakeHeaderRows()
    {
        var columns = Columns;
        MakeHeaderInner(columns);
    }

    private void MakeCenterRows()
    {
        var columns = Columns;
        MakeCenterInner(columns);
    }

    private void MakeAll()
    {
        #region Main_<MainStackPanel>

        Panel mainPanel = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };

        Grid mainStackPanel = new()
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        mainPanel.Children.Add(mainStackPanel);

        #endregion

        #region Search

        if (Search)
        {
            mainStackPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Parse("35") });
            Border headerSearchBorder = new()
            {
                Margin = Thickness.Parse("0,0,0,5"),
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                CornerRadius = CornerRadius.Parse("2,2,2,2"),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                [Grid.RowProperty] = 0
            };
            mainStackPanel.Children.Add(headerSearchBorder);

            Panel headerSearchStackPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Background = new SolidColorBrush(Color.FromArgb(150, 180, 154, 255))
            };
            //HeaderSearchStackPanel.Orientation = Orientation.Vertical;
            headerSearchBorder.Child = headerSearchStackPanel;

            TextBox searchTextBox = new()
            {
                Name = "SearchText",
                Watermark = "Поиск...",
                Margin = Thickness.Parse("1,1,1,1"),
                [!TextBox.TextProperty] = this[!SearchTextProperty]
            };
            headerSearchStackPanel.Children.Add(searchTextBox);
        }

        #endregion

        #region Header

        mainStackPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        Border headerBorder = new()
        {
            BorderThickness = Thickness.Parse("1"),
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            CornerRadius = CornerRadius.Parse("2,2,2,2"),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            [Grid.RowProperty] = 1 - (Search ? 0 : 1)
        };
        mainStackPanel.Children.Add(headerBorder);

        Panel headerPanel = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(150, 180, 154, 255))
        };
        headerBorder.Child = headerPanel;

        HeaderStackPanel = new StackPanel
        {
            Margin = Thickness.Parse(!Sum ? "2,2,2,2" : "20,2,20,2"),
            Orientation = Orientation.Vertical
        };

        if (!string.IsNullOrEmpty(Comment))
        {
            StackPanel headerStackPanel1 = new() { Orientation = Orientation.Vertical };
            headerPanel.Children.Add(headerStackPanel1);
            StackPanel headerStackPanel2 = new()
            {
                [!MarginProperty] = this[!FixedContentProperty],
                Orientation = Orientation.Horizontal
            };
            headerStackPanel2.Children.Add(new TextBlock { Text = Comment, Margin = Thickness.Parse("5,5,0,5") });
            HeaderStackPanel.Children.Add(headerStackPanel2);
        }
        headerPanel.Children.Add(HeaderStackPanel);

        #endregion

        #region Center

        mainStackPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Parse("*") });
        Border centerBorder = new()
        {
            Margin = Thickness.Parse("0,5,0,0"),
            BorderThickness = Thickness.Parse("1"),
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            CornerRadius = CornerRadius.Parse("2,2,2,2"),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            [Grid.RowProperty] = 2 - (Search ? 0 : 1)
        };
        mainStackPanel.Children.Add(centerBorder);

        Panel centerPanel = new()
        {
            //Background=new SolidColorBrush(Color.Parse("Black")),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255))
        };
        if (!Sum)
        {
            ScrollViewer centerScrollViewer = new()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                Content = centerPanel,
                VerticalAlignment = VerticalAlignment.Stretch,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            centerBorder.Child = centerScrollViewer;
        }
        else
        {
            Panel pnl = new();
            var h = 500;
            Canvas centerCanvas = new() { Height = h };
            //CenterPanel.Height = h;

            ScrollBar bar = new()
            {
                ZIndex = 999,
                Height = h,
                HorizontalAlignment = HorizontalAlignment.Right,
                [!MarginProperty] = this[!FixedContentProperty]
            };
            centerCanvas.Children.Add(bar);

            Binding b = new()
            {
                Source = bar,
                Path = nameof(bar.Value),
                Mode = BindingMode.TwoWay
            };

            ScrollViewer centerScrollViewer = new()
            {
                Height = h,
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                Content = centerPanel
            };

            bar[!RangeBase.MaximumProperty] = centerScrollViewer[!ScrollViewer.VerticalScrollBarMaximumProperty];

            centerScrollViewer[!ScrollViewer.VerticalScrollBarValueProperty] = b;
            centerCanvas.Children.Add(centerScrollViewer);

            pnl.Children.Add(centerCanvas);
            centerBorder.Child = pnl;
            if (!IsAutoSizable)
            {
                var i = 0;
                var RDef = ((DataGridRow)CenterStackPanel.Children.FirstOrDefault()).ColumnDefinitions;
                var w = RDef.Sum(r => r.Width.Value - 1);
                centerPanel.Width = w;
            }
        }
        CenterStackPanel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Margin = Thickness.Parse(!Sum ? "2,2,2,2" : "20,2,20,2")
        };
        centerPanel.Children.Add(CenterStackPanel);

        #endregion

        #region MiddleFooter

        mainStackPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        Border middleFooterBorder = new()
        {
            Margin = Thickness.Parse("0,5,0,0"),
            BorderThickness = Thickness.Parse("1"),
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            CornerRadius = CornerRadius.Parse("2,2,2,2"),
            [Grid.RowProperty] = 3 - (Search ? 0 : 1),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch
        };
        mainStackPanel.Children.Add(middleFooterBorder);

        StackPanel middleFooterStackPanel = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(150, 180, 154, 255)),
            Orientation = Orientation.Vertical
        };
        middleFooterBorder.Child = middleFooterStackPanel;

        if (Sum)
        {
            StackPanel middleFooterStackPanelS = new()
            {
                Name = "SumColumn",
                [!MarginProperty] = this[!FixedContentProperty],
                Orientation = Orientation.Horizontal
            };
            middleFooterStackPanelS.Children.Add(new TextBlock()
                { Text = "Сумма:", Margin = Thickness.Parse("5,0,0,0"), IsVisible = false, FontSize = 13 });
            middleFooterStackPanelS.Children.Add(new TextBlock()
            {
                [!TextBox.TextProperty] = this[!SumColumnProperty], Margin = Thickness.Parse("5,0,0,0"),
                IsVisible = false, FontSize = 13
            });
            middleFooterStackPanel.Children.Add(middleFooterStackPanelS);
        }
        StackPanel middleFooterStackPanel1 = new()
        {
            [!MarginProperty] = this[!FixedContentProperty],
            Orientation = Orientation.Horizontal
        };
        middleFooterStackPanel1.Children.Add(new TextBlock()
            { Text = "Кол-во страниц:", Margin = Thickness.Parse("5,0,0,0"), FontSize = 13 });
        middleFooterStackPanel1.Children.Add(new TextBlock()
            { [!TextBox.TextProperty] = this[!PageCountProperty], Margin = Thickness.Parse("5,0,0,0"), FontSize = 13 });
        middleFooterStackPanel.Children.Add(middleFooterStackPanel1);

        StackPanel middleFooterStackPanel2 = new()
        {
            [!MarginProperty] = this[!FixedContentProperty],
            Orientation = Orientation.Horizontal
        };
        middleFooterStackPanel2.Children.Add(new TextBlock 
            { Text = ShowAllReport ? "Кол-во строчек:" : "Кол-во отчетов", Margin = Thickness.Parse("5,0,0,0"), FontSize = 13 });
        middleFooterStackPanel2.Children.Add(new TextBlock
        {
            [!TextBox.TextProperty] = this[!ItemsCountProperty], Margin = Thickness.Parse("5,0,0,0"), FontSize = 13
        });
        middleFooterStackPanel.Children.Add(middleFooterStackPanel2);

        if (ShowAllReport)
        {
            StackPanel middleFooterStackPanelR = new()
            {
                [!MarginProperty] = this[!FixedContentProperty],
                Orientation = Orientation.Horizontal
            };
            middleFooterStackPanelR.Children.Add(new TextBlock
                { Text = "Кол-во отчетов:", Margin = Thickness.Parse("5,0,0,0"), FontSize = 13 });
            middleFooterStackPanelR.Children.Add(new TextBlock
            {
                [!TextBox.TextProperty] = this[!ReportCountProperty], Margin = Thickness.Parse("5,0,0,0"), FontSize = 13
            });
            middleFooterStackPanel2.Children.Add(middleFooterStackPanelR);
        }
        else if (Type is nameof(Report))
        {
            StackPanel middleFooterStackPanelR = new()
            {
                [!MarginProperty] = this[!FixedContentProperty],
                Orientation = Orientation.Horizontal
            };
            middleFooterStackPanelR.Children.Add(new TextBlock
                { Text = "Кол-во строчек:", Margin = Thickness.Parse("5,0,0,0"), FontSize = 13 });
            middleFooterStackPanelR.Children.Add(new TextBlock
            {
                [!TextBox.TextProperty] = this[!ReportStringCountProperty], Margin = Thickness.Parse("5,0,0,0"), FontSize = 13
            });
            middleFooterStackPanel2.Children.Add(middleFooterStackPanelR);
        }

        #endregion

        #region Footer

        mainStackPanel.RowDefinitions.Add(new RowDefinition { Height = GridLength.Parse("45") });
        Border footerBorder = new()
        {
            Margin = Thickness.Parse("0,5,0,0"),
            BorderThickness = Thickness.Parse("1"),
            BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
            CornerRadius = CornerRadius.Parse("2,2,2,2"),
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            [Grid.RowProperty] = 4 - (Search ? 0 : 1)
        };
        mainStackPanel.Children.Add(footerBorder);

        Panel footerPanel = new()
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(150, 180, 154, 255)),
            Height = 40
        };
        footerBorder.Child = footerPanel;

        StackPanel footerStackPanel = new()
        {
            Margin = Thickness.Parse("5,0,0,0"),
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Spacing = 5
        };
        footerPanel.Children.Add(footerStackPanel);

        Button btnDown = new()
        {
            Content = "<",
            Width = 30,
            Height = 30,
            CornerRadius = CornerRadius.Parse("2,2,2,2"),
            [!MarginProperty] = this[!FixedContentProperty]
        };
        btnDown.Click += NowPageDown;
        footerStackPanel.Children.Add(btnDown);

        TextBox box = new()
        {
            [!TextBox.TextProperty] = this[!NowPageProperty],
            TextAlignment = TextAlignment.Center,
            CornerRadius = CornerRadius.Parse("2,2,2,2"),
            Width = 30,
            Height = 30
        };
        footerStackPanel.Children.Add(box);

        Button btnUp = new()
        {
            Content = ">",
            Width = 30,
            Height = 30,
            CornerRadius = CornerRadius.Parse("2,2,2,2"),
        };
        btnUp.Click += NowPageUp;
        footerStackPanel.Children.Add(btnUp);

        #endregion

        Content = mainPanel;
    }

    #endregion

    
}