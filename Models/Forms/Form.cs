using Collections;
using Models.DataAccess;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Models.Abstracts
{
    public abstract class Form : INotifyPropertyChanged, IKey
    {
        public int Id { get; set; }

        #region FormNum
        public string FormNum_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Форма")]
        public RamAccess<string> FormNum
        {
            get => new RamAccess<string>(FormNum_Validation,FormNum_DB);
            set
            {
                FormNum_DB = value.Value;
                OnPropertyChanged(nameof(FormNum));
            }
        }
        private bool FormNum_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region NumberOfFields
        public int NumberOfFields_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("Число полей")]
        public RamAccess<int> NumberOfFields
        {
            get => new RamAccess<int>(NumberOfFields_Validation, NumberOfFields_DB);
            set
            {
                NumberOfFields_DB = value.Value;
                OnPropertyChanged(nameof(NumberOfFields));
            }
        }
        private bool NumberOfFields_Validation(RamAccess<int> value)
        {
            value.ClearErrors();
            return true;

        }
        #endregion

        #region For_Validation
        public abstract bool Object_Validation();
        #endregion

        #region INotifyPropertyChanged
        protected void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
