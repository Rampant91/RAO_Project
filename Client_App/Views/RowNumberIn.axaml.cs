using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.Interactivity;
using Models.Collections;
using System.ComponentModel;
using System.Linq;
using Models.DBRealization;
using Models;
using ReactiveUI;
using System.Collections.Generic;
using Models.DataAccess;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using OfficeOpenXml;

namespace Client_App.Views
{
    public class RowNumberIn : ReactiveWindow<ViewModels.ChangeOrCreateVM>,INotifyPropertyChanged
    {
        public static readonly DirectProperty<RowNumberIn, string> NumberProperty1 =
                     AvaloniaProperty.RegisterDirect<RowNumberIn, string>(
                nameof(Number1),
                o => o.Number1,
                (o, v) => o.Number1 = v);

        private string _Number1 = "0";
        public string Number1
        {
            get => _Number1;
            set
            {
                try
                {
                    var t = System.Convert.ToInt32(value);
                    if (t > 0 &&t<=10000&& t.ToString() != Number1)
                    {
                        SetAndRaise(NumberProperty1, ref _Number1, value);
                    }
                    else
                    {
                        OnPropertyChanged(nameof(Number1));
                    }

                }
                catch
                {

                }
            }
        }

        public static readonly DirectProperty<RowNumberIn, string> NumberProperty2 =
             AvaloniaProperty.RegisterDirect<RowNumberIn, string>(
        nameof(Number2),
        o => o.Number2,
        (o, v) => o.Number2 = v);

        private string _Number2 = "0";
        public string Number2
        {
            get => _Number2;
            set
            {
                try
                {
                    var t = System.Convert.ToInt32(value);
                    if (t > 0 && t <= 10000 && t.ToString() != Number2)
                    {
                        SetAndRaise(NumberProperty1, ref _Number2, value);
                    }
                    else
                    {
                        OnPropertyChanged(nameof(Number2));
                    }

                }
                catch
                {

                }
            }
        }
        public RowNumberIn()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        public RowNumberIn(int num)
        {
            Number1 = num.ToString();
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        bool flag = false;
        protected override void OnClosing(CancelEventArgs e)
        {
            if(!flag)
            {
                _Number2 = "0";
            }
            base.OnClosing(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var item = this.Get<TextBox>("MainTextBox");
            item.SelectAll();
            item.Focus();
        }

        private void OnButtonClick(object sender, RoutedEventArgs e)
        {
            flag = true;
            this.Close();
        }
        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
