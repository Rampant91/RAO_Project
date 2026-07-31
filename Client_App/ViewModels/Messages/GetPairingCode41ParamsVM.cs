using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class GetPairingCode41ParamsVM : INotifyPropertyChanged
{
    public bool Ok;
    private bool _syncingAll11;
    private bool _syncingAll12;
    private bool _syncingAll13;
    private bool _syncingAll14;

    private bool? _checkAll = true;
    public bool? CheckAll { get => _checkAll; set => SetAllValue(ref _checkAll, value, ref _syncingAll11, ApplyAll11To15); }
    private bool? _checkAll12To16 = true;
    public bool? CheckAll12To16 { get => _checkAll12To16; set => SetAllValue(ref _checkAll12To16, value, ref _syncingAll12, ApplyAll12To16); }
    private bool? _checkAll13To16 = true;
    public bool? CheckAll13To16 { get => _checkAll13To16; set => SetAllValue(ref _checkAll13To16, value, ref _syncingAll13, ApplyAll13To16); }
    private bool? _checkAll14To16 = true;
    public bool? CheckAll14To16 { get => _checkAll14To16; set => SetAllValue(ref _checkAll14To16, value, ref _syncingAll14, ApplyAll14To16); }

    private bool _checkOperationDate = true;
    public bool CheckOperationDate { get => _checkOperationDate; set => SetField(ref _checkOperationDate, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkPassportNumber = true;
    public bool CheckPassportNumber { get => _checkPassportNumber; set => SetField(ref _checkPassportNumber, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkType = true;
    public bool CheckType { get => _checkType; set => SetField(ref _checkType, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkRadionuclids = true;
    public bool CheckRadionuclids { get => _checkRadionuclids; set => SetField(ref _checkRadionuclids, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkFactoryNumber = true;
    public bool CheckFactoryNumber { get => _checkFactoryNumber; set => SetField(ref _checkFactoryNumber, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkActivity = true;
    public bool CheckActivity { get => _checkActivity; set => SetField(ref _checkActivity, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkQuantity = true;
    public bool CheckQuantity { get => _checkQuantity; set => SetField(ref _checkQuantity, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkCreationDate = true;
    public bool CheckCreationDate { get => _checkCreationDate; set => SetField(ref _checkCreationDate, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkDocumentVid = true;
    public bool CheckDocumentVid { get => _checkDocumentVid; set => SetField(ref _checkDocumentVid, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkDocumentNumber = true;
    public bool CheckDocumentNumber { get => _checkDocumentNumber; set => SetField(ref _checkDocumentNumber, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkDocumentDate = true;
    public bool CheckDocumentDate { get => _checkDocumentDate; set => SetField(ref _checkDocumentDate, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkProviderOrRecieverOkpo = true;
    public bool CheckProviderOrRecieverOkpo { get => _checkProviderOrRecieverOkpo; set => SetField(ref _checkProviderOrRecieverOkpo, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkTransporterOkpo = true;
    public bool CheckTransporterOkpo { get => _checkTransporterOkpo; set => SetField(ref _checkTransporterOkpo, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkPackName = true;
    public bool CheckPackName { get => _checkPackName; set => SetField(ref _checkPackName, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkPackType = true;
    public bool CheckPackType { get => _checkPackType; set => SetField(ref _checkPackType, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkPackNumber = true;
    public bool CheckPackNumber { get => _checkPackNumber; set => SetField(ref _checkPackNumber, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkOperationCode = true;
    public bool CheckOperationCode { get => _checkOperationCode; set => SetField(ref _checkOperationCode, value, ref _syncingAll11, UpdateCheckAll11To15); }

    private bool _checkOperationDate12To16 = true;
    public bool CheckOperationDate12To16 { get => _checkOperationDate12To16; set => SetField(ref _checkOperationDate12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkMass12To16 = true;
    public bool CheckMass12To16 { get => _checkMass12To16; set => SetField(ref _checkMass12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkBetaGammaActivity12To16 = true;
    public bool CheckBetaGammaActivity12To16 { get => _checkBetaGammaActivity12To16; set => SetField(ref _checkBetaGammaActivity12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkAlphaActivity12To16 = true;
    public bool CheckAlphaActivity12To16 { get => _checkAlphaActivity12To16; set => SetField(ref _checkAlphaActivity12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkActivityMeasurementDate12To16 = true;
    public bool CheckActivityMeasurementDate12To16 { get => _checkActivityMeasurementDate12To16; set => SetField(ref _checkActivityMeasurementDate12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkDocumentVid12To16 = true;
    public bool CheckDocumentVid12To16 { get => _checkDocumentVid12To16; set => SetField(ref _checkDocumentVid12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkDocumentNumber12To16 = true;
    public bool CheckDocumentNumber12To16 { get => _checkDocumentNumber12To16; set => SetField(ref _checkDocumentNumber12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkDocumentDate12To16 = true;
    public bool CheckDocumentDate12To16 { get => _checkDocumentDate12To16; set => SetField(ref _checkDocumentDate12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkPackName12To16 = true;
    public bool CheckPackName12To16 { get => _checkPackName12To16; set => SetField(ref _checkPackName12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkPackType12To16 = true;
    public bool CheckPackType12To16 { get => _checkPackType12To16; set => SetField(ref _checkPackType12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkPackNumber12To16 = true;
    public bool CheckPackNumber12To16 { get => _checkPackNumber12To16; set => SetField(ref _checkPackNumber12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }
    private bool _checkCodeRao12To16 = true;
    public bool CheckCodeRao12To16 { get => _checkCodeRao12To16; set => SetField(ref _checkCodeRao12To16, value, ref _syncingAll12, UpdateCheckAll12To16); }

    private bool _checkOperationDate13To16 = true;
    public bool CheckOperationDate13To16 { get => _checkOperationDate13To16; set => SetField(ref _checkOperationDate13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkMainRadionuclids13To16 = true;
    public bool CheckMainRadionuclids13To16 { get => _checkMainRadionuclids13To16; set => SetField(ref _checkMainRadionuclids13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkTritiumActivity13To16 = true;
    public bool CheckTritiumActivity13To16 { get => _checkTritiumActivity13To16; set => SetField(ref _checkTritiumActivity13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkBetaGammaActivity13To16 = true;
    public bool CheckBetaGammaActivity13To16 { get => _checkBetaGammaActivity13To16; set => SetField(ref _checkBetaGammaActivity13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkAlphaActivity13To16 = true;
    public bool CheckAlphaActivity13To16 { get => _checkAlphaActivity13To16; set => SetField(ref _checkAlphaActivity13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkTransuraniumActivity13To16 = true;
    public bool CheckTransuraniumActivity13To16 { get => _checkTransuraniumActivity13To16; set => SetField(ref _checkTransuraniumActivity13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkActivityMeasurementDate13To16 = true;
    public bool CheckActivityMeasurementDate13To16 { get => _checkActivityMeasurementDate13To16; set => SetField(ref _checkActivityMeasurementDate13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkDocumentVid13To16 = true;
    public bool CheckDocumentVid13To16 { get => _checkDocumentVid13To16; set => SetField(ref _checkDocumentVid13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkDocumentNumber13To16 = true;
    public bool CheckDocumentNumber13To16 { get => _checkDocumentNumber13To16; set => SetField(ref _checkDocumentNumber13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkDocumentDate13To16 = true;
    public bool CheckDocumentDate13To16 { get => _checkDocumentDate13To16; set => SetField(ref _checkDocumentDate13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkPackName13To16 = true;
    public bool CheckPackName13To16 { get => _checkPackName13To16; set => SetField(ref _checkPackName13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkPackType13To16 = true;
    public bool CheckPackType13To16 { get => _checkPackType13To16; set => SetField(ref _checkPackType13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkPackNumber13To16 = true;
    public bool CheckPackNumber13To16 { get => _checkPackNumber13To16; set => SetField(ref _checkPackNumber13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }
    private bool _checkCodeRao13To16 = true;
    public bool CheckCodeRao13To16 { get => _checkCodeRao13To16; set => SetField(ref _checkCodeRao13To16, value, ref _syncingAll13, UpdateCheckAll13To16); }

    private bool _checkOperationDate14To16 = true;
    public bool CheckOperationDate14To16 { get => _checkOperationDate14To16; set => SetField(ref _checkOperationDate14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkVolume14To16 = true;
    public bool CheckVolume14To16 { get => _checkVolume14To16; set => SetField(ref _checkVolume14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkMass14To16 = true;
    public bool CheckMass14To16 { get => _checkMass14To16; set => SetField(ref _checkMass14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkMainRadionuclids14To16 = true;
    public bool CheckMainRadionuclids14To16 { get => _checkMainRadionuclids14To16; set => SetField(ref _checkMainRadionuclids14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkTritiumActivity14To16 = true;
    public bool CheckTritiumActivity14To16 { get => _checkTritiumActivity14To16; set => SetField(ref _checkTritiumActivity14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkBetaGammaActivity14To16 = true;
    public bool CheckBetaGammaActivity14To16 { get => _checkBetaGammaActivity14To16; set => SetField(ref _checkBetaGammaActivity14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkAlphaActivity14To16 = true;
    public bool CheckAlphaActivity14To16 { get => _checkAlphaActivity14To16; set => SetField(ref _checkAlphaActivity14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkTransuraniumActivity14To16 = true;
    public bool CheckTransuraniumActivity14To16 { get => _checkTransuraniumActivity14To16; set => SetField(ref _checkTransuraniumActivity14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkActivityMeasurementDate14To16 = true;
    public bool CheckActivityMeasurementDate14To16 { get => _checkActivityMeasurementDate14To16; set => SetField(ref _checkActivityMeasurementDate14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkDocumentVid14To16 = true;
    public bool CheckDocumentVid14To16 { get => _checkDocumentVid14To16; set => SetField(ref _checkDocumentVid14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkDocumentNumber14To16 = true;
    public bool CheckDocumentNumber14To16 { get => _checkDocumentNumber14To16; set => SetField(ref _checkDocumentNumber14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkDocumentDate14To16 = true;
    public bool CheckDocumentDate14To16 { get => _checkDocumentDate14To16; set => SetField(ref _checkDocumentDate14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkPackName14To16 = true;
    public bool CheckPackName14To16 { get => _checkPackName14To16; set => SetField(ref _checkPackName14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkPackType14To16 = true;
    public bool CheckPackType14To16 { get => _checkPackType14To16; set => SetField(ref _checkPackType14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkPackNumber14To16 = true;
    public bool CheckPackNumber14To16 { get => _checkPackNumber14To16; set => SetField(ref _checkPackNumber14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }
    private bool _checkCodeRao14To16 = true;
    public bool CheckCodeRao14To16 { get => _checkCodeRao14To16; set => SetField(ref _checkCodeRao14To16, value, ref _syncingAll14, UpdateCheckAll14To16); }

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

    private void ApplyAll11To15(bool allChecked)
    {
        _syncingAll11 = true;
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
        CheckOperationCode = allChecked;
        _syncingAll11 = false;
        UpdateCheckAll11To15();
    }

    private void UpdateCheckAll11To15()
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
                  && CheckPackNumber
                  && CheckOperationCode;

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
                   && !CheckPackNumber
                   && !CheckOperationCode;

        _syncingAll11 = true;
        CheckAll = all ? true : none ? false : null;
        _syncingAll11 = false;
    }

    private void ApplyAll12To16(bool value)
    {
        _syncingAll12 = true;
        CheckOperationDate12To16 = value;
        CheckMass12To16 = value;
        CheckBetaGammaActivity12To16 = value;
        CheckAlphaActivity12To16 = value;
        CheckActivityMeasurementDate12To16 = value;
        CheckDocumentVid12To16 = value;
        CheckDocumentNumber12To16 = value;
        CheckDocumentDate12To16 = value;
        CheckPackName12To16 = value;
        CheckPackType12To16 = value;
        CheckPackNumber12To16 = value;
        CheckCodeRao12To16 = value;
        _syncingAll12 = false;
        UpdateCheckAll12To16();
    }

    private void UpdateCheckAll12To16()
    {
        var all = CheckOperationDate12To16
                  && CheckMass12To16
                  && CheckBetaGammaActivity12To16
                  && CheckAlphaActivity12To16
                  && CheckActivityMeasurementDate12To16
                  && CheckDocumentVid12To16
                  && CheckDocumentNumber12To16
                  && CheckDocumentDate12To16
                  && CheckPackName12To16
                  && CheckPackType12To16
                  && CheckPackNumber12To16
                  && CheckCodeRao12To16;

        var none = !CheckOperationDate12To16
                   && !CheckMass12To16
                   && !CheckBetaGammaActivity12To16
                   && !CheckAlphaActivity12To16
                   && !CheckActivityMeasurementDate12To16
                   && !CheckDocumentVid12To16
                   && !CheckDocumentNumber12To16
                   && !CheckDocumentDate12To16
                   && !CheckPackName12To16
                   && !CheckPackType12To16
                   && !CheckPackNumber12To16
                   && !CheckCodeRao12To16;

        _syncingAll12 = true;
        CheckAll12To16 = all ? true : none ? false : null;
        _syncingAll12 = false;
    }

    private void ApplyAll13To16(bool value)
    {
        _syncingAll13 = true;
        CheckOperationDate13To16 = value;
        CheckMainRadionuclids13To16 = value;
        CheckTritiumActivity13To16 = value;
        CheckBetaGammaActivity13To16 = value;
        CheckAlphaActivity13To16 = value;
        CheckTransuraniumActivity13To16 = value;
        CheckActivityMeasurementDate13To16 = value;
        CheckDocumentVid13To16 = value;
        CheckDocumentNumber13To16 = value;
        CheckDocumentDate13To16 = value;
        CheckPackName13To16 = value;
        CheckPackType13To16 = value;
        CheckPackNumber13To16 = value;
        CheckCodeRao13To16 = value;
        _syncingAll13 = false;
        UpdateCheckAll13To16();
    }

    private void UpdateCheckAll13To16()
    {
        var all = CheckOperationDate13To16
                  && CheckMainRadionuclids13To16
                  && CheckTritiumActivity13To16
                  && CheckBetaGammaActivity13To16
                  && CheckAlphaActivity13To16
                  && CheckTransuraniumActivity13To16
                  && CheckActivityMeasurementDate13To16
                  && CheckDocumentVid13To16
                  && CheckDocumentNumber13To16
                  && CheckDocumentDate13To16
                  && CheckPackName13To16
                  && CheckPackType13To16
                  && CheckPackNumber13To16
                  && CheckCodeRao13To16;

        var none = !CheckOperationDate13To16
                   && !CheckMainRadionuclids13To16
                   && !CheckTritiumActivity13To16
                   && !CheckBetaGammaActivity13To16
                   && !CheckAlphaActivity13To16
                   && !CheckTransuraniumActivity13To16
                   && !CheckActivityMeasurementDate13To16
                   && !CheckDocumentVid13To16
                   && !CheckDocumentNumber13To16
                   && !CheckDocumentDate13To16
                   && !CheckPackName13To16
                   && !CheckPackType13To16
                   && !CheckPackNumber13To16
                   && !CheckCodeRao13To16;

        _syncingAll13 = true;
        CheckAll13To16 = all ? true : none ? false : null;
        _syncingAll13 = false;
    }

    private void ApplyAll14To16(bool value)
    {
        _syncingAll14 = true;
        CheckOperationDate14To16 = value;
        CheckVolume14To16 = value;
        CheckMass14To16 = value;
        CheckMainRadionuclids14To16 = value;
        CheckTritiumActivity14To16 = value;
        CheckBetaGammaActivity14To16 = value;
        CheckAlphaActivity14To16 = value;
        CheckTransuraniumActivity14To16 = value;
        CheckActivityMeasurementDate14To16 = value;
        CheckDocumentVid14To16 = value;
        CheckDocumentNumber14To16 = value;
        CheckDocumentDate14To16 = value;
        CheckPackName14To16 = value;
        CheckPackType14To16 = value;
        CheckPackNumber14To16 = value;
        CheckCodeRao14To16 = value;
        _syncingAll14 = false;
        UpdateCheckAll14To16();
    }

    private void UpdateCheckAll14To16()
    {
        var all = CheckOperationDate14To16
                  && CheckVolume14To16
                  && CheckMass14To16
                  && CheckMainRadionuclids14To16
                  && CheckTritiumActivity14To16
                  && CheckBetaGammaActivity14To16
                  && CheckAlphaActivity14To16
                  && CheckTransuraniumActivity14To16
                  && CheckActivityMeasurementDate14To16
                  && CheckDocumentVid14To16
                  && CheckDocumentNumber14To16
                  && CheckDocumentDate14To16
                  && CheckPackName14To16
                  && CheckPackType14To16
                  && CheckPackNumber14To16
                  && CheckCodeRao14To16;

        var none = !CheckOperationDate14To16
                   && !CheckVolume14To16
                   && !CheckMass14To16
                   && !CheckMainRadionuclids14To16
                   && !CheckTritiumActivity14To16
                   && !CheckBetaGammaActivity14To16
                   && !CheckAlphaActivity14To16
                   && !CheckTransuraniumActivity14To16
                   && !CheckActivityMeasurementDate14To16
                   && !CheckDocumentVid14To16
                   && !CheckDocumentNumber14To16
                   && !CheckDocumentDate14To16
                   && !CheckPackName14To16
                   && !CheckPackType14To16
                   && !CheckPackNumber14To16
                   && !CheckCodeRao14To16;

        _syncingAll14 = true;
        CheckAll14To16 = all ? true : none ? false : null;
        _syncingAll14 = false;
    }

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
