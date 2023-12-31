﻿using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Models.Collections;

public class DBObservable : INotifyPropertyChanged
{
    [NotMapped] private bool _isChanged = true;

    public int Id { get; set; }

    public DBObservable()
    {
        Reports_Collection_DB = new ObservableCollectionWithItemPropertyChanged<Reports>();
        Reports_Collection.CollectionChanged += CollectionChanged;
    }

    #region Reports_Collection
    ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection_DB;
    public virtual ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection
    {
        get => Reports_Collection_DB;
        set
        {
            Reports_Collection_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Reports_Collection));
        OnPropertyChanged(nameof(Reports_Collection10));
        OnPropertyChanged(nameof(Reports_Collection20));
    }
    #endregion

    #region Reports_Collection10
    [NotMapped]
    public virtual ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection10
    {
        get
        {
            var sm = Reports_Collection_DB.Where(t => t.Master.FormNum.Value == "1.0");
            var obj = new ObservableCollectionWithItemPropertyChanged<Reports>(sm);
            return obj;
        }
    }
    #endregion

    #region Reports_Collection20
    [NotMapped]
    public virtual ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection20
    {
        get
        {
            var sm = Reports_Collection_DB.Where(t => t.Master.FormNum.Value == "2.0");
            var obj = new ObservableCollectionWithItemPropertyChanged<Reports>(sm);
            return obj;
        }
    }
    #endregion

    private static bool Reports_Collection_Validation(DbSet<Reports> value)
    {
        return true;
    }

    //Property Changed
    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    public event PropertyChangedEventHandler PropertyChanged;
}