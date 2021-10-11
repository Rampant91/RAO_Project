using System;
using System.Collections.Generic;
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
using Collections;

namespace Client_App.Controls.DataGrid
{
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

    public class DataGrid : UserControl
    {
        public static readonly DirectProperty<DataGrid, IEnumerable<INotifyPropertyChanged>> ItemsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, IEnumerable<INotifyPropertyChanged>>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v, defaultBindingMode: BindingMode.TwoWay);

        public static readonly DirectProperty<DataGrid, IEnumerable<INotifyPropertyChanged>> SelectedItemsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, IEnumerable<INotifyPropertyChanged>>(
                nameof(SelectedItems),
                o => o.SelectedItems,
                (o, v) => o.SelectedItems = v);

        public static readonly DirectProperty<DataGrid, IList<INotifyPropertyChanged>> SelectedCellsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, IList<INotifyPropertyChanged>>(
                nameof(SelectedCells),
                o => o.SelectedCells,
                (o, v) => o.SelectedCells = v);

        public static readonly DirectProperty<DataGrid, string> TypeProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, string>(
                nameof(Type),
                o => o.Type,
                (o, v) => o.Type = v);

        public static readonly StyledProperty<ChooseMode> ChooseModeProperty =
            AvaloniaProperty.Register<DataGrid, ChooseMode>(nameof(ChooseMode));

        public static readonly StyledProperty<MultilineMode> MultilineModeProperty =
            AvaloniaProperty.Register<DataGrid, MultilineMode>(nameof(MultilineMode));

        public static readonly StyledProperty<Brush> ChooseColorProperty =
            AvaloniaProperty.Register<DataGrid, Brush>(nameof(ChooseColor));

        private IList<INotifyPropertyChanged> _selectedCells =
            new List<INotifyPropertyChanged>();

        private IEnumerable<INotifyPropertyChanged> _items =
            new ObservableCollectionWithItemPropertyChanged<IKey>();

        private IEnumerable<INotifyPropertyChanged> _selecteditems =
            new ObservableCollectionWithItemPropertyChanged<IKey>();

        private string _type = "";

