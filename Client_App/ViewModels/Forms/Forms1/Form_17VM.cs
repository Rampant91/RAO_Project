using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using Models.Forms.Form1;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_17VM : BaseFormVM
{
    public override string FormType => "1.7";

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    //public ObservableCollection<Form17> Form17List => new(FormList.Cast<Form17>());
    
    //public ObservableCollection<Form17> SelectedForms17 => new(SelectedForms.Cast<Form17>());
    
    //public Form17 SelectedForm17
    //{
    //    get => SelectedForm as Form17;
    //    set
    //    {
    //        SelectedForm = value;
    //        UpdateFormList();
    //    }
    //}

    #region Constructors

    public Form_17VM() { }
    
    public Form_17VM(Report report) : base(report) { }

    #endregion

    /*
    #region UpdateFormList
    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        //OnPropertyChanged(nameof(Form17List));
        //OnPropertyChanged(nameof(SelectedForms17));
        //OnPropertyChanged(nameof(SelectedForm17));
    }

    #endregion
    */
}