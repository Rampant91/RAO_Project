using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Идентификаторы:")]
    public class Form32_3: Form3
    {
        public Form32_3(bool isSQL) : base()
        {
            FormNum = "32_3";
            NumberOfFields = 2;
            if (isSQL)
            {
                _IdName = new SQLite("IdName", FormNum, 0);
                _Value = new SQLite("Value", FormNum, 0);
            }
            else
            {
                _IdName = new File();
                _Value = new File();
            }
        }

        [Attributes.FormVisual("Форма")]
        public override void Object_Validation()
        {

        }

        //IdName property
        [Attributes.FormVisual("Идентификатор")]
        public string IdName
        {
            get
            {
                if (GetErrors(nameof(IdName)) != null)
                {
                    return (string)_IdName.Get();
                }
                else
                {
                    return _IdName_Not_Valid;
                }
            }
            set
            {
                _IdName_Not_Valid = value;
                if (GetErrors(nameof(IdName)) != null)
                {
                    _IdName.Set(_IdName_Not_Valid);
                }
                OnPropertyChanged(nameof(IdName));
            }
        }
        private IDataLoadEngine _IdName;
        private string _IdName_Not_Valid = "";
        //IdName Property

        //Value property
        [Attributes.FormVisual("Значение")]
        public string Value
        {
            get
            {
                if (GetErrors(nameof(Value)) != null)
                {
                    return (string)_Value.Get();
                }
                else
                {
                    return _Value_Not_Valid;
                }
            }
            set
            {
                _Value_Not_Valid = value;
                if (GetErrors(nameof(Value)) != null)
                {
                    _Value.Set(_Value_Not_Valid);
                }
                OnPropertyChanged(nameof(Value));
            }
        }
        private IDataLoadEngine _Value;
        private string _Value_Not_Valid = "";
        //Value property
    }
}
