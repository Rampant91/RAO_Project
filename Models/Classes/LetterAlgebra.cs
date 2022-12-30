using System.Collections.Generic;
using System.Linq;

namespace Models.Classes;

public class LetterAlgebra
{
    private List<char> _innerNumberRng  = new();

    public LetterAlgebra(IEnumerable<char> obj)
    {
        _innerNumberRng = new List<char>(obj);
    }

    public LetterAlgebra(string obj)
    {
        _innerNumberRng = new List<char>(obj);
    }

    public LetterAlgebra(char obj)
    {
        _innerNumberRng.Add(obj);
    }

    public static LetterAlgebra operator++(LetterAlgebra obj)
    {
        var zap = 1;

        if (obj != null)
        {
            for (var i = obj._innerNumberRng.Count() - 1; i >= 0; i--)
            {
                if (obj._innerNumberRng[i] + zap == 'Z' + zap)
                {
                    obj._innerNumberRng[i] = 'A';
                    zap = 1;
                    try
                    {
                        var t = obj._innerNumberRng[i - 1];
                    }
                    catch
                    {
                        obj._innerNumberRng.Insert(i,'A');
                        break;
                    }
                }
                else
                {
                    obj._innerNumberRng[i] = (char)(obj._innerNumberRng[i] + zap);
                    zap = 0;
                    break;
                }
            }
        }
        return obj;
    }

    public new string ToString()
    {
        return string.Join("",_innerNumberRng);
    }
}