using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_12VM : BaseFormVM
{
    public override string FormType => "1.2";

    #region Constructors

    public Form_12VM() { }

    public Form_12VM(Report report) : base(report) { }

    public Form_12VM(in Reports reps)
    {
        var formNum = FormType;
        Report = new Report
        {
            FormNum_DB = formNum,
            StartPeriod =
            {
                Value = reps.Report_Collection
                    .Where(x => x.FormNum_DB == formNum && DateOnly.TryParse(x.EndPeriod_DB, out _))
                    .OrderBy(x => DateOnly.Parse(x.EndPeriod_DB))
                    .Select(x => x.EndPeriod_DB)
                    .LastOrDefault() ?? ""
            },
            Reports = reps

        };

        base.InitializeUserControls();
        Reports = reps;
    }

    #endregion

    #region Commands

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this); 
    
    #endregion

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

    /*
    #region UpdateFormList
    public new async void UpdateFormList()
    {
        base.UpdateFormList();
        
        //OnPropertyChanged(nameof(Form12List));
        //OnPropertyChanged(nameof(SelectedForms12));
        //OnPropertyChanged(nameof(SelectedForm12));
    }

    #endregion
    */
}