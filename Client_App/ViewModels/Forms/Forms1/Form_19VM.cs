using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Linq;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_19VM : BaseFormVM
{
    public override string FormType => "1.9";

    #region Constructors

    public Form_19VM() { }

    public Form_19VM(Report report) : base(report) { }

    public Form_19VM(in Reports reps)
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

        SelectReportPopupVM = new SelectReportPopupVM(this);
    }

    #endregion

    //public ObservableCollection<Form19> Form19List => new(FormList.Cast<Form19>());

    //public ObservableCollection<Form19> SelectedForms19 => new(SelectedForms.Cast<Form19>());

    //public Form19 SelectedForm19
    //{
    //    get => SelectedForm as Form19;
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
        
        //OnPropertyChanged(nameof(Form19List));
        //OnPropertyChanged(nameof(SelectedForms19));
        //OnPropertyChanged(nameof(SelectedForm19));
    }

    #endregion
    */
}