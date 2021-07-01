using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Collections;

namespace Client_App.Controls.DataGrid
{
    public class Cell : UserControl
    {

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

        public event PropertyChangedEventHandler PropertyChanged;

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var t = (TextBox) ((Panel) ((Border) Content).Child).Children[0];
            t.IsEnabled = !IsReadOnly;
        }

        public void PanelPointerDown(object sender, PointerPressedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell) sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed) OnPropertyChanged(this,"Down");
        }

        public void PanelPointerMoved(object sender, PointerEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell) sender);
            if (mouse.Properties.IsLeftButtonPressed)
                OnPropertyChanged(this,"DownMove");
        }

        public void PanelPointerUp(object sender, PointerReleasedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell) sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased) OnPropertyChanged(this,"Up");
        }

        //Property Changed
        protected void OnPropertyChanged(object obj,[CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(obj, new PropertyChangedEventArgs(prop));
        }
        //Property Changed
    }
}