using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.SyncCommands;
using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_13VM : BaseFormVM
{
    public override string FormType => "1.3";

    #region Constructors

    public Form_13VM() { }

    public Form_13VM(Report report) : base(report) { }

    public Form_13VM(in Reports reps)
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