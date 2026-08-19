using Client_App.Services.DataAccess;
using Models.DBRealization;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.ViewModels.Controls;
using CommunityToolkit.Mvvm.Input;
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

    public string OperationCodePattern => @"^\d{0,2}$";

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
    public ICollection<string?> ValidDocumentVids => DocumentVidProvider.GetValidCodesForForms11To16();

    public string DocumentVidPattern => "^([1-9]|1[0-5]|19)$";

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

    public string RefineOrSortRAOCodePattern => @"^(?:\d{0,2}|-)$";

    #endregion 

    #endregion

    #region FrozenColumnCount

    private int _frozenColumnCount = 0;

    public int FrozenColumnCount
    {
        get => _frozenColumnCount;
        set
        {
            var clamped = Math.Clamp(value, 0, 3);
            if (_frozenColumnCount == clamped) return;
            _frozenColumnCount = clamped;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsFrozenHeaderVisible));
            OnPropertyChanged(nameof(IsZeroFrozenMode));
            OnPropertyChanged(nameof(IsFrozenCol1Visible));
            OnPropertyChanged(nameof(IsFrozenCol1OnlyVisible));
            OnPropertyChanged(nameof(IsFrozenCol2Visible));
            OnPropertyChanged(nameof(CanDecreaseFrozen));
            OnPropertyChanged(nameof(CanIncreaseFrozen));
            OnPropertyChanged(nameof(IsScrollableGroupHeaderFull));
        }
    }

    public bool IsZeroFrozenMode => FrozenColumnCount == 0;
    public bool IsFrozenHeaderVisible => FrozenColumnCount > 0;
    public bool IsFrozenCol1Visible => FrozenColumnCount >= 2;
    public bool IsFrozenCol1OnlyVisible => FrozenColumnCount == 2;
    public bool IsFrozenCol2Visible => FrozenColumnCount >= 3;
    public bool IsScrollableGroupHeaderFull => FrozenColumnCount < 2;
    public bool CanDecreaseFrozen => FrozenColumnCount > 0;
    public bool CanIncreaseFrozen => FrozenColumnCount < 3;

    private ICommand? _decreaseFrozenCommand;
    public ICommand DecreaseFrozenColumnCountCommand =>
        _decreaseFrozenCommand ??= new RelayCommand(() => FrozenColumnCount--);

    private ICommand? _increaseFrozenCommand;
    public ICommand IncreaseFrozenColumnCountCommand =>
        _increaseFrozenCommand ??= new RelayCommand(() => FrozenColumnCount++);

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
                Value = OrgReportsQuery.GetLatestEndPeriod(
                    StaticConfiguration.DBModel, reps.Id, formNum)
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