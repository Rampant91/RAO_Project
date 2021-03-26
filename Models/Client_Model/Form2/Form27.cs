using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
    public class Form27 : Form2
    {
        public override string FormNum { get { return "2.7"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 15;

        private string _permissionNumber = "";
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

        private string _radionuclidNameNote = "";
        public string RadionuclidNameNote
        {
            get { return _radionuclidNameNote; }
            set
            {
                _radionuclidNameNote = value;
                OnPropertyChanged("RadionuclidNameNote");
            }
        }

        private string _allowedWasteValue = "";
        [Attributes.FormVisual("Разрешенный выброс радионуклида в атмосферу за отчетный год, Бк")]
        public string AllowedWasteValue
        {
            get { return _allowedWasteValue; }
            set
            {
                _allowedWasteValue = value;
                OnPropertyChanged("AllowedWasteValue");
            }
        }

        private string _allowedWasteValueNote = "";
        public string AllowedWasteValueNote
        {
            get { return _allowedWasteValueNote; }
            set
            {
                _allowedWasteValueNote = value;
                OnPropertyChanged("AllowedWasteValueNote");
            }
        }

        private string _factedWasteValue = "";
        [Attributes.FormVisual("Фактический выброс радионуклида в атмосферу за отчетный год, Бк")]
        public string FactedWasteValue
        {
            get { return _factedWasteValue; }
            set
            {
                _factedWasteValue = value;
                OnPropertyChanged("FactedWasteValue");
            }
        }

        private string _factedWasteValueNote = "";
        public string FactedWasteValueNote
        {
            get { return _factedWasteValueNote; }
            set
            {
                _factedWasteValueNote = value;
                OnPropertyChanged("FactedWasteValueNote");
            }
        }

        private string _wasteOutbreakPreviousYear = "";
        [Attributes.FormVisual("Фактический выброс радионуклида в атмосферу за предыдущий год, Бк")]
        public string WasteOutbreakPreviousYear
        {
            get { return _wasteOutbreakPreviousYear; }
            set
            {
                _wasteOutbreakPreviousYear = value;
                OnPropertyChanged("WasteOutbreakPreviousYear");
            }
        }
    }
}
