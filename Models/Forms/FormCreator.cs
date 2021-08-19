using Models.Abstracts;

namespace Models
{
    public class FormCreator
    {
        public static Abstracts.Form Create(string Param)
        {
            Form tmp = null;
            switch (Param)
            {
                case "1.0": tmp=new Form10(); break;
                case "1.1": tmp=new Form11(); break;
                case "1.2": tmp=new Form12(); break;
                case "1.3": tmp=new Form13(); break;
                case "1.4": tmp=new Form14(); break;
                case "1.5": tmp=new Form15(); break;
                case "1.6": tmp=new Form16(); break;
                case "1.7": tmp=new Form17(); break;
                case "1.8": tmp=new Form18(); break;
                case "1.9": tmp=new Form19(); break;

                case "2.0": tmp=new Form20(); break;
                case "2.1": tmp=new Form21(); break;
                case "2.2": tmp=new Form22(); break;
                case "2.3": tmp=new Form23(); break;
                case "2.4": tmp=new Form24(); break;
                case "2.5": tmp=new Form25(); break;
                case "2.6": tmp=new Form26(); break;
                case "2.7": tmp=new Form27(); break;
                case "2.8": tmp=new Form28(); break;
                case "2.9": tmp=new Form29(); break;
                case "2.10": tmp=new Form210(); break;
                case "2.11": tmp=new Form211(); break;
                case "2.12": tmp=new Form212(); break;
            }

            if (tmp != null)
            {
                tmp.FormNum.Value = Param;
            }

            return tmp;
        }
    }
}
