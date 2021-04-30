using System.Collections.Generic;

namespace Client_App.PasteRealization
{
    public interface IPaste
    {
        public List<Models.Abstracts.Form> Convert(string Data, string Form);
        public string ConvertBack(Models.Abstracts.Form[] Param);
    }
}
