using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

/// <summary>
/// Параметры сопоставления операций приёма-передачи (формы 1.1 и 1.3).
/// </summary>
public class GetTransferReceiveParamsVM : INotifyPropertyChanged
{
    public bool Ok;
    private bool _syncingAll;
    private bool _syncingAll13;

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

    private void OnPropertyChanged([CallerMemberName] string prop = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
}
