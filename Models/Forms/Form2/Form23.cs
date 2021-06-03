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
            FormNum = "23";
            NumberOfFields = 17;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]
        public string StoragePlaceName
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceName));
                }
                else
                {
                    return _StoragePlaceName_Not_Valid;
                }
            }
            set
            {
                StoragePlaceName_Validation(value);
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceName), value);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
        private string _StoragePlaceName_Not_Valid = "";
        private void StoragePlaceName_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceName));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(StoragePlaceName), "Поле не заполнено");
                return;
            }
            var spr = new List<string>();
            if (!spr.Contains(value))
            {
                AddError(nameof(StoragePlaceName), "Недопустиое значение");
                return;
            }
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public string StoragePlaceNameNote
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceNameNote));
                }
                else
                {
                    return _StoragePlaceNameNote_Not_Valid;
                }
            }
            set
            {
                StoragePlaceNameNote_Validation(value);
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), value);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation
        private string _StoragePlaceNameNote_Not_Valid = "";
        private void StoragePlaceNameNote_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceNameNote));
        }
        //StoragePlaceNameNote property

        //StoragePlaceCode property
        [Attributes.Form_Property("Код ПХ")]
        public string StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceCode)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceCode));
                }
                else
                {
                    return _StoragePlaceCode_Not_Valid;
                }
            }
            set
            {
                StoragePlaceCode_Validation(value);
                if (GetErrors(nameof(StoragePlaceCode)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), value);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation
        private string _StoragePlaceCode_Not_Valid = "";
        private void StoragePlaceCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(StoragePlaceCode));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(StoragePlaceName), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            var spr = new List<string>();
            if (!spr.Contains(value))
            {
                AddError(nameof(StoragePlaceName), "Недопустиое значение");
                return;
            }
        }
        //StoragePlaceCode property

        //ProjectVolume property
        [Attributes.Form_Property("Проектный объем, куб. м")]
        public string ProjectVolume
        {
            get
            {
                if (GetErrors(nameof(ProjectVolume)) == null)
                {
                    return (string)_dataAccess.Get(nameof(ProjectVolume));
                }
                else
                {
                    return _ProjectVolume_Not_Valid;
                }
            }
            set
            {
                ProjectVolume_Validation(value);
                if (GetErrors(nameof(ProjectVolume)) == null)
                {
                    _dataAccess.Set(nameof(ProjectVolume), value);
                }
                OnPropertyChanged(nameof(ProjectVolume));
            }
        }

        private string _ProjectVolume_Not_Valid = "";
        private void ProjectVolume_Validation(string value)//TODO
        {
            ClearErrors(nameof(ProjectVolume));
            if (value.Equals("прим."))
            {

            }
            if (!((value.Contains('e') || value.Contains('E'))))
            {
                AddError(nameof(ProjectVolume), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(ProjectVolume), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(ProjectVolume), "Недопустимое значение");
            }
        }
        //ProjectVolume property

        //ProjectVolumeNote property
        public double ProjectVolumeNote
        {
            get
            {
                if (GetErrors(nameof(ProjectVolumeNote)) == null)
                {
                    return (double)_dataAccess.Get(nameof(ProjectVolumeNote));
                }
                else
                {
                    return _ProjectVolumeNote_Not_Valid;
                }
            }
            set
            {
                ProjectVolumeNote_Validation(value);
                if (GetErrors(nameof(ProjectVolumeNote)) == null)
                {
                    _dataAccess.Set(nameof(ProjectVolumeNote), value);
                }
                OnPropertyChanged(nameof(ProjectVolumeNote));
            }
        }

        private double _ProjectVolumeNote_Not_Valid = -1;
        private void ProjectVolumeNote_Validation(double value)//TODO
        {
            ClearErrors(nameof(ProjectVolumeNote));
        }
        //ProjectVolumeNote property

        //CodeRAO property
        [Attributes.Form_Property("Код РАО")]
        public string CodeRAO
        {
            get
            {
                if (GetErrors(nameof(CodeRAO)) == null)
                {
                    return (string)_dataAccess.Get(nameof(CodeRAO));
                }
                else
                {
                    return _CodeRAO_Not_Valid;
                }
            }
            set
            {
                CodeRAO_Validation(value);
                if (GetErrors(nameof(CodeRAO)) == null)
                {
                    _dataAccess.Set(nameof(CodeRAO), value);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }

        private string _CodeRAO_Not_Valid = "";
        private void CodeRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAO));
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(CodeRAO), "Недопустимое значение");
                return;
            }
        }
        //CodeRAO property

        //Volume property
        [Attributes.Form_Property("Разрешенный объем, куб. м")]
        public string Volume
        {
            get
            {
                if (GetErrors(nameof(Volume)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Volume));
                }
                else
                {
                    return _Volume_Not_Valid;
                }
            }
            set
            {
                Volume_Validation(value);
                if (GetErrors(nameof(Volume)) == null)
                {
                    _dataAccess.Set(nameof(Volume), value);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }

        private string _Volume_Not_Valid = "";
        private void Volume_Validation(string value)//TODO
        {
            ClearErrors(nameof(Volume));
            if (string.IsNullOrEmpty(value)) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(Volume), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Volume), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(Volume), "Недопустимое значение");
            }
        }
        //Volume property

        //Mass Property
        [Attributes.Form_Property("Разрешенная масса, т")]
        public string Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Mass));
                }
                else
                {
                    return _Mass_Not_Valid;
                }
            }
            set
            {
                Mass_Validation(value);
                if (GetErrors(nameof(Mass)) == null)
                {
                    _dataAccess.Set(nameof(Mass), value);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }

        private string _Mass_Not_Valid = "";
        private void Mass_Validation(string value)//TODO
        {
            ClearErrors(nameof(Mass));
            if (string.IsNullOrEmpty(value)) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(Mass), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Mass), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(Mass), "Недопустимое значение");
            }
        }
        //Mass Property

        //QuantityOZIII property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public int? QuantityOZIII
        {
            get
            {
                if (GetErrors(nameof(QuantityOZIII)) == null)
                {
                    return (int?)_dataAccess.Get(nameof(QuantityOZIII));//OK
                }
                else
                {
                    return _QuantityOZIII_Not_Valid;
                }
            }
            set
            {
                QuantityOZIII_Validation(value);

                if (GetErrors(nameof(QuantityOZIII)) == null)
                {
                    _dataAccess.Set(nameof(QuantityOZIII), value);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.
        private int? _QuantityOZIII_Not_Valid = null;
        private void QuantityOZIII_Validation(int? value)//Ready
        {
            ClearErrors(nameof(QuantityOZIII));
            if (value == null) return;
            if ((int)value <= 0)
                AddError(nameof(QuantityOZIII), "Недопустимое значение");
        }
        //QuantityOZIII property

        //SummaryActivity property
        [Attributes.Form_Property("Суммарная активность, Бк")]
        public string SummaryActivity
        {
            get
            {
                if (GetErrors(nameof(SummaryActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(SummaryActivity));
                }
                else
                {
                    return _SummaryActivity_Not_Valid;
                }
            }
            set
            {
                SummaryActivity_Validation(value);
                if (GetErrors(nameof(SummaryActivity)) == null)
                {
                    _dataAccess.Set(nameof(SummaryActivity), value);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }

        private string _SummaryActivity_Not_Valid = "";
        private void SummaryActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(SummaryActivity));
            if (string.IsNullOrEmpty(value)) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(SummaryActivity), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(SummaryActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(SummaryActivity), "Недопустимое значение");
            }
        }
        //SummaryActivity property

        //DocumentNumber property
        [Attributes.Form_Property("Номер документа")]
        public string DocumentNumber
        {
            get
            {
                if (GetErrors(nameof(DocumentNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _DocumentNumber_Not_Valid;
                }
            }
            set
            {
                DocumentNumber_Validation(value);
                if (GetErrors(nameof(DocumentNumber)) == null)
                {
                    _dataAccess.Set(nameof(DocumentNumber), value);
                }
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }

        private string _DocumentNumber_Not_Valid = "";
        private void DocumentNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentNumber));
            if ((value == null) || value.Equals(_DocumentNumber_Not_Valid))//ok
            {
                AddError(nameof(DocumentNumber), "Поле не заполнено");
                return;
            }
        }
        //DocumentNumber property

        //DocumentNumberRecoded property
        public string DocumentNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(DocumentNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _DocumentNumberRecoded_Not_Valid;
                }
            }
            set
            {
                DocumentNumberRecoded_Validation(value);
                if (GetErrors(nameof(DocumentNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(DocumentNumberRecoded), value);
                }
                OnPropertyChanged(nameof(DocumentNumberRecoded));
            }
        }

        private string _DocumentNumberRecoded_Not_Valid = "";
        private void DocumentNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentNumberRecoded));
        }
        //DocumentNumberRecoded property

        //DocumentDate property
        [Attributes.Form_Property("Дата документа")]
        public string DocumentDate
        {
            get
            {
                if (GetErrors(nameof(DocumentDate)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentDate));//OK
                    if (tmp == null)
                        return _DocumentDate_Not_Valid;
                    return ((DateTimeOffset)tmp).Date.ToString("dd.MM.yyyy");// дает дату в формате дд.мм.гггг
                }
                else
                {
                    return _DocumentDate_Not_Valid;
                }
            }
            set
            {
                DocumentDate_Validation(value);

                if (GetErrors(nameof(DocumentDate)) == null)
                {
                    _dataAccess.Set(nameof(DocumentDate), DateTimeOffset.Parse(value));
                }
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        //if change this change validation
        protected string _DocumentDate_Not_Valid = "";
        private void DocumentDate_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentDate));
            if ((value == null) || value.Equals(""))
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
        }
        //DocumentDate property

        //ExpirationDate property
        [Attributes.Form_Property("Срок действия документа")]
        public string ExpirationDate
        {
            get
            {
                if (GetErrors(nameof(ExpirationDate)) == null)
                {
                    return (string)_dataAccess.Get(nameof(ExpirationDate));
                }
                else
                {
                    return _ExpirationDate_Not_Valid;
                }
            }
            set
            {
                ExpirationDate_Validation(value);
                if (GetErrors(nameof(ExpirationDate)) == null)
                {
                    _dataAccess.Set(nameof(ExpirationDate), value);
                }
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }

        private string _ExpirationDate_Not_Valid = "";
        private void ExpirationDate_Validation(string value)//TODO
        {
            ClearErrors(nameof(ExpirationDate));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(ExpirationDate), "Поле не заполнено");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(ExpirationDate), "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value); }
            catch (Exception)
            {
                AddError(nameof(ExpirationDate), "Недопустимое значение");
                return;
            }
        }
        //ExpirationDate property

        //DocumentName property
        [Attributes.Form_Property("Наименование документа")]
        public string DocumentName
        {
            get
            {
                if (GetErrors(nameof(DocumentName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(DocumentName));
                }
                else
                {
                    return _DocumentName_Not_Valid;
                }
            }
            set
            {
                DocumentName_Validation(value);
                if (GetErrors(nameof(DocumentName)) == null)
                {
                    _dataAccess.Set(nameof(DocumentName), value);
                }
                OnPropertyChanged(nameof(DocumentName));
            }
        }

        private string _DocumentName_Not_Valid = "";
        private void DocumentName_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentName));
            if (string.IsNullOrEmpty(value))
            {
                AddError(nameof(DocumentName), "Поле не заполнено");
                return;
            }
        }
        //DocumentName property
    }
}
