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
    public class Report : INotifyPropertyChanged
    {
        IDataAccessCollection _dataAccess { get; set; }

        public Report(IDataAccessCollection Access)
        {
            _dataAccess = Access;
            Init();
        }

        public Report()
        {
            _dataAccess = new DataAccessCollection();
            Init();
        }
        void Init()
        {
            _dataAccess.Init<string>(nameof(FormNum), FormNum_Validation, "");

            _dataAccess.Init<ObservableCollection<Models.Form10>>(nameof(Rows10), Rows10_Validation, null);
            Rows10 = new ObservableCollection<Models.Form10>();
            Rows10.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form11>>(nameof(Rows11), Rows11_Validation, null);
            Rows11 = new ObservableCollection<Models.Form11>();
            Rows11.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form12>>(nameof(Rows12), Rows12_Validation, null);
            Rows12 = new ObservableCollection<Models.Form12>();
            Rows12.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form13>>(nameof(Rows13), Rows13_Validation, null);
            Rows13 = new ObservableCollection<Models.Form13>();
            Rows13.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form14>>(nameof(Rows14), Rows14_Validation, null);
            Rows14 = new ObservableCollection<Models.Form14>();
            Rows14.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form15>>(nameof(Rows15), Rows15_Validation, null);
            Rows15 = new ObservableCollection<Models.Form15>();
            Rows15.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form16>>(nameof(Rows16), Rows16_Validation, null);
            Rows16 = new ObservableCollection<Models.Form16>();
            Rows16.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form17>>(nameof(Rows17), Rows17_Validation, null);
            Rows17 = new ObservableCollection<Models.Form17>();
            Rows17.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form18>>(nameof(Rows18), Rows18_Validation, null);
            Rows18 = new ObservableCollection<Models.Form18>();
            Rows18.CollectionChanged += CollectionChanged;
            _dataAccess.Init<ObservableCollection<Models.Form19>>(nameof(Rows19), Rows19_Validation, null);
            Rows19 = new ObservableCollection<Models.Form19>();
            Rows19.CollectionChanged += CollectionChanged;


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
                return _dataAccess.Get<ObservableCollection<Models.Form10>>(nameof(Rows10)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form10>>(nameof(Rows10)).Value=value;
                OnPropertyChanged(nameof(Rows10));
            }
        }
        private bool Rows10_Validation(RamAccess<ObservableCollection<Models.Form10>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form11> Rows11
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form11>>(nameof(Rows11)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form11>>(nameof(Rows11)).Value = value;
                OnPropertyChanged(nameof(Rows11));
            }
        }
        private bool Rows11_Validation(RamAccess<ObservableCollection<Models.Form11>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form12> Rows12
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form12>>(nameof(Rows12)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form12>>(nameof(Rows12)).Value=value;
                OnPropertyChanged(nameof(Rows12));
            }
        }
        private bool Rows12_Validation(RamAccess<ObservableCollection<Models.Form12>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form13> Rows13
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form13>>(nameof(Rows13)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form13>>(nameof(Rows13)).Value=value;
                OnPropertyChanged(nameof(Rows13));
            }
        }
        private bool Rows13_Validation(RamAccess<ObservableCollection<Models.Form13>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form14> Rows14
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form14>>(nameof(Rows14)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form14>>(nameof(Rows14)).Value=value;
                OnPropertyChanged(nameof(Rows14));
            }
        }
        private bool Rows14_Validation(RamAccess<ObservableCollection<Models.Form14>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form15> Rows15
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form15>>(nameof(Rows15)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form15>>(nameof(Rows15)).Value=value;
                OnPropertyChanged(nameof(Rows15));
            }
        }
        private bool Rows15_Validation(RamAccess<ObservableCollection<Models.Form15>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form16> Rows16
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form16>>(nameof(Rows16)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form16>>(nameof(Rows16)).Value=value;
                OnPropertyChanged(nameof(Rows16));
            }
        }
        private bool Rows16_Validation(RamAccess<ObservableCollection<Models.Form16>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form17> Rows17
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form17>>(nameof(Rows17)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form17>>(nameof(Rows17)).Value=value;
                OnPropertyChanged(nameof(Rows17));
            }
        }
        private bool Rows17_Validation(RamAccess<ObservableCollection<Models.Form17>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form18> Rows18
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form18>>(nameof(Rows18)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form18>>(nameof(Rows18)).Value=value;
                OnPropertyChanged(nameof(Rows18));
            }
        }
        private bool Rows18_Validation(RamAccess<ObservableCollection<Models.Form18>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form19> Rows19
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form19>>(nameof(Rows19)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form19>>(nameof(Rows19)).Value=value;
                OnPropertyChanged(nameof(Rows19));
            }
        }
        private bool Rows19_Validation(RamAccess<ObservableCollection<Models.Form19>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form20> Rows20
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form20>>(nameof(Rows20)).Value=value;
                OnPropertyChanged(nameof(Rows20));
            }
        }
        private bool Rows20_Validation(RamAccess<ObservableCollection<Models.Form20>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form21> Rows21
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form21>>(nameof(Rows21)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form21>>(nameof(Rows21)).Value=value;
                OnPropertyChanged(nameof(Rows21));
            }
        }
        private bool Rows21_Validation(RamAccess<ObservableCollection<Models.Form21>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form22> Rows22
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form22>>(nameof(Rows22)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form22>>(nameof(Rows22)).Value=value;
                OnPropertyChanged(nameof(Rows22));
            }
        }
        private bool Rows22_Validation(RamAccess<ObservableCollection<Models.Form22>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form23> Rows23
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form23>>(nameof(Rows23)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form23>>(nameof(Rows23)).Value=value;
                OnPropertyChanged(nameof(Rows23));
            }
        }
        private bool Rows23_Validation(RamAccess<ObservableCollection<Models.Form23>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form24> Rows24
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form24>>(nameof(Rows24)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form24>>(nameof(Rows24)).Value=value;
                OnPropertyChanged(nameof(Rows24));
            }
        }
        private bool Rows24_Validation(RamAccess<ObservableCollection<Models.Form24>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form25> Rows25
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form25>>(nameof(Rows25)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form25>>(nameof(Rows25)).Value=value;
                OnPropertyChanged(nameof(Rows25));
            }
        }
        private bool Rows25_Validation(RamAccess<ObservableCollection<Models.Form25>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form26> Rows26
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form26>>(nameof(Rows26)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form26>>(nameof(Rows26)).Value=value;
                OnPropertyChanged(nameof(Rows26));
            }
        }
        private bool Rows26_Validation(RamAccess<ObservableCollection<Models.Form26>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form27> Rows27
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form27>>(nameof(Rows27)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form27>>(nameof(Rows27)).Value=value;
                OnPropertyChanged(nameof(Rows27));
            }
        }
        private bool Rows27_Validation(RamAccess<ObservableCollection<Models.Form27>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form28> Rows28
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form28>>(nameof(Rows28)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form28>>(nameof(Rows28)).Value=value;
                OnPropertyChanged(nameof(Rows28));
            }
        }
        private bool Rows28_Validation(RamAccess<ObservableCollection<Models.Form28>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form29> Rows29
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form29>>(nameof(Rows29)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form29>>(nameof(Rows29)).Value=value;
                OnPropertyChanged(nameof(Rows29));
            }
        }
        private bool Rows29_Validation(RamAccess<ObservableCollection<Models.Form29>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form210> Rows210
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form210>>(nameof(Rows210)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form210>>(nameof(Rows210)).Value=value;
                OnPropertyChanged(nameof(Rows210));
            }
        }
        private bool Rows210_Validation(RamAccess<ObservableCollection<Models.Form210>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form211> Rows211
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form211>>(nameof(Rows211)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form211>>(nameof(Rows211)).Value=value;
                OnPropertyChanged(nameof(Rows211));
            }
        }
        private bool Rows211_Validation(RamAccess<ObservableCollection<Models.Form211>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form212> Rows212
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form212>>(nameof(Rows212)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form212>>(nameof(Rows212)).Value=value;
                OnPropertyChanged(nameof(Rows212));
            }
        }
        private bool Rows212_Validation(RamAccess<ObservableCollection<Models.Form212>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form30> Rows30
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form30>>(nameof(Rows30)).Value=value;
                OnPropertyChanged(nameof(Rows30));
            }
        }
        private bool Rows30_Validation(RamAccess<ObservableCollection<Models.Form30>> value)
        {
            return true;
        }


        public virtual ObservableCollection<Models.Form31> Rows31
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form31>>(nameof(Rows31)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form31>>(nameof(Rows31)).Value=value;
                OnPropertyChanged(nameof(Rows31));
            }
        }
        private bool Rows31_Validation(RamAccess<ObservableCollection<Models.Form31>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form31_1> Rows31_1
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form31_1>>(nameof(Rows31_1)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form31_1>>(nameof(Rows31_1)).Value=value;
                OnPropertyChanged(nameof(Rows31_1));
            }
        }
        private bool Rows31_1_Validation(RamAccess<ObservableCollection<Models.Form31_1>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form32> Rows32
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form32>>(nameof(Rows32)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form32>>(nameof(Rows32)).Value=value;
                OnPropertyChanged(nameof(Rows32));
            }
        }
        private bool Rows32_Validation(RamAccess<ObservableCollection<Models.Form32>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form32_1> Rows32_1
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form32_1>>(nameof(Rows32_1)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form32_1>>(nameof(Rows32_1)).Value=value;
                OnPropertyChanged(nameof(Rows32_1));
            }
        }
        private bool Rows32_1_Validation(RamAccess<ObservableCollection<Models.Form32_1>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form32_2> Rows32_2
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form32_2>>(nameof(Rows32_2)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form32_2>>(nameof(Rows32_2)).Value=value;
                OnPropertyChanged(nameof(Rows32_2));
            }
        }
        private bool Rows32_2_Validation(RamAccess<ObservableCollection<Models.Form32_2>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form32_3> Rows32_3
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form32_3>>(nameof(Rows32_3)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form32_3>>(nameof(Rows32_3)).Value=value;
                OnPropertyChanged(nameof(Rows32_3));
            }
        }
        private bool Rows32_3_Validation(RamAccess<ObservableCollection<Models.Form32_3>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form40> Rows40
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form40>>(nameof(Rows40)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form40>>(nameof(Rows40)).Value=value;
                OnPropertyChanged(nameof(Rows40));
            }
        }
        private bool Rows40_Validation(RamAccess<ObservableCollection<Models.Form40>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form41> Rows41
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form41>>(nameof(Rows41)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form41>>(nameof(Rows41)).Value=value;
                OnPropertyChanged(nameof(Rows41));
            }
        }
        private bool Rows41_Validation(RamAccess<ObservableCollection<Models.Form41>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form50> Rows50
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form50>>(nameof(Rows50)).Value=value;
                OnPropertyChanged(nameof(Rows50));
            }
        }
        private bool Rows50_Validation(RamAccess<ObservableCollection<Models.Form50>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form51> Rows51
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form51>>(nameof(Rows51)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form51>>(nameof(Rows51)).Value=value;
                OnPropertyChanged(nameof(Rows51));
            }
        }
        private bool Rows51_Validation(RamAccess<ObservableCollection<Models.Form51>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form52> Rows52
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form52>>(nameof(Rows52)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form52>>(nameof(Rows52)).Value=value;
                OnPropertyChanged(nameof(Rows52));
            }
        }
        private bool Rows52_Validation(RamAccess<ObservableCollection<Models.Form52>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form53> Rows53
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form53>>(nameof(Rows53)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form53>>(nameof(Rows53)).Value=value;
                OnPropertyChanged(nameof(Rows53));
            }
        }
        private bool Rows53_Validation(RamAccess<ObservableCollection<Models.Form53>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form54> Rows54
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form54>>(nameof(Rows54)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form54>>(nameof(Rows54)).Value=value;
                OnPropertyChanged(nameof(Rows54));
            }
        }
        private bool Rows54_Validation(RamAccess<ObservableCollection<Models.Form54>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form55> Rows55
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form55>>(nameof(Rows55)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form55>>(nameof(Rows55)).Value=value;
                OnPropertyChanged(nameof(Rows55));
            }
        }
        private bool Rows55_Validation(RamAccess<ObservableCollection<Models.Form55>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form56> Rows56
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form56>>(nameof(Rows56)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form56>>(nameof(Rows56)).Value=value;
                OnPropertyChanged(nameof(Rows56));
            }
        }
        private bool Rows56_Validation(RamAccess<ObservableCollection<Models.Form56>> value)
        {
            return true;
        }

        public virtual ObservableCollection<Models.Form57> Rows57
        {
            get
            {
                return _dataAccess.Get<ObservableCollection<Models.Form57>>(nameof(Rows57)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Form57>>(nameof(Rows57)).Value=value;
                OnPropertyChanged(nameof(Rows57));
            }
        }
        private bool Rows57_Validation(RamAccess<ObservableCollection<Models.Form57>> value)
        {
            return true;
        }

        [Form_Property("Форма")]
        public RamAccess<string> FormNum
        {
            get
            {
                return _dataAccess.Get<string>(nameof(FormNum));
            }
            set
            {
                _dataAccess.Set(nameof(FormNum), value);
                OnPropertyChanged(nameof(FormNum));
            }
        }
        public bool FormNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
            return true;
        }

        //IsCorrection 
        [Form_Property("Корректирующий отчет")]
        public RamAccess<bool> IsCorrection
        {
            get
            {
                return _dataAccess.Get<bool>(nameof(IsCorrection));
            }
            set
            {
                _dataAccess.Set(nameof(IsCorrection), value);
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        private bool IsCorrection_Validation(RamAccess<bool> value)
        {
            return true;
        }
        //IsCorrection

        //CorrectionNumber property
        [Form_Property("Номер корректировки")]
        public RamAccess<byte> CorrectionNumber
        {
            get
            {
                return _dataAccess.Get<byte>(nameof(CorrectionNumber));
            }
            set
            {
                _dataAccess.Set(nameof(CorrectionNumber), value);
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors(); return true;
        }
        //CorrectionNumber property

        //NumberInOrder property
        [Form_Property("Номер")]
        public RamAccess<string> NumberInOrder
        {
            get
            {
                return _dataAccess.Get<string>(nameof(NumberInOrder));
            }
            set
            {
                _dataAccess.Set(nameof(NumberInOrder), value);
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private bool NumberInOrder_Validation(RamAccess<string> value)
        {
            return true;
        }
        //NumberInOrder property

        //Comments property
        [Form_Property("Комментарий")]
        public RamAccess<string> Comments
        {
            get
            {
                return _dataAccess.Get<string>(nameof(Comments));
            }
            set
            {
                _dataAccess.Set(nameof(Comments), value);
                OnPropertyChanged(nameof(Comments));
            }
        }
        private bool Comments_Validation(RamAccess<string> value)
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
                return _dataAccess.Get<ObservableCollection<Models.Note>>(nameof(Notes)).Value;
            }
            set
            {
                _dataAccess.Get<ObservableCollection<Models.Note>>(nameof(Notes)).Value=value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        private bool Notes_Validation(RamAccess<ObservableCollection<Models.Note>> value)
        {
            return true;
        }
        //Notes property

        //StartPeriod
        [Form_Property("Начало")]
        public RamAccess<string> StartPeriod
        {
            get
            {
                return _dataAccess.Get<string>(nameof(StartPeriod));
            }
            set
            {
                _dataAccess.Set(nameof(StartPeriod), value);
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private bool StartPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //StartPeriod

        //EndPeriod
        [Form_Property("Конец")]
        public RamAccess<string> EndPeriod
        {
            get
            {
                return _dataAccess.Get<string>(nameof(EndPeriod));
            }
            set
            {
                _dataAccess.Set(nameof(EndPeriod), value);
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        private bool EndPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //EndPeriod

        //ExportDate
        [Form_Property("Дата выгрузки")]
        public RamAccess<string> ExportDate
        {
            get
            {
                return _dataAccess.Get<string>(nameof(ExportDate));
            }
            set
            {
                _dataAccess.Set(nameof(ExportDate), value);
                OnPropertyChanged(nameof(ExportDate));
            }
        }
        private bool ExportDate_Validation(RamAccess<string> value)
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
    }
}
