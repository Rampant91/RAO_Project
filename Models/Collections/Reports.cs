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

[Table("ReportsCollection_DbSet")]
[Index(nameof(DBObservable), IsUnique = true)]
public class Reports : IKey, IDataGridColumn
{
    [NotMapped]
    public long Order
    {
        get
        {
            try
            {
                var num_str = "0";
                if (Master_DB.RegNoRep.Value.Length >= 5)
                {
                    num_str = Master_DB.RegNoRep.Value[..5];
                }
                else 
                {
                    num_str = Master_DB.RegNoRep.Value;
                }
                var num_int = Convert.ToInt64(num_str);
                return num_int;
            }
            catch 
            {
                return 0;
            }
            //throw new NotImplementedException();
        }
    }
    public Reports()
    {
        Init();
    }

    private void Init()
    {
        Report_Collection = new ObservableCollectionWithItemPropertyChanged<Report>();
        Report_Collection.CollectionChanged += CollectionChanged;
    }


    [ForeignKey(nameof(DBObservable))]
    public int? DBObservableId { get; set; }

    public virtual DBObservable DBObservable { get; set; }

    [ForeignKey(nameof(Report))]
    public int? Master_DBId { get; set; }

    public Report Master_DB { get; set; }

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

    public void Sort()
    {
        Report_Collection.QuickSort();
    }
    public async Task SortAsync()
    {
        await Report_Collection.QuickSortAsync();
    }

    public event PropertyChangedEventHandler PropertyChanged;

    [Key]
    public int Id { get; set; }

    public void SetOrder(long index) { }

    public void CollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Report_Collection));
    }

    public void CleanIds()
    {
        Id = 0;
        Master.CleanIds();
        foreach (Report item in Report_Collection)
        {
            item.CleanIds();
        }
    }

    private static bool Master_Validation(RamAccess<Report> value)
    {
        return true;
    }

    private static bool Report_Collection_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Report>> value)
    {
        return true;
    }

    //Property Changed
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
    //Property Changed

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
        var regNoR = ((Attributes.FormPropertyAttribute)typeof(Form10).GetProperty(nameof(Form10.RegNo)).GetCustomAttributes(typeof(Attributes.FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
        regNoR.SizeCol = 50;
        regNoR.Binding = $"{nameof(Master)}.{nameof(Report.RegNoRep)}";

        var shortJurLicoR = ((Attributes.FormPropertyAttribute)typeof(Form10).GetProperty(nameof(Form10.ShortJurLico)).GetCustomAttributes(typeof(Attributes.FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
        shortJurLicoR.SizeCol = 603;
        shortJurLicoR.Binding = $"{nameof(Master)}.{nameof(Report.ShortJurLicoRep)}";
        regNoR += shortJurLicoR;

        var okpoR = ((Attributes.FormPropertyAttribute)typeof(Form10).GetProperty(nameof(Form10.Okpo)).GetCustomAttributes(typeof(Attributes.FormPropertyAttribute), true).FirstOrDefault()).GetDataColumnStructureD();
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