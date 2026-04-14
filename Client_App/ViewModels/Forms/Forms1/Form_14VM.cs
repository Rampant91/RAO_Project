using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.Commands.SyncCommands;
using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_14VM : BaseFormVM
{
    public override string FormType => "1.4";

    public ObservableCollection<OperationCodeItem> OperationCodes => 
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => OperationCodesProvider.GetValidCodesForForm14().Contains(x.Code)));
    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm14();

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<DocumentVidItem> DocumentVids =>
        new(DocumentVidProvider.AllDocumentVids);

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> ValidDocumentVids => DocumentVidProvider.GetValidCodesForForm11().ToList();

    #region Constructors

    public Form_14VM() { }

    public Form_14VM(Report report) : base(report) { }

    public Form_14VM(in Reports reps)
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
