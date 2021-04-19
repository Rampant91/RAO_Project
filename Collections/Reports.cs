using System;
using System.Collections.Generic;
using System.Text;
using Models;

namespace Collections
{
    public class Reports
    {
        IDataAccess _dataAccess { get; set; }

        public Reports(IDataAccess Access)
        {
            _dataAccess = Access;

        }
    }
}
