using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
    public class Form27 : Abstracts.Form2
    {
        public Form27() : base()
        {
            FormNum.Value = "2.7";
            //NumberOfFields.Value = 13;
            Validate_all();
        }

        private void Validate_all()
        {
            ValidThru_Validation(ValidThru);
            ValidBegin_Validation(ValidBegin);
            PermissionNumber_Validation(PermissionNumber);
            PermissionIssueDate_Validation(PermissionIssueDate);
            PermissionDocumentName_Validation(PermissionDocumentName);
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
#region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Номер источника выбросов")]
        public RamAccess<string> ObservedSourceNumber
        {
            get => new RamAccess<string>(ObservedSourceNumber_Validation, _DB);
            set
            {
                ObservedSourceNumber_DB = value.Value;
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
        #endregion

        //PermissionNumber property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PermissionNumber_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PermissionNumber_DB = value.Value;
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
        #endregion

        //PermissionIssueDate property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PermissionIssueDate_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PermissionIssueDate_DB = value.Value;
                }
                OnPropertyChanged(nameof(PermissionIssueDate));
            }
        }


        private bool PermissionIssueDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionIssueDate property
        #endregion

        //PermissionDocumentName property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(PermissionDocumentName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    PermissionDocumentName_DB = value.Value;
                }
                OnPropertyChanged(nameof(PermissionDocumentName));
            }
        }


        private bool PermissionDocumentName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PermissionDocumentName property
        #endregion

        //ValidBegin property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(ValidBegin_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    ValidBegin_DB = value.Value;
                }
                OnPropertyChanged(nameof(ValidBegin));
            }
        }


        private bool ValidBegin_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidBegin property
        #endregion

        //ValidThru property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(ValidThru_Validation, _DB);
                }

                {

                }
            }
            set
            {


                {
                    ValidThru_DB = value.Value;
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }


        private bool ValidThru_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ValidThru property
        #endregion

        //RadionuclidName property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Наименование радионуклида")]
        public RamAccess<string> RadionuclidName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(RadionuclidName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    RadionuclidName_DB = value.Value;
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
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();
            foreach (Tuple<string, string> item in spr)
            {
                if (item.Item1.Equals(value.Value))
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //RadionuclidName property
        #endregion

        ////RadionuclidNameNote property
        //public RamAccess<string> RadionuclidNameNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(RadionuclidNameNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            RadionuclidNameNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(RadionuclidNameNote));
        //    }
        //}


        //private bool RadionuclidNameNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////RadionuclidNameNote property

        //AllowedWasteValue property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Разрешенный выброс радионуклида в атмосферу за отчетный год, Бк")]
        public RamAccess<string> AllowedWasteValue
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(AllowedWasteValue_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    AllowedWasteValue_DB = value.Value;
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        #endregion

        ////AllowedWasteValueNote property
        //public RamAccess<string> AllowedWasteValueNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(AllowedWasteValueNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            AllowedWasteValueNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(AllowedWasteValueNote));
        //    }
        //}


        //private bool AllowedWasteValueNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////AllowedWasteValueNote property

        //FactedWasteValue property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за отчетный год, Бк")]
        public RamAccess<string> FactedWasteValue
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(FactedWasteValue_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    FactedWasteValue_DB = value.Value;
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        #endregion

        ////FactedWasteValueNote property
        //public RamAccess<string> FactedWasteValueNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(FactedWasteValueNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            FactedWasteValueNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(FactedWasteValueNote));
        //    }
        //}


        //private bool FactedWasteValueNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////FactedWasteValueNote property

        //WasteOutbreakPreviousYear property
        #region  
public int _DB { get; set; } = 0; [NotMapped]
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за предыдущий год, Бк")]
        public RamAccess<string> WasteOutbreakPreviousYear
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(WasteOutbreakPreviousYear_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    WasteOutbreakPreviousYear_DB = value.Value;
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
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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
        #endregion
    }
}
