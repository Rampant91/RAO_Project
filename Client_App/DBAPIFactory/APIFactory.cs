using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_App.DBAPIFactory;

public static partial class EssenceMethods
{
    public class APIFactory<T> where T : class
    {
        #region GetObjectByType

        private static IEssenceMethods<T> GetObjectByType()
        {
            var name = typeof(T).Name;
            try
            {
                if (MethodsList.TryGetValue(name, out var value))
                {
                    return (IEssenceMethods<T>)value.Invoke();
                }
                throw new Exception("No this type(" + name + ") implemented");
            }
            catch
            {
                throw new Exception("Error while creating API methods with type: " + name);
            }
        }

        #endregion

        #region MethodsNotAsync

        public T Post(T obj)
        {
            return GetObjectByType().Post(obj);
        }

        public T Get(int id)
        {
            return GetObjectByType().Get(id);
        }
        public List<T> GetAll()
        {
            return GetObjectByType().GetAll();
        }
        public bool Update(T obj)
        {
            return GetObjectByType().Update(obj);
        }
        public bool Delete(int id)
        {
            return GetObjectByType().Delete(id);
        }

        #endregion

        #region MethodsAsync

        public async Task<T> PostAsync(T obj)
        {
            return await GetObjectByType().PostAsync(obj);
        }

        public async Task<T> GetAsync(int id)
        {
            return await GetObjectByType().GetAsync(id);
        }
        public async Task<List<T>> GetAllAsync(string param = "")
        {
            return await GetObjectByType().GetAllAsync(param);
        }
        public async Task<bool> UpdateAsync(T obj)
        {
            return await GetObjectByType().UpdateAsync(obj);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await GetObjectByType().DeleteAsync(id);
        }

        #endregion
    }

    public class APIFactory
    {
        public APIFactory(Type T)
        {
            InnerType = T;
        }
        public Type InnerType { get; set; }

        #region GetObjectByType

        private IEssenceMethods<T> GetObjectByType<T>() where T : class
        {
            var name = InnerType.Name;
            try
            {
                if (MethodsList.TryGetValue(name, out var value))
                {
                    return (IEssenceMethods<T>)value.Invoke();
                }
                throw new Exception("No this type(" + name + ") implemented");
            }
            catch
            {
                throw new Exception("Error while creating API methods with type: " + name);
            }
        }

        #endregion

        #region MethodsNotAsync

        public T Post<T>(T obj) where T : class
        {
            return GetObjectByType<T>().Post(obj);
        }

        public T Get<T>(int id) where T : class
        {
            return GetObjectByType<T>().Get(id);
        }
        public List<T> GetAll<T>() where T : class
        {
            return GetObjectByType<T>().GetAll();
        }
        public bool Update<T>(T obj) where T : class
        {
            return GetObjectByType<T>().Update(obj);
        }
        public bool Delete<T>(int id) where T : class
        {
            return GetObjectByType<T>().Delete(id);
        }

        #endregion

        #region MethodsAsync

        public async Task<T> PostAsync<T>(T obj) where T : class
        {
            return await GetObjectByType<T>().PostAsync(obj);
        }

        public async Task<T> GetAsync<T>(int id) where T : class
        {
            return await GetObjectByType<T>().GetAsync(id);
        }
        public async Task<List<T>> GetAllAsync<T>() where T : class
        {
            return await GetObjectByType<T>().GetAllAsync();
        }
        public async Task<bool> UpdateAsync<T>(T obj) where T : class
        {
            return await GetObjectByType<T>().UpdateAsync(obj);
        }
        public async Task<bool> DeleteAsync<T>(int id) where T : class
        {
            return await GetObjectByType<T>().DeleteAsync(id);
        }

        #endregion
    }
}