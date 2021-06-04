using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.7: Поступление радионуклидов в атмосферный воздух")]
    public class Form27 : Abstracts.Form2
    {
        public Form27() : base()
        {
            FormNum.Value = "27";
            NumberOfFields.Value = 13;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PermissionNumber property
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PermissionNumber));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PermissionNumber), value);
                }
                OnPropertyChanged(nameof(PermissionNumber));
            }
        }

        
        private void PermissionNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PermissionNumber property

        //PermissionIssueDate property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PermissionIssueDate));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PermissionIssueDate), value);
                }
                OnPropertyChanged(nameof(PermissionIssueDate));
            }
        }

        
        private void PermissionIssueDate_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PermissionIssueDate property

        //PermissionDocumentName property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PermissionDocumentName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PermissionDocumentName), value);
                }
                OnPropertyChanged(nameof(PermissionDocumentName));
            }
        }

        
        private void PermissionDocumentName_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PermissionDocumentName property

        //ValidBegin property
        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ValidBegin));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ValidBegin), value);
                }
                OnPropertyChanged(nameof(ValidBegin));
            }
        }

        
        private void ValidBegin_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //ValidBegin property

        //ValidThru property
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ValidThru));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ValidThru), value);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }

        
        private void ValidThru_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //ValidThru property

        //RadionuclidNameNote property
        public RamAccess<string> RadionuclidNameNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RadionuclidNameNote));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RadionuclidNameNote), value);
                }
                OnPropertyChanged(nameof(RadionuclidNameNote));
            }
        }

        
        private void RadionuclidNameNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //RadionuclidNameNote property

        //AllowedWasteValue property
        [Attributes.Form_Property("Разрешенный выброс радионуклида в атмосферу за отчетный год, Бк")]
        public RamAccess<string> AllowedWasteValue
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AllowedWasteValue));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AllowedWasteValue), value);
                }
                OnPropertyChanged(nameof(AllowedWasteValue));
            }
        }

        
        private void AllowedWasteValue_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //AllowedWasteValue property

        //AllowedWasteValueNote property
        public RamAccess<string> AllowedWasteValueNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AllowedWasteValueNote));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AllowedWasteValueNote), value);
                }
                OnPropertyChanged(nameof(AllowedWasteValueNote));
            }
        }

        
        private void AllowedWasteValueNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //AllowedWasteValueNote property

        //FactedWasteValue property
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за отчетный год, Бк")]
        public RamAccess<string> FactedWasteValue
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactedWasteValue));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FactedWasteValue), value);
                }
                OnPropertyChanged(nameof(FactedWasteValue));
            }
        }

        
        private void FactedWasteValue_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //FactedWasteValue property

        //FactedWasteValueNote property
        public RamAccess<string> FactedWasteValueNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactedWasteValueNote));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FactedWasteValueNote), value);
                }
                OnPropertyChanged(nameof(FactedWasteValueNote));
            }
        }

        
        private void FactedWasteValueNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //FactedWasteValueNote property

        //WasteOutbreakPreviousYear property
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за предыдущий год, Бк")]
        public RamAccess<string> WasteOutbreakPreviousYear
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteOutbreakPreviousYear));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(WasteOutbreakPreviousYear), value);
                }
                OnPropertyChanged(nameof(WasteOutbreakPreviousYear));
            }
        }

        
        private void WasteOutbreakPreviousYear_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //WasteOutbreakPreviousYear property
    }
}
