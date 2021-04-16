using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;

namespace Client_App.PasteRealization
{
    public interface IPaste
    {
        public List<Models.Abstracts.Form> Convert(string Data, string Form);
        public string ConvertBack(Models.Abstracts.Form[] Param);
    }
}
