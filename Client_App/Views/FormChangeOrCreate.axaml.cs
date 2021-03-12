using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Models.Client_Model;
using Models.Attributes;
using Avalonia.Data;

namespace Client_App.Views
{
    public class FormChangeOrCreate : Window
    {
        public Form _Form { get; set; }
        public FormChangeOrCreate(Form frm)
        {
            _Form = frm;
            this.DataContext = _Form;
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            Init();
        }

        public FormChangeOrCreate()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        void Init()
        {
            var panel=this.FindControl<Panel>("ChangingPanel");
            var props = _Form.GetType().GetProperties();

            var Row = 0;
            foreach(var item in props)
            {
                var attrs = item.GetCustomAttributes(false);
                if (attrs.Length > 0)
                {
                    var attr = (FormVisualAttribute)attrs[0];

                    Grid grd = new Grid();
                    grd.ColumnDefinitions.Add(new ColumnDefinition());
                    grd.ColumnDefinitions.Add(new ColumnDefinition());
                    grd.Margin = Thickness.Parse("0," + ((Row * (30 + 25))) + ",0,0");

                    TextBlock blck = new TextBlock();
                    blck.Text = attr.Name;
                    blck.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
                    blck.Width = 300;
                    blck.SetValue(Grid.ColumnProperty, 0);
                    grd.Children.Add(blck);

                    TextBox txt = new TextBox();
                    Binding bnd = new Binding(item.Name);
                    txt.Bind(TextBox.TextProperty, bnd);
                    txt.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
                    //txt.Width = 300;
                    txt.SetValue(Grid.ColumnProperty,1);
                    
                    grd.Children.Add(txt);

                    panel.Children.Add(grd);

                    Row++;
                }
            }
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
