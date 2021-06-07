using Models.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;
using Avalonia.Data;

namespace Models.Abstracts
{
    public abstract class Form : INotifyPropertyChanged
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
