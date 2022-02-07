using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBRealization.DBAPIFactory
{
    internal class ReportEssence : IEssence
    {
        public T Post<T>(T obj) where T : class, IKey
        {
            return null;
        }
        public void Get(int ID) { }
        public void Update(int ID) { }
        public void Delete(int ID) { }
    }
}
