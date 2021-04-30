using System.Collections.Generic;

namespace Client_App.PasteRealization
{
    public class Excel : IPaste
    {
        public List<Models.Abstracts.Form> Convert(string Data, string Form)
        {
            List<Models.Abstracts.Form> lst = new List<Models.Abstracts.Form>();
            //var form = FormCreator.Create(Form);

            //List<string[]> slst = new List<string[]>();
            //string[] mas = new string[form.NumberOfFields+1];
            //mas[0] = Form;

            //int Row = 0;
            //int Column = 1;

            //string rt = "";

            //foreach(var item in Data)
            //{
            //    if (item == '\n')
            //    {
            //        mas[Column] = rt;
            //        slst.Add(mas);
            //        Row++;
            //        Column = 0;
            //        mas=new string[form.NumberOfFields+1];
            //        mas[0] = Form;
            //        rt = "";
            //    }
            //    else
            //    {
            //        if (item == '\t')
            //        {
            //            mas[Column] = rt;
            //            Column++;
            //            rt = "";
            //        }
            //        else
            //        {
            //            if(item!='\r')
            //            {
            //                rt += item;
            //            }
            //        }
            //    }
            //}

            //foreach(var item in slst)
            //{
            //    //Form frm = FormCreator.Create(item);
            //    //lst.Add(form);
            //}

            return lst;

        }
        public string ConvertBack(Models.Abstracts.Form[] Param)
        {
            return "";
        }
    }
}
