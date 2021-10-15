using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.VisualTree;

namespace Client_App.Controls.DataGrid
{
    public class CustomCell : Cell
    {


        public CustomCell(Control ctrl,object DataContext, string BindingPath, bool IsReadOnly)
        {
            this.DataContext = DataContext;
            this.BindingPath = BindingPath;
            this.IsReadOnly = IsReadOnly;
            this.Control = ctrl;

            InitializeComponent();

            Focusable = false;
        }

        public CustomCell()
        {
            InitializeComponent();
        }

        public Control Control { get; set; }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var t = ((Panel) ((Border) Content).Child);
            t.Children.Add(Control);
            
        }
    }
}