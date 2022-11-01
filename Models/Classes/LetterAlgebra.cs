using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Classes
{
    public class LetterAlgebra
    {
        private List<char> _innerNumberRng  = new();

        public LetterAlgebra(IEnumerable<char> Obj)
        {
            _innerNumberRng = new List<char>(Obj);
        }

        public LetterAlgebra(string Obj)
        {
            _innerNumberRng = new List<char>(Obj);
        }

        public LetterAlgebra(char Obj)
        {
            _innerNumberRng.Add(Obj);
        }
        private LetterAlgebra()
        {

        }

        public static LetterAlgebra operator++(LetterAlgebra obj)
        {
            int zap = 1;

            if (obj != null)
            {
                for (int i = obj._innerNumberRng.Count() - 1; i >= 0; i--)
                {
                    if (obj._innerNumberRng[i] + zap == ('Z' + zap))
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
}
