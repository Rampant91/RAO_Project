using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

/// <summary>
/// Параметры сопоставления операций приёма-передачи (формы 1.1–1.6).
/// </summary>
public class GetTransferReceiveParamsVM : INotifyPropertyChanged
{
    public bool Ok;
    private bool _syncingAll;
    private bool _syncingAll12;
    private bool _syncingAll13;
    private bool _syncingAll14;
    private bool _syncingAll15;
    private bool _syncingAll16;

    #region Form 1.1

    private bool? _checkAll = true;
    public bool? CheckAll
    {
        get => _checkAll;
        set => SetAllValue(ref _checkAll, value, ref _syncingAll, ApplyAll);
    }

    private bool _checkOperationCode = true;
    public bool CheckOperationCode
    {
        get => _checkOperationCode;
        set => SetField(ref _checkOperationCode, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkOperationDate = true;
    public bool CheckOperationDate
    {
        get => _checkOperationDate;
        set => SetField(ref _checkOperationDate, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkPassportNumber = true;
    public bool CheckPassportNumber
    {
        get => _checkPassportNumber;
        set => SetField(ref _checkPassportNumber, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkType = true;
    public bool CheckType
    {
        get => _checkType;
        set => SetField(ref _checkType, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkRadionuclids = true;
    public bool CheckRadionuclids
    {
        get => _checkRadionuclids;
        set => SetField(ref _checkRadionuclids, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkFactoryNumber = true;
    public bool CheckFactoryNumber
    {
        get => _checkFactoryNumber;
        set => SetField(ref _checkFactoryNumber, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkQuantity = true;
    public bool CheckQuantity
    {
        get => _checkQuantity;
        set => SetField(ref _checkQuantity, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkActivity = true;
    public bool CheckActivity
    {
        get => _checkActivity;
        set => SetField(ref _checkActivity, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkCreatorOkpo = true;
    public bool CheckCreatorOkpo
    {
        get => _checkCreatorOkpo;
        set => SetField(ref _checkCreatorOkpo, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkCreationDate = true;
    public bool CheckCreationDate
    {
        get => _checkCreationDate;
        set => SetField(ref _checkCreationDate, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkProviderOrRecieverOkpo = true;
    public bool CheckProviderOrRecieverOkpo
    {
        get => _checkProviderOrRecieverOkpo;
        set => SetField(ref _checkProviderOrRecieverOkpo, value, ref _syncingAll, UpdateCheckAll);
    }

    private bool _checkPackNumber = true;
    public bool CheckPackNumber
    {
        get => _checkPackNumber;
        set => SetField(ref _checkPackNumber, value, ref _syncingAll, UpdateCheckAll);
    }

    #endregion

    #region Form 1.2

    private bool? _checkAll12 = true;
    public bool? CheckAll12
    {
        get => _checkAll12;
        set => SetAllValue(ref _checkAll12, value, ref _syncingAll12, ApplyAll12);
    }

    private bool _checkOperationCode12 = true;
    public bool CheckOperationCode12
    {
        get => _checkOperationCode12;
        set => SetField(ref _checkOperationCode12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkOperationDate12 = true;
    public bool CheckOperationDate12
    {
        get => _checkOperationDate12;
        set => SetField(ref _checkOperationDate12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkPassportNumber12 = true;
    public bool CheckPassportNumber12
    {
        get => _checkPassportNumber12;
        set => SetField(ref _checkPassportNumber12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkName12 = true;
    public bool CheckName12
    {
        get => _checkName12;
        set => SetField(ref _checkName12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkFactoryNumber12 = true;
    public bool CheckFactoryNumber12
    {
        get => _checkFactoryNumber12;
        set => SetField(ref _checkFactoryNumber12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkMass12 = true;
    public bool CheckMass12
    {
        get => _checkMass12;
        set => SetField(ref _checkMass12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkCreatorOkpo12 = true;
    public bool CheckCreatorOkpo12
    {
        get => _checkCreatorOkpo12;
        set => SetField(ref _checkCreatorOkpo12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkCreationDate12 = true;
    public bool CheckCreationDate12
    {
        get => _checkCreationDate12;
        set => SetField(ref _checkCreationDate12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkProviderOrRecieverOkpo12 = true;
    public bool CheckProviderOrRecieverOkpo12
    {
        get => _checkProviderOrRecieverOkpo12;
        set => SetField(ref _checkProviderOrRecieverOkpo12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkPackType12 = true;
    public bool CheckPackType12
    {
        get => _checkPackType12;
        set => SetField(ref _checkPackType12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    private bool _checkPackNumber12 = true;
    public bool CheckPackNumber12
    {
        get => _checkPackNumber12;
        set => SetField(ref _checkPackNumber12, value, ref _syncingAll12, UpdateCheckAll12);
    }

    #endregion

    #region Form 1.3

    private bool? _checkAll13 = true;
    public bool? CheckAll13
    {
        get => _checkAll13;
        set => SetAllValue(ref _checkAll13, value, ref _syncingAll13, ApplyAll13);
    }

    private bool _checkOperationCode13 = true;
    public bool CheckOperationCode13
    {
        get => _checkOperationCode13;
        set => SetField(ref _checkOperationCode13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkOperationDate13 = true;
    public bool CheckOperationDate13
    {
        get => _checkOperationDate13;
        set => SetField(ref _checkOperationDate13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkPassportNumber13 = true;
    public bool CheckPassportNumber13
    {
        get => _checkPassportNumber13;
        set => SetField(ref _checkPassportNumber13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkType13 = true;
    public bool CheckType13
    {
        get => _checkType13;
        set => SetField(ref _checkType13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkRadionuclids13 = true;
    public bool CheckRadionuclids13
    {
        get => _checkRadionuclids13;
        set => SetField(ref _checkRadionuclids13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkFactoryNumber13 = true;
    public bool CheckFactoryNumber13
    {
        get => _checkFactoryNumber13;
        set => SetField(ref _checkFactoryNumber13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkAggregateState13 = true;
    public bool CheckAggregateState13
    {
        get => _checkAggregateState13;
        set => SetField(ref _checkAggregateState13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkActivity13 = true;
    public bool CheckActivity13
    {
        get => _checkActivity13;
        set => SetField(ref _checkActivity13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkCreatorOkpo13 = true;
    public bool CheckCreatorOkpo13
    {
        get => _checkCreatorOkpo13;
        set => SetField(ref _checkCreatorOkpo13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkCreationDate13 = true;
    public bool CheckCreationDate13
    {
        get => _checkCreationDate13;
        set => SetField(ref _checkCreationDate13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkProviderOrRecieverOkpo13 = true;
    public bool CheckProviderOrRecieverOkpo13
    {
        get => _checkProviderOrRecieverOkpo13;
        set => SetField(ref _checkProviderOrRecieverOkpo13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    private bool _checkPackNumber13 = true;
    public bool CheckPackNumber13
    {
        get => _checkPackNumber13;
        set => SetField(ref _checkPackNumber13, value, ref _syncingAll13, UpdateCheckAll13);
    }

    #endregion

    #region Form 1.4

    private bool? _checkAll14 = true;
    public bool? CheckAll14
    {
        get => _checkAll14;
        set => SetAllValue(ref _checkAll14, value, ref _syncingAll14, ApplyAll14);
    }

    private bool _checkOperationCode14 = true;
    public bool CheckOperationCode14
    {
        get => _checkOperationCode14;
        set => SetField(ref _checkOperationCode14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkOperationDate14 = true;
    public bool CheckOperationDate14
    {
        get => _checkOperationDate14;
        set => SetField(ref _checkOperationDate14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkPassportNumber14 = true;
    public bool CheckPassportNumber14
    {
        get => _checkPassportNumber14;
        set => SetField(ref _checkPassportNumber14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkName14 = true;
    public bool CheckName14
    {
        get => _checkName14;
        set => SetField(ref _checkName14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkSort14 = true;
    public bool CheckSort14
    {
        get => _checkSort14;
        set => SetField(ref _checkSort14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkRadionuclids14 = true;
    public bool CheckRadionuclids14
    {
        get => _checkRadionuclids14;
        set => SetField(ref _checkRadionuclids14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkActivity14 = true;
    public bool CheckActivity14
    {
        get => _checkActivity14;
        set => SetField(ref _checkActivity14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkActivityMeasurementDate14 = true;
    public bool CheckActivityMeasurementDate14
    {
        get => _checkActivityMeasurementDate14;
        set => SetField(ref _checkActivityMeasurementDate14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkVolume14 = true;
    public bool CheckVolume14
    {
        get => _checkVolume14;
        set => SetField(ref _checkVolume14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkMass14 = true;
    public bool CheckMass14
    {
        get => _checkMass14;
        set => SetField(ref _checkMass14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkAggregateState14 = true;
    public bool CheckAggregateState14
    {
        get => _checkAggregateState14;
        set => SetField(ref _checkAggregateState14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkProviderOrRecieverOkpo14 = true;
    public bool CheckProviderOrRecieverOkpo14
    {
        get => _checkProviderOrRecieverOkpo14;
        set => SetField(ref _checkProviderOrRecieverOkpo14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    private bool _checkPackNumber14 = true;
    public bool CheckPackNumber14
    {
        get => _checkPackNumber14;
        set => SetField(ref _checkPackNumber14, value, ref _syncingAll14, UpdateCheckAll14);
    }

    #endregion

    #region Form 1.5

    private bool? _checkAll15 = true;
    public bool? CheckAll15
    {
        get => _checkAll15;
        set => SetAllValue(ref _checkAll15, value, ref _syncingAll15, ApplyAll15);
    }

    private bool _checkOperationCode15 = true;
    public bool CheckOperationCode15
    {
        get => _checkOperationCode15;
        set => SetField(ref _checkOperationCode15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkOperationDate15 = true;
    public bool CheckOperationDate15
    {
        get => _checkOperationDate15;
        set => SetField(ref _checkOperationDate15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkPassportNumber15 = true;
    public bool CheckPassportNumber15
    {
        get => _checkPassportNumber15;
        set => SetField(ref _checkPassportNumber15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkType15 = true;
    public bool CheckType15
    {
        get => _checkType15;
        set => SetField(ref _checkType15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkRadionuclids15 = true;
    public bool CheckRadionuclids15
    {
        get => _checkRadionuclids15;
        set => SetField(ref _checkRadionuclids15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkFactoryNumber15 = true;
    public bool CheckFactoryNumber15
    {
        get => _checkFactoryNumber15;
        set => SetField(ref _checkFactoryNumber15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkQuantity15 = true;
    public bool CheckQuantity15
    {
        get => _checkQuantity15;
        set => SetField(ref _checkQuantity15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkActivity15 = true;
    public bool CheckActivity15
    {
        get => _checkActivity15;
        set => SetField(ref _checkActivity15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkCreationDate15 = true;
    public bool CheckCreationDate15
    {
        get => _checkCreationDate15;
        set => SetField(ref _checkCreationDate15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkStatusRao15 = true;
    public bool CheckStatusRao15
    {
        get => _checkStatusRao15;
        set => SetField(ref _checkStatusRao15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkProviderOrRecieverOkpo15 = true;
    public bool CheckProviderOrRecieverOkpo15
    {
        get => _checkProviderOrRecieverOkpo15;
        set => SetField(ref _checkProviderOrRecieverOkpo15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkPackName15 = true;
    public bool CheckPackName15
    {
        get => _checkPackName15;
        set => SetField(ref _checkPackName15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkPackType15 = true;
    public bool CheckPackType15
    {
        get => _checkPackType15;
        set => SetField(ref _checkPackType15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkPackNumber15 = true;
    public bool CheckPackNumber15
    {
        get => _checkPackNumber15;
        set => SetField(ref _checkPackNumber15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkSubsidy15 = true;
    public bool CheckSubsidy15
    {
        get => _checkSubsidy15;
        set => SetField(ref _checkSubsidy15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    private bool _checkFcpNumber15 = true;
    public bool CheckFcpNumber15
    {
        get => _checkFcpNumber15;
        set => SetField(ref _checkFcpNumber15, value, ref _syncingAll15, UpdateCheckAll15);
    }

    #endregion

    #region Form 1.6

    private bool? _checkAll16 = true;
    public bool? CheckAll16
    {
        get => _checkAll16;
        set => SetAllValue(ref _checkAll16, value, ref _syncingAll16, ApplyAll16);
    }

    private bool _checkOperationCode16 = true;
    public bool CheckOperationCode16
    {
        get => _checkOperationCode16;
        set => SetField(ref _checkOperationCode16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkOperationDate16 = true;
    public bool CheckOperationDate16
    {
        get => _checkOperationDate16;
        set => SetField(ref _checkOperationDate16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkCodeRao16 = true;
    public bool CheckCodeRao16
    {
        get => _checkCodeRao16;
        set => SetField(ref _checkCodeRao16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkStatusRao16 = true;
    public bool CheckStatusRao16
    {
        get => _checkStatusRao16;
        set => SetField(ref _checkStatusRao16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkVolume16 = true;
    public bool CheckVolume16
    {
        get => _checkVolume16;
        set => SetField(ref _checkVolume16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkMass16 = true;
    public bool CheckMass16
    {
        get => _checkMass16;
        set => SetField(ref _checkMass16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkQuantity16 = true;
    public bool CheckQuantity16
    {
        get => _checkQuantity16;
        set => SetField(ref _checkQuantity16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkRadionuclids16 = true;
    public bool CheckRadionuclids16
    {
        get => _checkRadionuclids16;
        set => SetField(ref _checkRadionuclids16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkTritiumActivity16 = true;
    public bool CheckTritiumActivity16
    {
        get => _checkTritiumActivity16;
        set => SetField(ref _checkTritiumActivity16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkBetaGammaActivity16 = true;
    public bool CheckBetaGammaActivity16
    {
        get => _checkBetaGammaActivity16;
        set => SetField(ref _checkBetaGammaActivity16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkAlphaActivity16 = true;
    public bool CheckAlphaActivity16
    {
        get => _checkAlphaActivity16;
        set => SetField(ref _checkAlphaActivity16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkTransuraniumActivity16 = true;
    public bool CheckTransuraniumActivity16
    {
        get => _checkTransuraniumActivity16;
        set => SetField(ref _checkTransuraniumActivity16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkActivityMeasurementDate16 = true;
    public bool CheckActivityMeasurementDate16
    {
        get => _checkActivityMeasurementDate16;
        set => SetField(ref _checkActivityMeasurementDate16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkProviderOrRecieverOkpo16 = true;
    public bool CheckProviderOrRecieverOkpo16
    {
        get => _checkProviderOrRecieverOkpo16;
        set => SetField(ref _checkProviderOrRecieverOkpo16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkPackType16 = true;
    public bool CheckPackType16
    {
        get => _checkPackType16;
        set => SetField(ref _checkPackType16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkPackNumber16 = true;
    public bool CheckPackNumber16
    {
        get => _checkPackNumber16;
        set => SetField(ref _checkPackNumber16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkSubsidy16 = true;
    public bool CheckSubsidy16
    {
        get => _checkSubsidy16;
        set => SetField(ref _checkSubsidy16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    private bool _checkFcpNumber16 = true;
    public bool CheckFcpNumber16
    {
        get => _checkFcpNumber16;
        set => SetField(ref _checkFcpNumber16, value, ref _syncingAll16, UpdateCheckAll16);
    }

    #endregion

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField(ref bool field, bool value, ref bool syncFlag, Action updateAll, [CallerMemberName] string prop = "")
    {
        if (field == value) return;
        field = value;
        OnPropertyChanged(prop);
        if (!syncFlag)
        {
            updateAll();
        }
    }

    private void SetAllValue(ref bool? target, bool? value, ref bool syncFlag, Action<bool> applyAll, [CallerMemberName] string prop = "")
    {
        if (target == value) return;
        target = value;
        OnPropertyChanged(prop);
        if (!syncFlag && value is bool allChecked) applyAll(allChecked);
    }

    private void ApplyAll(bool allChecked)
    {
        _syncingAll = true;
        CheckOperationCode = allChecked;
        CheckOperationDate = allChecked;
        CheckPassportNumber = allChecked;
        CheckType = allChecked;
        CheckRadionuclids = allChecked;
        CheckFactoryNumber = allChecked;
        CheckQuantity = allChecked;
        CheckActivity = allChecked;
        CheckCreatorOkpo = allChecked;
        CheckCreationDate = allChecked;
        CheckProviderOrRecieverOkpo = allChecked;
        CheckPackNumber = allChecked;
        _syncingAll = false;
        UpdateCheckAll();
    }

    private void UpdateCheckAll()
    {
        var all = CheckOperationCode && CheckOperationDate && CheckPassportNumber && CheckType
                  && CheckRadionuclids && CheckFactoryNumber && CheckQuantity && CheckActivity
                  && CheckCreatorOkpo && CheckCreationDate && CheckProviderOrRecieverOkpo && CheckPackNumber;
        var none = !CheckOperationCode && !CheckOperationDate && !CheckPassportNumber && !CheckType
                   && !CheckRadionuclids && !CheckFactoryNumber && !CheckQuantity && !CheckActivity
                   && !CheckCreatorOkpo && !CheckCreationDate && !CheckProviderOrRecieverOkpo && !CheckPackNumber;

        _syncingAll = true;
        CheckAll = all ? true : none ? false : null;
        _syncingAll = false;
    }

    private void ApplyAll12(bool allChecked)
    {
        _syncingAll12 = true;
        CheckOperationCode12 = allChecked;
        CheckOperationDate12 = allChecked;
        CheckPassportNumber12 = allChecked;
        CheckName12 = allChecked;
        CheckFactoryNumber12 = allChecked;
        CheckMass12 = allChecked;
        CheckCreatorOkpo12 = allChecked;
        CheckCreationDate12 = allChecked;
        CheckProviderOrRecieverOkpo12 = allChecked;
        CheckPackType12 = allChecked;
        CheckPackNumber12 = allChecked;
        _syncingAll12 = false;
        UpdateCheckAll12();
    }

    private void UpdateCheckAll12()
    {
        var all = CheckOperationCode12 && CheckOperationDate12 && CheckPassportNumber12 && CheckName12
                  && CheckFactoryNumber12 && CheckMass12 && CheckCreatorOkpo12 && CheckCreationDate12
                  && CheckProviderOrRecieverOkpo12 && CheckPackType12 && CheckPackNumber12;
        var none = !CheckOperationCode12 && !CheckOperationDate12 && !CheckPassportNumber12 && !CheckName12
                   && !CheckFactoryNumber12 && !CheckMass12 && !CheckCreatorOkpo12 && !CheckCreationDate12
                   && !CheckProviderOrRecieverOkpo12 && !CheckPackType12 && !CheckPackNumber12;

        _syncingAll12 = true;
        CheckAll12 = all ? true : none ? false : null;
        _syncingAll12 = false;
    }

    private void ApplyAll13(bool allChecked)
    {
        _syncingAll13 = true;
        CheckOperationCode13 = allChecked;
        CheckOperationDate13 = allChecked;
        CheckPassportNumber13 = allChecked;
        CheckType13 = allChecked;
        CheckRadionuclids13 = allChecked;
        CheckFactoryNumber13 = allChecked;
        CheckAggregateState13 = allChecked;
        CheckActivity13 = allChecked;
        CheckCreatorOkpo13 = allChecked;
        CheckCreationDate13 = allChecked;
        CheckProviderOrRecieverOkpo13 = allChecked;
        CheckPackNumber13 = allChecked;
        _syncingAll13 = false;
        UpdateCheckAll13();
    }

    private void UpdateCheckAll13()
    {
        var all = CheckOperationCode13 && CheckOperationDate13 && CheckPassportNumber13 && CheckType13
                  && CheckRadionuclids13 && CheckFactoryNumber13 && CheckAggregateState13 && CheckActivity13
                  && CheckCreatorOkpo13 && CheckCreationDate13 && CheckProviderOrRecieverOkpo13 && CheckPackNumber13;
        var none = !CheckOperationCode13 && !CheckOperationDate13 && !CheckPassportNumber13 && !CheckType13
                   && !CheckRadionuclids13 && !CheckFactoryNumber13 && !CheckAggregateState13 && !CheckActivity13
                   && !CheckCreatorOkpo13 && !CheckCreationDate13 && !CheckProviderOrRecieverOkpo13 && !CheckPackNumber13;

        _syncingAll13 = true;
        CheckAll13 = all ? true : none ? false : null;
        _syncingAll13 = false;
    }

    private void ApplyAll14(bool allChecked)
    {
        _syncingAll14 = true;
        CheckOperationCode14 = allChecked;
        CheckOperationDate14 = allChecked;
        CheckPassportNumber14 = allChecked;
        CheckName14 = allChecked;
        CheckSort14 = allChecked;
        CheckRadionuclids14 = allChecked;
        CheckActivity14 = allChecked;
        CheckActivityMeasurementDate14 = allChecked;
        CheckVolume14 = allChecked;
        CheckMass14 = allChecked;
        CheckAggregateState14 = allChecked;
        CheckProviderOrRecieverOkpo14 = allChecked;
        CheckPackNumber14 = allChecked;
        _syncingAll14 = false;
        UpdateCheckAll14();
    }

    private void UpdateCheckAll14()
    {
        var all = CheckOperationCode14 && CheckOperationDate14 && CheckPassportNumber14 && CheckName14
                  && CheckSort14 && CheckRadionuclids14 && CheckActivity14 && CheckActivityMeasurementDate14
                  && CheckVolume14 && CheckMass14 && CheckAggregateState14
                  && CheckProviderOrRecieverOkpo14 && CheckPackNumber14;
        var none = !CheckOperationCode14 && !CheckOperationDate14 && !CheckPassportNumber14 && !CheckName14
                   && !CheckSort14 && !CheckRadionuclids14 && !CheckActivity14 && !CheckActivityMeasurementDate14
                   && !CheckVolume14 && !CheckMass14 && !CheckAggregateState14
                   && !CheckProviderOrRecieverOkpo14 && !CheckPackNumber14;

        _syncingAll14 = true;
        CheckAll14 = all ? true : none ? false : null;
        _syncingAll14 = false;
    }

    private void ApplyAll15(bool allChecked)
    {
        _syncingAll15 = true;
        CheckOperationCode15 = allChecked;
        CheckOperationDate15 = allChecked;
        CheckPassportNumber15 = allChecked;
        CheckType15 = allChecked;
        CheckRadionuclids15 = allChecked;
        CheckFactoryNumber15 = allChecked;
        CheckQuantity15 = allChecked;
        CheckActivity15 = allChecked;
        CheckCreationDate15 = allChecked;
        CheckStatusRao15 = allChecked;
        CheckProviderOrRecieverOkpo15 = allChecked;
        CheckPackName15 = allChecked;
        CheckPackType15 = allChecked;
        CheckPackNumber15 = allChecked;
        CheckSubsidy15 = allChecked;
        CheckFcpNumber15 = allChecked;
        _syncingAll15 = false;
        UpdateCheckAll15();
    }

    private void UpdateCheckAll15()
    {
        var all = CheckOperationCode15 && CheckOperationDate15 && CheckPassportNumber15 && CheckType15
                  && CheckRadionuclids15 && CheckFactoryNumber15 && CheckQuantity15 && CheckActivity15
                  && CheckCreationDate15 && CheckStatusRao15 && CheckProviderOrRecieverOkpo15
                  && CheckPackName15 && CheckPackType15 && CheckPackNumber15
                  && CheckSubsidy15 && CheckFcpNumber15;
        var none = !CheckOperationCode15 && !CheckOperationDate15 && !CheckPassportNumber15 && !CheckType15
                   && !CheckRadionuclids15 && !CheckFactoryNumber15 && !CheckQuantity15 && !CheckActivity15
                   && !CheckCreationDate15 && !CheckStatusRao15 && !CheckProviderOrRecieverOkpo15
                   && !CheckPackName15 && !CheckPackType15 && !CheckPackNumber15
                   && !CheckSubsidy15 && !CheckFcpNumber15;

        _syncingAll15 = true;
        CheckAll15 = all ? true : none ? false : null;
        _syncingAll15 = false;
    }

    private void ApplyAll16(bool allChecked)
    {
        _syncingAll16 = true;
        CheckOperationCode16 = allChecked;
        CheckOperationDate16 = allChecked;
        CheckCodeRao16 = allChecked;
        CheckStatusRao16 = allChecked;
        CheckVolume16 = allChecked;
        CheckMass16 = allChecked;
        CheckQuantity16 = allChecked;
        CheckRadionuclids16 = allChecked;
        CheckTritiumActivity16 = allChecked;
        CheckBetaGammaActivity16 = allChecked;
        CheckAlphaActivity16 = allChecked;
        CheckTransuraniumActivity16 = allChecked;
        CheckActivityMeasurementDate16 = allChecked;
        CheckProviderOrRecieverOkpo16 = allChecked;
        CheckPackType16 = allChecked;
        CheckPackNumber16 = allChecked;
        CheckSubsidy16 = allChecked;
        CheckFcpNumber16 = allChecked;
        _syncingAll16 = false;
        UpdateCheckAll16();
    }

    private void UpdateCheckAll16()
    {
        var all = CheckOperationCode16 && CheckOperationDate16 && CheckCodeRao16 && CheckStatusRao16
                  && CheckVolume16 && CheckMass16 && CheckQuantity16 && CheckRadionuclids16
                  && CheckTritiumActivity16 && CheckBetaGammaActivity16 && CheckAlphaActivity16
                  && CheckTransuraniumActivity16 && CheckActivityMeasurementDate16
                  && CheckProviderOrRecieverOkpo16 && CheckPackType16 && CheckPackNumber16
                  && CheckSubsidy16 && CheckFcpNumber16;
        var none = !CheckOperationCode16 && !CheckOperationDate16 && !CheckCodeRao16 && !CheckStatusRao16
                   && !CheckVolume16 && !CheckMass16 && !CheckQuantity16 && !CheckRadionuclids16
                   && !CheckTritiumActivity16 && !CheckBetaGammaActivity16 && !CheckAlphaActivity16
                   && !CheckTransuraniumActivity16 && !CheckActivityMeasurementDate16
                   && !CheckProviderOrRecieverOkpo16 && !CheckPackType16 && !CheckPackNumber16
                   && !CheckSubsidy16 && !CheckFcpNumber16;

        _syncingAll16 = true;
        CheckAll16 = all ? true : none ? false : null;
        _syncingAll16 = false;
    }

    private void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
