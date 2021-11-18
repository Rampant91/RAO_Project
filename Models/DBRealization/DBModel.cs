using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Models.DataAccess;
using Models;

namespace Models.DBRealization
{
    public class DBModel : DataContext
    {
        public DBModel(string Path = "") : base(Path)
        {

        }
    }
}
