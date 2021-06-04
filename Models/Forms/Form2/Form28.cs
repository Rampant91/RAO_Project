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
            FormNum.Value = "28";
            NumberOfFields.Value = 24;
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

        
        private bool PermissionNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
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

        
        private bool PermissionIssueDate_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
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

        
        private bool PermissionDocumentName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
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

        
        private bool ValidBegin_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
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

        
        private bool ValidThru_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //ValidThru property

        //PermissionNumber1 property
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber1
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(PermissionNumber1));
            }
            set
            {
                    _dataAccess.Set(nameof(PermissionNumber1), value);
                OnPropertyChanged(nameof(PermissionNumber1));
            }
        }

        
        private bool PermissionNumber1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PermissionNumber1 property

        //PermissionIssueDate1 property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate1
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(PermissionIssueDate1));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(PermissionIssueDate1), value);
                OnPropertyChanged(nameof(PermissionIssueDate1));
            }
        }

        
        private bool PermissionIssueDate1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PermissionIssueDate1 property

        //PermissionDocumentName1 property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName1
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(PermissionDocumentName1));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(PermissionDocumentName1), value);
                OnPropertyChanged(nameof(PermissionDocumentName1));
            }
        }

        
        private bool PermissionDocumentName1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PermissionDocumentName1 property

        //ValidBegin1 property
        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin1
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(ValidBegin1));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(ValidBegin1), value);
                OnPropertyChanged(nameof(ValidBegin1));
            }
        }

        
        private bool ValidBegin1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //ValidBegin1 property

        //ValidThru1 property
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru1
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(ValidThru1));
            }
            set
            {
                    _dataAccess.Set(nameof(ValidThru1), value);
                OnPropertyChanged(nameof(ValidThru1));
            }
        }

        
        private bool ValidThru1_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //ValidThru1 property

        //PermissionNumber2 property
        [Attributes.Form_Property("Номер разрешительного документа")]
        public RamAccess<string> PermissionNumber2
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(PermissionNumber2));
            }
            set
            {
                    _dataAccess.Set(nameof(PermissionNumber2), value);
                OnPropertyChanged(nameof(PermissionNumber2));
            }
        }

        
        private bool PermissionNumber2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PermissionNumber2 property

        //PermissionIssueDate2 property
        [Attributes.Form_Property("Дата выпуска разрешительного документа")]
        public RamAccess<string> PermissionIssueDate2
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(PermissionIssueDate2));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(PermissionIssueDate2), value);
                OnPropertyChanged(nameof(PermissionIssueDate2));
            }
        }

        
        private bool PermissionIssueDate2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PermissionIssueDate property

        //PermissionDocumentName2 property
        [Attributes.Form_Property("Наименование разрешительного документа")]
        public RamAccess<string> PermissionDocumentName2
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(PermissionDocumentName2));
                }
                
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

        
        private bool PermissionDocumentName2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PermissionDocumentName2 property

        //ValidBegin2 property
        [Attributes.Form_Property("Действует с")]
        public RamAccess<string> ValidBegin2
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(ValidBegin2));
            }
            set
            {
                    _dataAccess.Set(nameof(ValidBegin2), value);
                OnPropertyChanged(nameof(ValidBegin2));
            }
        }

        
        private bool ValidBegin2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //ValidBegin2 property

        //ValidThru2 property
        [Attributes.Form_Property("Действует по")]
        public RamAccess<string> ValidThru2
        {
            get
            {
                {
                    return _dataAccess.Get<string>(nameof(ValidThru2));
                }
                
                {
                    
                }
            }
            set
            {
                    _dataAccess.Set(nameof(ValidThru2), value);
                OnPropertyChanged(nameof(ValidThru2));
            }
        }

        
        private bool ValidThru2_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //ValidThru2 property

        //WasteSourceName property
        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public RamAccess<string> WasteSourceName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteSourceName));
                }
                
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

        
        private bool WasteSourceName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //WasteSourceName property

        //WasteRecieverName property
        [Attributes.Form_Property("Наименование приемника отведенных вод")]
        public RamAccess<string> WasteRecieverName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(WasteRecieverName));
                }
                
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

        
        private bool WasteRecieverName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //WasteRecieverName property

        //RecieverTypeCode property
        [Attributes.Form_Property("Код типа приемника отведенных вод")]
        public RamAccess<string> RecieverTypeCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RecieverTypeCode));
                }
                
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

        
        private bool RecieverTypeCode_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //RecieverTypeCode property

        //PoolDistrictName property
        [Attributes.Form_Property("Наименование бассейнового округа приемника отведенных вод")]
        public RamAccess<string> PoolDistrictName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PoolDistrictName));
                }
                
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

        
        private bool PoolDistrictName_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PoolDistrictName property

        //AllowedWasteRemovalVolume property
        [Attributes.Form_Property("Допустимый объем водоотведения за год, тыс. куб. м")]
        public RamAccess<double> AllowedWasteRemovalVolume
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(AllowedWasteRemovalVolume));
                }
                
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

        
        private bool AllowedWasteRemovalVolume_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //AllowedWasteRemovalVolume property

        //RemovedWasteVolume property
        [Attributes.Form_Property("Отведено за отчетный период, тыс. куб. м")]
        public RamAccess<double> RemovedWasteVolume
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(RemovedWasteVolume));
                }
                
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

        
        private bool RemovedWasteVolume_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //RemovedWasteVolume property

        //RemovedWasteVolumeNote property
        public RamAccess<double> RemovedWasteVolumeNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<double>(nameof(RemovedWasteVolumeNote));
                }
                
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

        
        private bool RemovedWasteVolumeNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //RemovedWasteVolumeNote property
    }
}
