using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.3: Разрешение на размещение РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form23: Abstracts.Form2
    {
        public Form23(int RowID) : base(RowID)
        {
            FormNum = "23";
            NumberOfFields = 17;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]
        public string StoragePlaceName
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceName)) != null)
                {
                    return (string)_StoragePlaceName.Get();
                }
                else
                {
                    return _StoragePlaceName_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceName_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceName)) != null)
                {
                    _StoragePlaceName.Set(_StoragePlaceName_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        private IDataLoadEngine _StoragePlaceName;//If change this change validation
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
                if (GetErrors(nameof(StoragePlaceNameNote)) != null)
                {
                    return (string)_StoragePlaceNameNote.Get();
                }
                else
                {
                    return _StoragePlaceNameNote_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceNameNote_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceNameNote)) != null)
                {
                    _StoragePlaceNameNote.Set(_StoragePlaceNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        private IDataLoadEngine _StoragePlaceNameNote;//If change this change validation
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
                if (GetErrors(nameof(StoragePlaceCode)) != null)
                {
                    return (string)_StoragePlaceCode.Get();
                }
                else
                {
                    return _StoragePlaceCode_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceCode_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceCode)) != null)
                {
                    _StoragePlaceCode.Set(_StoragePlaceCode_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        private IDataLoadEngine _StoragePlaceCode;//if change this change validation
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
                if (GetErrors(nameof(ProjectVolume)) != null)
                {
                    return (double)_ProjectVolume.Get();
                }
                else
                {
                    return _ProjectVolume_Not_Valid;
                }
            }
            set
            {
                _ProjectVolume_Not_Valid = value;
                if (GetErrors(nameof(ProjectVolume)) != null)
                {
                    _ProjectVolume.Set(_ProjectVolume_Not_Valid);
                }
                OnPropertyChanged(nameof(ProjectVolume));
            }
        }
        private IDataLoadEngine _ProjectVolume;
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
                if (GetErrors(nameof(ProjectVolumeNote)) != null)
                {
                    return (double)_ProjectVolumeNote.Get();
                }
                else
                {
                    return _ProjectVolumeNote_Not_Valid;
                }
            }
            set
            {
                _ProjectVolumeNote_Not_Valid = value;
                if (GetErrors(nameof(ProjectVolumeNote)) != null)
                {
                    _ProjectVolumeNote.Set(_ProjectVolumeNote_Not_Valid);
                }
                OnPropertyChanged(nameof(ProjectVolumeNote));
            }
        }
        private IDataLoadEngine _ProjectVolumeNote;
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
                if (GetErrors(nameof(CodeRAO)) != null)
                {
                    return (string)_CodeRAO.Get();
                }
                else
                {
                    return _CodeRAO_Not_Valid;
                }
            }
            set
            {
                _CodeRAO_Not_Valid = value;
                if (GetErrors(nameof(CodeRAO)) != null)
                {
                    _CodeRAO.Set(_CodeRAO_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }
        private IDataLoadEngine _CodeRAO;
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
                if (GetErrors(nameof(Volume)) != null)
                {
                    return (double)_Volume.Get();
                }
                else
                {
                    return _Volume_Not_Valid;
                }
            }
            set
            {
                _Volume_Not_Valid = value;
                if (GetErrors(nameof(Volume)) != null)
                {
                    _Volume.Set(_Volume_Not_Valid);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }
        private IDataLoadEngine _Volume;
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
                if (GetErrors(nameof(Mass)) != null)
                {
                    return (double)_Mass.Get();
                }
                else
                {
                    return _Mass_Not_Valid;
                }
            }
            set
            {
                _Mass_Not_Valid = value;
                if (GetErrors(nameof(Mass)) != null)
                {
                    _Mass.Set(_Mass_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }
        private IDataLoadEngine _Mass;
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
                if (GetErrors(nameof(QuantityOZIII)) != null)
                {
                    return (int)_QuantityOZIII.Get();
                }
                else
                {
                    return _QuantityOZIII_Not_Valid;
                }
            }
            set
            {
                _QuantityOZIII_Not_Valid = value;
                if (GetErrors(nameof(QuantityOZIII)) != null)
                {
                    _QuantityOZIII.Set(_QuantityOZIII_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        private IDataLoadEngine _QuantityOZIII;  // positive int.
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
                if (GetErrors(nameof(SummaryActivity)) != null)
                {
                    return (string)_SummaryActivity.Get();
                }
                else
                {
                    return _SummaryActivity_Not_Valid;
                }
            }
            set
            {
                _SummaryActivity_Not_Valid = value;
                if (GetErrors(nameof(SummaryActivity)) != null)
                {
                    _SummaryActivity.Set(_SummaryActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }
        private IDataLoadEngine _SummaryActivity;
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
                if (GetErrors(nameof(DocumentNumber)) != null)
                {
                    return (string)_DocumentNumber.Get();
                }
                else
                {
                    return _DocumentNumber_Not_Valid;
                }
            }
            set
            {
                _DocumentNumber_Not_Valid = value;
                if (GetErrors(nameof(DocumentNumber)) != null)
                {
                    _DocumentNumber.Set(_DocumentNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNumber));
            }
        }
        private IDataLoadEngine _DocumentNumber;
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
                if (GetErrors(nameof(DocumentNumberRecoded)) != null)
                {
                    return (string)_DocumentNumberRecoded.Get();
                }
                else
                {
                    return _DocumentNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _DocumentNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(DocumentNumberRecoded)) != null)
                {
                    _DocumentNumberRecoded.Set(_DocumentNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNumberRecoded));
            }
        }
        private IDataLoadEngine _DocumentNumberRecoded;
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
                if (GetErrors(nameof(DocumentDate)) != null)
                {
                    return (DateTime)_DocumentDate.Get();
                }
                else
                {
                    return _DocumentDate_Not_Valid;
                }
            }
            set
            {
                _DocumentDate_Not_Valid = value;
                if (GetErrors(nameof(DocumentDate)) != null)
                {
                    _DocumentDate.Set(_DocumentDate_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentDate));
            }
        }
        private IDataLoadEngine _DocumentDate;//if change this change validation
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
                if (GetErrors(nameof(ExpirationDate)) != null)
                {
                    return (DateTime)_ExpirationDate.Get();
                }
                else
                {
                    return _ExpirationDate_Not_Valid;
                }
            }
            set
            {
                _ExpirationDate_Not_Valid = value;
                if (GetErrors(nameof(ExpirationDate)) != null)
                {
                    _ExpirationDate.Set(_ExpirationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(ExpirationDate));
            }
        }
        private IDataLoadEngine _ExpirationDate;
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
                if (GetErrors(nameof(DocumentName)) != null)
                {
                    return (string)_DocumentName.Get();
                }
                else
                {
                    return _DocumentName_Not_Valid;
                }
            }
            set
            {
                _DocumentName_Not_Valid = value;
                if (GetErrors(nameof(DocumentName)) != null)
                {
                    _DocumentName.Set(_DocumentName_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentName));
            }
        }
        private IDataLoadEngine _DocumentName;
        private string _DocumentName_Not_Valid = "";
        private void DocumentName_Validation(string value)//Ready
        {
            ClearErrors(nameof(DocumentName));
        }
        //DocumentName property
    }
}
