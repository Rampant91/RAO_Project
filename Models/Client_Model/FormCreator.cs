using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public class FormCreator
    {
        public static Form Create(string param)
        {
            switch (param)
            {
                case "10": return new Form10();
                case "11": return new Form11();
                case "12": return new Form12();
                case "13": return new Form13();
                case "14": return new Form14();
                case "15": return new Form15();
                case "16": return new Form16();
                case "17": return new Form17();
                case "18": return new Form18();
                case "19": return new Form19();

                case "20": return new Form20();
                case "21": return new Form21();
                case "22": return new Form22();
                case "23": return new Form23();
                case "24": return new Form24();
                case "25": return new Form25();
                case "26": return new Form26();
                case "27": return new Form27();
                case "28": return new Form28();
                case "29": return new Form29();
                case "210": return new Form210();
                case "211": return new Form211();
                case "212": return new Form212();

                case "30": return new Form30();
                case "31": return new Form31();
                case "32": return new Form32();

                case "40": return new Form40();
                case "41": return new Form41();

                case "50": return new Form50();
                case "51": return new Form51();
                case "52": return new Form52();
                case "53": return new Form53();
                case "54": return new Form54();
                case "55": return new Form55();
                case "56": return new Form56();
                case "57": return new Form57();

                default: return null;
            }
        }

        //Сделать
        public static Form Create(string[] values)
        {
            switch (values[0])
            {
                case "10": return new Form10();
                case "11": return new Form11(values);
                case "12": return new Form12();
                case "13": return new Form13();
                case "14": return new Form14();
                case "15": return new Form15();
                case "16": return new Form16();
                case "17": return new Form17();
                case "18": return new Form18();
                case "19": return new Form19();

                case "20": return new Form20();
                case "21": return new Form21();
                case "22": return new Form22();
                case "23": return new Form23();
                case "24": return new Form24();
                case "25": return new Form25();
                case "26": return new Form26();
                case "27": return new Form27();
                case "28": return new Form28();
                case "29": return new Form29();
                case "210": return new Form210();
                case "211": return new Form211();
                case "212": return new Form212();

                case "30": return new Form30();
                case "31": return new Form31();
                case "32": return new Form32();

                case "40": return new Form40();
                case "41": return new Form41();

                case "50": return new Form50();
                case "51": return new Form51();
                case "52": return new Form52();
                case "53": return new Form53();
                case "54": return new Form54();
                case "55": return new Form55();
                case "56": return new Form56();
                case "57": return new Form57();

                default: return null;
            }
        }
    }
}
