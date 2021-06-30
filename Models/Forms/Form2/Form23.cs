using Models.DataAccess;
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
            //FormNum.Value = "23";
            //NumberOfFields.Value = 17;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ProjectVolume), ProjectVolume_Validation, null);
            ProjectVolume.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(CodeRAO), CodeRAO_Validation, null);
            CodeRAO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Volume), Volume_Validation, null);
            Volume.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Mass), Mass_Validation, null);
            Mass.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(SummaryActivity), SummaryActivity_Validation, null);
            SummaryActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<int?>(nameof(QuantityOZIII), QuantityOZIII_Validation, null);
            QuantityOZIII.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(DocumentNumber), DocumentNumber_Validation, null);
            DocumentNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ExpirationDate), ExpirationDate_Validation, null);
            ExpirationDate.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(DocumentName), DocumentName_Validation, null);
            DocumentName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(DocumentDate), DocumentDate_Validation, null);
            DocumentDate.PropertyChanged += InPropertyChanged;
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
        public int? StoragePlaceNameId { get; set; }
        [Attributes.Form_Property("Наименование ПХ")]
        public virtual RamAccess<string> StoragePlaceName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceName), value);
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

        ////StoragePlaceNameNote property
        //public virtual RamAccess<string> StoragePlaceNameNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(StoragePlaceNameNote));
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(StoragePlaceNameNote), value);
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
        public int? StoragePlaceCodeId { get; set; }
        [Attributes.Form_Property("Код ПХ")]
        public virtual RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceCode));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceCode), value);
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

        //ProjectVolume property
        public int? ProjectVolumeId { get; set; }
        [Attributes.Form_Property("Проектный объем, куб. м")]
        public virtual RamAccess<string> ProjectVolume
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ProjectVolume));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(ProjectVolume), value);
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

        ////ProjectVolumeNote property
        //public virtual RamAccess<double> ProjectVolumeNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<double>(nameof(ProjectVolumeNote));
        //        }

        //        {

        //        }
        //    }
        //    set
        //    {
        //        DataAccess.Set(nameof(ProjectVolumeNote), value);
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
        public int? CodeRAOId { get; set; }
        [Attributes.Form_Property("Код РАО")]
        public virtual RamAccess<string> CodeRAO
        {
            get
            {
                {
                    return DataAccess.Get<string>(nameof(CodeRAO));
                }

                {

                }
            }
            set
            {
                {
                    DataAccess.Set(nameof(CodeRAO), value);
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

        //Volume property
        public int? VolumeId { get; set; }
        [Attributes.Form_Property("Разрешенный объем, куб. м")]
        public virtual RamAccess<string> Volume
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Volume));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Volume), value);
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

        //Mass Property
        public int? MassId { get; set; }
        [Attributes.Form_Property("Разрешенная масса, т")]
        public virtual RamAccess<string> Mass
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Mass));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Mass), value);
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

        //QuantityOZIII property
        public int? QuantityOZIIIId { get; set; }
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public virtual RamAccess<int?> QuantityOZIII
        {
            get
            {

                {
                    return DataAccess.Get<int?>(nameof(QuantityOZIII));//OK
                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityOZIII), value);
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

        //SummaryActivity property
        public int? SummaryActivityId { get; set; }
        [Attributes.Form_Property("Суммарная активность, Бк")]
        public virtual RamAccess<string> SummaryActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(SummaryActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(SummaryActivity), value);
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

        //DocumentNumber property
        public int? DocumentNumberId { get; set; }
        [Attributes.Form_Property("Номер документа")]
        public virtual RamAccess<string> DocumentNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(DocumentNumber));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(DocumentNumber), value);
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

        //DocumentNumberRecoded property
        public virtual RamAccess<string> DocumentNumberRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(DocumentNumberRecoded));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(DocumentNumberRecoded), value);
                }
                OnPropertyChanged(nameof(DocumentNumberRecoded));
            }
        }


        private bool DocumentNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //DocumentNumberRecoded property

        //DocumentDate property
        public int? DocumentDateId { get; set; }
        [Attributes.Form_Property("Дата документа")]
        public virtual RamAccess<string> DocumentDate
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(DocumentDate));//OK
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(DocumentDate), value);
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

        //ExpirationDate property
        public int? ExpirationDateId { get; set; }
        [Attributes.Form_Property("Срок действия документа")]
        public virtual RamAccess<string> ExpirationDate
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ExpirationDate));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(ExpirationDate), value);
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

        //DocumentName property
        public int? DocumentNameId { get; set; }
        [Attributes.Form_Property("Наименование документа")]
        public virtual RamAccess<string> DocumentName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(DocumentName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(DocumentName), value);
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
    }
}
