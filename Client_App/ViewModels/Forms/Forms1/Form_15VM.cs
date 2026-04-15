using Client_App.Commands.AsyncCommands.ExcelExport;
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

public class Form_15VM : BaseFormVM
{
    #region Properties
    
    public override string FormType => "1.5";

    #region OpCodes

    public ObservableCollection<OperationCodeItem> OperationCodes =>
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => ValidOperationCodes.Contains(x.Code)));

    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm15();

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
    public ICollection<string> ValidDocumentVids => DocumentVidProvider.GetValidCodesForForms11To18();

    #endregion

    #region RefineOrSortRAOCodes

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<RefineOrSortRAOCodeItem> RefineOrSortRAOCodes =>
        new(RefineOrSortRAOCodeProvider.AllRefineOrSortRAOCodes
            .Where(x => ValidRefineOrSortRAOCodes.Contains(x.Code)));

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> ValidRefineOrSortRAOCodes => RefineOrSortRAOCodeProvider.GetValidCodes();

    #endregion 

    #endregion

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

        InitializeUserControls();
        Reports = reps;

        SelectReportPopupVM = new SelectReportPopupVM(this);
    }

    #endregion

    #region Commands

    public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
    
    #endregion
}