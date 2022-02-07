using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Collections;

namespace Models.DBRealization
{
    public class DBUse
    {
        #region GetData10Main
        public static async Task<ObservableCollectionWithItemPropertyChanged<Reports>> GetData10MainAsync()
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                await db.Database.MigrateAsync();
                return new ObservableCollectionWithItemPropertyChanged<Reports>(await db.ReportsCollectionDbSet
                    .Where(x => x.Master_DB.FormNum_DB == "1.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Report_Collection)
                    .ToListAsync());
            }
        }
        public static ObservableCollectionWithItemPropertyChanged<Reports> GetData10Main()
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                db.Database.Migrate();
                return new ObservableCollectionWithItemPropertyChanged<Reports>(db.ReportsCollectionDbSet
                    .Where(x => x.Master_DB.FormNum_DB == "1.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Report_Collection)
                    .ToList());
            }

        }
        #endregion

        #region GetData20Main
        public static async Task<ObservableCollectionWithItemPropertyChanged<IKey>> GetData20MainAsync()
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                await db.Database.MigrateAsync();
                return new ObservableCollectionWithItemPropertyChanged<IKey>(await db.ReportsCollectionDbSet
                    .Where(x => x.Master_DB.FormNum_DB == "2.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Report_Collection)
                    .ToListAsync());
            }
        }
        public static ObservableCollectionWithItemPropertyChanged<Reports> GetData20Main()
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                db.Database.Migrate();
                return new ObservableCollectionWithItemPropertyChanged<Reports>(db.ReportsCollectionDbSet
                    .Where(x => x.Master_DB.FormNum_DB == "2.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows20)
                    .Include(x => x.Report_Collection)
                    .ToList());
            }

        }
        #endregion

        #region GetDataForm
        public static async Task<Report> GetDataFormAsync(int ID)
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                await db.Database.MigrateAsync();
                return await db.ReportCollectionDbSet
                    .Where(x => x.Id == ID)
                    .FirstOrDefaultAsync();
            }
        }
        public static Report GetDataForm(int ID)
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                db.Database.Migrate();
                return db.ReportCollectionDbSet
                    .Where(x => x.Id == ID)
                    .FirstOrDefault();
            }
        }
        #endregion

        #region GetData10Org
        public static async Task<Reports> GetData10OrgAsync(int ID)
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                await db.Database.MigrateAsync();
                return await db.ReportsCollectionDbSet
                    .Where(x => x.Id == ID && x.Master_DB.FormNum_DB == "1.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .FirstOrDefaultAsync();
            }
        }
        public static Reports GetData10Org(int ID)
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                db.Database.Migrate();
                return db.ReportsCollectionDbSet
                    .Where(x => x.Id == ID && x.Master_DB.FormNum_DB == "1.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .FirstOrDefault();
            }
        }
        #endregion

        #region GetData20Org
        public static async Task<Reports> GetData20OrgAsync(int ID)
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                await db.Database.MigrateAsync();
                return await db.ReportsCollectionDbSet
                    .Where(x => x.Id == ID && x.Master_DB.FormNum_DB == "2.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .FirstOrDefaultAsync();
            }
        }
        public static Reports GetData20Org(int ID)
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                db.Database.Migrate();
                return db.ReportsCollectionDbSet
                    .Where(x => x.Id == ID && x.Master_DB.FormNum_DB == "2.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .FirstOrDefault();
            }
        }
        #endregion


        #region SetData10Main
        public static void SetData10Main(ObservableCollectionWithItemPropertyChanged<Reports> new_reps)
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                db.Database.Migrate();
                var cur_reps = new ObservableCollectionWithItemPropertyChanged<Reports>(db.ReportsCollectionDbSet
                    .Where(x => x.Master_DB.FormNum_DB == "1.0")
                    .Include(x => x.Master_DB).ThenInclude(x => x.Rows10)
                    .Include(x => x.Report_Collection)
                    .ToList());

                foreach (Reports reps in new_reps)
                {
                    var notIn = true;
                    foreach (Reports reps1 in cur_reps)
                    {
                        if (reps.Id == reps1.Id)
                        {
                            cur_reps.Remove(reps1);
                            cur_reps.Add(reps);
                            notIn = false;
                        }
                    }
                    if (notIn)
                    {
                        cur_reps.Add(reps);
                    }
                }

                db.SaveChanges();
            }

        }
        #endregion
    }
}
