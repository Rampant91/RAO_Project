using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class GroupBulkExportReportsMessageVM : INotifyPropertyChanged
{
    public bool Ok;

    private string _excelFilePath = string.Empty;
    public string ExcelFilePath
    {
        get => _excelFilePath;
        set
        {
            if (_excelFilePath == value) return;
            _excelFilePath = value;
            OnPropertyChanged();
        }
    }

    private string _outputFolderPath = string.Empty;
    public string OutputFolderPath
    {
        get => _outputFolderPath;
        set
        {
            if (_outputFolderPath == value) return;
            _outputFolderPath = value;
            OnPropertyChanged();
        }
    }

    private string _initialDate = string.Empty;
    public string InitialDate
    {
        get => _initialDate;
        set
        {
            _initialDate = DateTime.TryParse(value, out var dateTimeValue)
                ? dateTimeValue.ToShortDateString()
                : value;
            OnPropertyChanged();
        }
    }

    private string _residualDate = string.Empty;
    public string ResidualDate
    {
        get => _residualDate;
        set
        {
            _residualDate = DateTime.TryParse(value, out var dateTimeValue)
                ? dateTimeValue.ToShortDateString()
                : value;
            OnPropertyChanged();
        }
    }

    private bool _syncingAllForms;

    private bool? _checkAllForms = true;
    public bool? CheckAllForms
    {
        get => _checkAllForms;
        set
        {
            if (_checkAllForms == value) return;
            _checkAllForms = value;
            OnPropertyChanged();
            if (!_syncingAllForms && value is bool allChecked)
            {
                ApplyAllFormsChecked(allChecked);
            }
        }
    }

    private bool _checkForm11 = true;
    public bool CheckForm11 { get => _checkForm11; set => SetFormChecked(ref _checkForm11, value); }

    private bool _checkForm12 = true;
    public bool CheckForm12 { get => _checkForm12; set => SetFormChecked(ref _checkForm12, value); }

    private bool _checkForm13 = true;
    public bool CheckForm13 { get => _checkForm13; set => SetFormChecked(ref _checkForm13, value); }

    private bool _checkForm14 = true;
    public bool CheckForm14 { get => _checkForm14; set => SetFormChecked(ref _checkForm14, value); }

    private bool _checkForm15 = true;
    public bool CheckForm15 { get => _checkForm15; set => SetFormChecked(ref _checkForm15, value); }

    private bool _checkForm16 = true;
    public bool CheckForm16 { get => _checkForm16; set => SetFormChecked(ref _checkForm16, value); }

    private bool _checkForm17 = true;
    public bool CheckForm17 { get => _checkForm17; set => SetFormChecked(ref _checkForm17, value); }

    private bool _checkForm18 = true;
    public bool CheckForm18 { get => _checkForm18; set => SetFormChecked(ref _checkForm18, value); }

    private bool _checkForm19 = true;
    public bool CheckForm19 { get => _checkForm19; set => SetFormChecked(ref _checkForm19, value); }

    private bool _checkForm21 = true;
    public bool CheckForm21 { get => _checkForm21; set => SetFormChecked(ref _checkForm21, value); }

    private bool _checkForm22 = true;
    public bool CheckForm22 { get => _checkForm22; set => SetFormChecked(ref _checkForm22, value); }

    private bool _checkForm23 = true;
    public bool CheckForm23 { get => _checkForm23; set => SetFormChecked(ref _checkForm23, value); }

    private bool _checkForm24 = true;
    public bool CheckForm24 { get => _checkForm24; set => SetFormChecked(ref _checkForm24, value); }

    private bool _checkForm25 = true;
    public bool CheckForm25 { get => _checkForm25; set => SetFormChecked(ref _checkForm25, value); }

    private bool _checkForm26 = true;
    public bool CheckForm26 { get => _checkForm26; set => SetFormChecked(ref _checkForm26, value); }

    private bool _checkForm27 = true;
    public bool CheckForm27 { get => _checkForm27; set => SetFormChecked(ref _checkForm27, value); }

    private bool _checkForm28 = true;
    public bool CheckForm28 { get => _checkForm28; set => SetFormChecked(ref _checkForm28, value); }

    private bool _checkForm29 = true;
    public bool CheckForm29 { get => _checkForm29; set => SetFormChecked(ref _checkForm29, value); }

    private bool _checkForm210 = true;
    public bool CheckForm210 { get => _checkForm210; set => SetFormChecked(ref _checkForm210, value); }

    private bool _checkForm211 = true;
    public bool CheckForm211 { get => _checkForm211; set => SetFormChecked(ref _checkForm211, value); }

    private bool _checkForm212 = true;
    public bool CheckForm212 { get => _checkForm212; set => SetFormChecked(ref _checkForm212, value); }

    private void SetFormChecked(ref bool field, bool value, [CallerMemberName] string? propertyName = null)
    {
        if (field == value) return;
        field = value;
        OnPropertyChanged(propertyName);
        if (!_syncingAllForms)
        {
            UpdateCheckAllFormsState();
        }
    }

    public IReadOnlyList<string> GetSelectedFormNumbers()
    {
        var forms = new List<string>(21);
        if (CheckForm11) forms.Add("1.1");
        if (CheckForm12) forms.Add("1.2");
        if (CheckForm13) forms.Add("1.3");
        if (CheckForm14) forms.Add("1.4");
        if (CheckForm15) forms.Add("1.5");
        if (CheckForm16) forms.Add("1.6");
        if (CheckForm17) forms.Add("1.7");
        if (CheckForm18) forms.Add("1.8");
        if (CheckForm19) forms.Add("1.9");
        if (CheckForm21) forms.Add("2.1");
        if (CheckForm22) forms.Add("2.2");
        if (CheckForm23) forms.Add("2.3");
        if (CheckForm24) forms.Add("2.4");
        if (CheckForm25) forms.Add("2.5");
        if (CheckForm26) forms.Add("2.6");
        if (CheckForm27) forms.Add("2.7");
        if (CheckForm28) forms.Add("2.8");
        if (CheckForm29) forms.Add("2.9");
        if (CheckForm210) forms.Add("2.10");
        if (CheckForm211) forms.Add("2.11");
        if (CheckForm212) forms.Add("2.12");
        return forms;
    }

    private IEnumerable<bool> AllFormCheckStates =>
    [
        CheckForm11, CheckForm12, CheckForm13, CheckForm14, CheckForm15,
        CheckForm16, CheckForm17, CheckForm18, CheckForm19,
        CheckForm21, CheckForm22, CheckForm23, CheckForm24, CheckForm25,
        CheckForm26, CheckForm27, CheckForm28, CheckForm29,
        CheckForm210, CheckForm211, CheckForm212
    ];

    private void ApplyAllFormsChecked(bool isChecked)
    {
        _syncingAllForms = true;
        try
        {
            _checkForm11 = _checkForm12 = _checkForm13 = _checkForm14 = _checkForm15 =
                _checkForm16 = _checkForm17 = _checkForm18 = _checkForm19 =
                _checkForm21 = _checkForm22 = _checkForm23 = _checkForm24 = _checkForm25 =
                _checkForm26 = _checkForm27 = _checkForm28 = _checkForm29 =
                _checkForm210 = _checkForm211 = _checkForm212 = isChecked;
            NotifyAllFormCheckboxesChanged();
        }
        finally
        {
            _syncingAllForms = false;
        }
    }

    private void NotifyAllFormCheckboxesChanged()
    {
        OnPropertyChanged(nameof(CheckForm11));
        OnPropertyChanged(nameof(CheckForm12));
        OnPropertyChanged(nameof(CheckForm13));
        OnPropertyChanged(nameof(CheckForm14));
        OnPropertyChanged(nameof(CheckForm15));
        OnPropertyChanged(nameof(CheckForm16));
        OnPropertyChanged(nameof(CheckForm17));
        OnPropertyChanged(nameof(CheckForm18));
        OnPropertyChanged(nameof(CheckForm19));
        OnPropertyChanged(nameof(CheckForm21));
        OnPropertyChanged(nameof(CheckForm22));
        OnPropertyChanged(nameof(CheckForm23));
        OnPropertyChanged(nameof(CheckForm24));
        OnPropertyChanged(nameof(CheckForm25));
        OnPropertyChanged(nameof(CheckForm26));
        OnPropertyChanged(nameof(CheckForm27));
        OnPropertyChanged(nameof(CheckForm28));
        OnPropertyChanged(nameof(CheckForm29));
        OnPropertyChanged(nameof(CheckForm210));
        OnPropertyChanged(nameof(CheckForm211));
        OnPropertyChanged(nameof(CheckForm212));
    }

    private void UpdateCheckAllFormsState()
    {
        if (_syncingAllForms) return;

        var states = AllFormCheckStates.ToArray();
        _checkAllForms = states.All(x => x)
            ? true
            : states.All(x => !x)
                ? false
                : null;
        OnPropertyChanged(nameof(CheckAllForms));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

public sealed class GroupBulkExportReportsDialogResult
{
    public required string ExcelFilePath { get; init; }
    public required string OutputFolderPath { get; init; }
    public required IReadOnlyList<string> FormNumbers { get; init; }
    public required DateOnly PeriodStart { get; init; }
    public required DateOnly PeriodEnd { get; init; }
}
