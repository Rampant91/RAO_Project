using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using Models;
using Models.Attributes;
using Models.DataAccess;

namespace Collections
{
    public class Report : IKey,INotifyPropertyChanged
    {
        //ExportDate

        public enum Forms
        {
            None,
            Form10,
            Form11,
            Form12,
            Form13,
            Form14,
            Form15,
            Form16,
            Form17,
            Form18,
            Form19,
            Form20,
            Form21,
            Form22,
            Form23,
            Form24,
            Form25,
            Form26,
            Form27,
            Form28,
            Form29,
            Form210,
            Form211,
            Form212
        }

        [NotMapped] private bool _isChanged = true;
        [NotMapped] private Forms _lastAddedForm = Forms.None;

        [NotMapped]
        public Forms LastAddedForm
        {
            get
            {
                return _lastAddedForm;
            }
            set
            {
                if(!(value==_lastAddedForm)) _lastAddedForm = value;
            }
        }
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

        private DataAccessCollection DataAccess { get; }

        public virtual ObservableCollectionWithItemPropertyChanged<Form10> Rows10
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form10>>(nameof(Rows10)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form10>>(nameof(Rows10)).Value = value;
                OnPropertyChanged(nameof(Rows10));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form11> Rows11
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form11>>(nameof(Rows11)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form11>>(nameof(Rows11)).Value = value;
                OnPropertyChanged(nameof(Rows11));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form12> Rows12
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form12>>(nameof(Rows12)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form12>>(nameof(Rows12)).Value = value;
                OnPropertyChanged(nameof(Rows12));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form13> Rows13
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form13>>(nameof(Rows13)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form13>>(nameof(Rows13)).Value = value;
                OnPropertyChanged(nameof(Rows13));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form14> Rows14
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form14>>(nameof(Rows14)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form14>>(nameof(Rows14)).Value = value;
                OnPropertyChanged(nameof(Rows14));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form15> Rows15
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form15>>(nameof(Rows15)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form15>>(nameof(Rows15)).Value = value;
                OnPropertyChanged(nameof(Rows15));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form16> Rows16
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form16>>(nameof(Rows16)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form16>>(nameof(Rows16)).Value = value;
                OnPropertyChanged(nameof(Rows16));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form17> Rows17
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form17>>(nameof(Rows17)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form17>>(nameof(Rows17)).Value = value;
                OnPropertyChanged(nameof(Rows17));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form18> Rows18
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form18>>(nameof(Rows18)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form18>>(nameof(Rows18)).Value = value;
                OnPropertyChanged(nameof(Rows18));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form19> Rows19
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form19>>(nameof(Rows19)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form19>>(nameof(Rows19)).Value = value;
                OnPropertyChanged(nameof(Rows19));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form20> Rows20
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form20>>(nameof(Rows20)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form20>>(nameof(Rows20)).Value = value;
                OnPropertyChanged(nameof(Rows20));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form21> Rows21
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form21>>(nameof(Rows21)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form21>>(nameof(Rows21)).Value = value;
                OnPropertyChanged(nameof(Rows21));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form22> Rows22
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form22>>(nameof(Rows22)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form22>>(nameof(Rows22)).Value = value;
                OnPropertyChanged(nameof(Rows22));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form23> Rows23
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form23>>(nameof(Rows23)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form23>>(nameof(Rows23)).Value = value;
                OnPropertyChanged(nameof(Rows23));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form24> Rows24
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form24>>(nameof(Rows24)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form24>>(nameof(Rows24)).Value = value;
                OnPropertyChanged(nameof(Rows24));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form25> Rows25
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form25>>(nameof(Rows25)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form25>>(nameof(Rows25)).Value = value;
                OnPropertyChanged(nameof(Rows25));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form26> Rows26
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form26>>(nameof(Rows26)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form26>>(nameof(Rows26)).Value = value;
                OnPropertyChanged(nameof(Rows26));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form27> Rows27
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form27>>(nameof(Rows27)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form27>>(nameof(Rows27)).Value = value;
                OnPropertyChanged(nameof(Rows27));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form28> Rows28
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form28>>(nameof(Rows28)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form28>>(nameof(Rows28)).Value = value;
                OnPropertyChanged(nameof(Rows28));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form29> Rows29
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form29>>(nameof(Rows29)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form29>>(nameof(Rows29)).Value = value;
                OnPropertyChanged(nameof(Rows29));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form210> Rows210
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form210>>(nameof(Rows210)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form210>>(nameof(Rows210)).Value = value;
                OnPropertyChanged(nameof(Rows210));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form211> Rows211
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form211>>(nameof(Rows211)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form211>>(nameof(Rows211)).Value = value;
                OnPropertyChanged(nameof(Rows211));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form212> Rows212
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form212>>(nameof(Rows212)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form212>>(nameof(Rows212)).Value = value;
                OnPropertyChanged(nameof(Rows212));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form30> Rows30
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form30>>(nameof(Rows30)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form30>>(nameof(Rows30)).Value = value;
                OnPropertyChanged(nameof(Rows30));
            }
        }


        public virtual ObservableCollectionWithItemPropertyChanged<Form31> Rows31
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form31>>(nameof(Rows31)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form31>>(nameof(Rows31)).Value = value;
                OnPropertyChanged(nameof(Rows31));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form31_1> Rows31_1
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form31_1>>(nameof(Rows31_1)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form31_1>>(nameof(Rows31_1)).Value = value;
                OnPropertyChanged(nameof(Rows31_1));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form32> Rows32
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form32>>(nameof(Rows32)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form32>>(nameof(Rows32)).Value = value;
                OnPropertyChanged(nameof(Rows32));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form32_1> Rows32_1
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form32_1>>(nameof(Rows32_1)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form32_1>>(nameof(Rows32_1)).Value = value;
                OnPropertyChanged(nameof(Rows32_1));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form32_2> Rows32_2
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form32_2>>(nameof(Rows32_2)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form32_2>>(nameof(Rows32_2)).Value = value;
                OnPropertyChanged(nameof(Rows32_2));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form32_3> Rows32_3
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form32_3>>(nameof(Rows32_3)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form32_3>>(nameof(Rows32_3)).Value = value;
                OnPropertyChanged(nameof(Rows32_3));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form40> Rows40
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form40>>(nameof(Rows40)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form40>>(nameof(Rows40)).Value = value;
                OnPropertyChanged(nameof(Rows40));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form41> Rows41
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form41>>(nameof(Rows41)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form41>>(nameof(Rows41)).Value = value;
                OnPropertyChanged(nameof(Rows41));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form50> Rows50
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form50>>(nameof(Rows50)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form50>>(nameof(Rows50)).Value = value;
                OnPropertyChanged(nameof(Rows50));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form51> Rows51
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form51>>(nameof(Rows51)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form51>>(nameof(Rows51)).Value = value;
                OnPropertyChanged(nameof(Rows51));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form52> Rows52
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form52>>(nameof(Rows52)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form52>>(nameof(Rows52)).Value = value;
                OnPropertyChanged(nameof(Rows52));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form53> Rows53
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form53>>(nameof(Rows53)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form53>>(nameof(Rows53)).Value = value;
                OnPropertyChanged(nameof(Rows53));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form54> Rows54
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form54>>(nameof(Rows54)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form54>>(nameof(Rows54)).Value = value;
                OnPropertyChanged(nameof(Rows54));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form55> Rows55
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form55>>(nameof(Rows55)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form55>>(nameof(Rows55)).Value = value;
                OnPropertyChanged(nameof(Rows55));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form56> Rows56
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form56>>(nameof(Rows56)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form56>>(nameof(Rows56)).Value = value;
                OnPropertyChanged(nameof(Rows56));
            }
        }

        public virtual ObservableCollectionWithItemPropertyChanged<Form57> Rows57
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form57>>(nameof(Rows57)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Form57>>(nameof(Rows57)).Value = value;
                OnPropertyChanged(nameof(Rows57));
            }
        }

        public int? FormNumId { get; set; }

        [Form_Property("Форма")]
        public virtual RamAccess<string> FormNum
        {
            get
            {
                if (Rows10.Count() > 0) return new RamAccess<string>(null, "1.0");
                if (Rows11.Count() > 0) return new RamAccess<string>(null, "1.1");
                if (Rows12.Count() > 0) return new RamAccess<string>(null, "1.2");
                if (Rows13.Count() > 0) return new RamAccess<string>(null, "1.3");
                if (Rows14.Count() > 0) return new RamAccess<string>(null, "1.4");
                if (Rows15.Count() > 0) return new RamAccess<string>(null, "1.5");
                if (Rows16.Count() > 0) return new RamAccess<string>(null, "1.6");
                if (Rows17.Count() > 0) return new RamAccess<string>(null, "1.7");
                if (Rows18.Count() > 0) return new RamAccess<string>(null, "1.8");
                if (Rows19.Count() > 0) return new RamAccess<string>(null, "1.9");
                if (Rows20.Count() > 0) return new RamAccess<string>(null, "2.0");
                if (Rows21.Count() > 0) return new RamAccess<string>(null, "2.1");
                if (Rows22.Count() > 0) return new RamAccess<string>(null, "2.2");
                if (Rows23.Count() > 0) return new RamAccess<string>(null, "2.3");
                if (Rows24.Count() > 0) return new RamAccess<string>(null, "2.4");
                if (Rows25.Count() > 0) return new RamAccess<string>(null, "2.5");
                if (Rows26.Count() > 0) return new RamAccess<string>(null, "2.6");
                if (Rows27.Count() > 0) return new RamAccess<string>(null, "2.7");
                if (Rows28.Count() > 0) return new RamAccess<string>(null, "2.8");
                if (Rows29.Count() > 0) return new RamAccess<string>(null, "2.9");
                if (Rows210.Count() > 0) return new RamAccess<string>(null, "2.10");
                if (Rows211.Count() > 0) return new RamAccess<string>(null, "2.11");
                if (Rows212.Count() > 0) return new RamAccess<string>(null, "2.12");
                if (Rows30.Count() > 0) return new RamAccess<string>(null, "3.0");
                if (Rows31.Count() > 0) return new RamAccess<string>(null, "3.1");
                if (Rows31_1.Count() > 0) return new RamAccess<string>(null, "3.1_1");
                if (Rows32.Count() > 0) return new RamAccess<string>(null, "3.2");
                if (Rows32_1.Count() > 0) return new RamAccess<string>(null, "3.2_1");
                if (Rows32_2.Count() > 0) return new RamAccess<string>(null, "3.2_2");
                if (Rows32_3.Count() > 0) return new RamAccess<string>(null, "3.2_3");
                if (Rows40.Count() > 0) return new RamAccess<string>(null, "4.0");
                if (Rows41.Count() > 0) return new RamAccess<string>(null, "4.1");
                if (Rows50.Count() > 0) return new RamAccess<string>(null, "5.0");
                if (Rows51.Count() > 0) return new RamAccess<string>(null, "5.1");
                if (Rows52.Count() > 0) return new RamAccess<string>(null, "5.2");
                if (Rows53.Count() > 0) return new RamAccess<string>(null, "5.3");
                if (Rows54.Count() > 0) return new RamAccess<string>(null, "5.4");
                if (Rows55.Count() > 0) return new RamAccess<string>(null, "5.5");
                if (Rows56.Count() > 0) return new RamAccess<string>(null, "5.6");
                if (Rows57.Count() > 0) return new RamAccess<string>(null, "5.7");
                return new RamAccess<string>(null, "0");
            }
            set { }
        }

        //IsCorrection 
        public int? IsCorrectionId { get; set; }

        [Form_Property("Корректирующий отчет")]
        public virtual RamAccess<bool> IsCorrection
        {
            get => DataAccess.Get<bool>(nameof(IsCorrection));
            set
            {
                DataAccess.Set(nameof(IsCorrection), value);
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        //IsCorrection

        //CorrectionNumber property
        public int? CorrectionNumberId { get; set; }

        [Form_Property("Номер корректировки")]
        public virtual RamAccess<byte> CorrectionNumber
        {
            get => DataAccess.Get<byte>(nameof(CorrectionNumber));
            set
            {
                DataAccess.Set(nameof(CorrectionNumber), value);
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        //CorrectionNumber property

        //NumberInOrder property
        public int? NumberInOrderId { get; set; }

        [Form_Property("Номер")]
        public virtual RamAccess<string> NumberInOrder
        {
            get => DataAccess.Get<string>(nameof(NumberInOrder));
            set
            {
                DataAccess.Set(nameof(NumberInOrder), value);
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        //NumberInOrder property

        //Comments property
        public int? CommentsId { get; set; }

        [Form_Property("Комментарий")]
        public virtual RamAccess<string> Comments
        {
            get => DataAccess.Get<string>(nameof(Comments));
            set
            {
                DataAccess.Set(nameof(Comments), value);
                OnPropertyChanged(nameof(Comments));
            }
        }
        //Comments property

        //Notes property
        public int? NotesId { get; set; }

        [Form_Property("Примечания")]
        public virtual ObservableCollectionWithItemPropertyChanged<Note> Notes
        {
            get => DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Note>>(nameof(Notes)).Value;
            set
            {
                DataAccess.Get<ObservableCollectionWithItemPropertyChanged<Note>>(nameof(Notes)).Value = value;
                OnPropertyChanged(nameof(Notes));
            }
        }
        //Notes property

        //StartPeriod
        public int? StartPeriodId { get; set; }

        [Form_Property("Начало")]
        public virtual RamAccess<string> StartPeriod
        {
            get => DataAccess.Get<string>(nameof(StartPeriod));
            set
            {
                DataAccess.Set(nameof(StartPeriod), value);
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        //StartPeriod

        //EndPeriod
        public int? EndPeriodId { get; set; }

        [Form_Property("Конец")]
        public virtual RamAccess<string> EndPeriod
        {
            get => DataAccess.Get<string>(nameof(EndPeriod));
            set
            {
                DataAccess.Set(nameof(EndPeriod), value);
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        //EndPeriod

        //ExportDate
        public int? ExportDateId { get; set; }

        [Form_Property("Дата выгрузки")]
        public virtual RamAccess<string> ExportDate
        {
            get => DataAccess.Get<string>(nameof(ExportDate));
            set
            {
                DataAccess.Set(nameof(ExportDate), value);
                OnPropertyChanged(nameof(ExportDate));
            }
        }

        public bool IsChanged
        {
            get => _isChanged;
            set
            {
                if (_isChanged != value)
                {
                    _isChanged = value;
                    OnPropertyChanged(nameof(IsChanged));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

        private void Init()
        {
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form10>>(nameof(Rows10), Rows10_Validation,
                null);
            Rows10 = new ObservableCollectionWithItemPropertyChanged<Form10>();
            Rows10.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form11>>(nameof(Rows11), Rows11_Validation,
                null);
            Rows11 = new ObservableCollectionWithItemPropertyChanged<Form11>();
            Rows11.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form12>>(nameof(Rows12), Rows12_Validation,
                null);
            Rows12 = new ObservableCollectionWithItemPropertyChanged<Form12>();
            Rows12.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form13>>(nameof(Rows13), Rows13_Validation,
                null);
            Rows13 = new ObservableCollectionWithItemPropertyChanged<Form13>();
            Rows13.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form14>>(nameof(Rows14), Rows14_Validation,
                null);
            Rows14 = new ObservableCollectionWithItemPropertyChanged<Form14>();
            Rows14.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form15>>(nameof(Rows15), Rows15_Validation,
                null);
            Rows15 = new ObservableCollectionWithItemPropertyChanged<Form15>();
            Rows15.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form16>>(nameof(Rows16), Rows16_Validation,
                null);
            Rows16 = new ObservableCollectionWithItemPropertyChanged<Form16>();
            Rows16.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form17>>(nameof(Rows17), Rows17_Validation,
                null);
            Rows17 = new ObservableCollectionWithItemPropertyChanged<Form17>();
            Rows17.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form18>>(nameof(Rows18), Rows18_Validation,
                null);
            Rows18 = new ObservableCollectionWithItemPropertyChanged<Form18>();
            Rows18.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form19>>(nameof(Rows19), Rows19_Validation,
                null);
            Rows19 = new ObservableCollectionWithItemPropertyChanged<Form19>();
            Rows19.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form20>>(nameof(Rows20), Rows20_Validation,
                null);
            Rows20 = new ObservableCollectionWithItemPropertyChanged<Form20>();
            Rows20.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form21>>(nameof(Rows21), Rows21_Validation,
                null);
            Rows21 = new ObservableCollectionWithItemPropertyChanged<Form21>();
            Rows21.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form22>>(nameof(Rows22), Rows22_Validation,
                null);
            Rows22 = new ObservableCollectionWithItemPropertyChanged<Form22>();
            Rows22.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form23>>(nameof(Rows23), Rows23_Validation,
                null);
            Rows23 = new ObservableCollectionWithItemPropertyChanged<Form23>();
            Rows23.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form24>>(nameof(Rows24), Rows24_Validation,
                null);
            Rows24 = new ObservableCollectionWithItemPropertyChanged<Form24>();
            Rows24.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form25>>(nameof(Rows25), Rows25_Validation,
                null);
            Rows25 = new ObservableCollectionWithItemPropertyChanged<Form25>();
            Rows25.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form26>>(nameof(Rows26), Rows26_Validation,
                null);
            Rows26 = new ObservableCollectionWithItemPropertyChanged<Form26>();
            Rows26.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form27>>(nameof(Rows27), Rows27_Validation,
                null);
            Rows27 = new ObservableCollectionWithItemPropertyChanged<Form27>();
            Rows27.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form28>>(nameof(Rows28), Rows28_Validation,
                null);
            Rows28 = new ObservableCollectionWithItemPropertyChanged<Form28>();
            Rows28.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form29>>(nameof(Rows29), Rows29_Validation,
                null);
            Rows29 = new ObservableCollectionWithItemPropertyChanged<Form29>();
            Rows29.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form210>>(nameof(Rows210), Rows210_Validation,
                null);
            Rows210 = new ObservableCollectionWithItemPropertyChanged<Form210>();
            Rows210.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form211>>(nameof(Rows211), Rows211_Validation,
                null);
            Rows211 = new ObservableCollectionWithItemPropertyChanged<Form211>();
            Rows211.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form212>>(nameof(Rows212), Rows212_Validation,
                null);
            Rows212 = new ObservableCollectionWithItemPropertyChanged<Form212>();
            Rows212.CollectionChanged += CollectionChanged;

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form30>>(nameof(Rows30), Rows30_Validation,
                null);
            Rows30 = new ObservableCollectionWithItemPropertyChanged<Form30>();
            Rows30.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form31>>(nameof(Rows31), Rows31_Validation,
                null);
            Rows31 = new ObservableCollectionWithItemPropertyChanged<Form31>();
            Rows31.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form31_1>>(nameof(Rows31_1),
                Rows31_1_Validation, null);
            Rows31_1 = new ObservableCollectionWithItemPropertyChanged<Form31_1>();
            Rows31_1.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form32>>(nameof(Rows32), Rows32_Validation,
                null);
            Rows32 = new ObservableCollectionWithItemPropertyChanged<Form32>();
            Rows32.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form32_1>>(nameof(Rows32_1),
                Rows32_1_Validation, null);
            Rows32_1 = new ObservableCollectionWithItemPropertyChanged<Form32_1>();
            Rows32_1.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form32_2>>(nameof(Rows32_2),
                Rows32_2_Validation, null);
            Rows32_2 = new ObservableCollectionWithItemPropertyChanged<Form32_2>();
            Rows32_2.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form32_3>>(nameof(Rows32_3),
                Rows32_3_Validation, null);
            Rows32_3 = new ObservableCollectionWithItemPropertyChanged<Form32_3>();
            Rows32_3.CollectionChanged += CollectionChanged;

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form40>>(nameof(Rows40), Rows40_Validation,
                null);
            Rows40 = new ObservableCollectionWithItemPropertyChanged<Form40>();
            Rows40.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form41>>(nameof(Rows41), Rows41_Validation,
                null);
            Rows41 = new ObservableCollectionWithItemPropertyChanged<Form41>();
            Rows41.CollectionChanged += CollectionChanged;

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form50>>(nameof(Rows50), Rows50_Validation,
                null);
            Rows50 = new ObservableCollectionWithItemPropertyChanged<Form50>();
            Rows50.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form51>>(nameof(Rows51), Rows51_Validation,
                null);
            Rows51 = new ObservableCollectionWithItemPropertyChanged<Form51>();
            Rows51.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form52>>(nameof(Rows52), Rows52_Validation,
                null);
            Rows52 = new ObservableCollectionWithItemPropertyChanged<Form52>();
            Rows52.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form53>>(nameof(Rows53), Rows53_Validation,
                null);
            Rows53 = new ObservableCollectionWithItemPropertyChanged<Form53>();
            Rows53.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form54>>(nameof(Rows54), Rows54_Validation,
                null);
            Rows54 = new ObservableCollectionWithItemPropertyChanged<Form54>();
            Rows54.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form55>>(nameof(Rows55), Rows55_Validation,
                null);
            Rows55 = new ObservableCollectionWithItemPropertyChanged<Form55>();
            Rows55.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form56>>(nameof(Rows56), Rows56_Validation,
                null);
            Rows56 = new ObservableCollectionWithItemPropertyChanged<Form56>();
            Rows56.CollectionChanged += CollectionChanged;
            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Form57>>(nameof(Rows57), Rows57_Validation,
                null);
            Rows57 = new ObservableCollectionWithItemPropertyChanged<Form57>();
            Rows57.CollectionChanged += CollectionChanged;

            DataAccess.Init<ObservableCollectionWithItemPropertyChanged<Note>>(nameof(Notes), Notes_Validation, null);
            Notes = new ObservableCollectionWithItemPropertyChanged<Note>();
            Notes.CollectionChanged += CollectionChanged;


            DataAccess.Init(nameof(StartPeriod), StartPeriod_Validation, "");
            DataAccess.Init(nameof(Comments), Comments_Validation, "");
            DataAccess.Init<byte>(nameof(CorrectionNumber), CorrectionNumber_Validation, 0);
            DataAccess.Init(nameof(IsCorrection), IsCorrection_Validation, false);
            DataAccess.Init(nameof(NumberInOrder), NumberInOrder_Validation, "");
            DataAccess.Init(nameof(EndPeriod), EndPeriod_Validation, "");
            DataAccess.Init(nameof(ExportDate), ExportDate_Validation, "");
        }

        public bool Equals(object obj)
        {
            if (obj is Report)
            {
                var obj1 = this;
                var obj2 = obj as Report;

                return obj1.DataAccess == obj2.DataAccess;
            }

            return false;
            return true;
        }

        public static bool operator ==(Report obj1, Report obj2)
        {
            if (obj1 as object != null)
                return obj1.Equals(obj2);
            return obj2 as object == null ? true : false;
        }

        public static bool operator !=(Report obj1, Report obj2)
        {
            if (obj1 as object != null)
                return !obj1.Equals(obj2);
            return obj2 as object != null ? true : false;
        }


        protected void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged(nameof(Notes));

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

        private bool Rows10_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form10>> value)
        {
            return true;
        }

        private bool Rows11_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form11>> value)
        {
            return true;
        }

        private bool Rows12_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form12>> value)
        {
            return true;
        }

        private bool Rows13_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form13>> value)
        {
            return true;
        }

        private bool Rows14_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form14>> value)
        {
            return true;
        }

        private bool Rows15_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form15>> value)
        {
            return true;
        }

        private bool Rows16_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form16>> value)
        {
            return true;
        }

        private bool Rows17_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form17>> value)
        {
            return true;
        }

        private bool Rows18_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form18>> value)
        {
            return true;
        }

        private bool Rows19_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form19>> value)
        {
            return true;
        }

        private bool Rows20_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form20>> value)
        {
            return true;
        }

        private bool Rows21_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form21>> value)
        {
            return true;
        }

        private bool Rows22_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form22>> value)
        {
            return true;
        }

        private bool Rows23_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form23>> value)
        {
            return true;
        }

        private bool Rows24_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form24>> value)
        {
            return true;
        }

        private bool Rows25_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form25>> value)
        {
            return true;
        }

        private bool Rows26_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form26>> value)
        {
            return true;
        }

        private bool Rows27_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form27>> value)
        {
            return true;
        }

        private bool Rows28_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form28>> value)
        {
            return true;
        }

        private bool Rows29_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form29>> value)
        {
            return true;
        }

        private bool Rows210_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form210>> value)
        {
            return true;
        }

        private bool Rows211_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form211>> value)
        {
            return true;
        }

        private bool Rows212_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form212>> value)
        {
            return true;
        }

        private bool Rows30_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form30>> value)
        {
            return true;
        }

        private bool Rows31_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form31>> value)
        {
            return true;
        }

        private bool Rows31_1_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form31_1>> value)
        {
            return true;
        }

        private bool Rows32_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form32>> value)
        {
            return true;
        }

        private bool Rows32_1_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form32_1>> value)
        {
            return true;
        }

        private bool Rows32_2_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form32_2>> value)
        {
            return true;
        }

        private bool Rows32_3_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form32_3>> value)
        {
            return true;
        }

        private bool Rows40_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form40>> value)
        {
            return true;
        }

        private bool Rows41_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form41>> value)
        {
            return true;
        }

        private bool Rows50_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form50>> value)
        {
            return true;
        }

        private bool Rows51_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form51>> value)
        {
            return true;
        }

        private bool Rows52_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form52>> value)
        {
            return true;
        }

        private bool Rows53_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form53>> value)
        {
            return true;
        }

        private bool Rows54_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form54>> value)
        {
            return true;
        }

        private bool Rows55_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form55>> value)
        {
            return true;
        }

        private bool Rows56_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form56>> value)
        {
            return true;
        }

        private bool Rows57_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Form57>> value)
        {
            return true;
        }

        private bool IsCorrection_Validation(RamAccess<bool> value)
        {
            return true;
        }

        private bool CorrectionNumber_Validation(RamAccess<byte> value)
        {
            value.ClearErrors();
            return true;
        }

        private bool NumberInOrder_Validation(RamAccess<string> value)
        {
            return true;
        }

        private bool Comments_Validation(RamAccess<string> value)
        {
            return true;
        }

        private bool Notes_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Note>> value)
        {
            return true;
        }

        private bool StartPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        private bool EndPeriod_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }

        private bool ExportDate_Validation(RamAccess<string> value)
        {
            return true;
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
        //Property Changed
    }
}