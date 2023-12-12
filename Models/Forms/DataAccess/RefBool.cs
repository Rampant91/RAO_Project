namespace Models.Forms.DataAccess;

public class RefBool : RamAccess
{
    private bool _value;

    public RefBool(bool value)
    {
        _value = value;
    }

    public bool Get()
    {
        return _value;
    }
    public void Set(bool value)
    {
        _value = value;
        OnPropertyChanged();
    }

    public bool Equals(RefBool obj)
    {
        if (obj == null) return false;
        return _value == obj._value;
    }

    public bool Equals(bool obj)
    {
        return _value == obj;
    }
}