using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Collections;

namespace Client_App.Controls.DataGrid
{
    public class Cell : UserControl, IChanged
    {
        private bool _isChanged;

        public Cell(object DataContext, string BindingPath, bool IsReadOnly)
        {
            this.DataContext = DataContext;
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            InitializeComponent();

            AddHandler(PointerPressedEvent, PanelPointerDown, handledEventsToo: true);
            AddHandler(PointerMovedEvent, PanelPointerMoved, handledEventsToo: true);
            AddHandler(PointerReleasedEvent, PanelPointerUp, handledEventsToo: true);
        }

        public Cell(string BindingPath, bool IsReadOnly)
        {
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            InitializeComponent();

            AddHandler(PointerPressedEvent, PanelPointerDown, handledEventsToo: true);
            AddHandler(PointerMovedEvent, PanelPointerMoved, handledEventsToo: true);
            AddHandler(PointerReleasedEvent, PanelPointerUp, handledEventsToo: true);
        }

        public Cell()
        {
            InitializeComponent();
        }

        public string BindingPath { get; set; } = "";

        public bool IsReadOnly { get; set; }

        public int CellRow { get; set; } = -1;

        public int CellColumn { get; set; } = -1;

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                _isChanged = value;
                OnPropertyChanged(nameof(IsChanged));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var t = (TextBox) ((Panel) ((Border) Content).Child).Children[0];
            t.IsEnabled = !IsReadOnly;
            //if (BindingPath != "")
            //{
            //    t.Bind(TextBox.DataContextProperty, new Binding("$parent[2].DataContext."+BindingPath));
            //}
        }

        public void PanelPointerDown(object sender, PointerPressedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell) sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed) OnPropertyChanged("Down");
        }

        public void PanelPointerMoved(object sender, PointerEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell) sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
                OnPropertyChanged("DownMove");
        }

        public void PanelPointerUp(object sender, PointerReleasedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell) sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased) OnPropertyChanged("Up");
        }

        //Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (prop != nameof(IsChanged))
            {
                IsChanged = true;
                if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        //Property Changed
    }
}