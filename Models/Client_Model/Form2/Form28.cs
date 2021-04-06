using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.8: Отведение сточных вод, содержащих радионуклиды")]
    public class Form28 : Form2
    {
        public Form28() : base()
        {
            FormNum = "28";
            NumberOfFields = 24;
        }

        [Attributes.FormVisual("Форма")]
        public override void Object_Validation()
        {

        }

        //PermissionNumber property
        [Attributes.FormVisual("Номер разрешительного документа")]
        public string PermissionNumber
        {
            get
            {
                if (GetErrors(nameof(PermissionNumber)) != null)
                {
                    return (string)_PermissionNumber.Get();
                }
                else
                {
                    return _PermissionNumber_Not_Valid;
                }
            }
            set
            {
                _PermissionNumber_Not_Valid = value;
                if (GetErrors(nameof(PermissionNumber)) != null)
                {
                    _PermissionNumber.Set(_PermissionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNumber));
            }
        }
        private IDataLoadEngine _PermissionNumber;
        private string _PermissionNumber_Not_Valid = "";
        private void PermissionNumber_Validation()
        {
            ClearErrors(nameof(PermissionNumber));
        }
        //PermissionNumber property

        //PermissionIssueDate property
        [Attributes.FormVisual("Дата выпуска разрешительного документа")]
        public string PermissionIssueDate
        {
            get
            {
                if (GetErrors(nameof(PermissionIssueDate)) != null)
                {
                    return (string)_PermissionIssueDate.Get();
                }
                else
                {
                    return _PermissionIssueDate_Not_Valid;
                }
            }
            set
            {
                _PermissionIssueDate_Not_Valid = value;
                if (GetErrors(nameof(PermissionIssueDate)) != null)
                {
                    _PermissionIssueDate.Set(_PermissionIssueDate_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionIssueDate));
            }
        }
        private IDataLoadEngine _PermissionIssueDate;
        private string _PermissionIssueDate_Not_Valid = "";
        private void PermissionIssueDate_Validation()
        {
            ClearErrors(nameof(PermissionIssueDate));
        }
        //PermissionIssueDate property

        //PermissionDocumentName property
        [Attributes.FormVisual("Наименование разрешительного документа")]
        public string PermissionDocumentName
        {
            get
            {
                if (GetErrors(nameof(PermissionDocumentName)) != null)
                {
                    return (string)_PermissionDocumentName.Get();
                }
                else
                {
                    return _PermissionDocumentName_Not_Valid;
                }
            }
            set
            {
                _PermissionDocumentName_Not_Valid = value;
                if (GetErrors(nameof(PermissionDocumentName)) != null)
                {
                    _PermissionDocumentName.Set(_PermissionDocumentName_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionDocumentName));
            }
        }
        private IDataLoadEngine _PermissionDocumentName;
        private string _PermissionDocumentName_Not_Valid = "";
        private void PermissionDocumentName_Validation()
        {
            ClearErrors(nameof(PermissionDocumentName));
        }
        //PermissionDocumentName property

        //ValidBegin property
        [Attributes.FormVisual("Действует с")]
        public DateTimeOffset ValidBegin
        {
            get
            {
                if (GetErrors(nameof(ValidBegin)) != null)
                {
                    return (DateTime)_ValidBegin.Get();
                }
                else
                {
                    return _ValidBegin_Not_Valid;
                }
            }
            set
            {
                _ValidBegin_Not_Valid = value;
                if (GetErrors(nameof(ValidBegin)) != null)
                {
                    _ValidBegin.Set(_ValidBegin_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidBegin));
            }
        }
        private IDataLoadEngine _ValidBegin;
        private DateTimeOffset _ValidBegin_Not_Valid = DateTimeOffset.MinValue;
        private void ValidBegin_Validation()
        {
            ClearErrors(nameof(ValidBegin));
        }
        //ValidBegin property

        //ValidThru property
        [Attributes.FormVisual("Действует по")]
        public DateTimeOffset ValidThru
        {
            get
            {
                if (GetErrors(nameof(ValidThru)) != null)
                {
                    return (DateTime)_ValidThru.Get();
                }
                else
                {
                    return _ValidThru_Not_Valid;
                }
            }
            set
            {
                _ValidThru_Not_Valid = value;
                if (GetErrors(nameof(ValidThru)) != null)
                {
                    _ValidThru.Set(_ValidThru_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }
        private IDataLoadEngine _ValidThru;
        private DateTimeOffset _ValidThru_Not_Valid = DateTimeOffset.MinValue;
        private void ValidThru_Validation()
        {
            ClearErrors(nameof(ValidThru));
        }
        //ValidThru property

        //PermissionNumber1 property
        [Attributes.FormVisual("Номер разрешительного документа")]
        public string PermissionNumber1
        {
            get
            {
                if (GetErrors(nameof(PermissionNumber1)) != null)
                {
                    return (string)_PermissionNumber1.Get();
                }
                else
                {
                    return _PermissionNumber1_Not_Valid;
                }
            }
            set
            {
                _PermissionNumber1_Not_Valid = value;
                if (GetErrors(nameof(PermissionNumber1)) != null)
                {
                    _PermissionNumber1.Set(_PermissionNumber1_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNumber1));
            }
        }
        private IDataLoadEngine _PermissionNumber1;
        private string _PermissionNumber1_Not_Valid = "";
        private void PermissionNumber1_Validation()
        {
            ClearErrors(nameof(PermissionNumber1));
        }
        //PermissionNumber1 property

        //PermissionIssueDate1 property
        [Attributes.FormVisual("Дата выпуска разрешительного документа")]
        public string PermissionIssueDate1
        {
            get
            {
                if (GetErrors(nameof(PermissionIssueDate1)) != null)
                {
                    return (string)_PermissionIssueDate1.Get();
                }
                else
                {
                    return _PermissionIssueDate1_Not_Valid;
                }
            }
            set
            {
                _PermissionIssueDate1_Not_Valid = value;
                if (GetErrors(nameof(PermissionIssueDate1)) != null)
                {
                    _PermissionIssueDate1.Set(_PermissionIssueDate1_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionIssueDate1));
            }
        }
        private IDataLoadEngine _PermissionIssueDate1;
        private string _PermissionIssueDate1_Not_Valid = "";
        private void PermissionIssueDate1_Validation()
        {
            ClearErrors(nameof(PermissionIssueDate1));
        }
        //PermissionIssueDate1 property

        //PermissionDocumentName1 property
        [Attributes.FormVisual("Наименование разрешительного документа")]
        public string PermissionDocumentName1
        {
            get
            {
                if (GetErrors(nameof(PermissionDocumentName1)) != null)
                {
                    return (string)_PermissionDocumentName1.Get();
                }
                else
                {
                    return _PermissionDocumentName1_Not_Valid;
                }
            }
            set
            {
                _PermissionDocumentName1_Not_Valid = value;
                if (GetErrors(nameof(PermissionDocumentName1)) != null)
                {
                    _PermissionDocumentName1.Set(_PermissionDocumentName1_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionDocumentName1));
            }
        }
        private IDataLoadEngine _PermissionDocumentName1;
        private string _PermissionDocumentName1_Not_Valid = "";
        private void PermissionDocumentName1_Validation()
        {
            ClearErrors(nameof(PermissionDocumentName1));
        }
        //PermissionDocumentName property

        //ValidBegin1 property
        [Attributes.FormVisual("Действует с")]
        public DateTimeOffset ValidBegin1
        {
            get
            {
                if (GetErrors(nameof(ValidBegin1)) != null)
                {
                    return (DateTime)_ValidBegin1.Get();
                }
                else
                {
                    return _ValidBegin1_Not_Valid;
                }
            }
            set
            {
                _ValidBegin1_Not_Valid = value;
                if (GetErrors(nameof(ValidBegin1)) != null)
                {
                    _ValidBegin1.Set(_ValidBegin1_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidBegin1));
            }
        }
        private IDataLoadEngine _ValidBegin1;
        private DateTimeOffset _ValidBegin1_Not_Valid = DateTimeOffset.MinValue;
        private void ValidBegin1_Validation()
        {
            ClearErrors(nameof(ValidBegin1));
        }
        //ValidBegin1 property

        //ValidThru1 property
        [Attributes.FormVisual("Действует по")]
        public DateTimeOffset ValidThru1
        {
            get
            {
                if (GetErrors(nameof(ValidThru1)) != null)
                {
                    return (DateTime)_ValidThru1.Get();
                }
                else
                {
                    return _ValidThru1_Not_Valid;
                }
            }
            set
            {
                _ValidThru1_Not_Valid = value;
                if (GetErrors(nameof(ValidThru1)) != null)
                {
                    _ValidThru1.Set(_ValidThru1_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru1));
            }
        }
        private IDataLoadEngine _ValidThru1;
        private DateTimeOffset _ValidThru1_Not_Valid = DateTimeOffset.MinValue;
        private void ValidThru1_Validation()
        {
            ClearErrors(nameof(ValidThru1));
        }
        //ValidThru1 property

        //PermissionNumber2 property
        [Attributes.FormVisual("Номер разрешительного документа")]
        public string PermissionNumber2
        {
            get
            {
                if (GetErrors(nameof(PermissionNumber2)) != null)
                {
                    return (string)_PermissionNumber2.Get();
                }
                else
                {
                    return _PermissionNumber2_Not_Valid;
                }
            }
            set
            {
                _PermissionNumber2_Not_Valid = value;
                if (GetErrors(nameof(PermissionNumber2)) != null)
                {
                    _PermissionNumber2.Set(_PermissionNumber2_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNumber2));
            }
        }
        private IDataLoadEngine _PermissionNumber2;
        private string _PermissionNumber2_Not_Valid = "";
        private void PermissionNumber2_Validation()
        {
            ClearErrors(nameof(PermissionNumber2));
        }
        //PermissionNumber2 property

        //PermissionIssueDate2 property
        [Attributes.FormVisual("Дата выпуска разрешительного документа")]
        public string PermissionIssueDate2
        {
            get
            {
                if (GetErrors(nameof(PermissionIssueDate2)) != null)
                {
                    return (string)_PermissionIssueDate2.Get();
                }
                else
                {
                    return _PermissionIssueDate2_Not_Valid;
                }
            }
            set
            {
                _PermissionIssueDate2_Not_Valid = value;
                if (GetErrors(nameof(PermissionIssueDate2)) != null)
                {
                    _PermissionIssueDate2.Set(_PermissionIssueDate2_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionIssueDate2));
            }
        }
        private IDataLoadEngine _PermissionIssueDate2;
        private string _PermissionIssueDate2_Not_Valid = "";
        private void PermissionIssueDate2_Validation()
        {
            ClearErrors(nameof(PermissionIssueDate2));
        }
        //PermissionIssueDate property

        //PermissionDocumentName2 property
        [Attributes.FormVisual("Наименование разрешительного документа")]
        public string PermissionDocumentName2
        {
            get
            {
                if (GetErrors(nameof(PermissionDocumentName2)) != null)
                {
                    return (string)_PermissionDocumentName2.Get();
                }
                else
                {
                    return _PermissionDocumentName2_Not_Valid;
                }
            }
            set
            {
                _PermissionDocumentName2_Not_Valid = value;
                if (GetErrors(nameof(PermissionDocumentName)) != null)
                {
                    _PermissionDocumentName2.Set(_PermissionDocumentName2_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionDocumentName2));
            }
        }
        private IDataLoadEngine _PermissionDocumentName2;
        private string _PermissionDocumentName2_Not_Valid = "";
        private void PermissionDocumentName2_Validation()
        {
            ClearErrors(nameof(PermissionDocumentName2));
        }
        //PermissionDocumentName property

        //ValidBegin2 property
        [Attributes.FormVisual("Действует с")]
        public DateTimeOffset ValidBegin2
        {
            get
            {
                if (GetErrors(nameof(ValidBegin2)) != null)
                {
                    return (DateTime)_ValidBegin2.Get();
                }
                else
                {
                    return _ValidBegin2_Not_Valid;
                }
            }
            set
            {
                _ValidBegin2_Not_Valid = value;
                if (GetErrors(nameof(ValidBegin2)) != null)
                {
                    _ValidBegin2.Set(_ValidBegin2_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidBegin2));
            }
        }
        private IDataLoadEngine _ValidBegin2;
        private DateTimeOffset _ValidBegin2_Not_Valid = DateTimeOffset.MinValue;
        private void ValidBegin2_Validation()
        {
            ClearErrors(nameof(ValidBegin2));
        }
        //ValidBegin2 property

        //ValidThru2 property
        [Attributes.FormVisual("Действует по")]
        public DateTimeOffset ValidThru2
        {
            get
            {
                if (GetErrors(nameof(ValidThru2)) != null)
                {
                    return (DateTime)_ValidThru2.Get();
                }
                else
                {
                    return _ValidThru2_Not_Valid;
                }
            }
            set
            {
                _ValidThru2_Not_Valid = value;
                if (GetErrors(nameof(ValidThru2)) != null)
                {
                    _ValidThru2.Set(_ValidThru2_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru2));
            }
        }
        private IDataLoadEngine _ValidThru2;
        private DateTimeOffset _ValidThru2_Not_Valid = DateTimeOffset.MinValue;
        private void ValidThru2_Validation()
        {
            ClearErrors(nameof(ValidThru2));
        }
        //ValidThru2 property
        
        //WasteSourceName property
        [Attributes.FormVisual("Наименование, номер выпуска сточных вод")]
        public string WasteSourceName
        {
            get
            {
                if (GetErrors(nameof(WasteSourceName)) != null)
                {
                    return (string)_WasteSourceName.Get();
                }
                else
                {
                    return _WasteSourceName_Not_Valid;
                }
            }
            set
            {
                _WasteSourceName_Not_Valid = value;
                if (GetErrors(nameof(WasteSourceName)) != null)
                {
                    _WasteSourceName.Set(_WasteSourceName_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }
        private IDataLoadEngine _WasteSourceName;
        private string _WasteSourceName_Not_Valid = "";
        private void WasteSourceName_Validation()
        {
            ClearErrors(nameof(WasteSourceName));
        }
        //WasteSourceName property
        
        //WasteRecieverName property
        [Attributes.FormVisual("Наименование приемника отведенных вод")]
        public string WasteRecieverName
        {
            get
            {
                if (GetErrors(nameof(WasteRecieverName)) != null)
                {
                    return (string)_WasteRecieverName.Get();
                }
                else
                {
                    return _WasteRecieverName_Not_Valid;
                }
            }
            set
            {
                _WasteRecieverName_Not_Valid = value;
                if (GetErrors(nameof(WasteRecieverName)) != null)
                {
                    _WasteRecieverName.Set(_WasteRecieverName_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteRecieverName));
            }
        }
        private IDataLoadEngine _WasteRecieverName;
        private string _WasteRecieverName_Not_Valid = "";
        private void WasteRecieverName_Validation()
        {
            ClearErrors(nameof(WasteRecieverName));
        }
        //WasteRecieverName property
        
        //RecieverTypeCode property
        [Attributes.FormVisual("Код типа приемника отведенных вод")]
        public string RecieverTypeCode
        {
            get
            {
                if (GetErrors(nameof(RecieverTypeCode)) != null)
                {
                    return (string)_RecieverTypeCode.Get();
                }
                else
                {
                    return _RecieverTypeCode_Not_Valid;
                }
            }
            set
            {
                _RecieverTypeCode_Not_Valid = value;
                if (GetErrors(nameof(RecieverTypeCode)) != null)
                {
                    _RecieverTypeCode.Set(_RecieverTypeCode_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverTypeCode));
            }
        }
        private IDataLoadEngine _RecieverTypeCode;
        private string _RecieverTypeCode_Not_Valid = "";
        private void RecieverTypeCode_Validation()
        {
            ClearErrors(nameof(RecieverTypeCode));
        }
        //RecieverTypeCode property
        
        //PoolDistrictName property
        [Attributes.FormVisual("Наименование бассейнового округа приемника отведенных вод")]
        public string PoolDistrictName
        {
            get
            {
                if (GetErrors(nameof(PoolDistrictName)) != null)
                {
                    return (string)_PoolDistrictName.Get();
                }
                else
                {
                    return _PoolDistrictName_Not_Valid;
                }
            }
            set
            {
                _PoolDistrictName_Not_Valid = value;
                if (GetErrors(nameof(PoolDistrictName)) != null)
                {
                    _PoolDistrictName.Set(_PoolDistrictName_Not_Valid);
                }
                OnPropertyChanged(nameof(PoolDistrictName));
            }
        }
        private IDataLoadEngine _PoolDistrictName;
        private string _PoolDistrictName_Not_Valid = "";
        private void PoolDistrictName_Validation()
        {
            ClearErrors(nameof(PoolDistrictName));
        }
        //PoolDistrictName property

        //AllowedWasteRemovalVolume property
        [Attributes.FormVisual("Допустимый объем водоотведения за год, тыс. куб. м")]
        public double AllowedWasteRemovalVolume
        {
            get
            {
                if (GetErrors(nameof(AllowedWasteRemovalVolume)) != null)
                {
                    return (double)_AllowedWasteRemovalVolume.Get();
                }
                else
                {
                    return _AllowedWasteRemovalVolume_Not_Valid;
                }
            }
            set
            {
                _AllowedWasteRemovalVolume_Not_Valid = value;
                if (GetErrors(nameof(AllowedWasteRemovalVolume)) != null)
                {
                    _AllowedWasteRemovalVolume.Set(_AllowedWasteRemovalVolume_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedWasteRemovalVolume));
            }
        }
        private IDataLoadEngine _AllowedWasteRemovalVolume;
        private double _AllowedWasteRemovalVolume_Not_Valid = -1;
        private void AllowedWasteRemovalVolume_Validation()
        {
            ClearErrors(nameof(AllowedWasteRemovalVolume));
        }
        //AllowedWasteRemovalVolume property

        //RemovedWasteVolume property
        [Attributes.FormVisual("Отведено за отчетный период, тыс. куб. м")]
        public double RemovedWasteVolume
        {
            get
            {
                if (GetErrors(nameof(RemovedWasteVolume)) != null)
                {
                    return (double)_RemovedWasteVolume.Get();
                }
                else
                {
                    return _RemovedWasteVolume_Not_Valid;
                }
            }
            set
            {
                _RemovedWasteVolume_Not_Valid = value;
                if (GetErrors(nameof(RemovedWasteVolume)) != null)
                {
                    _RemovedWasteVolume.Set(_RemovedWasteVolume_Not_Valid);
                }
                OnPropertyChanged(nameof(RemovedWasteVolume));
            }
        }
        private IDataLoadEngine _RemovedWasteVolume;
        private double _RemovedWasteVolume_Not_Valid = -1;
        private void RemovedWasteVolume_Validation()
        {
            ClearErrors(nameof(RemovedWasteVolume));
        }
        //RemovedWasteVolume property

        private double _removedWasteVolumeNote = -1;
        public double RemovedWasteVolumeNote
        {
            get { return _removedWasteVolumeNote; }
            set
            {
                _removedWasteVolumeNote = value;
                OnPropertyChanged("RemovedWasteVolumeNote");
            }
        }
    }
}
