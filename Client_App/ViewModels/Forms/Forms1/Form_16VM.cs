using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using Models.Forms.Form1;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_16VM : BaseFormVM
{
    public override string FormType => "1.6";

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    //public ObservableCollection<Form16> Form16List => new(FormList.Cast<Form16>());
    
    //public ObservableCollection<Form16> SelectedForms16 => new(SelectedForms.Cast<Form16>());
    
    //public Form16 SelectedForm16
    //{
    //    get => SelectedForm as Form16;
    //    set
    //    {
    //        SelectedForm = value;
    //        UpdateFormList();
    //    }
    //}

    #region Constructors

    public Form_16VM() { }
    
    public Form_16VM(Report report) : base(report) { }

    #endregion

    /*
    #region UpdateFormList
    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        //OnPropertyChanged(nameof(Form16List));
        //OnPropertyChanged(nameof(SelectedForms16));
        //OnPropertyChanged(nameof(SelectedForm16));
    }

    #endregion
    */
}