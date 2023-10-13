using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Client_App.ViewModels;

namespace Client_App.Views;

public partial class RowNumber : BaseWindow<ChangeOrCreateVM>, INotifyPropertyChanged
{
    #region Number
    public static readonly DirectProperty<RowNumber, string> NumberProperty =
        AvaloniaProperty.RegisterDirect<RowNumber, string>(
            nameof(Number),
            o => o.Number,
            (o, v) => o.Number = v);

    private string _Number = "0";
    public string Number
    {
        get => _Number;
        set
        {
            try
            {
                var t = System.Convert.ToInt32(value);
                if (t is > 0 and <= 10000 && t.ToString() != Number)
                {
                    SetAndRaise(NumberProperty, ref _Number, value);
                }
                else
                {
                    OnPropertyChanged();
                }
            }
            catch
            {

            }
        }
    }
    #endregion

    public RowNumber()
    {
        InitializeComponent();
        var item = this.Get<TextBox>("MainTextBox");
        item.SelectAll();
        item.AttachedToVisualTree += (s, e) => item.Focus();
    }

    bool flag;
    protected override void OnClosing(WindowClosingEventArgs e)
    {
        if(!flag)
        {
            _Number = "0";
        }
        base.OnClosing(e);
    }

    private void OnButtonClick(object sender, RoutedEventArgs e)
    {
        flag = true;
        Close();
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