using System;
using System.Globalization;
using DBRealization;
using Collections.Rows_Collection;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
    public class Form19: Abstracts.Form1
    {
        public Form19(IDataAccess Access) : base(Access)
        {

        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //CodeTypeAccObject property
        [Attributes.Form_Property("Код типа объектов учета")]
        public short CodeTypeAccObject
        {
            get
            {
                if (GetErrors(nameof(CodeTypeAccObject)) == null)
                {
                    return (short)_dataAccess.Get(nameof(CodeTypeAccObject))[0][0];
                }
                else
                {
                    return _CodeTypeAccObject_Not_Valid;
                }
            }
            set
            {
                _CodeTypeAccObject_Not_Valid = value;
                if (GetErrors(nameof(CodeTypeAccObject)) == null)
                {
                    _dataAccess.Set(nameof(CodeTypeAccObject), _CodeTypeAccObject_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeTypeAccObject));
            }
        }
        
        private short _CodeTypeAccObject_Not_Valid = 0;
        private void CodeTypeAccObject_Validation(short value)//TODO
        {
            ClearErrors(nameof(CodeTypeAccObject));
        }
        //CodeTypeAccObject property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Radionuclids))[0][0];                }
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
                    _dataAccess.Set(nameof(Radionuclids), _Radionuclids_Not_Valid);                }
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

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public string Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Activity))[0][0];
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
    }
}
