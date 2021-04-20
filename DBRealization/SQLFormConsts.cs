using System;
using System.Collections.Generic;
using System.Text;

namespace DBRealization
{
    public class SQLFormConsts
    {
        //Const_Params
        public const string strNotNullDeclaration = " varchar(255) not null, ";
        public const string intNotNullDeclaration = " int not null, ";
        public const string shortNotNullDeclaration = " smallint not null, ";
        public const string dateNotNullDeclaration = " datetimeoffset not null, ";
        public const string doubleNotNullDeclaration = " float(53) not null, ";
        //Const_Params

        public static string Reports()
        {
            return "";
        }

        public static string Report()
        {
            return "";
        }

        //1_Forms
        protected static string Form1()
        {
            return
                "NumberInOrder" + intNotNullDeclaration +
                "CorrectionNumber" + shortNotNullDeclaration +
                "OperationCode" + shortNotNullDeclaration +
                "OperationDate" + dateNotNullDeclaration +
                "DocumentVid" + shortNotNullDeclaration +
                "DocumentNumber" + strNotNullDeclaration +
                "DocumentNumberRecoded" + strNotNullDeclaration +
                "DocumentDate" + dateNotNullDeclaration.Replace(",","");
        }

        public static string Form10()
        {
            return
                "RegNo" + strNotNullDeclaration +
                "OrganUprav" + strNotNullDeclaration +
                "SubjectRF" + strNotNullDeclaration +
                "JurLico" + strNotNullDeclaration +
                "ShortJurLico" + strNotNullDeclaration +
                "JurLicoAddress" + strNotNullDeclaration +
                "JurLicoFactAddress" + strNotNullDeclaration +
                "GradeFIO" + strNotNullDeclaration +
                "Telephone" + strNotNullDeclaration +
                "Fax" + strNotNullDeclaration +
                "Email" + strNotNullDeclaration +
                "Okpo" + strNotNullDeclaration +
                "Okved" + strNotNullDeclaration +
                "Okogu" + strNotNullDeclaration +
                "Oktmo" + strNotNullDeclaration +
                "Inn" + strNotNullDeclaration +
                "Kpp" + strNotNullDeclaration +
                "Okopf" + strNotNullDeclaration +
                "Okfs" + strNotNullDeclaration.Replace(",","");
        }

        public static string Form11()
        {
            return Form1() +
                "PassportNumber" + strNotNullDeclaration +
                "PassportNumberNote" + strNotNullDeclaration +
                "PassportNumberRecoded" + strNotNullDeclaration +
                "Type" + strNotNullDeclaration +
                "TypeRecoded" + strNotNullDeclaration +
                "Radionuclids" + strNotNullDeclaration +
                "FactoryNumber" + strNotNullDeclaration +
                "FactoryNumberRecoded" + strNotNullDeclaration +
                "Quantity" + intNotNullDeclaration +
                "Activity" + strNotNullDeclaration +
                "ActivityNote" + strNotNullDeclaration +
                "CreationDate" + dateNotNullDeclaration +
                "CreatorOKPO" + strNotNullDeclaration +
                "CreatorOKPONote" + strNotNullDeclaration +
                "Category" + shortNotNullDeclaration +
                "SignedServicePeriod" + intNotNullDeclaration +
                "PropertyCode" + shortNotNullDeclaration +
                "Owner" + strNotNullDeclaration +
                "ProviderOrRecieverOKPO" + strNotNullDeclaration +
                "ProviderOrRecieverOKPONote" + strNotNullDeclaration +
                "TransporterOKPO" + strNotNullDeclaration +
                "TransporterOKPONote" + strNotNullDeclaration +
                "PackName" + strNotNullDeclaration +
                "PackNameNote" + strNotNullDeclaration +
                "PackType" + strNotNullDeclaration +
                "PackTypeRecoded" + strNotNullDeclaration +
                "PackTypeNote" + strNotNullDeclaration +
                "PackNumber" + strNotNullDeclaration +
                "PackNumberRecoded" + strNotNullDeclaration.Replace(",","");
        }

