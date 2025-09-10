using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_15VM : BaseFormVM
{
    public override string FormType => "1.5";

    #region Constructors

    public Form_15VM() { }

    public Form_15VM(Report report) : base(report) { }

    #endregion

    #region Commands

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this); 
    
    #endregion

    //public ObservableCollection<Form15> Form15List => new(FormList.Cast<Form15>());

    //public ObservableCollection<Form15> SelectedForms15 => new(SelectedForms.Cast<Form15>());

    //public Form15 SelectedForm15
    //{
    //    get => SelectedForm as Form15;
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
        
        //OnPropertyChanged(nameof(Form15List));
        //OnPropertyChanged(nameof(SelectedForms15));
        //OnPropertyChanged(nameof(SelectedForm15));
    }

    #endregion
    */
}