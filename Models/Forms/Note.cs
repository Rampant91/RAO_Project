using Models.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Note : INotifyPropertyChanged
    {
        protected IDataAccessCollection _dataAccess { get; set; }
        public Note(IDataAccessCollection Access)
        {
            _dataAccess = Access;
        }
        public Note()
        {
            _dataAccess = new DataAccessCollection();
        }
        [Key]
        public int NoteId { get; set; }

        //RowNumber property
        [Attributes.Form_Property("Номер строки")]
        public RamAccess<int> RowNumber
        {
            get
            {
                return _dataAccess.Get<int>(nameof(RowNumber));
            }
            set
            {
                _dataAccess.Set(nameof(RowNumber),value);
                OnPropertyChanged(nameof(RowNumber));
            }
        }
        private bool RowNumber_Validation(IDataAccess<int> value)
        {
            value.ClearErrors();
            return true;
        }
        //RowNumber property

        //GraphNumber property
        [Attributes.Form_Property("Номер графы")]
        public RamAccess<int> GraphNumber
        {
            get
            {
                return _dataAccess.Get<int>(nameof(GraphNumber));
            }
            set
            {
                _dataAccess.Set(nameof(GraphNumber), value);
                OnPropertyChanged(nameof(GraphNumber));
            }
        }
        private bool GraphNumber_Validation(IDataAccess<int> value)
        {
            value.ClearErrors();
            return true;
        }
        //GraphNumber property

        //Comment property
        [Attributes.Form_Property("Комментарий")]
        public RamAccess<string> Comment
        {
            get
            {
                return _dataAccess.Get<string>(nameof(Comment));
            }
            set
            {
                _dataAccess.Set(nameof(Comment), value);
                OnPropertyChanged(nameof(Comment));
            }
        }
        private bool Comment_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        //Comment property

        //Для валидации
        public bool Object_Validation()
        {
            return true;
        }
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
