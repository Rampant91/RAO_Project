using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using OfficeOpenXml;

namespace Models.Forms.DataAccess;

public class RamAccess<T> : RamAccess, INotifyDataErrorInfo
{
    [NotMapped]
    public Func<RamAccess<T>, bool> Handler { get; set; }
    public int Id { get; set; }

    public RefBool IsGet { get; set; }
    private void  IsGetChanged(object sender, PropertyChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Value));
    }
    public RefBool IsSet { get; set; }
    private void IsSetChanged(object sender, PropertyChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Value));
    }

    public int? ValueId { get; set; }
    private T _value;
    public T Value
    {
        get
        {
            if (IsGet == null || IsGet.Get())
            {
                return _value;
            }
            return default(T);
        }
        set
        {
            if (IsSet?.Get() ?? true)
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
                Handler?.Invoke(this);
            }
        }
    }
    public T ValueWithOutHandler
    {
        get => _value;
        set
        {
            if (value != null)
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }
    }
    public T ValueWithOutHandlerAndPropChanged
    {
        get => _value;
        set => _value = value;
    }
    public T ValueWithOutPropChanged
    {
        get => _value;
        set
        {
            _value = value;
            Handler?.Invoke(this);
        }
    }

    public RamAccess(Func<RamAccess<T>, bool> handler, T value)
    {
        Handler = handler;
        _value = value;
        handler?.Invoke(this);
    }
    public RamAccess(Func<RamAccess<T>, bool> handler, T value, RefBool isGet, RefBool isSet)
    {
        Handler = handler;
        _value = value;
        handler?.Invoke(this);
        IsGet = isGet;
        IsGet.PropertyChanged += IsGetChanged;
        IsSet = isSet;
        IsSet.PropertyChanged += IsSetChanged;
    }

    public RamAccess()
    {

    }

    public void ClearErrors()
    {
        ClearErrors(nameof(Value));
    }
    public void AddError(string error)
    {
        AddError(nameof(Value), error);
    }

    #region Equals
    public override bool Equals(object obj)
    {
        if (obj is RamAccess<T> ramAccess)
        {
            dynamic val1 = Value;
            dynamic val2 = ramAccess.Value;
            return val1 == val2;
        }
        return false;
    }

    public static bool operator ==(RamAccess<T> obj1, RamAccess<T> obj2)
    {
        if (obj1 as object != null)
        {
            return obj1.Equals(obj2);
        }
        return obj2 as object == null;
    }
    public static bool operator !=(RamAccess<T> obj1, RamAccess<T> obj2)
    {
        if (obj1 as object != null)
        {
            return !obj1.Equals(obj2);
        }
        return obj2 as object != null;
    }
    #endregion

    #region INotifyDataErrorInfo
    protected readonly List<string> _errorsByPropertyName = new();
    public bool HasErrors => _errorsByPropertyName.Any();
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    public IEnumerable GetErrors(string propertyName)
    {
        var tmp = _errorsByPropertyName.Count > 0 ?
            _errorsByPropertyName : null;
        return tmp?.Select(item => new Exception(item)).ToList();
    }
    protected void OnErrorsChanged(string propertyName)
    {
        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
    }
    protected void ClearErrors(string propertyName)
    {
        if (_errorsByPropertyName.Count > 0)
        {
            _errorsByPropertyName.Clear();
            OnErrorsChanged(propertyName);
        }
    }
    protected void AddError(string propertyName, string error)
    {
        if (!_errorsByPropertyName.Contains(error))
        {
            _errorsByPropertyName.Add(error);
            OnErrorsChanged(propertyName);
        }
    }
    #endregion

    #region IExcel
    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool tanspon = true)
    {
        throw new NotImplementedException();
    }

    public int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpon = true)
    {
        throw new NotImplementedException();
    }
    #endregion
}