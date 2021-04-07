using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 5.7: Перечень подведомственных организаций ведомственного информационно-аналитического центра федерального органа исполнительной власти")]
    public class Form57: Form5
    {
        public Form57(bool isSQL) : base()
        {
            FormNum = "57";
            NumberOfFields = 9;
            if (isSQL)
            {
                _Note = new SQLite("Note", FormNum, 0);
                _PermissionNameNumber = new SQLite("PermissionNameNumber", FormNum, 0);
                _DocumentNameNumber = new SQLite("DocumentNameNumber", FormNum, 0);
                _OrgName = new SQLite("OrgName", FormNum, 0);
                _AllowedActivity = new SQLite("AllowedActivity", FormNum, 0);
                _RegNo = new SQLite("RegNo", FormNum, 0);
                _Okpo = new SQLite("Okpo", FormNum, 0);
            }
            else
            {
                _Note = new File();
                _PermissionNameNumber = new File();
                _DocumentNameNumber = new File();
                _OrgName = new File();
                _AllowedActivity = new File();
                _RegNo = new File();
                _Okpo = new File();
            }
        }

        [Attributes.FormVisual("Форма")]
        public override void Object_Validation()
        {

        }

        //RegNo property
        [Attributes.FormVisual("Регистрационный номер")]
        public string RegNo
        {
            get
            {
                if (GetErrors(nameof(RegNo)) != null)
                {
                    return (string)_RegNo.Get();
                }
                else
                {
                    return _RegNo_Not_Valid;
                }
            }
            set
            {
                _RegNo_Not_Valid = value;
                if (GetErrors(nameof(RegNo)) != null)
                {
                    _RegNo.Set(_RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }
        private IDataLoadEngine _RegNo;
        private string _RegNo_Not_Valid = "";
        //RegNo property

        //Okpo property
        [Attributes.FormVisual("ОКПО")]
        public string Okpo
        {
            get
            {
                if (GetErrors(nameof(Okpo)) != null)
                {
                    return (string)_Okpo.Get();
                }
                else
                {
                    return _Okpo_Not_Valid;
                }
            }
            set
            {
                _Okpo_Not_Valid = value;
                if (GetErrors(nameof(Okpo)) != null)
                {
                    _Okpo.Set(_Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        private IDataLoadEngine _Okpo;
        private string _Okpo_Not_Valid = "";
        //Okpo property

        //OrgName property
        [Attributes.FormVisual("Наименование организации")]
        public string OrgName
        {
            get
            {
                if (GetErrors(nameof(OrgName)) != null)
                {
                    return (string)_OrgName.Get();
                }
                else
                {
                    return _OrgName_Not_Valid;
                }
            }
            set
            {
                _OrgName_Not_Valid = value;
                if (GetErrors(nameof(OrgName)) != null)
                {
                    _OrgName.Set(_OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }
        private IDataLoadEngine _OrgName;
        private string _OrgName_Not_Valid = "";
        //OrgName property

        //DocumentNameNumber property
        [Attributes.FormVisual("Наименование и номер докумета о признании")]
        public string DocumentNameNumber
        {
            get
            {
                if (GetErrors(nameof(DocumentNameNumber)) != null)
                {
                    return (string)_DocumentNameNumber.Get();
                }
                else
                {
                    return _DocumentNameNumber_Not_Valid;
                }
            }
            set
            {
                _DocumentNameNumber_Not_Valid = value;
                if (GetErrors(nameof(DocumentNameNumber)) != null)
                {
                    _DocumentNameNumber.Set(_DocumentNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNameNumber));
            }
        }
        private IDataLoadEngine _DocumentNameNumber;
        private string _DocumentNameNumber_Not_Valid = "";
        //DocumentNameNumber property

        //PermissionNameNumber property
        [Attributes.FormVisual("Наименование и номер разрешительного докумета")]
        public string PermissionNameNumber
        {
            get
            {
                if (GetErrors(nameof(PermissionNameNumber)) != null)
                {
                    return (string)_PermissionNameNumber.Get();
                }
                else
                {
                    return _PermissionNameNumber_Not_Valid;
                }
            }
            set
            {
                _PermissionNameNumber_Not_Valid = value;
                if (GetErrors(nameof(PermissionNameNumber)) != null)
                {
                    _PermissionNameNumber.Set(_PermissionNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNameNumber));
            }
        }
        private IDataLoadEngine _PermissionNameNumber;
        private string _PermissionNameNumber_Not_Valid = "";
        //PermissionNameNumber property

        //AllowedActivity property
        [Attributes.FormVisual("Разрешенный вид деятельности")]
        public string AllowedActivity
        {
            get
            {
                if (GetErrors(nameof(AllowedActivity)) != null)
                {
                    return (string)_AllowedActivity.Get();
                }
                else
                {
                    return _AllowedActivity_Not_Valid;
                }
            }
            set
            {
                _AllowedActivity_Not_Valid = value;
                if (GetErrors(nameof(AllowedActivity)) != null)
                {
                    _AllowedActivity.Set(_AllowedActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }
        private IDataLoadEngine _AllowedActivity;
        private string _AllowedActivity_Not_Valid = "";
        private void AllowedActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(AllowedActivity));
        }
        //AllowedActivity property

        //Note property
        [Attributes.FormVisual("Примечание")]
        public string Note
        {
            get
            {
                if (GetErrors(nameof(Note)) != null)
                {
                    return (string)_Note.Get();
                }
                else
                {
                    return _Note_Not_Valid;
                }
            }
            set
            {
                _Note_Not_Valid = value;
                if (GetErrors(nameof(Note)) != null)
                {
                    _Note.Set(_Note_Not_Valid);
                }
                OnPropertyChanged(nameof(Note));
            }
        }
        private IDataLoadEngine _Note;
        private string _Note_Not_Valid = "";
        //Note property
    }
}
//if (isSQL)
//{
//    _Note = new SQLite("Note", FormNum, 0);
//    _PermissionNameNumber = new SQLite("PermissionNameNumber", FormNum, 0);
//    _DocumentNameNumber = new SQLite("DocumentNameNumber", FormNum, 0);
//    _NameIOU = new SQLite("NameIOU", FormNum, 0);
//    _TypeOfAccountedParts = new SQLite("TypeOfAccountedParts", FormNum, 0);
//    _KindOri = new SQLite("KindOri", FormNum, 0);
//    _Authority1 = new SQLite("Authority1", FormNum, 0);
//    _NumberInOrder = new SQLite("NumberInOrder", FormNum, 0);
//    _LicenseInfo = new SQLite("LicenseInfo", FormNum, 0);
//    _QuantityOfFormsInv = new SQLite("QuantityOfFormsInv", FormNum, 0);
//    _QuantityOfFormsOper = new SQLite("QuantityOfFormsOper", FormNum, 0);
//    _QuantityOfFormsYear = new SQLite("QuantityOfFormsYear", FormNum, 0);
//    _Notes = new SQLite("Notes", FormNum, 0);
//    _Year = new SQLite("Year", FormNum, 0);
//    _SubjectAuthorityName = new SQLite("SubjectAuthorityName", FormNum, 0);
//    _ShortSubjectAuthorityName = new SQLite("ShortSubjectAuthorityName", FormNum, 0);
//    _FactAddress = new SQLite("FactAddress", FormNum, 0);
//    _GradeFIOchef = new SQLite("GradeFIOchef", FormNum, 0);
//    _GradeFIOresponsibleExecutor = new SQLite("GradeFIOresponsibleExecutor", FormNum, 0);
//    _Telephone1 = new SQLite("Telephone1", FormNum, 0);
//    _Fax1 = new SQLite("Fax1", FormNum, 0);
//    _Email1 = new SQLite("Email1", FormNum, 0);
//    _OrgName = new SQLite("OrgName", FormNum, 0);
//    _ShortOrgName = new SQLite("ShortOrgName", FormNum, 0);
//    _FactAddress1 = new SQLite("FactAddress1", FormNum, 0);
//    _GradeFIOchef1 = new SQLite("GradeFIOchef1", FormNum, 0);
//    _GradeFIOresponsibleExecutor1 = new SQLite("GradeFIOresponsibleExecutor1", FormNum, 0);
//    _IdName = new SQLite("IdName", FormNum, 0);
//    _Value = new SQLite("Value", FormNum, 0);
//    _DepletedUraniumMass = new SQLite("DepletedUraniumMass", FormNum, 0);
//    _CreationYear = new SQLite("CreationYear", FormNum, 0);
//    _Id = new SQLite("Id", FormNum, 0);
//    _CertificateId = new SQLite("CertificateId", FormNum, 0);
//    _NuclearMaterialPresence = new SQLite("NuclearMaterialPresence", FormNum, 0);
//    _Kategory = new SQLite("Kategory", FormNum, 0);
//    _ActivityOnCreation = new SQLite("ActivityOnCreation", FormNum, 0);
//    _UniqueAgreementId = new SQLite("UniqueAgreementId", FormNum, 0);
//    _SupplyDate = new SQLite("SupplyDate", FormNum, 0);
//    _FieldsOfWorking = new SQLite("FieldsOfWorking", FormNum, 0);
//    _LicenseIdRv = new SQLite("LicenseIdRv", FormNum, 0);
//    _ValidThruRv = new SQLite("ValidThruRv", FormNum, 0);
//    _LicenseIdRao = new SQLite("LicenseIdRao", FormNum, 0);
//    _ValidThruRao = new SQLite("ValidThruRao", FormNum, 0);
//    _SupplyAddress = new SQLite("SupplyAddress", FormNum, 0);
//    _RecieverName = new SQLite("RecieverName", FormNum, 0);
//    _RecieverAddress = new SQLite("RecieverAddress", FormNum, 0);
//    _RecieverFactAddress = new SQLite("RecieverFactAddress", FormNum, 0);
//    _LicenseId = new SQLite("LicenseId", FormNum, 0);
//    _SuggestedSolutionDate = new SQLite("SuggestedSolutionDate", FormNum, 0);
//    _UserName = new SQLite("UserName", FormNum, 0);
//    _UserAddress = new SQLite("UserAddress", FormNum, 0);
//    _UserFactAddress = new SQLite("UserFactAddress", FormNum, 0);
//    _UserTelephone = new SQLite("UserTelephone", FormNum, 0);
//    _UserFax = new SQLite("UserFax", FormNum, 0);
//    _ZriUsageScope = new SQLite("ZriUsageScope", FormNum, 0);
//    _ContractId = new SQLite("ContractId", FormNum, 0);
//    _ContractDate = new SQLite("ContractDate", FormNum, 0);
//    _CountryCreator = new SQLite("CountryCreator", FormNum, 0);
//    _OperationCode = new SQLite("OperationCode", FormNum, 0);
//    _ObjectTypeCode = new SQLite("ObjectTypeCode", FormNum, 0);
//    _SpecificActivityOfPlot = new SQLite("SpecificActivityOfPlot", FormNum, 0);
//    _SpecificActivityOfLiquidPart = new SQLite("SpecificActivityOfLiquidPart", FormNum, 0);
//    _SpecificActivityOfDensePart = new SQLite("SpecificActivityOfDensePart", FormNum, 0);
//    _IndicatorName = new SQLite("IndicatorName", FormNum, 0);
//    _PlotName = new SQLite("PlotName", FormNum, 0);
//    _PlotKadastrNumber = new SQLite("FcpNumber", FormNum, 0);
//    _PlotCode = new SQLite("PlotCode", FormNum, 0);
//    _InfectedArea = new SQLite("InfectedArea", FormNum, 0);
//    _AvgGammaRaysDosePower = new SQLite("AvgGammaRaysDosePower", FormNum, 0);
//    _MaxGammaRaysDosePower = new SQLite("MaxGammaRaysDosePower", FormNum, 0);
//    _WasteDensityAlpha = new SQLite("WasteDensityAlpha", FormNum, 0);
//    _WasteDensityBeta = new SQLite("WasteDensityBeta", FormNum, 0);
//    _AllowedActivity = new SQLite("AllowedActivity", FormNum, 0);
//    _AllowedActivityNote = new SQLite("AllowedActivityNote", FormNum, 0);
//    _FactedActivity = new SQLite("FactedActivity", FormNum, 0);
//    _FactedActivityNote = new SQLite("FactedActivityNote", FormNum, 0);
//    _WasteSourceName = new SQLite("WasteSourceName", FormNum, 0);
//    _RadionuclidName = new SQLite("RadionuclidName", FormNum, 0);
//    _PermissionNumber1 = new SQLite("PermissionNumber1", FormNum, 0);
//    _PermissionIssueDate1 = new SQLite("PermissionIssueDate1", FormNum, 0);
//    _PermissionDocumentName1 = new SQLite("PermissionDocumentName1", FormNum, 0);
//    _ValidBegin1 = new SQLite("ValidBegin1", FormNum, 0);
//    _ValidThru1 = new SQLite("ValidThru1", FormNum, 0);
//    _PermissionNumber2 = new SQLite("PermissionNumber2", FormNum, 0);
//    _PermissionIssueDate2 = new SQLite("PermissionIssueDate2", FormNum, 0);
//    _ValidBegin2 = new SQLite("ValidBegin2", FormNum, 0);
//    _PermissionDocumentName2 = new SQLite("PermissionDocumentName2", FormNum, 0);
//    _ValidThru2 = new SQLite("ValidThru2", FormNum, 0);
//    _WasteSourceName = new SQLite("WasteSourceName", FormNum, 0);
//    _WasteRecieverName = new SQLite("WasteRecieverName", FormNum, 0);
//    _RecieverTypeCode = new SQLite("RecieverTypeCode", FormNum, 0);
//    _PoolDistrictName = new SQLite("PoolDistrictName", FormNum, 0);
//    _AllowedWasteRemovalVolume = new SQLite("AllowedWasteRemovalVolume", FormNum, 0);
//    _RemovedWasteVolume = new SQLite("RemovedWasteVolume", FormNum, 0);
//    _RemovedWasteVolumeNote = new SQLite("RemovedWasteVolumeNote", FormNum, 0);
//    _PermissionNumber = new SQLite("PermissionNumber", FormNum, 0);
//    _PermissionIssueDate = new SQLite("PermissionIssueDate", FormNum, 0);
//    _PermissionDocumentName = new SQLite("PermissionDocumentName", FormNum, 0);
//    _ValidBegin = new SQLite("ValidBegin", FormNum, 0);
//    _PermissionNumber = new SQLite("PermissionNumber", FormNum, 0);
//    _PermissionIssueDate = new SQLite("PermissionIssueDate", FormNum, 0);
//    _PermissionDocumentName = new SQLite("PermissionDocumentName", FormNum, 0);
//    _ValidBegin = new SQLite("ValidBegin", FormNum, 0);
//    _ValidThru = new SQLite("ValidThru", FormNum, 0);
//    _RadionuclidNameNote = new SQLite("RadionuclidNameNote", FormNum, 0);
//    _AllowedWasteValue = new SQLite("AllowedWasteValue", FormNum, 0);
//    _AllowedWasteValueNote = new SQLite("AllowedWasteValueNote", FormNum, 0);
//    _FactedWasteValue = new SQLite("FactedWasteValue", FormNum, 0);
//    _FactedWasteValueNote = new SQLite("FactedWasteValueNote", FormNum, 0);
//    _WasteOutbreakPreviousYear = new SQLite("WasteOutbreakPreviousYear", FormNum, 0);
//    _SourcesQuantity = new SQLite("SourcesQuantity", FormNum, 0);
//    _ObservedSourceNumber = new SQLite("ObservedSourceNumber", FormNum, 0);
//    _ControlledAreaName = new SQLite("ControlledAreaName", FormNum, 0);
//    _SupposedWasteSource = new SQLite("SupposedWasteSource", FormNum, 0);
//    _DistanceToWasteSource = new SQLite("DistanceToWasteSource", FormNum, 0);
//    _TestDepth = new SQLite("TestDepth", FormNum, 0);
//    _TestDepthNote = new SQLite("TestDepthNote", FormNum, 0);
//    _RadionuclidName = new SQLite("RadionuclidName", FormNum, 0);
//    _AverageYearConcentration = new SQLite("AverageYearConcentration", FormNum, 0);
//    _CodeOYATnote = new SQLite("CodeOYATnote", FormNum, 0);
//    _MassCreated = new SQLite("MassCreated", FormNum, 0);
//    _QuantityCreated = new SQLite("QuantityCreated", FormNum, 0);
//    _QuantityCreatedNote = new SQLite("QuantityCreatedNote", FormNum, 0);
//    _MassFromAnothers = new SQLite("MassFromAnothers", FormNum, 0);
//    _QuantityFromAnothers = new SQLite("QuantityFromAnothers", FormNum, 0);
//    _QuantityFromAnothersNote = new SQLite("QuantityFromAnothersNote", FormNum, 0);
//    _MassFromAnothersImported = new SQLite("MassFromAnothersImported", FormNum, 0);
//    _QuantityFromAnothersImported = new SQLite("QuantityFromAnothersImported", FormNum, 0);
//    _QuantityFromAnothersImportedNote = new SQLite("QuantityFromAnothersImportedNote", FormNum, 0);
//    _MassAnotherReasons = new SQLite("MassAnotherReasons", FormNum, 0);
//    _QuantityAnotherReasons = new SQLite("QuantityAnotherReasons", FormNum, 0);
//    _QuantityAnotherReasonsNote = new SQLite("QuantityAnotherReasonsNote", FormNum, 0);
//    _MassRefined = new SQLite("MassRefined", FormNum, 0);
//    _MassTransferredToAnother = new SQLite("MassTransferredToAnother", FormNum, 0);
//    _QuantityTransferredToAnother = new SQLite("QuantityTransferredToAnother", FormNum, 0);
//    _QuantityTransferredToAnotherNote = new SQLite("CodeOYAT", FormNum, 0);
//    _QuantityRefined = new SQLite("QuantityRefined", FormNum, 0);
//    _QuantityRefinedNote = new SQLite("QuantityRefinedNote", FormNum, 0);
//    _MassRemovedFromAccount = new SQLite("MassRemovedFromAccount", FormNum, 0);
//    _QuantityRemovedFromAccount = new SQLite("QuantityRemovedFromAccount", FormNum, 0);
//    _QuantityRemovedFromAccountNote = new SQLite("QuantityRemovedFromAccountNote", FormNum, 0);
//    _CodeOYAT = new SQLite("CodeOYAT", FormNum, 0);
//    _DocumentDate = new SQLite("DocumentDate", FormNum, 0);
//    _DocumentName = new SQLite("DocumentName", FormNum, 0);
//    _DocumentNumber = new SQLite("DocumentNumber", FormNum, 0);
//    _DocumentNumberRecoded = new SQLite("DocumentNumberRecoded", FormNum, 0);
//    _ExpirationDate = new SQLite("ExpirationDate", FormNum, 0);
//    _ProjectVolume = new SQLite("ProjectVolume", FormNum, 0);
//    _ProjectVolumeNote = new SQLite("ProjectVolumeNote", FormNum, 0);
//    _SummaryActivity = new SQLite("SummaryActivity", FormNum, 0);
//    _QuantityOZIII = new SQLite("QuantityOZIII", FormNum, 0);
//    _PackQuantity = new SQLite("PackQuantity", FormNum, 0);
//    _MassInPack = new SQLite("MassInPack", FormNum, 0);
//    _VolumeInPack = new SQLite("VolumeInPack", FormNum, 0);
//    _RefineMachineName = new SQLite("RefineMachineName", FormNum, 0);
//    _MachineCode = new SQLite("MachineCode", FormNum, 0);
//    _MachinePower = new SQLite("MachinePower", FormNum, 0);
//    _NumberOfHoursPerYear = new SQLite("NumberOfHoursPerYear", FormNum, 0);
//    _CodeRAOIn = new SQLite("CodeRAOIn", FormNum, 0);
//    _StatusRAOIn = new SQLite("StatusRAOIn", FormNum, 0);
//    _VolumeIn = new SQLite("VolumeIn", FormNum, 0);
//    _MassIn = new SQLite("MassIn", FormNum, 0);
//    _QuantityIn = new SQLite("QuantityIn", FormNum, 0);
//    _CodeRAOout = new SQLite("CodeRAOout", FormNum, 0);
//    _StatusRAOout = new SQLite("StatusRAOout", FormNum, 0);
//    _VolumeOut = new SQLite("VolumeOut", FormNum, 0);
//    _MassOut = new SQLite("MassOut", FormNum, 0);
//    _TritiumActivityIn = new SQLite("TritiumActivityIn", FormNum, 0);
//    _TritiumActivityOut = new SQLite("TritiumActivityOut", FormNum, 0);
//    _QuantityOZIIIout = new SQLite("QuantityOZIIIout", FormNum, 0);
//    _TransuraniumActivityIn = new SQLite("TransuraniumActivityIn", FormNum, 0);
//    _TransuraniumActivityOut = new SQLite("TransuraniumActivityOut", FormNum, 0);
//    _BetaGammaActivityIn = new SQLite("BetaGammaActivityIn", FormNum, 0);
//    _AlphaActivityIn = new SQLite("AlphaActivityIn", FormNum, 0);
//    _BetaGammaActivityOut = new SQLite("BetaGammaActivityOut", FormNum, 0);
//    _AlphaActivityOut = new SQLite("AlphaActivityOut", FormNum, 0);
//    _RegNo = new SQLite("RegNo", FormNum, 0);
//    _OrganUprav = new SQLite("OrganUprav", FormNum, 0);
//    _SubjectRF = new SQLite("SubjectRF", FormNum, 0);
//    _JurLico = new SQLite("JurLico", FormNum, 0);
//    _ShortJurLico = new SQLite("ShortJurLico", FormNum, 0);
//    _JurLicoAddress = new SQLite("JurLicoAddress", FormNum, 0);
//    _JurLicoFactAddress = new SQLite("JurLicoFactAddress", FormNum, 0);
//    _GradeFIO = new SQLite("GradeFIO", FormNum, 0);
//    _Telephone = new SQLite("Telephone", FormNum, 0);
//    _Fax = new SQLite("Fax", FormNum, 0);
//    _Email = new SQLite("Email", FormNum, 0);
//    _Okpo = new SQLite("Okpo", FormNum, 0);
//    _Okved = new SQLite("Okved", FormNum, 0);
//    _Okogu = new SQLite("Okogu", FormNum, 0);
//    _Oktmo = new SQLite("Oktmo", FormNum, 0);
//    _Inn = new SQLite("Inn", FormNum, 0);
//    _Kpp = new SQLite("Kpp", FormNum, 0);
//    _Okopf = new SQLite("Okopf", FormNum, 0);
//    _Okfs = new SQLite("Okfs", FormNum, 0);
//    _Volume20 = new SQLite("Volume20", FormNum, 0);
//    _Volume6 = new SQLite("Volume6", FormNum, 0);
//    _SaltConcentration = new SQLite("SaltConcentration", FormNum, 0);
//    _SpecificActivity = new SQLite("SpecificActivity", FormNum, 0);
//    _Mass21 = new SQLite("Mass21", FormNum, 0);
//    _Mass7 = new SQLite("Mass7", FormNum, 0);
//    _IndividualNumberZHRO = new SQLite("IndividualNumberZHRO", FormNum, 0);
//    _IndividualNumberZHROrecoded = new SQLite("IndividualNumberZHRORecoded", FormNum, 0);
//    _VolumeOutOfPack = new SQLite("VolumeOutOfPack", FormNum, 0);
//    _PackFactoryNumber = new SQLite("PackFactoryNumber", FormNum, 0);
//    _MassOutOfPack = new SQLite("MassOutOfPack", FormNum, 0);
//    _FormingDate = new SQLite("FormingDate", FormNum, 0);
//    _MainRadionuclids = new SQLite("MainRadionuclids", FormNum, 0);
//    _CodeRAO = new SQLite("CodeRAO", FormNum, 0);
//    _AlphaActivity = new SQLite("AlphaActivity", FormNum, 0);
//    _BetaGammaActivity = new SQLite("BetaGammaActivity", FormNum, 0);
//    _TritiumActivity = new SQLite("TritiumActivity", FormNum, 0);
//    _TransuraniumActivity = new SQLite("TransuraniumActivity", FormNum, 0);
//    _StoragePlaceCode = new SQLite("StoragePlaceCode", FormNum, 0);
//    _StoragePlaceName = new SQLite("StoragePlaceName", FormNum, 0);
//    _Subsidy = new SQLite("Subsidy", FormNum, 0);
//    _StoragePlaceNameNote = new SQLite("StoragePlaceNameNote", FormNum, 0);
//    _StatusRAO = new SQLite("StatusRAO", FormNum, 0);
//    _RefineOrSortRAOCode = new SQLite("RefineOrSortRAOCode", FormNum, 0);
//    _FcpNumber = new SQLite("FcpNumber", FormNum, 0);
//    _Volume = new SQLite("Volume", FormNum, 0);
//    _Mass = new SQLite("Mass", FormNum, 0);
//    _ActivityMeasurementDate = new SQLite("ActivityMeasurementDate", FormNum, 0);
//    _Sort = new SQLite("Sort", FormNum, 0);
//    _Name = new SQLite("Name", FormNum, 0);
//    _AggregateState = new SQLite("AggregateState", FormNum, 0);
//    _PassportNumber = new SQLite("PassportNumber", FormNum, 0);
//    _PassportNumberRecoded = new SQLite("PassportNumberRecoded", FormNum, 0);
//    _PassportNumberNote = new SQLite("PassportNumberNote", FormNum, 0);
//    _Type = new SQLite("Type", FormNum, 0);
//    _TypeRecoded = new SQLite("TypeRecoded", FormNum, 0);
//    _Radionuclids = new SQLite("Radionuclids", FormNum, 0);
//    _FactoryNumber = new SQLite("FactoryNumber", FormNum, 0);
//    _FactoryNumberRecoded = new SQLite("FactoryNumberRecoded", FormNum, 0);
//    _Quantity = new SQLite("Quantity", FormNum, 0);
//    _Activity = new SQLite("Activity", FormNum, 0);
//    _ActivityNote = new SQLite("ActivityNote", FormNum, 0);
//    _CreationDate = new SQLite("CreationDate", FormNum, 0);
//    _CreatorOKPO = new SQLite("CreatorOKPO", FormNum, 0);
//    _CreatorOKPONote = new SQLite("CreatorOKPONote", FormNum, 0);
//    _Category = new SQLite("Category", FormNum, 0);
//    _SignedServicePeriod = new SQLite("SignedServicePeriod", FormNum, 0);
//    _PropertyCode = new SQLite("PropertyCode", FormNum, 0);
//    _Owner = new SQLite("Owner", FormNum, 0);
//    _ProviderOrRecieverOKPO = new SQLite("ProviderOrRecieverOKPO", FormNum, 0);
//    _ProviderOrRecieverOKPONote = new SQLite("ProviderOrRecieverOKPONote", FormNum, 0);
//    _TransporterOKPO = new SQLite("TransporterOKPO", FormNum, 0);
//    _TransporterOKPONote = new SQLite("TransporterOKPONote", FormNum, 0);
//    _PackName = new SQLite("PackName", FormNum, 0);
//    _PackNameNote = new SQLite("PackNameNote", FormNum, 0);
//    _PackType = new SQLite("PackType", FormNum, 0);
//    _PackTypeRecoded = new SQLite("PackTypeRecoded", FormNum, 0);
//    _PackTypeNote = new SQLite("PackTypeNote", FormNum, 0);
//    _PackNumber = new SQLite("PackNumber", FormNum, 0);
//    _PackNumberRecoded = new SQLite("PackNumberRecoded", FormNum, 0);
//}
//else
//{
//    _Note = new File();
//    _PermissionNameNumber = new File();
//    _DocumentNameNumber = new File();
//    _NameIOU = new File();
//    _TypeOfAccountedParts = new File();
//    _KindOri = new File();
//    _Authority1 = new File();
//    _NumberInOrder = new File();
//    _LicenseInfo = new File();
//    _QuantityOfFormsInv = new File();
//    _QuantityOfFormsOper = new File();
//    _QuantityOfFormsYear = new File();
//    _Notes = new File();
//    _Year = new File();
//    _SubjectAuthorityName = new File();
//    _ShortSubjectAuthorityName = new File();
//    _FactAddress = new File();
//    _GradeFIOchef = new File();
//    _GradeFIOresponsibleExecutor = new File();
//    _Telephone1 = new File();
//    _Fax1 = new File();
//    _Email1 = new File();
//    _OrgName = new File();
//    _ShortOrgName = new File();
//    _FactAddress1 = new File();
//    _GradeFIOchef1 = new File();
//    _GradeFIOresponsibleExecutor1 = new File();
//    _IdName = new File();
//    _Value = new File();
//    _DepletedUraniumMass = new File();
//    _CreationYear = new File();
//    _Id = new File();
//    _CertificateId = new File();
//    _NuclearMaterialPresence = new File();
//    _Kategory = new File();
//    _ActivityOnCreation = new File();
//    _UniqueAgreementId = new File();
//    _SupplyDate = new File();
//    _FieldsOfWorking = new File();
//    _LicenseIdRv = new File();
//    _ValidThruRv = new File();
//    _LicenseIdRao = new File();
//    _ValidThruRao = new File();
//    _SupplyAddress = new File();
//    _RecieverName = new File();
//    _RecieverAddress = new File();
//    _RecieverFactAddress = new File();
//    _LicenseId = new File();
//    _SuggestedSolutionDate = new File();
//    _UserName = new File();
//    _UserAddress = new File();
//    _UserFactAddress = new File();
//    _UserTelephone = new File();
//    _UserFax = new File();
//    _ZriUsageScope = new File();
//    _ContractId = new File();
//    _ContractDate = new File();
//    _CountryCreator = new File();
//    _OperationCode = new File();
//    _ObjectTypeCode = new File();
//    _SpecificActivityOfPlot = new File();
//    _SpecificActivityOfLiquidPart = new File();
//    _SpecificActivityOfDensePart = new File();
//    _IndicatorName = new File();
//    _PlotName = new File();
//    _PlotKadastrNumber = new File();
//    _PlotCode = new File();
//    _InfectedArea = new File();
//    _AvgGammaRaysDosePower = new File();
//    _MaxGammaRaysDosePower = new File();
//    _WasteDensityAlpha = new File();
//    _WasteDensityBeta = new File();
//    _FcpNumber = new File();
//    _AllowedActivity = new File();
//    _AllowedActivityNote = new File();
//    _FactedActivity = new File();
//    _FactedActivityNote = new File();
//    _WasteSourceName = new File();
//    _RadionuclidName = new File();
//    _PermissionNumber1 = new File();
//    _PermissionIssueDate1 = new File();
//    _PermissionDocumentName1 = new File();
//    _ValidBegin1 = new File();
//    _ValidThru1 = new File();
//    _PermissionNumber2 = new File();
//    _PermissionIssueDate2 = new File();
//    _ValidBegin2 = new File();
//    _PermissionDocumentName2 = new File();
//    _ValidThru2 = new File();
//    _WasteSourceName = new File();
//    _WasteRecieverName = new File();
//    _RecieverTypeCode = new File();
//    _PoolDistrictName = new File();
//    _AllowedWasteRemovalVolume = new File();
//    _RemovedWasteVolume = new File();
//    _RemovedWasteVolumeNote = new File();
//    _PermissionNumber = new File();
//    _PermissionIssueDate = new File();
//    _PermissionDocumentName = new File();
//    _ValidBegin = new File();
//    _PermissionNumber = new File();
//    _PermissionIssueDate = new File();
//    _PermissionDocumentName = new File();
//    _ValidBegin = new File();
//    _ValidThru = new File();
//    _RadionuclidNameNote = new File();
//    _AllowedWasteValue = new File();
//    _AllowedWasteValueNote = new File();
//    _FactedWasteValue = new File();
//    _FactedWasteValueNote = new File();
//    _WasteOutbreakPreviousYear = new File();
//    _SourcesQuantity = new File();
//    _ObservedSourceNumber = new File();
//    _ControlledAreaName = new File();
//    _SupposedWasteSource = new File();
//    _DistanceToWasteSource = new File();
//    _TestDepth = new File();
//    _TestDepthNote = new File();
//    _RadionuclidName = new File();
//    _AverageYearConcentration = new File();
//    _CodeOYATnote = new File();
//    _MassCreated = new File();
//    _QuantityCreated = new File();
//    _QuantityCreatedNote = new File();
//    _MassFromAnothers = new File();
//    _QuantityFromAnothers = new File();
//    _QuantityFromAnothersNote = new File();
//    _MassFromAnothersImported = new File();
//    _QuantityFromAnothersImported = new File();
//    _QuantityFromAnothersImportedNote = new File();
//    _MassAnotherReasons = new File();
//    _QuantityAnotherReasons = new File();
//    _QuantityAnotherReasonsNote = new File();
//    _MassRefined = new File();
//    _MassTransferredToAnother = new File();
//    _QuantityTransferredToAnother = new File();
//    _QuantityTransferredToAnotherNote = new File();
//    _QuantityRefined = new File();
//    _QuantityRefinedNote = new File();
//    _MassRemovedFromAccount = new File();
//    _QuantityRemovedFromAccount = new File();
//    _QuantityRemovedFromAccountNote = new File();
//    _CodeOYAT = new File();
//    _DocumentDate = new File();
//    _DocumentName = new File();
//    _DocumentNumber = new File();
//    _DocumentNumberRecoded = new File();
//    _ExpirationDate = new File();
//    _ProjectVolume = new File();
//    _ProjectVolumeNote = new File();
//    _SummaryActivity = new File();
//    _QuantityOZIII = new File();
//    _PackQuantity = new File();
//    _MassInPack = new File();
//    _VolumeInPack = new File();
//    _RefineMachineName = new File();
//    _MachineCode = new File();
//    _MachinePower = new File();
//    _NumberOfHoursPerYear = new File();
//    _CodeRAOIn = new File();
//    _StatusRAOIn = new File();
//    _VolumeIn = new File();
//    _MassIn = new File();
//    _QuantityIn = new File();
//    _CodeRAOout = new File();
//    _StatusRAOout = new File();
//    _VolumeOut = new File();
//    _MassOut = new File();
//    _TritiumActivityIn = new File();
//    _TritiumActivityOut = new File();
//    _QuantityOZIIIout = new File();
//    _TransuraniumActivityIn = new File();
//    _TransuraniumActivityOut = new File();
//    _BetaGammaActivityIn = new File();
//    _BetaGammaActivityOut = new File();
//    _AlphaActivityIn = new File();
//    _AlphaActivityOut = new File();
//    _RegNo = new File();
//    _OrganUprav = new File();
//    _SubjectRF = new File();
//    _JurLico = new File();
//    _ShortJurLico = new File();
//    _JurLicoAddress = new File();
//    _JurLicoFactAddress = new File();
//    _GradeFIO = new File();
//    _Telephone = new File();
//    _Fax = new File();
//    _Email = new File();
//    _Okpo = new File();
//    _Okved = new File();
//    _Okogu = new File();
//    _Oktmo = new File();
//    _Inn = new File();
//    _Kpp = new File();
//    _Okopf = new File();
//    _Okfs = new File();
//    _Volume20 = new File();
//    _Volume6 = new File();
//    _SaltConcentration = new File();
//    _SpecificActivity = new File();
//    _Mass21 = new File();
//    _Mass7 = new File();
//    _IndividualNumberZHRO = new File();
//    _IndividualNumberZHROrecoded = new File();
//    _VolumeOutOfPack = new File();
//    _PackFactoryNumber = new File();
//    _MassOutOfPack = new File();
//    _FormingDate = new File();
//    _MainRadionuclids = new File();
//    _CodeRAO = new File();
//    _AlphaActivity = new File();
//    _BetaGammaActivity = new File();
//    _TritiumActivity = new File();
//    _TransuraniumActivity = new File();
//    _StoragePlaceCode = new File();
//    _StoragePlaceName = new File();
//    _Subsidy = new File();
//    _StoragePlaceNameNote = new File();
//    _StatusRAO = new File();
//    _RefineOrSortRAOCode = new File();
//    _FcpNumber = new File();
//    _Volume = new File();
//    _Mass = new File();
//    _ActivityMeasurementDate = new File();
//    _Sort = new File();
//    _Name = new File();
//    _AggregateState = new File();
//    _PassportNumber = new File();
//    _PassportNumberRecoded = new File();
//    _PassportNumberNote = new File();
//    _Type = new File();
//    _TypeRecoded = new File();
//    _Radionuclids = new File();
//    _FactoryNumber = new File();
//    _FactoryNumberRecoded = new File();
//    _Quantity = new File();
//    _Activity = new File();
//    _ActivityNote = new File();
//    _CreationDate = new File();
//    _CreatorOKPO = new File();
//    _CreatorOKPONote = new File();
//    _Category = new File();
//    _SignedServicePeriod = new File();
//    _PropertyCode = new File();
//    _Owner = new File();
//    _ProviderOrRecieverOKPO = new File();
//    _ProviderOrRecieverOKPONote = new File();
//    _TransporterOKPO = new File();
//    _TransporterOKPONote = new File();
//    _PackName = new File();
//    _PackNameNote = new File();
//    _PackType = new File();
//    _PackTypeRecoded = new File();
//    _PackTypeNote = new File();
//    _PackNumber = new File();
//    _PackNumberRecoded = new File();
//}
