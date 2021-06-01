using Models.DataAccess;
using Models.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Collections.Specialized;

namespace Collections
{
    public class Report : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        IDataAccess _dataAccess { get; set; }

        public Report(IDataAccess Access)
        {
            _dataAccess = Access;
            Init();
        }

        public Report()
        {
            _dataAccess = new Models.DataAccess.RamAccess();
            Init();
        }

        void Init()
        {
            Rows10.CollectionChanged += CollectionChanged;
            Rows11.CollectionChanged += CollectionChanged;
            Rows12.CollectionChanged += CollectionChanged;
            Rows13.CollectionChanged += CollectionChanged;
            Rows14.CollectionChanged += CollectionChanged;
            Rows15.CollectionChanged += CollectionChanged;
            Rows16.CollectionChanged += CollectionChanged;
            Rows17.CollectionChanged += CollectionChanged;
            Rows18.CollectionChanged += CollectionChanged;
            Rows19.CollectionChanged += CollectionChanged;

            Rows20.CollectionChanged += CollectionChanged;
            Rows21.CollectionChanged += CollectionChanged;
            Rows22.CollectionChanged += CollectionChanged;
            Rows23.CollectionChanged += CollectionChanged;
            Rows24.CollectionChanged += CollectionChanged;
            Rows25.CollectionChanged += CollectionChanged;
            Rows26.CollectionChanged += CollectionChanged;
            Rows27.CollectionChanged += CollectionChanged;
            Rows28.CollectionChanged += CollectionChanged;
            Rows29.CollectionChanged += CollectionChanged;
            Rows210.CollectionChanged += CollectionChanged;
            Rows211.CollectionChanged += CollectionChanged;
            Rows212.CollectionChanged += CollectionChanged;

            Rows30.CollectionChanged += CollectionChanged;
            Rows31.CollectionChanged += CollectionChanged;
            Rows31_1.CollectionChanged += CollectionChanged;
            Rows32.CollectionChanged += CollectionChanged;
            Rows32_1.CollectionChanged += CollectionChanged;
            Rows32_2.CollectionChanged += CollectionChanged;
            Rows32_3.CollectionChanged += CollectionChanged;

            Rows40.CollectionChanged += CollectionChanged;
            Rows41.CollectionChanged += CollectionChanged;

            Rows50.CollectionChanged += CollectionChanged;
            Rows51.CollectionChanged += CollectionChanged;
            Rows52.CollectionChanged += CollectionChanged;
            Rows53.CollectionChanged += CollectionChanged;
            Rows54.CollectionChanged += CollectionChanged;
            Rows55.CollectionChanged += CollectionChanged;
            Rows56.CollectionChanged += CollectionChanged;
            Rows57.CollectionChanged += CollectionChanged;
        }

