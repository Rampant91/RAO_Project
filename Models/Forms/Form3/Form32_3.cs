using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Идентификаторы:")]
    public class Form32_3 : Abstracts.Form3
    {
        public Form32_3() : base()
        {
            FormNum = "32_3";
            NumberOfFields = 2;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //IdName property
        [Attributes.Form_Property("Идентификатор")]
        public string IdName
        {
            get
            {
                if (GetErrors(nameof(IdName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(IdName));
                }
                else
                {
                    return _IdName_Not_Valid;
                }
            }
            set
            {
                _IdName_Not_Valid = Val;
                if (GetErrors(nameof(IdName)) == null)
                {
                    _dataAccess.Set(nameof(IdName), _IdName_Not_Valid);
                }
                OnPropertyChanged(nameof(IdName));
            }
        }

        private string _IdName_Not_Valid = "";
        //IdName Property

        //Val property
        [Attributes.Form_Property("Значение")]
        public string Val
        {
            get
            {
                if (GetErrors(nameof(Val)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Val));
                }
                else
                {
                    return _Val_Not_Valid;
                }
            }
            set
            {
                _Val_Not_Valid = Val;
                if (GetErrors(nameof(Val)) == null)
                {
                    _dataAccess.Set(nameof(Val), _Val_Not_Valid);
                }
                OnPropertyChanged(nameof(Val));
            }
        }

        private string _Val_Not_Valid = "";
        //Val property
    }
}
