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
using Collections;
using System.ComponentModel.DataAnnotations.Schema;

namespace Collections
{
    public class Report : IChanged,IKey
    {
        DataAccessCollection DataAccess { get; set; }

        public Report(DataAccessCollection Access)
        {
            DataAccess = Access;
            Init();
        }

        public Report()
        {
            DataAccess = new DataAccessCollection();
            Init();
        }
        void Init()
        {

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form10>>(nameof(Rows10), Rows10_Validation, null);
            Rows10 = new ObservableCollectionWithItemPropertyChanged<Models.Form10>();
            Rows10.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form11>>(nameof(Rows11), Rows11_Validation, null);
            Rows11 = new ObservableCollectionWithItemPropertyChanged<Models.Form11>();
            Rows11.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form12>>(nameof(Rows12), Rows12_Validation, null);
            Rows12 = new ObservableCollectionWithItemPropertyChanged<Models.Form12>();
            Rows12.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form13>>(nameof(Rows13), Rows13_Validation, null);
            Rows13 = new ObservableCollectionWithItemPropertyChanged<Models.Form13>();
            Rows13.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form14>>(nameof(Rows14), Rows14_Validation, null);
            Rows14 = new ObservableCollectionWithItemPropertyChanged<Models.Form14>();
            Rows14.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form15>>(nameof(Rows15), Rows15_Validation, null);
            Rows15 = new ObservableCollectionWithItemPropertyChanged<Models.Form15>();
            Rows15.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form16>>(nameof(Rows16), Rows16_Validation, null);
            Rows16 = new ObservableCollectionWithItemPropertyChanged<Models.Form16>();
            Rows16.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form17>>(nameof(Rows17), Rows17_Validation, null);
            Rows17 = new ObservableCollectionWithItemPropertyChanged<Models.Form17>();
            Rows17.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form18>>(nameof(Rows18), Rows18_Validation, null);
            Rows18 = new ObservableCollectionWithItemPropertyChanged<Models.Form18>();
            Rows18.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form19>>(nameof(Rows19), Rows19_Validation, null);
            Rows19 = new ObservableCollectionWithItemPropertyChanged<Models.Form19>();
            Rows19.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form20>>(nameof(Rows20), Rows20_Validation, null);
            Rows20 = new ObservableCollectionWithItemPropertyChanged<Models.Form20>();
            Rows20.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form21>>(nameof(Rows21), Rows21_Validation, null);
            Rows21 = new ObservableCollectionWithItemPropertyChanged<Models.Form21>();
            Rows21.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form22>>(nameof(Rows22), Rows22_Validation, null);
            Rows22 = new ObservableCollectionWithItemPropertyChanged<Models.Form22>();
            Rows22.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form23>>(nameof(Rows23), Rows23_Validation, null);
            Rows23 = new ObservableCollectionWithItemPropertyChanged<Models.Form23>();
            Rows23.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form24>>(nameof(Rows24), Rows24_Validation, null);
            Rows24 = new ObservableCollectionWithItemPropertyChanged<Models.Form24>();
            Rows24.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form25>>(nameof(Rows25), Rows25_Validation, null);
            Rows25 = new ObservableCollectionWithItemPropertyChanged<Models.Form25>();
            Rows25.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form26>>(nameof(Rows26), Rows26_Validation, null);
            Rows26 = new ObservableCollectionWithItemPropertyChanged<Models.Form26>();
            Rows26.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form27>>(nameof(Rows27), Rows27_Validation, null);
            Rows27 = new ObservableCollectionWithItemPropertyChanged<Models.Form27>();
            Rows27.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form28>>(nameof(Rows28), Rows28_Validation, null);
            Rows28 = new ObservableCollectionWithItemPropertyChanged<Models.Form28>();
            Rows28.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form29>>(nameof(Rows29), Rows29_Validation, null);
            Rows29 = new ObservableCollectionWithItemPropertyChanged<Models.Form29>();
            Rows29.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form210>>(nameof(Rows210), Rows210_Validation, null);
            Rows210 = new ObservableCollectionWithItemPropertyChanged<Models.Form210>();
            Rows210.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form211>>(nameof(Rows211), Rows211_Validation, null);
            Rows211 = new ObservableCollectionWithItemPropertyChanged<Models.Form211>();
            Rows211.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form212>>(nameof(Rows212), Rows212_Validation, null);
            Rows212 = new ObservableCollectionWithItemPropertyChanged<Models.Form212>();
            Rows212.CollectionChanged += CollectionChanged;

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form30>>(nameof(Rows30), Rows30_Validation, null);
            Rows30 = new ObservableCollectionWithItemPropertyChanged<Models.Form30>();
            Rows30.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form31>>(nameof(Rows31), Rows31_Validation, null);
            Rows31 = new ObservableCollectionWithItemPropertyChanged<Models.Form31>();
            Rows31.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form31_1>>(nameof(Rows31_1), Rows31_1_Validation, null);
            Rows31_1 = new ObservableCollectionWithItemPropertyChanged<Models.Form31_1>();
            Rows31_1.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form32>>(nameof(Rows32), Rows32_Validation, null);
            Rows32 = new ObservableCollectionWithItemPropertyChanged<Models.Form32>();
            Rows32.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form32_1>>(nameof(Rows32_1), Rows32_1_Validation, null);
            Rows32_1 = new ObservableCollectionWithItemPropertyChanged<Models.Form32_1>();
            Rows32_1.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form32_2>>(nameof(Rows32_2), Rows32_2_Validation, null);
            Rows32_2 = new ObservableCollectionWithItemPropertyChanged<Models.Form32_2>();
            Rows32_2.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form32_3>>(nameof(Rows32_3), Rows32_3_Validation, null);
            Rows32_3 = new ObservableCollectionWithItemPropertyChanged<Models.Form32_3>();
            Rows32_3.CollectionChanged += CollectionChanged;

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form40>>(nameof(Rows40), Rows40_Validation, null);
            Rows40 = new ObservableCollectionWithItemPropertyChanged<Models.Form40>();
            Rows40.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form41>>(nameof(Rows41), Rows41_Validation, null);
            Rows41 = new ObservableCollectionWithItemPropertyChanged<Models.Form41>();
            Rows41.CollectionChanged += CollectionChanged;

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form50>>(nameof(Rows50), Rows50_Validation, null);
            Rows50 = new ObservableCollectionWithItemPropertyChanged<Models.Form50>();
            Rows50.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form51>>(nameof(Rows51), Rows51_Validation, null);
            Rows51 = new ObservableCollectionWithItemPropertyChanged<Models.Form51>();
            Rows51.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form52>>(nameof(Rows52), Rows52_Validation, null);
            Rows52 = new ObservableCollectionWithItemPropertyChanged<Models.Form52>();
            Rows52.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form53>>(nameof(Rows53), Rows53_Validation, null);
            Rows53 = new ObservableCollectionWithItemPropertyChanged<Models.Form53>();
            Rows53.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form54>>(nameof(Rows54), Rows54_Validation, null);
            Rows54 = new ObservableCollectionWithItemPropertyChanged<Models.Form54>();
            Rows54.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form55>>(nameof(Rows55), Rows55_Validation, null);
            Rows55 = new ObservableCollectionWithItemPropertyChanged<Models.Form55>();
            Rows55.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form56>>(nameof(Rows56), Rows56_Validation, null);
            Rows56 = new ObservableCollectionWithItemPropertyChanged<Models.Form56>();
            Rows56.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Form57>>(nameof(Rows57), Rows57_Validation, null);
            Rows57 = new ObservableCollectionWithItemPropertyChanged<Models.Form57>();
            Rows57.CollectionChanged += CollectionChanged;

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Models.Note>>(nameof(Notes), Notes_Validation, null);
            Notes = new ObservableCollectionWithItemPropertyChanged<Models.Note>();
            //Notes.CollectionChanged += CollectionChanged;

            DataAccess.Init<string>(nameof(StartPeriod), StartPeriod_Validation, "");
            DataAccess.Init<string>(nameof(Comments), Comments_Validation, "");
            DataAccess.Init<byte>(nameof(CorrectionNumber), CorrectionNumber_Validation, 0);
            DataAccess.Init<bool>(nameof(IsCorrection), IsCorrection_Validation, false);
            DataAccess.Init<string>(nameof(NumberInOrder), NumberInOrder_Validation, "");
            DataAccess.Init<string>(nameof(EndPeriod), EndPeriod_Validation, "");
            DataAccess.Init<string>(nameof(ExportDate), ExportDate_Validation, "");
        }

