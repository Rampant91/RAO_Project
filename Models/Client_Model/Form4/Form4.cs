using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Client_Model
{
    public abstract class Form4:Form
    {
        [Attributes.FormVisual("Форма")]
        public override string FormNum { get; } = "4";
        public override abstract int NumberOfFields { get; }
    }
}
