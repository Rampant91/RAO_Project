using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RAO_Calculator_Library
{
    [Serializable]
    public class Radionuclide : INotifyPropertyChanged
    {
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj is Radionuclide)
            {
                return this.Name == (obj as Radionuclide).Name;
            }
            else
            {
                return false;
            }
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        private string name;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value != name)
                {
                    name = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double halfLifePeriod;
        public double HalfLifePeriod
        {
            get
            {
                return halfLifePeriod;
            }
            set 
            {
                if (value != halfLifePeriod)
                {
                    halfLifePeriod = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private string radiationType;
        public string RadiationType 
        {
            get
            {
                return radiationType;
            }
            set
            {
                if (value != radiationType)
                {
                    radiationType = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool afterUran;
        public bool AfterUran
        {
            get
            {
                return afterUran;
            }
            set
            {
                if (value != afterUran)
                {
                    afterUran = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double pzua_t;
        public double PZUA_T
        {
            get
            {
                return pzua_t;
            }
            set
            {
                if (value != pzua_t)
                {
                    pzua_t = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double pzua_w;
        public double PZUA_W
        {
            get
            {
                return pzua_w;
            }
            set
            {
                if (value != pzua_w)
                {
                    pzua_w = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double pzua_g;
        public double PZUA_G
        {
            get
            {
                return pzua_g;
            }
            set
            {
                if (value != pzua_g)
                {
                    pzua_g = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double uda;
        public double UDA
        {
            get
            {
                return uda;
            }
            set
            {
                if (value != uda)
                {
                    uda = value;
                    NotifyPropertyChanged();
                    NotifyPropertyChanged("UDA_PZUA");
                }
            }
        }

        private string twg;
        public string TWG
        {
            get
            {
                return twg;
            }
            set
            {
                if (value != twg)
                {
                    twg = value;
                    NotifyPropertyChanged();
                }
            }
        }
        private double moi;
        public double MOI
        {
            get
            {
                return moi;
            }
            set
            {
                if (value != moi)
                {
                    moi = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private double d_val;
        public double D_val
        {
            get
            {
                return d_val;
            }
            set
            {
                if (value != d_val)
                {
                    d_val = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public double PZUA
        {
            get
            {
                if(twg=="t")
                {
                    return pzua_t;
                }
                if (twg == "w")
                {
                    return pzua_w;
                }
                if (twg == "g")
                {
                    return pzua_g;
                }
                return 0;
            }
        }
        public double UDA_PZUA
        {
            get
            {
                if (TWG == "t")
                {
                    return UDA / PZUA_T;
                }
                if (TWG == "w")
                {
                    return UDA / PZUA_W;
                }
                if (TWG == "g")
                {
                    return UDA / PZUA_G;
                }
                return -1;
            }
        }

        public static bool operator ==(Radionuclide obj1,Radionuclide obj2)
        {
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Radionuclide obj1, Radionuclide obj2)
        {
            return !obj1.Equals(obj2);
        }
    }
}
