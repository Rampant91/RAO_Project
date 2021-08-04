using Collections;
using Models.DataAccess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Collections;

namespace Models
{
    public class Note : INotifyPropertyChanged
    {
        protected DataAccessCollection DataAccess { get; set; }
        public Note(DataAccessCollection Access)
        {
            DataAccess = Access;
        }
        public Note(int rowNumber, int graphNumber, string comment)
        {
            DataAccess = new DataAccessCollection();
            RowNumber.Value = rowNumber;
            GraphNumber.Value = graphNumber;
            Comment.Value = comment;
        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }
        public Note()
        {
            DataAccess = new DataAccessCollection();
            DataAccess.Init<int?>(nameof(RowNumber), RowNumber_Validation, null);
            RowNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<int?>(nameof(GraphNumber), GraphNumber_Validation, null);
            GraphNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Comment), Comment_Validation, null);
            Comment.PropertyChanged += InPropertyChanged;
            RowNumber_Validation(RowNumber);
            GraphNumber_Validation(GraphNumber);
            Comment_Validation(Comment);
        }
        [Key]
        public int NoteId { get; set; }

        //RowNumber property
        [Attributes.Form_Property("Номер строки")]
        public RamAccess<int?> RowNumber
        {
            get => DataAccess.Get<int?>(nameof(RowNumber));
            set
            {
                DataAccess.Set(nameof(RowNumber), value);
                OnPropertyChanged(nameof(RowNumber));
            }
        }
        private bool RowNumber_Validation(RamAccess<int?> value)
        {
            value.ClearErrors();
            return true;
        }
        //RowNumber property

        //GraphNumber property
        [Attributes.Form_Property("Номер графы")]
        public RamAccess<int?> GraphNumber
        {
            get => DataAccess.Get<int?>(nameof(GraphNumber));
            set
            {
                DataAccess.Set(nameof(GraphNumber), value);
                OnPropertyChanged(nameof(GraphNumber));
            }
        }
        private bool GraphNumber_Validation(RamAccess<int?> value)
        {
            value.ClearErrors();
            return true;
        }
        //GraphNumber property

        //Comment property
        [Attributes.Form_Property("Примечание")]
        public RamAccess<string> Comment
        {
            get => DataAccess.Get<string>(nameof(Comment));
            set
            {
                DataAccess.Set(nameof(Comment), value);
                OnPropertyChanged(nameof(Comment));
            }
        }
        private bool Comment_Validation(RamAccess<string> value)
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
