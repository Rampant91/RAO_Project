using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.8: Отведение сточных вод, содержащих радионуклиды")]
    public class Form28 : Abstracts.Form2
    {
        public Form28() : base()
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

        //PermissionNumber1 property
        [Attributes.Form_Property("Номер разрешительного документа")]
        public IDataAccess<string> PermissionNumber1
        {
            get
            {
                if (GetErrors(nameof(PermissionNumber1)) == null)
                {
                    return _dataAccess.Get<string>(nameof(PermissionNumber1));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(PermissionNumber1)) == null)
                {
                    _dataAccess.Set(nameof(PermissionNumber1), value);
                }
                OnPropertyChanged(nameof(PermissionNumber1));
            }
        }

        
        private void PermissionNumber1_Validation()
        {
            ClearErrors(nameof(PermissionNumber1));
        }
        //PermissionNumber1 property

        //PermissionIssueDate1 property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public IDataAccess<string> PermissionIssueDate1
        {
            get
            {
                if (GetErrors(nameof(PermissionIssueDate1)) == null)
                {
                    return _dataAccess.Get<string>(nameof(PermissionIssueDate1));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(PermissionIssueDate1)) == null)
                {
                    _dataAccess.Set(nameof(PermissionIssueDate1), value);
                }
                OnPropertyChanged(nameof(PermissionIssueDate1));
            }
        }

        
        private void PermissionIssueDate1_Validation()
        {
            ClearErrors(nameof(PermissionIssueDate1));
        }
        //PermissionIssueDate1 property

        //PermissionDocumentName1 property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public IDataAccess<string> PermissionDocumentName1
        {
            get
            {
                if (GetErrors(nameof(PermissionDocumentName1)) == null)
                {
                    return _dataAccess.Get<string>(nameof(PermissionDocumentName1));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(PermissionDocumentName1)) == null)
                {
                    _dataAccess.Set(nameof(PermissionDocumentName1), value);
                }
                OnPropertyChanged(nameof(PermissionDocumentName1));
            }
        }

        
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
                if (GetErrors(nameof(ValidBegin1)) == null)
                {
                    return _dataAccess.Get<string>(nameof(ValidBegin1));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(ValidBegin1)) == null)
                {
                    _dataAccess.Set(nameof(ValidBegin1), value);
                }
                OnPropertyChanged(nameof(ValidBegin1));
            }
        }

        
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
                if (GetErrors(nameof(ValidThru1)) == null)
                {
                    return _dataAccess.Get<string>(nameof(ValidThru1));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(ValidThru1)) == null)
                {
                    _dataAccess.Set(nameof(ValidThru1), value);
                }
                OnPropertyChanged(nameof(ValidThru1));
            }
        }

        
        private void ValidThru1_Validation()
        {
            ClearErrors(nameof(ValidThru1));
        }
        //ValidThru1 property

        //PermissionNumber2 property
        [Attributes.Form_Property("Номер разрешительного документа")]
        public IDataAccess<string> PermissionNumber2
        {
            get
            {
                if (GetErrors(nameof(PermissionNumber2)) == null)
                {
                    return _dataAccess.Get<string>(nameof(PermissionNumber2));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(PermissionNumber2)) == null)
                {
                    _dataAccess.Set(nameof(PermissionNumber2), value);
                }
                OnPropertyChanged(nameof(PermissionNumber2));
            }
        }

        
        private void PermissionNumber2_Validation()
        {
            ClearErrors(nameof(PermissionNumber2));
        }
        //PermissionNumber2 property

        //PermissionIssueDate2 property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public IDataAccess<string> PermissionIssueDate2
        {
            get
            {
                if (GetErrors(nameof(PermissionIssueDate2)) == null)
                {
                    return _dataAccess.Get<string>(nameof(PermissionIssueDate2));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(PermissionIssueDate2)) == null)
                {
                    _dataAccess.Set(nameof(PermissionIssueDate2), value);
                }
                OnPropertyChanged(nameof(PermissionIssueDate2));
            }
        }

        
        private void PermissionIssueDate2_Validation()
        {
            ClearErrors(nameof(PermissionIssueDate2));
        }
        //PermissionIssueDate property

        //PermissionDocumentName2 property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public IDataAccess<string> PermissionDocumentName2
        {
            get
            {
                if (GetErrors(nameof(PermissionDocumentName2)) == null)
                {
                    return _dataAccess.Get<string>(nameof(PermissionDocumentName2));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PermissionDocumentName2), value);
                }
                OnPropertyChanged(nameof(PermissionDocumentName2));
            }
        }

        
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
                if (GetErrors(nameof(ValidBegin2)) == null)
                {
                    return _dataAccess.Get<string>(nameof(ValidBegin2));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(ValidBegin2)) == null)
                {
                    _dataAccess.Set(nameof(ValidBegin2), value);
                }
                OnPropertyChanged(nameof(ValidBegin2));
            }
        }

        
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
                if (GetErrors(nameof(ValidThru2)) == null)
                {
                    return _dataAccess.Get<string>(nameof(ValidThru2));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(ValidThru2)) == null)
                {
                    _dataAccess.Set(nameof(ValidThru2), value);
                }
                OnPropertyChanged(nameof(ValidThru2));
            }
        }

        
        private void ValidThru2_Validation()
        {
            ClearErrors(nameof(ValidThru2));
        }
        //ValidThru2 property

        //WasteSourceName property
        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public IDataAccess<string> WasteSourceName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteSourceName));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(WasteSourceName), value);
                }
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }

        
        private void WasteSourceName_Validation()
        {
            value.ClearErrors();
        }
        //WasteSourceName property

        //WasteRecieverName property
        [Attributes.Form_Property("Наименование приемника отведенных вод")]
        public IDataAccess<string> WasteRecieverName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteRecieverName));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(WasteRecieverName), value);
                }
                OnPropertyChanged(nameof(WasteRecieverName));
            }
        }

        
        private void WasteRecieverName_Validation()
        {
            value.ClearErrors();
        }
        //WasteRecieverName property

        //RecieverTypeCode property
        [Attributes.Form_Property("Код типа приемника отведенных вод")]
        public IDataAccess<string> RecieverTypeCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RecieverTypeCode));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RecieverTypeCode), value);
                }
                OnPropertyChanged(nameof(RecieverTypeCode));
            }
        }

        
        private void RecieverTypeCode_Validation()
        {
            value.ClearErrors();
        }
        //RecieverTypeCode property

        //PoolDistrictName property
        [Attributes.Form_Property("Наименование бассейнового округа приемника отведенных вод")]
        public IDataAccess<string> PoolDistrictName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PoolDistrictName));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PoolDistrictName), value);
                }
                OnPropertyChanged(nameof(PoolDistrictName));
            }
        }

        
        private void PoolDistrictName_Validation()
        {
            value.ClearErrors();
        }
        //PoolDistrictName property

        //AllowedWasteRemovalVolume property
        [Attributes.Form_Property("Допустимый объем водоотведения за год, тыс. куб. м")]
        public double AllowedWasteRemovalVolume
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(AllowedWasteRemovalVolume));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AllowedWasteRemovalVolume), value);
                }
                OnPropertyChanged(nameof(AllowedWasteRemovalVolume));
            }
        }

        
        private void AllowedWasteRemovalVolume_Validation()
        {
            value.ClearErrors();
        }
        //AllowedWasteRemovalVolume property

        //RemovedWasteVolume property
        [Attributes.Form_Property("Отведено за отчетный период, тыс. куб. м")]
        public double RemovedWasteVolume
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RemovedWasteVolume));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RemovedWasteVolume), value);
                }
                OnPropertyChanged(nameof(RemovedWasteVolume));
            }
        }

        
        private void RemovedWasteVolume_Validation()
        {
            value.ClearErrors();
        }
        //RemovedWasteVolume property

        //RemovedWasteVolumeNote property
        public double RemovedWasteVolumeNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RemovedWasteVolumeNote));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RemovedWasteVolumeNote), value);
                }
                OnPropertyChanged(nameof(RemovedWasteVolumeNote));
            }
        }

        
        private void RemovedWasteVolumeNote_Validation()
        {
            value.ClearErrors();
        }
        //RemovedWasteVolumeNote property
    }
}
