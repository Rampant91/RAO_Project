using Client_App.Commands.AsyncCommands;
using Client_App.ViewModels.Controls;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers;
using CommunityToolkit.Mvvm.Input;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_16VM : BaseFormVM
{
    #region Properties
    
    public override string FormType => "1.6";

    public ObservableCollection<OperationCodeItem> OperationCodes =>
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => ValidOperationCodes.Contains(x.Code)));
    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm16();

    public string OperationCodePattern => @"^\d{0,2}$";

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<DocumentVidItem> DocumentVids =>
        new(DocumentVidProvider.AllDocumentVids
            .Where(x => ValidDocumentVids.Contains(x.Code.ToString())));

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string?> ValidDocumentVids => DocumentVidProvider.GetValidCodesForForms11To16();

    public string DocumentVidPattern => "^([1-9]|1[0-5]|19)$";

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

    public string RefineOrSortRAOCodePattern => @"^(?:\d{0,2}|-)$";

    #endregion

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

        base.InitializeUserControls();
        Reports = reps;

        SelectReportPopupVM = new SelectReportPopupVM(this);
    }

    #endregion

    #region Commands
    public ICommand PasteRows => new NewPasteRowsAsyncCommand(this.Report.Rows16);
    #endregion
}