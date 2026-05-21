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

    private bool? _checkAllForms = true;
    public bool? CheckAllForms
    {
        get => _checkAllForms;
        set
        {
            if (_checkAllForms == value) return;
            _checkAllForms = value;
            OnPropertyChanged();
        }
    }

    private bool _checkForm11 = true;
    public bool CheckForm11
    {
        get => _checkForm11;
        set { if (_checkForm11 == value) return; _checkForm11 = value; OnPropertyChanged(); }
    }

    private bool _checkForm12 = true;
    public bool CheckForm12
    {
        get => _checkForm12;
        set { if (_checkForm12 == value) return; _checkForm12 = value; OnPropertyChanged(); }
    }

    private bool _checkForm13 = true;
    public bool CheckForm13
    {
        get => _checkForm13;
        set { if (_checkForm13 == value) return; _checkForm13 = value; OnPropertyChanged(); }
    }

    private bool _checkForm14 = true;
    public bool CheckForm14
    {
        get => _checkForm14;
        set { if (_checkForm14 == value) return; _checkForm14 = value; OnPropertyChanged(); }
    }

    private bool _checkForm15 = true;
    public bool CheckForm15
    {
        get => _checkForm15;
        set { if (_checkForm15 == value) return; _checkForm15 = value; OnPropertyChanged(); }
    }

    private bool _checkForm16 = true;
    public bool CheckForm16
    {
        get => _checkForm16;
        set { if (_checkForm16 == value) return; _checkForm16 = value; OnPropertyChanged(); }
    }

    private bool _checkForm17 = true;
    public bool CheckForm17
    {
        get => _checkForm17;
        set { if (_checkForm17 == value) return; _checkForm17 = value; OnPropertyChanged(); }
    }

    private bool _checkForm18 = true;
    public bool CheckForm18
    {
        get => _checkForm18;
        set { if (_checkForm18 == value) return; _checkForm18 = value; OnPropertyChanged(); }
    }

    private bool _checkForm19 = true;
    public bool CheckForm19
    {
        get => _checkForm19;
        set { if (_checkForm19 == value) return; _checkForm19 = value; OnPropertyChanged(); }
    }

    public IReadOnlyList<string> GetSelectedFormNumbers()
    {
        var forms = new List<string>(9);
        if (CheckForm11) forms.Add("1.1");
        if (CheckForm12) forms.Add("1.2");
        if (CheckForm13) forms.Add("1.3");
        if (CheckForm14) forms.Add("1.4");
        if (CheckForm15) forms.Add("1.5");
        if (CheckForm16) forms.Add("1.6");
        if (CheckForm17) forms.Add("1.7");
        if (CheckForm18) forms.Add("1.8");
        if (CheckForm19) forms.Add("1.9");
        return forms;
    }

    public void SetAllFormsChecked(bool isChecked)
    {
        CheckForm11 = CheckForm12 = CheckForm13 = CheckForm14 = CheckForm15 =
            CheckForm16 = CheckForm17 = CheckForm18 = CheckForm19 = isChecked;
        CheckAllForms = isChecked;
    }

    public void UpdateCheckAllFormsState()
    {
        var states = new[] { CheckForm11, CheckForm12, CheckForm13, CheckForm14, CheckForm15,
            CheckForm16, CheckForm17, CheckForm18, CheckForm19 };
        CheckAllForms = states.All(x => x)
            ? true
            : states.All(x => !x)
                ? false
                : null;
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
