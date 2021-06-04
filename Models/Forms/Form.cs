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
        private void FormNum_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
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
        private void NumberOfFields_Validation(IDataAccess<int> value)
        {
            value.ClearErrors();
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
