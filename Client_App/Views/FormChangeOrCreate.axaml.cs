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
        public FormChangeOrCreate(Models.LocalStorage.LocalStorage _Storage)
        {
            ((ViewModels.ChangeOrCreateVM)this.DataContext).Storage= _Storage;
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
            Init();
        }

        void Init()
        {
            var panel=this.FindControl<Panel>("ChangingPanel");

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
