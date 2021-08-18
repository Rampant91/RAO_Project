using Collections;
using Models.DataAccess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Models.Abstracts
{
    public abstract class Form : INotifyPropertyChanged, IKey
    {
        protected DataAccessCollection DataAccess { get; set; }
        protected DBRealization.DBModel dbm { get; set; }

        public Form()
        {
            dbm = DBRealization.StaticConfiguration.DBModel;
            DataAccess = new DataAccessCollection();
            Init();
        }
        public Form(string T)
        {
            dbm = DBRealization.StaticConfiguration.DBModel;
            DataAccess = new DataAccessCollection();
            Init();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(FormNum), FormNum_Validation, "");
            DataAccess.Init<int>(nameof(NumberOfFields), NumberOfFields_Validation, 0);
        }

        public bool Equals(object obj)
        {
            if (obj is Form)
            {
                Form obj1 = this;
                Form obj2 = obj as Form;

                return obj1.DataAccess == obj2.DataAccess;
            }
            else
            {
                return false;
            }
        }

        public static bool operator ==(Form obj1, Form obj2)
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
        public static bool operator !=(Form obj1, Form obj2)
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

        public int Id { get; set; }

        //FormNum property
        public int? FormNumId { get; set; }
        [Attributes.Form_Property("Форма")]
        public virtual RamAccess<string> FormNum
        {
            get
            {
                return DataAccess.Get<string>(nameof(FormNum));
            }
            set
            {
                DataAccess.Set(nameof(FormNum), value);
                OnPropertyChanged(nameof(FormNum));
            }
        }
        private bool FormNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        //FormNum property

        public int? NumberOfFieldsId { get; set; }
        //NumberOfFields property
        public virtual RamAccess<int> NumberOfFields
        {
            get => DataAccess.Get<int>(nameof(NumberOfFields));
            set
            {
                DataAccess.Set(nameof(NumberOfFields), value);
                OnPropertyChanged(nameof(NumberOfFields));
            }
        }
        private bool NumberOfFields_Validation(RamAccess<int> value)
        {
            value.ClearErrors();
            return true;

        }
        //NumberOfFields property

        //Для валидации
        public abstract bool Object_Validation();
        //Для валидации

        [NotMapped]
        private bool _isChanged = true;
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

        //Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (prop != nameof(IsChanged))
            {
                IsChanged = true;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed
    }
}
