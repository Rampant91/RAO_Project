using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_15VM : BaseFormVM
{
    public override string FormType => "1.5";

    #region Constructors

    public Form_15VM() { }

    public Form_15VM(Report report) : base(report) { }

    public Form_15VM(in Reports reps)
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

        SelectReportPopupVM = new SelectReportPopupVM(this);
    }

    #endregion

    #region Commands

    public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
    
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