        public bool Equals(object obj)
        {
            if (obj is Report)
            {
                var obj1 = this;
                var obj2 = obj as Report;

                return obj1.DataAccess == obj2.DataAccess;
            }
            else
            {
                return false;
            }
            return true;
        }

        public static bool operator ==(Report obj1, Report obj2)
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
        public static bool operator !=(Report obj1, Report obj2)
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


        protected void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
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

        public int Id { get; set; }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form10> Rows10
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form10>>(nameof(Rows10)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form10>>(nameof(Rows10)).Value=value;
                OnPropertyChanged(nameof(Rows10));
            }
        }
        private bool Rows10_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form10>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form11> Rows11
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form11>>(nameof(Rows11)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form11>>(nameof(Rows11)).Value = value;
                OnPropertyChanged(nameof(Rows11));
            }
        }
        private bool Rows11_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form11>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form12> Rows12
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form12>>(nameof(Rows12)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form12>>(nameof(Rows12)).Value=value;
                OnPropertyChanged(nameof(Rows12));
            }
        }
        private bool Rows12_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form12>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form13> Rows13
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form13>>(nameof(Rows13)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form13>>(nameof(Rows13)).Value=value;
                OnPropertyChanged(nameof(Rows13));
            }
        }
        private bool Rows13_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form13>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form14> Rows14
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form14>>(nameof(Rows14)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form14>>(nameof(Rows14)).Value=value;
                OnPropertyChanged(nameof(Rows14));
            }
        }
        private bool Rows14_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form14>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form15> Rows15
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form15>>(nameof(Rows15)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form15>>(nameof(Rows15)).Value=value;
                OnPropertyChanged(nameof(Rows15));
            }
        }
        private bool Rows15_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form15>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form16> Rows16
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form16>>(nameof(Rows16)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form16>>(nameof(Rows16)).Value=value;
                OnPropertyChanged(nameof(Rows16));
            }
        }
        private bool Rows16_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form16>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form17> Rows17
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form17>>(nameof(Rows17)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form17>>(nameof(Rows17)).Value=value;
                OnPropertyChanged(nameof(Rows17));
            }
        }
        private bool Rows17_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form17>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form18> Rows18
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form18>>(nameof(Rows18)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form18>>(nameof(Rows18)).Value=value;
                OnPropertyChanged(nameof(Rows18));
            }
        }
        private bool Rows18_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form18>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form19> Rows19
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form19>>(nameof(Rows19)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form19>>(nameof(Rows19)).Value=value;
                OnPropertyChanged(nameof(Rows19));
            }
        }
        private bool Rows19_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form19>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form20> Rows20
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form20>>(nameof(Rows20)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form20>>(nameof(Rows20)).Value=value;
                OnPropertyChanged(nameof(Rows20));
            }
        }
        private bool Rows20_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form20>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form21> Rows21
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form21>>(nameof(Rows21)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form21>>(nameof(Rows21)).Value=value;
                OnPropertyChanged(nameof(Rows21));
            }
        }
        private bool Rows21_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form21>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form22> Rows22
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form22>>(nameof(Rows22)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form22>>(nameof(Rows22)).Value=value;
                OnPropertyChanged(nameof(Rows22));
            }
        }
        private bool Rows22_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form22>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form23> Rows23
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form23>>(nameof(Rows23)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form23>>(nameof(Rows23)).Value=value;
                OnPropertyChanged(nameof(Rows23));
            }
        }
        private bool Rows23_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form23>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form24> Rows24
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form24>>(nameof(Rows24)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form24>>(nameof(Rows24)).Value=value;
                OnPropertyChanged(nameof(Rows24));
            }
        }
        private bool Rows24_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form24>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form25> Rows25
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form25>>(nameof(Rows25)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form25>>(nameof(Rows25)).Value=value;
                OnPropertyChanged(nameof(Rows25));
            }
        }
        private bool Rows25_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form25>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form26> Rows26
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form26>>(nameof(Rows26)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form26>>(nameof(Rows26)).Value=value;
                OnPropertyChanged(nameof(Rows26));
            }
        }
        private bool Rows26_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form26>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form27> Rows27
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form27>>(nameof(Rows27)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form27>>(nameof(Rows27)).Value=value;
                OnPropertyChanged(nameof(Rows27));
            }
        }
        private bool Rows27_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form27>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form28> Rows28
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form28>>(nameof(Rows28)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form28>>(nameof(Rows28)).Value=value;
                OnPropertyChanged(nameof(Rows28));
            }
        }
        private bool Rows28_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form28>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form29> Rows29
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form29>>(nameof(Rows29)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form29>>(nameof(Rows29)).Value=value;
                OnPropertyChanged(nameof(Rows29));
            }
        }
        private bool Rows29_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form29>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form210> Rows210
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form210>>(nameof(Rows210)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form210>>(nameof(Rows210)).Value=value;
                OnPropertyChanged(nameof(Rows210));
            }
        }
        private bool Rows210_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form210>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form211> Rows211
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form211>>(nameof(Rows211)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form211>>(nameof(Rows211)).Value=value;
                OnPropertyChanged(nameof(Rows211));
            }
        }
        private bool Rows211_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form211>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form212> Rows212
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form212>>(nameof(Rows212)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form212>>(nameof(Rows212)).Value=value;
                OnPropertyChanged(nameof(Rows212));
            }
        }
        private bool Rows212_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form212>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form30> Rows30
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form30>>(nameof(Rows30)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form30>>(nameof(Rows30)).Value=value;
                OnPropertyChanged(nameof(Rows30));
            }
        }
        private bool Rows30_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form30>> value)
        {
            return true;
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form31> Rows31
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form31>>(nameof(Rows31)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form31>>(nameof(Rows31)).Value=value;
                OnPropertyChanged(nameof(Rows31));
            }
        }
        private bool Rows31_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form31>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form31_1> Rows31_1
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form31_1>>(nameof(Rows31_1)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form31_1>>(nameof(Rows31_1)).Value=value;
                OnPropertyChanged(nameof(Rows31_1));
            }
        }
        private bool Rows31_1_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form31_1>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form32> Rows32
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form32>>(nameof(Rows32)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form32>>(nameof(Rows32)).Value=value;
                OnPropertyChanged(nameof(Rows32));
            }
        }
        private bool Rows32_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form32>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form32_1> Rows32_1
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form32_1>>(nameof(Rows32_1)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form32_1>>(nameof(Rows32_1)).Value=value;
                OnPropertyChanged(nameof(Rows32_1));
            }
        }
        private bool Rows32_1_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form32_1>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form32_2> Rows32_2
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form32_2>>(nameof(Rows32_2)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form32_2>>(nameof(Rows32_2)).Value=value;
                OnPropertyChanged(nameof(Rows32_2));
            }
        }
        private bool Rows32_2_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form32_2>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form32_3> Rows32_3
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form32_3>>(nameof(Rows32_3)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form32_3>>(nameof(Rows32_3)).Value=value;
                OnPropertyChanged(nameof(Rows32_3));
            }
        }
        private bool Rows32_3_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form32_3>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form40> Rows40
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form40>>(nameof(Rows40)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form40>>(nameof(Rows40)).Value=value;
                OnPropertyChanged(nameof(Rows40));
            }
        }
        private bool Rows40_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form40>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form41> Rows41
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form41>>(nameof(Rows41)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form41>>(nameof(Rows41)).Value=value;
                OnPropertyChanged(nameof(Rows41));
            }
        }
        private bool Rows41_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form41>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form50> Rows50
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form50>>(nameof(Rows50)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form50>>(nameof(Rows50)).Value=value;
                OnPropertyChanged(nameof(Rows50));
            }
        }
        private bool Rows50_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form50>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form51> Rows51
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form51>>(nameof(Rows51)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form51>>(nameof(Rows51)).Value=value;
                OnPropertyChanged(nameof(Rows51));
            }
        }
        private bool Rows51_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form51>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form52> Rows52
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form52>>(nameof(Rows52)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form52>>(nameof(Rows52)).Value=value;
                OnPropertyChanged(nameof(Rows52));
            }
        }
        private bool Rows52_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form52>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form53> Rows53
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form53>>(nameof(Rows53)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form53>>(nameof(Rows53)).Value=value;
                OnPropertyChanged(nameof(Rows53));
            }
        }
        private bool Rows53_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form53>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form54> Rows54
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form54>>(nameof(Rows54)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form54>>(nameof(Rows54)).Value=value;
                OnPropertyChanged(nameof(Rows54));
            }
        }
        private bool Rows54_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form54>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form55> Rows55
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form55>>(nameof(Rows55)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form55>>(nameof(Rows55)).Value=value;
                OnPropertyChanged(nameof(Rows55));
            }
        }
        private bool Rows55_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form55>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form56> Rows56
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form56>>(nameof(Rows56)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form56>>(nameof(Rows56)).Value=value;
                OnPropertyChanged(nameof(Rows56));
            }
        }
        private bool Rows56_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form56>> value)
        {
            return true;
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Models.Form57> Rows57
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form57>>(nameof(Rows57)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Form57>>(nameof(Rows57)).Value=value;
                OnPropertyChanged(nameof(Rows57));
            }
        }
        private bool Rows57_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Form57>> value)
        {
            return true;
        }

        [Form_Property("Форма")]public int? FormNumId { get; set; }
        public virtual RamAccess<string> FormNum
        {
            get
            {
                if (Rows10.Count() > 0) { return new RamAccess<string>(null,"1/0"); }
                if (Rows11.Count() > 0) { return new RamAccess<string>(null,"1/1"); }
                if (Rows12.Count() > 0) { return new RamAccess<string>(null,"1/2"); }
                if (Rows13.Count() > 0) { return new RamAccess<string>(null,"1/3"); }
                if (Rows14.Count() > 0) { return new RamAccess<string>(null,"1/4"); }
                if (Rows15.Count() > 0) { return new RamAccess<string>(null,"1/5"); }
                if (Rows16.Count() > 0) { return new RamAccess<string>(null,"1/6"); }
                if (Rows17.Count() > 0) { return new RamAccess<string>(null,"1/7"); }
                if (Rows18.Count() > 0) { return new RamAccess<string>(null,"1/8"); }
                if (Rows19.Count() > 0) { return new RamAccess<string>(null,"1/9"); }

                if (Rows20.Count() > 0) { return new RamAccess<string>(null,"2/0"); }
                if (Rows21.Count() > 0) { return new RamAccess<string>(null,"2/1"); }
                if (Rows22.Count() > 0) { return new RamAccess<string>(null,"2/2"); }
                if (Rows23.Count() > 0) { return new RamAccess<string>(null,"2/3"); }
                if (Rows24.Count() > 0) { return new RamAccess<string>(null,"2/4"); }
                if (Rows25.Count() > 0) { return new RamAccess<string>(null,"2/5"); }
                if (Rows26.Count() > 0) { return new RamAccess<string>(null,"2/6"); }
                if (Rows27.Count() > 0) { return new RamAccess<string>(null,"2/7"); }
                if (Rows28.Count() > 0) { return new RamAccess<string>(null,"2/8"); }
                if (Rows29.Count() > 0) { return new RamAccess<string>(null,"2/9"); }
                if (Rows210.Count() > 0) { return new RamAccess<string>(null,"2/10"); }
                if (Rows211.Count() > 0) { return new RamAccess<string>(null,"2/11"); }
                if (Rows212.Count() > 0) { return new RamAccess<string>(null,"2/12"); }

                if (Rows30.Count() > 0) { return new RamAccess<string>(null,"3/0"); }
                if (Rows31.Count() > 0) { return new RamAccess<string>(null,"3/1"); }
                if (Rows31_1.Count() > 0) { return new RamAccess<string>(null,"3/1_1"); }
                if (Rows32.Count() > 0) { return new RamAccess<string>(null,"3/2"); }
                if (Rows32_1.Count() > 0) { return new RamAccess<string>(null,"3/2_1"); }
                if (Rows32_2.Count() > 0) { return new RamAccess<string>(null,"3/2_2"); }
                if (Rows32_3.Count() > 0) { return new RamAccess<string>(null,"3/2_3"); }

                if (Rows40.Count() > 0) { return new RamAccess<string>(null,"4/0"); }
                if (Rows41.Count() > 0) { return new RamAccess<string>(null,"4/1"); }

                if (Rows50.Count() > 0) { return new RamAccess<string>(null,"5/0"); }
                if (Rows51.Count() > 0) { return new RamAccess<string>(null,"5/1"); }
                if (Rows52.Count() > 0) { return new RamAccess<string>(null,"5/2"); }
                if (Rows53.Count() > 0) { return new RamAccess<string>(null,"5/3"); }
                if (Rows54.Count() > 0) { return new RamAccess<string>(null,"5/4"); }
                if (Rows55.Count() > 0) { return new RamAccess<string>(null,"5/5"); }
                if (Rows56.Count() > 0) { return new RamAccess<string>(null,"5/6"); }
                if (Rows57.Count() > 0) { return new RamAccess<string>(null,"5/7"); }
                return new RamAccess<string>(null,"0");
            }
            set
            {

            }
        }

        //IsCorrection 
        public int? IsCorrectionId { get; set; }
        [Form_Property("Корректирующий отчет")]
        public virtual RamAccess<bool> IsCorrection
        {
            get
            {
                return DataAccess.Get<bool>(nameof(IsCorrection));
            }
            set
            {
                DataAccess.Set(nameof(IsCorrection), value);
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        private bool IsCorrection_Validation(RamAccess<bool> value)
        {
            return true;
        }
        //IsCorrection

        //CorrectionNumber property
        public int? CorrectionNumberId { get; set; }
        [Form_Property("Номер корректировки")]
        public virtual RamAccess<byte> CorrectionNumber
        {
            get
            {
                return DataAccess.Get<byte>(nameof(CorrectionNumber));
            }
            set
            {
                DataAccess.Set(nameof(CorrectionNumber), value);
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors(); return true;
        }
        //CorrectionNumber property

        //NumberInOrder property
        public int? NumberInOrderId { get; set; }
        [Form_Property("Номер")]
        public virtual RamAccess<string> NumberInOrder
        {
            get
            {
                return DataAccess.Get<string>(nameof(NumberInOrder));
            }
            set
            {
                DataAccess.Set(nameof(NumberInOrder), value);
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private bool NumberInOrder_Validation(RamAccess<string> value)
        {
            return true;
        }
        //NumberInOrder property

        //Comments property
        public int? CommentsId { get; set; }
        [Form_Property("Комментарий")]
        public virtual RamAccess<string> Comments
        {
            get
            {
                return DataAccess.Get<string>(nameof(Comments));
            }
            set
            {
                DataAccess.Set(nameof(Comments), value);
                OnPropertyChanged(nameof(Comments));
            }
        }
        private bool Comments_Validation(RamAccess<string> value)
        {
            return true;
        }
        //Comments property

        //Notes property
        public int? NotesId { get; set; }
        [Form_Property("Примечания")]
        public virtual ObservableCollectionWithItemPropertyChanged<Models.Note> Notes
        {
            get
            {
                return DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Note>>(nameof(Notes)).Value;
            }
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Models.Note>>(nameof(Notes)).Value=value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        private bool Notes_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Models.Note>> value)
        {
            return true;
        }
        //Notes property

        //StartPeriod
        public int? StartPeriodId { get; set; }
        [Form_Property("Начало")]
        public virtual RamAccess<string> StartPeriod
        {
            get
            {
                return DataAccess.Get<string>(nameof(StartPeriod));
            }
            set
            {
                DataAccess.Set(nameof(StartPeriod), value);
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private bool StartPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //StartPeriod

        //EndPeriod
        public int? EndPeriodId { get; set; }
        [Form_Property("Конец")]
        public virtual RamAccess<string> EndPeriod
        {
            get
            {
                return DataAccess.Get<string>(nameof(EndPeriod));
            }
            set
            {
                DataAccess.Set(nameof(EndPeriod), value);
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        private bool EndPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //EndPeriod

        //ExportDate
        public int? ExportDateId { get; set; }
        [Form_Property("Дата выгрузки")]
        public virtual RamAccess<string> ExportDate
        {
            get
            {
                return DataAccess.Get<string>(nameof(ExportDate));
            }
            set
            {
                DataAccess.Set(nameof(ExportDate), value);
                OnPropertyChanged(nameof(ExportDate));
            }
        }
        private bool ExportDate_Validation(RamAccess<string> value)
        {
            return true;
        }
        //ExportDate

        [NotMapped]
        bool _isChanged = true;
        public bool IsChanged
        {
            get
            {
                return _isChanged;
            }
            set
            {
                if (_isChanged != value)
                {
                    _isChanged = value;
                    OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        //Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (prop != nameof(IsChanged))
            {
                IsChanged = true;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed
    }
}
