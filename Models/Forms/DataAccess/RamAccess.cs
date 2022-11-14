using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using OfficeOpenXml;

namespace Models.DataAccess;

public class RamAccess<T> : RamAccess, INotifyDataErrorInfo, INotifyPropertyChanged
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
            if (IsGet==null?true:IsGet.Get())
            {
                return _value;
            }
            else
            {
                return default(T);
            }
        }
        set
        {
            if (IsSet == null ? true : IsSet.Get())
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
                if (Handler != null)
                {
                    Handler(this);
                }
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
        set
        {
            _value = value;
        }
    }
    public T ValueWithOutPropChanged
    {
        get => _value;
        set
        {
            _value = value;
            if (Handler != null)
            {
                Handler(this);
            }
        }
    }

    public RamAccess(Func<RamAccess<T>, bool> Handler, T Value)
    {
        this.Handler = Handler;
        _value = Value;
        if (Handler != null)
        {
            Handler(this);
        }
    }
    public RamAccess(Func<RamAccess<T>, bool> Handler, T Value,RefBool IsGet,RefBool IsSet)
    {
        this.Handler = Handler;
        _value = Value;
        if (Handler != null)
        {
            Handler(this);
        }
        this.IsGet = IsGet;
        this.IsGet.PropertyChanged += IsGetChanged;
        this.IsSet = IsSet;
        this.IsSet.PropertyChanged += IsSetChanged;
    }

    public RamAccess()
    {

    }

    public void ClearErrors()
    {
        ClearErrors("Value");
    }
    public void AddError(string error)
    {
        AddError("Value", error);
    }

    #region Equals
    public override bool Equals(object obj)
    {
        if (obj is RamAccess<T>)
        {
            dynamic val1 = Value;
            dynamic val2 = (obj as RamAccess<T>).Value;
            return val1 == val2;
        }
        else
        {
            return false;
        }
    }

    public static bool operator ==(RamAccess<T> obj1, RamAccess<T> obj2)
    {
        if (obj1 as object != null)
        {
            return obj1.Equals(obj2);
        }
        else
        {
            return obj2 as object == null ? true : false;
        }
    }
    public static bool operator !=(RamAccess<T> obj1, RamAccess<T> obj2)
    {
        if (obj1 as object != null)
        {
            return !obj1.Equals(obj2);
        }
        else
        {
            return obj2 as object != null ? true : false;
        }
    }
    #endregion

    #region INotifyDataErrorInfo
    protected readonly List<string> _errorsByPropertyName = new();
    public bool HasErrors => _errorsByPropertyName.Any();
    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
    public IEnumerable GetErrors(string propertyName)
    {
        List<string> tmp = _errorsByPropertyName.Count > 0 ?
            _errorsByPropertyName : null;
        if (tmp != null)
        {
            List<Exception> lst = new();
            foreach (string item in tmp)
            {
                lst.Add(new Exception(item));
            }
            return lst;
        }
        else
        {
            return null;
        }
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
    public int ExcelRow(ExcelWorksheet worksheet, int Row, int Column, bool Tanspon = true)
    {
        throw new NotImplementedException();
    }

    public int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
    {
        throw new NotImplementedException();
    }
    #endregion
}