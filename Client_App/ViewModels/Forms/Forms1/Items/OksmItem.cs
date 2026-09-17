using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Forms.Forms1.Items
{
    public class OksmItem
    {

        public string Code { get; set; }
        public string Country { get; set; } = string.Empty;


        public override string ToString()
        {
            return Country;
        }
    }
}
