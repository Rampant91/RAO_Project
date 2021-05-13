using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using FirebirdSql.Data.FirebirdClient;
using System.IO;
using DBRealization;
using System.Collections.ObjectModel;

namespace DBRealization
{
    public class DBModel : DbContext
    {
        string _path{get;set;}
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

            modelBuilder.Entity<Collections.DBObservable>()
                .ToTable("coll_reports");
            modelBuilder.Entity<Collections.Reports>()
                .ToTable("reports");
            modelBuilder.Entity<Collections.Report>()
                .ToTable("report");
            modelBuilder.Entity<Models.Form11>()
                .ToTable("form_11");
            modelBuilder.Entity<Models.Note>()
                .ToTable("note");
        }

        public DbSet<Collections.DBObservable> coll_reports { get; set; }
        public DbSet<Collections.Reports> reports { get; set; }
        public DbSet<Collections.Report> report { get; set; }
        public DbSet<Models.Form11> form_11 { get; set; }
        public DbSet<Models.Note> note { get; set; }
    }
}
