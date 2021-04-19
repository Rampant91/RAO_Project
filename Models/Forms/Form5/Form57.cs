using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.7: Перечень подведомственных организаций ведомственного информационно-аналитического центра федерального органа исполнительной власти")]
    public class Form57: Abstracts.Form5
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            string doubleNotNullDeclaration = " float(53) not null, ";
            return
                Abstracts.Form5.SQLCommandParamsBase() +
            nameof(Note) + strNotNullDeclaration +
            nameof(PermissionNameNumber) + strNotNullDeclaration +
            nameof(DocumentNameNumber) + strNotNullDeclaration +
            nameof(OrgName) + strNotNullDeclaration +
            nameof(AllowedActivity) + strNotNullDeclaration +
            nameof(RegNo) + strNotNullDeclaration +
            nameof(Okpo) + " varchar(255) not null";
        }
        public Form57(IDataAccess Access) : base(Access)
        {
            FormNum = "57";
            NumberOfFields = 9;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //RegNo property
        [Attributes.Form_Property("Регистрационный номер")]
        public string RegNo
        {
            get
            {
                if (GetErrors(nameof(RegNo)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RegNo));
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
                    _dataAccess.Set(nameof(RegNo), _RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }
        
        private string _RegNo_Not_Valid = "";
        //RegNo property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public string Okpo
        {
            get
            {
                if (GetErrors(nameof(Okpo)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Okpo));
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
                    _dataAccess.Set(nameof(Okpo), _Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        
        private string _Okpo_Not_Valid = "";
        //Okpo property

        //OrgName property
        [Attributes.Form_Property("Наименование организации")]
        public string OrgName
        {
            get
            {
                if (GetErrors(nameof(OrgName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(OrgName));
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
                    _dataAccess.Set(nameof(OrgName), _OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }
        
        private string _OrgName_Not_Valid = "";
        //OrgName property

        //DocumentNameNumber property
        [Attributes.Form_Property("Наименование и номер докумета о признании")]
        public string DocumentNameNumber
        {
            get
            {
                if (GetErrors(nameof(DocumentNameNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(DocumentNameNumber));
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
                    _dataAccess.Set(nameof(DocumentNameNumber), _DocumentNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(DocumentNameNumber));
            }
        }
        
        private string _DocumentNameNumber_Not_Valid = "";
        //DocumentNameNumber property

        //PermissionNameNumber property
        [Attributes.Form_Property("Наименование и номер разрешительного докумета")]
        public string PermissionNameNumber
        {
            get
            {
                if (GetErrors(nameof(PermissionNameNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PermissionNameNumber));
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
                    _dataAccess.Set(nameof(PermissionNameNumber), _PermissionNameNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PermissionNameNumber));
            }
        }
        
        private string _PermissionNameNumber_Not_Valid = "";
        //PermissionNameNumber property

        //AllowedActivity property
        [Attributes.Form_Property("Разрешенный вид деятельности")]
        public string AllowedActivity
        {
            get
            {
                if (GetErrors(nameof(AllowedActivity)) != null)
                {
                    return (string)_dataAccess.Get(nameof(AllowedActivity));
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
                    _dataAccess.Set(nameof(AllowedActivity), _AllowedActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }
        
        private string _AllowedActivity_Not_Valid = "";
        private void AllowedActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(AllowedActivity));
        }
        //AllowedActivity property

        //Note property
        [Attributes.Form_Property("Примечание")]
        public string Note
        {
            get
            {
                if (GetErrors(nameof(Note)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Note));
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
                    _dataAccess.Set(nameof(Note), _Note_Not_Valid);
                }
                OnPropertyChanged(nameof(Note));
            }
        }
        
        private string _Note_Not_Valid = "";
        //Note property
    }
}
//if (isSQL)
//{
//public static string SQLCommandParams()
//{
//    string strNotNullDeclaration = " varchar(255) not null, ";
//    string intNotNullDeclaration = " int not null, ";
//    string shortNotNullDeclaration = " smallint not null, ";
//    string byteNotNullDeclaration = " tinyint not null, ";
//    string dateNotNullDeclaration = " ????, ";
//    string doubleNotNullDeclaration = " float(53) not null, ";
//    return
//        Abstracts.Form1.SQLCommandParamsBase() +
//    nameof(Note) + strNotNullDeclaration +
//    nameof(PermissionNameNumber) + strNotNullDeclaration +
//    nameof(DocumentNameNumber) + strNotNullDeclaration +
//    nameof(NameIOU) + strNotNullDeclaration +
//    nameof(TypeOfAccountedParts) + intNotNullDeclaration +
//    nameof(KindOri) + intNotNullDeclaration +
//    nameof(Authority1) + strNotNullDeclaration +
//    nameof(NumberInOrder) + intNotNullDeclaration +
//    nameof(LicenseInfo) + strNotNullDeclaration +
//    nameof(QuantityOfFormsInv) + intNotNullDeclaration +
//    nameof(QuantityOfFormsOper) + intNotNullDeclaration +
//    nameof(QuantityOfFormsYear) + intNotNullDeclaration +
//    nameof(Notes) + strNotNullDeclaration +
//    nameof(Year) + strNotNullDeclaration +
//    nameof(SubjectAuthorityName) + strNotNullDeclaration +
//    nameof(ShortSubjectAuthorityName) + strNotNullDeclaration +
//    nameof(FactAddress) + strNotNullDeclaration +
//    nameof(GradeFIOchef) + strNotNullDeclaration +
//    nameof(GradeFIOresponsibleExecutor) + strNotNullDeclaration +
//    nameof(Telephone1) + strNotNullDeclaration +
//    nameof(Fax1) + strNotNullDeclaration +
//    nameof(Email1) + strNotNullDeclaration +
//    nameof(OrgName) + strNotNullDeclaration +
//    nameof(ShortOrgName) + strNotNullDeclaration +
//    nameof(FactAddress1) + strNotNullDeclaration +
//    nameof(GradeFIOchef1) + strNotNullDeclaration +
//    nameof(GradeFIOresponsibleExecutor1) + strNotNullDeclaration +
//    nameof(IdName) + strNotNullDeclaration +
//    nameof(Value) + strNotNullDeclaration +
//    nameof(DepletedUraniumMass) + doubleNotNullDeclaration +
//    nameof(CreationYear) + strNotNullDeclaration +
//    nameof(Id) + strNotNullDeclaration +
//    nameof(CertificateId) + strNotNullDeclaration +
//    nameof(NuclearMaterialPresence) + strNotNullDeclaration +
//    nameof(Kategory) + shortNotNullDeclaration +
//    nameof(ActivityOnCreation) + strNotNullDeclaration +
//    nameof(UniqueAgreementId) + strNotNullDeclaration +
//    nameof(SupplyDate) + dateNotNullDeclaration +
//    nameof(FieldsOfWorking) + strNotNullDeclaration +
//    nameof(LicenseIdRv) + strNotNullDeclaration +
//    nameof(ValidThruRv) + dateNotNullDeclaration +
//    nameof(LicenseIdRao) + strNotNullDeclaration +
//    nameof(ValidThruRao) + dateNotNullDeclaration +
//    nameof(SupplyAddress) + strNotNullDeclaration +
//    nameof(RecieverName) + strNotNullDeclaration +
//    nameof(RecieverAddress) + strNotNullDeclaration +
//    nameof(RecieverFactAddress) + strNotNullDeclaration +
//    nameof(LicenseId) + strNotNullDeclaration +
//    nameof(SuggestedSolutionDate) + dateNotNullDeclaration +
//    nameof(UserName) + strNotNullDeclaration +
//    nameof(UserAddress) + strNotNullDeclaration +
//    nameof(UserFactAddress) + strNotNullDeclaration +
//    nameof(UserTelephone) + strNotNullDeclaration +
//    nameof(UserFax) + strNotNullDeclaration +
//    nameof(ZriUsageScope) + strNotNullDeclaration +
//    nameof(ContractId) + strNotNullDeclaration +
//    nameof(ContractDate) + dateNotNullDeclaration +
//    nameof(CountryCreator) + strNotNullDeclaration +
//    nameof(OperationCode) + strNotNullDeclaration +
//    nameof(ObjectTypeCode) + strNotNullDeclaration +
//    nameof(SpecificActivityOfPlot) + strNotNullDeclaration +
//    nameof(SpecificActivityOfLiquidPart) + strNotNullDeclaration +
//    nameof(SpecificActivityOfDensePart) + strNotNullDeclaration +
//    nameof(IndicatorName) + strNotNullDeclaration +
//    nameof(PlotName) + strNotNullDeclaration +
//    nameof(PlotKadastrNumber) + strNotNullDeclaration +
//    nameof(PlotCode) + strNotNullDeclaration +
//    nameof(InfectedArea) + strNotNullDeclaration +
//    nameof(AvgGammaRaysDosePower) + strNotNullDeclaration +
//    nameof(MaxGammaRaysDosePower) + strNotNullDeclaration +
//    nameof(WasteDensityAlpha) + strNotNullDeclaration +
//    nameof(WasteDensityBeta) + strNotNullDeclaration +
//    nameof(AllowedActivity) + strNotNullDeclaration +
//    nameof(AllowedActivityNote) + strNotNullDeclaration +
//    nameof(FactedActivity) + strNotNullDeclaration +
//    nameof(FactedActivityNote) + strNotNullDeclaration +
//    nameof(PermissionNumber1) + strNotNullDeclaration +
//    nameof(PermissionIssueDate1) + dateNotNullDeclaration +
//    nameof(PermissionDocumentName1) + strNotNullDeclaration +
//    nameof(ValidBegin1) + dateNotNullDeclaration +
//    nameof(ValidThru1) + dateNotNullDeclaration +
//    nameof(PermissionNumber2) + strNotNullDeclaration +
//    nameof(PermissionIssueDate2) + dateNotNullDeclaration +
//    nameof(ValidBegin2) + dateNotNullDeclaration +
//    nameof(PermissionDocumentName2) + strNotNullDeclaration +
//    nameof(ValidThru2) + dateNotNullDeclaration +
//    nameof(WasteSourceName) + strNotNullDeclaration +
//    nameof(WasteRecieverName) + strNotNullDeclaration +
//    nameof(RecieverTypeCode) + strNotNullDeclaration +
//    nameof(PoolDistrictName) + strNotNullDeclaration +
//    nameof(AllowedWasteRemovalVolume) + doubleNotNullDeclaration +
//    nameof(RemovedWasteVolume) + doubleNotNullDeclaration +
//    nameof(RemovedWasteVolumeNote) + doubleNotNullDeclaration +
//    nameof(PermissionNumber) + strNotNullDeclaration +
//    nameof(PermissionIssueDate) + strNotNullDeclaration +
//    nameof(PermissionDocumentName) + strNotNullDeclaration +
//    nameof(ValidBegin) + dateNotNullDeclaration +
//    nameof(ValidThru) + dateNotNullDeclaration +
//    nameof(RadionuclidNameNote) + strNotNullDeclaration +
//    nameof(AllowedWasteValue) + strNotNullDeclaration +
//    nameof(AllowedWasteValueNote) + strNotNullDeclaration +
//    nameof(FactedWasteValue) + strNotNullDeclaration +
//    nameof(FactedWasteValueNote) + strNotNullDeclaration +
//    nameof(WasteOutbreakPreviousYear) + strNotNullDeclaration +
//    nameof(SourcesQuantity) + intNotNullDeclaration +
//    nameof(ObservedSourceNumber) + strNotNullDeclaration +
//    nameof(ControlledAreaName) + strNotNullDeclaration +
//    nameof(SupposedWasteSource) + strNotNullDeclaration +
//    nameof(DistanceToWasteSource) + intNotNullDeclaration +
//    nameof(TestDepth) + intNotNullDeclaration +
//    nameof(TestDepthNote) + intNotNullDeclaration +
//    nameof(RadionuclidName) + strNotNullDeclaration +
//    nameof(AverageYearConcentration) + strNotNullDeclaration +
//    nameof(CodeOYATnote) + strNotNullDeclaration +
//    nameof(MassCreated) + doubleNotNullDeclaration +
//    nameof(QuantityCreated) + intNotNullDeclaration +
//    nameof(QuantityCreatedNote) + intNotNullDeclaration +
//    nameof(MassFromAnothers) + doubleNotNullDeclaration +
//    nameof(QuantityFromAnothers) + intNotNullDeclaration +
//    nameof(QuantityFromAnothersNote) + intNotNullDeclaration +
//    nameof(MassFromAnothersImported) + doubleNotNullDeclaration +
//    nameof(QuantityFromAnothersImported) + intNotNullDeclaration +
//    nameof(QuantityFromAnothersImportedNote) + intNotNullDeclaration +
//    nameof(MassAnotherReasons) + doubleNotNullDeclaration +
//    nameof(QuantityAnotherReasons) + intNotNullDeclaration +
//    nameof(QuantityAnotherReasonsNote) + intNotNullDeclaration +
//    nameof(MassRefined) + doubleNotNullDeclaration +
//    nameof(MassTransferredToAnother) + doubleNotNullDeclaration +
//    nameof(QuantityTransferredToAnother) + intNotNullDeclaration +
//    nameof(QuantityTransferredToAnotherNote) + intNotNullDeclaration +
//    nameof(QuantityRefined) + intNotNullDeclaration +
//    nameof(QuantityRefinedNote) + intNotNullDeclaration +
//    nameof(MassRemovedFromAccount) + doubleNotNullDeclaration +
//    nameof(QuantityRemovedFromAccount) + intNotNullDeclaration +
//    nameof(QuantityRemovedFromAccountNote) + intNotNullDeclaration +
//    nameof(CodeOYAT) + strNotNullDeclaration +
//    nameof(DocumentDate) + dateNotNullDeclaration +
//    nameof(DocumentName) + strNotNullDeclaration +
//    nameof(DocumentNumber) + strNotNullDeclaration +
//    nameof(DocumentNumberRecoded) + strNotNullDeclaration +
//    nameof(ExpirationDate) + dateNotNullDeclaration +
//    nameof(ProjectVolume) + doubleNotNullDeclaration +
//    nameof(ProjectVolumeNote) + doubleNotNullDeclaration +
//    nameof(SummaryActivity) + strNotNullDeclaration +
//    nameof(QuantityOZIII) + intNotNullDeclaration +
//    nameof(PackQuantity) + intNotNullDeclaration +
//    nameof(MassInPack) + doubleNotNullDeclaration +
//    nameof(VolumeInPack) + doubleNotNullDeclaration +
//    nameof(RefineMachineName) + strNotNullDeclaration +
//    nameof(MachineCode) + strNotNullDeclaration +
//    nameof(MachinePower) + strNotNullDeclaration +
//    nameof(NumberOfHoursPerYear) + strNotNullDeclaration +
//    nameof(CodeRAOIn) + strNotNullDeclaration +
//    nameof(StatusRAOIn) + strNotNullDeclaration +
//    nameof(VolumeIn) + doubleNotNullDeclaration +
//    nameof(MassIn) + doubleNotNullDeclaration +
//    nameof(QuantityIn) + intNotNullDeclaration +
//    nameof(CodeRAOout) + strNotNullDeclaration +
//    nameof(StatusRAOout) + strNotNullDeclaration +
//    nameof(VolumeOut) + doubleNotNullDeclaration +
//    nameof(MassOut) + doubleNotNullDeclaration +
//    nameof(TritiumActivityIn) + strNotNullDeclaration +
//    nameof(TritiumActivityOut) + strNotNullDeclaration +
//    nameof(QuantityOZIIIout) + intNotNullDeclaration +
//    nameof(TransuraniumActivityIn) + strNotNullDeclaration +
//    nameof(TransuraniumActivityOut) + strNotNullDeclaration +
//    nameof(BetaGammaActivityIn) + strNotNullDeclaration +
//    nameof(AlphaActivityIn) + strNotNullDeclaration +
//    nameof(BetaGammaActivityOut) + strNotNullDeclaration +
//    nameof(AlphaActivityOut) + strNotNullDeclaration +
//    nameof(RegNo) + strNotNullDeclaration +
//    nameof(OrganUprav) + strNotNullDeclaration +
//    nameof(SubjectRF) + strNotNullDeclaration +
//    nameof(JurLico) + strNotNullDeclaration +
//    nameof(ShortJurLico) + strNotNullDeclaration +
//    nameof(JurLicoAddress) + strNotNullDeclaration +
//    nameof(JurLicoFactAddress) + strNotNullDeclaration +
//    nameof(GradeFIO) + strNotNullDeclaration +
//    nameof(Telephone) + strNotNullDeclaration +
//    nameof(Fax) + strNotNullDeclaration +
//    nameof(Email) + strNotNullDeclaration +
//    nameof(Okpo) + strNotNullDeclaration +
//    nameof(Okved) + strNotNullDeclaration +
//    nameof(Okogu) + strNotNullDeclaration +
//    nameof(Oktmo) + strNotNullDeclaration +
//    nameof(Inn) + strNotNullDeclaration +
//    nameof(Kpp) + strNotNullDeclaration +
//    nameof(Okopf) + strNotNullDeclaration +
//    nameof(Okfs) + strNotNullDeclaration +
//    nameof(Volume20) + doubleNotNullDeclaration +
//    nameof(Volume6) + doubleNotNullDeclaration +
//    nameof(SaltConcentration) + strNotNullDeclaration +
//    nameof(SpecificActivity) + strNotNullDeclaration +
//    nameof(Mass21) + doubleNotNullDeclaration +
//    nameof(Mass7) + doubleNotNullDeclaration +
//    nameof(IndividualNumberZHRO) + strNotNullDeclaration +
//    nameof(IndividualNumberZHROrecoded) + strNotNullDeclaration +
//    nameof(VolumeOutOfPack) + doubleNotNullDeclaration +
//    nameof(PackFactoryNumber) + strNotNullDeclaration +
//    nameof(MassOutOfPack) + doubleNotNullDeclaration +
//    nameof(FormingDate) + dateNotNullDeclaration +
//    nameof(MainRadionuclids) + strNotNullDeclaration +
//    nameof(CodeRAO) + strNotNullDeclaration +
//    nameof(AlphaActivity) + strNotNullDeclaration +
//    nameof(BetaGammaActivity) + strNotNullDeclaration +
//    nameof(TritiumActivity) + strNotNullDeclaration +
//    nameof(TransuraniumActivity) + strNotNullDeclaration +
//    nameof(StoragePlaceCode) + strNotNullDeclaration +
//    nameof(StoragePlaceName) + strNotNullDeclaration +
//    nameof(Subsidy) + strNotNullDeclaration +
//    nameof(StoragePlaceNameNote) + strNotNullDeclaration +
//    nameof(StatusRAO) + strNotNullDeclaration +
//    nameof(RefineOrSortRAOCode) + strNotNullDeclaration +
//    nameof(FcpNumber) + strNotNullDeclaration +
//    nameof(Volume) + doubleNotNullDeclaration +
//    nameof(Mass) + doubleNotNullDeclaration +
//    nameof(ActivityMeasurementDate) + dateNotNullDeclaration +
//    nameof(Sort) + strNotNullDeclaration +
//    nameof(Name) + strNotNullDeclaration +
//    nameof(AggregateState) + strNotNullDeclaration +
//    nameof(PassportNumber) + strNotNullDeclaration +
//    nameof(PassportNumberRecoded) + strNotNullDeclaration +
//    nameof(PassportNumberNote) + strNotNullDeclaration +
//    nameof(Type) + strNotNullDeclaration +
//    nameof(TypeRecoded) + strNotNullDeclaration +
//    nameof(Radionuclids) + strNotNullDeclaration +
//    nameof(FactoryNumber) + strNotNullDeclaration +
//    nameof(FactoryNumberRecoded) + strNotNullDeclaration +
//    nameof(Quantity) + intNotNullDeclaration +
//    nameof(Activity) + strNotNullDeclaration +
//    nameof(ActivityNote) + strNotNullDeclaration +
//    nameof(CreationDate) + dateNotNullDeclaration +
//    nameof(CreatorOKPO) + strNotNullDeclaration +
//    nameof(CreatorOKPONote) + strNotNullDeclaration +
//    nameof(Category) + shortNotNullDeclaration +
//    nameof(SignedServicePeriod) + strNotNullDeclaration +
//    nameof(PropertyCode) + strNotNullDeclaration +
//    nameof(Owner) + strNotNullDeclaration +
//    nameof(ProviderOrRecieverOKPO) + strNotNullDeclaration +
//    nameof(ProviderOrRecieverOKPONote) + strNotNullDeclaration +
//    nameof(TransporterOKPO) + strNotNullDeclaration +
//    nameof(TransporterOKPONote) + strNotNullDeclaration +
//    nameof(PackName) + strNotNullDeclaration +
//    nameof(PackNameNote) + strNotNullDeclaration +
//    nameof(PackType) + strNotNullDeclaration +
//    nameof(PackTypeRecoded) + strNotNullDeclaration +
//    nameof(PackTypeNote) + strNotNullDeclaration +
//    nameof(PackNumber) + strNotNullDeclaration +
//    nameof(PackNumberRecoded) + strNotNullDeclaration +
//}
