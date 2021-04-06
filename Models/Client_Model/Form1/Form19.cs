using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
    public class Form19: Form1
    {
        public Form19(bool isSQL) : base()
        {
            FormNum = "19";
            NumberOfFields = 11;
            if (isSQL)
            {
                _Radionuclids = new SQLite("Radionuclids", FormNum, 0);
                _Radionuclids = new SQLite("Radionuclids", FormNum, 0);
                _Activity = new SQLite("Activity", FormNum, 0);
            }
            else
            {
                _Radionuclids = new File();
                _Radionuclids = new File();
                _Activity = new File();
            }
        }

        [Attributes.FormVisual("Форма")]
        public override void Object_Validation()
        {

        }

        //CodeTypeAccObject property
        [Attributes.FormVisual("Код типа объектов учета")]
        public short CodeTypeAccObject
        {
            get
            {
                if (GetErrors(nameof(CodeTypeAccObject)) != null)
                {
                    return (short)_CodeTypeAccObject.Get();
                }
                else
                {
                    return _CodeTypeAccObject_Not_Valid;
                }
            }
            set
            {
                _CodeTypeAccObject_Not_Valid = value;
                if (GetErrors(nameof(CodeTypeAccObject)) != null)
                {
                    _CodeTypeAccObject.Set(_CodeTypeAccObject_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeTypeAccObject));
            }
        }
        private IDataLoadEngine _CodeTypeAccObject;
        private short _CodeTypeAccObject_Not_Valid = 0;
        private void CodeTypeAccObject_Validation(short value)//TODO
        {
            ClearErrors(nameof(CodeTypeAccObject));
        }
        //CodeTypeAccObject property

        //Radionuclids property
        [Attributes.FormVisual("Радионуклиды")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    return (string)_Radionuclids.Get();
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                _Radionuclids_Not_Valid = value;
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    _Radionuclids.Set(_Radionuclids_Not_Valid);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        private IDataLoadEngine _Radionuclids;//If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation()//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }
        //Radionuclids property

        //Activity property
        [Attributes.FormVisual("Активность, Бк")]
        public string Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) != null)
                {
                    return (string)_Activity.Get();
                }
                else
                {
                    return _Activity_Not_Valid;
                }
            }
            set
            {
                _Activity_Not_Valid = value;
                if (GetErrors(nameof(Activity)) != null)
                {
                    _Activity.Set(_Activity_Not_Valid);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }
        private IDataLoadEngine _Activity;
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
