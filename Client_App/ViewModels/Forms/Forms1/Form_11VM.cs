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
using CommunityToolkit.Mvvm.Input;
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

    public string CategoryCodePattern => "^[1-5]$";

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

    public string DocumentVidPattern => "^([1-9]|1[0-5]|19)$";

    #endregion 

    #region OpCodes

    /// <summary>
    /// Справочник кодов операции для AutoCompleteBox с описаниями (только допустимые для формы)
    /// </summary>
    public ObservableCollection<OperationCodeItem> OperationCodes =>
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => ValidOperationCodes.Contains(x.Code)));

    public ObservableCollection<KeyValuePair<string, string>> OKSM =>
        new(Spravochniki.Spravochniks.OKSM.AsEnumerable());

    /// <summary>
    /// Список допустимых кодов операции для валидации (только коды без описаний)
    /// </summary>
    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm11();

    public string OperationCodePattern => @"^\d{0,2}$";

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

    public string OwnershipCodePattern => "^[1-6,9]$";

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

    /// <summary>Режим без замороженных колонок: вся шапка скроллируется, включая "№ п/п".</summary>
    public bool IsZeroFrozenMode => FrozenColumnCount == 0;

    /// <summary>
    /// Показывать ли фиксированную секцию шапки.
    /// При FrozenColumnCount=0 секция скрыта; скроллируемая шапка выравнивается по
    /// DataGrid col 1 через отрицательный сдвиг в FrozenHeaderScrollSyncBehavior.
    /// </summary>
    public bool IsFrozenHeaderVisible => FrozenColumnCount > 0;

    /// <summary>Колонка 1 ("код") попала в фиксированную область (FrozenColumnCount >= 2).</summary>
    public bool IsFrozenCol1Visible => FrozenColumnCount >= 2;

    /// <summary>Только колонка 1 заморожена, колонка 2 — нет (FrozenColumnCount == 2).
    /// Используется для группового заголовка с ColumnSpan=1.</summary>
    public bool IsFrozenCol1OnlyVisible => FrozenColumnCount == 2;

    /// <summary>Колонка 2 ("дата") попала в фиксированную область (FrozenColumnCount >= 3).</summary>
    public bool IsFrozenCol2Visible => FrozenColumnCount >= 3;

    /// <summary>
    /// Группа "Сведения об операции" полностью в скроллируемой области (FrozenCount == 1).
    /// При FrozenCount>=2 col 0 ("код") уходит в фиксированную область,
    /// и spanning-border заменяется на одиночный border для col 1 без левой границы.
    /// </summary>
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
    public ICommand SourceTransmission => new SourceTransmissionAsyncCommand(this);

    #endregion
}