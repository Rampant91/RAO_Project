using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    interface ISumGroup
    {
        #region  Sum
        public bool Sum_DB { get; set; }
        public RamAccess<bool> Sum { get; set; }
        #endregion

        #region  SumGroup
        public bool SumGroup_DB { get; set; }
        public RamAccess<bool> SumGroup { get; set; }
        #endregion
    }
}
