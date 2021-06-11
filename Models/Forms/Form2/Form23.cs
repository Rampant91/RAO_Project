using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.3: Разрешение на размещение РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form23 : Abstracts.Form2
    {
        public Form23() : base()
        {
            FormNum.Value = "23";
            NumberOfFields.Value = 17;
            Init();
            Validate_all();
        }

        private void Init()
        {
            _dataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(ProjectVolume), ProjectVolume_Validation, null);
            ProjectVolume.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(CodeRAO), CodeRAO_Validation, null);
            CodeRAO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Volume), Volume_Validation, null);
            Volume.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Mass), Mass_Validation, null);
            Mass.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(SummaryActivity), SummaryActivity_Validation, null);
            SummaryActivity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<int?>(nameof(QuantityOZIII), QuantityOZIII_Validation, null);
            QuantityOZIII.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(DocumentNumber), DocumentNumber_Validation, null);
            DocumentNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(ExpirationDate), ExpirationDate_Validation, null);
            ExpirationDate.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(DocumentName), DocumentName_Validation, null);
            DocumentName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(DocumentDate), DocumentDate_Validation, null);
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
        [Attributes.Form_Property("Наименование ПХ")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(StoragePlaceName), value);
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
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError("Недопустиое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public RamAccess<string> StoragePlaceNameNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceNameNote));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), value);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation
        
        private bool StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //StoragePlaceNameNote property

        //StoragePlaceCode property
        [Attributes.Form_Property("Код ПХ")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceCode));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), value);
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
            var spr = new List<string>();
            if (!spr.Contains(value.Value))
            {
                value.AddError( "Недопустиое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceCode property

        //ProjectVolume property
        [Attributes.Form_Property("Проектный объем, куб. м")]
        public RamAccess<string> ProjectVolume
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ProjectVolume));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ProjectVolume), value);
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
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
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

        //ProjectVolumeNote property
        public RamAccess<double> ProjectVolumeNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(ProjectVolumeNote));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(ProjectVolumeNote), value);
                OnPropertyChanged(nameof(ProjectVolumeNote));
            }
        }

        
        private bool ProjectVolumeNote_Validation(RamAccess<double?> value)//TODO
        {
            value.ClearErrors();
            return true;
        }
        //ProjectVolumeNote property

        //CodeRAO property
        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(CodeRAO));
                }
                
                {
                    
                }
            }
            set
            {
                {
                    _dataAccess.Set(nameof(CodeRAO), value);
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
            var a = new Regex("^[0-9X]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAO property

        //Volume property
        [Attributes.Form_Property("Разрешенный объем, куб. м")]
        public RamAccess<string> Volume
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Volume));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Volume), value);
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
        //Volume property

        //Mass Property
        [Attributes.Form_Property("Разрешенная масса, т")]
        public RamAccess<string> Mass
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Mass));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Mass), value);
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
        //Mass Property

        //QuantityOZIII property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<int?> QuantityOZIII
        {
            get
            {
                
                {
                    return _dataAccess.Get<int?>(nameof(QuantityOZIII));//OK
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(QuantityOZIII), value);
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
        [Attributes.Form_Property("Суммарная активность, Бк")]
        public RamAccess<string> SummaryActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SummaryActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SummaryActivity), value);
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
        //SummaryActivity property

        //DocumentNumber property
        [Attributes.Form_Property("Номер документа")]
        public RamAccess<string> DocumentNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(DocumentNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(DocumentNumber), value);
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
        public RamAccess<string> DocumentNumberRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(DocumentNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(DocumentNumberRecoded), value);
                }
                OnPropertyChanged(nameof(DocumentNumberRecoded));
            }
        }

        
        private bool DocumentNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //DocumentNumberRecoded property

        //DocumentDate property
        [Attributes.Form_Property("Дата документа")]
        public RamAccess<string> DocumentDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(DocumentDate));//OK
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(DocumentDate), value);
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
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
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
        [Attributes.Form_Property("Срок действия документа")]
        public RamAccess<string> ExpirationDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ExpirationDate));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ExpirationDate), value);
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
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
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
        [Attributes.Form_Property("Наименование документа")]
        public RamAccess<string> DocumentName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(DocumentName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(DocumentName), value);
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
