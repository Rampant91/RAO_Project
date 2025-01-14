using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels;

public class GetSnkParamsVM : INotifyPropertyChanged
{
    public bool Ok = false;

    #region Properties

    #region Date

    private string _date;
    public string Date
    {
        get => _date;
        set
        {
            if (_date == value) return;
            _date = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region PasNum

    private bool _checkPasNum = true;
    public bool CheckPasNum
    {
        get => _checkPasNum;
        set
        {
            if (_checkPasNum == value) return;
            _checkPasNum = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Type

    private bool _checkType = true;
    public bool CheckType
    {
        get => _checkType;
        set
        {
            if (_checkType == value) return;
            _checkType = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Radionuclids

    private bool _checkRadionuclids = true;
    public bool CheckRadionuclids
    {
        get => _checkRadionuclids;
        set
        {
            if (_checkRadionuclids == value) return;
            _checkRadionuclids = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region FacNum

    private bool _checkFacNum = true;
    public bool CheckFacNum
    {
        get => _checkFacNum;
        set
        {
            if (_checkFacNum == value) return;
            _checkFacNum = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region PackNumber

    private bool _checkPackNumber = true;
    public bool CheckPackNumber
    {
        get => _checkPackNumber;
        set
        {
            if (_checkPackNumber == value) return;
            _checkPackNumber = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    #region PropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}
