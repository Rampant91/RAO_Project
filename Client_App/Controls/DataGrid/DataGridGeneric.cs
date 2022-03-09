using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Models.Collections;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Models;
using ReactiveUI;
using System.Reactive;
using System.Runtime.CompilerServices;
using Avalonia.LogicalTree;
using Client_App.Controls.DataGrid;
using Models.DBRealization;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Abstracts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Models.DataAccess;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Reactive.Subjects;
using Client_App.Converters;
using Client_App.ViewModels;
using Models.Attributes;
using System.Text.RegularExpressions;
using Models.Interfaces;

namespace Client_App.Controls.DataGrid
{

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

    public class DataGrid<T> : UserControl,IDataGrid where T : class, IKey, IDataGridColumn, new()
    {
        #region Items
        public static readonly DirectProperty<DataGrid<T>, IKeyCollection> ItemsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid<T>, IKeyCollection>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v, defaultBindingMode: BindingMode.TwoWay);

        private IKeyCollection _items = null;

        public IKeyCollection Items
        {
            get => _items;
            set
            {
                if (value != null)
                {
                    SetAndRaise(ItemsProperty, ref _items, value);
                    UpdateCells();
                    SetSelectedControls();
                }
            }
        }
        #endregion

        #region ItemsCount
        public static readonly DirectProperty<DataGrid<T>, string> ItemsCountProperty =
            AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
                nameof(ItemsCount),
                o => o.ItemsCount,
                (o, v) => o.ItemsCount = v);

        private string _ItemsCount = "0";
        public string ItemsCount
        {
            get => Items!=null?Items.Count.ToString():"0";
            set
            {
                SetAndRaise(ItemsCountProperty, ref _ItemsCount, Items != null ? Items.Count.ToString() : "0");
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
                if (value != null)
                {
                    if (value.Count != 0)
                    {
                        SetAndRaise(SelectedItemsProperty, ref _selecteditems, value);
                        if (Sum) SumColumn = "0";
                    }
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

        private IList<Control> _selectedCells =
                new List<Control>();
        public IList<Control> SelectedCells
        {
            get => _selectedCells;
            set
            {
                if (value != null) SetAndRaise(SelectedCellsProperty, ref _selectedCells, value);
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
            get
            {

                return typeof(T).Name;
            }
            set
            {

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
            get
            {
                return _Comment;
            }
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

        private bool _Search = false;
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

        private bool _Sum = false;
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

        public string GetSum() 
        {
            object[] answ = new object[3];
            answ[0] = SelectedItems;
            answ[1] = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
            answ[2] = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
            IKeyCollection collection = answ[0] as IKeyCollection;
            int minColumn = Convert.ToInt32(answ[1]) + 1;
            int maxColumn = Convert.ToInt32(answ[2]) + 1;
            Double _s = 0.0;
            if (minColumn == maxColumn)
            {
                foreach (IKey item in collection.GetEnumerable().OrderBy(x => x.Order))
                {
                    var props = item.GetType().GetProperties();
                    foreach (var prop in props)
                    {
                        var attr = (Form_PropertyAttribute)prop.GetCustomAttributes(typeof(Form_PropertyAttribute), false).FirstOrDefault();
                        if (attr != null)
                        {
                            try
                            {
                                var columnNum = Convert.ToInt32(attr.Number);
                                if (columnNum >= minColumn && columnNum <= maxColumn)
                                {
                                    var midvalue = prop.GetMethod.Invoke(item, null);
                                    var _value = midvalue.GetType().GetProperty("Value").GetMethod.Invoke(midvalue, null);
                                    if (_value != null && _value != " ")
                                    {
                                        try
                                        {
                                            _value = _value.ToString().Replace("е", "e").Replace("Е", "E").Replace(".", ",");
                                            _s += Convert.ToDouble(_value);
                                            var stackPanel = (StackPanel)((StackPanel)((Border)((Grid)((Panel)this.Content).Children[0]).Children[2]).Child).Children[0];
                                            stackPanel.Children[0].IsVisible = true;
                                            stackPanel.Children[1].IsVisible = true;
                                        }
                                        catch 
                                        {
                                            var stackPanel = (StackPanel)((StackPanel)((Border)((Grid)((Panel)this.Content).Children[0]).Children[2]).Child).Children[0];
                                            stackPanel.Children[0].IsVisible = false;
                                            stackPanel.Children[1].IsVisible = false;
                                            return null;
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
            else
            {
                var stackPanel = (StackPanel)((StackPanel)((Border)((Grid)((Panel)this.Content).Children[0]).Children[2]).Child).Children[0];
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

        private bool _IsReadable = false;
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

        #region IsReadableSum
        public static readonly DirectProperty<DataGrid<T>, bool> IsReadableSumProperty =
            AvaloniaProperty.RegisterDirect<DataGrid<T>, bool>(
                nameof(IsReadableSum),
                o => o.IsReadableSum,
                (o, v) => o.IsReadableSum = v);

        private bool _IsReadableSum = false;
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
            set
            {
                SetAndRaise(FixedContentProperty, ref _FixedContent, value);
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
            get => (Items!=null?Items.Count / PageSize + 1:0).ToString();
            set
            {
                SetAndRaise(PageCountProperty, ref _PageCount, (Items != null ? Items.Count / PageSize + 1 : 0).ToString());
            }
        }
        #endregion

        #region NowPage
        public static readonly DirectProperty<DataGrid<T>, string> NowPageProperty =
            AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
                nameof(NowPage),
                o => o.NowPage,
                (o, v) => o.NowPage = v, defaultBindingMode: BindingMode.TwoWay);

        private string _nowPage = "1";
        public string NowPage
        {
            get => _nowPage.ToString();
            set
            {
                try
                {
                    var val = Convert.ToInt32(value);

                    if (val != null&&Items!=null)
                    {
                        int maxpage = (Items.Count / PageSize) + 1;
                        if (val.ToString() != _nowPage)
                        {
                            if (val <= maxpage && val >= 1)
                            {
                                SetAndRaise(NowPageProperty, ref _nowPage, value);
                                UpdateCells();
                            }
                            else
                            {
                                if (val > maxpage)
                                {
                                    if (_nowPage != maxpage.ToString())
                                    {
                                        SetAndRaise(NowPageProperty, ref _nowPage, maxpage.ToString());
                                        UpdateCells();
                                    }
                                }
                                if (val < 1)
                                {
                                    if (_nowPage != "1")
                                    {
                                        SetAndRaise(NowPageProperty, ref _nowPage, "1");
                                        UpdateCells();
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                }
            }
        }

        public void NowPageDown(object sender, RoutedEventArgs args)
        {
            NowPage = (Convert.ToInt32(NowPage) - 1).ToString();
        }
        public void NowPageUp(object sender, RoutedEventArgs args)
        {
            NowPage = (Convert.ToInt32(NowPage) + 1).ToString();
        }
        #endregion

        #region CommandsList
        public static readonly DirectProperty<DataGrid<T>, ObservableCollection<KeyComand>> CommandsListProperty =
            AvaloniaProperty.RegisterDirect<DataGrid<T>, ObservableCollection<KeyComand>>(
                nameof(CommandsList),
                o => o.CommandsList,
                (o, v) => o.CommandsList = v);

        private ObservableCollection<KeyComand> _CommandsList = new ObservableCollection<KeyComand>();
        public ObservableCollection<KeyComand> CommandsList
        {
            get => _CommandsList;
            set
            {
                SetAndRaise(CommandsListProperty, ref _CommandsList, value);
            }
        }
        private void CommandListChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            foreach (KeyComand item in args.NewItems)
            {
                if (item.IsContextMenuCommand)
                {
                    MakeContextMenu();
                    break;
                }
            }
        }
        private object GetParamByParamName(KeyComand param)
        {
            if(param.ParamName=="SelectedItems")
            {
                return SelectedItems;
            }
            if (param.ParamName == "SelectedCells")
            {
                return SelectedCells;
            }
            if (param.ParamName == "FormType")
            {
                return SelectedCells;
            }
            if (param.ParamName == "Copy")
            {
                object[] answ = new object[3];
                answ[0] = SelectedItems;
                answ[1] = Math.Min(FirstPressedItem[1],LastPressedItem[1]);
                answ[2] = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
                return answ;
            }
            if (param.ParamName == "Paste")
            {
                object[] answ = new object[3];
                answ[0] = SelectedItems;
                answ[1] = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
                answ[2] = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
                return answ;
            }
            if (param.ParamName == "SelectAll")
            {
                int maxRow = PageSize;
                int maxColumn = Rows[0].Children.Count;
                FirstPressedItem[0] = 0;
                FirstPressedItem[1] = 0;
                LastPressedItem[0] = maxRow-1;
                LastPressedItem[1] = maxColumn-1;
                SetSelectedControls();
                ObservableCollectionWithItemPropertyChanged<IKey> lst = new ObservableCollectionWithItemPropertyChanged<IKey>();
                foreach (var item in Items)
                {
                    lst.Add(item);
                }
                SelectedItems = lst;
            }
            if (param.ParamName ==null|| param.ParamName =="")
            {
                return param.Param;
            }
            if (param.ParamName == "1.0" || param.ParamName == "2.0")
            {
                return param.ParamName;
            }
            return null;
        }

        private void ComandTapped(object sender,RoutedEventArgs args)
        {
            if (sender != null)
            {
                var selectItem = (string)((MenuItem)sender).Header;
                if (selectItem != null)
                {
                    var rt = CommandsList.Where(item => item.IsContextMenuCommand && item.ContextMenuText.Contains(selectItem));
                    foreach (var item in rt)
                    {
                        item.DoCommand(GetParamByParamName(item));
                        if (item.IsUpdateCells)
                        {
                            UpdateCells();
                        }
                    }
                }
            }
        }
        #endregion

        #region Columns
        private DataGridColumns _Columns = null;
        private DataGridColumns Columns
        {
            get
            {
                if(_Columns==null)
                {
                    var t = new T();
                    string tmp = "";
                    if (Name == "Form1AllDataGrid_")
                    {
                        tmp = "1.0";
                    }
                    if (Name == "Form2AllDataGrid_")
                    {
                        tmp = "2.0";
                    }
                    _Columns = t.GetColumnStructure(tmp);
                }
                return _Columns;
            }
            set
            {
                if(_Columns!=value)
                {
                    _Columns = value;
                    Init();
                }
            }
        }
        #endregion

        #region SearchText
        public static readonly DirectProperty<DataGrid<T>, string> SearchTextProperty =
            AvaloniaProperty.RegisterDirect<DataGrid<T>, string>(
                nameof(SearchText),
                o => o.SearchText,
                (o, v) => o.SearchText = v);
        private string _SearchText = "";
        public string SearchText
        {
            get
            {
                return _SearchText;
            }
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

        private List<DataGridRow> Rows { get; set; } = new List<DataGridRow>();

        private StackPanel HeaderStackPanel { get; set; }
        private StackPanel CenterStackPanel { get; set; }
        public DataGrid(string Name = "")
        {
            this.Name = Name;
            this.Focusable = true;
            FirstPressedItem[0] = -1;
            FirstPressedItem[1] = -1;
            LastPressedItem[0] = -1;
            LastPressedItem[1] = -1;

            this.AddHandler(PointerPressedEvent,MousePressed,handledEventsToo:true);
            this.AddHandler(PointerMovedEvent, MouseMoved, handledEventsToo: true);
            this.AddHandler(PointerReleasedEvent, MouseReleased, handledEventsToo: true);
            this.AddHandler(DoubleTappedEvent, MouseDoublePressed, handledEventsToo: true);

            this.AddHandler(KeyDownEvent, OnDataGridKeyDown, handledEventsToo: true);

            _CommandsList.CollectionChanged += CommandListChanged;
        }

        #region SetSelectedControls
        private void SetSelectedControls()
        {
            if (Items != null)
            {
                if (Items.Count != 0)
                {
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
            }
        }

        private void SetSelectedControls_LineSingle()
        {
            var Row = LastPressedItem[0];

            if (Row != -1)
            {
                var tmp1 = Rows.SelectMany(x => x.Children).Where(item => ((Cell)item).Row != Row);

                foreach (Cell item in tmp1)
                {
                    item.ChooseColor = (SolidColorBrush)Background;
                }

                SelectedCells.Clear();

                ObservableCollectionWithItemPropertyChanged<IKey> tmpSelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();

                var tmp2 = Rows.SelectMany(x => x.Children).Where(item => ((Cell)item).Row == Row);

                foreach (Cell item in tmp2)
                {
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

                foreach (Cell item in tmp1)
                {
                    item.ChooseColor = (SolidColorBrush)Background;
                }

                SelectedCells.Clear();
                SelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();
            }
        }

        //Not Done
        private void SetSelectedControls_CellSingle()
        {
            var Row = LastPressedItem[0];
            var Column = LastPressedItem[1];

            if (Row != -1 && Column != -1)
            {

                var tmp1 = Rows.Where(item => ((Cell)item.Children.FirstOrDefault()).Row != Row && ((Cell)item.Children.FirstOrDefault()).Column != Column);

                foreach (DataGridRow item in tmp1)
                {
                    item.ChooseColor = (SolidColorBrush)Background;
                }

                SelectedCells.Clear();

                ObservableCollectionWithItemPropertyChanged<IKey> tmpSelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();

                var tmp2 = Rows.Where(item => ((Cell)item.Children.FirstOrDefault()).Row == Row && ((Cell)item.Children.FirstOrDefault()).Column == Column);

                foreach (DataGridRow item in tmp2)
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

                foreach (Cell item in tmp1)
                {
                    item.ChooseColor = (SolidColorBrush)Background;
                }

                SelectedCells.Clear();
                SelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();
            }
        }

        private void SetSelectedControls_LineMulti()
        {
            var minRow = Math.Min(FirstPressedItem[0],LastPressedItem[0]);
            var maxRow = Math.Max(FirstPressedItem[0], LastPressedItem[0]);
            if (minRow != -1 && maxRow != -1)
            {
                var tmp1 = Rows.SelectMany(x => x.Children).Where(item => !(((Cell)item).Row >= minRow && ((Cell)item).Row <= maxRow));

                foreach (Cell item in tmp1)
                {
                    item.ChooseColor = (SolidColorBrush)Background;
                }

                SelectedCells.Clear();

                ObservableCollectionWithItemPropertyChanged<IKey> tmpSelectedItems = new ObservableCollectionWithItemPropertyChanged<IKey>();

                var tmp2 = Rows.SelectMany(x => x.Children).Where(item => (((Cell)item).Row >= minRow && ((Cell)item).Row <= maxRow));

                foreach (Cell item in tmp2)
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

                foreach (Cell item in tmp1)
                {
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
                var tmp1 = Rows.SelectMany(x => x.Children).Where(item => !((((Cell)item).Row >= minRow && ((Cell)item).Row <= maxRow) &&
                                                (((Cell)item).Column >= minColumn && ((Cell)item).Column <= maxColumn)));

                foreach (Cell item in tmp1)
                {
                    item.ChooseColor = (SolidColorBrush)Background;
                }

                SelectedCells.Clear();

                Dictionary<long,ObservableCollectionWithItemPropertyChanged<IKey>> tmpSelectedItems = new Dictionary<long, ObservableCollectionWithItemPropertyChanged<IKey>>();

                var tmp2 = Rows.SelectMany(x => x.Children).Where(item => ((((Cell)item).Row >= minRow && ((Cell)item).Row <= maxRow) &&
                                                  (((Cell)item).Column >= minColumn && ((Cell)item).Column <= maxColumn)));

                foreach (Cell item in tmp2)
                {
                    item.ChooseColor = (SolidColorBrush)ChooseColor;
                    SelectedCells.Add(item);
                    if (item.DataContext is T)
                    {
                        if (!tmpSelectedItems.ContainsKey(((T)item.DataContext).Order))
                        {
                            tmpSelectedItems.Add((((T)item.DataContext).Order), new ObservableCollectionWithItemPropertyChanged<IKey>());
                        }
                    }
                    else
                    {
                        break;
                    }
                    tmpSelectedItems[(((T)item.DataContext).Order)].Add((T)item.DataContext);
                }
                var tmp = new ObservableCollectionWithItemPropertyChanged<IKey>();
                foreach (var item in tmpSelectedItems)
                {
                    var value = item.Value.FirstOrDefault();
                    if (value != null)
                    {
                        tmp.Add(value);
                    }
                }
                SelectedItems = tmp;
            }
            else
            {
                var tmp1 = Rows.SelectMany(x => x.Children);

                foreach (Cell item in tmp1)
                {
                    item.ChooseColor = (SolidColorBrush)Background;
                }

                SelectedCells.Clear();
                SelectedItems=new ObservableCollectionWithItemPropertyChanged<IKey>();
            }
        }
        #endregion

        #region DataGridPoiter
        public bool DownFlag { get; set; }
        public int[] FirstPressedItem { get; set; } = new int[2];
        public int[] LastPressedItem { get; set; } = new int[2];
        private bool SetFirstPressed(int[] First)
        {
            if (FirstPressedItem[0] != First[0] || FirstPressedItem[1] != First[1])
            {
                FirstPressedItem[0] = First[0];
                FirstPressedItem[1] = First[1];
                return true;
            }
            return false;
        }
        private bool SetLastPressed(int[] Last)
        {
            if (LastPressedItem[0] != Last[0] || LastPressedItem[1] != Last[1])
            {
                LastPressedItem[0] = Last[0];
                LastPressedItem[1] = Last[1];
                return true;
            }
            return false;
        }

        private int[] FindMousePress(double[] mouse)
        {
            var tmp = new int[2];

            var sumy = 0.0;
            var flag = false;
            var doFlag = false;
            foreach(var item in Rows)
            {
                sumy += item.Bounds.Height;
                if(mouse[0]<=sumy)
                {
                    if (mouse[0] >= 0)
                    {
                        var sumx = 0.0;
                        foreach (Cell it in item.Children)
                        {
                            sumx += it.Bounds.Width;
                            if (mouse[1] <= sumx)
                            {
                                if (mouse[1] >= 0)
                                {
                                    tmp[0] = it.Row;
                                    tmp[1] = it.Column;
                                    flag = true;
                                    doFlag = true;
                                    break;
                                }
                                else
                                {
                                    flag = true;
                                    break;
                                }
                            }
                        }
                        if (flag)
                            break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            if(!doFlag)
            {
                tmp[0] = -1;
                tmp[1] = -1;
            }
            return tmp;
        }
        private void MousePressed(object sender, PointerPressedEventArgs args)
        {
            var paramKey = args.GetPointerPoint(this).Properties.PointerUpdateKind;
            var paramPos = args.GetCurrentPoint(CenterStackPanel).Position;
            bool doSetItemFlag = false;

            if (paramKey == PointerUpdateKind.LeftButtonPressed || paramKey == PointerUpdateKind.RightButtonPressed)
            {
                var paramRowColumn = FindMousePress(new double[] { paramPos.Y, paramPos.X });
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
                        if (!IsReadableSum)
                        {
                            this.ContextMenu.Close();
                            var tmp1 = (Cell)Rows.SelectMany(x => x.Children).Where(item => (((Cell)item).Row == paramRowColumn[0] && ((Cell)item).Column == paramRowColumn[1])).FirstOrDefault();
                            this.ContextMenu.PlacementTarget = tmp1;
                            this.ContextMenu.Open();
                        }
                    }
                }
                else
                {
                    if (!IsReadableSum)
                    {
                        this.ContextMenu.Close();
                    }
                    doSetItemFlag = SetFirstPressed(paramRowColumn);
                    if (doSetItemFlag)
                    {
                        LastPressedItem[0] = FirstPressedItem[0];
                        LastPressedItem[1] = FirstPressedItem[1];
                    }
                    doSetItemFlag = doSetItemFlag || SetLastPressed(paramRowColumn);
                }

                if (doSetItemFlag)
                {
                    SetSelectedControls();
                    if(paramKey == PointerUpdateKind.LeftButtonPressed)
                    {
                        Cell item = (Cell)SelectedCells.FirstOrDefault();
                        if (item != null)
                        {
                            if (item.Control is TextBox)
                            {
                                var ctrl = (TextBox)item.Control;
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

                }
            }
        }
        private void MouseDoublePressed(object sender, EventArgs args)
        {
            if (FirstPressedItem[0] != -1)
            {
                var commands = _CommandsList.Where(item => item.IsDoubleTappedCommand);
                foreach (var item in commands)
                {
                    item.DoCommand(GetParamByParamName(item));
                }
            }
        }
        private void MouseReleased(object sender, PointerReleasedEventArgs args)
        {
            var paramKey = args.GetPointerPoint(this).Properties.PointerUpdateKind;
            var paramPos = args.GetCurrentPoint(CenterStackPanel).Position;

            if (paramKey == PointerUpdateKind.LeftButtonReleased)
            {
                var paramRowColumn = FindMousePress(new double[] { paramPos.Y, paramPos.X });
                if (LastPressedItem[0] != paramRowColumn[0] || LastPressedItem[1] != paramRowColumn[1])
                {
                    LastPressedItem = paramRowColumn;
                    SetSelectedControls();
                }
            }
        }
        private void MouseMoved(object sender, PointerEventArgs args)
        {
            var paramKey = args.GetPointerPoint(this).Properties;
            var paramPos = args.GetCurrentPoint(CenterStackPanel).Position;

            if (paramKey.IsLeftButtonPressed)
            {
                var paramRowColumn = FindMousePress(new double[] { paramPos.Y, paramPos.X });
                if (LastPressedItem[0] != paramRowColumn[0] || LastPressedItem[1] != paramRowColumn[1])
                {
                    LastPressedItem = paramRowColumn;
                    SetSelectedControls();
                }
            }
        }
        #endregion

        #region UpdateCells
        private void UpdateCells()
        {
            var count = 0;

            var num = Convert.ToInt32(_nowPage);
            var offset = (num-1) * (PageSize);
            var offsetMax= num * (PageSize);

            if (Items != null)
            {
                IKeyCollection tmp_coll = new ObservableCollectionWithItemPropertyChanged<IKey>(Items.GetEnumerable());
                if (Search)
                {
                    IKeyCollection tmp2_coll = new ObservableCollectionWithItemPropertyChanged<IKey>();
                    var searchText = ((TextBox)((StackPanel)((StackPanel)((Border)((Grid)((Panel)this.Content).Children[0]).Children[0]).Child).Children[0]).Children[0]).Text;
                    if (searchText != null)
                    {
                        searchText = searchText.ToLower();
                        searchText = Regex.Replace(searchText, "[-.?!)(,: ]", "");
                        if (searchText != "")
                        {
                            foreach (var it in tmp_coll)
                            {
                                var rowsText = ((Reports)it).Master_DB.OkpoRep.Value +
                                            ((Reports)it).Master_DB.ShortJurLicoRep.Value +
                                            ((Reports)it).Master_DB.RegNoRep.Value;
                                rowsText = rowsText.ToLower();
                                rowsText = Regex.Replace(rowsText, "[-.?!)(,: ]", "");
                                if (rowsText.Contains(searchText))
                                {
                                    tmp2_coll.Add(it);
                                }
                            }
                            tmp_coll = tmp2_coll;
                        }
                    }
                }
                if (tmp_coll.Count != 0)
                {
                    for (int i = offset; i < offsetMax; i++)
                    {
                        if (count < PageSize && i < tmp_coll.Count)
                        {
                            Rows[count].DataContext = tmp_coll.Get<T>(i);

                            Rows[count].IsVisible = true;

                            count++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                if (tmp_coll.Count < offsetMax)
                {
                    for (int i = tmp_coll.Count; i < offsetMax; i++)
                    {
                        try
                        {
                            if (Rows[i - offset].IsVisible)
                            {
                                Rows[i - offset].IsVisible = false;
                            }
                        }
                        catch { }
                    }
                }
                var t = typeof(T).FindInterfaces(new System.Reflection.TypeFilter((x,y)=> 
                {
                    if (x.ToString() == y.ToString())
                        return true;
                    else
                        return false;
                }), typeof(IBaseColor).FullName);
                if (t.Count()!=0)
                {
                    for (int i = 0; i < PageSize; i++)
                    {
                        if (Rows[i].DataContext is IBaseColor)
                        {
                            var _t = (IBaseColor)Rows[i].DataContext;
                            if (_t != null)
                            {

                                var tmp2 = Rows.SelectMany(x => x.Children).Where(item => ((Cell)item).Row == i);
                                int index = (int)_t.BaseColor;
                                var color = IBaseColor.ColorTypeList[index];

                                foreach (Cell item in tmp2)
                                {
                                    item.Background = new SolidColorBrush(Color.FromArgb(color.A, color.R, color.G, color.B));
                                }
                            }
                        }
                    }
                }
            }

            PageCount = "0";
            ItemsCount = "0";
        }
        #endregion

        #region KeyDown
        private void OnDataGridKeyDown(object sender,KeyEventArgs args)
        {
            if(args.Key==Key.Left)
            {
                LastPressedItem[1] = LastPressedItem[1]==1? LastPressedItem[1]: LastPressedItem[1]-1;
                if(args.KeyModifiers!=KeyModifiers.Shift)
                {
                    FirstPressedItem[0] = LastPressedItem[0];
                    FirstPressedItem[1] = LastPressedItem[1];
                }
                SetSelectedControls();
                Cell item = (Cell)SelectedCells.FirstOrDefault();
                if (item != null)
                {
                    if (item.Control is TextBox)
                    {
                        var ctrl = (TextBox)item.Control;
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
            if (args.Key == Key.Right)
            {
                LastPressedItem[1] = LastPressedItem[1] == Rows[0].Children.Count-1 ? LastPressedItem[1] : LastPressedItem[1] + 1;
                if (args.KeyModifiers != KeyModifiers.Shift)
                {
                    FirstPressedItem[0] = LastPressedItem[0];
                    FirstPressedItem[1] = LastPressedItem[1];
                }
                SetSelectedControls();
                Cell item = (Cell)SelectedCells.FirstOrDefault();
                if (item != null)
                {
                    if (item.Control is TextBox)
                    {
                        var ctrl = (TextBox)item.Control;
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
            if (args.Key == Key.Down)
            {
                LastPressedItem[0] = LastPressedItem[0] == Rows.Count-1||LastPressedItem[0] == Items.Count-1 ? LastPressedItem[0] : LastPressedItem[0] + 1;
                if (args.KeyModifiers != KeyModifiers.Shift)
                {
                    FirstPressedItem[0] = LastPressedItem[0];
                    FirstPressedItem[1] = LastPressedItem[1];
                }
                SetSelectedControls();
                Cell item = (Cell)SelectedCells.FirstOrDefault();
                if (item != null)
                {
                    if (item.Control is TextBox)
                    {
                        var ctrl = (TextBox)item.Control;
                        ctrl.Focus();
                        ctrl.SelectAll();
                        var num = 0;
                        if(ctrl.Text!=null)
                        {
                            num = ctrl.Text.Length;
                        }
                        ctrl.CaretIndex = num - 1;
                    }
                }
            }
            if (args.Key == Key.Up)
            {
                LastPressedItem[0] = LastPressedItem[0] == 0 ? LastPressedItem[0] : LastPressedItem[0] - 1;
                if (args.KeyModifiers != KeyModifiers.Shift)
                {
                    FirstPressedItem[0] = LastPressedItem[0];
                    FirstPressedItem[1] = LastPressedItem[1];
                }
                SetSelectedControls();
                Cell item = (Cell)SelectedCells.FirstOrDefault();
                if (item != null)
                {
                    if (item.Control is TextBox)
                    {
                        var ctrl = (TextBox)item.Control;
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

            var rt = CommandsList.Where(item=>item.Key==args.Key&&item.KeyModifiers==args.KeyModifiers);

            if (!IsReadableSum)
            {
                foreach (var item in rt)
                {
                    item.DoCommand(GetParamByParamName(item));
                }
            }
        }
        #endregion

        #region Init

        public void ChooseAllRow(object sender,RoutedEventArgs args)
        {
            FirstPressedItem[1] = 0;
            LastPressedItem[1] = Rows[0].Children.Count;
            SetSelectedControls();
        }

        public void Init()
        {
            MakeAll();
            MakeHeaderRows();
            MakeCenterRows();

            UpdateCells();
            MakeContextMenu();
        }

        private void MakeContextMenu()
        {

            if (!IsReadableSum)
            {
                var lst = CommandsList.Where(item => item.IsContextMenuCommand).GroupBy(item => item.ContextMenuText[0]);
                ContextMenu menu = new ContextMenu();
                List<MenuItem> lr = new List<MenuItem>();
                foreach (var item in lst)
                {
                    if (item.Count() == 1)
                    {
                        var tmp = new MenuItem { Header = item.First().ContextMenuText[0] };
                        tmp.Tapped += ComandTapped;
                        lr.Add(tmp);
                    }
                    if (item.Count() == 2)
                    {
                        List<MenuItem> inlr = new List<MenuItem>();
                        foreach (var it in item)
                        {
                            var tmp = new MenuItem { Header = it.ContextMenuText[1] };
                            tmp.Tapped += ComandTapped;
                            inlr.Add(tmp);
                        }
                        lr.Add(new MenuItem { Header = item.Key, Items = inlr });
                    }
                }
                menu.Items = lr;
                this.ContextMenu = menu;
            }
            else
            {
                this.ContextMenu = null;
            }
        }

        List<ColumnDefinition> HeadersColumns = new List<ColumnDefinition>();
        int GridSplitterSize = 2;
        private void MakeHeaderInner(DataGridColumns ls)
        {
            
            if (ls==null)
            {
                return;
            }
            else
            {
                int Level = ls.Level;

                this.Width = ls.SizeCol;
                HeadersColumns.Clear();
                var tre = ls.GetLevel(Level-1);
                for (int i = Level-1; i >= 1; i--)
                {
                    Grid HeaderRow = new Grid();
                    var count = 0;
                    foreach (var item in tre)
                    {
                        Binding b = new Binding()
                        {
                            Source = item,
                            Path = "GridLength",
                            Mode = BindingMode.TwoWay,
                            Converter = new stringToGridLength_Converter()
                        };
                        var Column = new ColumnDefinition() { 
                            [!ColumnDefinition.WidthProperty]=b
                        };
                        HeaderRow.ColumnDefinitions.Add(Column);

                        Cell cell = new Cell() {
                            [Grid.ColumnProperty]=count
                        };
                        cell.HorizontalAlignment = HorizontalAlignment.Stretch;
                        cell.Height = 30;
                        cell.BorderColor = new SolidColorBrush(Color.Parse("Gray"));
                        cell.Background = new SolidColorBrush(Color.Parse("White"));

                        TextBlock textBlock = new TextBlock();
                        textBlock.Text = item.name.Contains("null") ?"": item.name;
                        textBlock.TextAlignment = TextAlignment.Center;
                        textBlock.VerticalAlignment = VerticalAlignment.Center;
                        textBlock.HorizontalAlignment = HorizontalAlignment.Stretch;

                        cell.Control = textBlock;

                        HeaderRow.Children.Add(cell);
                        if (count + 1 < tre.Count * 2 - 1)
                        {
                            HeaderRow.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Parse(GridSplitterSize.ToString()) });
                            if (i == 1&&IsColumnResize)
                            {
                                HeaderRow.Children.Add(new GridSplitter()
                                {
                                    [Grid.ColumnProperty] = count + 1,
                                    ResizeDirection = GridResizeDirection.Columns,
                                    Background = new SolidColorBrush(Color.Parse("Gray")),
                                    ResizeBehavior = GridResizeBehavior.BasedOnAlignment
                                });
                            }
                            count += 2;
                        }
                        else
                        {
                            count++;
                        }
                        if (i == 1)
                        {
                            HeadersColumns.Add(Column);
                        }
                    }
                    HeaderStackPanel.Children.Add(HeaderRow);
                    if (i - 1 >= 1)
                    {
                        tre = ls.GetLevel(i - 1);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private void MakeCenterInner(DataGridColumns ls)
        {
            if (ls == null)
            {
                return;
            }
            else
            {
                Rows.Clear();
                var lst = ls.GetLevel(1);

                for (int i = 0; i < PageSize; i++)
                {
                    var Column = 0;
                    var count = 0;
                    DataGridRow RowStackPanel = new();
                    RowStackPanel.Row = i;

                    foreach (var item in lst)
                    {
                        Binding b = new Binding()
                        {
                            Source = HeadersColumns[Column],
                            Path = nameof(ColumnDefinition.Width)
                        };
                        var Columnq = new ColumnDefinition() { [!ColumnDefinition.WidthProperty] = b };
                        RowStackPanel.ColumnDefinitions.Add(Columnq);

                        Control textBox = null;

                        Cell cell = new Cell()
                        {
                            [Grid.ColumnProperty] = count
                        };
                        cell.HorizontalAlignment = HorizontalAlignment.Stretch;
                        cell.Row = i;
                        cell.Column = Column;
                        cell.BorderColor = new SolidColorBrush(Color.Parse("Gray"));
                        cell.Background = new SolidColorBrush(Color.Parse("White"));
                        if (item.ChooseLine)
                        {
                            cell.Tapped += ChooseAllRow;
                        }
                        if (IsReadable || item.Blocked || IsReadableSum)
                        {
                            textBox = new TextBlock()
                            {
                                [!TextBlock.DataContextProperty] = new Binding(item.Binding),
                                [!TextBlock.TextProperty] = new Binding("Value"),
                                [!TextBox.BackgroundProperty] = cell[!Cell.ChooseColorProperty]
                            };
                            if (item.Blocked)
                            {
                                textBox[!TextBox.BackgroundProperty] = cell[!Cell.ChooseColorProperty];
                            }
                            ((TextBlock)textBox).TextAlignment = TextAlignment.Center;
                            textBox.VerticalAlignment = VerticalAlignment.Center;
                            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                            ((TextBlock)textBox).Padding = new Thickness(0, 5, 0, 5);
                            textBox.Height = 30;
                            textBox.ContextMenu = new ContextMenu() { Width = 0, Height = 0 };
                        }
                        else
                        {
                            textBox = new TextBox()
                            {
                                [!TextBox.DataContextProperty] = new Binding(item.Binding),
                                [!TextBox.TextProperty] = new Binding("Value"),
                                [!TextBox.BackgroundProperty] = cell[!Cell.ChooseColorProperty],
                            };
                            ((TextBox)textBox).TextAlignment = TextAlignment.Left;
                            textBox.VerticalAlignment = VerticalAlignment.Stretch;
                            textBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                            textBox.ContextMenu = new ContextMenu() { Width = 0, Height = 0 };
                            if (item.IsTextWrapping)
                            {
                                ((TextBox)textBox).TextWrapping = TextWrapping.Wrap;
                                ((TextBox)textBox).AcceptsReturn = true;
                            }
                        }

                        cell.Control = textBox;

                        RowStackPanel.Children.Add(cell);
                        if (count + 1 < lst.Count * 2 - 1)
                        {
                            RowStackPanel.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Parse(GridSplitterSize.ToString()) });
                            count += 2;
                        }
                        else
                        {
                            count++;
                        }
                        Column++;
                    }

                    RowStackPanel.IsVisible = false;
                    CenterStackPanel.Children.Add(RowStackPanel);
                    Rows.Add(RowStackPanel);
                }
            }
        }

        private void MakeHeaderRows()
        {
            var Columns = this.Columns;
            MakeHeaderInner(Columns);
        }

        private void MakeCenterRows()
        {
            var Columns = this.Columns;
            MakeCenterInner(Columns);
        }

        private void MakeAll()
        {
            #region Main_<MainStackPanel>
            Panel MainPanel = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            Grid MainStackPanel = new Grid() {};
            MainStackPanel.VerticalAlignment = VerticalAlignment.Stretch;
            MainPanel.Children.Add(MainStackPanel);
            #endregion

            #region Search
            if(Search)
            {
                MainStackPanel.RowDefinitions.Add(new RowDefinition() { Height=GridLength.Parse("35") });
                Border HeaderSearchBorder = new()
                {
                    Margin = Thickness.Parse("0,0,0,5"),
                    BorderThickness = Thickness.Parse("1"),
                    BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                    CornerRadius = CornerRadius.Parse("2,2,2,2"),
                    [Grid.RowProperty] = 0
                };
                MainStackPanel.Children.Add(HeaderSearchBorder);

                StackPanel HeaderSearchStackPanel = new();
                HeaderSearchStackPanel.Background = new SolidColorBrush(Color.FromArgb(150, 180, 154, 255));
                HeaderSearchStackPanel.Orientation = Orientation.Vertical;
                HeaderSearchBorder.Child = HeaderSearchStackPanel;

                StackPanel HeaderSearchSP = new();
                TextBox SearchTextBox = new TextBox() 
                {
                    Name = "SearchText",
                    Watermark = "Поиск...",
                    Margin = Thickness.Parse("1,1,1,1")
                };
                SearchTextBox[!TextBox.TextProperty] = this[!DataGrid<T>.SearchTextProperty];
                HeaderSearchSP.Children.Add(SearchTextBox);
                HeaderSearchStackPanel.Children.Add(HeaderSearchSP);

            }
            #endregion

            #region Header
            MainStackPanel.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            Border HeaderBorder = new()
            {
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                CornerRadius = CornerRadius.Parse("2,2,2,2"),
                [Grid.RowProperty] = 1 - (Search ? 0 : 1)
            };
            MainStackPanel.Children.Add(HeaderBorder);

            Panel HeaderPanel = new();
            HeaderPanel.Background = new SolidColorBrush(Color.FromArgb(150,180, 154, 255));
            HeaderBorder.Child = HeaderPanel;

            HeaderStackPanel = new();
            if (!Sum)
            {
                HeaderStackPanel.Margin = Thickness.Parse("2,2,20,2");
            }
            else
            {
                HeaderStackPanel.Margin = Thickness.Parse("20,2,20,2");
            }
            HeaderStackPanel.Orientation = Orientation.Vertical;

            

            if (Comment != null && Comment != "")
            {
                StackPanel HeaderStackPanel1 = new() { Orientation = Orientation.Vertical};
                HeaderPanel.Children.Add(HeaderStackPanel1);
                StackPanel HeaderStackPanel2 = new();
                HeaderStackPanel2[!StackPanel.MarginProperty] = this[!DataGrid<T>.FixedContentProperty];
                HeaderStackPanel2.Orientation = Orientation.Horizontal;
                HeaderStackPanel2.Children.Add(new TextBlock() { Text = Comment, Margin = Thickness.Parse("5,5,0,5") });
                HeaderStackPanel.Children.Add(HeaderStackPanel2);
            }
            HeaderPanel.Children.Add(HeaderStackPanel);
            #endregion

            #region Center
            MainStackPanel.RowDefinitions.Add(new RowDefinition() { Height=GridLength.Parse("*") });
            Border CenterBorder = new()
            {
                Margin = Thickness.Parse("0,5,0,0"),
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                CornerRadius = CornerRadius.Parse("2,2,2,2"),
                VerticalAlignment = VerticalAlignment.Stretch,
                [Grid.RowProperty] = 2 - (Search ? 0 : 1)
            };
            MainStackPanel.Children.Add(CenterBorder);

            Panel CenterPanel = new()
            {
                //Background=new SolidColorBrush(Color.Parse("Red")),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            CenterPanel.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            if (!Sum)
            {
                ScrollViewer CenterScrollViewer = new ScrollViewer();
                CenterScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
                CenterScrollViewer.Content = CenterPanel;
                CenterScrollViewer.VerticalAlignment = VerticalAlignment.Stretch;

                CenterBorder.Child = CenterScrollViewer;
            }
            else
            {
                Panel pnl = new Panel();
                int h = 500;
                Canvas CenterCanvas = new Canvas() { Height=h};
                //CenterPanel.Height = h;

                ScrollBar bar = new ScrollBar() {ZIndex=999,Height=h,HorizontalAlignment=HorizontalAlignment.Right};
                bar[!ScrollBar.MarginProperty] = this[!DataGrid<T>.FixedContentProperty];
                CenterCanvas.Children.Add(bar);

                Binding b = new Binding() {
                    Source = bar,
                    Path = nameof(bar.Value),
                    Mode = BindingMode.TwoWay
                };

                ScrollViewer CenterScrollViewer = new ScrollViewer() { Height=h};
                CenterScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;

                CenterScrollViewer.Content = CenterPanel;

                bar[!ScrollBar.MaximumProperty] = CenterScrollViewer[!ScrollViewer.VerticalScrollBarMaximumProperty];

                CenterScrollViewer[!ScrollViewer.VerticalScrollBarValueProperty] = b;
                CenterCanvas.Children.Add(CenterScrollViewer);

                pnl.Children.Add(CenterCanvas);
                CenterBorder.Child = pnl;

                double w = 0;
                int i = 0;
                var RDef = ((DataGridRow)CenterStackPanel.Children.FirstOrDefault()).ColumnDefinitions;
                foreach (var r in RDef)
                {
                    w += r.Width.Value-1;
                }
                CenterPanel.Width = w;
            }

            CenterStackPanel = new();
            CenterStackPanel.Orientation = Orientation.Vertical;
            if (!Sum)
            {
                CenterStackPanel.Margin = Thickness.Parse("2,2,2,2");
            }
            else
            {
                CenterStackPanel.Margin = Thickness.Parse("20,2,20,2");
            }
            CenterPanel.Children.Add(CenterStackPanel);
            #endregion

            #region MiddleFooter
            MainStackPanel.RowDefinitions.Add(new RowDefinition() {Height= GridLength.Auto });
            Border MiddleFooterBorder = new()
            {
                Margin = Thickness.Parse("0,5,0,0"),
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                CornerRadius = CornerRadius.Parse("2,2,2,2"),
                [Grid.RowProperty] = 3 - (Search ? 0 : 1)
            };
            MainStackPanel.Children.Add(MiddleFooterBorder);

            StackPanel MiddleFooterStackPanel = new();
            MiddleFooterStackPanel.Background = new SolidColorBrush(Color.FromArgb(150, 180, 154, 255));
            MiddleFooterStackPanel.Orientation = Orientation.Vertical;
            MiddleFooterBorder.Child = MiddleFooterStackPanel;

            if (Sum) 
            {
                StackPanel MiddleFooterStackPanelS = new();
                MiddleFooterStackPanelS.Name = "SumColumn";
                MiddleFooterStackPanelS[!StackPanel.MarginProperty] = this[!DataGrid<T>.FixedContentProperty];
                MiddleFooterStackPanelS.Orientation = Orientation.Horizontal;
                MiddleFooterStackPanelS.Children.Add(new TextBlock() { Text = "Сумма:", Margin = Thickness.Parse("5,0,0,0"), IsVisible = false });
                MiddleFooterStackPanelS.Children.Add(new TextBlock() { [!TextBox.TextProperty] = this[!DataGrid<T>.SumColumnProperty], Margin = Thickness.Parse("5,0,0,0"), IsVisible = false});
                MiddleFooterStackPanel.Children.Add(MiddleFooterStackPanelS);
            }
            StackPanel MiddleFooterStackPanel1 = new();
            MiddleFooterStackPanel1[!StackPanel.MarginProperty] = this[!DataGrid<T>.FixedContentProperty];
            MiddleFooterStackPanel1.Orientation = Orientation.Horizontal;
            MiddleFooterStackPanel1.Children.Add(new TextBlock() { Text = "Кол-во страниц:",Margin=Thickness.Parse("5,0,0,0") });
            MiddleFooterStackPanel1.Children.Add(new TextBlock() { [!TextBox.TextProperty] = this[!DataGrid<T>.PageCountProperty], Margin = Thickness.Parse("5,0,0,0") });
            MiddleFooterStackPanel.Children.Add(MiddleFooterStackPanel1);

            StackPanel MiddleFooterStackPanel2 = new();
            MiddleFooterStackPanel2[!StackPanel.MarginProperty] = this[!DataGrid<T>.FixedContentProperty];
            MiddleFooterStackPanel2.Orientation = Orientation.Horizontal;
            MiddleFooterStackPanel2.Children.Add(new TextBlock() { Text = "Кол-во строчек:", Margin = Thickness.Parse("5,0,0,0") });
            MiddleFooterStackPanel2.Children.Add(new TextBlock() { [!TextBox.TextProperty] = this[!DataGrid<T>.ItemsCountProperty], Margin = Thickness.Parse("5,0,0,0") });
            MiddleFooterStackPanel.Children.Add(MiddleFooterStackPanel2);
            #endregion

            #region Footer
            MainStackPanel.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Parse("45") });
            Border FooterBorder = new()
            {
                Margin = Thickness.Parse("0,5,0,0"),
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(Color.Parse("Gray")),
                CornerRadius = CornerRadius.Parse("2,2,2,2"),
                [Grid.RowProperty] = 4 - (Search ? 0 : 1)
            };
            MainStackPanel.Children.Add(FooterBorder);

            Panel FooterPanel = new();
            FooterPanel.Background = new SolidColorBrush(Color.FromArgb(150, 180, 154, 255));
            FooterPanel.Height = 40;
            FooterBorder.Child=FooterPanel;

            StackPanel FooterStackPanel = new StackPanel()
            {
                Margin = Thickness.Parse("5,0,0,0"),
                Orientation = Orientation.Horizontal,
                Spacing = 5
            };
            FooterPanel.Children.Add(FooterStackPanel);

            Button btnDown = new Button
            {
                Content = "<",
                Width = 30,
                Height = 30,
                CornerRadius = CornerRadius.Parse("2,2,2,2"),
                [!Button.MarginProperty] = this[!DataGrid<T>.FixedContentProperty]
            };
            btnDown.Click += NowPageDown;
            FooterStackPanel.Children.Add(btnDown);

            TextBox box = new TextBox()
            {
                [!TextBox.TextProperty] = this[!DataGrid<T>.NowPageProperty],
                TextAlignment = TextAlignment.Center,
                CornerRadius = CornerRadius.Parse("2,2,2,2"),
            };
            box.Width = 30;
            box.Height = 30;
            FooterStackPanel.Children.Add(box);

            Button btnUp = new Button
            {
                Content = ">",
                Width = 30,
                Height = 30,
                CornerRadius = CornerRadius.Parse("2,2,2,2"),
            };
            btnUp.Click += NowPageUp;
            FooterStackPanel.Children.Add(btnUp);

            #endregion

            Content = MainPanel;
        }
        #endregion
    }
}
