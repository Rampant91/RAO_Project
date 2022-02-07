using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBRealization.DBAPIFactory
{
    public interface IEssence
    {
        T Post<T>(T obj) where T : class, IKey;
        void Get(int ID);
        void Update(int ID);
        void Delete(int ID);

        //void PostAsync<T>() where T : class;
        //void GetAsync<T>(int ID) where T : class;
        //void UpdateAsync<T>(int ID) where T : class;
        //void DeleteAsync<T>(int ID) where T : class;

    }
}
