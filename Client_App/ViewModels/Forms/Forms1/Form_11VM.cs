using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Calculator;
using Client_App.Commands.AsyncCommands.ExcelExport;
using Client_App.Commands.AsyncCommands.Passports;
using Client_App.Commands.AsyncCommands.SourceTransmission;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers;
using Models.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_11VM : BaseFormVM
{
    #region Properties

    public override string FormType => "1.1";

    #region CategoryCodes

    /// <summary>
    /// Справочник категорий опасности для AutoCompleteBox с описаниями (только допустимые для формы)
    /// </summary>
    public ObservableCollection<CategoryItem> CategoryCodes =>
        new(CategoryProvider.AllCategories
            .Where(x => ValidCategories.Contains(x.Code)));

    /// <summary>
    /// Список допустимых категорий для валидации (только коды без описаний)
    /// </summary>
    public ICollection<short?> ValidCategories => CategoryProvider.GetValidCategoriesForForm11();

    #endregion

    #region DocumentVids

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

    #endregion 

    #region OpCodes

    /// <summary>
    /// Справочник кодов операции для AutoCompleteBox с описаниями (только допустимые для формы)
    /// </summary>
    public ObservableCollection<OperationCodeItem> OperationCodes =>
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => ValidOperationCodes.Contains(x.Code)));

    /// <summary>
    /// Список допустимых кодов операции для валидации (только коды без описаний)
    /// </summary>
    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm11();

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
    
    #endregion

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

        InitializeUserControls();
        Reports = reps;

    }

    #endregion

    #region Commands

    public ICommand CategoryCalculationFromReport => new CategoryCalculationFromReportAsyncCommand();
    public ICommand CopyPasName => new CopyPasNameAsyncCommand();
    public ICommand ExcelExportSourceMovementHistory => new ExcelExportSourceMovementHistoryAsyncCommand();
    public ICommand OpenPas => new OpenPasAsyncCommand();
    public ICommand SourceTransmission => new NewSourceTransmissionAsyncCommand(this);

    #endregion
}