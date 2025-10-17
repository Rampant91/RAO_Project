using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_11VM : BaseFormVM
{
    public override string FormType => "1.1";

    #region Constructors

    public Form_11VM() { }

    public Form_11VM(Report report) : base(report) { }

    public Form_11VM(in Reports reps)
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

    public ICommand CategoryCalculationFromReport => new CategoryCalculationFromReportAsyncCommand();
    public ICommand CopyPasName => new CopyPasNameAsyncCommand();
    public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
    public ICommand OpenPas => new OpenPasAsyncCommand();
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

    //#region FilterProperty



    //#endregion

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