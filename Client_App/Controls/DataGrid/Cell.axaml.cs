using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.Controls.DataGrid
{
    public partial class Cell : UserControl, IChanged
    {
        private string _BindingPath = "";
        public string BindingPath
        {
            get => _BindingPath;
            set => _BindingPath = value;
        }

        private bool _IsReadOnly = false;
        public bool IsReadOnly
        {
            get => _IsReadOnly;
            set => _IsReadOnly = value;
        }

        private int _cellRow = -1;
        public int CellRow
        {
            get => _cellRow;
            set => _cellRow = value;
        }

        private int _cellColumn = -1;
        public int CellColumn
        {
            get => _cellColumn;
            set => _cellColumn = value;
        }
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

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            TextBox? t = (TextBox)((Panel)((Border)Content).Child).Children[0];
            t.IsEnabled = !IsReadOnly;
            //if (BindingPath != "")
            //{
            //    t.Bind(TextBox.DataContextProperty, new Binding("$parent[2].DataContext."+BindingPath));
            //}
        }

        public void PanelPointerDown(object sender, PointerPressedEventArgs args)
        {
            PointerPoint? mouse = args.GetCurrentPoint((Cell)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                OnPropertyChanged("Down");
            }
        }
        public void PanelPointerMoved(object sender, PointerEventArgs args)
        {
            PointerPoint? mouse = args.GetCurrentPoint((Cell)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                OnPropertyChanged("DownMove");
            }
        }
        public void PanelPointerUp(object sender, PointerReleasedEventArgs args)
        {
            PointerPoint? mouse = args.GetCurrentPoint((Cell)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                OnPropertyChanged("Up");
            }
        }

        private bool _isChanged = false;
        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                _isChanged = value;
                OnPropertyChanged(nameof(IsChanged));
            }
        }
        //Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (prop != nameof(IsChanged))
            {
                IsChanged = true;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed

    }
}
