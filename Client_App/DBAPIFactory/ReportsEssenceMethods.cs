using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Client_App.Interfaces.Logger;
using Microsoft.EntityFrameworkCore;
using Models.Collections;
using Models.DBRealization;

namespace Client_App.DBAPIFactory;

public static partial class EssenceMethods
{
    protected class ReportsEssenceMethods : IEssenceMethods<Reports>
    {
        protected static Type InnerType { get; } = typeof(Reports);
        public static ReportsEssenceMethods GetMethods()
        {
            return new ReportsEssenceMethods();
        }
        public ReportsEssenceMethods() { }

        #region CheckType
        private static bool CheckType<T>(T obj)
        {
            return obj.GetType() == InnerType;
        }
        private static bool CheckType(Type Type)
        {
            return Type == InnerType;
        }
        #endregion

        #region MethodsRealizationNotAsync
        #region Post
        T IEssenceMethods.Post<T>(T obj) where T : class
        {
            if (!CheckType(obj) || (obj as Reports).Id != 0) return null;
            using var db = new DBModel(StaticConfiguration.DBPath);
            db.Database.Migrate();
            db.ReportsCollectionDbSet.Add(obj as Reports);
            db.SaveChanges();
            return obj;
        }
        #endregion

        #region Get
        T? IEssenceMethods.Get<T>(int ID) where T : class
        {
            if (!CheckType(typeof(T))) return null;
            using var db = new DBModel(StaticConfiguration.DBPath);
            db.Database.Migrate();
            return db.ReportsCollectionDbSet.Where(x => x.Id == ID)
                .Include(x => x.Master_DB)
                .FirstOrDefault() as T;
        }
        #endregion

        #region GetAll
        List<T?> IEssenceMethods.GetAll<T>() where T : class
        {
            if (!CheckType(typeof(T))) return null;
            using var db = new DBModel(StaticConfiguration.DBPath);
            db.Database.Migrate();
            IQueryable<Reports> dbQ = db.ReportsCollectionDbSet;
            return dbQ
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                .Include(x => x.Report_Collection)
                .Select(x => x as T).ToList();
        }
        #endregion

        #region Update
        bool IEssenceMethods.Update<T>(T obj) where T : class
        {
            if (!CheckType(obj)) return false;
            using var db = new DBModel(StaticConfiguration.DBPath);
            db.Database.Migrate();
            var rep = obj as Reports;
            db.ReportsCollectionDbSet.Update(rep);
            db.SaveChanges();
            return true;
        }
        #endregion

        #region Delete
        bool IEssenceMethods.Delete<T>(int id) where T : class
        {
            if (!CheckType(typeof(T))) return false;
            using var db = new DBModel(StaticConfiguration.DBPath);
            db.Database.Migrate();
            var rep = db.ReportsCollectionDbSet.FirstOrDefault(x => x.Id == id);
            db.ReportsCollectionDbSet.Remove(rep);
            db.SaveChanges();
            return true;
        }
        #endregion
        #endregion

        #region MethodsRealizationAsync

        #region PostAsync

        async Task<T> IEssenceMethods.PostAsync<T>(T obj) where T : class
        {
            if (!CheckType(obj) || (obj as Reports).Id != 0) return null;
            await using var db = new DBModel(StaticConfiguration.DBPath);
            await db.Database.MigrateAsync(ReportsStorage.cancellationToken);
            await db.ReportsCollectionDbSet.AddAsync(obj as Reports, ReportsStorage.cancellationToken);
            await db.SaveChangesAsync(ReportsStorage.cancellationToken);
            return obj;
        }

        #endregion

        #region GetAsync

        async Task<T?> IEssenceMethods.GetAsync<T>(int id) where T : class
        {
            if (!CheckType(typeof(T))) return null;
            await using var db = new DBModel(StaticConfiguration.DBPath);
            var tmp = new object() as T;
            try
            {
                await db.Database.MigrateAsync(ReportsStorage.cancellationToken);
                tmp = await db.ReportsCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Where(x => x.Id == id)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows11)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows12)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows13)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows14)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows15)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows16)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows17)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows18)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows19)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows21)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows22)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows23)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows24)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows25)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows26)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows27)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows28)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows29)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows210)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows211)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Rows212)
                    .Include(x => x.Report_Collection).ThenInclude(x => x.Notes)
                    .FirstOrDefaultAsync(ReportsStorage.cancellationToken) as T;
            }
            catch (Exception ex)
            {
                ServiceExtension.LoggerManager.Error(ex.Message);
            }

            return tmp;
        }

        #endregion

        #region GetAllAsync

        async Task<List<T>> IEssenceMethods.GetAllAsync<T>(string param) where T : class
        {
            if (!CheckType(typeof(T))) return null;
            await using var db = new DBModel(StaticConfiguration.DBPath);
            try
            {
                await db.Database.MigrateAsync(ReportsStorage.cancellationToken);
                IQueryable<Reports> dbQ = db.ReportsCollectionDbSet;
                var tmp = await dbQ
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Report_Collection)
                    .Select(x => x as T)
                    .ToListAsync(ReportsStorage.cancellationToken);
                return tmp;
            }
            catch (Exception ex)
            {
                ServiceExtension.LoggerManager.Error(ex.Message);
            }

            return null;
        }

        #endregion

        #region UpdateAsync

        async Task<bool> IEssenceMethods.UpdateAsync<T>(T obj) where T : class
        {
            if (!CheckType(obj)) return false;
            await using var db = new DBModel(StaticConfiguration.DBPath);
            await db.Database.MigrateAsync(ReportsStorage.cancellationToken);
            db.ReportsCollectionDbSet.Update(obj as Reports);
            await db.SaveChangesAsync(ReportsStorage.cancellationToken);
            return true;
        }

        #endregion

        #region DeleteAsync

        async Task<bool> IEssenceMethods.DeleteAsync<T>(int id) where T : class
        {
            if (!CheckType(typeof(T))) return false;
            await using var db = new DBModel(StaticConfiguration.DBPath);
            await db.Database.MigrateAsync(ReportsStorage.cancellationToken);
            var reps = await db.ReportsCollectionDbSet
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(ReportsStorage.cancellationToken);
            if (reps != null)
            {
                db.ReportsCollectionDbSet.Remove(reps);
                await db.SaveChangesAsync(ReportsStorage.cancellationToken);
            }
            return true;
        } 

        #endregion

        #endregion
    }
}