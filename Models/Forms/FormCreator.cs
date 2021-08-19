namespace Models
{
    public class FormCreator
    {
        public static Abstracts.Form Create(string Param)
        {
            switch (Param)
            {
                case "1.0": return new Form10();
                case "1.1": return new Form11();
                case "1.2": return new Form12();
                case "1.3": return new Form13();
                case "1.4": return new Form14();
                case "1.5": return new Form15();
                case "1.6": return new Form16();
                case "1.7": return new Form17();
                case "1.8": return new Form18();
                case "1.9": return new Form19();

                case "2.0": return new Form20();
                case "2.1": return new Form21();
                case "2.2": return new Form22();
                case "2.3": return new Form23();
                case "2.4": return new Form24();
                case "2.5": return new Form25();
                case "2.6": return new Form26();
                case "2.7": return new Form27();
                case "2.8": return new Form28();
                case "2.9": return new Form29();
                case "2.10": return new Form210();
                case "2.11": return new Form211();
                case "2.12": return new Form212();

                //case "30": return new Form30();
                //case "31": return new Form31();
                //case "32": return new Form32();

                //case "40": return new Form40();
                //case "41": return new Form41();

                //case "50": return new Form50();
                //case "51": return new Form51();
                //case "52": return new Form52();
                //case "53": return new Form53();
                //case "54": return new Form54();
                //case "55": return new Form55();
                //case "56": return new Form56();
                //case "57": return new Form57();

                default: return null;
            }
        }
    }
}
