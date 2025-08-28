using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using Models.Forms.Form1;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_14VM : BaseFormVM
{
    public override string FormType => "1.4";

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    //public ObservableCollection<Form14> Form14List => new(FormList.Cast<Form14>());
    
    //public ObservableCollection<Form14> SelectedForms14 => new(SelectedForms.Cast<Form14>());
    
    //public Form14 SelectedForm14
    //{
    //    get => SelectedForm as Form14;
    //    set
    //    {
    //        SelectedForm = value;
    //        UpdateFormList();
    //    }
    //}

    #region Constructors

    public Form_14VM() { }
    
    public Form_14VM(Report report) : base(report) { }

    #endregion

    /*
    #region UpdateFormList
    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        //OnPropertyChanged(nameof(Form14List));
        //OnPropertyChanged(nameof(SelectedForms14));
        //OnPropertyChanged(nameof(SelectedForm14));
    }

    #endregion
    */
}