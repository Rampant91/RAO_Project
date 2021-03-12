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
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 2;

        private string _idName = "";
        public string IdName
        {
            get { return _idName; }
            set
            {
                _idName = value;
                OnPropertyChanged("IdName");
            }
        }

        private string _value = "";
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }
    }
}
