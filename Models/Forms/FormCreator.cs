using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class FormCreator
    {
        public static Abstracts.Form Create(string param)
        {
            int RowID = 0;
            switch (param)
            {
                case "10": return new Form10(RowID);
                case "11": return new Form11(RowID);
                case "12": return new Form12(RowID);
                case "13": return new Form13(RowID);
                case "14": return new Form14(RowID);
                case "15": return new Form15(RowID);
                case "16": return new Form16(RowID);
                case "17": return new Form17(RowID);
                case "18": return new Form18(RowID);
                case "19": return new Form19(RowID);

                case "20": return new Form20(RowID);
                case "21": return new Form21(RowID);
                case "22": return new Form22(RowID);
                case "23": return new Form23(RowID);
                case "24": return new Form24(RowID);
                case "25": return new Form25(RowID);
                case "26": return new Form26(RowID);
                case "27": return new Form27(RowID);
                case "28": return new Form28(RowID);
                case "29": return new Form29(RowID);
                case "210": return new Form210(RowID);
                case "211": return new Form211(RowID);
                case "212": return new Form212(RowID);

                case "30": return new Form30(RowID);
                case "31": return new Form31(RowID);
                case "32": return new Form32(RowID);

                case "40": return new Form40(RowID);
                case "41": return new Form41(RowID);

                case "50": return new Form50(RowID);
                case "51": return new Form51(RowID);
                case "52": return new Form52(RowID);
                case "53": return new Form53(RowID);
                case "54": return new Form54(RowID);
                case "55": return new Form55(RowID);
                case "56": return new Form56(RowID);
                case "57": return new Form57(RowID);

                default: return null;
            }
        }
        public static Abstracts.Form Create(string param,int RowID)
        {
            switch (param)
            {
                case "10": return new Form10(RowID);
                case "11": return new Form11(RowID);
                case "12": return new Form12(RowID);
                case "13": return new Form13(RowID);
                case "14": return new Form14(RowID);
                case "15": return new Form15(RowID);
                case "16": return new Form16(RowID);
                case "17": return new Form17(RowID);
                case "18": return new Form18(RowID);
                case "19": return new Form19(RowID);

                case "20": return new Form20(RowID);
                case "21": return new Form21(RowID);
                case "22": return new Form22(RowID);
                case "23": return new Form23(RowID);
                case "24": return new Form24(RowID);
                case "25": return new Form25(RowID);
                case "26": return new Form26(RowID);
                case "27": return new Form27(RowID);
                case "28": return new Form28(RowID);
                case "29": return new Form29(RowID);
                case "210": return new Form210(RowID);
                case "211": return new Form211(RowID);
                case "212": return new Form212(RowID);

                case "30": return new Form30(RowID);
                case "31": return new Form31(RowID);
                case "32": return new Form32(RowID);

                case "40": return new Form40(RowID);
                case "41": return new Form41(RowID);

                case "50": return new Form50(RowID);
                case "51": return new Form51(RowID);
                case "52": return new Form52(RowID);
                case "53": return new Form53(RowID);
                case "54": return new Form54(RowID);
                case "55": return new Form55(RowID);
                case "56": return new Form56(RowID);
                case "57": return new Form57(RowID);

                default: return null;
            }
        }
    }
}
