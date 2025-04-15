using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Client_App.ViewModels.Calculator;

public class ActivityCalculatorVM : INotifyPropertyChanged
{
    #region Properties

    private string _selectedNuclid;
    public string SelectedNuclid
    {
        get => _selectedNuclid;
        set
        {
            if (_selectedNuclid == value) return;
            _selectedNuclid = value;
            OnPropertyChanged();
        }
    }

    private string _stringSearch;
    public string StringSearch
    {
        get => _stringSearch;
        set
        {
            _stringSearch = value;
            OnPropertyChanged();
            SearchCommand?.Execute(null);
        }
    }

    private ObservableCollection<Radionuclid>? _radionuclids;
    public ObservableCollection<Radionuclid>? Radionuclids
    {
        get => _radionuclids;
        set
        {
            if (_radionuclids != value && value != null)
            {
                _radionuclids = value;
            }
            OnPropertyChanged();
        }
    }

    #endregion

    #region Constructor

    public ActivityCalculatorVM()
    {
        //SearchCommand = new SearchReportsAsyncCommand(Navigator, this);
    }

    #endregion

    public ICommand SearchCommand { get; set; }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}

public class Radionuclid
{
    public string Name { get; set; }

    public string _Name
    {
        get
        {
            return Name;
        }
        //set
        //{
        //    _Name = value;
        //    OnPropertyChanged(nameof(RegNoRep));
        //}
    }

    public string Abbreviation;
}

//public class LostFocusUpdateBindingBehavior : Behavior<TextBox>
//{
//    static LostFocusUpdateBindingBehavior()
//    {
//        TextProperty.Changed.Subscribe(e =>
//        {
//            ((LostFocusUpdateBindingBehavior)e.Sender).OnBindingValueChanged();
//        });
//    }

//    public static readonly StyledProperty<string> TextProperty 
//        = AvaloniaProperty.Register<LostFocusUpdateBindingBehavior, string>("Text", defaultBindingMode: BindingMode.TwoWay);

//    public string Text
//    {
//        get => GetValue(TextProperty);
//        set => SetValue(TextProperty, value);
//    }

//    protected override void OnAttached()
//    {
//        AssociatedObject.LostFocus += OnLostFocus;
//        base.OnAttached();
//    }

//    protected override void OnDetaching()
//    {
//        AssociatedObject.LostFocus -= OnLostFocus;
//        base.OnDetaching();
//    }

//    private void OnLostFocus(object? sender, RoutedEventArgs e)
//    {
//        if (AssociatedObject != null)
//            Text = AssociatedObject.Text;
//    }

//    private void OnBindingValueChanged()
//    {
//        if (AssociatedObject != null)
//            AssociatedObject.Text = Text;
//    }
//}