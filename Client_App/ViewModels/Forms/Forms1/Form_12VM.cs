using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using Models.Forms.Form1;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_12VM : BaseFormVM
{
    public override string FormType => "1.2";

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #region Form12-specific properties for Compile Bindings

    private ObservableCollection<Form12> _form12List = new();
    public ObservableCollection<Form12> Form12List
    {
        get => _form12List;
        set
        {
            _form12List = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<Form12> _selectedForms12 = new();
    public ObservableCollection<Form12> SelectedForms12
    {
        get => _selectedForms12;
        set
        {
            _selectedForms12 = value;
            OnPropertyChanged();
        }
    }

    private Form12 _selectedForm12;
    public Form12 SelectedForm12
    {
        get => _selectedForm12;
        set
        {
            _selectedForm12 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Constructors

    public Form_12VM() 
    {
        _form12List = new ObservableCollection<Form12>();
        _selectedForms12 = new ObservableCollection<Form12>();
    }
    
    public Form_12VM(Report report) : base(report) 
    {
        _form12List = new ObservableCollection<Form12>();
        _selectedForms12 = new ObservableCollection<Form12>();
        UpdateFormList();
    }

    #endregion

    #region Override pagination properties to ensure our UpdateFormList is called

    public new int RowCount
    {
        get => base.RowCount;
        set
        {
            var oldValue = base.RowCount;
            base.RowCount = value;
            if (oldValue != value)
            {
                UpdateFormList(); // This will call our overridden method
            }
        }
    }

    public new int CurrentPage
    {
        get => base.CurrentPage;
        set
        {
            var oldValue = base.CurrentPage;
            base.CurrentPage = value;
            if (oldValue != value)
            {
                UpdateFormList(); // This will call our overridden method
            }
        }
    }

    #endregion

    #region Override UpdateFormList to notify Form12-specific properties

    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        // Update the typed Form12List with current FormList data
        _form12List.Clear();
        foreach (var form in FormList.Cast<Form12>())
        {
            _form12List.Add(form);
        }
        
        // Notify that Form12List has changed
        OnPropertyChanged(nameof(Form12List));
        
        // Update selected forms
        _selectedForms12.Clear();
        foreach (var form in SelectedForms.Cast<Form12>())
        {
            _selectedForms12.Add(form);
        }
        
        OnPropertyChanged(nameof(SelectedForm12));
        OnPropertyChanged(nameof(SelectedForms12));
    }

    #endregion
}