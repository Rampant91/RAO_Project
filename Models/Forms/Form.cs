using Models.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using Avalonia.Data;
using System.ComponentModel.DataAnnotations.Schema;
using Models.Collections;

namespace Models.Abstracts
{
    public abstract class Form : IChanged
    {
        protected IDataAccessCollection _dataAccess { get; set; }

        public Form()
        {
            _dataAccess = new DataAccessCollection();
            Init();
        }

        void Init()
        {
            _dataAccess.Init<string>(nameof(FormNum), FormNum_Validation, "");
            _dataAccess.Init<int>(nameof(NumberOfFields), NumberOfFields_Validation, 0);
        }

        public bool Equals(object obj)
        {
            if(obj is Form)
            {
                var obj1 = this;
                var obj2 = obj as Form;

                return obj1._dataAccess == obj2._dataAccess;
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

        [Key]
        public int RowId { get; set; }

        //FormNum property
        [Attributes.Form_Property("Форма")]
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
        private bool FormNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        //FormNum property

        //NumberOfFields property
        public RamAccess<int> NumberOfFields
        {
            get
            {
                return _dataAccess.Get<int>(nameof(NumberOfFields));
            }
            set
            {
                _dataAccess.Set(nameof(NumberOfFields), value);
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
