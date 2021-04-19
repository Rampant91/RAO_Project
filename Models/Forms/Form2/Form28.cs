using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DBRealization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.8: Отведение сточных вод, содержащих радионуклиды")]
    public class Form28 : Abstracts.Form2
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            string doubleNotNullDeclaration = " float(53) not null, ";
            return
                Abstracts.Form2.SQLCommandParamsBase() +
            nameof(WasteSourceName) + strNotNullDeclaration +
            nameof(PermissionNumber1) + strNotNullDeclaration +
            nameof(PermissionIssueDate1) + dateNotNullDeclaration +
            nameof(PermissionDocumentName1) + strNotNullDeclaration +
            nameof(ValidBegin1) + dateNotNullDeclaration +
            nameof(ValidThru1) + dateNotNullDeclaration +
            nameof(PermissionNumber2) + strNotNullDeclaration +
            nameof(PermissionIssueDate2) + dateNotNullDeclaration +
            nameof(ValidBegin2) + dateNotNullDeclaration +
            nameof(PermissionDocumentName2) + strNotNullDeclaration +
            nameof(ValidThru2) + dateNotNullDeclaration +
            nameof(WasteRecieverName) + strNotNullDeclaration +
            nameof(RecieverTypeCode) + strNotNullDeclaration +
            nameof(PoolDistrictName) + strNotNullDeclaration +
            nameof(AllowedWasteRemovalVolume) + doubleNotNullDeclaration +
            nameof(RemovedWasteVolume) + doubleNotNullDeclaration +
            nameof(RemovedWasteVolumeNote) + doubleNotNullDeclaration +
            nameof(PermissionNumber) + strNotNullDeclaration +
            nameof(PermissionIssueDate) + strNotNullDeclaration +
            nameof(ValidBegin) + dateNotNullDeclaration +
            nameof(ValidThru) + dateNotNullDeclaration +
            nameof(PermissionDocumentName) + " varchar(255) not null";
        }
        public Form28(IDataAccess Access) : base(Access)
        {
            FormNum = "28";
            NumberOfFields = 24;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PermissionNumber property
        [Attributes.Form_Property("Номер разрешительного документа")]
        public string PermissionNumber
        {
            get
            {
                if (GetErrors(nameof(PermissionNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionNumber));
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
                    _dataAccess.Set(nameof(PermissionNumber), _PermissionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNumber));
            }
        }
        
        private string _PermissionNumber_Not_Valid = "";
        private void PermissionNumber_Validation()
        {
            ClearErrors(nameof(PermissionNumber));
        }
        //PermissionNumber property

        //PermissionIssueDate property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public string PermissionIssueDate
        {
            get
            {
                if (GetErrors(nameof(PermissionIssueDate)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionIssueDate));
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
                    _dataAccess.Set(nameof(PermissionIssueDate), _PermissionIssueDate_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionIssueDate));
            }
        }
        
        private string _PermissionIssueDate_Not_Valid = "";
        private void PermissionIssueDate_Validation()
        {
            ClearErrors(nameof(PermissionIssueDate));
        }
        //PermissionIssueDate property

        //PermissionDocumentName property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public string PermissionDocumentName
        {
            get
            {
                if (GetErrors(nameof(PermissionDocumentName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionDocumentName));
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
                    _dataAccess.Set(nameof(PermissionDocumentName), _PermissionDocumentName_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionDocumentName));
            }
        }
        
        private string _PermissionDocumentName_Not_Valid = "";
        private void PermissionDocumentName_Validation()
        {
            ClearErrors(nameof(PermissionDocumentName));
        }
        //PermissionDocumentName property

        //ValidBegin property
        [Attributes.Form_Property("Действует с")]
        public DateTimeOffset ValidBegin
        {
            get
            {
                if (GetErrors(nameof(ValidBegin)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidBegin));
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
                    _dataAccess.Set(nameof(ValidBegin), _ValidBegin_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidBegin));
            }
        }
        
        private DateTimeOffset _ValidBegin_Not_Valid = DateTimeOffset.MinValue;
        private void ValidBegin_Validation()
        {
            ClearErrors(nameof(ValidBegin));
        }
        //ValidBegin property

        //ValidThru property
        [Attributes.Form_Property("Действует по")]
        public DateTimeOffset ValidThru
        {
            get
            {
                if (GetErrors(nameof(ValidThru)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidThru));
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
                    _dataAccess.Set(nameof(ValidThru), _ValidThru_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }
        
        private DateTimeOffset _ValidThru_Not_Valid = DateTimeOffset.MinValue;
        private void ValidThru_Validation()
        {
            ClearErrors(nameof(ValidThru));
        }
        //ValidThru property

        //PermissionNumber1 property
        [Attributes.Form_Property("Номер разрешительного документа")]
        public string PermissionNumber1
        {
            get
            {
                if (GetErrors(nameof(PermissionNumber1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionNumber1));
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
                    _dataAccess.Set(nameof(PermissionNumber1), _PermissionNumber1_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNumber1));
            }
        }
        
        private string _PermissionNumber1_Not_Valid = "";
        private void PermissionNumber1_Validation()
        {
            ClearErrors(nameof(PermissionNumber1));
        }
        //PermissionNumber1 property

        //PermissionIssueDate1 property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public string PermissionIssueDate1
        {
            get
            {
                if (GetErrors(nameof(PermissionIssueDate1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionIssueDate1));
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
                    _dataAccess.Set(nameof(PermissionIssueDate1), _PermissionIssueDate1_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionIssueDate1));
            }
        }
        
        private string _PermissionIssueDate1_Not_Valid = "";
        private void PermissionIssueDate1_Validation()
        {
            ClearErrors(nameof(PermissionIssueDate1));
        }
        //PermissionIssueDate1 property

        //PermissionDocumentName1 property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public string PermissionDocumentName1
        {
            get
            {
                if (GetErrors(nameof(PermissionDocumentName1)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionDocumentName1));
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
                    _dataAccess.Set(nameof(PermissionDocumentName1), _PermissionDocumentName1_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionDocumentName1));
            }
        }
        
        private string _PermissionDocumentName1_Not_Valid = "";
        private void PermissionDocumentName1_Validation()
        {
            ClearErrors(nameof(PermissionDocumentName1));
        }
        //PermissionDocumentName1 property

        //ValidBegin1 property
        [Attributes.Form_Property("Действует с")]
        public DateTimeOffset ValidBegin1
        {
            get
            {
                if (GetErrors(nameof(ValidBegin1)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidBegin1));
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
                    _dataAccess.Set(nameof(ValidBegin1), _ValidBegin1_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidBegin1));
            }
        }
        
        private DateTimeOffset _ValidBegin1_Not_Valid = DateTimeOffset.MinValue;
        private void ValidBegin1_Validation()
        {
            ClearErrors(nameof(ValidBegin1));
        }
        //ValidBegin1 property

        //ValidThru1 property
        [Attributes.Form_Property("Действует по")]
        public DateTimeOffset ValidThru1
        {
            get
            {
                if (GetErrors(nameof(ValidThru1)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidThru1));
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
                    _dataAccess.Set(nameof(ValidThru1), _ValidThru1_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru1));
            }
        }
        
        private DateTimeOffset _ValidThru1_Not_Valid = DateTimeOffset.MinValue;
        private void ValidThru1_Validation()
        {
            ClearErrors(nameof(ValidThru1));
        }
        //ValidThru1 property

        //PermissionNumber2 property
        [Attributes.Form_Property("Номер разрешительного документа")]
        public string PermissionNumber2
        {
            get
            {
                if (GetErrors(nameof(PermissionNumber2)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionNumber2));
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
                    _dataAccess.Set(nameof(PermissionNumber2), _PermissionNumber2_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNumber2));
            }
        }
        
        private string _PermissionNumber2_Not_Valid = "";
        private void PermissionNumber2_Validation()
        {
            ClearErrors(nameof(PermissionNumber2));
        }
        //PermissionNumber2 property

        //PermissionIssueDate2 property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public string PermissionIssueDate2
        {
            get
            {
                if (GetErrors(nameof(PermissionIssueDate2)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionIssueDate2));
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
                    _dataAccess.Set(nameof(PermissionIssueDate2), _PermissionIssueDate2_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionIssueDate2));
            }
        }
        
        private string _PermissionIssueDate2_Not_Valid = "";
        private void PermissionIssueDate2_Validation()
        {
            ClearErrors(nameof(PermissionIssueDate2));
        }
        //PermissionIssueDate property

        //PermissionDocumentName2 property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public string PermissionDocumentName2
        {
            get
            {
                if (GetErrors(nameof(PermissionDocumentName2)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionDocumentName2));
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
                    _dataAccess.Set(nameof(PermissionDocumentName2), _PermissionDocumentName2_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionDocumentName2));
            }
        }
        
        private string _PermissionDocumentName2_Not_Valid = "";
        private void PermissionDocumentName2_Validation()
        {
            ClearErrors(nameof(PermissionDocumentName2));
        }
        //PermissionDocumentName2 property

        //ValidBegin2 property
        [Attributes.Form_Property("Действует с")]
        public DateTimeOffset ValidBegin2
        {
            get
            {
                if (GetErrors(nameof(ValidBegin2)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidBegin2));
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
                    _dataAccess.Set(nameof(ValidBegin2), _ValidBegin2_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidBegin2));
            }
        }
        
        private DateTimeOffset _ValidBegin2_Not_Valid = DateTimeOffset.MinValue;
        private void ValidBegin2_Validation()
        {
            ClearErrors(nameof(ValidBegin2));
        }
        //ValidBegin2 property

        //ValidThru2 property
        [Attributes.Form_Property("Действует по")]
        public DateTimeOffset ValidThru2
        {
            get
            {
                if (GetErrors(nameof(ValidThru2)) != null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidThru2));
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
                    _dataAccess.Set(nameof(ValidThru2), _ValidThru2_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThru2));
            }
        }
        
        private DateTimeOffset _ValidThru2_Not_Valid = DateTimeOffset.MinValue;
        private void ValidThru2_Validation()
        {
            ClearErrors(nameof(ValidThru2));
        }
        //ValidThru2 property
        
        //WasteSourceName property
        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public string WasteSourceName
        {
            get
            {
                if (GetErrors(nameof(WasteSourceName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(WasteSourceName));
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
                    _dataAccess.Set(nameof(WasteSourceName), _WasteSourceName_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }
        
        private string _WasteSourceName_Not_Valid = "";
        private void WasteSourceName_Validation()
        {
            ClearErrors(nameof(WasteSourceName));
        }
        //WasteSourceName property
        
        //WasteRecieverName property
        [Attributes.Form_Property("Наименование приемника отведенных вод")]
        public string WasteRecieverName
        {
            get
            {
                if (GetErrors(nameof(WasteRecieverName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(WasteRecieverName));
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
                    _dataAccess.Set(nameof(WasteRecieverName), _WasteRecieverName_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteRecieverName));
            }
        }
        
        private string _WasteRecieverName_Not_Valid = "";
        private void WasteRecieverName_Validation()
        {
            ClearErrors(nameof(WasteRecieverName));
        }
        //WasteRecieverName property
        
        //RecieverTypeCode property
        [Attributes.Form_Property("Код типа приемника отведенных вод")]
        public string RecieverTypeCode
        {
            get
            {
                if (GetErrors(nameof(RecieverTypeCode)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RecieverTypeCode));
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
                    _dataAccess.Set(nameof(RecieverTypeCode), _RecieverTypeCode_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverTypeCode));
            }
        }
        
        private string _RecieverTypeCode_Not_Valid = "";
        private void RecieverTypeCode_Validation()
        {
            ClearErrors(nameof(RecieverTypeCode));
        }
        //RecieverTypeCode property
        
        //PoolDistrictName property
        [Attributes.Form_Property("Наименование бассейнового округа приемника отведенных вод")]
        public string PoolDistrictName
        {
            get
            {
                if (GetErrors(nameof(PoolDistrictName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PoolDistrictName));
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
                    _dataAccess.Set(nameof(PoolDistrictName), _PoolDistrictName_Not_Valid);
                }
                OnPropertyChanged(nameof(PoolDistrictName));
            }
        }
        
        private string _PoolDistrictName_Not_Valid = "";
        private void PoolDistrictName_Validation()
        {
            ClearErrors(nameof(PoolDistrictName));
        }
        //PoolDistrictName property

        //AllowedWasteRemovalVolume property
        [Attributes.Form_Property("Допустимый объем водоотведения за год, тыс. куб. м")]
        public double AllowedWasteRemovalVolume
        {
            get
            {
                if (GetErrors(nameof(AllowedWasteRemovalVolume)) != null)
                {
                    return (double)_dataAccess.Get(nameof(AllowedWasteRemovalVolume));
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
                    _dataAccess.Set(nameof(AllowedWasteRemovalVolume), _AllowedWasteRemovalVolume_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedWasteRemovalVolume));
            }
        }
        
        private double _AllowedWasteRemovalVolume_Not_Valid = -1;
        private void AllowedWasteRemovalVolume_Validation()
        {
            ClearErrors(nameof(AllowedWasteRemovalVolume));
        }
        //AllowedWasteRemovalVolume property

        //RemovedWasteVolume property
        [Attributes.Form_Property("Отведено за отчетный период, тыс. куб. м")]
        public double RemovedWasteVolume
        {
            get
            {
                if (GetErrors(nameof(RemovedWasteVolume)) != null)
                {
                    return (double)_dataAccess.Get(nameof(RemovedWasteVolume));
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
                    _dataAccess.Set(nameof(RemovedWasteVolume), _RemovedWasteVolume_Not_Valid);
                }
                OnPropertyChanged(nameof(RemovedWasteVolume));
            }
        }
        
        private double _RemovedWasteVolume_Not_Valid = -1;
        private void RemovedWasteVolume_Validation()
        {
            ClearErrors(nameof(RemovedWasteVolume));
        }
        //RemovedWasteVolume property

        //RemovedWasteVolumeNote property
        public double RemovedWasteVolumeNote
        {
            get
            {
                if (GetErrors(nameof(RemovedWasteVolumeNote)) != null)
                {
                    return (double)_dataAccess.Get(nameof(RemovedWasteVolumeNote));
                }
                else
                {
                    return _RemovedWasteVolumeNote_Not_Valid;
                }
            }
            set
            {
                _RemovedWasteVolumeNote_Not_Valid = value;
                if (GetErrors(nameof(RemovedWasteVolumeNote)) != null)
                {
                    _dataAccess.Set(nameof(RemovedWasteVolumeNote), _RemovedWasteVolumeNote_Not_Valid);
                }
                OnPropertyChanged(nameof(RemovedWasteVolumeNote));
            }
        }
        
        private double _RemovedWasteVolumeNote_Not_Valid = -1;
        private void RemovedWasteVolumeNote_Validation()
        {
            ClearErrors(nameof(RemovedWasteVolumeNote));
        }
        //RemovedWasteVolumeNote property
    }
}
