using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_18VM : BaseFormVM
{
    public override string FormType => "1.8";

    public ObservableCollection<OperationCodeItem> OperationCodes => 
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => OperationCodesProvider.GetValidCodesForForm18().Contains(x.Code)));
    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm18();

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<DocumentVidItem> DocumentVids =>
        new(DocumentVidProvider.AllDocumentVids);

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> ValidDocumentVids => DocumentVidProvider.GetValidCodes().ToList();

    #region Constructors

    public Form_18VM() { }

    public Form_18VM(Report report) : base(report) { }

    public Form_18VM(in Reports reps)
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

    //public ObservableCollection<Form18> Form18List => new(FormList.Cast<Form18>());

    //public ObservableCollection<Form18> SelectedForms18 => new(SelectedForms.Cast<Form18>());

    //public Form18 SelectedForm18
    //{
    //    get => SelectedForm as Form18;
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
        
        //OnPropertyChanged(nameof(Form18List));
        //OnPropertyChanged(nameof(SelectedForms18));
        //OnPropertyChanged(nameof(SelectedForm18));
    }

    #endregion
    */
}
