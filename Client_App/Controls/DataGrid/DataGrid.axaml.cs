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

namespace Client_App.Controls.DataGrid
{

    public class DataGrid : UserControl
    {
        public static readonly DirectProperty<DataGrid, IEnumerable> ItemsProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, IEnumerable>(
                nameof(Items),
                o => o.Items,
                (o, v) => o.Items = v);

        private IEnumerable _items;

        public IEnumerable Items
        {
            get { return _items; }
            set 
            { 
                SetAndRaise(ItemsProperty, ref _items, value);
                Update();
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
            set { SetAndRaise(TypeProperty, ref _type, value); }
        }

        public static new readonly DirectProperty<DataGrid, string> NameProperty =
            AvaloniaProperty.RegisterDirect<DataGrid, string>(
        nameof(Name),
        o => o.Name,
        (o, v) => o.Name = v);
        private string _Name = "";
        public new string Name
        {
            get { return _Name; }
            set { SetAndRaise(NameProperty, ref _Name, value); }
        }

        public Panel Columns { get; set; }
        public StackPanel Rows { get; set; }

        public DataGrid()
        {
            this.DataContext = new Support.DataGrid_DataContext(this);
            InitializeComponent();
            Columns =this.FindControl<Panel>("Columns");
            Rows=this.FindControl<StackPanel>("Rows");

            ItemsProperty.Changed.Subscribe(new ItemsObserver(Rows, ItemsChanged));
            MakeHeader();

        }

        public void Update()
        {
            ItemsChanged("ALL", null);
            MakeHeader();
        }
        
        void ItemsChanged(object sender, PropertyChangedEventArgs args)
        {
            if(Items!=null)
            {
                if (sender != null)
                {
                    if(sender is string)
                    {
                        if((string)sender=="ALL")
                        {
                            Rows.Children.Clear();
                            foreach(var item in Items)
                            {
                                var it = (Report)item;
                                if(it.ReportId!=null)
                                {
                                    var tmp = Support.RenderDataGridRow.Render.GetControl(Type,Name+it.ReportId, CellPressed,CellReleased,CellMoved);
                                    if(tmp!=null)
                                    {
                                        Panel pnl = new Panel() { Name = this.Name + it.ReportId };
                                        pnl.Children.Add(tmp);
                                        pnl.DataContext = item;
                                        Rows.Children.Add(pnl);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        bool First = false;
        int[] _first = new int[2];
        object fsender=null;

        void CellMoved(object sender, PointerEventArgs args)
        {
            if (args.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                if (fsender != sender)
                {
                    fsender = sender;
                    var n = ((Control)sender).Name;
                    n = n.Replace(this.Name, "");
                    var row = System.Convert.ToInt32(n.Split('_')[0]);
                    var column = System.Convert.ToInt32(n.Split('_')[1]);

                    var prt = (StackPanel)((Control)sender).Parent.Parent.Parent.Parent;

                    var lt = prt.Children;

                    var t1 = -1;
                    var t2 = -1;
                    var t3 = -1;
                    var t4 = -1;
                    if (_first[0] >= row)
                    {
                        t1 = _first[0];
                        t2 = row;
                    }
                    else
                    {
                        t1 = row;
                        t2 = _first[0];
                    }
                    if (_first[1] >= column)
                    {
                        t3 = _first[1];
                        t4 = column;
                    }
                    else
                    {
                        t3 = column;
                        t4 = _first[1];
                    }

                    foreach (var item in lt)
                    {
                        var ty = (StackPanel)((Panel)item).Children[0];
                        var tu = ty.Children;
                        foreach (Border it in tu)
                        {
                            var nam = ((Button)it.Child).Name;
                            if (nam.Contains(this.Name))
                            {
                                var nm = nam;
                                nm = nm.Replace(this.Name, "");
                                var rw = System.Convert.ToInt32(nm.Split('_')[0]);
                                var clmn = System.Convert.ToInt32(nm.Split('_')[1]);


                                if (rw >= t2 && rw <= t1)
                                {
                                    if (clmn >= t4 && clmn <= t3)
                                    {
                                        ((Button)it.Child).Background = new SolidColorBrush(Color.FromArgb(150, 114, 121, 158));
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void CellPressed(object sender,PointerPressedEventArgs args)
        {
            if(args.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                First = true;
                var nm = ((Control)sender).Name;
                nm = nm.Replace(this.Name, "");
                var row = nm.Split('_')[0];
                var column = nm.Split('_')[1];
                _first[0] = System.Convert.ToInt32(row);
                _first[1] = System.Convert.ToInt32(column);
            }
        }
        void CellReleased(object sender, PointerReleasedEventArgs args)
        {
            if (First)
            {
                First = false;

                _first = new int[2];
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
        }
    }
}
