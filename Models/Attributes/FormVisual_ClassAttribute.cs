using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Attributes
{
    public class FormVisual_ClassAttribute : System.Attribute
    {
        public string Name { get; set; }
        public FormVisual_ClassAttribute(string Name)
        {
            this.Name = Name;
        }
    }
}
