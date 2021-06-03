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
        public IDataAccess<string> PermissionNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PermissionNumber));
                }
                else
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

        
        private void PermissionNumber_Validation()
        {
            value.ClearErrors();
        }
        //PermissionNumber property

        //PermissionIssueDate property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public IDataAccess<string> PermissionIssueDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PermissionIssueDate));
                }
                else
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

        
        private void PermissionIssueDate_Validation()
        {
            value.ClearErrors();
        }
        //PermissionIssueDate property

        //PermissionDocumentName property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public IDataAccess<string> PermissionDocumentName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PermissionDocumentName));
                }
                else
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

        
        private void PermissionDocumentName_Validation()
        {
            value.ClearErrors();
        }
        //PermissionDocumentName property

        //ValidBegin property
        [Attributes.Form_Property("Действует с")]
        public DateTimeOffset ValidBegin
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ValidBegin));
                }
                else
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

        
        private void ValidBegin_Validation()
        {
            value.ClearErrors();
        }
        //ValidBegin property

        //ValidThru property
        [Attributes.Form_Property("Действует по")]
        public DateTimeOffset ValidThru
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ValidThru));
                }
                else
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

        
        private void ValidThru_Validation()
        {
            value.ClearErrors();
        }
        //ValidThru property

        //RadionuclidNameNote property
        public IDataAccess<string> RadionuclidNameNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RadionuclidNameNote));
                }
                else
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

        
        private void RadionuclidNameNote_Validation()
        {
            value.ClearErrors();
        }
        //RadionuclidNameNote property

        //AllowedWasteValue property
        [Attributes.Form_Property("Разрешенный выброс радионуклида в атмосферу за отчетный год, Бк")]
        public IDataAccess<string> AllowedWasteValue
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AllowedWasteValue));
                }
                else
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

        
        private void AllowedWasteValue_Validation()
        {
            value.ClearErrors();
        }
        //AllowedWasteValue property

        //AllowedWasteValueNote property
        public IDataAccess<string> AllowedWasteValueNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AllowedWasteValueNote));
                }
                else
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

        
        private void AllowedWasteValueNote_Validation()
        {
            value.ClearErrors();
        }
        //AllowedWasteValueNote property

        //FactedWasteValue property
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за отчетный год, Бк")]
        public IDataAccess<string> FactedWasteValue
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactedWasteValue));
                }
                else
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

        
        private void FactedWasteValue_Validation()
        {
            value.ClearErrors();
        }
        //FactedWasteValue property

        //FactedWasteValueNote property
        public IDataAccess<string> FactedWasteValueNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactedWasteValueNote));
                }
                else
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

        
        private void FactedWasteValueNote_Validation()
        {
            value.ClearErrors();
        }
        //FactedWasteValueNote property

        //WasteOutbreakPreviousYear property
        [Attributes.Form_Property("Фактический выброс радионуклида в атмосферу за предыдущий год, Бк")]
        public IDataAccess<string> WasteOutbreakPreviousYear
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteOutbreakPreviousYear));
                }
                else
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

        
        private void WasteOutbreakPreviousYear_Validation()
        {
            value.ClearErrors();
        }
        //WasteOutbreakPreviousYear property
    }
}
