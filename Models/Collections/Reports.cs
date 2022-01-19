using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using Models.DBRealization;
using Models;
using Models.DataAccess;
using OfficeOpenXml;
using System;

namespace Models.Collections
{

    public class Reports : IKey,INumberInOrder,IDataGridColumn
    {
        [NotMapped]
        public long Order
        {
            get
            {
                throw new NotImplementedException();
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

        public Report Master_DB { get; set; }

        [NotMapped]
        public Report Master
        {
            get
            {
                return Master_DB;
            }
            set
            {
                Master_DB = value;
                OnPropertyChanged(nameof(Master));
            }
        }

        ObservableCollectionWithItemPropertyChanged<Report> Report_Collection_DB;

        public ObservableCollectionWithItemPropertyChanged<Report> Report_Collection
        {
            get
            {
                return Report_Collection_DB;
            }
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

        public event PropertyChangedEventHandler PropertyChanged;

        public int Id { get; set; }

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

        private bool Master_Validation(RamAccess<Report> value)
        {
            return true;
        }

        private bool Report_Collection_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Report>> value)
        {
            return true;
        }

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //Property Changed

        public int ExcelRow(ExcelWorksheet worksheet,int Row, int Column, bool Tanspon = true)
        {
            throw new System.NotImplementedException();
        }

        public int ExcelHeader(ExcelWorksheet worksheet, int Row,int Column,bool Transpon=true)
        {
            throw new System.NotImplementedException();
        }

        #region IDataGridColumn
        public DataGridColumns GetColumnStructure()
        {
            DataGridColumns dataGridColumns = new();
            dataGridColumns.innertCol = new System.Collections.Generic.List<DataGridColumns>();

            DataGridColumns regNoR = new();
            regNoR.SizeCol = 50;
            regNoR.binding = nameof(Report.RegNoRep);
            regNoR.name = ((Attributes.Form_PropertyAttribute)typeof(Form10).GetProperty("RegNo").GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).Name;
            dataGridColumns.innertCol.Add(regNoR);

            DataGridColumns okpoR = new();
            okpoR.SizeCol = 50;
            okpoR.binding = nameof(Report.OkpoRep);
            okpoR.name = ((Attributes.Form_PropertyAttribute)typeof(Form10).GetProperty("Okpo").GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).Name;
            dataGridColumns.innertCol.Add(okpoR);

            DataGridColumns shortJurLicoR = new();
            shortJurLicoR.SizeCol = 50;
            shortJurLicoR.binding = nameof(Report.ShortJurLicoRep);
            shortJurLicoR.name = ((Attributes.Form_PropertyAttribute)typeof(Form10).GetProperty("ShortJurLico").GetCustomAttributes(typeof(Attributes.Form_PropertyAttribute), true).FirstOrDefault()).Name;
            dataGridColumns.innertCol.Add(shortJurLicoR);

            return dataGridColumns;
        }
        #endregion
    }
}