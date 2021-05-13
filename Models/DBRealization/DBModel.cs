using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace DBRealization
{
    public class DBModel : DbContext
    {
        public DBModel() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connectionString = "database=localhost:demo.fdb;user=sysdba;password=masterkey";
            optionsBuilder.UseFirebird(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var demoConf = modelBuilder.Entity<Models.Form11>();
            demoConf.HasKey(x => x.RowID);
            demoConf.ToTable("DEMO");
        }

        public DbSet<Models.Form11> forms_11 { get; set; }
        //public DbSet<Post> Posts { get; set; }
    }
}