        public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Rows10));
            OnPropertyChanged(nameof(Rows11));
            OnPropertyChanged(nameof(Rows12));
            OnPropertyChanged(nameof(Rows13));
            OnPropertyChanged(nameof(Rows14));
            OnPropertyChanged(nameof(Rows15));
            OnPropertyChanged(nameof(Rows16));
            OnPropertyChanged(nameof(Rows17));
            OnPropertyChanged(nameof(Rows18));
            OnPropertyChanged(nameof(Rows19));

            OnPropertyChanged(nameof(Rows20));
            OnPropertyChanged(nameof(Rows21));
            OnPropertyChanged(nameof(Rows22));
            OnPropertyChanged(nameof(Rows23));
            OnPropertyChanged(nameof(Rows24));
            OnPropertyChanged(nameof(Rows25));
            OnPropertyChanged(nameof(Rows26));
            OnPropertyChanged(nameof(Rows27));
            OnPropertyChanged(nameof(Rows28));
            OnPropertyChanged(nameof(Rows29));
            OnPropertyChanged(nameof(Rows210));
            OnPropertyChanged(nameof(Rows211));
            OnPropertyChanged(nameof(Rows212));

            OnPropertyChanged(nameof(Rows30));
            OnPropertyChanged(nameof(Rows31));
            OnPropertyChanged(nameof(Rows31_1));
            OnPropertyChanged(nameof(Rows32));
            OnPropertyChanged(nameof(Rows32_1));
            OnPropertyChanged(nameof(Rows32_2));
            OnPropertyChanged(nameof(Rows32_3));

            OnPropertyChanged(nameof(Rows40));
            OnPropertyChanged(nameof(Rows41));

            OnPropertyChanged(nameof(Rows50));
            OnPropertyChanged(nameof(Rows51));
            OnPropertyChanged(nameof(Rows52));
            OnPropertyChanged(nameof(Rows53));
            OnPropertyChanged(nameof(Rows54));
            OnPropertyChanged(nameof(Rows55));
            OnPropertyChanged(nameof(Rows56));
            OnPropertyChanged(nameof(Rows57));
        }

        [Key]
        public int ReportId { get; set; }

        public virtual ObservableCollection<Models.Form10> Rows10
        {
            get
            {
                if (GetErrors(nameof(Rows10)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows10));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows10), new ObservableCollection<Models.Form10>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows10));
                    return (ObservableCollection<Models.Form10>)tmp;
                }
                else
                {
                    return _Rows10_Not_Valid;
                }
            }
            set
            {
                _Rows10_Not_Valid = value;
                if (GetErrors(nameof(Rows10)) == null)
                {
                    _dataAccess.Set(nameof(Rows10), _Rows10_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows10));
            }
        }
        private ObservableCollection<Models.Form10> _Rows10_Not_Valid = new ObservableCollection<Models.Form10>();
        private bool Rows10_Validation()
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form11> Rows11
        {
            get
            {
                if (GetErrors(nameof(Rows11)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows11));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows11), new ObservableCollection<Models.Form11>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows11));
                    return (ObservableCollection<Models.Form11>)tmp;
                }
                else
                {
                    return _Rows11_Not_Valid;
                }
            }
            set
            {
                _Rows11_Not_Valid = value;
                if (GetErrors(nameof(Rows11)) == null)
                {
                    _dataAccess.Set(nameof(Rows11), _Rows11_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows11));
            }
        }
        private ObservableCollection<Models.Form11> _Rows11_Not_Valid = new ObservableCollection<Models.Form11>();
        private bool Rows11_Validation()
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form12> Rows12
        {
            get
            {
                if (GetErrors(nameof(Rows12)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows12));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows12), new ObservableCollection<Models.Form12>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows12));
                    return (ObservableCollection<Models.Form12>)tmp;
                }
                else
                {
                    return _Rows12_Not_Valid;
                }
            }
            set
            {
                _Rows12_Not_Valid = value;
                if (GetErrors(nameof(Rows12)) == null)
                {
                    _dataAccess.Set(nameof(Rows12), _Rows12_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows12));
            }
        }
        private ObservableCollection<Models.Form12> _Rows12_Not_Valid = new ObservableCollection<Models.Form12>();
        private bool Rows12_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form13> Rows13
        {
            get
            {
                if (GetErrors(nameof(Rows13)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows13));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows13), new ObservableCollection<Models.Form13>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows13));
                    return (ObservableCollection<Models.Form13>)tmp;
                }
                else
                {
                    return _Rows13_Not_Valid;
                }
            }
            set
            {
                _Rows13_Not_Valid = value;
                if (GetErrors(nameof(Rows13)) == null)
                {
                    _dataAccess.Set(nameof(Rows13), _Rows13_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows13));
            }
        }
        private ObservableCollection<Models.Form13> _Rows13_Not_Valid = new ObservableCollection<Models.Form13>();
        private bool Rows13_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form14> Rows14
        {
            get
            {
                if (GetErrors(nameof(Rows14)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows14));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows14), new ObservableCollection<Models.Form14>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows14));
                    return (ObservableCollection<Models.Form14>)tmp;
                }
                else
                {
                    return _Rows14_Not_Valid;
                }
            }
            set
            {
                _Rows14_Not_Valid = value;
                if (GetErrors(nameof(Rows14)) == null)
                {
                    _dataAccess.Set(nameof(Rows14), _Rows14_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows14));
            }
        }
        private ObservableCollection<Models.Form14> _Rows14_Not_Valid = new ObservableCollection<Models.Form14>();
        private bool Rows14_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form15> Rows15
        {
            get
            {
                if (GetErrors(nameof(Rows15)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows15));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows15), new ObservableCollection<Models.Form15>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows15));
                    return (ObservableCollection<Models.Form15>)tmp;
                }
                else
                {
                    return _Rows15_Not_Valid;
                }
            }
            set
            {
                _Rows15_Not_Valid = value;
                if (GetErrors(nameof(Rows15)) == null)
                {
                    _dataAccess.Set(nameof(Rows15), _Rows15_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows15));
            }
        }
        private ObservableCollection<Models.Form15> _Rows15_Not_Valid = new ObservableCollection<Models.Form15>();
        private bool Rows15_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form16> Rows16
        {
            get
            {
                if (GetErrors(nameof(Rows16)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows16));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows16), new ObservableCollection<Models.Form16>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows16));
                    return (ObservableCollection<Models.Form16>)tmp;
                }
                else
                {
                    return _Rows16_Not_Valid;
                }
            }
            set
            {
                _Rows16_Not_Valid = value;
                if (GetErrors(nameof(Rows16)) == null)
                {
                    _dataAccess.Set(nameof(Rows16), _Rows16_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows16));
            }
        }
        private ObservableCollection<Models.Form16> _Rows16_Not_Valid = new ObservableCollection<Models.Form16>();
        private bool Rows16_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form17> Rows17
        {
            get
            {
                if (GetErrors(nameof(Rows17)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows17));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows17), new ObservableCollection<Models.Form17>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows17));
                    return (ObservableCollection<Models.Form17>)tmp;
                }
                else
                {
                    return _Rows17_Not_Valid;
                }
            }
            set
            {
                _Rows17_Not_Valid = value;
                if (GetErrors(nameof(Rows17)) == null)
                {
                    _dataAccess.Set(nameof(Rows17), _Rows17_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows17));
            }
        }
        private ObservableCollection<Models.Form17> _Rows17_Not_Valid = new ObservableCollection<Models.Form17>();
        private bool Rows17_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form18> Rows18
        {
            get
            {
                if (GetErrors(nameof(Rows18)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows18));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows18), new ObservableCollection<Models.Form18>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows18));
                    return (ObservableCollection<Models.Form18>)tmp;
                }
                else
                {
                    return _Rows18_Not_Valid;
                }
            }
            set
            {
                _Rows18_Not_Valid = value;
                if (GetErrors(nameof(Rows18)) == null)
                {
                    _dataAccess.Set(nameof(Rows18), _Rows18_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows18));
            }
        }
        private ObservableCollection<Models.Form18> _Rows18_Not_Valid = new ObservableCollection<Models.Form18>();
        private bool Rows18_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form19> Rows19
        {
            get
            {
                if (GetErrors(nameof(Rows19)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows19));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows19), new ObservableCollection<Models.Form19>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows19));
                    return (ObservableCollection<Models.Form19>)tmp;
                }
                else
                {
                    return _Rows19_Not_Valid;
                }
            }
            set
            {
                _Rows19_Not_Valid = value;
                if (GetErrors(nameof(Rows19)) == null)
                {
                    _dataAccess.Set(nameof(Rows19), _Rows19_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows19));
            }
        }
        private ObservableCollection<Models.Form19> _Rows19_Not_Valid = new ObservableCollection<Models.Form19>();
        private bool Rows19_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form20> Rows20
        {
            get
            {
                if (GetErrors(nameof(Rows20)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows20));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows20), new ObservableCollection<Models.Form20>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows20));
                    return (ObservableCollection<Models.Form20>)tmp;
                }
                else
                {
                    return _Rows20_Not_Valid;
                }
            }
            set
            {
                _Rows20_Not_Valid = value;
                if (GetErrors(nameof(Rows20)) == null)
                {
                    _dataAccess.Set(nameof(Rows20), _Rows20_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows20));
            }
        }
        private ObservableCollection<Models.Form20> _Rows20_Not_Valid = new ObservableCollection<Models.Form20>();
        private bool Rows20_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form21> Rows21
        {
            get
            {
                if (GetErrors(nameof(Rows21)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows21));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows21), new ObservableCollection<Models.Form21>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows21));
                    return (ObservableCollection<Models.Form21>)tmp;
                }
                else
                {
                    return _Rows21_Not_Valid;
                }
            }
            set
            {
                _Rows21_Not_Valid = value;
                if (GetErrors(nameof(Rows21)) == null)
                {
                    _dataAccess.Set(nameof(Rows21), _Rows21_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows21));
            }
        }
        private ObservableCollection<Models.Form21> _Rows21_Not_Valid = new ObservableCollection<Models.Form21>();
        private bool Rows21_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form22> Rows22
        {
            get
            {
                if (GetErrors(nameof(Rows22)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows22));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows22), new ObservableCollection<Models.Form22>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows22));
                    return (ObservableCollection<Models.Form22>)tmp;
                }
                else
                {
                    return _Rows22_Not_Valid;
                }
            }
            set
            {
                _Rows22_Not_Valid = value;
                if (GetErrors(nameof(Rows22)) == null)
                {
                    _dataAccess.Set(nameof(Rows22), _Rows22_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows22));
            }
        }
        private ObservableCollection<Models.Form22> _Rows22_Not_Valid = new ObservableCollection<Models.Form22>();
        private bool Rows22_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form23> Rows23
        {
            get
            {
                if (GetErrors(nameof(Rows23)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows23));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows23), new ObservableCollection<Models.Form23>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows23));
                    return (ObservableCollection<Models.Form23>)tmp;
                }
                else
                {
                    return _Rows23_Not_Valid;
                }
            }
            set
            {
                _Rows23_Not_Valid = value;
                if (GetErrors(nameof(Rows23)) == null)
                {
                    _dataAccess.Set(nameof(Rows23), _Rows23_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows23));
            }
        }
        private ObservableCollection<Models.Form23> _Rows23_Not_Valid = new ObservableCollection<Models.Form23>();
        private bool Rows23_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form24> Rows24
        {
            get
            {
                if (GetErrors(nameof(Rows24)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows24));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows24), new ObservableCollection<Models.Form24>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows24));
                    return (ObservableCollection<Models.Form24>)tmp;
                }
                else
                {
                    return _Rows24_Not_Valid;
                }
            }
            set
            {
                _Rows24_Not_Valid = value;
                if (GetErrors(nameof(Rows24)) == null)
                {
                    _dataAccess.Set(nameof(Rows24), _Rows24_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows24));
            }
        }
        private ObservableCollection<Models.Form24> _Rows24_Not_Valid = new ObservableCollection<Models.Form24>();
        private bool Rows24_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form25> Rows25
        {
            get
            {
                if (GetErrors(nameof(Rows25)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows25));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows25), new ObservableCollection<Models.Form25>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows25));
                    return (ObservableCollection<Models.Form25>)tmp;
                }
                else
                {
                    return _Rows25_Not_Valid;
                }
            }
            set
            {
                _Rows25_Not_Valid = value;
                if (GetErrors(nameof(Rows25)) == null)
                {
                    _dataAccess.Set(nameof(Rows25), _Rows25_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows25));
            }
        }
        private ObservableCollection<Models.Form25> _Rows25_Not_Valid = new ObservableCollection<Models.Form25>();
        private bool Rows25_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form26> Rows26
        {
            get
            {
                if (GetErrors(nameof(Rows26)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows26));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows26), new ObservableCollection<Models.Form26>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows26));
                    return (ObservableCollection<Models.Form26>)tmp;
                }
                else
                {
                    return _Rows26_Not_Valid;
                }
            }
            set
            {
                _Rows26_Not_Valid = value;
                if (GetErrors(nameof(Rows26)) == null)
                {
                    _dataAccess.Set(nameof(Rows26), _Rows26_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows26));
            }
        }
        private ObservableCollection<Models.Form26> _Rows26_Not_Valid = new ObservableCollection<Models.Form26>();
        private bool Rows26_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form27> Rows27
        {
            get
            {
                if (GetErrors(nameof(Rows27)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows27));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows27), new ObservableCollection<Models.Form27>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows27));
                    return (ObservableCollection<Models.Form27>)tmp;
                }
                else
                {
                    return _Rows27_Not_Valid;
                }
            }
            set
            {
                _Rows27_Not_Valid = value;
                if (GetErrors(nameof(Rows27)) == null)
                {
                    _dataAccess.Set(nameof(Rows27), _Rows27_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows27));
            }
        }
        private ObservableCollection<Models.Form27> _Rows27_Not_Valid = new ObservableCollection<Models.Form27>();
        private bool Rows27_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form28> Rows28
        {
            get
            {
                if (GetErrors(nameof(Rows28)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows28));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows28), new ObservableCollection<Models.Form28>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows28));
                    return (ObservableCollection<Models.Form28>)tmp;
                }
                else
                {
                    return _Rows28_Not_Valid;
                }
            }
            set
            {
                _Rows28_Not_Valid = value;
                if (GetErrors(nameof(Rows28)) == null)
                {
                    _dataAccess.Set(nameof(Rows28), _Rows28_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows28));
            }
        }
        private ObservableCollection<Models.Form28> _Rows28_Not_Valid = new ObservableCollection<Models.Form28>();
        private bool Rows28_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form29> Rows29
        {
            get
            {
                if (GetErrors(nameof(Rows29)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows29));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows29), new ObservableCollection<Models.Form29>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows29));
                    return (ObservableCollection<Models.Form29>)tmp;
                }
                else
                {
                    return _Rows29_Not_Valid;
                }
            }
            set
            {
                _Rows29_Not_Valid = value;
                if (GetErrors(nameof(Rows29)) == null)
                {
                    _dataAccess.Set(nameof(Rows29), _Rows29_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows29));
            }
        }
        private ObservableCollection<Models.Form29> _Rows29_Not_Valid = new ObservableCollection<Models.Form29>();
        private bool Rows29_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form210> Rows210
        {
            get
            {
                if (GetErrors(nameof(Rows210)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows210));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows210), new ObservableCollection<Models.Form210>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows210));
                    return (ObservableCollection<Models.Form210>)tmp;
                }
                else
                {
                    return _Rows210_Not_Valid;
                }
            }
            set
            {
                _Rows210_Not_Valid = value;
                if (GetErrors(nameof(Rows210)) == null)
                {
                    _dataAccess.Set(nameof(Rows210), _Rows210_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows210));
            }
        }
        private ObservableCollection<Models.Form210> _Rows210_Not_Valid = new ObservableCollection<Models.Form210>();
        private bool Rows210_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form211> Rows211
        {
            get
            {
                if (GetErrors(nameof(Rows211)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows211));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows211), new ObservableCollection<Models.Form211>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows211));
                    return (ObservableCollection<Models.Form211>)tmp;
                }
                else
                {
                    return _Rows211_Not_Valid;
                }
            }
            set
            {
                _Rows211_Not_Valid = value;
                if (GetErrors(nameof(Rows211)) == null)
                {
                    _dataAccess.Set(nameof(Rows211), _Rows211_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows211));
            }
        }
        private ObservableCollection<Models.Form211> _Rows211_Not_Valid = new ObservableCollection<Models.Form211>();
        private bool Rows211_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form212> Rows212
        {
            get
            {
                if (GetErrors(nameof(Rows212)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows212));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows212), new ObservableCollection<Models.Form212>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows212));
                    return (ObservableCollection<Models.Form212>)tmp;
                }
                else
                {
                    return _Rows212_Not_Valid;
                }
            }
            set
            {
                _Rows212_Not_Valid = value;
                if (GetErrors(nameof(Rows212)) == null)
                {
                    _dataAccess.Set(nameof(Rows212), _Rows212_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows212));
            }
        }
        private ObservableCollection<Models.Form212> _Rows212_Not_Valid = new ObservableCollection<Models.Form212>();
        private bool Rows212_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form30> Rows30
        {
            get
            {
                if (GetErrors(nameof(Rows30)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows30));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows30), new ObservableCollection<Models.Form30>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows30));
                    return (ObservableCollection<Models.Form30>)tmp;
                }
                else
                {
                    return _Rows30_Not_Valid;
                }
            }
            set
            {
                _Rows30_Not_Valid = value;
                if (GetErrors(nameof(Rows30)) == null)
                {
                    _dataAccess.Set(nameof(Rows30), _Rows30_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows30));
            }
        }
        private ObservableCollection<Models.Form30> _Rows30_Not_Valid = new ObservableCollection<Models.Form30>();
        private bool Rows30_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form31> Rows31
        {
            get
            {
                if (GetErrors(nameof(Rows31)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows31));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows31), new ObservableCollection<Models.Form31>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows31));
                    return (ObservableCollection<Models.Form31>)tmp;
                }
                else
                {
                    return _Rows31_Not_Valid;
                }
            }
            set
            {
                _Rows31_Not_Valid = value;
                if (GetErrors(nameof(Rows31)) == null)
                {
                    _dataAccess.Set(nameof(Rows31), _Rows31_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows31));
            }
        }
        private ObservableCollection<Models.Form31> _Rows31_Not_Valid = new ObservableCollection<Models.Form31>();
        private bool Rows31_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form31_1> Rows31_1
        {
            get
            {
                if (GetErrors(nameof(Rows31_1)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows31_1));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows31_1), new ObservableCollection<Models.Form31_1>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows31_1));
                    return (ObservableCollection<Models.Form31_1>)tmp;
                }
                else
                {
                    return _Rows31_1_Not_Valid;
                }
            }
            set
            {
                _Rows31_1_Not_Valid = value;
                if (GetErrors(nameof(Rows31_1)) == null)
                {
                    _dataAccess.Set(nameof(Rows31_1), _Rows31_1_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows31_1));
            }
        }
        private ObservableCollection<Models.Form31_1> _Rows31_1_Not_Valid = new ObservableCollection<Models.Form31_1>();
        private bool Rows31_1_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form32> Rows32
        {
            get
            {
                if (GetErrors(nameof(Rows32)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows32));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows32), new ObservableCollection<Models.Form32>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows32));
                    return (ObservableCollection<Models.Form32>)tmp;
                }
                else
                {
                    return _Rows32_Not_Valid;
                }
            }
            set
            {
                _Rows32_Not_Valid = value;
                if (GetErrors(nameof(Rows32)) == null)
                {
                    _dataAccess.Set(nameof(Rows32), _Rows32_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows32));
            }
        }
        private ObservableCollection<Models.Form32> _Rows32_Not_Valid = new ObservableCollection<Models.Form32>();
        private bool Rows32_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form32_1> Rows32_1
        {
            get
            {
                if (GetErrors(nameof(Rows32_1)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows32_1));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows32_1), new ObservableCollection<Models.Form32_1>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows32_1));
                    return (ObservableCollection<Models.Form32_1>)tmp;
                }
                else
                {
                    return _Rows32_1_Not_Valid;
                }
            }
            set
            {
                _Rows32_1_Not_Valid = value;
                if (GetErrors(nameof(Rows32_1)) == null)
                {
                    _dataAccess.Set(nameof(Rows32_1), _Rows32_1_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows32_1));
            }
        }
        private ObservableCollection<Models.Form32_1> _Rows32_1_Not_Valid = new ObservableCollection<Models.Form32_1>();
        private bool Rows32_1_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form32_2> Rows32_2
        {
            get
            {
                if (GetErrors(nameof(Rows32_2)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows32_2));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows32_2), new ObservableCollection<Models.Form32_2>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows32_2));
                    return (ObservableCollection<Models.Form32_2>)tmp;
                }
                else
                {
                    return _Rows32_2_Not_Valid;
                }
            }
            set
            {
                _Rows32_2_Not_Valid = value;
                if (GetErrors(nameof(Rows32_2)) == null)
                {
                    _dataAccess.Set(nameof(Rows32_2), _Rows32_2_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows32_2));
            }
        }
        private ObservableCollection<Models.Form32_2> _Rows32_2_Not_Valid = new ObservableCollection<Models.Form32_2>();
        private bool Rows32_2_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form32_3> Rows32_3
        {
            get
            {
                if (GetErrors(nameof(Rows32_3)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows32_3));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows32_3), new ObservableCollection<Models.Form32_3>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows32_3));
                    return (ObservableCollection<Models.Form32_3>)tmp;
                }
                else
                {
                    return _Rows32_3_Not_Valid;
                }
            }
            set
            {
                _Rows32_3_Not_Valid = value;
                if (GetErrors(nameof(Rows32_3)) == null)
                {
                    _dataAccess.Set(nameof(Rows32_3), _Rows32_3_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows32_3));
            }
        }
        private ObservableCollection<Models.Form32_3> _Rows32_3_Not_Valid = new ObservableCollection<Models.Form32_3>();
        private bool Rows32_3_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form40> Rows40
        {
            get
            {
                if (GetErrors(nameof(Rows40)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows40));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows40), new ObservableCollection<Models.Form40>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows40));
                    return (ObservableCollection<Models.Form40>)tmp;
                }
                else
                {
                    return _Rows40_Not_Valid;
                }
            }
            set
            {
                _Rows40_Not_Valid = value;
                if (GetErrors(nameof(Rows40)) == null)
                {
                    _dataAccess.Set(nameof(Rows40), _Rows40_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows40));
            }
        }
        private ObservableCollection<Models.Form40> _Rows40_Not_Valid = new ObservableCollection<Models.Form40>();
        private bool Rows40_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form41> Rows41
        {
            get
            {
                if (GetErrors(nameof(Rows41)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows41));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows41), new ObservableCollection<Models.Form41>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows41));
                    return (ObservableCollection<Models.Form41>)tmp;
                }
                else
                {
                    return _Rows41_Not_Valid;
                }
            }
            set
            {
                _Rows41_Not_Valid = value;
                if (GetErrors(nameof(Rows41)) == null)
                {
                    _dataAccess.Set(nameof(Rows41), _Rows41_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows41));
            }
        }
        private ObservableCollection<Models.Form41> _Rows41_Not_Valid = new ObservableCollection<Models.Form41>();
        private bool Rows41_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form50> Rows50
        {
            get
            {
                if (GetErrors(nameof(Rows50)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows50));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows50), new ObservableCollection<Models.Form50>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows50));
                    return (ObservableCollection<Models.Form50>)tmp;
                }
                else
                {
                    return _Rows50_Not_Valid;
                }
            }
            set
            {
                _Rows50_Not_Valid = value;
                if (GetErrors(nameof(Rows50)) == null)
                {
                    _dataAccess.Set(nameof(Rows50), _Rows50_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows50));
            }
        }
        private ObservableCollection<Models.Form50> _Rows50_Not_Valid = new ObservableCollection<Models.Form50>();
        private bool Rows50_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form51> Rows51
        {
            get
            {
                if (GetErrors(nameof(Rows51)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows51));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows51), new ObservableCollection<Models.Form51>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows51));
                    return (ObservableCollection<Models.Form51>)tmp;
                }
                else
                {
                    return _Rows51_Not_Valid;
                }
            }
            set
            {
                _Rows51_Not_Valid = value;
                if (GetErrors(nameof(Rows51)) == null)
                {
                    _dataAccess.Set(nameof(Rows51), _Rows51_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows51));
            }
        }
        private ObservableCollection<Models.Form51> _Rows51_Not_Valid = new ObservableCollection<Models.Form51>();
        private bool Rows51_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form52> Rows52
        {
            get
            {
                if (GetErrors(nameof(Rows52)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows52));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows52), new ObservableCollection<Models.Form52>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows52));
                    return (ObservableCollection<Models.Form52>)tmp;
                }
                else
                {
                    return _Rows52_Not_Valid;
                }
            }
            set
            {
                _Rows52_Not_Valid = value;
                if (GetErrors(nameof(Rows52)) == null)
                {
                    _dataAccess.Set(nameof(Rows52), _Rows52_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows52));
            }
        }
        private ObservableCollection<Models.Form52> _Rows52_Not_Valid = new ObservableCollection<Models.Form52>();
        private bool Rows52_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form53> Rows53
        {
            get
            {
                if (GetErrors(nameof(Rows53)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows53));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows53), new ObservableCollection<Models.Form53>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows53));
                    return (ObservableCollection<Models.Form53>)tmp;
                }
                else
                {
                    return _Rows53_Not_Valid;
                }
            }
            set
            {
                _Rows53_Not_Valid = value;
                if (GetErrors(nameof(Rows53)) == null)
                {
                    _dataAccess.Set(nameof(Rows53), _Rows53_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows53));
            }
        }
        private ObservableCollection<Models.Form53> _Rows53_Not_Valid = new ObservableCollection<Models.Form53>();
        private bool Rows53_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form54> Rows54
        {
            get
            {
                if (GetErrors(nameof(Rows54)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows54));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows54), new ObservableCollection<Models.Form54>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows54));
                    return (ObservableCollection<Models.Form54>)tmp;
                }
                else
                {
                    return _Rows54_Not_Valid;
                }
            }
            set
            {
                _Rows54_Not_Valid = value;
                if (GetErrors(nameof(Rows54)) == null)
                {
                    _dataAccess.Set(nameof(Rows54), _Rows54_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows54));
            }
        }
        private ObservableCollection<Models.Form54> _Rows54_Not_Valid = new ObservableCollection<Models.Form54>();
        private bool Rows54_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form55> Rows55
        {
            get
            {
                if (GetErrors(nameof(Rows55)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows55));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows55), new ObservableCollection<Models.Form55>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows55));
                    return (ObservableCollection<Models.Form55>)tmp;
                }
                else
                {
                    return _Rows55_Not_Valid;
                }
            }
            set
            {
                _Rows55_Not_Valid = value;
                if (GetErrors(nameof(Rows55)) == null)
                {
                    _dataAccess.Set(nameof(Rows55), _Rows55_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows55));
            }
        }
        private ObservableCollection<Models.Form55> _Rows55_Not_Valid = new ObservableCollection<Models.Form55>();
        private bool Rows55_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form56> Rows56
        {
            get
            {
                if (GetErrors(nameof(Rows56)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows56));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows56), new ObservableCollection<Models.Form56>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows56));
                    return (ObservableCollection<Models.Form56>)tmp;
                }
                else
                {
                    return _Rows56_Not_Valid;
                }
            }
            set
            {
                _Rows56_Not_Valid = value;
                if (GetErrors(nameof(Rows56)) == null)
                {
                    _dataAccess.Set(nameof(Rows56), _Rows56_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows56));
            }
        }
        private ObservableCollection<Models.Form56> _Rows56_Not_Valid = new ObservableCollection<Models.Form56>();
        private bool Rows56_Validation()
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form57> Rows57
        {
            get
            {
                if (GetErrors(nameof(Rows57)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows57));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows57), new ObservableCollection<Models.Form57>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows57));
                    return (ObservableCollection<Models.Form57>)tmp;
                }
                else
                {
                    return _Rows57_Not_Valid;
                }
            }
            set
            {
                _Rows57_Not_Valid = value;
                if (GetErrors(nameof(Rows57)) == null)
                {
                    _dataAccess.Set(nameof(Rows57), _Rows57_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows57));
            }
        }
        private ObservableCollection<Models.Form57> _Rows57_Not_Valid = new ObservableCollection<Models.Form57>();
        private bool Rows57_Validation()
        {
            return true;
        }

        [Form_Property("Форма")]
        public string FormNum 
        {
            get
            {
                if (Rows10.Count > 0) { return "10"; }
                if (Rows11.Count > 0) { return "11"; }
                if (Rows12.Count > 0) { return "12"; }
                if (Rows13.Count > 0) { return "13"; }
                if (Rows14.Count > 0) { return "14"; }
                if (Rows15.Count > 0) { return "15"; }
                if (Rows16.Count > 0) { return "16"; }
                if (Rows17.Count > 0) { return "17"; }
                if (Rows18.Count > 0) { return "18"; }
                if (Rows19.Count > 0) { return "19"; }

                if (Rows20.Count > 0) { return "20"; }
                if (Rows21.Count > 0) { return "21"; }
                if (Rows22.Count > 0) { return "22"; }
                if (Rows23.Count > 0) { return "23"; }
                if (Rows24.Count > 0) { return "24"; }
                if (Rows25.Count > 0) { return "25"; }
                if (Rows26.Count > 0) { return "26"; }
                if (Rows27.Count > 0) { return "27"; }
                if (Rows28.Count > 0) { return "28"; }
                if (Rows29.Count > 0) { return "29"; }
                if (Rows210.Count > 0) { return "210"; }
                if (Rows211.Count > 0) { return "211"; }
                if (Rows212.Count > 0) { return "212"; }

                if (Rows30.Count > 0) { return "30"; }
                if (Rows31.Count > 0) { return "31"; }
                if (Rows31_1.Count > 0) { return "31_1"; }
                if (Rows32.Count > 0) { return "32"; }
                if (Rows32_1.Count > 0) { return "32_1"; }
                if (Rows32_2.Count > 0) { return "32_2"; }
                if (Rows32_3.Count > 0) { return "32_3"; }

                if (Rows40.Count > 0) { return "40"; }
                if (Rows41.Count > 0) { return "41"; }

                if (Rows50.Count > 0) { return "50"; }
                if (Rows51.Count > 0) { return "51"; }
                if (Rows52.Count > 0) { return "52"; }
                if (Rows53.Count > 0) { return "53"; }
                if (Rows54.Count > 0) { return "54"; }
                if (Rows55.Count > 0) { return "55"; }
                if (Rows56.Count > 0) { return "56"; }
                if (Rows57.Count > 0) { return "57"; }
                return "0";
            }
        }

        //IsCorrection 
        [Form_Property("Корректирующий отчет")]
        public bool IsCorrection
        {
            get
            {
                if (GetErrors(nameof(IsCorrection)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(IsCorrection));
                    return tmp != null ? (bool)tmp : false;
                }
                else
                {
                    return _IsCorrection_Not_Valid;
                }
            }
            set
            {
                _IsCorrection_Not_Valid = value;
                if (GetErrors(nameof(IsCorrection)) == null)
                {
                    _dataAccess.Set(nameof(IsCorrection), _IsCorrection_Not_Valid);
                }
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        private bool _IsCorrection_Not_Valid = false;
        private bool IsCorrection_Validation()
        {
            return true;
        }
        //IsCorrection

        //CorrectionNumber property
        [Form_Property("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CorrectionNumber));
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) == null)
                {
                    _dataAccess.Set(nameof(CorrectionNumber), _CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

        //NumberInOrder property
        [Form_Property("Номер")]
        public string NumberInOrder
        {
            get
            {
                if (GetErrors(nameof(NumberInOrder)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(NumberInOrder));
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _NumberInOrder_Not_Valid;
                }
            }
            set
            {
                _NumberInOrder_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) == null)
                {
                    _dataAccess.Set(nameof(NumberInOrder), _NumberInOrder_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private string _NumberInOrder_Not_Valid = "";
        private bool NumberInOrder_Validation()
        {
            return true;
        }
        //NumberInOrder property

        //Comments property
        [Form_Property("Комментарий")]
        public string Comments
        {
            get
            {
                if (GetErrors(nameof(Comments)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Comments));
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Comments_Not_Valid;
                }
            }
            set
            {
                _Comments_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) == null)
                {
                    _dataAccess.Set(nameof(Comments), _Comments_Not_Valid);
                }
                OnPropertyChanged(nameof(Comments));
            }
        }
        private string _Comments_Not_Valid = "";
        private bool Comments_Validation()
        {
            return true;
        }
        //Comments property

        //Notes property
        [Form_Property("Примечания")]
        public virtual ObservableCollection<Models.Note> Notes
        {
            get
            {
                if (GetErrors(nameof(Notes)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Notes));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Notes), new ObservableCollection<Models.Note>());
                    }
                    tmp = _dataAccess.Get(nameof(Notes));
                    return (ObservableCollection<Models.Note>)tmp;
                }
                else
                {
                    return _Notes_Not_Valid;
                }
            }
            set
            {
                _Notes_Not_Valid = value;
                if (GetErrors(nameof(Notes)) == null)
                {
                    _dataAccess.Set(nameof(Notes), _Notes_Not_Valid);
                }
                OnPropertyChanged(nameof(Notes));
            }
        }
        private ObservableCollection<Models.Note> _Notes_Not_Valid = new ObservableCollection<Models.Note>();
        private bool Notes_Validation()
        {
            return true;
        }
        //Notes property

        //StartPeriod
        [Form_Property("Начало")]
        public DateTimeOffset StartPeriod
        {
            get
            {
                if (GetErrors(nameof(StartPeriod)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(StartPeriod));
                    return tmp != null ? (DateTimeOffset)tmp : DateTimeOffset.MinValue;
                }
                else
                {
                    return _StartPeriod_Not_Valid;
                }
            }
            set
            {
                _StartPeriod_Not_Valid = value;
                if (GetErrors(nameof(StartPeriod)) == null)
                {
                    _dataAccess.Set(nameof(StartPeriod), _StartPeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private DateTimeOffset _StartPeriod_Not_Valid = DateTimeOffset.Now;
        private void StartPeriod_Validation()
        {
            ClearErrors(nameof(StartPeriod));
            if (_StartPeriod_Not_Valid.Equals(DateTimeOffset.MinValue))
                AddError(nameof(StartPeriod), "Не заполнено начало периода");
            else
                if (!EndPeriod.Equals(DateTimeOffset.MinValue))
                if (_StartPeriod_Not_Valid.CompareTo(_EndPeriod_Not_Valid) > 0)
                    AddError(nameof(StartPeriod), "Начало периода не может быть позже его конца");
        }
        //StartPeriod

        //EndPeriod
        [Form_Property("Конец")]
        public DateTimeOffset EndPeriod
        {
            get
            {
                if (GetErrors(nameof(EndPeriod)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(EndPeriod));
                    return tmp != null ? (DateTimeOffset)tmp : DateTimeOffset.MinValue;
                }
                else
                {
                    return _EndPeriod_Not_Valid;
                }
            }
            set
            {
                _EndPeriod_Not_Valid = value;
                if (GetErrors(nameof(EndPeriod)) == null)
                {
                    _dataAccess.Set(nameof(EndPeriod), _EndPeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        private DateTimeOffset _EndPeriod_Not_Valid = DateTimeOffset.Now;
        private void EndPeriod_Validation()
        {
            ClearErrors(nameof(EndPeriod));
            if (_EndPeriod_Not_Valid.Equals(DateTimeOffset.MinValue))
                AddError(nameof(EndPeriod), "Не заполнено начало периода");
            else
                if (!EndPeriod.Equals(DateTimeOffset.MinValue))
                if (_StartPeriod_Not_Valid.CompareTo(_EndPeriod_Not_Valid) > 0)
                    AddError(nameof(EndPeriod), "Начало периода не может быть позже его конца");
        }
        //EndPeriod

        //ExportDate
        [Form_Property("Дата выгрузки")]
        public DateTimeOffset ExportDate
        {
            get
            {
                if (GetErrors(nameof(ExportDate)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ExportDate));
                    return tmp != null ? (DateTimeOffset)tmp : DateTimeOffset.MinValue;
                }
                else
                {
                    return _ExportDate_Not_Valid;
                }
            }
            set
            {
                _ExportDate_Not_Valid = value;
                if (GetErrors(nameof(ExportDate)) == null)
                {
                    _dataAccess.Set(nameof(ExportDate), _ExportDate_Not_Valid);
                }
                OnPropertyChanged(nameof(ExportDate));
            }
        }
        private DateTimeOffset _ExportDate_Not_Valid = DateTimeOffset.MinValue;
        private bool ExportDate_Validation()
        {
            return true;
        }
        //ExportDate

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed

        //Data Validation
        protected readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => _errorsByPropertyName.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            var tmp = _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
            if (tmp != null)
            {
                List<Exception> lst = new List<Exception>();
                foreach (var item in tmp)
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
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        //Data Validation
    }
}
