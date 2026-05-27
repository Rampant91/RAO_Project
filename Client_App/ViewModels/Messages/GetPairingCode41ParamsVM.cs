using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class GetPairingCode41ParamsVM : INotifyPropertyChanged
{
    public bool Ok;

    private bool _syncingAll;

    private bool? _checkAll = true;
    public bool? CheckAll
    {
        get => _checkAll;
        set
        {
            if (_checkAll == value) return;
            _checkAll = value;
            OnPropertyChanged();
            if (!_syncingAll && value is bool allChecked)
            {
                ApplyAll(allChecked);
            }
        }
    }

    private bool _checkOperationDate = true;
    public bool CheckOperationDate { get => _checkOperationDate; set => SetField(ref _checkOperationDate, value); }

    private bool _checkPassportNumber = true;
    public bool CheckPassportNumber { get => _checkPassportNumber; set => SetField(ref _checkPassportNumber, value); }

    private bool _checkType = true;
    public bool CheckType { get => _checkType; set => SetField(ref _checkType, value); }

    private bool _checkRadionuclids = true;
    public bool CheckRadionuclids { get => _checkRadionuclids; set => SetField(ref _checkRadionuclids, value); }

    private bool _checkFactoryNumber = true;
    public bool CheckFactoryNumber { get => _checkFactoryNumber; set => SetField(ref _checkFactoryNumber, value); }

    private bool _checkActivity = true;
    public bool CheckActivity { get => _checkActivity; set => SetField(ref _checkActivity, value); }

    private bool _checkQuantity = true;
    public bool CheckQuantity { get => _checkQuantity; set => SetField(ref _checkQuantity, value); }

    private bool _checkCreationDate = true;
    public bool CheckCreationDate { get => _checkCreationDate; set => SetField(ref _checkCreationDate, value); }

    private bool _checkDocumentVid = true;
    public bool CheckDocumentVid { get => _checkDocumentVid; set => SetField(ref _checkDocumentVid, value); }

    private bool _checkDocumentNumber = true;
    public bool CheckDocumentNumber { get => _checkDocumentNumber; set => SetField(ref _checkDocumentNumber, value); }

    private bool _checkDocumentDate = true;
    public bool CheckDocumentDate { get => _checkDocumentDate; set => SetField(ref _checkDocumentDate, value); }

    private bool _checkProviderOrRecieverOkpo = true;
    public bool CheckProviderOrRecieverOkpo { get => _checkProviderOrRecieverOkpo; set => SetField(ref _checkProviderOrRecieverOkpo, value); }

    private bool _checkTransporterOkpo = true;
    public bool CheckTransporterOkpo { get => _checkTransporterOkpo; set => SetField(ref _checkTransporterOkpo, value); }

    private bool _checkPackName = true;
    public bool CheckPackName { get => _checkPackName; set => SetField(ref _checkPackName, value); }

    private bool _checkPackType = true;
    public bool CheckPackType { get => _checkPackType; set => SetField(ref _checkPackType, value); }

    private bool _checkPackNumber = true;
    public bool CheckPackNumber { get => _checkPackNumber; set => SetField(ref _checkPackNumber, value); }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetField(ref bool field, bool value, [CallerMemberName] string prop = "")
    {
        if (field == value) return;
        field = value;
        OnPropertyChanged(prop);
        if (!_syncingAll)
        {
            UpdateCheckAll();
        }
    }

    private void ApplyAll(bool allChecked)
    {
        _syncingAll = true;
        CheckOperationDate = allChecked;
        CheckPassportNumber = allChecked;
        CheckType = allChecked;
        CheckRadionuclids = allChecked;
        CheckFactoryNumber = allChecked;
        CheckActivity = allChecked;
        CheckQuantity = allChecked;
        CheckCreationDate = allChecked;
        CheckDocumentVid = allChecked;
        CheckDocumentNumber = allChecked;
        CheckDocumentDate = allChecked;
        CheckProviderOrRecieverOkpo = allChecked;
        CheckTransporterOkpo = allChecked;
        CheckPackName = allChecked;
        CheckPackType = allChecked;
        CheckPackNumber = allChecked;
        _syncingAll = false;
        UpdateCheckAll();
    }

    private void UpdateCheckAll()
    {
        var all = CheckOperationDate
                  && CheckPassportNumber
                  && CheckType
                  && CheckRadionuclids
                  && CheckFactoryNumber
                  && CheckActivity
                  && CheckQuantity
                  && CheckCreationDate
                  && CheckDocumentVid
                  && CheckDocumentNumber
                  && CheckDocumentDate
                  && CheckProviderOrRecieverOkpo
                  && CheckTransporterOkpo
                  && CheckPackName
                  && CheckPackType
                  && CheckPackNumber;

        var none = !CheckOperationDate
                   && !CheckPassportNumber
                   && !CheckType
                   && !CheckRadionuclids
                   && !CheckFactoryNumber
                   && !CheckActivity
                   && !CheckQuantity
                   && !CheckCreationDate
                   && !CheckDocumentVid
                   && !CheckDocumentNumber
                   && !CheckDocumentDate
                   && !CheckProviderOrRecieverOkpo
                   && !CheckTransporterOkpo
                   && !CheckPackName
                   && !CheckPackType
                   && !CheckPackNumber;

        _syncingAll = true;
        CheckAll = all ? true : none ? false : null;
        _syncingAll = false;
    }

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
