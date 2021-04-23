using System;
using System.Globalization;
using DBRealization;
using Collections.Rows_Collection;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.3: Разрешение на размещение РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form23: Abstracts.Form2
    {
        public Form23(IDataAccess Access) : base(Access)
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
                    return (string)_dataAccess.Get(nameof(StoragePlaceName))[0][0];
                }
                else
                {
                    return _StoragePlaceName_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceName_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceName), _StoragePlaceName_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
        private string _StoragePlaceName_Not_Valid = "";
        private void StoragePlaceName_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceName));
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public string StoragePlaceNameNote
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceNameNote))[0][0];
                }
                else
                {
                    return _StoragePlaceNameNote_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceNameNote_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), _StoragePlaceNameNote_Not_Valid);
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
                    return (string)_dataAccess.Get(nameof(StoragePlaceCode))[0][0];
                }
                else
                {
                    return _StoragePlaceCode_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceCode_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceCode)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), _StoragePlaceCode_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation
        private string _StoragePlaceCode_Not_Valid = "";
        private void StoragePlaceCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(StoragePlaceCode));
            if (!(value == "-"))
                if (value.Length != 8)
                    AddError(nameof(StoragePlaceCode), "Недопустимое значение");
                else
                    for (int i = 0; i < 8; i++)
                    {
                        if (!((value[i] >= '0') && (value[i] <= '9')))
                        {
                            AddError(nameof(StoragePlaceCode), "Недопустимое значение");
                            return;
                        }
                    }
        }
        //StoragePlaceCode property

        //ProjectVolume property
        [Attributes.Form_Property("Проектный объем, куб. м")]
        public double ProjectVolume
        {
            get
            {
                if (GetErrors(nameof(ProjectVolume)) == null)
                {
                    return (double)_dataAccess.Get(nameof(ProjectVolume))[0][0];
                }
                else
                {
                    return _ProjectVolume_Not_Valid;
                }
            }
            set
            {
                _ProjectVolume_Not_Valid = value;
                if (GetErrors(nameof(ProjectVolume)) == null)
                {
                    _dataAccess.Set(nameof(ProjectVolume), _ProjectVolume_Not_Valid);
                }
                OnPropertyChanged(nameof(ProjectVolume));
            }
        }
        
        private double _ProjectVolume_Not_Valid = -1;
        private void ProjectVolume_Validation(double value)//TODO
        {
            ClearErrors(nameof(ProjectVolume));
        }
        //ProjectVolume property

        //ProjectVolumeNote property
        public double ProjectVolumeNote
        {
            get
            {
                if (GetErrors(nameof(ProjectVolumeNote)) == null)
                {
                    return (double)_dataAccess.Get(nameof(ProjectVolumeNote))[0][0];
                }
                else
                {
                    return _ProjectVolumeNote_Not_Valid;
                }
            }
            set
            {
                _ProjectVolumeNote_Not_Valid = value;
                if (GetErrors(nameof(ProjectVolumeNote)) == null)
                {
                    _dataAccess.Set(nameof(ProjectVolumeNote), _ProjectVolumeNote_Not_Valid);
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
                    return (string)_dataAccess.Get(nameof(CodeRAO))[0][0];
                }
                else
                {
                    return _CodeRAO_Not_Valid;
                }
            }
            set
            {
                _CodeRAO_Not_Valid = value;
                if (GetErrors(nameof(CodeRAO)) == null)
                {
                    _dataAccess.Set(nameof(CodeRAO), _CodeRAO_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }
        
        private string _CodeRAO_Not_Valid = "";
        private void CodeRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAO));
        }
        //CodeRAO property

        //Volume property
        [Attributes.Form_Property("Разрешенный объем, куб. м")]
        public double Volume
        {
            get
            {
                if (GetErrors(nameof(Volume)) == null)
                {
                    return (double)_dataAccess.Get(nameof(Volume))[0][0];
                }
                else
                {
                    return _Volume_Not_Valid;
                }
            }
            set
            {
                _Volume_Not_Valid = value;
                if (GetErrors(nameof(Volume)) == null)
                {
                    _dataAccess.Set(nameof(Volume), _Volume_Not_Valid);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }
        
        private double _Volume_Not_Valid = -1;
        private void Volume_Validation(double value)//TODO
        {
            ClearErrors(nameof(Volume));
        }
        //Volume property

        //Mass Property
        [Attributes.Form_Property("Разрешенная масса, т")]
        public double Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) == null)
                {
                    return (double)_dataAccess.Get(nameof(Mass))[0][0];
                }
                else
                {
                    return _Mass_Not_Valid;
                }
            }
            set
            {
                _Mass_Not_Valid = value;
                if (GetErrors(nameof(Mass)) == null)
                {
                    _dataAccess.Set(nameof(Mass), _Mass_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }
        
        private double _Mass_Not_Valid = -1;
        private void Mass_Validation()//TODO
        {
            ClearErrors(nameof(Mass));
        }
        //Mass Property

        //QuantityOZIII property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public int QuantityOZIII
        {
            get
            {
                if (GetErrors(nameof(QuantityOZIII)) == null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityOZIII))[0][0];
                }
                else
                {
                    return _QuantityOZIII_Not_Valid;
                }
            }
            set
            {
                _QuantityOZIII_Not_Valid = value;
                if (GetErrors(nameof(QuantityOZIII)) == null)
                {
                    _dataAccess.Set(nameof(QuantityOZIII), _QuantityOZIII_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
          // positive int.
        private int _QuantityOZIII_Not_Valid = -1;
        private void QuantityOZIII_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOZIII));
            if (value <= 0)
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
                    return (string)_dataAccess.Get(nameof(SummaryActivity))[0][0];
                }
                else
                {
                    return _SummaryActivity_Not_Valid;
                }
            }
            set
            {
                _SummaryActivity_Not_Valid = value;
                if (GetErrors(nameof(SummaryActivity)) == null)
                {
                    _dataAccess.Set(nameof(SummaryActivity), _SummaryActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }
        
        private string _SummaryActivity_Not_Valid = "";
        private void SummaryActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(SummaryActivity));
            string tmp = value;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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
                    return (string)_dataAccess.Get(nameof(DocumentNumber))[0][0];
                }
                else
                {
                    return _DocumentNumber_Not_Valid;
                }
            }
            set
            {
                _DocumentNumber_Not_Valid = value;
                if (GetErrors(nameof(DocumentNumber)) == null)
                {
                    _dataAccess.Set(nameof(DocumentNumber), _DocumentNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }
        
        private string _DocumentNumber_Not_Valid = "";
        private void DocumentNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentNumber));
        }
        //DocumentNumber property

        //DocumentNumberRecoded property
        public string DocumentNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(DocumentNumberRecoded)) == null)
                {
                    return (string)_dataAccess.Get(nameof(DocumentNumberRecoded))[0][0];
                }
                else
                {
                    return _DocumentNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _DocumentNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(DocumentNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(DocumentNumberRecoded), _DocumentNumberRecoded_Not_Valid);
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
        public DateTimeOffset DocumentDate
        {
            get
            {
                if (GetErrors(nameof(DocumentDate)) == null)
                {
                    return (DateTime)_dataAccess.Get(nameof(DocumentDate))[0][0];
                }
                else
                {
                    return _DocumentDate_Not_Valid;
                }
            }
            set
            {
                _DocumentDate_Not_Valid = value;
                if (GetErrors(nameof(DocumentDate)) == null)
                {
                    _dataAccess.Set(nameof(DocumentDate), _DocumentDate_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        //if change this change validation
        private DateTimeOffset _DocumentDate_Not_Valid = DateTimeOffset.MinValue;
        private void DocumentDate_Validation(DateTimeOffset value)//Ready
        {
            ClearErrors(nameof(DocumentDate));
        }
        //DocumentDate property

        //ExpirationDate property
        [Attributes.Form_Property("Срок действия документа")]
        public DateTimeOffset ExpirationDate
        {
            get
            {
                if (GetErrors(nameof(ExpirationDate)) == null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ExpirationDate))[0][0];
                }
                else
                {
                    return _ExpirationDate_Not_Valid;
                }
            }
            set
            {
                _ExpirationDate_Not_Valid = value;
                if (GetErrors(nameof(ExpirationDate)) == null)
                {
                    _dataAccess.Set(nameof(ExpirationDate), _ExpirationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }
        
        private DateTimeOffset _ExpirationDate_Not_Valid = DateTimeOffset.MinValue;
        private void ExpirationDate_Validation(DateTimeOffset value)//TODO
        {
            ClearErrors(nameof(ExpirationDate));
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
                    return (string)_dataAccess.Get(nameof(DocumentName))[0][0];
                }
                else
                {
                    return _DocumentName_Not_Valid;
                }
            }
            set
            {
                _DocumentName_Not_Valid = value;
                if (GetErrors(nameof(DocumentName)) == null)
                {
                    _dataAccess.Set(nameof(DocumentName), _DocumentName_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentName));
            }
        }
        
        private string _DocumentName_Not_Valid = "";
        private void DocumentName_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentName));
        }
        //DocumentName property
    }
}
