using Models.DataAccess;
using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.2: Сведения о наличии ЗРИ в подведомственных организациях")]
    public class Form52 : Abstracts.Form5
    {
        public Form52() : base()
        {
            FormNum = "52";
            NumberOfFields = 6;
        }
        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Radionuclids));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                _Radionuclids_Not_Valid = value;
                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    _dataAccess.Set(nameof(Radionuclids), _Radionuclids_Not_Valid);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation()//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }
        //Radionuclids property

        //Kategory property
        [Attributes.Form_Property("Категория")]
        public short Kategory
        {
            get
            {
                if (GetErrors(nameof(Kategory)) == null)
                {
                    return (short)_dataAccess.Get(nameof(Kategory));
                }
                else
                {
                    return _Kategory_Not_Valid;
                }
            }
            set
            {
                _Kategory_Not_Valid = value;
                if (GetErrors(nameof(Kategory)) == null)
                {
                    _dataAccess.Set(nameof(Kategory), _Kategory_Not_Valid);
                }
                OnPropertyChanged(nameof(Kategory));
            }
        }

        private short _Kategory_Not_Valid = -1;
        private void Kategory_Validation(short value)//TODO
        {
            ClearErrors(nameof(Kategory));
        }
        //Kategory property

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public string Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Activity));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Activity_Not_Valid;
                }
            }
            set
            {
                _Activity_Not_Valid = value;
                if (GetErrors(nameof(Activity)) == null)
                {
                    _dataAccess.Set(nameof(Activity), _Activity_Not_Valid);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }

        private string _Activity_Not_Valid = "";
        private void Activity_Validation(string value)//Ready
        {
            ClearErrors(nameof(Activity));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(Activity), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')))
            {
                AddError(nameof(Activity), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Activity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(Activity), "Недопустимое значение");
            }
        }
        //Activity property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Quantity));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    return _Quantity_Not_Valid;
                }
            }
            set
            {
                Quantity_Validation(value);
                //_Quantity_Not_Valid = value;

                if (GetErrors(nameof(Quantity)) == null)
                {
                    _dataAccess.Set(nameof(Quantity), _Quantity_Not_Valid);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
        private int _Quantity_Not_Valid = -1;
        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }
        //Quantity property
    }
}
