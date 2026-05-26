using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class AskListOfOrgsPeriodMessageVM : INotifyPropertyChanged
{
    #region Properties

    private string _form1StartDate = string.Empty;
    public string Form1StartDate
    {
        get => _form1StartDate;
        set { _form1StartDate = value ?? string.Empty; OnPropertyChanged(); }
    }

    private string _form1EndDate = string.Empty;
    public string Form1EndDate
    {
        get => _form1EndDate;
        set { _form1EndDate = value ?? string.Empty; OnPropertyChanged(); }
    }

    private string _form2StartYear = string.Empty;
    public string Form2StartYear
    {
        get => _form2StartYear;
        set { _form2StartYear = value ?? string.Empty; OnPropertyChanged(); }
    }

    private string _form2EndYear = string.Empty;
    public string Form2EndYear
    {
        get => _form2EndYear;
        set { _form2EndYear = value ?? string.Empty; OnPropertyChanged(); }
    }

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}
