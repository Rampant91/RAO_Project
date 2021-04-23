using System;
using System.Collections.Generic;
using System.Text;
using DBRealization;
using Collections.Rows_Collection;

namespace Models
{
    public class FormCreator
    {
        public static Abstracts.Form Create(string param, IDataAccess Access)
        {
            switch (param)
            {
                case "10": return new Form10(Access);
                case "11": return new Form11(Access);
                case "12": return new Form12(Access);
                case "13": return new Form13(Access);
                case "14": return new Form14(Access);
                case "15": return new Form15(Access);
                case "16": return new Form16(Access);
                case "17": return new Form17(Access);
                case "18": return new Form18(Access);
                case "19": return new Form19(Access);

                case "20": return new Form20(Access);
                case "21": return new Form21(Access);
                case "22": return new Form22(Access);
                case "23": return new Form23(Access);
                case "24": return new Form24(Access);
                case "25": return new Form25(Access);
                case "26": return new Form26(Access);
                case "27": return new Form27(Access);
                case "28": return new Form28(Access);
                case "29": return new Form29(Access);
                case "210": return new Form210(Access);
                case "211": return new Form211(Access);
                case "212": return new Form212(Access);

                case "30": return new Form30(Access);
                case "31": return new Form31(Access);
                case "32": return new Form32(Access);

                case "40": return new Form40(Access);
                case "41": return new Form41(Access);

                case "50": return new Form50(Access);
                case "51": return new Form51(Access);
                case "52": return new Form52(Access);
                case "53": return new Form53(Access);
                case "54": return new Form54(Access);
                case "55": return new Form55(Access);
                case "56": return new Form56(Access);
                case "57": return new Form57(Access);

                default: return null;
            }
        }
    }
}
