using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Models.Collections;
using Models.DBRealization;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App;

public class AskViacMessage : Window, INotifyPropertyChanged
{
    private ObservableCollection<string> _viacList;
    public ObservableCollection<string> ViacList
    {
        get
        {
            return _viacList;
        }
        set
        {
            _viacList = value;
            OnPropertyChanged();
        }
    }

    private string _selectedViac;
    public string SelectedViac
    {
        get
        {
            return _selectedViac;
        }
        set
        {
            _selectedViac = value;
            OnPropertyChanged();
        }
    }
    public AskViacMessage()
    {
        _viacList = new ObservableCollection<string>();
        AvaloniaXamlLoader.Load(this);
        DataContext = this;

    }
    public AskViacMessage(ObservableCollection<string> viacList)
    {
        _viacList = viacList;
        AvaloniaXamlLoader.Load(this);
        DataContext = this;

    }
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        this.Cursor = new Cursor(StandardCursorType.Wait);
        string? result = SelectedViac;

        this.Cursor = new Cursor(StandardCursorType.Arrow);
        Close(result);
    }

    private void Cancel_Click(object sender, RoutedEventArgs e)
    {
        // Return a cancellation indicator (could use null or sentinel value)
        Close(null);
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}