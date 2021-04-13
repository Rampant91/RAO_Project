﻿using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Идентификаторы:")]
    public class Form32_3: Abstracts.Form3
    {
        public Form32_3(int RowID) : base(RowID)
        {
            FormNum = "32_3";
            NumberOfFields = 2;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //IdName property
        [Attributes.Form_Property("Идентификатор")]
        public string IdName
        {
            get
            {
                if (GetErrors(nameof(IdName)) != null)
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
                _IdName_Not_Valid = value;
                if (GetErrors(nameof(IdName)) != null)
                {
                    _dataAccess.Set(nameof(IdName), _IdName_Not_Valid);
                }
                OnPropertyChanged(nameof(IdName));
            }
        }
        
        private string _IdName_Not_Valid = "";
        //IdName Property

        //Value property
        [Attributes.Form_Property("Значение")]
        public string Value
        {
            get
            {
                if (GetErrors(nameof(Value)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Value));
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
                    _dataAccess.Set(nameof(Value), _Value_Not_Valid);
                }
                OnPropertyChanged(nameof(Value));
            }
        }
        
        private string _Value_Not_Valid = "";
        //Value property
    }
}