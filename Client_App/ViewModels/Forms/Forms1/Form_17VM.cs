using Avalonia.Threading;
using Client_App.Commands.AsyncCommands;
using Client_App.Commands.AsyncCommands.Generate;
using Client_App.ViewModels.Controls;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Items;
using Client_App.ViewModels.Forms.Forms1.Providers;
using Client_App.ViewModels.Forms.Forms1.Providers.AutoCompleteProviders;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace Client_App.ViewModels.Forms.Forms1;

public class Form_17VM : BaseFormVM
{
    #region Properties

    private PackagePassportPanelControlVM _packagePassportPanelControlVM = new();
    public PackagePassportPanelControlVM PackagePassportPanelControlVM
    {
        get
        {
            return _packagePassportPanelControlVM;
        }
        set
        {
            _packagePassportPanelControlVM = value;
        }
    }

    public override string FormType => "1.7";

    public override ObservableCollection<Form> SelectedForms
    {
        get => base.SelectedForms;
        set
        {
            base.SelectedForms = value;
            var form17 = value.OrderBy(f17 => f17.NumberInOrder_DB).FirstOrDefault() as Form17;
            PackagePassportPanelControlVM.SelectPassport(form17.PassportNumber_DB, form17.PackType_DB);
        }
    }

    #region UktProvider

    private UktProvider _uktProvider = new UktProvider();
    public UktProvider UktProvider 
    {
        get
        {
            return _uktProvider;
        }
    } 

    public ObservableCollection<UktItem> UktItems { 
        get
        {
            return _uktProvider.TypedItemsCollection;
        }
    }
    #endregion

    #region OpCodes

    public ObservableCollection<OperationCodeItem> OperationCodes =>
        new(OperationCodesProvider.AllOperationCodes
            .Where(x => ValidOperationCodes.Contains(x.Code)));

    public ICollection<string> ValidOperationCodes => OperationCodesProvider.GetValidCodesForForm17();

    public string OperationCodePattern => @"^(?:\d{0,2}|-)$";

    #endregion

    #region DocVids

    /// <summary>
    /// 
    /// </summary>
    public ObservableCollection<DocumentVidItem> DocumentVids =>
        new(DocumentVidProvider.AllDocumentVids
            .Where(x => ValidDocumentVids.Contains(x.DisplayCode)));

    /// <summary>
    /// 
    /// </summary>
    public ICollection<string?> ValidDocumentVids => DocumentVidProvider.GetValidCodesForForms17To18();

    public string DocumentVidPattern => "^(-|[1-9]|1[0-5]|19)?$";

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
    public ICollection<string?> ValidRefineOrSortRAOCodes => RefineOrSortRAOCodeProvider.GetValidCodes();

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

    #region Functions
    public void UpdatePassportSelection()
    {
        Form17 form17 = null;

        var formsList = Report.Rows17.ToList();
        var index = formsList.IndexOf(SelectedForms.MinBy(f17 => f17.NumberInOrder_DB));

        for (int i = index; i >= 0; i--)
        {
            if (!string.IsNullOrWhiteSpace(formsList[i].OperationCode_DB)
                && formsList[i].OperationCode_DB is not "-")
            {
                form17 = formsList[i];
                break;
            }
        }

        PackagePassportPanelControlVM.SelectPassport(form17.PassportNumber_DB, form17.PackType_DB);
    }
    #endregion

    #region Commands
    public ICommand GenerateForm17 => new GenerateForm17AsyncCommand(this);
    public ICommand GeneratePackagePassport => new GeneratePackagePassportAsyncCommand(this);
    public ICommand PasteRows => new NewPasteRowsAsyncCommand(this.Report.Rows17);
    #endregion

    #region Events
    protected override void SelectedForms_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)

    {
        base.SelectedForms_CollectionChanged(sender, e);


        Dispatcher.UIThread.InvokeAsync(() =>
        {
            List<Form17> selectedForms17 = SelectedForms.Cast<Form17>().ToList();
            if (selectedForms17 is null || selectedForms17.Count <= 0) return;

            var x = selectedForms17.Select(f17 => f17.OperationCode_DB).ToList();
            Form17 form17 = selectedForms17.FirstOrDefault(f17 =>
                !string.IsNullOrWhiteSpace(f17.OperationCode_DB)
                && f17.OperationCode_DB is not "-");

            if (form17 is null)
            {
                var formsList = Report.Rows17.ToList();
                var index = formsList.IndexOf(selectedForms17.MinBy(f17 => f17.NumberInOrder_DB));

                for (int i = index; i >= 0; i--)
                {
                    if (!string.IsNullOrWhiteSpace(formsList[i].OperationCode_DB)
                        && formsList[i].OperationCode_DB is not "-")
                    {
                        form17 = formsList[i];
                        break;
                    }
                }
            }
            
            PackagePassportPanelControlVM.SelectPassport(form17?.PassportNumber_DB, form17?.PackType_DB);
        });
    }

    #endregion


    #region Constructors

    public Form_17VM() : base()
    {
        //SubscribeSelectedForms(SelectedForms);
    }

    public Form_17VM(Report report) : base(report)
    {
        //SubscribeSelectedForms(SelectedForms);
    }

    public Form_17VM(in Reports reps)
    {
        SelectedForms.CollectionChanged += SelectedForms_CollectionChanged;
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

    #region Deconstructor

    //~Form_17VM()
    //{
    //    UnsubscribeSelectedForms(SelectedForms);
    //}
    #endregion
}