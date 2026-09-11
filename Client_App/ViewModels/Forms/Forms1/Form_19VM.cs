using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
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

public class Form_19VM : BaseFormVM
{
    public override string FormType => "1.9";

    #region OpCodes
    
    public ObservableCollection<OperationCodeItem> OperationCodes => 
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => ValidOperationCodes.Contains(x.Code)));
    
    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm19();

    public string OperationCodePattern => "^1?$|^10$";

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
    public ICollection<string?> ValidDocumentVids => DocumentVidProvider.GetValidCodesForForms19();

    public string DocumentVidPattern => "^1?$";

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

    public string CodeTypeAccObjectsPattern => @"^\d{0,2}$";

    #endregion


    #region Commands
    public ICommand PasteRows => new NewPasteRowsAsyncCommand(this.Report.Rows19);

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