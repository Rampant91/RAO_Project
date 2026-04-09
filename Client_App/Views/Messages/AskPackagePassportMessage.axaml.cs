using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using Avalonia.Input;
using Models.Passports;

namespace Client_App;

public partial class AskPackagePassportMessage : Window, INotifyPropertyChanged
{
    private ObservableCollection<PackagePassport> _passportList;
    public ObservableCollection<PackagePassport> PassportList
    {
        get
        {
            return _passportList;
        }
        set
        {
            _passportList = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<PackagePassport>? _selectedPassportsCollection;
    public ObservableCollection<PackagePassport>? SelectedPassportsCollection
    {
        get
        {
            return _selectedPassportsCollection;
        }
        set
        {
            _selectedPassportsCollection = value;
            OnPropertyChanged();
        }
    }

    private Reports _reports;
    public Reports Reports
    {
        get
        {
            return _reports;
        }
        set
        {
            _reports = value;
            OnPropertyChanged();
        }
    }

    public AskPackagePassportMessage()
    {
        PassportList = new ObservableCollection<PackagePassport>(
            StaticConfiguration.DBModel.package_passport
            .Include(p => p.ContentCharacteristics)
            .ThenInclude(c=> c.RadionuclidsList)
            .AsEnumerable());
        SelectedPassportsCollection = new ObservableCollection<PackagePassport>();
        DataContext = this;

        AvaloniaXamlLoader.Load(this);
    }
    private void Accept_Click(object sender, RoutedEventArgs e)
    {
        this.Cursor = new Cursor(StandardCursorType.Wait);
        // Return the integer result from ViewModel

        this.Cursor = new Cursor(StandardCursorType.Arrow);
        Close(SelectedPassportsCollection);
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