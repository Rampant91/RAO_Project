using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Collections;

namespace Models.DBRealization.DBAPIFactory
{
    public static partial class EssanceMethods
    {
        protected interface IEssenceMethods
        {
            #region NotAsync
            T Post<T>(T obj) where T:class;
            T Get<T>(int ID) where T : class;
            List<T> GetAll<T>() where T : class;
            bool Update<T>(T obj) where T : class;
            bool Delete<T>(int ID) where T : class;
            #endregion

            #region Async
            Task<T> PostAsync<T>(T obj) where T : class;
            Task<T> GetAsync<T>(int ID) where T : class;
            Task<List<T>> GetAllAsync<T>() where T : class;
            Task<bool> UpdateAsync<T>(T obj) where T : class;
            Task<bool> DeleteAsync<T>(int ID) where T : class;
            #endregion
        }
    }
}
