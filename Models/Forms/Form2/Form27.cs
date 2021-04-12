using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
    public class Form27 : Abstracts.Form2
    {
        public Form27(int RowID) : base(RowID)
        {
            FormNum = "27";
            NumberOfFields = 15;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //PermissionNumber property
        [Attributes.Form_Property("Номер разрешительного документа")]
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
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
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
        [Attributes.Form_Property("Наименование разрешительного документа")]
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
        [Attributes.Form_Property("Действует с")]
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
        [Attributes.Form_Property("Действует по")]
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

        //RadionuclidNameNote property
        public string RadionuclidNameNote
        {
            get
            {
                if (GetErrors(nameof(RadionuclidNameNote)) != null)
                {
                    return (string)_RadionuclidNameNote.Get();
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
                    _RadionuclidNameNote.Set(_RadionuclidNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(RadionuclidNameNote));
            }
        }
        private IDataLoadEngine _RadionuclidNameNote;
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

        //AllowedWasteValueNote property
        public string AllowedWasteValueNote
        {
            get
            {
                if (GetErrors(nameof(AllowedWasteValueNote)) != null)
                {
                    return (string)_AllowedWasteValueNote.Get();
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
                    _AllowedWasteValueNote.Set(_AllowedWasteValueNote_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedWasteValueNote));
            }
        }
        private IDataLoadEngine _AllowedWasteValueNote;
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

        //FactedWasteValueNote property
        public string FactedWasteValueNote
        {
            get
            {
                if (GetErrors(nameof(FactedWasteValueNote)) != null)
                {
                    return (string)_FactedWasteValueNote.Get();
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
                    _FactedWasteValueNote.Set(_FactedWasteValueNote_Not_Valid);
                }
                OnPropertyChanged(nameof(FactedWasteValueNote));
            }
        }
        private IDataLoadEngine _FactedWasteValueNote;
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
