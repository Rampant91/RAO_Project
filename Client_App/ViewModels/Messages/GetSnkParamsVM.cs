using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class GetSnkParamsVM : INotifyPropertyChanged
{
    /// <summary>
    /// Была ли нажата пользователем кнопка "Ок".
    /// </summary>
    public bool Ok = false;

    #region Properties

    #region Region

    /// <summary>
    /// Поле для номера региона. По умолчанию "00".
    /// </summary>
    private string _region = "00";
    public string Region
    {
        get => _region;
        set
        {
            if (value == null || (value.Length <= 2 && value.All(char.IsDigit)))
            {
                _region = value;
                OnPropertyChanged();
            }
        }
    }
    #endregion

    #region Date

    /// <summary>
    /// Поле для вводимой даты. По умолчанию равно текущей дате.
    /// </summary>
    private string _date = DateTime.Now.ToShortDateString();
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

    #region All

    /// <summary>
    /// Поле чекбокса, позволяющего выбрать/отменить выбор для всех остальных чекбоксов. Может быть null (третье состояние чекбокса).
    /// </summary>
    private bool? _checkAll = true;
    public bool? CheckAll
    {
        get => _checkAll;
        set
        {
            if (_checkAll == value) return;
            _checkAll = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region PasNum

    /// <summary>
    /// Поле чекбокса для номера паспорта.
    /// </summary>
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

    /// <summary>
    /// Поле чекбокса для типа.
    /// </summary>
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

    /// <summary>
    /// Поле чекбокса для радионуклидов.
    /// </summary>
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

    /// <summary>
    /// Поле чекбокса для заводского номера.
    /// </summary>
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

    /// <summary>
    /// Поле чекбокса для номера паспорта.
    /// </summary>
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

    #region CommandName

    /// <summary>
    /// Имя команды.
    /// </summary>
    private string _commandName = "";
    public string CommandName
    {
        get => _commandName;
        set
        {
            if (_commandName == value) return;
            _commandName = value;
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
