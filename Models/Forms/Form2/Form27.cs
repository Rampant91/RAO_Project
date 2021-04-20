using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DBRealization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
    public class Form27 : Abstracts.Form2
    {


        public Form27(IDataAccess Access) : base(Access)
        {
            FormNum = "27";
            NumberOfFields = 13;
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

        //RadionuclidNameNote property
        public string RadionuclidNameNote
        {
            get
            {
                if (GetErrors(nameof(RadionuclidNameNote)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RadionuclidNameNote));
                }
                else
                {
                    return _RadionuclidNameNote_Not_Valid;
                }
            }
            set
            {
                _RadionuclidNameNote_Not_Valid = value;
                if (GetErrors(nameof(RadionuclidNameNote)) != null)
                {
                    _dataAccess.Set(nameof(RadionuclidNameNote), _RadionuclidNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(RadionuclidNameNote));
            }
        }
        
        private string _RadionuclidNameNote_Not_Valid = "";
        private void RadionuclidNameNote_Validation()
        {
            ClearErrors(nameof(RadionuclidNameNote));
        }
        //RadionuclidNameNote property

        //AllowedWasteValue property
        [Attributes.Form_Property("Разрешенный выброс радионуклида в атмосферу за отчетный год, Бк")]
        public string AllowedWasteValue
        {
            get
            {
                if (GetErrors(nameof(AllowedWasteValue)) != null)
                {
                    return (string)_dataAccess.Get(nameof(AllowedWasteValue));
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
                    _dataAccess.Set(nameof(AllowedWasteValue), _AllowedWasteValue_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedWasteValue));
            }
        }
        
        private string _AllowedWasteValue_Not_Valid = "";
        private void AllowedWasteValue_Validation()
        {
            ClearErrors(nameof(AllowedWasteValue));
        }
        //AllowedWasteValue property

        //AllowedWasteValueNote property
        public string AllowedWasteValueNote
        {
            get
            {
                if (GetErrors(nameof(AllowedWasteValueNote)) != null)
                {
                    return (string)_dataAccess.Get(nameof(AllowedWasteValueNote));
                }
                else
                {
                    return _AllowedWasteValueNote_Not_Valid;
                }
            }
            set
            {
                _AllowedWasteValueNote_Not_Valid = value;
                if (GetErrors(nameof(AllowedWasteValueNote)) != null)
                {
                    _dataAccess.Set(nameof(AllowedWasteValueNote), _AllowedWasteValueNote_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedWasteValueNote));
            }
        }
        
        private string _AllowedWasteValueNote_Not_Valid = "";
        private void AllowedWasteValueNote_Validation()
        {
            ClearErrors(nameof(AllowedWasteValueNote));
        }
        //AllowedWasteValueNote property

        //FactedWasteValue property
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за отчетный год, Бк")]
        public string FactedWasteValue
        {
            get
            {
                if (GetErrors(nameof(FactedWasteValue)) != null)
                {
                    return (string)_dataAccess.Get(nameof(FactedWasteValue));
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
                    _dataAccess.Set(nameof(FactedWasteValue), _FactedWasteValue_Not_Valid);
                }
                OnPropertyChanged(nameof(FactedWasteValue));
            }
        }
        
        private string _FactedWasteValue_Not_Valid = "";
        private void FactedWasteValue_Validation()
        {
            ClearErrors(nameof(FactedWasteValue));
        }
        //FactedWasteValue property

        //FactedWasteValueNote property
        public string FactedWasteValueNote
        {
            get
            {
                if (GetErrors(nameof(FactedWasteValueNote)) != null)
                {
                    return (string)_dataAccess.Get(nameof(FactedWasteValueNote));
                }
                else
                {
                    return _FactedWasteValueNote_Not_Valid;
                }
            }
            set
            {
                _FactedWasteValueNote_Not_Valid = value;
                if (GetErrors(nameof(FactedWasteValueNote)) != null)
                {
                    _dataAccess.Set(nameof(FactedWasteValueNote), _FactedWasteValueNote_Not_Valid);
                }
                OnPropertyChanged(nameof(FactedWasteValueNote));
            }
        }
        
        private string _FactedWasteValueNote_Not_Valid = "";
        private void FactedWasteValueNote_Validation()
        {
            ClearErrors(nameof(FactedWasteValueNote));
        }
        //FactedWasteValueNote property

        //WasteOutbreakPreviousYear property
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за предыдущий год, Бк")]
        public string WasteOutbreakPreviousYear
        {
            get
            {
                if (GetErrors(nameof(WasteOutbreakPreviousYear)) != null)
                {
                    return (string)_dataAccess.Get(nameof(WasteOutbreakPreviousYear));
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
                    _dataAccess.Set(nameof(WasteOutbreakPreviousYear), _WasteOutbreakPreviousYear_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteOutbreakPreviousYear));
            }
        }
        
        private string _WasteOutbreakPreviousYear_Not_Valid = "";
        private void WasteOutbreakPreviousYear_Validation()
        {
            ClearErrors(nameof(WasteOutbreakPreviousYear));
        }
        //WasteOutbreakPreviousYear property
    }
}
