namespace Models.DBRealization
{
    public class SQLFormConsts
    {
        //Const_Params
        public const string strNotNullDeclaration = " varchar(255) not null, ";
        public const string intNotNullDeclaration = " int not null, ";
        public const string shortNotNullDeclaration = " smallint not null, ";
        public const string byteNullDeclaration = " tinyint null, ";
        public const string dateNotNullDeclaration = " timestamp not null, ";
        public const string doubleNotNullDeclaration = " float(53) not null, ";
        public const string boolNotNullDeclaration = " boolean not null, ";
        public const string floatNotNullDeclaration = " float(24) not null, ";
        public const string shortNullDeclaration = " smallint null, ";
        public const string intNullDeclaration = " int null, ";
        public const string doubleNullDeclaration = " float(53) null, ";
        //Const_Params

        //Список всех форм
        public static string Reports()
        {
            return "";
        }
        //Форма (страница, много строчек)

        //1_Forms
        protected static string Form1()
        {
            return
                $"NumberInOrder{intNotNullDeclaration}CorrectionNumber{shortNotNullDeclaration}OperationCode{strNotNullDeclaration}OperationDate{dateNotNullDeclaration}DocumentVid{byteNullDeclaration}DocumentNumber{strNotNullDeclaration}DocumentNumberRecoded{strNotNullDeclaration}DocumentDateNote{strNotNullDeclaration}DocumentDate{dateNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form10()
        {
            return
                $"RegNo{strNotNullDeclaration}OrganUprav{strNotNullDeclaration}SubjectRF{strNotNullDeclaration}JurLico{strNotNullDeclaration}ShortJurLico{strNotNullDeclaration}JurLicoAddress{strNotNullDeclaration}JurLicoFactAddress{strNotNullDeclaration}GradeFIO{strNotNullDeclaration}Telephone{strNotNullDeclaration}Fax{strNotNullDeclaration}Email{strNotNullDeclaration}Okpo{strNotNullDeclaration}Okved{strNotNullDeclaration}Okogu{strNotNullDeclaration}Oktmo{strNotNullDeclaration}Inn{strNotNullDeclaration}Kpp{strNotNullDeclaration}Okopf{strNotNullDeclaration}Okfs{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Report()
        {
            return
                $"IsCorrection{boolNotNullDeclaration}CorrectionNumber{shortNotNullDeclaration}NumberInOrder{intNotNullDeclaration}Comments{strNotNullDeclaration}StartPeriod{dateNotNullDeclaration}EndPeriod{dateNotNullDeclaration}ExportDate{dateNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form11()
        {
            return
                $"{Form1()},PassportNumber{strNotNullDeclaration}PassportNumberNote{strNotNullDeclaration}PassportNumberRecoded{strNotNullDeclaration}Type{strNotNullDeclaration}TypeRecoded{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}FactoryNumber{strNotNullDeclaration}FactoryNumberRecoded{strNotNullDeclaration}Quantity{intNullDeclaration}Activity{strNotNullDeclaration}ActivityNote{strNotNullDeclaration}CreationDate{strNotNullDeclaration}CreatorOKPO{strNotNullDeclaration}CreatorOKPONote{strNotNullDeclaration}Category{shortNotNullDeclaration}SignedServicePeriod{floatNotNullDeclaration}PropertyCode{shortNotNullDeclaration}Owner{strNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration}TransporterOKPO{strNotNullDeclaration}TransporterOKPONote{strNotNullDeclaration}PackName{strNotNullDeclaration}PackNameNote{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration}PackTypeNote{strNotNullDeclaration}PackNumber{strNotNullDeclaration}OwnerNote{strNotNullDeclaration}ActivityNote{strNotNullDeclaration}PackNumberNote{strNotNullDeclaration}CreationDateNote{strNotNullDeclaration}PackNumberRecoded{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form12()
        {
            return
                $"{Form1()},PassportNumber{strNotNullDeclaration}PassportNumberNote{strNotNullDeclaration}PassportNumberRecoded{strNotNullDeclaration}NameIOU{strNotNullDeclaration}FactoryNumber{strNotNullDeclaration}FactoryNumberRecoded{strNotNullDeclaration}CreationDate{strNotNullDeclaration}CreatorOKPO{strNotNullDeclaration}CreatorOKPONote{strNotNullDeclaration}SignedServicePeriod{floatNotNullDeclaration}PropertyCode{shortNotNullDeclaration}Owner{strNotNullDeclaration}Mass{strNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration}TransporterOKPO{strNotNullDeclaration}TransporterOKPONote{strNotNullDeclaration}PackName{strNotNullDeclaration}PackNameNote{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration}PackTypeNote{strNotNullDeclaration}PackNumber{strNotNullDeclaration}PackNumberRecoded{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form13()
        {
            return
                $"{Form1()},PassportNumber{strNotNullDeclaration}PassportNumberNote{strNotNullDeclaration}PassportNumberRecoded{strNotNullDeclaration}Type{strNotNullDeclaration}FactoryNumber{strNotNullDeclaration}FactoryNumberRecoded{strNotNullDeclaration}CreationDate{strNotNullDeclaration}CreatorOKPO{strNotNullDeclaration}CreatorOKPONote{strNotNullDeclaration}AggregateState{shortNotNullDeclaration}PropertyCode{shortNotNullDeclaration}Owner{strNotNullDeclaration}TypeRecoded{strNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration}TransporterOKPO{strNotNullDeclaration}TransporterOKPONote{strNotNullDeclaration}PackName{strNotNullDeclaration}PackNameNote{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration}PackTypeNote{strNotNullDeclaration}PackNumber{strNotNullDeclaration}PackNumberRecoded{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Activity{strNotNullDeclaration}ActivityNote{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form14()
        {
            return
                $"{Form1()},PassportNumber{strNotNullDeclaration}PassportNumberNote{strNotNullDeclaration}PassportNumberRecoded{strNotNullDeclaration}Name{strNotNullDeclaration}Sort{shortNotNullDeclaration}Type{strNotNullDeclaration}ActivityMeasurementDate{dateNotNullDeclaration}Volume{doubleNotNullDeclaration}Mass{doubleNotNullDeclaration}AggregateState{shortNotNullDeclaration}PropertyCode{shortNotNullDeclaration}Owner{strNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration}TransporterOKPO{strNotNullDeclaration}TransporterOKPONote{strNotNullDeclaration}PackName{strNotNullDeclaration}PackNameNote{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration}PackTypeNote{strNotNullDeclaration}PackNumber{strNotNullDeclaration}PackNumberRecoded{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Activity{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form15()
        {
            return
                $"{Form1()},PassportNumber{strNotNullDeclaration}PassportNumberNote{strNotNullDeclaration}PassportNumberRecoded{strNotNullDeclaration}StatusRAO{strNotNullDeclaration}StoragePlaceName{strNotNullDeclaration}StoragePlaceNameNote{strNotNullDeclaration}StoragePlaceCode{strNotNullDeclaration}RefineOrSortRAOCode{strNotNullDeclaration}Subsidy{strNotNullDeclaration}FcpNumber{strNotNullDeclaration}Quantity{intNotNullDeclaration}Type{strNotNullDeclaration}FactoryNumber{strNotNullDeclaration}FactoryNumberRecoded{strNotNullDeclaration}CreationDate{dateNotNullDeclaration}TypeRecoded{strNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration}TransporterOKPO{strNotNullDeclaration}TransporterOKPONote{strNotNullDeclaration}PackName{strNotNullDeclaration}PackNameNote{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration}PackTypeNote{strNotNullDeclaration}PackNumber{strNotNullDeclaration}PackNumberRecoded{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Activity{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form16()
        {
            return
                $"{Form1()},MainRadionuclids{strNotNullDeclaration}CodeRAO{strNotNullDeclaration}AlphaActivity{strNotNullDeclaration}BetaGammaActivity{strNotNullDeclaration}TritiumActivity{strNotNullDeclaration}TransuraniumActivity{strNotNullDeclaration}Subsidy{strNotNullDeclaration}StatusRAO{strNotNullDeclaration}RefineOrSortRAOCode{strNotNullDeclaration}FcpNumber{strNotNullDeclaration}Volume{doubleNotNullDeclaration}Mass{doubleNotNullDeclaration}ActivityMeasurementDate{dateNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration}TransporterOKPO{strNotNullDeclaration}TransporterOKPONote{strNotNullDeclaration}PackName{strNotNullDeclaration}PackNameNote{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration}PackTypeNote{strNotNullDeclaration}PackNumber{strNotNullDeclaration}PackNumberRecoded{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form17()
        {
            return
                $"{Form1()},SpecificActivity{strNotNullDeclaration}VolumeOutOfPack{strNotNullDeclaration}PackFactoryNumber{strNotNullDeclaration}MassOutOfPack{strNotNullDeclaration}FormingDate{strNotNullDeclaration}CodeRAO{strNotNullDeclaration}AlphaActivity{strNotNullDeclaration}BetaGammaActivity{strNotNullDeclaration}TritiumActivity{strNotNullDeclaration}TransuraniumActivity{strNotNullDeclaration}StoragePlaceCode{strNotNullDeclaration}StoragePlaceName{strNotNullDeclaration}Subsidy{strNotNullDeclaration}StoragePlaceNameNote{strNotNullDeclaration}StatusRAO{strNotNullDeclaration}RefineOrSortRAOCode{strNotNullDeclaration}FcpNumber{strNotNullDeclaration}Volume{strNotNullDeclaration}Mass{strNotNullDeclaration}PassportNumber{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Quantity{intNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration}TransporterOKPO{strNotNullDeclaration}TransporterOKPONote{strNotNullDeclaration}PackName{strNotNullDeclaration}PackNameNote{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration}PackTypeNote{strNotNullDeclaration}PackNumber{strNotNullDeclaration}PackNumberRecoded{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form18()
        {
            return
                $"{Form1()},Volume20{strNotNullDeclaration}Volume6{strNotNullDeclaration}SaltConcentration{doubleNullDeclaration}SpecificActivity{strNotNullDeclaration}Mass21{strNotNullDeclaration}Mass7{strNotNullDeclaration}IndividualNumberZHRO{strNotNullDeclaration}IndividualNumberZHROrecoded{strNotNullDeclaration}CodeRAO{strNotNullDeclaration}AlphaActivity{strNotNullDeclaration}BetaGammaActivity{strNotNullDeclaration}TritiumActivity{strNotNullDeclaration}TransuraniumActivity{strNotNullDeclaration}StoragePlaceCode{strNotNullDeclaration}StoragePlaceName{strNotNullDeclaration}Subsidy{strNotNullDeclaration}StoragePlaceNameNote{strNotNullDeclaration}StatusRAO{strNotNullDeclaration}RefineOrSortRAOCode{strNotNullDeclaration}FcpNumber{strNotNullDeclaration}PassportNumber{strNotNullDeclaration}PassportNumberRecoded{strNotNullDeclaration}PassportNumberNote{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration}TransporterOKPO{strNotNullDeclaration}TransporterOKPONote{strNotNullDeclaration.Replace(",", "")}";
        }

        public static string Form19()
        {
            return
                $"{Form1()},Activity{strNotNullDeclaration}CodeTypeAccObject{shortNotNullDeclaration}Quantity{intNullDeclaration}Radionuclids{strNotNullDeclaration.Replace(",", "")}";
        }
        //1_Forms

        //2_Forms
        public static string Form2()
        {
            return
                $"NumberInOrder{intNotNullDeclaration}CorrectionNumber{shortNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form20()
        {
            return
                $"RegNo{strNotNullDeclaration}OrganUprav{strNotNullDeclaration}SubjectRF{strNotNullDeclaration}JurLico{strNotNullDeclaration}ShortJurLico{strNotNullDeclaration}JurLicoAddress{strNotNullDeclaration}JurLicoFactAddress{strNotNullDeclaration}GradeFIO{strNotNullDeclaration}Telephone{strNotNullDeclaration}Fax{strNotNullDeclaration}Email{strNotNullDeclaration}Okpo{strNotNullDeclaration}Okved{strNotNullDeclaration}Okogu{strNotNullDeclaration}Oktmo{strNotNullDeclaration}Inn{strNotNullDeclaration}Kpp{strNotNullDeclaration}Okopf{strNotNullDeclaration}Okfs{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form21()
        {
            return
                $"{Form2()},RefineMachineName{strNotNullDeclaration}MachineCode{byteNullDeclaration}MachinePower{strNotNullDeclaration}NumberOfHoursPerYear{strNotNullDeclaration}CodeRAOIn{strNotNullDeclaration}StatusRAOIn{strNotNullDeclaration}VolumeIn{strNotNullDeclaration}MassIn{strNotNullDeclaration}QuantityIn{strNotNullDeclaration}CodeRAOout{strNotNullDeclaration}StatusRAOout{strNotNullDeclaration}VolumeOut{doubleNotNullDeclaration}MassOut{doubleNotNullDeclaration}TritiumActivityIn{strNotNullDeclaration}TritiumActivityOut{strNotNullDeclaration}QuantityOZIIIout{strNotNullDeclaration}TransuraniumActivityIn{strNotNullDeclaration}TransuraniumActivityOut{strNotNullDeclaration}BetaGammaActivityIn{strNotNullDeclaration}AlphaActivityIn{strNotNullDeclaration}BetaGammaActivityOut{strNotNullDeclaration}AlphaActivityOut{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form22()
        {
            return
                $"{Form2()},TransuraniumActivity{strNotNullDeclaration}TritiumActivity{strNotNullDeclaration}BetaGammaActivity{strNotNullDeclaration}AlphaActivity{strNotNullDeclaration}MainRadionuclids{strNotNullDeclaration}Subsidy{strNotNullDeclaration}FcpNumber{strNotNullDeclaration}QuantityOZIII{intNullDeclaration}PackQuantity{intNullDeclaration}CodeRAO{strNotNullDeclaration}StatusRAO{strNotNullDeclaration}PackName{strNotNullDeclaration}PackNameNote{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration}PackTypeNote{strNotNullDeclaration}StoragePlaceCode{strNotNullDeclaration}StoragePlaceName{strNotNullDeclaration}StoragePlaceNameNote{strNotNullDeclaration}VolumeOutOfPack{strNotNullDeclaration}MassOutOfPack{strNotNullDeclaration}VolumeInPack{strNotNullDeclaration}MassInPack{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form23()
        {
            return
                $"{Form2()},DocumentDate{strNotNullDeclaration}DocumentName{strNotNullDeclaration}DocumentNumber{strNotNullDeclaration}DocumentNumberRecoded{strNotNullDeclaration}ExpirationDate{strNotNullDeclaration}ProjectVolume{strNotNullDeclaration}ProjectVolumeNote{strNotNullDeclaration}SummaryActivity{strNotNullDeclaration}QuantityOZIII{intNotNullDeclaration}CodeRAO{strNotNullDeclaration}StoragePlaceCode{strNotNullDeclaration}StoragePlaceName{strNotNullDeclaration}StoragePlaceNameNote{strNotNullDeclaration}Volume{strNotNullDeclaration}Mass{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form24()
        {
            return
                $"{Form2()},CodeOYATnote{strNotNullDeclaration}MassCreated{doubleNotNullDeclaration}QuantityCreated{intNotNullDeclaration}QuantityCreatedNote{intNotNullDeclaration}MassFromAnothers{doubleNotNullDeclaration}QuantityFromAnothers{intNotNullDeclaration}QuantityFromAnothersNote{intNotNullDeclaration}MassFromAnothersImported{doubleNotNullDeclaration}QuantityFromAnothersImported{intNotNullDeclaration}QuantityFromAnothersINote{intNotNullDeclaration}MassAnotherReasons{doubleNotNullDeclaration}QuantityAnotherReasons{intNotNullDeclaration}QuantityAnotherReasonsNote{intNotNullDeclaration}MassRefined{doubleNotNullDeclaration}MassTransferredToAnother{doubleNotNullDeclaration}QuantityTransferredToAnother{intNotNullDeclaration}QuantityTransferredToNote{intNotNullDeclaration}QuantityRefined{intNotNullDeclaration}QuantityRefinedNote{intNotNullDeclaration}MassRemovedFromAccount{doubleNotNullDeclaration}QuantityRemovedFromAccount{intNotNullDeclaration}QuantityRemovedFromNote{intNotNullDeclaration}CodeOYAT{strNotNullDeclaration}FcpNumber{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form25()
        {
            return
                $"{Form2()},CodeOYATnote{strNotNullDeclaration}CodeOYAT{strNotNullDeclaration}AlphaActivity{strNotNullDeclaration}BetaGammaActivity{strNotNullDeclaration}StoragePlaceCode{strNotNullDeclaration}StoragePlaceName{strNotNullDeclaration}FcpNumber{strNotNullDeclaration}Quantity{intNotNullDeclaration}CellMass{doubleNotNullDeclaration}FuelMass{doubleNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form26()
        {
            return
                $"{Form2()},SourcesQuantity{intNotNullDeclaration}ObservedSourceNumber{strNotNullDeclaration}ControlledAreaName{strNotNullDeclaration}SupposedWasteSource{strNotNullDeclaration}DistanceToWasteSource{intNotNullDeclaration}TestDepth{intNotNullDeclaration}TestDepthNote{intNotNullDeclaration}RadionuclidName{strNotNullDeclaration}AverageYearConcentration{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form27()
        {
            return
                $"{Form2()},PermissionNumber{strNotNullDeclaration}PermissionIssueDate{strNotNullDeclaration}PermissionDocumentName{strNotNullDeclaration}ValidBegin{dateNotNullDeclaration}ValidThru{dateNotNullDeclaration}RadionuclidNameNote{strNotNullDeclaration}AllowedWasteValue{strNotNullDeclaration}AllowedWasteValueNote{strNotNullDeclaration}FactedWasteValue{strNotNullDeclaration}FactedWasteValueNote{strNotNullDeclaration}WasteOutbreakPreviousYear{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form28()
        {
            return
                $"{Form2()},WasteSourceName{strNotNullDeclaration}PermissionNumber1{strNotNullDeclaration}PermissionIssueDate1{dateNotNullDeclaration}PermissionDocumentName1{strNotNullDeclaration}ValidBegin1{dateNotNullDeclaration}ValidThru1{dateNotNullDeclaration}PermissionNumber2{strNotNullDeclaration}PermissionIssueDate2{dateNotNullDeclaration}ValidBegin2{dateNotNullDeclaration}PermissionDocumentName2{strNotNullDeclaration}ValidThru2{dateNotNullDeclaration}WasteRecieverName{strNotNullDeclaration}RecieverTypeCode{strNotNullDeclaration}PoolDistrictName{strNotNullDeclaration}AllowedWasteRemovalVolume{doubleNotNullDeclaration}RemovedWasteVolume{doubleNotNullDeclaration}RemovedWasteVolumeNote{doubleNotNullDeclaration}PermissionNumber{strNotNullDeclaration}PermissionIssueDate{strNotNullDeclaration}ValidBegin{dateNotNullDeclaration}ValidThru{dateNotNullDeclaration}PermissionDocumentName{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form29()
        {
            return
                $"{Form2()},AllowedActivity{strNotNullDeclaration}AllowedActivityNote{strNotNullDeclaration}FactedActivity{strNotNullDeclaration}FactedActivityNote{strNotNullDeclaration}RadionuclidName{strNotNullDeclaration}WasteSourceName{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form210()
        {
            return
                $"{Form2()},IndicatorName{strNotNullDeclaration}PlotName{strNotNullDeclaration}PlotKadastrNumber{strNotNullDeclaration}PlotCode{strNotNullDeclaration}InfectedArea{intNotNullDeclaration}AvgGammaRaysDosePower{doubleNotNullDeclaration}MaxGammaRaysDosePower{doubleNotNullDeclaration}WasteDensityAlpha{doubleNotNullDeclaration}WasteDensityBeta{doubleNotNullDeclaration}FcpNumber{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form211()
        {
            return
                $"{Form2()},Radionuclids{strNotNullDeclaration}PlotName{strNotNullDeclaration}PlotKadastrNumber{strNotNullDeclaration}PlotCode{strNotNullDeclaration}InfectedArea{intNotNullDeclaration}RadionuclidNameNote{strNotNullDeclaration}SpecificActivityOfPlot{strNotNullDeclaration}SpecificActivityOfLiquidPart{strNotNullDeclaration}SpecificActivityOfDensePart{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form212()
        {
            return
                $"{Form2()},Radionuclids{strNotNullDeclaration}OperationCode{strNotNullDeclaration}ObjectTypeCode{strNotNullDeclaration}Activity{doubleNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration.Replace(",", "")}";
        }
        //2_Forms

        //3_Forms
        public static string Form3()
        {
            return
                $"CorrectionNumber{shortNotNullDeclaration}NotificationDate{dateNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form30()
        {
            return
                $"RegNo{strNotNullDeclaration}OrganUprav{strNotNullDeclaration}SubjectRF{strNotNullDeclaration}JurLico{strNotNullDeclaration}ShortJurLico{strNotNullDeclaration}JurLicoAddress{strNotNullDeclaration}JurLicoFactAddress{strNotNullDeclaration}GradeFIO{strNotNullDeclaration}Telephone{strNotNullDeclaration}Fax{strNotNullDeclaration}Email{strNotNullDeclaration}Okpo{strNotNullDeclaration}Okved{strNotNullDeclaration}Okogu{strNotNullDeclaration}Oktmo{strNotNullDeclaration}Inn{strNotNullDeclaration}Kpp{strNotNullDeclaration}Okopf{strNotNullDeclaration}Okfs{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form31()
        {
            return
                $"{Form3()},RecieverName{strNotNullDeclaration}RecieverAddress{strNotNullDeclaration}RecieverFactAddress{strNotNullDeclaration}LicenseId{strNotNullDeclaration}SuggestedSolutionDate{dateNotNullDeclaration}UserName{strNotNullDeclaration}UserAddress{strNotNullDeclaration}UserFactAddress{strNotNullDeclaration}UserTelephone{strNotNullDeclaration}UserFax{strNotNullDeclaration}ZriUsageScope{strNotNullDeclaration}ContractId{strNotNullDeclaration}ContractDate{dateNotNullDeclaration}CountryCreator{strNotNullDeclaration}ValidThru{dateNotNullDeclaration}Email{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form31_1()
        {
            return
                $"{Form3()},SummaryActivity{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Quantity{intNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form32()
        {
            return
                $"{Form3()},UniqueAgreementId{strNotNullDeclaration}SupplyDate{dateNotNullDeclaration}FieldsOfWorking{strNotNullDeclaration}LicenseIdRv{strNotNullDeclaration}ValidThruRv{dateNotNullDeclaration}LicenseIdRao{strNotNullDeclaration}ValidThruRao{dateNotNullDeclaration}SupplyAddress{strNotNullDeclaration}RecieverName{strNotNullDeclaration}SummaryActivity{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Quantity{intNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form32_1()
        {
            return
                $"{Form3()},CertificateId{strNotNullDeclaration}NuclearMaterialPresence{strNotNullDeclaration}Kategory{shortNotNullDeclaration}ActivityOnCreation{strNotNullDeclaration}ValidThru{dateNotNullDeclaration}PassportNumber{strNotNullDeclaration}PassportNumberNote{strNotNullDeclaration}Type{strNotNullDeclaration}TypeRecoded{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}FactoryNumber{strNotNullDeclaration}FactoryNumberRecoded{strNotNullDeclaration}CreationDate{dateNotNullDeclaration}CreatorOKPO{strNotNullDeclaration}CreatorOKPONote{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form32_2()
        {
            return
                $"{Form3()},DepletedUraniumMass{doubleNotNullDeclaration}CreationYear{strNotNullDeclaration}Id{strNotNullDeclaration}PackName{strNotNullDeclaration}PackType{strNotNullDeclaration}PackTypeRecoded{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form32_3()
        {
            return
                $"{Form3()},IdName{strNotNullDeclaration}Val{strNotNullDeclaration.Replace(",", "")}";
        }
        //3_Forms

        //4_Forms
        public static string Form4()
        {
            return "";
        }
        public static string Form40()
        {
            return
                $"{Form4()}SubjectRF{strNotNullDeclaration}Yyear{intNotNullDeclaration}SubjectAuthorityName{intNotNullDeclaration}ShortSubjectAuthorityName{intNotNullDeclaration}FactAddress{strNotNullDeclaration}GradeFIOchef{strNotNullDeclaration}GradeFIOresponsibleExecutor{strNotNullDeclaration}Telephone{strNotNullDeclaration}Fax{strNotNullDeclaration}Email{strNotNullDeclaration}Telephone1{strNotNullDeclaration}Fax1{strNotNullDeclaration}Email1{strNotNullDeclaration}OrgName{strNotNullDeclaration}ShortOrgName{strNotNullDeclaration}FactAddress1{strNotNullDeclaration}GradeFIOchef1{strNotNullDeclaration}GradeFIOresponsibleExecutor1{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form41()
        {
            return
                $"{Form4()}Notes{strNotNullDeclaration}OrgName{strNotNullDeclaration}NumberInOrder{intNotNullDeclaration}LicenseInfo{strNotNullDeclaration}QuantityOfFormsInv{intNotNullDeclaration}QuantityOfFormsOper{intNotNullDeclaration}QuantityOfFormsYear{intNotNullDeclaration}RegNo{strNotNullDeclaration}Okpo{strNotNullDeclaration.Replace(",", "")}";
        }
        //4_Forms

        //5_Forms
        public static string Form5()
        {
            return
                $"NumberInOrder{intNotNullDeclaration}CorrectionNumber{shortNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form50()
        {
            return
                $"Authority1{intNotNullDeclaration}Yyear{intNotNullDeclaration}JurLico{strNotNullDeclaration}ShortJurLico{strNotNullDeclaration}JurLicoAddress{strNotNullDeclaration}JurLicoFactAddress{strNotNullDeclaration}GradeFIO{strNotNullDeclaration}GradeFIOresponsibleExecutor{strNotNullDeclaration}Telephone{strNotNullDeclaration}Fax{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form51()
        {
            return
                $"{Form5()},OperationCode{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Quantity{intNotNullDeclaration}Activity{strNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form52()
        {
            return
                $"{Form5()},Kategory{shortNotNullDeclaration}Radionuclids{strNotNullDeclaration}Quantity{intNotNullDeclaration}Activity{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form53()
        {
            return
                $"{Form5()},TypeOfAccountedParts{intNotNullDeclaration}KindOri{intNotNullDeclaration}OperationCode{strNotNullDeclaration}Volume{doubleNotNullDeclaration}Mass{doubleNotNullDeclaration}AggregateState{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Quantity{intNotNullDeclaration}Activity{strNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form54()
        {
            return
                $"{Form5()},TypeOfAccountedParts{intNotNullDeclaration}KindOri{intNotNullDeclaration}Volume{doubleNotNullDeclaration}Mass{doubleNotNullDeclaration}AggregateState{strNotNullDeclaration}Radionuclids{strNotNullDeclaration}Quantity{intNotNullDeclaration}Activity{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form55()
        {
            return
                $"{Form5()},OperationCode{strNotNullDeclaration}Mass{doubleNotNullDeclaration}Name{strNotNullDeclaration}Quantity{intNotNullDeclaration}ProviderOrRecieverOKPO{strNotNullDeclaration}ProviderOrRecieverOKPONote{strNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form56()
        {
            return
                $"{Form5()},NameIOU{strNotNullDeclaration}Mass{doubleNotNullDeclaration}Quantity{intNotNullDeclaration.Replace(",", "")}";
        }
        public static string Form57()
        {
            return
                $"{Form5()},Note{strNotNullDeclaration}PermissionNameNumber{strNotNullDeclaration}DocumentNameNumber{strNotNullDeclaration}OrgName{strNotNullDeclaration}AllowedActivity{strNotNullDeclaration}RegNo{strNotNullDeclaration}Okpo{strNotNullDeclaration.Replace(",", "")}";
        }
        //5_Forms
    }
}