        public static string Form12()
        {
            return
                Form1() +
                "PassportNumber" + strNotNullDeclaration +
                "PassportNumberNote" + strNotNullDeclaration +
                "PassportNumberRecoded" + strNotNullDeclaration +
                "NameIOU" + strNotNullDeclaration +
                "FactoryNumber" + strNotNullDeclaration +
                "FactoryNumberRecoded" + strNotNullDeclaration +
                "CreationDate" + dateNotNullDeclaration +
                "CreatorOKPO" + strNotNullDeclaration +
                "CreatorOKPONote" + strNotNullDeclaration +
                "SignedServicePeriod" + intNotNullDeclaration +
                "PropertyCode" + shortNotNullDeclaration +
                "Owner" + strNotNullDeclaration +
                "Mass" + strNotNullDeclaration +
                "ProviderOrRecieverOKPO" + strNotNullDeclaration +
                "ProviderOrRecieverOKPONote" + strNotNullDeclaration +
                "TransporterOKPO" + strNotNullDeclaration +
                "TransporterOKPONote" + strNotNullDeclaration +
                "PackName" + strNotNullDeclaration +
                "PackNameNote" + strNotNullDeclaration +
                "PackType" + strNotNullDeclaration +
                "PackTypeRecoded" + strNotNullDeclaration +
                "PackTypeNote" + strNotNullDeclaration +
                "PackNumber" + strNotNullDeclaration +
                "PackNumberRecoded"+strNotNullDeclaration.Replace(",","");
        }

        public static string Form13()
        {
            return
                Form1() +
                "PassportNumber" + strNotNullDeclaration +
                "PassportNumberNote" + strNotNullDeclaration +
                "PassportNumberRecoded" + strNotNullDeclaration +
                "Type" + strNotNullDeclaration +
                "FactoryNumber" + strNotNullDeclaration +
                "FactoryNumberRecoded" + strNotNullDeclaration +
                "CreationDate" + dateNotNullDeclaration +
                "CreatorOKPO" + strNotNullDeclaration +
                "CreatorOKPONote" + strNotNullDeclaration +
                "AggregateState" + shortNotNullDeclaration +
                "PropertyCode" + shortNotNullDeclaration +
                "Owner" + strNotNullDeclaration +
                "TypeRecoded" + strNotNullDeclaration +
                "ProviderOrRecieverOKPO" + strNotNullDeclaration +
                "ProviderOrRecieverOKPONote" + strNotNullDeclaration +
                "TransporterOKPO" + strNotNullDeclaration +
                "TransporterOKPONote" + strNotNullDeclaration +
                "PackName" + strNotNullDeclaration +
                "PackNameNote" + strNotNullDeclaration +
                "PackType" + strNotNullDeclaration +
                "PackTypeRecoded" + strNotNullDeclaration +
                "PackTypeNote" + strNotNullDeclaration +
                "PackNumber" + strNotNullDeclaration +
                "PackNumberRecoded" + strNotNullDeclaration +
                "Radionuclids" + strNotNullDeclaration +
                "Activity" + strNotNullDeclaration +
                "ActivityNote" + strNotNullDeclaration.Replace(",","");
        }

        public static string Form14()
        {
            return
                Form1() +
                "PassportNumber" + strNotNullDeclaration +
                "PassportNumberNote" + strNotNullDeclaration +
                "PassportNumberRecoded" + strNotNullDeclaration +
                "Name" + strNotNullDeclaration +
                "Sort" + shortNotNullDeclaration +
                "Type" + strNotNullDeclaration +
                "ActivityMeasurementDate" + dateNotNullDeclaration +
                "Volume" + doubleNotNullDeclaration +
                "Mass" + doubleNotNullDeclaration +
                "AggregateState" + shortNotNullDeclaration +
                "PropertyCode" + shortNotNullDeclaration +
                "Owner" + strNotNullDeclaration +
                "ProviderOrRecieverOKPO" + strNotNullDeclaration +
                "ProviderOrRecieverOKPONote" + strNotNullDeclaration +
                "TransporterOKPO" + strNotNullDeclaration +
                "TransporterOKPONote" + strNotNullDeclaration +
                "PackName" + strNotNullDeclaration +
                "PackNameNote" + strNotNullDeclaration +
                "PackType" + strNotNullDeclaration +
                "PackTypeRecoded" + strNotNullDeclaration +
                "PackTypeNote" + strNotNullDeclaration +
                "PackNumber" + strNotNullDeclaration +
                "PackNumberRecoded" + strNotNullDeclaration +
                "Radionuclids" + strNotNullDeclaration +
                "Activity" + strNotNullDeclaration.Replace(",", "");
        }

