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
    public class RowNumber : ReactiveWindow<ViewModels.ChangeOrCreateVM>,INotifyPropertyChanged
    {
        public static readonly DirectProperty<RowNumber, string> NumberProperty =
                     AvaloniaProperty.RegisterDirect<RowNumber, string>(
                nameof(Number),
                o => o.Number,
                (o, v) => o.Number = v);

        private string _Number = "1";
        public string Number
        {
            get => _Number;
            set
            {
                try
                {
                    var t = System.Convert.ToInt32(value);
                    if (t > 0 &&t<=10000&& t.ToString() != Number)
                    {
                        SetAndRaise(NumberProperty, ref _Number, value);
                    }
                    else
                    {
                        OnPropertyChanged(nameof(Number));
                    }

                }
                catch
                {

                }
            }
        }
        public RowNumber()
        {
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
                _Number = "0";
            }
            base.OnClosing(e);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
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
