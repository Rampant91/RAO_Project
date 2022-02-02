using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Metadata;
using Models.Collections;
using Models;
using ReactiveUI;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.LogicalTree;
using Client_App.Controls.DataGrid;
using Models.DBRealization;
using System;
using Models.Attributes;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Models.Abstracts;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Avalonia;
using Client_App.Views;
using DynamicData;
using OfficeOpenXml;
using Spravochniki;
using System.IO;
using System.Threading;
using System.Timers;
using Microsoft.EntityFrameworkCore;
using System.Reactive.Linq;
using Avalonia.Media;

namespace Client_App.ViewModels
{
    public class ChangeOrCreateVM : BaseVM, INotifyPropertyChanged
    {
        public string WindowHeader { get; set; } = "default";
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        private string _FormType;
        public string FormType
        {
            get => _FormType;
            set
            {
                if (_FormType != value)
                {
                    _FormType = value;
                    NotifyPropertyChanged("FormType");
                }
            }
        }

        private Report _Storage;
        public Report Storage
        {
            get => _Storage;
            set
            {
                if (_Storage != value)
                {
                    _Storage = value;
                    NotifyPropertyChanged("Storage");
                }
            }
        }
        private Reports _Storages;
        public Reports Storages
        {
            get => _Storages;
            set
            {
                if (_Storages != value)
                {
                    _Storages = value;
                    NotifyPropertyChanged("Storages");
                }
            }
        }
        private DBObservable _DBO;
        public DBObservable DBO
        {
            get => _DBO;
            set
            {
                if (_DBO != value)
                {
                    _DBO = value;
                    NotifyPropertyChanged("DBO");
                }
            }
        }

        private Form10 _Storage10;
        public Form10 Storage10
        {
            get
            {
                int count = Storage.Rows10.Count;
                return Storage.Rows10[count - 1];
            }
            set
            {
                if (_Storage10 != value)
                {
                    _Storage10 = value;
                    NotifyPropertyChanged("Storage10");
                }
            }
        }

        private Form20 _Storage20;
        public Form20 Storage20
        {
            get
            {
                int count = Storage.Rows20.Count;
                return Storage.Rows20[count - 1];
            }
            set
            {
                if (_Storage20 != value)
                {
                    _Storage20 = value;
                    NotifyPropertyChanged("Storage20");
                }
            }
        }

        #region CheckReport
        public ReactiveCommand<Unit, Unit> CheckReport { get; protected set; }
        private void _CheckReport()
        {
            IsCanSaveReportEnabled = true;
        }
        #endregion

        #region ChangeReportOrder
        public ReactiveCommand<Unit, Unit> ChangeReportOrder { get; protected set; }
        private void _ChangeReportOrder()
        {
            Storage.Rows10.Sorted = false;
            Storage.Rows20.Sorted = false;
            var tmp = Storage.Rows10[0].Order;
            Storage.Rows10[0].SetOrder(Storage.Rows10[1].Order);
            Storage.Rows10[1].SetOrder(tmp);
            Storage.Rows10.QuickSort();

            tmp = Storage.Rows20[0].Order;
            Storage.Rows20[0].SetOrder(Storage.Rows20[1].Order);
            Storage.Rows20[1].SetOrder(tmp);
            Storage.Rows20.QuickSort();
        }
        #endregion

        #region SumRow
        public ReactiveCommand<Unit, Unit> SumRow { get; protected set; }
        private void _SumRow()
        {
            Storage.Rows21.Sum();
            Storage.Rows22.Sum();
        }
        #endregion

        #region AddSort
        public ReactiveCommand<string, Unit> AddSort { get; protected set; }
        #endregion

        #region AddNote
        public ReactiveCommand<object, Unit> AddNote { get; protected set; }
        private async Task _AddNote(object param)
        {
            Note? nt = new Note();
            nt.Order = GetNumberInOrder(Storage.Notes);
            Storage.Notes.Add(nt);
            Storage.Sort();
        }
        #endregion

        #region AddRow
        public ReactiveCommand<object, Unit> AddRow { get; protected set; }
        private async Task _AddRow(object Param)
        {
            var frm = FormCreator.Create(FormType);
            frm.NumberInOrder_DB = GetNumberInOrder(Storage[Storage.FormNum_DB]);
            Storage[Storage.FormNum_DB].Add(frm);
            Storage.Sort();
        }
        #endregion

