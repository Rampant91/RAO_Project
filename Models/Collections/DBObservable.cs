using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;

namespace Models.Collections;

[Table(name: "DBObservable_DbSet")]
public class DBObservable : INotifyPropertyChanged
{
    #region Id
    
    [Key]
    public int Id { get; set; }

    #endregion

    #region Constructor
    
    public DBObservable()
    {
        Reports_Collection_DB = new ObservableCollectionWithItemPropertyChanged<Reports>();
        Reports_Collection.CollectionChanged += CollectionChanged;
    }

    private void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Reports_Collection));
        OnPropertyChanged(nameof(Reports_Collection10));
        OnPropertyChanged(nameof(Reports_Collection20));
        OnPropertyChanged(nameof(Reports_Collection40));
    }

    #endregion

    #region Reports_Collection

    private ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection_DB;

    [NotMapped]
    public virtual ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection
    {
        get => Reports_Collection_DB;
        set
        {
            Reports_Collection_DB = value;
            OnPropertyChanged();
        }
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

    #region Reports_Collection40

    [NotMapped]
    public virtual ObservableCollectionWithItemPropertyChanged<Reports> Reports_Collection40
    {
        get
        {
            var sm = Reports_Collection_DB.Where(t => t.Master.FormNum.Value == "4.0");
            var obj = new ObservableCollectionWithItemPropertyChanged<Reports>(sm);
            return obj;
        }
    }

    #endregion

    #region Validation

    private static bool Reports_Collection_Validation(DbSet<Reports> value)
    {
        return true;
    }

    #endregion

    #region PropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion
}