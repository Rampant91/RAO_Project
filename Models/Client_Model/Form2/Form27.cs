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
        public DateTime ValidBegin
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
        private DateTime _ValidBegin_Not_Valid = DateTime.MinValue;
        private void ValidBegin_Validation()
        {
            ClearErrors(nameof(ValidBegin));
        }
        //ValidBegin property

        //ValidThru property
        [Attributes.FormVisual("Действует по")]
        public DateTime ValidThru
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
        private DateTime _ValidThru_Not_Valid = DateTime.MinValue;
        private void ValidThru_Validation()
        {
            ClearErrors(nameof(ValidThru));
        }
        //ValidThru property

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
        
        //AllowedWasteValue property
        [Attributes.FormVisual("Разрешенный выброс радионуклида в атмосферу за отчетный год, Бк")]
        public string AllowedWasteValue
        {
            get
            {
                if (GetErrors(nameof(AllowedWasteValue)) != null)
                {
                    return (string)_AllowedWasteValue.Get();
                }
                else
                {
                    return _AllowedWasteValue_Not_Valid;
                }
            }
            set
            {
                _AllowedWasteValue_Not_Valid = value;
                if (GetErrors(nameof(AllowedWasteValue)) != null)
                {
                    _AllowedWasteValue.Set(_AllowedWasteValue_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedWasteValue));
            }
        }
        private IDataLoadEngine _AllowedWasteValue;
        private string _AllowedWasteValue_Not_Valid = "";
        private void AllowedWasteValue_Validation()
        {
            ClearErrors(nameof(AllowedWasteValue));
        }
        //AllowedWasteValue property

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
        
        //FactedWasteValue property
        [Attributes.FormVisual("Фактический выброс радионуклида в атмосферу за отчетный год, Бк")]
        public string FactedWasteValue
        {
            get
            {
                if (GetErrors(nameof(FactedWasteValue)) != null)
                {
                    return (string)_FactedWasteValue.Get();
                }
                else
                {
                    return _FactedWasteValue_Not_Valid;
                }
            }
            set
            {
                _FactedWasteValue_Not_Valid = value;
                if (GetErrors(nameof(FactedWasteValue)) != null)
                {
                    _FactedWasteValue.Set(_FactedWasteValue_Not_Valid);
                }
                OnPropertyChanged(nameof(FactedWasteValue));
            }
        }
        private IDataLoadEngine _FactedWasteValue;
        private string _FactedWasteValue_Not_Valid = "";
        private void FactedWasteValue_Validation()
        {
            ClearErrors(nameof(FactedWasteValue));
        }
        //FactedWasteValue property

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

        //WasteOutbreakPreviousYear property
        [Attributes.FormVisual("Фактический выброс радионуклида в атмосферу за предыдущий год, Бк")]
        public string WasteOutbreakPreviousYear
        {
            get
            {
                if (GetErrors(nameof(WasteOutbreakPreviousYear)) != null)
                {
                    return (string)_WasteOutbreakPreviousYear.Get();
                }
                else
                {
                    return _WasteOutbreakPreviousYear_Not_Valid;
                }
            }
            set
            {
                _WasteOutbreakPreviousYear_Not_Valid = value;
                if (GetErrors(nameof(WasteOutbreakPreviousYear)) != null)
                {
                    _WasteOutbreakPreviousYear.Set(_WasteOutbreakPreviousYear_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteOutbreakPreviousYear));
            }
        }
        private IDataLoadEngine _WasteOutbreakPreviousYear;
        private string _WasteOutbreakPreviousYear_Not_Valid = "";
        private void WasteOutbreakPreviousYear_Validation()
        {
            ClearErrors(nameof(WasteOutbreakPreviousYear));
        }
        //WasteOutbreakPreviousYear property
    }
}
