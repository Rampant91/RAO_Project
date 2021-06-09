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

namespace Client_App.Controls.DataGrid
{
    public partial class Cell : UserControl
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

        int _cellID = -1;
        public int CellID
        {
            get { return _cellID; }
            set { _cellID = value; }
        }
        public Cell(object DataContext, string BindingPath, bool IsReadOnly)
        {
            this.DataContext = DataContext;
            this.BindingPath = BindingPath;
            this.IsEnabled = !IsReadOnly;
            InitializeComponent();
        }
        public Cell()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var t = (TextBox)this.Content;
            t.IsReadOnly = IsReadOnly;
            t.Bind(TextBox.DataContextProperty, new Binding(BindingPath));
        }
    }
}
