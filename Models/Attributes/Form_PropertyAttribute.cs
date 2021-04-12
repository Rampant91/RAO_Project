using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Attributes
{
    public class Form_PropertyAttribute:System.Attribute
    {
        public string Name { get; set; }

        public Form_PropertyAttribute(string Name)
        {
            this.Name = Name;  
        }
    }
}
