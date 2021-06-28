using Models.DataAccess;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
    public class Form27 : Abstracts.Form2
    {
        public Form27() : base()
        {
            //FormNum.Value = "27";
            //NumberOfFields.Value = 13;
            Init();
            Validate_all();
        }

        private void Init()
        {
            _dataAccess.Init<string>(nameof(ObservedSourceNumber), ObservedSourceNumber_Validation, null);
            ObservedSourceNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(RadionuclidName), RadionuclidName_Validation, null);
            RadionuclidName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(AllowedWasteValue), AllowedWasteValue_Validation, null);
            AllowedWasteValue.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FactedWasteValue), FactedWasteValue_Validation, null);
            FactedWasteValue.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(WasteOutbreakPreviousYear), WasteOutbreakPreviousYear_Validation, null);
            WasteOutbreakPreviousYear.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            ObservedSourceNumber_Validation(ObservedSourceNumber);
            RadionuclidName_Validation(RadionuclidName);
            AllowedWasteValue_Validation(AllowedWasteValue);
            FactedWasteValue_Validation(FactedWasteValue);
            WasteOutbreakPreviousYear_Validation(WasteOutbreakPreviousYear);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //ObservedSourceNumber property
        [Attributes.Form_Property("Номер наблюдательной скважины")]public int? ObservedSourceNumberId { get; set; }
        public virtual RamAccess<string> ObservedSourceNumber
        {
            get
            {
                return _dataAccess.Get<string>(nameof(ObservedSourceNumber));
            }
            set
            {
                _dataAccess.Set(nameof(ObservedSourceNumber), value);
                OnPropertyChanged(nameof(ObservedSourceNumber));
            }
        }
        //If change this change validation
        private bool ObservedSourceNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //ObservedSourceNumber property

        //PermissionNumber property
        [Attributes.Form_Property("Номер разрешительного документа")]public int? PermissionNumberId { get; set; }
        public virtual RamAccess<string> PermissionNumber
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(PermissionNumber));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(PermissionNumber), value);
                }
                OnPropertyChanged(nameof(PermissionNumber));
            }
        }


        private bool PermissionNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            return true;
        }
        //PermissionNumber property

        //PermissionIssueDate property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]public int? PermissionIssueDateId { get; set; }
        public virtual RamAccess<string> PermissionIssueDate
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(PermissionIssueDate));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(PermissionIssueDate), value);
                }
                OnPropertyChanged(nameof(PermissionIssueDate));
            }
        }


        private bool PermissionIssueDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionIssueDate property

        //PermissionDocumentName property
        [Attributes.Form_Property("Наименование разрешительного документа")]public int? PermissionDocumentNameId { get; set; }
        public virtual RamAccess<string> PermissionDocumentName
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(PermissionDocumentName));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(PermissionDocumentName), value);
                }
                OnPropertyChanged(nameof(PermissionDocumentName));
            }
        }


        private bool PermissionDocumentName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionDocumentName property

        //ValidBegin property
        [Attributes.Form_Property("Действует с")]public int? ValidBeginId { get; set; }
        public virtual RamAccess<string> ValidBegin
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(ValidBegin));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(ValidBegin), value);
                }
                OnPropertyChanged(nameof(ValidBegin));
            }
        }


        private bool ValidBegin_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidBegin property

        //ValidThru property
        [Attributes.Form_Property("Действует по")]public int? ValidThruId { get; set; }
        public virtual RamAccess<string> ValidThru
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(ValidThru));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(ValidThru), value);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }


        private bool ValidThru_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidThru property

        //RadionuclidName property
        public virtual RamAccess<string> RadionuclidName
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(RadionuclidName));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(RadionuclidName), value);
                }
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }


        private bool RadionuclidName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var spr = new List<Tuple<string, string>>();
            foreach (var item in spr)
            {
                if (item.Item1.Equals(value.Value))
                    return true;
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //RadionuclidName property

        //RadionuclidNameNote property
        public virtual RamAccess<string> RadionuclidNameNote
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(RadionuclidNameNote));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(RadionuclidNameNote), value);
                }
                OnPropertyChanged(nameof(RadionuclidNameNote));
            }
        }


        private bool RadionuclidNameNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //RadionuclidNameNote property

        //AllowedWasteValue property
        [Attributes.Form_Property("Разрешенный выброс радионуклида в атмосферу за отчетный год, Бк")]public int? AllowedWasteValueId { get; set; }
        public virtual RamAccess<string> AllowedWasteValue
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(AllowedWasteValue));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(AllowedWasteValue), value);
                }
                OnPropertyChanged(nameof(AllowedWasteValue));
            }
        }


        private bool AllowedWasteValue_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //AllowedWasteValue property

        //AllowedWasteValueNote property
        public virtual RamAccess<string> AllowedWasteValueNote
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(AllowedWasteValueNote));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(AllowedWasteValueNote), value);
                }
                OnPropertyChanged(nameof(AllowedWasteValueNote));
            }
        }


        private bool AllowedWasteValueNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //AllowedWasteValueNote property

        //FactedWasteValue property
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за отчетный год, Бк")]public int? FactedWasteValueId { get; set; }
        public virtual RamAccess<string> FactedWasteValue
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(FactedWasteValue));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(FactedWasteValue), value);
                }
                OnPropertyChanged(nameof(FactedWasteValue));
            }
        }


        private bool FactedWasteValue_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //FactedWasteValue property

        //FactedWasteValueNote property
        public virtual RamAccess<string> FactedWasteValueNote
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(FactedWasteValueNote));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(FactedWasteValueNote), value);
                }
                OnPropertyChanged(nameof(FactedWasteValueNote));
            }
        }


        private bool FactedWasteValueNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //FactedWasteValueNote property

        //WasteOutbreakPreviousYear property
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за предыдущий год, Бк")]public int? WasteOutbreakPreviousYearId { get; set; }
        public virtual RamAccess<string> WasteOutbreakPreviousYear
        {
            get
            {

                {
                    return _dataAccess.Get<string>(nameof(WasteOutbreakPreviousYear));
                }

                {

                }
            }
            set
            {


                {
                    _dataAccess.Set(nameof(WasteOutbreakPreviousYear), value);
                }
                OnPropertyChanged(nameof(WasteOutbreakPreviousYear));
            }
        }


        private bool WasteOutbreakPreviousYear_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //WasteOutbreakPreviousYear property
    }
}
