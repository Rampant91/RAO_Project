using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client_App.ViewModels.Messages;

public class GetRegionAndFormNumsVM : INotifyPropertyChanged
{
    /// <summary>
    /// Была ли нажата пользователем кнопка "Ок".
    /// </summary>
    public bool Ok = false;

    #region Properties

    #region Region

    /// <summary>
    /// Регион.
    /// </summary>
    private string _region = "";
    public string Region
    {
        get => _region;
        set
        {
            if (_region == value) return;
            _region = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Forms

    #region AllForms

    /// <summary>
    /// Поле чекбокса, позволяющего выбрать/отменить выбор для всех чекбоксов форм. Может быть null (третье состояние чекбокса).
    /// </summary>
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

    #endregion

    #region Form11

    /// <summary>
    /// Поле чекбокса формы 1.1.
    /// </summary>
    private bool _checkForm11 = true;
    public bool CheckForm11
    {
        get => _checkForm11;
        set
        {
            if (_checkForm11 == value) return;
            _checkForm11 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Form13

    /// <summary>
    /// Поле чекбокса формы 1.3.
    /// </summary>
    private bool _checkForm13 = true;
    public bool CheckForm13
    {
        get => _checkForm13;
        set
        {
            if (_checkForm13 == value) return;
            _checkForm13 = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region SnkParams

    #region AllSnkParams

    /// <summary>
    /// Поле чекбокса, позволяющего выбрать/отменить выбор для всех остальных чекбоксов. Может быть null (третье состояние чекбокса).
    /// </summary>
    private bool? _checkAllSnkParams = true;
    public bool? CheckAllSnkParams
    {
        get => _checkAllSnkParams;
        set
        {
            if (_checkAllSnkParams == value) return;
            _checkAllSnkParams = value;
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

    #endregion

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
