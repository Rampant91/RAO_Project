using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Attributes
{
    public class FormVisualAttribute:System.Attribute
    {
        public string Name { get; set; }

        public FormVisualAttribute(string Name)
        {
            this.Name = Name;  
        }
    }
}
