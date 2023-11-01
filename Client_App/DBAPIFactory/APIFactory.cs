using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client_App.DBAPIFactory
{
    public static partial class EssenceMethods
    {
        public class APIFactory<T> where T : class
        {
            #region GetObjectByType
            private EssenceMethods.IEssenceMethods<T> GetObjectByType()
            {
                var name = typeof(T).Name;
                try
                {
                    if (EssenceMethods.MethodsList.ContainsKey(name))
                    {
                        return (EssenceMethods.IEssenceMethods<T>)EssenceMethods.MethodsList[name].Invoke();
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

            public T Get(int ID)
            {
                return GetObjectByType().Get(ID);
            }
            public List<T> GetAll()
            {
                return GetObjectByType().GetAll();
            }
            public bool Update(T obj)
            {
                return GetObjectByType().Update(obj);
            }
            public bool Delete(int ID)
            {
                return GetObjectByType().Delete(ID);
            }
            #endregion

            #region MethodsAsync
            public async Task<T> PostAsync(T obj)
            {
                return await GetObjectByType().PostAsync(obj);
            }

            public async Task<T> GetAsync(int ID)
            {
                return await GetObjectByType().GetAsync(ID);
            }
            public async Task<List<T>> GetAllAsync(string param = "")
            {
                return await GetObjectByType().GetAllAsync(param);
            }
            public async Task<bool> UpdateAsync(T obj)
            {
                return await GetObjectByType().UpdateAsync(obj);
            }
            public async Task<bool> DeleteAsync(int ID)
            {
                return await GetObjectByType().DeleteAsync(ID);
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
            private EssenceMethods.IEssenceMethods<T> GetObjectByType<T>() where T : class
            {
                var name = InnerType.Name;
                try
                {
                    if (EssenceMethods.MethodsList.ContainsKey(name))
                    {
                        return (EssenceMethods.IEssenceMethods<T>)EssenceMethods.MethodsList[name].Invoke();
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

            public T Get<T>(int ID) where T : class
            {
                return GetObjectByType<T>().Get(ID);
            }
            public List<T> GetAll<T>() where T : class
            {
                return GetObjectByType<T>().GetAll();
            }
            public bool Update<T>(T obj) where T : class
            {
                return GetObjectByType<T>().Update(obj);
            }
            public bool Delete<T>(int ID) where T : class
            {
                return GetObjectByType<T>().Delete(ID);
            }
            #endregion

            #region MethodsAsync
            public async Task<T> PostAsync<T>(T obj) where T : class
            {
                return await GetObjectByType<T>().PostAsync(obj);
            }

            public async Task<T> GetAsync<T>(int ID) where T : class
            {
                return await GetObjectByType<T>().GetAsync(ID);
            }
            public async Task<List<T>> GetAllAsync<T>() where T : class
            {
                return await GetObjectByType<T>().GetAllAsync();
            }
            public async Task<bool> UpdateAsync<T>(T obj) where T : class
            {
                return await GetObjectByType<T>().UpdateAsync(obj);
            }
            public async Task<bool> DeleteAsync<T>(int ID) where T : class
            {
                return await GetObjectByType<T>().DeleteAsync(ID);
            }
            #endregion
        }
    }
}
