using Client_App.ViewModels.Controls;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_19VM : BaseFormVM
{
    public override string FormType => "1.9";

    #region OpCodes
    
    public ObservableCollection<OperationCodeItem> OperationCodes => 
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => ValidOperationCodes.Contains(x.Code)));
    
    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm19();

    #endregion

    #region DocVids
    
    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<DocumentVidItem> DocumentVids =>
        new(DocumentVidProvider.AllDocumentVids
            .Where(x => ValidDocumentVids.Contains(x.Code.ToString())));

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> ValidDocumentVids => DocumentVidProvider.GetValidCodesForForms19();

    #endregion

    #region CodeTypeAccObjects
    
    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<CodeTypeAccObjectItem> CodeTypeAccObjects =>
        new(CodeTypeAccObjectProvider.AllCodeTypeAccObjects
            .Where(x => ValidCodeTypeAccObjects.Contains(x.Code.ToString())));

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> ValidCodeTypeAccObjects => CodeTypeAccObjectProvider.GetValidCodes(); 
    
    #endregion

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

        InitializeUserControls();
        Reports = reps;

        SelectReportPopupVM = new SelectReportPopupVM(this);
    }

    #endregion
}