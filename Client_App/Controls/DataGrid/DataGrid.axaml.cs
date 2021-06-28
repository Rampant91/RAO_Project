using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

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
        public static readonly DirectProperty<DataGrid, IEnumerable<IChanged>> ItemsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, IEnumerable<IChanged>>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v, defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

        private IEnumerable<IChanged> _items = new ObservableCollectionWithItemPropertyChanged<IChanged>();

        public IEnumerable<IChanged> Items
        {
            get => _items;
            set
            {
                if (value != null)
                {
                    SetAndRaise(ItemsProperty, ref _items, value);
                    UpdateCells();
                }
            }
        }

        public static readonly DirectProperty<DataGrid, IEnumerable<IChanged>> SelectedItemsProperty =
                AvaloniaProperty.RegisterDirect<DataGrid, IEnumerable<IChanged>>(
                    nameof(SelectedItems),
                    o => o.SelectedItems,
                    (o, v) => o.SelectedItems = v);

        private IEnumerable<IChanged> _selecteditems = new ObservableCollectionWithItemPropertyChanged<IChanged>();
        public IEnumerable<IChanged> SelectedItems
        {
            get => _selecteditems;
            set
            {
                if (value != null)
                {
                    SetAndRaise(SelectedItemsProperty, ref _selecteditems, value);
                }
            }
        }

        public static readonly DirectProperty<DataGrid, string> TypeProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, string>(
                nameof(Type),
                o => o.Type,
                (o, v) => o.Type = v);
        private string _type = "";
        public string Type
        {
            get => _type;
            set
            {
                SetAndRaise(TypeProperty, ref _type, value);
                MakeHeader();
            }
        }

        public static readonly StyledProperty<ChooseMode> ChooseModeProperty =
            AvaloniaProperty.Register<DataGrid, ChooseMode>(nameof(ChooseMode));

        public ChooseMode ChooseMode
        {
            get => GetValue(ChooseModeProperty);
            set => SetValue(ChooseModeProperty, value);
        }

        public static readonly StyledProperty<MultilineMode> MultilineModeProperty =
            AvaloniaProperty.Register<DataGrid, MultilineMode>(nameof(MultilineMode));

        public MultilineMode MultilineMode
        {
            get => GetValue(MultilineModeProperty);
            set => SetValue(MultilineModeProperty, value);
        }

        public static readonly StyledProperty<Brush> ChooseColorProperty =
            AvaloniaProperty.Register<DataGrid, Brush>(nameof(ChooseColor));

        public Brush ChooseColor
        {
            get => GetValue(ChooseColorProperty);
            set => SetValue(ChooseColorProperty, value);
        }


        public Panel Columns { get; set; }
        private RowCollection Rows { get; set; }

        public DataGrid()
        {
            InitializeComponent();

            ItemsProperty.Changed.Subscribe(new ItemsObserver(ItemsChanged));
        }

        private readonly List<Control> SelectedCells = new List<Control>();

        private void SetSelectedControls()
        {
            if (ChooseMode == ChooseMode.Cell)
            {
                if (MultilineMode == MultilineMode.Multi)
                {
                    SetSelectedControls_CellMulti();
                }
                if (MultilineMode == MultilineMode.Single)
                {
                    SetSelectedControls_CellSingle();
                }
            }
            if (ChooseMode == ChooseMode.Line)
            {
                if (MultilineMode == MultilineMode.Multi)
                {
                    SetSelectedControls_LineMulti();
                }
                if (MultilineMode == MultilineMode.Single)
                {
                    SetSelectedControls_LineSingle();
                }
            }
        }

        private void SetSelectedControls_LineSingle()
        {
            int Row = FirstPressedItem[0];
            Control[]? sel = SelectedCells.ToArray();
            foreach (Row item in sel)
            {
                if (item.SRow != Row)
                {
                    item.Background = Background;
                    SelectedCells.Remove(item);
                }
            }
            if (!SelectedCells.Contains(Rows[Row] == null ? null : Rows[Row].SCells))
            {
                if (Rows[Row] != null)
                {
                    Rows[Row].SCells.Background = ChooseColor;
                    SelectedCells.Add(Rows[Row].SCells);
                }
            }
        }

        private void SetSelectedControls_CellSingle()
        {
            int Row = FirstPressedItem[0];
            int Column = FirstPressedItem[1];
            Control[]? sel = SelectedCells.ToArray();
            foreach (Cell item in sel)
            {
                if (item.CellRow != Row && item.CellColumn != Column)
                {
                    item.Background = Background;
                    SelectedCells.Remove(item);
                }
            }
            if (!SelectedCells.Contains(Rows[Row, Column]))
            {
                if (Rows[Row, Column] != null)
                {
                    Rows[Row, Column].Background = ChooseColor;
                    SelectedCells.Add(Rows[Row, Column]);
                }
            }
        }

        private void SetSelectedControls_LineMulti()
        {
            int minRow = Math.Min(FirstPressedItem[0], LastPressedItem[0]);
            int maxRow = Math.Max(FirstPressedItem[0], LastPressedItem[0]);
            Control[]? sel = SelectedCells.ToArray();
            foreach (Row item in sel)
            {
                if (!(item.SRow >= minRow && item.SRow <= maxRow))
                {
                    item.Background = Background;
                    SelectedCells.Remove(item);
                }
            }
            for (int i = minRow; i <= maxRow; i++)
            {
                if (!SelectedCells.Contains(Rows[i].SCells))
                {
                    if (Rows[i] != null)
                    {
                        Rows[i].SCells.Background = ChooseColor;
                        SelectedCells.Add(Rows[i].SCells);
                    }
                }
            }
        }

        private void SetSelectedControls_CellMulti()
        {
            int minRow = Math.Min(FirstPressedItem[0], LastPressedItem[0]);
            int maxRow = Math.Max(FirstPressedItem[0], LastPressedItem[0]);
            int minColumn = Math.Min(FirstPressedItem[1], LastPressedItem[1]);
            int maxColumn = Math.Max(FirstPressedItem[1], LastPressedItem[1]);
            Control[]? sel = SelectedCells.ToArray();
            foreach (Cell item in sel)
            {
                if (!(item.CellRow >= minRow && item.CellRow <= maxRow))
                {
                    item.Background = Background;
                    SelectedCells.Remove(item);
                }
                if (!(item.CellColumn >= minColumn && item.CellColumn <= maxColumn))
                {
                    item.Background = Background;
                    SelectedCells.Remove(item);
                }
            }
            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = minColumn; j <= maxColumn; j++)
                {
                    if (!SelectedCells.Contains(Rows[i, j]))
                    {
                        if (Rows[i, j] != null)
                        {
                            Rows[i, j].Background = ChooseColor;
                            SelectedCells.Add(Rows[i, j]);
                        }
                    }
                }
            }
        }

        private void SetSelectedItems()
        {
            ObservableCollectionWithItemPropertyChanged<IChanged>? lst = new ObservableCollectionWithItemPropertyChanged<IChanged>();
            if (FirstPressedItem[0] != 0 && FirstPressedItem[1] != 0)
            {
                if (LastPressedItem[0] != 0 && LastPressedItem[1] != 0)
                {
                    foreach (Control? item in SelectedCells)
                    {
                        if (item is Cell)
                        {
                            Border? ch = (Border)((Cell)item).Content;
                            Panel? ch2 = (Panel)ch.Child;
                            TextBox? text = (TextBox)ch2.Children[0];
                            lst.Add((IChanged)text.DataContext);
                        }
                        if (item is StackPanel)
                        {
                            Cell? ch = (Cell)((StackPanel)item).Children[0];
                            lst.Add((IChanged)ch.DataContext);
                        }
                    }
                }
            }
            _selecteditems = lst;
        }

        private void SetSelectedItemsWithHandler()
        {
            ObservableCollectionWithItemPropertyChanged<IChanged>? lst = new ObservableCollectionWithItemPropertyChanged<IChanged>();
            if (FirstPressedItem[0] != 0 && FirstPressedItem[1] != 0)
            {
                if (LastPressedItem[0] != 0 && LastPressedItem[1] != 0)
                {
                    foreach (Control? item in SelectedCells)
                    {
                        if (item is Cell)
                        {
                            Border? ch = (Border)((Cell)item).Content;
                            Panel? ch2 = (Panel)ch.Child;
                            TextBox? text = (TextBox)ch2.Children[0];
                            lst.Add((IChanged)text.DataContext);
                        }
                        if (item is StackPanel)
                        {
                            IChanged? ch = (IChanged)item.DataContext;
                            lst.Add(ch);
                        }
                    }
                }
            }
            SelectedItems = lst;
        }

        private int[] FirstPressedItem { get; set; } = new int[2];
        private int[] LastPressedItem { get; set; } = new int[2];

        private void CellPropChangeEventHandler(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Down")
            {
                FirstPressedItem[0] = ((Cell)sender).CellRow;
                FirstPressedItem[1] = ((Cell)sender).CellColumn;
                LastPressedItem[0] = ((Cell)sender).CellRow;
                LastPressedItem[1] = ((Cell)sender).CellColumn;
            }
            if (args.PropertyName == "DownMove")
            {
                LastPressedItem[0] = ((Cell)sender).CellRow;
                LastPressedItem[1] = ((Cell)sender).CellColumn;
            }
            if (args.PropertyName == "Up")
            {
                LastPressedItem[0] = ((Cell)sender).CellRow;
                LastPressedItem[1] = ((Cell)sender).CellColumn;
            }
            if (args.PropertyName == "Down" ||
                args.PropertyName == "DownMove" ||
                args.PropertyName == "Up")
            {
                SetSelectedControls();
                SetSelectedItemsWithHandler();
            }
        }

        private void UpdateAllCells()
        {
            NameScope scp = new NameScope();
            scp.Register(Name, this);
            Rows.Clear();
            int count = 1;
            foreach (IChanged? item in _items)
            {
                Row? tmp = (Row)Support.RenderDataGridRow.Render.GetControl(Type, count, scp, Name);
                Rows.Add(new CellCollection(tmp, CellPropChangeEventHandler), count);
                count++;
            }
            SetSelectedControls();
            SetSelectedItemsWithHandler();
        }

        private void UpdateCells()
        {
            NameScope scp = new NameScope();
            scp.Register(Name, this);
            int count = 1;
            foreach (IChanged? item in _items)
            {
                if (Rows.Count >= count)
                {
                    if (Rows[count, 1] != null)
                    {
                        if (((IKey)(Rows[count].SCells).DataContext) != null)
                        {
                            if (((IKey)(Rows[count].SCells).DataContext).Id != ((IKey)item).Id)
                            {
                                Row? tmp = (Row)Support.RenderDataGridRow.Render.GetControl(Type, count, scp, Name);
                                Rows.Add(new CellCollection(tmp, CellPropChangeEventHandler), count);
                            }
                        }
                    }
                    else
                    {
                        Row? tmp = (Row)Support.RenderDataGridRow.Render.GetControl(Type, count, scp, Name);
                        Rows.Add(new CellCollection(tmp, CellPropChangeEventHandler), count);
                        count++;
                    }
                }
                else
                {
                    Row? tmp = (Row)Support.RenderDataGridRow.Render.GetControl(Type, count, scp, Name);
                    Rows.Add(new CellCollection(tmp, CellPropChangeEventHandler), count);
                    count++;
                }
                count++;
            }
            SetSelectedControls();
            SetSelectedItemsWithHandler();
        }

        private void ItemsChanged(object sender, PropertyChangedEventArgs args)
        {
            if (Rows.Count > 0)
            {
                UpdateCells();
            }
            else
            {
                UpdateAllCells();
            }
        }

        public void MakeHeader()
        {
            Columns.Children.Clear();
            Columns.Children.Add(Support.RenderDataGridHeader.Render.GetControl(Type));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            Init();
        }

        private void Init()
        {

            Border brd = new Border
            {
                BorderThickness = Thickness.Parse("1"),
                BorderBrush = new SolidColorBrush(Color.Parse("Gray"))
            };

            ScrollViewer vwm = new ScrollViewer
            {
                //vw.SetValue(Grid.RowProperty, 1);
                Background = new SolidColorBrush(Color.Parse("WhiteSmoke")),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled,
                HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto
            };
            brd.Child = vwm;

            Panel p = new Panel
            {
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch
            };
            vwm.Content = p;

            Grid grd = new Grid();
            RowDefinition rd = new RowDefinition
            {
                Height = GridLength.Parse("30")
            };
            grd.RowDefinitions.Add(rd);
            grd.RowDefinitions.Add(new RowDefinition());
            p.Children.Add(grd);

            Panel pnl = new Panel();
            pnl.SetValue(Grid.RowProperty, 0);
            pnl.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            grd.Children.Add(pnl);
            Columns = pnl;

            ScrollViewer vw = new ScrollViewer();
            vw.SetValue(Grid.RowProperty, 1);
            vw.Background = new SolidColorBrush(Color.Parse("WhiteSmoke"));
            vw.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled;
            vw.VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto;
            grd.Children.Add(vw);

            StackPanel stck = new StackPanel
            {
                Margin = Thickness.Parse("0,-1,0,0"),
                Spacing = -1,
                Orientation = Avalonia.Layout.Orientation.Vertical,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch
            };
            vw.Content = stck;
            Rows = new RowCollection(stck, CellPropChangeEventHandler);

            Content = brd;
        }
    }
}
