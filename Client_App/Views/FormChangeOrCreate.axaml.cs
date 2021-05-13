using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Collections;

namespace Client_App.Views
{
    public class FormChangeOrCreate : Window
    {
        string _param = "";
        public FormChangeOrCreate(string param, string DBPath, Report rep)
        {
            var tmp = new ViewModels.ChangeOrCreateVM();
            if (DBPath != null)
            {
                tmp.DBPath = DBPath;
                tmp.Storage = rep;
            }
            else
            {
                tmp.Storage = rep;
            }

            this.DataContext = tmp;
            _param = param;

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

        void Form1Init(in Panel panel)
        {
            if (_param == "10")
                panel.Children.Add(Long_Visual.Form1_Visual.Form10_Visual());
            if (_param == "11")
                panel.Children.Add(Long_Visual.Form1_Visual.Form11_Visual());
            if (_param == "12")
                panel.Children.Add(Long_Visual.Form1_Visual.Form12_Visual());
            if (_param == "13")
                panel.Children.Add(Long_Visual.Form1_Visual.Form13_Visual());
            if (_param == "14")
                panel.Children.Add(Long_Visual.Form1_Visual.Form14_Visual());
            if (_param == "15")
                panel.Children.Add(Long_Visual.Form1_Visual.Form15_Visual());
            if (_param == "16")
                panel.Children.Add(Long_Visual.Form1_Visual.Form16_Visual());
            if (_param == "17")
                panel.Children.Add(Long_Visual.Form1_Visual.Form17_Visual());
            if (_param == "18")
                panel.Children.Add(Long_Visual.Form1_Visual.Form18_Visual());
            if (_param == "19")
                panel.Children.Add(Long_Visual.Form1_Visual.Form19_Visual());
        }

        void Init()
        {
            var panel = this.FindControl<Panel>("ChangingPanel");
            Form1Init(panel);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
