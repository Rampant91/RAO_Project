using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Collections;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_13VM : BaseFormVM
{
    public override string FormType => "1.3";

    #region Constructors

    public Form_13VM() { }

    public Form_13VM(Report report) : base(report) { }

    #endregion

    #region Commands

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #endregion

    //public ObservableCollection<Form13> Form13List => new(FormList.Cast<Form13>());

    //public ObservableCollection<Form13> SelectedForms13 => new(SelectedForms.Cast<Form13>());

    //public Form13 SelectedForm13
    //{
    //    get => SelectedForm as Form13;
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
        
        //OnPropertyChanged(nameof(Form13List));
        //OnPropertyChanged(nameof(SelectedForms13));
        //OnPropertyChanged(nameof(SelectedForm13));
    }

    #endregion
    */
}