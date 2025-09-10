using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_18VM : BaseFormVM
{
    public override string FormType => "1.8";

    #region Constructors

    public Form_18VM() { }

    public Form_18VM(Report report) : base(report) { }

    #endregion

    #region Commands

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #endregion

    //public ObservableCollection<Form18> Form18List => new(FormList.Cast<Form18>());

    //public ObservableCollection<Form18> SelectedForms18 => new(SelectedForms.Cast<Form18>());

    //public Form18 SelectedForm18
    //{
    //    get => SelectedForm as Form18;
    //    set
    //    {
    //        SelectedForm = value;
    //        UpdateFormList();
    //    }
    //}

    /*
    #region UpdateFormList
    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        //OnPropertyChanged(nameof(Form18List));
        //OnPropertyChanged(nameof(SelectedForms18));
        //OnPropertyChanged(nameof(SelectedForm18));
    }

    #endregion
    */
}