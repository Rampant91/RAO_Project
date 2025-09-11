using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Attributes;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_17VM : BaseFormVM
{
    public override string FormType => "1.7";

    #region Constructors

    public Form_17VM() { }

    public Form_17VM(Report report) : base(report) { }

    public Form_17VM(in Reports reps)
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

        Reports = reps;
    }

    #endregion

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