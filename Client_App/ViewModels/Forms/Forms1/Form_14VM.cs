using Client_App.Commands.AsyncCommands.SourceTransmission;
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
    #region Properties
    
    public override string FormType => "1.4";

    #region OpCodes

    public ObservableCollection<OperationCodeItem> OperationCodes =>
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => ValidOperationCodes.Contains(x.Code)));

    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm14();

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

    #region OwnershipForms

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<OwnershipItem> OwnershipForms =>
        new(OwnershipProvider.AllOwnershipForms
            .Where(x => ValidOwnershipForms.Contains(x.Code.ToString())));

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> ValidOwnershipForms => OwnershipProvider.GetValidCodes();

    #endregion

    #region AggregateStates

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<AggregateStateItem> AggregateStates =>
        new(AggregateStateProvider.AllAggregateStates
            .Where(x => ValidAggregateStates.Contains(x.Code.ToString())));

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string> ValidAggregateStates => AggregateStateProvider.GetValidCodes();

    #endregion 

    #endregion

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

        InitializeUserControls();
        Reports = reps;

        SelectReportPopupVM = new SelectReportPopupVM(this);
    }

    #endregion

    #region Commands

    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #endregion
}