        #region AddRowIn
        public ReactiveCommand<object, Unit> AddRowIn { get; protected set; }
        private async Task _AddRowIn(object _param)
        {
            var param = (IEnumerable)_param;
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                List<Models.Abstracts.Form> lst = new List<Models.Abstracts.Form>();
                foreach (object? item1 in param)
                {
                    lst.Add((Models.Abstracts.Form)item1);
                }

                var item = lst.FirstOrDefault();

                if (item != null)
                {
                    var number_cell = item.NumberInOrder_DB;
                    var t2 = await ShowDialogIn.Handle(number_cell);

                    if (t2 > 0)
                    {
                        foreach (Form it in Storage[item.FormNum_DB])
                        {
                            if (it.NumberInOrder_DB > number_cell - 1)
                            {
                                it.NumberInOrder.Value = it.NumberInOrder_DB + t2;
                            }
                        }

                        List<Form> _lst = new List<Form>();
                        for (int i = 0; i < t2; i++)
                        {
                            var frm = FormCreator.Create(FormType);
                            frm.NumberInOrder_DB = number_cell;

                            _lst.Add(frm);
                            number_cell++;
                        }
                        Storage[Storage.FormNum_DB].AddRange(_lst);
                        Storage.Sort();
                    }
                }
            }
        }
        #endregion

