﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading.Tasks;
using Models.Collections;
using Models.Forms;
using Models.Forms.Form1;
using Models.Forms.Form2;

namespace Models.DBRealization;

public class DataContext : DbContext
{
    public string _path { get; set; }
    public DataContext(string Path = "")
    {
        if (Path == "")
        {
            var system = Environment.GetFolderPath(Environment.SpecialFolder.System);
            var path = System.IO.Path.GetPathRoot(system);
            var tmp = System.IO.Path.Combine(path, "RAO");
            tmp = System.IO.Path.Combine(tmp, "Local_temp.raodb");
            _path = tmp;
        }
        else
        {
            _path = Path;
        }
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        var lt = RedDataBaseCreation.GetConnectionString(_path);
        optionsBuilder.UseFirebird(lt);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DBObservable>()
            .ToTable("DBObservable_DbSet");

        modelBuilder.Entity<Reports>()
            .ToTable("ReportsCollection_DbSet");
        modelBuilder.Entity<Report>()
            .ToTable("ReportCollection_DbSet");
        modelBuilder.Entity<Note>()
            .ToTable("notes");

        modelBuilder.Entity<Form10>()
            .ToTable("form_10");
        modelBuilder.Entity<Form11>()
            .ToTable("form_11");
        modelBuilder.Entity<Form12>()
            .ToTable("form_12");
        modelBuilder.Entity<Form13>()
            .ToTable("form_13");
        modelBuilder.Entity<Form14>()
            .ToTable("form_14");
        modelBuilder.Entity<Form15>()
            .ToTable("form_15");
        modelBuilder.Entity<Form16>()
            .ToTable("form_16");
        modelBuilder.Entity<Form17>()
            .ToTable("form_17");
        modelBuilder.Entity<Form18>()
            .ToTable("form_18");
        modelBuilder.Entity<Form19>()
            .ToTable("form_19");

        modelBuilder.Entity<Form20>()
            .ToTable("form_20");
        modelBuilder.Entity<Form21>()
            .ToTable("form_21");
        modelBuilder.Entity<Form22>()
            .ToTable("form_22");
        modelBuilder.Entity<Form23>()
            .ToTable("form_23");
        modelBuilder.Entity<Form24>()
            .ToTable("form_24");
        modelBuilder.Entity<Form25>()
            .ToTable("form_25");
        modelBuilder.Entity<Form26>()
            .ToTable("form_26");
        modelBuilder.Entity<Form27>()
            .ToTable("form_27");
        modelBuilder.Entity<Form28>()
            .ToTable("form_28");
        modelBuilder.Entity<Form29>()
            .ToTable("form_29");
        modelBuilder.Entity<Form210>()
            .ToTable("form_210");
        modelBuilder.Entity<Form211>()
            .ToTable("form_211");
        modelBuilder.Entity<Form212>()
            .ToTable("form_212");
    }

    public void LoadTables()
    {
        notes.Load();
        form_10.Load();
        form_11.Load();
        form_12.Load();
        form_13.Load();
        form_14.Load();
        form_15.Load();
        form_16.Load();
        form_17.Load();
        form_18.Load();
        form_19.Load();

        form_20.Load();
        form_21.Load();
        form_22.Load();
        form_23.Load();
        form_24.Load();
        form_25.Load();
        form_26.Load();
        form_27.Load();
        form_28.Load();
        form_29.Load();
        form_210.Load();
        form_211.Load();
        form_212.Load();

        ReportCollectionDbSet.Load();
        ReportsCollectionDbSet.Load();
        DBObservableDbSet.Load();
    }
    public async Task LoadTablesAsync()
    {
        try
        {
            await notes.LoadAsync();

            await form_10.LoadAsync();
            await form_11.LoadAsync();
            await form_12.LoadAsync();
            await form_13.LoadAsync();
            await form_14.LoadAsync();
            await form_15.LoadAsync();
            await form_16.LoadAsync();
            await form_17.LoadAsync();
            await form_18.LoadAsync();
            await form_19.LoadAsync();

            await form_20.LoadAsync();
            try
            {
                await form_21.LoadAsync();
                await form_22.LoadAsync();
            }
            catch
            {
                form_21.Local.Clear();
                form_22.Local.Clear();
            }
            await form_23.LoadAsync();
            await form_24.LoadAsync();
            await form_25.LoadAsync();
            await form_26.LoadAsync();
            await form_27.LoadAsync();
            await form_28.LoadAsync();
            await form_29.LoadAsync();
            await form_210.LoadAsync();
            await form_211.LoadAsync();
            await form_212.LoadAsync();

            await ReportCollectionDbSet.LoadAsync();
            await ReportsCollectionDbSet.LoadAsync();
            await DBObservableDbSet.LoadAsync();
        }
        catch
        {
            //ignored
        }
    }

    public async Task LoadNotesAsync()
    {
        try
        {
            await notes.LoadAsync();
        }
        catch
        {
            //ignored
        }
    }

    public void Restore()
    {
        var changedEntries = ChangeTracker
            .Entries()
            .Where(entry => entry.State != EntityState.Unchanged)
            .ToList();
        foreach (var entry in changedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
                
            }
        }
    }

    public DbSet<DBObservable> DBObservableDbSet { get; set; }
    public DbSet<Reports> ReportsCollectionDbSet { get; set; }
    public DbSet<Report> ReportCollectionDbSet { get; set; }
    public DbSet<Note> notes { get; set; }

    public DbSet<Form10> form_10 { get; set; }
    public DbSet<Form11> form_11 { get; set; }
    public DbSet<Form12> form_12 { get; set; }
    public DbSet<Form13> form_13 { get; set; }
    public DbSet<Form14> form_14 { get; set; }
    public DbSet<Form15> form_15 { get; set; }
    public DbSet<Form16> form_16 { get; set; }
    public DbSet<Form17> form_17 { get; set; }
    public DbSet<Form18> form_18 { get; set; }
    public DbSet<Form19> form_19 { get; set; }

    public DbSet<Form20> form_20 { get; set; }
    public DbSet<Form21> form_21 { get; set; }
    public DbSet<Form22> form_22 { get; set; }
    public DbSet<Form23> form_23 { get; set; }
    public DbSet<Form24> form_24 { get; set; }
    public DbSet<Form25> form_25 { get; set; }
    public DbSet<Form26> form_26 { get; set; }
    public DbSet<Form27> form_27 { get; set; }
    public DbSet<Form28> form_28 { get; set; }
    public DbSet<Form29> form_29 { get; set; }
    public DbSet<Form210> form_210 { get; set; }
    public DbSet<Form211> form_211 { get; set; }
    public DbSet<Form212> form_212 { get; set; }
}