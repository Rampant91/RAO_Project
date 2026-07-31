using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Forms.Forms1.Items
{
    public class BaseItem <T1,T2>
    {
        public T1 Code { get; set; }
        public T2 Description { get; set; }

        public bool DescriptionPrintMode { get; set; }  = false;

        public override string ToString()
        {
            if (DescriptionPrintMode)
                return Description?.ToString() ?? "";
            else
                return Code?.ToString() ?? "";
        }
    }

}
