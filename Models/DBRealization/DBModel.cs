using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Models.DataAccess;
using Models;

namespace DBRealization
{
    public class DBModel : DbContext
    {
        public string _path { get; set; }
        public DBModel(string Path)
        {
            _path = Path;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseFirebird(RedDataBaseCreation.GetConnectionString(_path));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Form11>().Property(x => x.OperationCode);
        }

        public void LoadTables()
        {
            accessString.Load();
            notes.Load();
            form_10.Load();
            form_11.Load();
            form_12.Load();
            //form_13.Load();
            //form_14.Load();
            //form_15.Load();
            //form_16.Load();
            //form_17.Load();
            //form_18.Load();
            //form_19.Load();

            //form_20.Load();
            //form_21.Load();
            //form_22.Load();
            //form_23.Load();
            //form_24.Load();
            //form_25.Load();
            //form_26.Load();
            //form_27.Load();
            //form_28.Load();
            //form_29.Load();
            //form_210.Load();
            //form_211.Load();
            //form_212.Load();

            ReportCollectionDbSet.Load();
            ReportsCollectionDbSet.Load();
            DBObservableDbSet.Load();
            var t =this;
        }

        public void UndoChanges()
        {
            var coll = ChangeTracker.Entries().ToList();
            foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entry in coll)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                }
            }
        }

        public DbSet<RamAccess<string>> accessString { get; set; }

        public DbSet<Collections.DBObservable> DBObservableDbSet { get; set; }
        public DbSet<Collections.Reports> ReportsCollectionDbSet { get; set; }
        public DbSet<Collections.Report> ReportCollectionDbSet { get; set; }
        public DbSet<Models.Note> notes { get; set; }

        public DbSet<Models.Form10> form_10 { get; set; }
        public DbSet<Models.Form11> form_11 { get; set; }
        public DbSet<Models.Form12> form_12 { get; set; }
        public DbSet<Models.Form13> form_13 { get; set; }
        public DbSet<Models.Form14> form_14 { get; set; }
        public DbSet<Models.Form15> form_15 { get; set; }
        public DbSet<Models.Form16> form_16 { get; set; }
        public DbSet<Models.Form17> form_17 { get; set; }
        public DbSet<Models.Form18> form_18 { get; set; }
        public DbSet<Models.Form19> form_19 { get; set; }

        public DbSet<Models.Form20> form_20 { get; set; }
        public DbSet<Models.Form21> form_21 { get; set; }
        public DbSet<Models.Form22> form_22 { get; set; }
        public DbSet<Models.Form23> form_23 { get; set; }
        public DbSet<Models.Form24> form_24 { get; set; }
        public DbSet<Models.Form25> form_25 { get; set; }
        public DbSet<Models.Form26> form_26 { get; set; }
        public DbSet<Models.Form27> form_27 { get; set; }
        public DbSet<Models.Form28> form_28 { get; set; }
        public DbSet<Models.Form29> form_29 { get; set; }
        public DbSet<Models.Form210> form_210 { get; set; }
        public DbSet<Models.Form211> form_211 { get; set; }
        public DbSet<Models.Form212> form_212 { get; set; }

        public DbSet<Models.Form30> form_30 { get; set; }
        public DbSet<Models.Form31> form_31 { get; set; }
        public DbSet<Models.Form31_1> form_31_1 { get; set; }
        public DbSet<Models.Form32> form_32 { get; set; }
        public DbSet<Models.Form32_1> form_32_1 { get; set; }
        public DbSet<Models.Form32_2> form_32_2 { get; set; }
        public DbSet<Models.Form32_3> form_32_3 { get; set; }

        public DbSet<Models.Form40> form_40 { get; set; }
        public DbSet<Models.Form41> form_41 { get; set; }

        public DbSet<Models.Form50> form_50 { get; set; }
        public DbSet<Models.Form51> form_51 { get; set; }
        public DbSet<Models.Form52> form_52 { get; set; }
        public DbSet<Models.Form53> form_53 { get; set; }
        public DbSet<Models.Form54> form_54 { get; set; }
        public DbSet<Models.Form55> form_55 { get; set; }
        public DbSet<Models.Form56> form_56 { get; set; }
        public DbSet<Models.Form57> form_57 { get; set; }
    }
}
