using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Collections;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using Avalonia.Collections;
using System.ComponentModel;
using Avalonia.Input;
using Avalonia.Media;
using System;

namespace Client_App.Controls.DataGrid
{
    public enum ChooseMode
    {
        Cell=0,
        Line
    }
    public enum MultilineMode
    {
        Multi=0,
        Single
    }
    public class DataGrid : UserControl
    {
        public static readonly DirectProperty<DataGrid, IEnumerable> ItemsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, IEnumerable>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v,defaultBindingMode:Avalonia.Data.BindingMode.TwoWay);

        private IEnumerable _items;

        public IEnumerable Items
        {
            get { return _items; }
            set
            {
                if (value != null)
                {
                    SetAndRaise(ItemsProperty, ref _items, value);
                    Update();
                }
            }
        }

        public static readonly DirectProperty<DataGrid, IEnumerable> SelectedItemsProperty =
                AvaloniaProperty.RegisterDirect<DataGrid, IEnumerable>(
                    nameof(SelectedItems),
                    o => o.SelectedItems,
                    (o, v) => o.SelectedItems = v);

        private IEnumerable _selecteditems =new ObservableCollection<object>();
        public IEnumerable SelectedItems
        {
            get
            {
                return _selecteditems;
            }
            set
            {
                ObservableCollection<Object> lst = new ObservableCollection<Object>();
                foreach (var item in FindSelectedItems())
                {
                    if (!lst.Contains(item))
                    {
                        lst.Add(item);
                    }
                }
                SetAndRaise(SelectedItemsProperty, ref _selecteditems, lst);
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
            get { return _type; }
            set
            {
                SetAndRaise(TypeProperty, ref _type, value);
                Update();
            }
        }

        public static readonly StyledProperty<ChooseMode> ChooseModeProperty =
            AvaloniaProperty.Register<DataGrid, ChooseMode>(nameof(ChooseMode));

        public ChooseMode ChooseMode
        {
            get { return GetValue(ChooseModeProperty); }
            set { SetValue(ChooseModeProperty, value); }
        }

        public static readonly StyledProperty<MultilineMode> MultilineModeProperty =
            AvaloniaProperty.Register<DataGrid, MultilineMode>(nameof(MultilineMode));

        public MultilineMode MultilineMode
        {
            get { return GetValue(MultilineModeProperty); }
            set { SetValue(MultilineModeProperty, value); }
        }

        public static readonly StyledProperty<Brush> ChooseColorProperty =
            AvaloniaProperty.Register<DataGrid, Brush>(nameof(ChooseColor));

        public Brush ChooseColor
        {
            get { return GetValue(ChooseColorProperty); }
            set { SetValue(ChooseColorProperty, value); }
        }

        public Panel Columns { get; set; }
        public StackPanel Rows { get; set; }

        public DataGrid()
        {
            InitializeComponent();

            ItemsProperty.Changed.Subscribe(new ItemsObserver(ItemsChanged));
            this.AddHandler(PointerPressedEvent, PanelPointerDown, handledEventsToo: true);
            this.AddHandler(PointerMovedEvent, PanelPointerMoved, handledEventsToo: true);
            this.AddHandler(PointerReleasedEvent, PanelPointerUp, handledEventsToo: true);
        }

        public void Update()
        {
            ItemsChanged(null, null);
            MakeHeader();
        }

        void ItemsChanged(object sender, PropertyChangedEventArgs args)
        {
            if (sender != null)
            {
                var I = (IEnumerable)(sender);
                SelectedItems = new ObservableCollection<Object>();
                Rows.Children.Clear();
                int count = 0;
                foreach (Object item in I)
                {
                    var tmp = Support.RenderDataGridRow.Render.GetControl(Type, Name + count, item);
                    if (tmp != null)
                    {
                        Panel pnl = new Panel() { Name = this.Name + count };
                        count++;
                        pnl.Children.Add(tmp);
                        pnl.DataContext = item;
                        Rows.Children.Add(pnl);
                    }
                }
            }
            else
            {
                if (Items != null)
                {
                    SelectedItems = new ObservableCollection<Object>();
                    Rows.Children.Clear();
                    int count = 0;
                    foreach (Object item in Items)
                    {
                        var tmp = Support.RenderDataGridRow.Render.GetControl(Type, Name + count, item);
                        if (tmp != null)
                        {
                            Panel pnl = new Panel() { Name = this.Name + count };
                            count++;
                            pnl.Children.Add(tmp);
                            pnl.DataContext = item;
                            Rows.Children.Add(pnl);
                        }
                    }
                }
            }
        }

        Control _firstControl = null;
        Control FirstControl
        {
            get
            {
                return _firstControl;
            }
            set
            {
                _firstControl = value;
                RenderSelectedControls();
            }
        }
        Control _lastControl = null;
        Control LastControl
        {
            get
            {
                return _lastControl;
            }
            set
            {
                _lastControl = value;
                RenderSelectedControls();
            }
        }
        bool IsMouseDown = false;
        bool IsXYinBounds(Point XY, Rect ctrl)
        {
            if (XY.X > ctrl.X && XY.X < ctrl.X + ctrl.Width)
            {
                if (XY.Y > ctrl.Y && XY.Y < ctrl.Y + ctrl.Height)
                {
                    return true;
                }
            }
            return false;
        }
        IEnumerable<Control> FindSelectedControls()
        {
            if (ChooseMode == Controls.DataGrid.ChooseMode.Line)
            {
                if (MultilineMode == Controls.DataGrid.MultilineMode.Single)
                {
                    foreach (var item in FindSelectedControls_Line_Single())
                    {
                        yield return item;
                    }
                }
                if (MultilineMode == Controls.DataGrid.MultilineMode.Multi)
                {
                    foreach (var item in FindSelectedControls_Line_Multi())
                    {
                        yield return item;
                    }
                }
            }
            if (ChooseMode == Controls.DataGrid.ChooseMode.Cell)
            {
                if (MultilineMode == Controls.DataGrid.MultilineMode.Single)
                {
                    foreach (var item in FindSelectedControls_Cell_Single())
                    {
                        yield return item;
                    }
                }
                if (MultilineMode == Controls.DataGrid.MultilineMode.Multi)
                {
                    foreach (var item in FindSelectedControls_Cell_Multi())
                    {
                        yield return item;
                    }
                }
            }
        }
        IEnumerable<Control> FindSelectedControls_Cell_Single()
        {
            if (FirstControl != null && LastControl != null)
            {
                var tp11 = System.Convert.ToInt32(FirstControl.Name.Replace(Name, "").Split('_')[0]);
                var tp12 = System.Convert.ToInt32(FirstControl.Name.Replace(Name, "").Split('_')[1]);
                var tp21 = System.Convert.ToInt32(LastControl.Name.Replace(Name, "").Split('_')[0]);
                var tp22 = System.Convert.ToInt32(LastControl.Name.Replace(Name, "").Split('_')[1]);

                var stack = (StackPanel)LastControl.Parent.Parent;
                foreach (Border it in stack.Children)
                {
                    var child = (Panel)it.Child;
                    var tpl1 = System.Convert.ToInt32(child.Name.Replace(Name, "").Split('_')[0]);
                    var tpl2 = System.Convert.ToInt32(child.Name.Replace(Name, "").Split('_')[1]);
                    if (tpl1 <= System.Math.Max(tp11, tp21) && tpl1 >= System.Math.Min(tp11, tp21))
                    {
                        if (tpl2 <= System.Math.Max(tp12, tp22) && tpl2 >= System.Math.Min(tp12, tp22))
                        {
                            yield return child;
                        }
                    }
                }
            }
        }
        IEnumerable<Control> FindSelectedControls_Line_Single()
        {
            if (FirstControl != null && LastControl != null)
            {
                var tp11 = System.Convert.ToInt32(FirstControl.Name.Replace(Name, "").Split('_')[0]);
                var tp12 = System.Convert.ToInt32(FirstControl.Name.Replace(Name, "").Split('_')[1]);
                var tp21 = System.Convert.ToInt32(LastControl.Name.Replace(Name, "").Split('_')[0]);
                var tp22 = System.Convert.ToInt32(LastControl.Name.Replace(Name, "").Split('_')[1]);

                var stack = (StackPanel)LastControl.Parent.Parent;
                foreach (Border it in stack.Children)
                {
                    var child = (Panel)it.Child;
                    var tpl1 = System.Convert.ToInt32(child.Name.Replace(Name, "").Split('_')[0]);
                    var tpl2 = System.Convert.ToInt32(child.Name.Replace(Name, "").Split('_')[1]);
                    if (tpl1 <= System.Math.Max(tp11, tp21) && tpl1 >= System.Math.Min(tp11, tp21))
                    {
                        yield return child;
                    }
                }
            }
        }
        IEnumerable<Control> FindSelectedControls_Cell_Multi()
        {
            if (FirstControl != null && LastControl != null)
            {
                var tp11 = System.Convert.ToInt32(FirstControl.Name.Replace(Name, "").Split('_')[0]);
                var tp12 = System.Convert.ToInt32(FirstControl.Name.Replace(Name, "").Split('_')[1]);
                var tp21 = System.Convert.ToInt32(LastControl.Name.Replace(Name, "").Split('_')[0]);
                var tp22 = System.Convert.ToInt32(LastControl.Name.Replace(Name, "").Split('_')[1]);

                var list = Rows.Children;
                foreach (Panel item in list)
                {
                    var stack = (StackPanel)item.Children[0];
                    foreach (Border it in stack.Children)
                    {
                        var child = (Panel)it.Child;
                        var tpl1 = System.Convert.ToInt32(child.Name.Replace(Name, "").Split('_')[0]);
                        var tpl2 = System.Convert.ToInt32(child.Name.Replace(Name, "").Split('_')[1]);
                        if (tpl1 <= System.Math.Max(tp11, tp21) && tpl1 >= System.Math.Min(tp11, tp21))
                        {
                            if (tpl2 <= System.Math.Max(tp12, tp22) && tpl2 >= System.Math.Min(tp12, tp22))
                            {
                                yield return child;
                            }
                        }
                    }
                }
            }
        }
        IEnumerable<Control> FindSelectedControls_Line_Multi()
        {
            if (FirstControl != null && LastControl != null)
            {
                var tp11 = System.Convert.ToInt32(FirstControl.Name.Replace(Name, "").Split('_')[0]);
                var tp12 = System.Convert.ToInt32(FirstControl.Name.Replace(Name, "").Split('_')[1]);
                var tp21 = System.Convert.ToInt32(LastControl.Name.Replace(Name, "").Split('_')[0]);
                var tp22 = System.Convert.ToInt32(LastControl.Name.Replace(Name, "").Split('_')[1]);

                var list = Rows.Children;
                foreach (Panel item in list)
                {
                    var stack = (StackPanel)item.Children[0];
                    foreach (Border it in stack.Children)
                    {
                        var child = (Panel)it.Child;
                        var tpl1 = System.Convert.ToInt32(child.Name.Replace(Name, "").Split('_')[0]);
                        var tpl2 = System.Convert.ToInt32(child.Name.Replace(Name, "").Split('_')[1]);
                        if (tpl1 <= System.Math.Max(tp11, tp21) && tpl1 >= System.Math.Min(tp11, tp21))
                        {
                            yield return child;
                        }
                    }
                }
            }
        }
        IEnumerable FindSelectedItems()
        {
            foreach (Panel item in FindSelectedControls())
            {
                yield return item.DataContext;
            }
        }

        void RenderSelectedControls()
        {
            ClearElseControls();
            if (ChooseMode == Controls.DataGrid.ChooseMode.Line)
            {
                RenderSelectedControls_Line();
            }
            if (ChooseMode == Controls.DataGrid.ChooseMode.Cell)
            {
                RenderSelectedControls_Cell();
            }
        }
        void RenderSelectedControls_Cell()
        {
            ClearElseControls();
            foreach (Panel item in FindSelectedControls())
            {
                if (ChooseColor != null)
                {
                    item.Background = ChooseColor;
                }
                else
                {
                    item.Background = new SolidColorBrush(Color.FromArgb(60, 120, 216, 250));
                }
            }
        }
        void RenderSelectedControls_Line()
        {
            ClearElseControls();
            foreach (Panel item in FindSelectedControls())
            {
                if (ChooseColor != null)
                {
                    item.Background = ChooseColor;
                }
                else
                {
                    item.Background = new SolidColorBrush(Color.FromArgb(60, 120, 216, 250));
                }
            }
        }

        void ClearElseControls()
        {
            var list = Rows.Children;
            foreach (Panel item in list)
            {
                var stack = (StackPanel)item.Children[0];
                foreach (Border it in stack.Children)
                {
                    var child = (Panel)it.Child;
                    foreach (Panel i in FindSelectedControls())
                    {
                        if (i.Name != child.Name)
                        {
                            child.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                        }
                    }
                }
            }
        }
        void ClearAllControls()
        {
            var list = Rows.Children;
            foreach (Panel item in list)
            {
                var stack = (StackPanel)item.Children[0];
                foreach (Border it in stack.Children)
                {
                    var child = (Panel)it.Child;
                    child.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                }
            }
        }

        Control FindPressedControl(PointerPoint mouse)
        {
            var list = Rows.Children;
            foreach (Panel item in list)
            {
                var stack = (StackPanel)item.Children[0];
                foreach (Border it in stack.Children)
                {
                    var child = (Panel)it.Child;
                    if (child.TransformedBounds != null)
                    {
                        var pnt = child.TransformedBounds.Value.Bounds.TransformToAABB(child.TransformedBounds.Value.Transform);
                        var mpnt = mouse.Position.Transform(this.TransformedBounds.Value.Transform);
                        if (IsXYinBounds(mpnt, pnt))
                        {
                            return child;
                        }
                    }
                }
            }
            return null;
        }
        public void PanelPointerDown(object sender, PointerPressedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((DataGrid)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                IsMouseDown = true;
                ClearAllControls();

                LastControl = FindPressedControl(mouse);
                FirstControl = FindPressedControl(mouse);
            }
        }
        public void PanelPointerMoved(object sender, PointerEventArgs args)
        {
            var mouse = args.GetCurrentPoint((DataGrid)sender);
            if (IsMouseDown)
            {
                if (LastControl != null)
                {
                    if (IsXYinBounds(mouse.Position, LastControl.Bounds))
                    {
                        return;
                    }
                }
                LastControl = FindPressedControl(mouse);
            }
        }
        public void PanelPointerUp(object sender, PointerReleasedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((DataGrid)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                IsMouseDown = false;
                SelectedItems = new ObservableCollection<object>();
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
        void Init()
        {

            Border brd = new Border();
            brd.BorderThickness = Thickness.Parse("1");
            brd.BorderBrush = new SolidColorBrush(Color.Parse("Gray"));

            ScrollViewer vwm = new ScrollViewer();
            //vw.SetValue(Grid.RowProperty, 1);
            vwm.Background = new SolidColorBrush(Color.Parse("WhiteSmoke"));
            vwm.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            vwm.HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            vwm.VerticalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled;
            vwm.HorizontalScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility.Auto;
            brd.Child = vwm;

            Panel p = new Panel();
            p.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            vwm.Content = p;

            Grid grd = new Grid();
            RowDefinition rd = new RowDefinition();
            rd.Height = GridLength.Parse("30");
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

            StackPanel stck = new StackPanel();
            stck.Margin = Thickness.Parse("0,-1,0,0");
            stck.Spacing = -1;
            stck.Orientation = Avalonia.Layout.Orientation.Vertical;
            stck.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
            stck.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
            vw.Content = stck;
            Rows = stck;

            this.Content = brd;
        }
    }
}
