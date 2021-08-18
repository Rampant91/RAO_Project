using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.3: Разрешение на размещение РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form23 : Abstracts.Form2
    {
        public Form23() : base()
        {
            FormNum.Value = "2.3";
            //NumberOfFields.Value = 17;
            Validate_all();
        }

        private void Validate_all()
        {
            StoragePlaceName_Validation(StoragePlaceName);
            StoragePlaceCode_Validation(StoragePlaceCode);
            ProjectVolume_Validation(ProjectVolume);
            CodeRAO_Validation(CodeRAO);
            Volume_Validation(Volume);
            Mass_Validation(Mass);
            SummaryActivity_Validation(SummaryActivity);
            QuantityOZIII_Validation(QuantityOZIII);
            DocumentNumber_Validation(DocumentNumber);
            DocumentNumberRecoded_Validation(DocumentNumberRecoded);
            ExpirationDate_Validation(ExpirationDate);
            DocumentName_Validation(DocumentName);
            DocumentDate_Validation(DocumentDate);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //StoragePlaceName property
#region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Наименование ПХ")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(StoragePlaceName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    StoragePlaceName_DB = value.Value;
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation

        private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<string> spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустиое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceName property
        #endregion

        ////StoragePlaceNameNote property
        //public RamAccess<string> StoragePlaceNameNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(StoragePlaceNameNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            StoragePlaceNameNote_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(StoragePlaceNameNote));
        //    }
        //}
        ////If change this change validation

        //private bool StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors(); return true;
        //}
        ////StoragePlaceNameNote property

        //StoragePlaceCode property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Код ПХ")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(StoragePlaceCode_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    StoragePlaceCode_DB = value.Value;
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation

        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
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
            List<string> spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустиое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceCode property
        #endregion

        //ProjectVolume property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Проектный объем, куб. м")]
        public RamAccess<string> ProjectVolume
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(ProjectVolume_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    ProjectVolume_DB = value.Value;
                }
                OnPropertyChanged(nameof(ProjectVolume));
            }
        }


        private bool ProjectVolume_Validation(RamAccess<string> value)//TODO
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
            if (!((value.Value.Contains('e') || value.Value.Contains('E'))))
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
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //ProjectVolume property
        #endregion

        ////ProjectVolumeNote property
        //public RamAccess<double> ProjectVolumeNote
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<double>(ProjectVolumeNote_Validation, _DB);
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {
        //        ProjectVolumeNote_DB = value.Value;
        //        OnPropertyChanged(nameof(ProjectVolumeNote));
        //    }
        //}


        //private bool ProjectVolumeNote_Validation(RamAccess<double?> value)//TODO
        //{
        //    value.ClearErrors();
        //    return true;
        //}
        ////ProjectVolumeNote property

        //CodeRAO property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                {
                    var tmp = new RamAccess<string>(CodeRAO_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                {
                    CodeRAO_DB = value.Value;
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }


        private bool CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9X]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAO property
        #endregion

        //Volume property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Разрешенный объем, куб. м")]
        public RamAccess<string> Volume
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(Volume_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    Volume_DB = value.Value;
                }
                OnPropertyChanged(nameof(Volume));
            }
        }


        private bool Volume_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        //Volume property
        #endregion

        //Mass Property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Разрешенная масса, т")]
        public RamAccess<string> Mass
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(Mass_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    Mass_DB = value.Value;
                }
                OnPropertyChanged(nameof(Mass));
            }
        }


        private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        //Mass Property
        #endregion

        //QuantityOZIII property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<int?> QuantityOZIII
        {
            get
            {

                {
                    var tmp = new RamAccess<int?>(QuantityOZIII_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {



                {
                    QuantityOZIII_DB = value.Value;
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.

        private bool QuantityOZIII_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                return true;
            }
            if ((int)value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //QuantityOZIII property
        #endregion

        //SummaryActivity property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Суммарная активность, Бк")]
        public RamAccess<string> SummaryActivity
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(SummaryActivity_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    SummaryActivity_DB = value.Value;
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }


        private bool SummaryActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        //SummaryActivity property
        #endregion

        //DocumentNumber property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Номер документа")]
        public RamAccess<string> DocumentNumber
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(DocumentNumber_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;

                }

                {

                }
            }
            set
            {


                {
                    DocumentNumber_DB = value.Value;
                }
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }


        private bool DocumentNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //DocumentNumber property
        #endregion

        //DocumentNumberRecoded property
        //public RamAccess<string> DocumentNumberRecoded
        //{
        //    get
        //    {

        //        {
        //            var tmp = new RamAccess<string>(DocumentNumberRecoded_Validation, _DB);//OK
        //            tmp.PropertyChanged += ValueChanged;
        //            return tmp;

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DocumentNumberRecoded_DB = value.Value;
        //        }
        //        OnPropertyChanged(nameof(DocumentNumberRecoded));
        //    }
        //}


        //private bool DocumentNumberRecoded_Validation(RamAccess<string> value)//Ready
        //{
        //    value.ClearErrors(); return true;
        //}
        //DocumentNumberRecoded property

        //DocumentDate property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Дата документа")]
        public RamAccess<string> DocumentDate
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(DocumentDate_Validation, _DB);//OK
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {
                DocumentDate_DB = value.Value;
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        //if change this change validation

        private bool DocumentDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //DocumentDate property
        #endregion

        //ExpirationDate property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Срок действия документа")]
        public RamAccess<string> ExpirationDate
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(ExpirationDate_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    ExpirationDate_DB = value.Value;
                }
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }


        private bool ExpirationDate_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ExpirationDate property
        #endregion

        //DocumentName property
        #region  
public int _DB { get; set; } = 0; [NotMapped]        [Attributes.Form_Property("Наименование документа")]
        public RamAccess<string> DocumentName
        {
            get
            {

                {
                    var tmp = new RamAccess<string>(DocumentName_Validation, _DB);
                    tmp.PropertyChanged += ValueChanged;
                    return tmp;
                }

                {

                }
            }
            set
            {


                {
                    DocumentName_DB = value.Value;
                }
                OnPropertyChanged(nameof(DocumentName));
            }
        }


        private bool DocumentName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //DocumentName property
        #endregion
    }
}
