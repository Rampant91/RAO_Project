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

    //public ObservableCollection<Form12> Form12List => new(FormList.Cast<Form12>());
    
    //public ObservableCollection<Form12> SelectedForms12 => new(SelectedForms.Cast<Form12>());
    
    //public Form12 SelectedForm12
    //{
    //    get => SelectedForm as Form12;
    //    set
    //    {
    //        SelectedForm = value;
    //        UpdateFormList();
    //    }
    //}

    #region Constructors

    public Form_12VM() { }
    
    public Form_12VM(Report report) : base(report) { }

    #endregion

    #region RowCount

    public new int RowCount
    {
        get => base.RowCount;
        set
        {
            var oldValue = base.RowCount;
            base.RowCount = value;
            if (oldValue != value)
            {
                UpdateFormList();
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
                UpdateFormList();
            }
        }
    }

    #endregion

    #region UpdateFormList

    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        //OnPropertyChanged(nameof(Form12List));
        //OnPropertyChanged(nameof(SelectedForms12));
        //OnPropertyChanged(nameof(SelectedForm12));
    }

    #endregion
}