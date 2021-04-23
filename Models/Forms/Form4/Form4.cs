using System;
using System.Collections.Generic;
using System.Text;
using DBRealization;
using Collections.Rows_Collection;

namespace Models.Abstracts
{
    public abstract class Form4:Form
    {
        public Form4(IDataAccess Access) : base(Access)
        {

        }
    }
}
