using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class AskIntMessageVM : BaseVM, INotifyPropertyChanged
{

    #region Properties

    private string _text = "";
    public string Text
    {
        get => _text;
        set
        {
            _text = value;

            OnPropertyChanged();
        }
    }
    private int? _result;
    public int? Result
    {
        get => _result;
        set
        {
            _result = value;

            OnPropertyChanged();
        }
    }
    #endregion

    #region Constructor

    public AskIntMessageVM(string text) { Text = text; }
    public AskIntMessageVM() { Text = "Введите целое число"; }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}