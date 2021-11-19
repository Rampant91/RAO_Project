using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.IO;

namespace Models.DBRealization
{
    public class DBExportModel : DataContext
    {
        public DBExportModel(string Path = "") : base(Path)
        {

        }
    }
}
