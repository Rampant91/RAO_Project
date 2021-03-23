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
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 24;

        private int _numberInOrder = -1;
        [Attributes.FormVisual("№ п/п")]
        public int NumberInOrder
        {
            get { return _numberInOrder; }
            set
            {
                _numberInOrder = value;
                OnPropertyChanged("NumberInOrder");
            }
        }

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return _correctionNumber;
                }
                else
                {
                    return _correctionNumber_Not_Valid;
                }
            }
            set
            {
                _correctionNumber_Not_Valid = value;
                if (CorrectionNumber_Validation())
                {
                    _correctionNumber = _correctionNumber_Not_Valid;
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private byte _correctionNumber = 255;
        private byte _correctionNumber_Not_Valid = 255;
        private bool CorrectionNumber_Validation()
        {
            return true;
            //ClearErrors(nameof(CorrectionNumber));
            ////Пример
            //if (value < 10)
            //    AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }

        private string _permissionNumber = "";
        [Attributes.FormVisual("Номер разрешительного документа")]
        public string PermissionNumber
        {
            get { return _permissionNumber; }
            set
            {
                _permissionNumber = value;
                OnPropertyChanged("PermissionNumber");
            }
        }

        private string _permissionIssueDate = "";
        public string PermissionIssueDate
        {
            get { return _permissionIssueDate; }
            set
            {
                _permissionIssueDate = value;
                OnPropertyChanged("PermissionIssueDate");
            }
        }

        private string _permissionDocumentName = "";
        [Attributes.FormVisual("Наименование разрешительного документа")]
        public string PermissionDocumentName
        {
            get { return _permissionDocumentName; }
            set
            {
                _permissionDocumentName = value;
                OnPropertyChanged("PermissionDocumentName");
            }
        }

        private DateTime _validBegin = DateTime.MinValue;
        public DateTime ValidBegin
        {
            get { return _validBegin; }
            set
            {
                _validBegin = value;
                OnPropertyChanged("ValidBegin");
            }
        }

        private DateTime _validThru = DateTime.MinValue;
        public DateTime ValidThru
        {
            get { return _validThru; }
            set
            {
                _validThru = value;
                OnPropertyChanged("ValidThru");
            }
        }

        private string _permissionNumber1 = "";
        [Attributes.FormVisual("Номер разрешительного документа")]
        public string PermissionNumber1
        {
            get { return _permissionNumber1; }
            set
            {
                _permissionNumber1 = value;
                OnPropertyChanged("PermissionNumber1");
            }
        }

        private string _permissionIssueDate1 = "";
        public string PermissionIssueDate1
        {
            get { return _permissionIssueDate1; }
            set
            {
                _permissionIssueDate1 = value;
                OnPropertyChanged("PermissionIssueDate1");
            }
        }

        private string _permissionDocumentName1 = "";
        public string PermissionDocumentName1
        {
            get { return _permissionDocumentName1; }
            set
            {
                _permissionDocumentName1 = value;
                OnPropertyChanged("PermissionDocumentName1");
            }
        }

        private DateTime _validBegin1 = DateTime.MinValue;
        public DateTime ValidBegin1
        {
            get { return _validBegin1; }
            set
            {
                _validBegin1 = value;
                OnPropertyChanged("ValidBegin1");
            }
        }

        private DateTime _validThru1 = DateTime.MinValue;
        public DateTime ValidThru1
        {
            get { return _validThru1; }
            set
            {
                _validThru1 = value;
                OnPropertyChanged("ValidThru1");
            }
        }

        private string _permissionNumber2 = "";
        [Attributes.FormVisual("Номер разрешительного документа")]
        public string PermissionNumber2
        {
            get { return _permissionNumber2; }
            set
            {
                _permissionNumber2 = value;
                OnPropertyChanged("PermissionNumber2");
            }
        }

        private string _permissionIssueDate2 = "";
        public string PermissionIssueDate2
        {
            get { return _permissionIssueDate2; }
            set
            {
                _permissionIssueDate2 = value;
                OnPropertyChanged("PermissionIssueDate2");
            }
        }

        private string _permissionDocumentName2 = "";
        public string PermissionDocumentName2
        {
            get { return _permissionDocumentName2; }
            set
            {
                _permissionDocumentName2 = value;
                OnPropertyChanged("PermissionDocumentName2");
            }
        }

        private DateTime _validBegin2 = DateTime.MinValue;
        public DateTime ValidBegin2
        {
            get { return _validBegin2; }
            set
            {
                _validBegin2 = value;
                OnPropertyChanged("ValidBegin2");
            }
        }

        private DateTime _validThru2 = DateTime.MinValue;
        public DateTime ValidThru2
        {
            get { return _validThru2; }
            set
            {
                _validThru2 = value;
                OnPropertyChanged("ValidThru2");
            }
        }

        private string _wasteSourceName = "";
        [Attributes.FormVisual("Наименование, номер выпуска сточных вод")]
        public string WasteSourceName
        {
            get { return _wasteSourceName; }
            set
            {
                _wasteSourceName = value;
                OnPropertyChanged("WasteSourceName");
            }
        }

        private string _wasteRecieverName = "";
        [Attributes.FormVisual("Наименование приемника отведенных вод")]
        public string WasteRecieverName
        {
            get { return _wasteRecieverName; }
            set
            {
                _wasteRecieverName = value;
                OnPropertyChanged("WasteRecieverName");
            }
        }

        private string _recieverTypeCode = "";
        [Attributes.FormVisual("Код типа приемника отведенных вод")]
        public string RecieverTypeCode
        {
            get { return _recieverTypeCode; }
            set
            {
                _recieverTypeCode = value;
                OnPropertyChanged("RecieverTypeCode");
            }
        }

        private string _poolDistrictName = "";
        [Attributes.FormVisual("Наименование бассейнового округа приемника отведенных вод")]
        public string PoolDistrictName
        {
            get { return _poolDistrictName; }
            set
            {
                _poolDistrictName = value;
                OnPropertyChanged("PoolDistrictName");
            }
        }

        private double _allowedWasteRemovalVolume = -1;
        [Attributes.FormVisual("Допустимый объем водоотведения за год, тыс. куб. м")]
        public double AllowedWasteRemovalVolume
        {
            get { return _allowedWasteRemovalVolume; }
            set
            {
                _allowedWasteRemovalVolume = value;
                OnPropertyChanged("AllowedWasteRemovalVolume");
            }
        }

        private double _removedWasteVolume = -1;
        [Attributes.FormVisual("Отведено за отчетный период, тыс. куб. м")]
        public double RemovedWasteVolume
        {
            get { return _removedWasteVolume; }
            set
            {
                _removedWasteVolume = value;
                OnPropertyChanged("RemovedWasteVolume");
            }
        }

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
