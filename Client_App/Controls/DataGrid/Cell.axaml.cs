using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Data;
using Avalonia.Media;
using Models.Attributes;
using Avalonia.Input;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Interactivity;
using Models.Collections;
using Models.DataAccess;
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Client_App.Controls.DataGrid
{
    public partial class Cell : UserControl,IChanged
    {
        string _BindingPath = "";
        public string BindingPath
        {
            get { return _BindingPath; }
            set { _BindingPath = value; }
        }

        bool _IsReadOnly = false;
        public bool IsReadOnly
        {
            get { return _IsReadOnly; }
            set { _IsReadOnly = value; }
        }

        int _cellRow = -1;
        public int CellRow
        {
            get { return _cellRow; }
            set { _cellRow = value; }
        }

        int _cellColumn = -1;
        public int CellColumn
        {
            get { return _cellColumn; }
            set { _cellColumn = value; }
        }
        public Cell(object DataContext, string BindingPath, bool IsReadOnly)
        {
            this.DataContext = DataContext;
            this.BindingPath = BindingPath;
            InitializeComponent(true);

            this.AddHandler(PointerPressedEvent, PanelPointerDown, handledEventsToo: true);
            this.AddHandler(PointerMovedEvent, PanelPointerMoved, handledEventsToo: true);
            this.AddHandler(PointerReleasedEvent, PanelPointerUp, handledEventsToo: true);
        }
        public Cell()
        {
            InitializeComponent(false);
        }

        private void InitializeComponent(bool IsReadOnly)
        {
            AvaloniaXamlLoader.Load(this);
            if (BindingPath != "")
            {
                var t = (TextBox)((Panel)((Border)this.Content).Child).Children[0];
                t.IsEnabled = IsReadOnly;
                t.Bind(TextBox.DataContextProperty, new Binding("$parent[2].DataContext."+BindingPath));
            }
        }

        public void PanelPointerDown(object sender, PointerPressedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonPressed)
            {
                OnPropertyChanged("Down");
            }
        }
        public void PanelPointerMoved(object sender, PointerEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                OnPropertyChanged("DownMove");
            }
        }
        public void PanelPointerUp(object sender, PointerReleasedEventArgs args)
        {
            var mouse = args.GetCurrentPoint((Cell)sender);
            if (mouse.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
            {
                OnPropertyChanged("Up");
            }
        }

        bool _isChanged = false;
        public bool IsChanged 
        {
            get
            {
                return _isChanged;
            }
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
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed

    }
}
