using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Forms.Forms1.Items
{
    public  class UktItem
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public double? Mass { get; set; } = null;
        public double? Volume { get; set; } = null;

        public bool IsNotNullVolume 
        {
            get => Volume != null;
        }
        public bool IsNotNullMass
        {
            get => Mass != null;
        }

        public override string ToString()
        {
            return Name + " " + Type;
        }
    }
}
