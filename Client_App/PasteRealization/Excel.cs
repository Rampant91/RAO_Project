using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Client_Model;

namespace Client_App.PasteRealization
{
    public class Excel:IPaste
    {
        public List<Form> Convert(string Data,string Form)
        {
            List<Form> lst = new List<Form>();
            var form = FormCreator.Create(Form);

            List<string[]> slst = new List<string[]>();
            string[] mas = new string[form.NumberOfFields];

            int Row = 0;
            int Column = 0;

            string rt = "";

            foreach(var item in Data)
            {
                if (item == '\n')
                {
                    mas[Column] = rt;
                    slst.Add(mas);
                    Row++;
                    Column = 0;
                    mas=new string[form.NumberOfFields];
                    rt = "";
                }
                else
                {
                    if (item == '\t')
                    {
                        mas[Column] = rt;
                        Column++;
                        rt = "";
                    }
                    else
                    {
                        if(item!='\r')
                        {
                            rt += item;
                        }
                    }
                }
            }

            foreach(var item in slst)
            {
                Form frm = FormCreator.Create(Form,item);
                lst.Add(form);
            }

            return lst;

        }
        public string ConvertBack(Form[] Param)
        {
            return "";
        }
    }
}