        public static readonly DirectProperty<DataGrid, bool> PaginationProperty =
     AvaloniaProperty.RegisterDirect<DataGrid, bool>(
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
                ItemsChanged(null, null);
            }
        }
        public static readonly DirectProperty<DataGrid, int> PageSizeProperty =
             AvaloniaProperty.RegisterDirect<DataGrid, int>(
        nameof(PageSize),
        o => o.PageSize,
        (o, v) => o.PageSize = v);

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                SetAndRaise(PageSizeProperty, ref _pageSize, value);
                ItemsChanged(null, null);
            }
        }
        public static readonly DirectProperty<DataGrid, string> NowPageProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, string>(
            nameof(NowPage),
            o => o.NowPage,
            (o, v) => o.NowPage = v,defaultBindingMode:BindingMode.TwoWay);

        private string _nowPage = "1";
        public string NowPage
        {
            get => _nowPage.ToString();
            set
            {
                try
                {
                    var val = Convert.ToInt32(value);
                    if (val != null)
                    {
                        int maxpage = (Items.Count() / PageSize) + 1;
                        if (val == 0)
                        {
                            val = 1;
                        }
                        if (val > maxpage)
                        {
                            SetAndRaise(NowPageProperty, ref _nowPage, maxpage.ToString());
                            ItemsChanged(null, null);
                        }
                        else
                        {
                            SetAndRaise(NowPageProperty, ref _nowPage, value);
                            ItemsChanged(null, null);
                        }
                    }
                }
                catch
                {

                }
            }
        }

        private IEnumerable<INotifyPropertyChanged> GetPageItems()
        {
            if (Pagination)
            {
                var items = _items.ToList();
                var val = Convert.ToInt32(_nowPage);
                int first_index = (val-1) * PageSize;
                int last_index = Math.Min(items.Count, first_index + PageSize);

                if (last_index <= first_index)
                {
                    items.Clear();
                    return items;
                }
                var pageItems = items.GetRange(first_index, last_index - first_index);

                return pageItems;
            }
            else
            {
                return _items;
            }
        }

        public DataGrid()
        {
            InitializeComponent();

            ItemsProperty.Changed.Subscribe(new ItemsObserver(ItemsChanged));
        }

        public IEnumerable<INotifyPropertyChanged> Items
        {
            get => _items;
            set
            {
                if (value != null)
                {
                    SetAndRaise(ItemsProperty, ref _items, value);
                    ItemsChanged(null, null);
                }
            }
        }

        public IEnumerable<INotifyPropertyChanged> SelectedItems
        {
            get => _selecteditems;
            set
            {
                if (value != null) SetAndRaise(SelectedItemsProperty, ref _selecteditems, value);
            }
        }
        public IList<INotifyPropertyChanged> SelectedCells
        {
            get => _selectedCells;
            set
            {
                if (value != null) SetAndRaise(SelectedCellsProperty, ref _selectedCells, value);
            }
        }

        public string Type
        {
            get => _type;
            set
            {
                SetAndRaise(TypeProperty, ref _type, value);
                MakeHeader();
            }
        }

        public ChooseMode ChooseMode
        {
            get => GetValue(ChooseModeProperty);
            set => SetValue(ChooseModeProperty, value);
        }

        public MultilineMode MultilineMode
        {
            get => GetValue(MultilineModeProperty);
            set => SetValue(MultilineModeProperty, value);
        }

        public Brush ChooseColor
        {
            get => GetValue(ChooseColorProperty);
            set => SetValue(ChooseColorProperty, value);
        }


        private RowCollection Columns { get; set; }
        private RowCollection Rows { get; set; }
        private Grid MainGrid { get; set; }

        public bool DownFlag { get; set; }
        public int[] FirstPressedItem { get; set; } = new int[2];
        public int[] LastPressedItem { get; set; } = new int[2];

        private void SetSelectedControls()
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

        private void SetSelectedControls_LineSingle()
        {
            var Row = LastPressedItem[0];
            var sel = SelectedCells.ToArray();
            foreach (Row item in sel)
                if (item.SRow != Row)
                {
                    var cells = item.Children;
                    foreach (Cell it in cells)
                    {
                        it.Background = Background;
                    }
                }

            SelectedCells.Clear();
            if (!SelectedCells.Contains(Rows[Row] == null ? null : Rows[Row].SCells))
                if (Rows[Row] != null)
                {
                    foreach (var item in Rows[Row].Cells)
                    {
                        item.Value.Background = ChooseColor;
                    }
                    SelectedCells.Add(Rows[Row].SCells);
                }
        }

        private void SetSelectedControls_CellSingle()
        {
            var Row = LastPressedItem[0];
            var Column = LastPressedItem[1];
            var sel = SelectedCells.ToArray();
            foreach (Cell item in sel)
                if (item.CellRow != Row && item.CellColumn != Column)
                {
                    item.Background = Background;
                }

            SelectedCells.Clear();
            if (!SelectedCells.Contains(Rows[Row, Column]))
                if (Rows[Row, Column] != null)
                {
                    Rows[Row, Column].Background = ChooseColor;
                    SelectedCells.Add(Rows[Row, Column]);
                }
        }

        private void SetSelectedControls_LineMulti()
        {
            var minRow = Math.Min(FirstPressedItem[0], LastPressedItem[0]);
            var maxRow = Math.Max(FirstPressedItem[0], LastPressedItem[0]);
            var sel = SelectedCells.ToArray();

            foreach (Row item in sel)
                if (!(item.SRow >= minRow && item.SRow <= maxRow))
                {
                    var cells = item.Children;
                    foreach (Cell it in cells)
                    {
                        it.Background = Background;
                    }
                }

            SelectedCells.Clear();
            for (var i = minRow; i <= maxRow; i++)
                if (!SelectedCells.Contains(Rows[i].SCells))
                    if (Rows[i] != null)
                    {
                        foreach (var item in Rows[i].Cells)
                        {
                            item.Value.Background = ChooseColor;
                        }
                        SelectedCells.Add(Rows[i].SCells);
                    }
        }

        private void SetSelectedControls_CellMulti()
        {
            var minRow = Math.Min(FirstPressedItem[0], LastPressedItem[0]);
            var maxRow = Math.Max(FirstPressedItem[0], LastPressedItem[0]);
            var minColumn = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
            var maxColumn = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
            var sel = SelectedCells.ToArray();
            foreach (Cell item in sel)
            {
                if (!(item.CellRow >= minRow && item.CellRow <= maxRow))
                {
                    item.Background = Background;
                }

                if (!(item.CellColumn >= minColumn && item.CellColumn <= maxColumn))
                {
                    item.Background = Background;
                }
            }
            SelectedCells.Clear();
            for (var i = minRow; i <= maxRow; i++)
            {
                for (var j = minColumn; j <= maxColumn; j++)
                {
                    if (Rows[i, j] != null)
                    {
                        Rows[i, j].Background = ChooseColor;
                        SelectedCells.Add(Rows[i, j]);
                    }
                }
            }
        }

        public void SetSelectedItems()
        {
            var lst = new ObservableCollectionWithItemPropertyChanged<IKey>();
            foreach (var item in SelectedCells)
            {
                if (item is Cell)
                {
                    var ch = (Border)((Cell)item).Content;
                    var ch2 = (Panel)ch.Child;
                    var text = (TextBox)ch2.Children[0];
                    lst.Add((IKey)text.DataContext);
                }

                if (item is StackPanel)
                {
                    var ch = (Cell)((StackPanel)item).Children[0];
                    lst.Add((IKey)ch.DataContext);
                }

                _selecteditems = lst;
            }
        }

        private void SetSelectedItemsWithHandler()
        {
            var lst = new ObservableCollectionWithItemPropertyChanged<IKey>();
            foreach (var item in SelectedCells)
            {
                if (item is Cell)
                {
                    var ch = (Row)((Cell)item).Parent;
                    lst.Add((IKey)ch.DataContext);
                }

                if (item is StackPanel)
                {
                    var ch = (IKey)(item as StackPanel).DataContext;
                    lst.Add(ch);
                }
            }

            SelectedItems = lst;
        }

        private int[] FindCell(PointerPoint Mainmouse)
        {
            PointerPoint mouse = Mainmouse;

            var num = Convert.ToInt32(_nowPage);
            var offset = (num - 1) * PageSize;
            var h = Rows[1+offset, 1].Height;
            int[] ret = new int[2];

            var t1 = (int)Math.Round(mouse.Position.Y / h, 0, MidpointRounding.ToNegativeInfinity) + 1+offset;
            if (t1 <= Rows.Count && t1 > 0)
            {
                ret[0] = t1;
                double sum = 0;
                for (var i = 1; i <= Rows[t1].Count; i++)
                {
                    var tp = Rows[t1, i];
                    if (tp != null)
                    {
                        sum += tp.Width;
                    }

                    if (mouse.Position.X <= sum)
                    {
                        ret[1] = i;
                        break;
                    }
                }
            }

            return ret;
        }

        public void DataGridPointerDown(object sender, PointerPressedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((StackPanel)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed ||
                mouse.Properties.PointerUpdateKind == PointerUpdateKind.RightButtonPressed)
                if (Rows.Count > 0)
                {
                    var tmp = FindCell(mouse);
                    FirstPressedItem = tmp;
                    LastPressedItem = tmp;

                    DownFlag = true;
                    SetSelectedControls();
                    SetSelectedItemsWithHandler();
                }
        }

        public void DataGridPointerMoved(object sender, PointerEventArgs args)
        {
            var mouse = args.GetCurrentPoint((StackPanel)sender);
            if (DownFlag)
            {
                if (Rows.Count > 0)
                {
                    var tmp = FindCell(mouse);
                    LastPressedItem = tmp;
                }

                SetSelectedControls();
                SetSelectedItemsWithHandler();
            }
        }

        public void DataGridPointerUp(object sender, PointerReleasedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((StackPanel)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased ||
                mouse.Properties.PointerUpdateKind == PointerUpdateKind.RightButtonReleased)
            {
                if (Rows.Count > 0)
                {
                    var tmp = FindCell(mouse);
                    LastPressedItem = tmp;
                }

                DownFlag = false;
                SetSelectedControls();
                SetSelectedItemsWithHandler();
            }
        }

        private void UpdateAllCells()
        {
            NameScope scp = new();
            scp.Register(Name, this);
            Rows.Clear();

            var num = Convert.ToInt32(_nowPage);
            var offset = (num - 1) * PageSize;
            var count = offset+1;
            var items = GetPageItems();
            foreach (var item in items)
            {
                var tmp = (Row)Support.RenderDataGridRow.Render.GetControl(Type, count, scp, Name);
                Rows.Add(new CellCollection(tmp), count);
                count++;
            }

            SetSelectedControls();
            SetSelectedItemsWithHandler();
        }

        private void UpdateCells()
        {
            //UpdateAllCells();
            NameScope scp = new();
            scp.Register(Name, this);
            var items = GetPageItems();
            if (items.Count() == 0)
            {
                UpdateAllCells();
                return;
            }

            var Id1 = from item in Rows select item.Value.SCells.DataContext.GetHashCode();
            var Id2 = from item in items select item.GetHashCode();

            var Outer1 = Id1.Except(Id2).ToArray();
            var Outer2 = Id2.Except(Id1).ToArray();

            foreach (var item in Outer1)
                foreach (var row in Rows)
                {
                    var tmp = row.Value.SCells.DataContext.GetHashCode();
                    if (item == tmp)
                    {
                        Rows.Remove(Convert.ToInt32(row.Key));
                        break;
                    }
                }

            foreach (var item in Outer2)
                foreach (var row in items)
                {
                    var tmp = row.GetHashCode();
                    if (item == tmp)
                    {
                        var num = Convert.ToInt32(_nowPage);
                        var offset = (num-1) * PageSize;
                        var tp = Rows.GetFreeRow()+offset;
                        var t = (Row)Support.RenderDataGridRow.Render.GetControl(Type, tp, scp, Name);
                        Rows.Add(new CellCollection(t), tp);
                    }
                }

            if (Outer1.Length != 0 || Outer2.Length != 0)
            {
                Rows.Reorgonize(scp, Name);
            }

            SetSelectedControls();
            SetSelectedItemsWithHandler();
        }

        private void ItemsChanged(object sender, PropertyChangedEventArgs args)
        {
            if (Rows.Count > 0)
                UpdateCells();
            else
                UpdateAllCells();
        }

        public void MakeHeader()
        {
            Columns.Clear();

            var lst = Support.RenderDataGridHeader.Render.GetControl(Type);
            if (lst != null)
            {
                foreach (var item in lst)
                {
                    Columns.Add(new CellCollection(item));
                }
                MainGrid.RowDefinitions[0].Height = GridLength.Parse((lst.First().Children.Count * 30).ToString());
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Init();
        }

        private void Init()
        {
            Border brd = new()
            {
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(Color.Parse("Gray"))
            };

            Panel p = new()
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            brd.Child = p;

            Grid grde = new();
            RowDefinition rde = new()
            {
                Height = GridLength.Parse("40")
            };
            grde.RowDefinitions.Add(new RowDefinition());
            grde.RowDefinitions.Add(rde);
            p.Children.Add(grde);

            Panel pan1 = new Panel();
            pan1.HorizontalAlignment = HorizontalAlignment.Stretch;
            pan1.VerticalAlignment = VerticalAlignment.Stretch;
            pan1.SetValue(Grid.RowProperty, 0);
            grde.Children.Add(pan1);

            ScrollViewer vw = new();
            vw.Background = new SolidColorBrush(Color.Parse("WhiteSmoke"));
            vw.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            vw.HorizontalScrollBarVisibility = ScrollBarVisibility.Visible;
            pan1.Children.Add(vw);

            Grid grd = new();
            RowDefinition rd = new()
            {
                Height = GridLength.Parse("0")
            };
            grd.RowDefinitions.Add(rd);
            grd.RowDefinitions.Add(new RowDefinition());
            MainGrid = grd;
            Panel pan3 = new Panel();
            pan3.HorizontalAlignment = HorizontalAlignment.Stretch;
            pan3.VerticalAlignment = VerticalAlignment.Stretch;
            vw.Content = pan3;
            pan3.Children.Add(grd);

            Panel pnl = new();
            pnl.SetValue(Grid.RowProperty, 0);
            pnl.HorizontalAlignment = HorizontalAlignment.Stretch;
            grd.Children.Add(pnl);

            StackPanel stckC = new()
            {
                Margin = Thickness.Parse("0,0,0,0"),
                Spacing = 0,
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            pnl.Children.Add(stckC);
            Columns = new RowCollection(stckC);

            Panel pan = new Panel();
            pan.HorizontalAlignment = HorizontalAlignment.Stretch;
            pan.VerticalAlignment = VerticalAlignment.Stretch;
            pan.SetValue(Grid.RowProperty, 1);
            grd.Children.Add(pan);

            ScrollViewer vw2 = new();
            vw2.Background = new SolidColorBrush(Color.Parse("WhiteSmoke"));
            vw2.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            vw2.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            pan.Children.Add(vw2);

            Panel pn = new Panel();
            pn.HorizontalAlignment = HorizontalAlignment.Stretch;
            pn.VerticalAlignment = VerticalAlignment.Stretch;
            //pn.AddHandler(PointerPressedEvent, DataGridPointerDown, handledEventsToo: true);
            //pn.AddHandler(PointerMovedEvent, DataGridPointerMoved, handledEventsToo: true);
            //pn.AddHandler(PointerReleasedEvent, DataGridPointerUp, handledEventsToo: true);
            vw2.Content = pn;
            StackPanel stck = new()
            {
                Margin = Thickness.Parse("0,-1,0,0"),
                Spacing = 0,
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            stck.AddHandler(PointerPressedEvent, DataGridPointerDown, handledEventsToo: true);
            stck.AddHandler(PointerMovedEvent, DataGridPointerMoved, handledEventsToo: true);
            stck.AddHandler(PointerReleasedEvent, DataGridPointerUp, handledEventsToo: true);
            pn.Children.Add(stck);
            Rows = new RowCollection(stck);

            Panel pnle = new Panel();
            pnle.HorizontalAlignment = HorizontalAlignment.Stretch;
            pnle.VerticalAlignment = VerticalAlignment.Stretch;
            pnle.SetValue(Grid.RowProperty, 1);
            pnle.Background = new SolidColorBrush(Color.Parse("LightGray"));
            grde.Children.Add(pnle);

            TextBox box = new TextBox()
            {
                [!TextBox.TextProperty] = this[!DataGrid.NowPageProperty]
            };
            box.Width = 30;
            box.Height = 30;
            pnle.Children.Add(box);


            Content = brd;
        }
    }
}