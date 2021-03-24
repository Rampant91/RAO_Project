using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public abstract class Form4:Form
    {
        [Attributes.FormVisual("Форма")]
        public abstract string FormNum { get; }
        public abstract int NumberOfFields { get; }
    }
}
