using System;
using System.Collections.Generic;
using System.Text;

namespace DBRealization
{
    public class SQLFormConsts
    {
        public const string strNotNullDeclaration = " varchar(255) not null, ";
        public const string intNotNullDeclaration = " int not null, ";
        public const string shortNotNullDeclaration = " smallint not null, ";
        public const string dateNotNullDeclaration = " datetimeoffset not null, ";
        public const string doubleNotNullDeclaration = " float(53) not null, ";

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
            return Form1() +
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
    }
}
