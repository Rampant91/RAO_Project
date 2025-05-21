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

    #endregion

    #region PropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}
