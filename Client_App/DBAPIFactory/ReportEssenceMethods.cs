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
    protected class ReportEssenceMethods : IEssenceMethods<Report>
    {
        protected static Type InnerType { get; } = typeof(Report);
        public static ReportEssenceMethods GetMethods()
        {
            return new ReportEssenceMethods();
        }
        public ReportEssenceMethods() { }

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
            if (!CheckType(obj) || (obj as Report).Id != 0) return null;
            using var db = new DBModel(StaticConfiguration.DBPath);
            db.Database.Migrate();
            db.ReportCollectionDbSet.Add(obj as Report);
            db.SaveChanges();
            return obj;
        }

        #endregion

        #region Get
        T? IEssenceMethods.Get<T>(int id) where T : class
        {
            if (!CheckType(typeof(T))) return null;
            using var db = new DBModel(StaticConfiguration.DBPath);
            db.Database.Migrate();
            return db.ReportCollectionDbSet
                .AsNoTracking()
                .Where(x => x.Id == id)
                .OrderBy(x => x.Order)
                .Include(x => x.Rows10)
                .Include(x => x.Rows11.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows12.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows13.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows14.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows15.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows16.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows17.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows18.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows19.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows20.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows21.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows22.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows23.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows24.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows25.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows26.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows27.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows28.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows29.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows210.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows211.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Rows212.OrderBy(x => x.NumberInOrder_DB))
                .Include(x => x.Notes.OrderBy(x => x.Order))
                .FirstOrDefault() as T;
        }

        #endregion

        #region GetAll
        List<T> IEssenceMethods.GetAll<T>() where T : class
        {
            if (CheckType(typeof(T)))
            {
                //DoSomething
            }
            return null;
        }

        #endregion

        #region Update
        bool IEssenceMethods.Update<T>(T obj) where T : class
        {
            if (!CheckType(obj)) return false;
            using var db = new DBModel(StaticConfiguration.DBPath);
            db.Database.Migrate();
            var _rep = obj as Report;
            db.ReportCollectionDbSet.Update(_rep);
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
            var rep = db.ReportCollectionDbSet.FirstOrDefault(x => x.Id == id);
            db.ReportCollectionDbSet.Remove(rep);
            db.SaveChanges();
            return true;
        }
        #endregion

        #endregion

        #region MethodsRealizationAsync

        #region PostAsync
        async Task<T> IEssenceMethods.PostAsync<T>(T obj) where T : class
        {
            if (!CheckType(obj) || (obj as Report).Id != 0) return null;
            await using var db = new DBModel(StaticConfiguration.DBPath);
            await db.Database.MigrateAsync(ReportsStorage.cancellationToken);
            await db.ReportCollectionDbSet.AddAsync(obj as Report, ReportsStorage.cancellationToken);
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
                tmp = await db.ReportCollectionDbSet
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Where(x => x.Id == id)
                    .Include(x => x.Reports).ThenInclude(x => x.DBObservable)
                    .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Reports).ThenInclude(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Rows10.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows11.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows12.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows13.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows14.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows15.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows16.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows17.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows18.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows19.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows20.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows21.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows22.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows23.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows24.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows25.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows26.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows27.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows28.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows29.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows210.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows211.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Rows212.OrderBy(x => x.NumberInOrder_DB))
                    .Include(x => x.Notes.OrderBy(x => x.Order))
                    .Where(x => x.Reports != null && x.Reports.DBObservable != null && x.Id == id)
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
        async Task<List<T?>> IEssenceMethods.GetAllAsync<T>(string param) where T : class
        {
            if (!CheckType(typeof(T))) return null;
            await using var db = new DBModel(StaticConfiguration.DBPath);
            List<T?> tmp = new();
            try
            {
                await db.Database.MigrateAsync(ReportsStorage.cancellationToken);
                switch (param)
                {
                    #region 1

                    case "1":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.Rows10.Count != 0 || x.Rows10.Count != null)
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows11)
                            .Include(x => x.Rows12)
                            .Include(x => x.Rows13)
                            .Include(x => x.Rows14)
                            .Include(x => x.Rows15)
                            .Include(x => x.Rows16)
                            .Include(x => x.Rows17)
                            .Include(x => x.Rows18)
                            .Include(x => x.Rows19)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.1

                    case "1.1":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows11)
                            .Include(x => x.Notes)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.2

                    case "1.2":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows12)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.3

                    case "1.3":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows13)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.4

                    case "1.4":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows14)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.5

                    case "1.5":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows15)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.6

                    case "1.6":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows16)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.7

                    case "1.7":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows17)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.8

                    case "1.8":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows18)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 1.9

                    case "1.9":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows10)
                            .Include(x => x.Rows19)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2

                    case "2":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.Rows20.Count != 0 || x.Rows20.Count != null)
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows21)
                            .Include(x => x.Rows22)
                            .Include(x => x.Rows23)
                            .Include(x => x.Rows24)
                            .Include(x => x.Rows25)
                            .Include(x => x.Rows26)
                            .Include(x => x.Rows27)
                            .Include(x => x.Rows28)
                            .Include(x => x.Rows29)
                            .Include(x => x.Rows210)
                            .Include(x => x.Rows211)
                            .Include(x => x.Rows212)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.1

                    case "2.1":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows21)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.2

                    case "2.2":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows22)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.3

                    case "2.3":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows23)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.4

                    case "2.4":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows24)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.5

                    case "2.5":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows25)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.6

                    case "2.6":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows26)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.7

                    case "2.7":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows27)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.8

                    case "2.8":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows28)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.9

                    case "2.9":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows29)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.10

                    case "2.10":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows210)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.11

                    case "2.11":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows211)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    #region 2.12

                    case "2.12":
                        tmp = await ((IQueryable<Report>)db.ReportCollectionDbSet)
                            .Where(x => x.FormNum_DB.Equals(param))
                            .Include(x => x.Rows20)
                            .Include(x => x.Rows212)
                            .Select(x => x as T)
                            .ToListAsync(ReportsStorage.cancellationToken);
                        break;

                    #endregion

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                if (ReportsStorage.cancellationToken.IsCancellationRequested)
                    ServiceExtension.LoggerManager.Info(ex.Message);
                else
                    ServiceExtension.LoggerManager.Warning($"{ex.Message}{Environment.NewLine}{ex.StackTrace}");
            }

            return tmp;
        }

        #endregion

        #region UpdateAsync

        async Task<bool> IEssenceMethods.UpdateAsync<T>(T obj) where T : class
        {
            if (!CheckType(obj)) return false;
            await using var db = new DBModel(StaticConfiguration.DBPath);
            await db.Database.MigrateAsync(ReportsStorage.cancellationToken);
            var _rep = obj as Report;
            db.ReportCollectionDbSet.Update(_rep);
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
            var rep = await db.ReportCollectionDbSet
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(ReportsStorage.cancellationToken);
            if (rep != null)
            {
                db.ReportCollectionDbSet.Remove(rep);
                await db.SaveChangesAsync(ReportsStorage.cancellationToken);
            }
            return true;
        }
        #endregion

        #endregion
    }
}