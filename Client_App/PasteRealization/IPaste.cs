using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Client_Model;

namespace Client_App.PasteRealization
{
    public interface IPaste
    {
        public List<Form> Convert(string Data, string Form);
        public string ConvertBack(Form[] Param);
    }
}
