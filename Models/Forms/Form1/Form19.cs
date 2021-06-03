using Models.DataAccess;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.9: Сведения о результатах инвентаризации РВ не в составе ЗРИ")]
    public class Form19 : Abstracts.Form1
    {
        public Form19() : base()
        {
            FormNum = "19";
            NumberOfFields = 13;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int? Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) == null)
                {
                    return (int?)_dataAccess.Get(nameof(Quantity));//OK;
                }
                else
                {
                    return _Quantity_Not_Valid;
                }
            }
            set
            {
                Quantity_Validation(value);

                if (GetErrors(nameof(Quantity)) == null)
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
        private int? _Quantity_Not_Valid = null;
        private void Quantity_Validation(int? value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value == null) return;
            if (value <= 0)
            {
                AddError(nameof(Quantity), "Недопустимое значение");
                return;
            }
        }
        //Quantity property

        //CodeTypeAccObject property
        [Attributes.Form_Property("Код типа объектов учета")]
        public short? CodeTypeAccObject
        {
            get
            {
                if (GetErrors(nameof(CodeTypeAccObject)) == null)
                {
                    return (short?)_dataAccess.Get(nameof(CodeTypeAccObject));
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
                    _dataAccess.Set(nameof(CodeTypeAccObject), value);
                }
                OnPropertyChanged(nameof(CodeTypeAccObject));
            }
        }

        private short? _CodeTypeAccObject_Not_Valid = 0;
        private void CodeTypeAccObject_Validation(short? value)//TODO
        {
            ClearErrors(nameof(CodeTypeAccObject));
            if (value == null)
            {
                AddError(nameof(CodeTypeAccObject), "Поле не заполнено");
                return;
            }
            List<short> spr = new List<short>();
            if (!spr.Contains((short)value))
            {
                AddError(nameof(CodeTypeAccObject), "Недопустимое значение");
                return;
            }
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
                Radionuclids_Validation(value);

                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation(string value)//TODO
        {
            ClearErrors(nameof(Radionuclids));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(Radionuclids), "Поле не заполнено");
                return;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value))
                {
                    Radionuclids = item.Item2;
                    return;
                }
            }
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
                    _dataAccess.Set(nameof(Activity), value);
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
            if (!(value.Contains('e')||value.Contains('E')))
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

        protected override void OperationCode_Validation(short? arg)//OK
        {
            ClearErrors(nameof(OperationCode));
            OperationCode = 10;
        }

        protected override void OperationDate_Validation(string value)
        {
            ClearErrors(nameof(OperationDate));
            if ((value == null) || value.Equals(_OperationDate_Not_Valid))
            {
                AddError(nameof(OperationDate), "Поле не заполнено");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(OperationDate), "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value); }
            catch (Exception)
            {
                AddError(nameof(OperationDate), "Недопустимое значение");
                return;
            }
            var date = DateTimeOffset.Parse(value);
            if (date.Date > DateTimeOffset.Now.Date)
            {
                AddError(nameof(OperationDate), "Недопустимое значение");
                return;
            }
        }

        protected override void DocumentDate_Validation(string value)
        {
            ClearErrors(nameof(DocumentDate));
            if ((value == null) || value.Equals(_DocumentDate_Not_Valid))
            {
                AddError(nameof(DocumentDate), "Поле не заполнено");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(DocumentDate), "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value); }
            catch (Exception)
            {
                AddError(nameof(DocumentDate), "Недопустимое значение");
                return;
            }
            var date = DateTimeOffset.Parse(value);
            if (date.Date > DateTimeOffset.Now.Date)
            {
                AddError(nameof(DocumentDate), "Недопустимое значение");
                return;
            }
        }

        protected override void DocumentNumber_Validation(string value)
        {
            ClearErrors(nameof(DocumentNumber));
            if ((value == null) || value.Equals(_DocumentNumber_Not_Valid))//ok
            {
                AddError(nameof(DocumentNumber), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {

            }
        }
    }
}