        public static string Form15()
        {
            return
                Form1() +
                "PassportNumber" + strNotNullDeclaration +
                "PassportNumberNote" + strNotNullDeclaration +
                "PassportNumberRecoded" + strNotNullDeclaration +
                "StatusRAO" + strNotNullDeclaration +
                "StoragePlaceName" + strNotNullDeclaration +
                "StoragePlaceNameNote" + strNotNullDeclaration +
                "StoragePlaceCode" + strNotNullDeclaration +
                "RefineOrSortRAOCode" + strNotNullDeclaration +
                "Subsidy" + strNotNullDeclaration +
                "FcpNumber" + strNotNullDeclaration +
                "Quantity" + intNotNullDeclaration +
                "Type" + strNotNullDeclaration +
                "FactoryNumber" + strNotNullDeclaration +
                "FactoryNumberRecoded" + strNotNullDeclaration +
                "CreationDate" + dateNotNullDeclaration +
                "TypeRecoded" + strNotNullDeclaration +
                "ProviderOrRecieverOKPO" + strNotNullDeclaration +
                "ProviderOrRecieverOKPONote" + strNotNullDeclaration +
                "TransporterOKPO" + strNotNullDeclaration +
                "TransporterOKPONote" + strNotNullDeclaration +
                "PackName" + strNotNullDeclaration +
                "PackNameNote" + strNotNullDeclaration +
                "PackType" + strNotNullDeclaration +
                "PackTypeRecoded" + strNotNullDeclaration +
                "PackTypeNote" + strNotNullDeclaration +
                "PackNumber" + strNotNullDeclaration +
                "PackNumberRecoded" + strNotNullDeclaration +
                "Radionuclids" + strNotNullDeclaration +
                "Activity" + strNotNullDeclaration.Replace(",", "");
        }

        public static string Form16()
        {
            return
                Form1() +
            "MainRadionuclids" + strNotNullDeclaration +
            "CodeRAO" + strNotNullDeclaration +
            "AlphaActivity" + strNotNullDeclaration +
            "BetaGammaActivity" + strNotNullDeclaration +
            "TritiumActivity" + strNotNullDeclaration +
            "TransuraniumActivity" + strNotNullDeclaration +
            "Subsidy" + strNotNullDeclaration +
            "StatusRAO" + strNotNullDeclaration +
            "RefineOrSortRAOCode" + strNotNullDeclaration +
            "FcpNumber" + strNotNullDeclaration +
            "Volume" + doubleNotNullDeclaration +
            "Mass" + doubleNotNullDeclaration +
            "ActivityMeasurementDate" + dateNotNullDeclaration +
            "ProviderOrRecieverOKPO" + strNotNullDeclaration +
            "ProviderOrRecieverOKPONote" + strNotNullDeclaration +
            "TransporterOKPO" + strNotNullDeclaration +
            "TransporterOKPONote" + strNotNullDeclaration +
            "PackName" + strNotNullDeclaration +
            "PackNameNote" + strNotNullDeclaration +
            "PackType" + strNotNullDeclaration +
            "PackTypeRecoded" + strNotNullDeclaration +
            "PackTypeNote" + strNotNullDeclaration +
            "PackNumber" + strNotNullDeclaration +
            "PackNumberRecoded" +  strNotNullDeclaration.Replace(",", "");
        }

        public static string Form17()
        {
            return
                Form1() +
            "SpecificActivity" + strNotNullDeclaration +
            "VolumeOutOfPack" + doubleNotNullDeclaration +
            "PackFactoryNumber" + strNotNullDeclaration +
            "MassOutOfPack" + doubleNotNullDeclaration +
            "FormingDate" + dateNotNullDeclaration +
            "CodeRAO" + strNotNullDeclaration +
            "AlphaActivity" + strNotNullDeclaration +
            "BetaGammaActivity" + strNotNullDeclaration +
            "TritiumActivity" + strNotNullDeclaration +
            "TransuraniumActivity" + strNotNullDeclaration +
            "StoragePlaceCode" + strNotNullDeclaration +
            "StoragePlaceName" + strNotNullDeclaration +
            "Subsidy" + strNotNullDeclaration +
            "StoragePlaceNameNote" + strNotNullDeclaration +
            "StatusRAO" + strNotNullDeclaration +
            "RefineOrSortRAOCode" + strNotNullDeclaration +
            "FcpNumber" + strNotNullDeclaration +
            "Volume" + strNotNullDeclaration +
            "Mass" + strNotNullDeclaration +
            "PassportNumber" + strNotNullDeclaration +
            "Radionuclids" + strNotNullDeclaration +
            "Quantity" + intNotNullDeclaration +
            "ProviderOrRecieverOKPO" + strNotNullDeclaration +
            "ProviderOrRecieverOKPONote" + strNotNullDeclaration +
            "TransporterOKPO" + strNotNullDeclaration +
            "TransporterOKPONote" + strNotNullDeclaration +
            "PackName" + strNotNullDeclaration +
            "PackNameNote" + strNotNullDeclaration +
            "PackType" + strNotNullDeclaration +
            "PackTypeRecoded" + strNotNullDeclaration +
            "PackTypeNote" + strNotNullDeclaration +
            "PackNumber" + strNotNullDeclaration +
            "PackNumberRecoded" + strNotNullDeclaration.Replace(",", "");
        }

