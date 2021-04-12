using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public class FormCreator
    {
        public static Form Create(bool isSQL,string param)
        {
            switch (param)
            {
                case "10": return new Form10(isSQL);
                case "11": return new Form11(isSQL);
                case "12": return new Form12(isSQL);
                case "13": return new Form13(isSQL);
                case "14": return new Form14(isSQL);
                case "15": return new Form15(isSQL);
                case "16": return new Form16(isSQL);
                case "17": return new Form17(isSQL);
                case "18": return new Form18(isSQL);
                case "19": return new Form19(isSQL);

                case "20": return new Form20(isSQL);
                case "21": return new Form21(isSQL);
                case "22": return new Form22(isSQL);
                case "23": return new Form23(isSQL);
                case "24": return new Form24(isSQL);
                case "25": return new Form25(isSQL);
                case "26": return new Form26(isSQL);
                case "27": return new Form27(isSQL);
                case "28": return new Form28(isSQL);
                case "29": return new Form29(isSQL);
                case "210": return new Form210(isSQL);
                case "211": return new Form211(isSQL);
                case "212": return new Form212(isSQL);

                case "30": return new Form30(isSQL);
                case "31": return new Form31(isSQL);
                case "32": return new Form32(isSQL);

                case "40": return new Form40(isSQL);
                case "41": return new Form41(isSQL);

                case "50": return new Form50(isSQL);
                case "51": return new Form51(isSQL);
                case "52": return new Form52(isSQL);
                case "53": return new Form53(isSQL);
                case "54": return new Form54(isSQL);
                case "55": return new Form55(isSQL);
                case "56": return new Form56(isSQL);
                case "57": return new Form57(isSQL);

                default: return null;
            }
        }

        //Сделать
        public static Form Create(bool isSQL, string[] values)
        {
            switch (values[0])
            {
                case "10": return new Form10(isSQL);
                case "11": return new Form11(isSQL,values);
                case "12": return new Form12(isSQL);
                case "13": return new Form13(isSQL);
                case "14": return new Form14(isSQL);
                case "15": return new Form15(isSQL);
                case "16": return new Form16(isSQL);
                case "17": return new Form17(isSQL);
                case "18": return new Form18(isSQL);
                case "19": return new Form19(isSQL);

                case "20": return new Form20(isSQL);
                case "21": return new Form21(isSQL);
                case "22": return new Form22(isSQL);
                case "23": return new Form23(isSQL);
                case "24": return new Form24(isSQL);
                case "25": return new Form25(isSQL);
                case "26": return new Form26(isSQL);
                case "27": return new Form27(isSQL);
                case "28": return new Form28(isSQL);
                case "29": return new Form29(isSQL);
                case "210": return new Form210(isSQL);
                case "211": return new Form211(isSQL);
                case "212": return new Form212(isSQL);

                case "30": return new Form30(isSQL);
                case "31": return new Form31(isSQL);
                case "32": return new Form32(isSQL);

                case "40": return new Form40(isSQL);
                case "41": return new Form41(isSQL);

                case "50": return new Form50(isSQL);
                case "51": return new Form51(isSQL);
                case "52": return new Form52(isSQL);
                case "53": return new Form53(isSQL);
                case "54": return new Form54(isSQL);
                case "55": return new Form55(isSQL);
                case "56": return new Form56(isSQL);
                case "57": return new Form57(isSQL);

                default: return null;
            }
        }
    }
}
