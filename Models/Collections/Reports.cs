using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using OfficeOpenXml;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using Models.Interfaces;

namespace Models.Collections;

[Table(name: "ReportsCollection_DbSet")]
[Index(nameof(DBObservable), IsUnique = false, Name = "IX_ReportsCollection_DbSet_DBO~")]
[Index(nameof(Master_DB), IsUnique = false, Name = "IX_ReportsCollection_DbSet_Mas~")]
public class Reports : IKey, IDataGridColumn
{
    #region Constructor

    public Reports()
    {
        Init();
    } 
    

    private void Init()
    {
        Report_Collection = new ObservableCollectionWithItemPropertyChanged<Report>();
        Report_Collection.CollectionChanged += CollectionChanged;
    }

    public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Report_Collection));
    }

    #endregion

    #region DBObservable
    
    [ForeignKey(nameof(DBObservable))]
    public int? DBObservableId { get; set; }

    public virtual DBObservable DBObservable { get; set; }

    #endregion

    #region Master
    
    [ForeignKey(nameof(Report))]
    public int? Master_DBId { get; set; }

    public virtual Report Master_DB { get; set; } 

    [NotMapped]
    public Report Master
    {
        get => Master_DB;
        set
        {
            Master_DB = value;
            OnPropertyChanged(nameof(Master));
        }
    }

    #endregion

    #region Report_Collection
    
    ObservableCollectionWithItemPropertyChanged<Report> Report_Collection_DB;

    public ObservableCollectionWithItemPropertyChanged<Report> Report_Collection
    {
        get => Report_Collection_DB;
        set
        {
            Report_Collection_DB = value;
            OnPropertyChanged(nameof(Report_Collection));
        }
    }

    #endregion

    #region Id
    
    [Key]
    public int Id { get; set; }

    public void CleanIds()
    {
        Id = 0;
        Master.CleanIds();
        foreach (Report item in Report_Collection)
        {
            item.CleanIds();
        }
    }
    
    #endregion

    #region Order
    
    [NotMapped]
    public long Order
    {
        get
        {
            try
            {
                var numStr = Master_DB.RegNoRep.Value.Length >= 5 
                    ? Master_DB.RegNoRep.Value[..5] 
                    : Master_DB.RegNoRep.Value;
                var numInt = Convert.ToInt64(numStr);
                return numInt;
            }
            catch
            {
                return 0;
            }
            //throw new NotImplementedException();
        }
    }

    public void SetOrder(long index) { }

    #endregion

    #region Sort
    
    public void Sort()
    {
        Report_Collection.QuickSort();
    }

    public async Task SortAsync()
    {
        await Report_Collection.QuickSortAsync().ConfigureAwait(false);
    }

    #endregion

    #region ClearAll

    public void ClearAll()
    {
        foreach (var key in Report_Collection)
        {
            var rep = (Report)key;
            rep.Rows11.Clear();
            rep.Rows12.Clear();
            rep.Rows13.Clear();
            rep.Rows14.Clear();
            rep.Rows15.Clear();
            rep.Rows16.Clear();
            rep.Rows17.Clear();
            rep.Rows18.Clear();
            rep.Rows19.Clear();
            rep.Rows21.Clear();
            rep.Rows22.Clear();
            rep.Rows23.Clear();
            rep.Rows24.Clear();
            rep.Rows25.Clear();
            rep.Rows26.Clear();
            rep.Rows27.Clear();
            rep.Rows28.Clear();
            rep.Rows29.Clear();
            rep.Rows210.Clear();
            rep.Rows211.Clear();
            rep.Rows212.Clear();
            rep.Rows41.Clear();
            rep.Rows51.Clear();
            rep.Rows52.Clear();
            rep.Rows53.Clear();
            rep.Rows54.Clear();
            rep.Rows55.Clear();
            rep.Rows56.Clear();
            rep.Rows57.Clear();
        }
        Master.Rows10.Clear();
        Master.Rows20.Clear();
        Master.Rows40.Clear();
        Master.Rows50.Clear();
        Report_Collection.Clear();
    }

    #endregion

    #region Validation

    private static bool Master_Validation(RamAccess<Report> value)
    {
        return true;
    }

    private static bool Report_Collection_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Report>> value)
    {
        return true;
    }

    #endregion

    #region OnPropertyChanged

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }

    #endregion

    #region IExcel

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        throw new NotImplementedException();
    }
    public int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose=true)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region IDataGridColumn

    public DataGridColumns GetColumnStructure(string param = "")
    {
        var regNoR = ((Attributes.FormPropertyAttribute)typeof(Form10)
                .GetProperty(nameof(Form10.RegNo))
                .GetCustomAttributes(typeof(Attributes.FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD();
        regNoR.SizeCol = 50;
        regNoR.Binding = $"{nameof(Master)}.{nameof(Report.RegNoRep)}";

        var shortJurLicoR = ((Attributes.FormPropertyAttribute)typeof(Form10)
                .GetProperty(nameof(Form10.ShortJurLico))
                .GetCustomAttributes(typeof(Attributes.FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD();
        shortJurLicoR.SizeCol = 603;
        shortJurLicoR.Binding = $"{nameof(Master)}.{nameof(Report.ShortJurLicoRep)}";
        regNoR += shortJurLicoR;

        var okpoR = ((Attributes.FormPropertyAttribute)typeof(Form10)
                .GetProperty(nameof(Form10.Okpo))
                .GetCustomAttributes(typeof(Attributes.FormPropertyAttribute), true)
                .FirstOrDefault())
            .GetDataColumnStructureD();
        okpoR.SizeCol = 102;
        okpoR.Binding = $"{nameof(Master)}.{nameof(Report.OkpoRep)}";
        regNoR += okpoR;

        return regNoR;
    }

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        throw new NotImplementedException();
    }

    #endregion
}