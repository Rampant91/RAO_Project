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
        public static async Task<ObservableCollectionWithItemPropertyChanged<IKey>> GetData10MainAsync()
        {
            using (var db = new DBModel(StaticConfiguration.DBPath))
            {
                await db.Database.MigrateAsync();
                return new ObservableCollectionWithItemPropertyChanged<IKey>(await db.ReportsCollectionDbSet
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
    }
}
