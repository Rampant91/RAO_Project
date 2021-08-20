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
        public Note()
        {
            Init();
        }
        public Note(int rowNumber, int graphNumber, string comment)
        {
            RowNumber.Value = rowNumber;
            GraphNumber.Value = graphNumber;
            Comment.Value = comment;
            Init();
        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }
        public void Init()
        {
            RowNumber.PropertyChanged += InPropertyChanged;
            GraphNumber.PropertyChanged += InPropertyChanged;
            Comment.PropertyChanged += InPropertyChanged;
            RowNumber_Validation(RowNumber);
            GraphNumber_Validation(GraphNumber);
            Comment_Validation(Comment);
        }

        public int Id { get; set; }

        #region RowNUmber
        public int? RowNumber_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("Номер строки")]
        public RamAccess<int?> RowNumber
        {
            get
            {
                var tmp = new RamAccess<int?>(RowNumber_Validation, RowNumber_DB);
                tmp.PropertyChanged += RowNumberValueChanged;
                return tmp;
            }
            set
            {
                RowNumber_DB = value.Value;
                OnPropertyChanged(nameof(RowNumber));
            }
        }
        private void RowNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RowNumber_DB = ((RamAccess<int?>)Value).Value;
            }
        }
        private bool RowNumber_Validation(RamAccess<int?> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region GraphNumber
        public int? GraphNumber_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("Номер графы")]
        public RamAccess<int?> GraphNumber
        {
            get
            {
                var tmp = new RamAccess<int?>(GraphNumber_Validation, GraphNumber_DB);
                tmp.PropertyChanged += GraphNumberValueChanged;
                return tmp;
            }
            set
            {
                GraphNumber_DB = value.Value;
                OnPropertyChanged(nameof(GraphNumber));
            }
        }
        private void GraphNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                GraphNumber_DB = ((RamAccess<int?>)Value).Value;
            }
        }
        private bool GraphNumber_Validation(RamAccess<int?> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region Comment
        public string Comment_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Примечание")]
        public RamAccess<string> Comment
        {
            get
            {
                var tmp = new RamAccess<string>(Comment_Validation, Comment_DB);
                tmp.PropertyChanged += CommentValueChanged;
                return tmp;
            }
            set
            {
                Comment_DB = value.Value;
                OnPropertyChanged(nameof(Comment));
            }
        }
        private void CommentValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Comment_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool Comment_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        //Для валидации
        public bool Object_Validation()
        {
            return true;
        }
        //Для валидации

        //Property Changed
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed
    }
}