        #region DeleteRow
        public ReactiveCommand<object, Unit> DeleteRow { get; protected set; }
        private async Task _DeleteRow(object _param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var param = (IEnumerable)_param;
                var answ = await ShowMessageT.Handle(new List<string>() { "Вы действительно хотите удалить строчку?", "Да", "Нет" });
                if (answ == "Да")
                {
                    List<Models.Abstracts.Form> lst = new List<Models.Abstracts.Form>();
                    foreach (object? item in param)
                    {
                        lst.Add((Models.Abstracts.Form)item);
                    }
                    foreach (var item in lst) 
                    {
                        if (item != null) 
                        {
                            foreach (Form it in Storage[item.FormNum_DB])
                            {
                                if (it.NumberInOrder_DB > item.NumberInOrder_DB)
                                {
                                    it.NumberInOrder.Value = it.NumberInOrder_DB - 1;
                                }
                            }
                            Storage[Storage.FormNum_DB].Remove(item);
                        }
                    }
                    //var grp = lst.GroupBy(x => x.NumberInOrder_DB);
                    //foreach (var group in grp)
                    //{
                    //    var item = group.FirstOrDefault();
                    //    if (item != null)
                    //    {
                    //        foreach (Form it in Storage[item.FormNum_DB])
                    //        {
                    //            if (it.NumberInOrder_DB > item.NumberInOrder_DB)
                    //            {
                    //                it.NumberInOrder.Value = it.NumberInOrder_DB - 1;
                    //            }
                    //        }
                    //        Storage[Storage.FormNum_DB].Remove(item);
                    //    }
                    //}
                    Storage.Sort();
                }
            }
        }
        #endregion

        #region DuplicateRowsx1
        public ReactiveCommand<object, Unit> DuplicateRowsx1 { get; protected set; }
        private async Task _DuplicateRowsx1(object param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var t = await ShowDialog.Handle(desktop.MainWindow);
                if (t > 0)
                {
                    List<Form> lst = new List<Form>();
                    var number = GetNumberInOrder(Storage.Rows);
                    for (int i = 0; i < t; i++)
                    {
                        var frm = FormCreator.Create(FormType);
                        frm.NumberInOrder_DB = number;
                        lst.Add(frm);
                        number++;
                    }

                    Storage.Rows.AddRange(lst);
                    Storage.Sort();
                }
            }
        }
        #endregion

        #region DuplicateNotes
        public ReactiveCommand<object, Unit> DuplicateNotes { get; protected set; }
        private async Task _DuplicateNotes(object param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var t = await ShowDialog.Handle(desktop.MainWindow);

                if (t > 0)
                {
                    List<Note> lst = new List<Note>();
                    var r = GetNumberInOrder(Storage.Notes);
                    for (int i = 0; i < t; i++)
                    {
                        var frm = new Note();
                        frm.Order = r;
                        lst.Add(frm);
                        r++;
                    }
                    Storage.Notes.AddRange(lst);
                }
            }
        }
        #endregion

        #region CopyRows
        public ReactiveCommand<object, Unit> CopyRows { get; protected set; }
        private async Task _CopyRows(object _param)
        {
            object[] param = _param as object[];
            IKeyCollection collection = param[0] as IKeyCollection;
            int minColumn = Convert.ToInt32(param[1]) + 1;
            int maxColumn = Convert.ToInt32(param[2]) + 1;

            string txt = "";

            Dictionary<long, Dictionary<int, string>> dic = new Dictionary<long, Dictionary<int, string>>();

            foreach (IKey item in collection.GetEnumerable().OrderBy(x => x.Order))
            {
                dic.Add(item.Order, new Dictionary<int, string>());
                var props = item.GetType().GetProperties();
                foreach (var prop in props)
                {
                    var attr = (Form_PropertyAttribute)prop.GetCustomAttributes(typeof(Form_PropertyAttribute), false).FirstOrDefault();
                    if (attr != null)
                    {
                        try
                        {
                            var columnNum = Convert.ToInt32(attr.Number);
                            if (columnNum >= minColumn && columnNum <= maxColumn)
                            {
                                var midvalue = prop.GetMethod.Invoke(item, null);
                                var value = midvalue.GetType().GetProperty("Value").GetMethod.Invoke(midvalue, null);
                                if (value != null)
                                {
                                    dic[item.Order].Add(columnNum, value.ToString());
                                }
                                else
                                {
                                    dic[item.Order].Add(columnNum, "");
                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }
            }

            foreach (var item in dic.OrderBy(x => x.Key))
            {
                foreach (var it in item.Value.OrderBy(x => x.Key))
                {
                    if (it.Value.Contains('\n') || it.Value.Contains('\r'))
                    {
                        txt += "\"" + it.Value + "\"" + "\t";
                    }
                    else
                    {
                        txt += it.Value + "\t";
                    }
                }
                txt = txt.Remove(txt.Length - 1, 1);
                txt += "\n";
            }
            txt = txt.Remove(txt.Length - 1, 1);

            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                await clip.ClearAsync();
                await clip.SetTextAsync(txt);
            }
        }
        #endregion

        #region PasteRows
        public ReactiveCommand<object, Unit> PasteRows { get; protected set; }
        private async Task _PasteRows(object _param)
        {
            object[] param = _param as object[];
            IKeyCollection collection = param[0] as IKeyCollection;
            int minColumn = Convert.ToInt32(param[1]) + 1;
            int maxColumn = Convert.ToInt32(param[2]) + 1;

            if (Avalonia.Application.Current.Clipboard is Avalonia.Input.Platform.IClipboard clip)
            {
                string? text = await clip.GetTextAsync();
                var rowsText = ParseInnerTextRows(text);

                foreach (IKey item in collection.GetEnumerable().OrderBy(x => x.Order))
                {
                    var props = item.GetType().GetProperties();

                    var rowText = rowsText[item.Order - collection.GetEnumerable().Min(x => x.Order)];
                    var columnsText = ParseInnerTextColumn(rowText);
                    foreach (var prop in props)
                    {
                        var attr = (Form_PropertyAttribute)prop.GetCustomAttributes(typeof(Form_PropertyAttribute), false).FirstOrDefault();
                        if (attr != null)
                        {
                            try
                            {
                                var columnNum = Convert.ToInt32(attr.Number);
                                if (columnNum >= minColumn && columnNum <= maxColumn)
                                {
                                    var midvalue = prop.GetMethod.Invoke(item, null);
                                    midvalue.GetType().GetProperty("Value").SetMethod.Invoke(midvalue, new object[] { columnsText[columnNum - minColumn] });
                                }
                            }
                            catch
                            {

                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region DeleteNote
        public ReactiveCommand<object, Unit> DeleteNote { get; protected set; }
        private async Task _DeleteNote(object _param)
        {
            if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                var param = (IEnumerable)_param;
                var answ = await ShowMessageT.Handle(new List<string>() { "Вы действительно хотите удалить комментарий?", "Да", "Нет" });
                if (answ == "Да")
                {
                    List<Note> lst = new List<Note>();
                    foreach (object? item in param)
                    {
                        lst.Add((Note)item);
                    }
                    foreach (var item in lst) 
                    {
                        if (item != null)
                        {
                            foreach (Note it in Storage.Notes)
                            {
                                if (it.Order > item.Order)
                                {
                                    it.Order = it.Order - 1;
                                }
                            }
                            foreach (Note nt in lst)
                            {
                                Storage.Notes.Remove((Note)nt);
                            }
                        }
                    }
                    //var grp = lst.GroupBy(x => x.Order);
                    //foreach (var group in grp)
                    //{
                    //    var item = group.FirstOrDefault();
                    //    if (item != null)
                    //    {
                    //        foreach (Note it in Storage.Notes)
                    //        {
                    //            if (it.Order > item.Order)
                    //            {
                    //                it.Order = it.Order - 1;
                    //            }
                    //        }
                    //        foreach (Note nt in lst)
                    //        {
                    //            Storage.Notes.Remove((Note)nt);
                    //        }
                    //    }
                    //}
                    Storage.Sort();
                }
            }
        }
        #endregion


        public ChangeOrCreateVM(string param, in Report rep,Reports reps)
        {
            Storage = rep;
            Storages = reps;
            FormType = param;
            Init();
        }
        public ChangeOrCreateVM(string param, in Reports reps)
        {
            Storage = new Report()
            {
                FormNum_DB = param
            };

            if (param.Split('.')[0] == "1")
            {
                if (param != "1.0")
                {
                    try
                    {
                        var ty = (from Report t in reps.Report_Collection where t.FormNum_DB == param && t.EndPeriod_DB != "" orderby DateTimeOffset.Parse(t.EndPeriod_DB) select t.EndPeriod_DB).LastOrDefault();

                        FormType = param;
                        Storage.StartPeriod.Value = ty;
                    }
                    catch
                    {

                    }
                }
            }
            else
            {
                if (param != "2.0")
                {
                    try
                    {
                        var ty = (from Report t in reps.Report_Collection where t.FormNum_DB == param && t.Year_DB != null orderby t.Year_DB select t.Year_DB).LastOrDefault();

                        FormType = param;
                        Storage.Year.Value = ty + 1;
                    }
                    catch
                    {

                    }
                }
            }

            if (param == "1.0")
            {
                var ty1 = (Form10)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows10.Add(ty1);
                Storage.Rows10.Add(ty2);
            }
            if (param == "2.0")
            {
                var ty1 = (Form20)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows20.Add(ty1);
                Storage.Rows20.Add(ty2);
            }
            Storages = reps;
            FormType = param;
            Init();
        }
        public ChangeOrCreateVM(string param, in DBObservable reps)
        {
            Storage = new Report()
            {
                FormNum_DB = param
            };

            if (param == "1.0")
            {
                var ty1 = (Form10)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form10)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows10.Add(ty1);
                Storage.Rows10.Add(ty2);
            }
            if (param == "2.0")
            {
                var ty1 = (Form20)FormCreator.Create(param);
                ty1.NumberInOrder_DB = 1;
                var ty2 = (Form20)FormCreator.Create(param);
                ty2.NumberInOrder_DB = 2;
                Storage.Rows20.Add(ty1);
                Storage.Rows20.Add(ty2);
            }

            FormType = param;
            DBO = reps;
            Init();
        }

        public Interaction<int, int> ShowDialogIn { get; protected set; }
        public Interaction<object, int> ShowDialog { get; protected set; }
        public Interaction<string, string> ShowMessage { get; protected set; }
        public Interaction<List<string>, string> ShowMessageT { get; protected set; }
        public void Init()
        {
            string a = FormType.Replace(".", "");
            if ((FormType.Split('.')[1] != "0" && FormType.Split('.')[0] == "1")||(FormType.Split('.')[1] != "0" && FormType.Split('.')[0] == "2"))
            {
                WindowHeader = ((Form_ClassAttribute)Type.GetType("Models.Form" + a + ",Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name +
                    " " + Storages.Master_DB.RegNoRep.Value + " " + Storages.Master_DB.ShortJurLicoRep.Value + " " + Storages.Master_DB.OkpoRep.Value;
            }
            if(FormType=="1.0"||FormType=="2.0")
            {
                WindowHeader = ((Form_ClassAttribute)Type.GetType("Models.Form" + a + ",Models").GetCustomAttributes(typeof(Form_ClassAttribute), false).First()).Name;
            }
            AddRow = ReactiveCommand.CreateFromTask<object>(_AddRow);
            AddRowIn = ReactiveCommand.CreateFromTask<object>(_AddRowIn);
            DeleteRow = ReactiveCommand.CreateFromTask<object>(_DeleteRow);
            ChangeReportOrder = ReactiveCommand.Create(_ChangeReportOrder);
            CheckReport = ReactiveCommand.Create(_CheckReport);
            SumRow = ReactiveCommand.Create(_SumRow);
            PasteRows = ReactiveCommand.CreateFromTask<object>(_PasteRows);
            DuplicateRowsx1 = ReactiveCommand.CreateFromTask<object>(_DuplicateRowsx1);
            CopyRows = ReactiveCommand.CreateFromTask<object>(_CopyRows);
            AddNote = ReactiveCommand.CreateFromTask<object>(_AddNote);
            DeleteNote = ReactiveCommand.CreateFromTask<object>(_DeleteNote);
            DuplicateNotes = ReactiveCommand.CreateFromTask<object>(_DuplicateNotes);

            ShowDialog = new Interaction<object,int>();
            ShowDialogIn = new Interaction<int, int>();
            ShowMessage = new Interaction<string, string>();
            ShowMessageT = new Interaction<List<string>, string>();
            Storage.Sort();
        }

        private bool _isCanSaveReportEnabled = false;

        private bool IsCanSaveReportEnabled
        {
            get => _isCanSaveReportEnabled;
            set
            {
                if (value == _isCanSaveReportEnabled)
                {
                    return;
                }

                _isCanSaveReportEnabled = value;
                PropertyChanged?
                    .Invoke(this, new PropertyChangedEventArgs(nameof(IsCanSaveReportEnabled)));
            }
        }

        [DependsOn(nameof(IsCanSaveReportEnabled))]
        private bool CanSaveReport(object parameter)
        {
            return _isCanSaveReportEnabled;
        }

        public void SaveReport()
        {
            if (Avalonia.Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (DBO != null)
                {
                    var tmp = new Reports();
                    tmp.Master = Storage;
                    if (tmp.Master.Rows10.Count != 0)
                    {
                        tmp.Master.Rows10[1].OrganUprav.Value = tmp.Master.Rows10[0].OrganUprav.Value;
                        tmp.Master.Rows10[1].RegNo.Value = tmp.Master.Rows10[0].RegNo.Value;
                    }
                    if (tmp.Master.Rows20.Count != 0)
                    {
                        tmp.Master.Rows20[1].OrganUprav.Value = tmp.Master.Rows20[0].OrganUprav.Value;
                        tmp.Master.Rows20[1].RegNo.Value = tmp.Master.Rows20[0].RegNo.Value;
                    }

                    DBO.Reports_Collection.Add(tmp);
                    DBO = null;
                    Storages = null;
                }
                else
                {
                    if (Storages != null)
                    {
                        if (FormType != "1.0" && FormType != "2.0")
                        {
                            if (!Storages.Report_Collection.Contains(Storage))
                            {
                                Storages.Report_Collection.Add(Storage);
                            }
                        }
                    }
                    else
                    {

                    }
                }
                if (Storages != null)
                {
                    Storages.Report_Collection.QuickSort();
                }
                
  
                var dbm = StaticConfiguration.DBModel;
                dbm.SaveChanges();
                IsCanSaveReportEnabled = false;
            }
        }

        public void _AddRow10()
        {
            Form10? frm = new Form10(); Storage.Rows10.Add(frm); Storage.LastAddedForm = Report.Forms.Form10;
        }
        public void _AddRow20()
        {
            Form20? frm = new Form20(); Storage.Rows20.Add(frm); Storage.LastAddedForm = Report.Forms.Form20;
        }

        int GetNumberInOrder(IKeyCollection lst)
        {
            int maxNum = 0;

            foreach (var item in lst)
            {
                var frm = (INumberInOrder)item;
                if (frm.Order >= maxNum)
                {
                    maxNum++;
                }
            }

            return maxNum + 1;
        }
      
        private string[] ParseInnerTextRows(string Text)
        {
            List<string> lst = new List<string>();

            bool comaFlag = false;
            Text = Text.Replace("\r\n","\n");

            string txt = "";
            foreach(char item in Text)
            {
                if(item=='\"')
                {
                    txt += item;
                    comaFlag = true;
                }
                else
                {
                    if(item=='\n')
                    {
                        if (!comaFlag)
                        {
                            lst.Add(txt);
                            txt = "";
                        }
                        else
                        {
                            txt += item;
                        }
                    }
                    else
                    {
                        txt += item;
                    }
                }
            }
            if(txt!="")
            {
                lst.Add(txt);
            }

            return lst.ToArray();
        }

        private string[] ParseInnerTextColumn(string Text)
        {
            List<string> lst = new List<string>();

            bool comaFlag = false;

            string txt = "";
            foreach (char item in Text)
            {
                if (item == '\"')
                {
                    comaFlag = true;
                }
                else
                {
                    if (item == '\t')
                    {
                        if (!comaFlag)
                        {
                            lst.Add(txt);
                            txt = "";
                        }
                        else
                        {
                            txt += item;
                        }
                    }
                    else
                    {
                        txt += item;
                    }
                }
            }
            if(txt!="")
            {
                lst.Add(txt);
            }

            return lst.ToArray();
        }
    }
}
