using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel;

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
        #region  StoragePlaceName
        public string StoragePlaceName_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименование ПХ")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                    var tmp = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                    tmp.PropertyChanged += StoragePlaceNameValueChanged;
                    return tmp;
            }
            set
            {
                    StoragePlaceName_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation

        private void StoragePlaceNameValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                StoragePlaceName_DB = ((RamAccess<string>)Value).Value;
}
}
private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            //List<string> spr = new List<string>();//here binds spr
            //if (!spr.Contains(value.Value))
            //{
            //    value.AddError("Недопустиое значение");
            //    return false;
            //}
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
        #region  StoragePlaceCode
        public string StoragePlaceCode_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Код ПХ")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                    var tmp = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                    tmp.PropertyChanged += StoragePlaceCodeValueChanged;
                    return tmp;
            }
            set
            {
                    StoragePlaceCode_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation

        private void StoragePlaceCodeValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                StoragePlaceCode_DB = ((RamAccess<string>)Value).Value;
}
}
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
            //List<string> spr = new List<string>();//here binds spr
            //if (!spr.Contains(value.Value))
            //{
            //    value.AddError("Недопустиое значение");
            //    return false;
            //}
            //return true;
            Regex a = new Regex("^[0-9]{8}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //StoragePlaceCode property
        #endregion

        //ProjectVolume property
        #region  ProjectVolume
        public string ProjectVolume_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Проектный объем, куб. м")]
        public RamAccess<string> ProjectVolume
        {
            get
            {
                    var tmp = new RamAccess<string>(ProjectVolume_Validation, ProjectVolume_DB);
                    tmp.PropertyChanged += ProjectVolumeValueChanged;
                    return tmp;
            }
            set
            {
                    ProjectVolume_DB = value.Value;
                OnPropertyChanged(nameof(ProjectVolume));
            }
        }


        private void ProjectVolumeValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                ProjectVolume_DB = ((RamAccess<string>)Value).Value;
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
        #region  CodeRAO
        public string CodeRAO_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                    var tmp = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
                    tmp.PropertyChanged += CodeRAOValueChanged;
                    return tmp;
            }
            set
            {
                    CodeRAO_DB = value.Value;
                OnPropertyChanged(nameof(CodeRAO));
            }
        }


        private void CodeRAOValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                CodeRAO_DB = ((RamAccess<string>)Value).Value;
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
        #region  Volume
        public string Volume_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Разрешенный объем, куб. м")]
        public RamAccess<string> Volume
        {
            get
            {
                    var tmp = new RamAccess<string>(Volume_Validation, Volume_DB);
                    tmp.PropertyChanged += VolumeValueChanged;
                    return tmp;
            }
            set
            {
                    Volume_DB = value.Value;
                OnPropertyChanged(nameof(Volume));
            }
        }


        private void VolumeValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Volume_DB = ((RamAccess<string>)Value).Value;
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
        #region  Mass
        public string Mass_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Разрешенная масса, т")]
        public RamAccess<string> Mass
        {
            get
            {
                    var tmp = new RamAccess<string>(Mass_Validation, Mass_DB);
                    tmp.PropertyChanged += MassValueChanged;
                    return tmp;
            }
            set
            {
                    Mass_DB = value.Value;
                OnPropertyChanged(nameof(Mass));
            }
        }


        private void MassValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Mass_DB = ((RamAccess<string>)Value).Value;
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
        #region  QuantityOZIII
        public int? QuantityOZIII_DB { get; set; } = null; [NotMapped]        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<int?> QuantityOZIII
        {
            get
            {
                    var tmp = new RamAccess<int?>(QuantityOZIII_Validation, QuantityOZIII_DB);//OK
                    tmp.PropertyChanged += QuantityOZIIIValueChanged;
                    return tmp;
            }
            set
            {
                    QuantityOZIII_DB = value.Value;
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.

        private void QuantityOZIIIValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                QuantityOZIII_DB = ((RamAccess<int?>)Value).Value;
}
}
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
        #region  SummaryActivity
        public string SummaryActivity_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Суммарная активность, Бк")]
        public RamAccess<string> SummaryActivity
        {
            get
            {
                    var tmp = new RamAccess<string>(SummaryActivity_Validation, SummaryActivity_DB);
                    tmp.PropertyChanged += SummaryActivityValueChanged;
                    return tmp;
            }
            set
            {
                    SummaryActivity_DB = value.Value;
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }


        private void SummaryActivityValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                SummaryActivity_DB = ((RamAccess<string>)Value).Value;
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
        #region  DocumentNumber
        public string DocumentNumber_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Номер документа")]
        public RamAccess<string> DocumentNumber
        {
            get
            {
                    var tmp = new RamAccess<string>(DocumentNumber_Validation, DocumentNumber_DB);//OK
                    tmp.PropertyChanged += DocumentNumberValueChanged;
                    return tmp;
            }
            set
            {
                    DocumentNumber_DB = value.Value;
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }


        private void DocumentNumberValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                DocumentNumber_DB = ((RamAccess<string>)Value).Value;
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
public string DocumentDate_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Дата документа")]
        public RamAccess<string> DocumentDate
        {
            get
            {
                    var tmp = new RamAccess<string>(DocumentDate_Validation, DocumentDate_DB);//OK
                    tmp.PropertyChanged += DocumentDateValueChanged;
                    return tmp;
            }
            set
            {
                DocumentDate_DB = value.Value;
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        //if change this change validation

        private void DocumentDateValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                DocumentDate_DB = ((RamAccess<string>)Value).Value;
}
}
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
        #region  ExpirationDate
        public string ExpirationDate_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Срок действия документа")]
        public RamAccess<string> ExpirationDate
        {
            get
            {
                    var tmp = new RamAccess<string>(ExpirationDate_Validation, ExpirationDate_DB);
                    tmp.PropertyChanged += ExpirationDateValueChanged;
                    return tmp;
            }
            set
            {
                    ExpirationDate_DB = value.Value;
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }


        private void ExpirationDateValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                ExpirationDate_DB = ((RamAccess<string>)Value).Value;
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
        #region  DocumentName
        public string DocumentName_DB { get; set; } = ""; [NotMapped]        [Attributes.Form_Property("Наименование документа")]
        public RamAccess<string> DocumentName
        {
            get
            {
                    var tmp = new RamAccess<string>(DocumentName_Validation, DocumentName_DB);
                    tmp.PropertyChanged += DocumentNameValueChanged;
                    return tmp;
            }
            set
            {
                    DocumentName_DB = value.Value;
                OnPropertyChanged(nameof(DocumentName));
            }
        }


        private void DocumentNameValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                DocumentName_DB = ((RamAccess<string>)Value).Value;
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