        public static string Form18()
        {
            return
                Form1() +
            "Volume20" + doubleNotNullDeclaration +
            "Volume6" + doubleNotNullDeclaration +
            "SaltConcentration" + doubleNotNullDeclaration +
            "SpecificActivity" + strNotNullDeclaration +
            "Mass21" + doubleNotNullDeclaration +
            "Mass7" + doubleNotNullDeclaration +
            "IndividualNumberZHRO" + strNotNullDeclaration +
            "IndividualNumberZHROrecoded" + strNotNullDeclaration +
            "CodeRAO" + strNotNullDeclaration +
            "AlphaActivity" + strNotNullDeclaration +
            "BetaGammaActivity" + strNotNullDeclaration +
            "TritiumActivity" + strNotNullDeclaration +
            "TransuraniumActivity" + strNotNullDeclaration +
            "StoragePlaceCode" + strNotNullDeclaration +
            "StoragePlaceName" + strNotNullDeclaration +
            "Subsidy" + strNotNullDeclaration +
            "StoragePlaceNameNote" + strNotNullDeclaration +
            "StatusRAO" + strNotNullDeclaration +
            "RefineOrSortRAOCode" + strNotNullDeclaration +
            "FcpNumber" + strNotNullDeclaration +
            "PassportNumber" + strNotNullDeclaration +
            "PassportNumberRecoded" + strNotNullDeclaration +
            "PassportNumberNote" + strNotNullDeclaration +
            "Radionuclids" + strNotNullDeclaration +
            "ProviderOrRecieverOKPO" + strNotNullDeclaration +
            "ProviderOrRecieverOKPONote" + strNotNullDeclaration +
            "TransporterOKPO" + strNotNullDeclaration +
            "TransporterOKPONote" + strNotNullDeclaration.Replace(",", "");
        }

        public static string Form19()
        {
            return
                Form1() +
            "Activity" + strNotNullDeclaration +
            "CodeTypeAccObject" + shortNotNullDeclaration +
            "Radionuclids" + strNotNullDeclaration.Replace(",", "");
        }
        //1_Forms

        //2_Forms
        public static string Form2()
        {
            return "";
        }
        public static string Form20()
        {
            return "";
        }
        public static string Form21()
        {
            return "";
        }
        public static string Form22()
        {
            return "";
        }
        public static string Form23()
        {
            return "";
        }
        public static string Form24()
        {
            return "";
        }
        public static string Form25()
        {
            return "";
        }
        public static string Form26()
        {
            return "";
        }
        public static string Form27()
        {
            return "";
        }
        public static string Form28()
        {
            return "";
        }
        public static string Form29()
        {
            return "";
        }
        public static string Form210()
        {
            return "";
        }
        public static string Form211()
        {
            return "";
        }
        public static string Form212()
        {
            return "";
        }
        //2_Forms

        //3_Forms
        public static string Form3()
        {
            return "";
        }
        public static string Form30()
        {
            return "";
        }
        public static string Form31()
        {
            return "";
        }
        public static string Form31_1()
        {
            return "";
        }
        public static string Form32()
        {
            return "";
        }
        public static string Form32_1()
        {
            return "";
        }
        public static string Form32_2()
        {
            return "";
        }
        public static string Form32_3()
        {
            return "";
        }
        //3_Forms

        //4_Forms
        public static string Form4()
        {
            return "";
        }
        public static string Form40()
        {
            return "";
        }
        public static string Form41()
        {
            return "";
        }
        //4_Forms

        //5_Forms
        public static string Form5()
        {
            return "";
        }
        public static string Form50()
        {
            return "";
        }
        public static string Form51()
        {
            return "";
        }
        public static string Form52()
        {
            return "";
        }
        public static string Form53()
        {
            return "";
        }
        public static string Form54()
        {
            return "";
        }
        public static string Form55()
        {
            return "";
        }
        public static string Form56()
        {
            return "";
        }
        public static string Form57()
        {
            return "";
        }
        //5_Forms
    }
}
