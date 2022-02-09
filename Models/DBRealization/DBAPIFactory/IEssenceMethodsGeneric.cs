using Models.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DBRealization.DBAPIFactory
{
    public static partial class EssanceMethods
    {
        protected interface IEssenceMethods<T> : IEssenceMethods where T:class
        {
            #region NotAsync
            T Post(T obj)
            {
                return Post<T>(obj);
            }

            T Get(int ID)
            {
                return Get<T>(ID);
            }
            List<T> GetAll()
            {
                return GetAll<T>();
            }
            bool Update(T obj)
            {
                return Update<T>(obj);
            }
            bool Delete(int ID)
            {
                return Delete<T>(ID);
            }
            #endregion

            #region Async
            async Task<T> PostAsync(T obj)
            {
                return await PostAsync<T>(obj);
            }

            async Task<T> GetAsync(int ID)
            {
                return await GetAsync<T>(ID);
            }
            async Task<List<T>> GetAllAsync()
            {
                return await GetAllAsync<T>();
            }
            async Task<bool> UpdateAsync(T obj)
            {
                return await UpdateAsync<T>(obj);
            }
            async Task<bool> DeleteAsync(int ID)
            {
                return await DeleteAsync<T>(ID);
            }
            #endregion
        }
    }
}
