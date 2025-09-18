using Client_App.Commands.AsyncCommands.SourceTransmission;
using Models.Attributes;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_16VM : BaseFormVM
{
    public override string FormType => "1.6";

    #region Constructors

    public Form_16VM() { }

    public Form_16VM(Report report) : base(report) { }

    public Form_16VM(in Reports reps)
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