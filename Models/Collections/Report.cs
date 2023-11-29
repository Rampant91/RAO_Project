using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System;
using System.Text.RegularExpressions;
using Models.Attributes;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Forms;
using Models.Forms.DataAccess;
using Models.Forms.Form1;
using Models.Forms.Form2;
using Models.Interfaces;

namespace Models.Collections;

public class Report : IKey, IDataGridColumn
{
    #region Constructor

    public Report()
    {
        Init();
    }

    #endregion

    #region Init

    private void Init()
    {
        Rows10 = new ObservableCollectionWithItemPropertyChanged<Form10>();
        Rows10.CollectionChanged += CollectionChanged10;

        Rows11 = new ObservableCollectionWithItemPropertyChanged<Form11>();
        Rows11.CollectionChanged += CollectionChanged11;

        Rows12 = new ObservableCollectionWithItemPropertyChanged<Form12>();
        Rows12.CollectionChanged += CollectionChanged12;

        Rows13 = new ObservableCollectionWithItemPropertyChanged<Form13>();
        Rows13.CollectionChanged += CollectionChanged13;

        Rows14 = new ObservableCollectionWithItemPropertyChanged<Form14>();
        Rows14.CollectionChanged += CollectionChanged14;

        Rows15 = new ObservableCollectionWithItemPropertyChanged<Form15>();
        Rows15.CollectionChanged += CollectionChanged15;

        Rows16 = new ObservableCollectionWithItemPropertyChanged<Form16>();
        Rows16.CollectionChanged += CollectionChanged16;

        Rows17 = new ObservableCollectionWithItemPropertyChanged<Form17>();
        Rows17.CollectionChanged += CollectionChanged17;

        Rows18 = new ObservableCollectionWithItemPropertyChanged<Form18>();
        Rows18.CollectionChanged += CollectionChanged18;

        Rows19 = new ObservableCollectionWithItemPropertyChanged<Form19>();
        Rows19.CollectionChanged += CollectionChanged19;

        Rows20 = new ObservableCollectionWithItemPropertyChanged<Form20>();
        Rows20.CollectionChanged += CollectionChanged20;

        Rows21 = new ObservableCollectionWithItemPropertyChanged<Form21>();
        Rows21.CollectionChanged += CollectionChanged21;

        Rows22 = new ObservableCollectionWithItemPropertyChanged<Form22>();
        Rows22.CollectionChanged += CollectionChanged22;

        Rows23 = new ObservableCollectionWithItemPropertyChanged<Form23>();
        Rows23.CollectionChanged += CollectionChanged23;

        Rows24 = new ObservableCollectionWithItemPropertyChanged<Form24>();
        Rows24.CollectionChanged += CollectionChanged24;

        Rows25 = new ObservableCollectionWithItemPropertyChanged<Form25>();
        Rows25.CollectionChanged += CollectionChanged25;

        Rows26 = new ObservableCollectionWithItemPropertyChanged<Form26>();
        Rows26.CollectionChanged += CollectionChanged26;

        Rows27 = new ObservableCollectionWithItemPropertyChanged<Form27>();
        Rows27.CollectionChanged += CollectionChanged27;

        Rows28 = new ObservableCollectionWithItemPropertyChanged<Form28>();
        Rows28.CollectionChanged += CollectionChanged28;

        Rows29 = new ObservableCollectionWithItemPropertyChanged<Form29>();
        Rows29.CollectionChanged += CollectionChanged29;

        Rows210 = new ObservableCollectionWithItemPropertyChanged<Form210>();
        Rows210.CollectionChanged += CollectionChanged210;

        Rows211 = new ObservableCollectionWithItemPropertyChanged<Form211>();
        Rows211.CollectionChanged += CollectionChanged211;

        Rows212 = new ObservableCollectionWithItemPropertyChanged<Form212>();
        Rows212.CollectionChanged += CollectionChanged212;

        Notes = new ObservableCollectionWithItemPropertyChanged<Note>();
        Notes.CollectionChanged += CollectionChangedNotes;
    }

    #endregion

    #region FormsEnum

    public enum Forms
    {
        None,
        Form10,
        Form11,
        Form12,
        Form13,
        Form14,
        Form15,
        Form16,
        Form17,
        Form18,
        Form19,
        Form20,
        Form21,
        Form22,
        Form23,
        Form24,
        Form25,
        Form26,
        Form27,
        Form28,
        Form29,
        Form210,
        Form211,
        Form212
    }

    [NotMapped]
    private bool _isChanged = true;

    [NotMapped]
    private Forms _lastAddedForm = Forms.None;

    [NotMapped]
    public Forms LastAddedForm
    {
        get => _lastAddedForm;
        set
        {
            if (value != _lastAddedForm)
            {
                _lastAddedForm = value;
            }
        }
    }

    #endregion

    #region Dictionary

    [NotMapped]
    Dictionary<string, RamAccess> Dictionary { get; set; } = new(); 
    
    #endregion

    #region Id

    public int Id { get; set; }

    #endregion

    //public int Master_DBid { get; set; }

    //[ForeignKey(nameof(Master_DBid))]
    //public Report Master_DB { get; set; }

    public int ReportsId { get; set; }

    [ForeignKey(nameof(ReportsId))]
    public Reports Reports { get; set; }

    #region Order
    
    public void SetOrder(long index) { }

    public long Order
    {
        get
        {
            try
            {
                var frm = FormNum_DB.Replace(".", "");
                var cor = Convert.ToInt32(CorrectionNumber_DB);
                if (FormNum_DB.Split('.')[0] == "1")
                {
                    try
                    {
                        var dt = DateTimeOffset.Parse(EndPeriod_DB);
                        var num = dt.Year.ToString();
                        num += dt.Month < 10 ? $"0{dt.Month}" : dt.Month.ToString();
                        num += dt.Day < 10 ? $"0{dt.Day}" : dt.Day.ToString();
                        num = num.Insert(0, "1");
                        frm += (int)(1.0 / Convert.ToInt32(num) * 100000000000000000.0);
                        frm += cor;
                    }
                    catch
                    {
                        frm += "000000000";
                    }
                    return Convert.ToInt64(frm);
                }

                var year = Convert.ToInt32(Year_DB);
                if (Year_DB != null && year != 0)
                {
                    frm += (int)(1.0 / year * 10000000);
                    frm += cor;
                }
                return Convert.ToInt32(frm);
            }
            catch
            {
                return 0;
            }
        }
    }

    #endregion

    #region Properties

    #region Comments

    public string Comments_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Комментарий")]
    public RamAccess<string> Comments
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Comments)))
            {
                ((RamAccess<string>)Dictionary[nameof(Comments)]).Value = Comments_DB;
                return (RamAccess<string>)Dictionary[nameof(Comments)];
            }
            var rm = new RamAccess<string>(Comments_Validation, Comments_DB);
            rm.PropertyChanged += CommentsValueChanged;
            Dictionary.Add(nameof(Comments), rm);
            return (RamAccess<string>)Dictionary[nameof(Comments)];
        }
        set
        {
            Comments_DB = value.Value;
            OnPropertyChanged(nameof(Comments));
        }
    }

    private void CommentsValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Comments_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool Comments_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region CorrectionNumber

    public byte CorrectionNumber_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Номер корректировки")]
    public RamAccess<byte> CorrectionNumber
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(CorrectionNumber)))
            {
                ((RamAccess<byte>)Dictionary[nameof(CorrectionNumber)]).Value = CorrectionNumber_DB;
                return (RamAccess<byte>)Dictionary[nameof(CorrectionNumber)];
            }
            var rm = new RamAccess<byte>(CorrectionNumber_Validation, CorrectionNumber_DB);
            rm.PropertyChanged += CorrectionNumberValueChanged;
            Dictionary.Add(nameof(CorrectionNumber), rm);
            return (RamAccess<byte>)Dictionary[nameof(CorrectionNumber)];
        }
        set
        {
            CorrectionNumber_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void CorrectionNumberValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            CorrectionNumber_DB = ((RamAccess<byte>)value).Value;
        }
    }

    private static bool CorrectionNumber_Validation(RamAccess<byte> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region FormNum

    public string FormNum_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Форма")]
    public RamAccess<string> FormNum
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(FormNum)))
            {
                ((RamAccess<string>)Dictionary[nameof(FormNum)]).Value = FormNum_DB;
                return (RamAccess<string>)Dictionary[nameof(FormNum)];
            }
            var rm = new RamAccess<string>(FormNum_Validation, FormNum_DB);
            rm.PropertyChanged += FormNumValueChanged;
            Dictionary.Add(nameof(FormNum), rm);
            return (RamAccess<string>)Dictionary[nameof(FormNum)];
        }
        set
        {
            FormNum_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void FormNumValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            FormNum_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool FormNum_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region ExecutorData & other form top data

    #region ExecutorData

    #region FIOexecutor

    public string FIOexecutor_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Номер корректировки")]
    public RamAccess<string> FIOexecutor
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(FIOexecutor)))
            {
                ((RamAccess<string>)Dictionary[nameof(FIOexecutor)]).Value = FIOexecutor_DB;
                return (RamAccess<string>)Dictionary[nameof(FIOexecutor)];
            }
            var rm = new RamAccess<string>(FIOexecutor_Validation, FIOexecutor_DB);
            rm.PropertyChanged += FIOexecutorValueChanged;
            Dictionary.Add(nameof(FIOexecutor), rm);
            return (RamAccess<string>)Dictionary[nameof(FIOexecutor)];
        }
        set
        {
            FIOexecutor_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void FIOexecutorValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            FIOexecutor_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool FIOexecutor_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region GradeExecutor

    public string GradeExecutor_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Номер корректировки")]
    public RamAccess<string> GradeExecutor
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(GradeExecutor)))
            {
                ((RamAccess<string>)Dictionary[nameof(GradeExecutor)]).Value = GradeExecutor_DB;
                return (RamAccess<string>)Dictionary[nameof(GradeExecutor)];
            }
            var rm = new RamAccess<string>(GradeExecutor_Validation, GradeExecutor_DB);
            rm.PropertyChanged += GradeExecutorValueChanged;
            Dictionary.Add(nameof(GradeExecutor), rm);
            return (RamAccess<string>)Dictionary[nameof(GradeExecutor)];
        }
        set
        {
            GradeExecutor_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void GradeExecutorValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            GradeExecutor_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool GradeExecutor_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region ExecPhone

    public string ExecPhone_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Номер корректировки")]
    public RamAccess<string> ExecPhone
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ExecPhone)))
            {
                ((RamAccess<string>)Dictionary[nameof(ExecPhone)]).Value = ExecPhone_DB;
                return (RamAccess<string>)Dictionary[nameof(ExecPhone)];
            }
            var rm = new RamAccess<string>(ExecPhone_Validation, ExecPhone_DB);
            rm.PropertyChanged += ExecPhoneValueChanged;
            Dictionary.Add(nameof(ExecPhone), rm);
            return (RamAccess<string>)Dictionary[nameof(ExecPhone)];
        }
        set
        {
            ExecPhone_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ExecPhoneValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ExecPhone_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool ExecPhone_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region ExecEmail

    public string ExecEmail_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Номер корректировки")]
    public RamAccess<string> ExecEmail
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ExecEmail)))
            {
                ((RamAccess<string>)Dictionary[nameof(ExecEmail)]).Value = ExecEmail_DB;
                return (RamAccess<string>)Dictionary[nameof(ExecEmail)];
            }
            var rm = new RamAccess<string>(ExecEmail_Validation, ExecEmail_DB);
            rm.PropertyChanged += ExecEmailValueChanged;
            Dictionary.Add(nameof(ExecEmail), rm);
            return (RamAccess<string>)Dictionary[nameof(ExecEmail)];
        }
        set
        {
            ExecEmail_DB = value.Value.Replace(" ", string.Empty);
            OnPropertyChanged();
        }
    }

    private void ExecEmailValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ExecEmail_DB = ((RamAccess<string>)value).Value.Replace(" ", string.Empty);
        }
    }

    private static bool ExecEmail_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var forbiddenSymbols = new[] { '<', '>', '(', ')', '[', ']', ',', ';', ':', '\\', '/', '"', '*' };
        if (forbiddenSymbols.Any(value.Value.Contains))
        {
            value.AddError("поле содержит запрещенные символы <>()[],;:\\/*\"");
            return false;
        }
        if (value.Value.Contains(' '))
        {
            value.AddError("поле содержит пробелы");
            return false;
        }
        var tmp = value.Value;
        if (!new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #endregion

    #region FormData26

    #region SourcesQuantity26

    public int? SourcesQuantity26_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Количество источников, шт")]
    public RamAccess<int?> SourcesQuantity26
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(SourcesQuantity26)))
            {
                ((RamAccess<int?>)Dictionary[nameof(SourcesQuantity26)]).Value = SourcesQuantity26_DB;
                return (RamAccess<int?>)Dictionary[nameof(SourcesQuantity26)];
            }
            var rm = new RamAccess<int?>(SourcesQuantity26_Validation, SourcesQuantity26_DB);
            rm.PropertyChanged += SourcesQuantity26ValueChanged;
            Dictionary.Add(nameof(SourcesQuantity26), rm);
            return (RamAccess<int?>)Dictionary[nameof(SourcesQuantity26)];
        }
        set
        {
            SourcesQuantity26_DB = value.Value;
            OnPropertyChanged(nameof(SourcesQuantity26));
        }
    }
    // positive int.

    private void SourcesQuantity26ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            SourcesQuantity26_DB = ((RamAccess<int?>)value).Value;
        }
    }

    private static bool SourcesQuantity26_Validation(RamAccess<int?> value)//Ready
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value <= 0)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion 

    #endregion

    #region FormData27

    #region  PermissionNumber27

    public string PermissionNumber27_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Номер разрешительного документа")]
    public RamAccess<string> PermissionNumber27
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PermissionNumber27), out var value))
            {
                ((RamAccess<string>)value).Value = PermissionNumber27_DB;
            }
            else
            {
                var rm = new RamAccess<string>(PermissionNumber27_Validation, PermissionNumber27_DB);
                rm.PropertyChanged += PermissionNumber27ValueChanged;
                Dictionary.Add(nameof(PermissionNumber27), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(PermissionNumber27)];
        }
        set
        {
            PermissionNumber27_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PermissionNumber27ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PermissionNumber27_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool PermissionNumber27_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region  PermissionIssueDate27

    public string PermissionIssueDate27_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Дата выпуска разрешительного документа")]
    public RamAccess<string> PermissionIssueDate27
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PermissionIssueDate27), out var value))
            {
                ((RamAccess<string>)value).Value = PermissionIssueDate27_DB;
            }
            else
            {
                var rm = new RamAccess<string>(PermissionIssueDate27_Validation, PermissionIssueDate27_DB);
                rm.PropertyChanged += PermissionIssueDate27ValueChanged;
                Dictionary.Add(nameof(PermissionIssueDate27), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate27)];
        }
        set
        {
            PermissionIssueDate27_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PermissionIssueDate27ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        PermissionIssueDate27_DB = tmp;
    }

    private static bool PermissionIssueDate27_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        if (!DateTimeOffset.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  ValidBegin27

    public string ValidBegin27_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Действует с")]
    public RamAccess<string> ValidBegin27
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ValidBegin27)))
            {
                ((RamAccess<string>)Dictionary[nameof(ValidBegin27)]).Value = ValidBegin27_DB;
                return (RamAccess<string>)Dictionary[nameof(ValidBegin27)];
            }
            var rm = new RamAccess<string>(ValidBegin27_Validation, ValidBegin27_DB);
            rm.PropertyChanged += ValidBegin27ValueChanged;
            Dictionary.Add(nameof(ValidBegin27), rm);
            return (RamAccess<string>)Dictionary[nameof(ValidBegin27)];
        }
        set
        {
            ValidBegin27_DB = value.Value;
            OnPropertyChanged(nameof(ValidBegin27));
        }
    }

    private void ValidBegin27ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)value).Value;
            if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            ValidBegin27_DB = tmp;
        }
    }

    private static bool ValidBegin27_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  ValidThru27

    public string ValidThru27_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Действует по")]
    public RamAccess<string> ValidThru27
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ValidThru27)))
            {
                ((RamAccess<string>)Dictionary[nameof(ValidThru27)]).Value = ValidThru27_DB;
                return (RamAccess<string>)Dictionary[nameof(ValidThru27)];
            }
            var rm = new RamAccess<string>(ValidThru27_Validation, ValidThru27_DB);
            rm.PropertyChanged += ValidThru27ValueChanged;
            Dictionary.Add(nameof(ValidThru27), rm);
            return (RamAccess<string>)Dictionary[nameof(ValidThru27)];
        }
        set
        {
            ValidThru27_DB = value.Value;
            OnPropertyChanged(nameof(ValidThru27));
        }
    }

    private void ValidThru27ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)value).Value;
            if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            ValidThru27_DB = tmp;
        }
    }

    private static bool ValidThru27_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region  PermissionDocumentName27

    public string PermissionDocumentName27_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наименование разрешительного документа")]
    public RamAccess<string> PermissionDocumentName27
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PermissionDocumentName27)))
            {
                ((RamAccess<string>)Dictionary[nameof(PermissionDocumentName27)]).Value = PermissionDocumentName27_DB;
                return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName27)];
            }
            var rm = new RamAccess<string>(PermissionDocumentName27_Validation, PermissionDocumentName27_DB);
            rm.PropertyChanged += PermissionDocumentName27ValueChanged;
            Dictionary.Add(nameof(PermissionDocumentName27), rm);
            return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName27)];
        }
        set
        {
            PermissionDocumentName27_DB = value.Value;
            OnPropertyChanged(nameof(PermissionDocumentName27));
        }
    }

    private void PermissionDocumentName27ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PermissionDocumentName27_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool PermissionDocumentName27_Validation(RamAccess<string> value)
    {
        value.ClearErrors(); return true;
    }

    #endregion

    #endregion

    #region FormData28

    #region Permission "Разрешение на сброс радионуклидов в водные объекты"

    #region PermissionNumber_28 

    public string PermissionNumber_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Разрешение на сброс радионуклидов в водные объекты №")]
    public RamAccess<string> PermissionNumber_28
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PermissionNumber_28)))
            {
                ((RamAccess<string>)Dictionary[nameof(PermissionNumber_28)]).Value = PermissionNumber_28_DB;
                return (RamAccess<string>)Dictionary[nameof(PermissionNumber_28)];
            }
            var rm = new RamAccess<string>(PermissionNumber_28_Validation, PermissionNumber_28_DB);
            rm.PropertyChanged += PermissionNumber_28ValueChanged;
            Dictionary.Add(nameof(PermissionNumber_28), rm);
            return (RamAccess<string>)Dictionary[nameof(PermissionNumber_28)];
        }
        set
        {
            PermissionNumber_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PermissionNumber_28ValueChanged(object value, PropertyChangedEventArgs args) 
    {
        if (args.PropertyName == "Value")
        {
            PermissionNumber_28_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool PermissionNumber_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region PermissionIssueDate_28

    public string PermissionIssueDate_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Дата выпуска разрешительного документа")]
    public RamAccess<string> PermissionIssueDate_28
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PermissionIssueDate_28)))
            {
                ((RamAccess<string>)Dictionary[nameof(PermissionIssueDate_28)]).Value = PermissionIssueDate_28_DB;
                return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate_28)];
            }
            var rm = new RamAccess<string>(PermissionIssueDate_28_Validation, PermissionIssueDate_28_DB);
            rm.PropertyChanged += PermissionIssueDate_28ValueChanged;
            Dictionary.Add(nameof(PermissionIssueDate_28), rm);
            return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate_28)];
        }
        set
        {
            PermissionIssueDate_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PermissionIssueDate_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        PermissionIssueDate_28_DB = tmp;
    }

    private static bool PermissionIssueDate_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region ValidBegin_28

    public string ValidBegin_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Действует с")]
    public RamAccess<string> ValidBegin_28
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ValidBegin_28)))
            {
                ((RamAccess<string>)Dictionary[nameof(ValidBegin_28)]).Value = ValidBegin_28_DB;
                return (RamAccess<string>)Dictionary[nameof(ValidBegin_28)];
            }
            var rm = new RamAccess<string>(ValidBegin_28_Validation, ValidBegin_28_DB);
            rm.PropertyChanged += ValidBegin_28ValueChanged;
            Dictionary.Add(nameof(ValidBegin_28), rm);
            return (RamAccess<string>)Dictionary[nameof(ValidBegin_28)];
        }
        set
        {
            ValidBegin_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ValidBegin_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        ValidBegin_28_DB = tmp;
    }

    private static bool ValidBegin_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region ValidThru_28

    public string ValidThru_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Действует по")]
    public RamAccess<string> ValidThru_28
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ValidThru_28)))
            {
                ((RamAccess<string>)Dictionary[nameof(ValidThru_28)]).Value = ValidThru_28_DB;
                return (RamAccess<string>)Dictionary[nameof(ValidThru_28)];
            }
            var rm = new RamAccess<string>(ValidThru_28_Validation, ValidThru_28_DB);
            rm.PropertyChanged += ValidThru_28ValueChanged;
            Dictionary.Add(nameof(ValidThru_28), rm);
            return (RamAccess<string>)Dictionary[nameof(ValidThru_28)];
        }
        set
        {
            ValidThru_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ValidThru_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        ValidThru_28_DB = tmp;
    }

    private static bool ValidThru_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region PermissionDocumentName_28 

    public string PermissionDocumentName_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наименование разрешительного документа")]
    public RamAccess<string> PermissionDocumentName_28
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PermissionDocumentName_28)))
            {
                ((RamAccess<string>)Dictionary[nameof(PermissionDocumentName_28)]).Value = PermissionDocumentName_28_DB;
                return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName_28)];
            }
            var rm = new RamAccess<string>(PermissionDocumentName_28_Validation, PermissionDocumentName_28_DB);
            rm.PropertyChanged += PermissionDocumentName_28ValueChanged;
            Dictionary.Add(nameof(PermissionDocumentName_28), rm);
            return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName_28)];
        }
        set
        {
            PermissionDocumentName_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PermissionDocumentName_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PermissionDocumentName_28_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool PermissionDocumentName_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #endregion

    #region Permission1 "Разрешение на сброс радионуклидов на рельеф местности"

    #region PermissionNumber1_28 

    public string PermissionNumber1_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Номер разрешительного документа")]
    public RamAccess<string> PermissionNumber1_28
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(PermissionNumber1_28)))
            {
                ((RamAccess<string>)Dictionary[nameof(PermissionNumber1_28)]).Value = PermissionNumber1_28_DB;
                return (RamAccess<string>)Dictionary[nameof(PermissionNumber1_28)];
            }
            var rm = new RamAccess<string>(PermissionNumber1_28_Validation, PermissionNumber1_28_DB);
            rm.PropertyChanged += PermissionNumber1_28ValueChanged;
            Dictionary.Add(nameof(PermissionNumber1_28), rm);
            return (RamAccess<string>)Dictionary[nameof(PermissionNumber1_28)];
        }
        set
        {
            PermissionNumber1_28_DB = value.Value;
            OnPropertyChanged(nameof(PermissionNumber1_28));
        }
    }

    private void PermissionNumber1_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PermissionNumber1_28_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool PermissionNumber1_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region PermissionIssueDate1_28 

    public string PermissionIssueDate1_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Дата выпуска разрешительного документа")]
    public RamAccess<string> PermissionIssueDate1_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PermissionIssueDate1_28), out var value))
            {
                ((RamAccess<string>)value).Value = PermissionIssueDate1_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(PermissionIssueDate1_28_Validation, PermissionIssueDate1_28_DB);
                rm.PropertyChanged += PermissionIssueDate1_28ValueChanged;
                Dictionary.Add(nameof(PermissionIssueDate1_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(PermissionIssueDate1_28)];
        }
        set
        {
            PermissionIssueDate1_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PermissionIssueDate1_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        PermissionIssueDate1_28_DB = tmp;
    }

    private static bool PermissionIssueDate1_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region ValidBegin1_28

    public string ValidBegin1_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Действует с")]
    public RamAccess<string> ValidBegin1_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ValidBegin1_28), out var value))
            {
                ((RamAccess<string>)value).Value = ValidBegin1_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(ValidBegin1_28_Validation, ValidBegin1_28_DB);
                rm.PropertyChanged += ValidBegin1_28ValueChanged;
                Dictionary.Add(nameof(ValidBegin1_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(ValidBegin1_28)];
        }
        set
        {
            ValidBegin1_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ValidBegin1_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        ValidBegin1_28_DB = tmp;
    }

    private static bool ValidBegin1_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region ValidThru1_28 

    public string ValidThru1_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Действует по")]
    public RamAccess<string> ValidThru1_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ValidThru1_28), out var value))
            {
                ((RamAccess<string>)value).Value = ValidThru1_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(ValidThru1_28_Validation, ValidThru1_28_DB);
                rm.PropertyChanged += ValidThru1_28ValueChanged;
                Dictionary.Add(nameof(ValidThru1_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(ValidThru1_28)];
        }
        set
        {
            ValidThru1_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ValidThru1_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        ValidThru1_28_DB = tmp;
    }

    private static bool ValidThru1_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region PermissionDocumentName1_28

    public string PermissionDocumentName1_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наименование разрешительного документа")]
    public RamAccess<string> PermissionDocumentName1_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(PermissionDocumentName1_28), out var value))
            {
                ((RamAccess<string>)value).Value = PermissionDocumentName1_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(PermissionDocumentName1_28_Validation, PermissionDocumentName1_28_DB);
                rm.PropertyChanged += PermissionDocumentName1_28ValueChanged;
                Dictionary.Add(nameof(PermissionDocumentName1_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(PermissionDocumentName1_28)];
        }
        set
        {
            PermissionDocumentName1_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void PermissionDocumentName1_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            PermissionDocumentName1_28_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool PermissionDocumentName1_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #endregion

    #region Contract "Договор на перелачу сточных вод в сети канализации"

    #region ContractNumber_28 

    public string ContractNumber_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Номер договора на передачу сточных вод")]
    public RamAccess<string> ContractNumber_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ContractNumber_28), out var value))
            {
                ((RamAccess<string>)value).Value = ContractNumber_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(ContractNumber_28_Validation, ContractNumber_28_DB);
                rm.PropertyChanged += ContractNumber_28ValueChanged;
                Dictionary.Add(nameof(ContractNumber_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(ContractNumber_28)];
        }
        set
        {
            ContractNumber_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ContractNumber_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ContractNumber_28_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool ContractNumber_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #region ContractIssueDate2_28

    public string ContractIssueDate2_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Дата выпуска разрешительного документа")]
    public RamAccess<string> ContractIssueDate2_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ContractIssueDate2_28), out var value))
            {
                ((RamAccess<string>)value).Value = ContractIssueDate2_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(ContractIssueDate2_28_Validation, ContractIssueDate2_28_DB);
                rm.PropertyChanged += ContractIssueDate2_28ValueChanged;
                Dictionary.Add(nameof(ContractIssueDate2_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(ContractIssueDate2_28)];
        }
        set => ContractIssueDate2_28_DB = value.Value;
    }

    private void ContractIssueDate2_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        ContractIssueDate2_28_DB = tmp;
    }

    private static bool ContractIssueDate2_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region ValidBegin2_28

    public string ValidBegin2_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Действует с")]
    public RamAccess<string> ValidBegin2_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ValidBegin2_28), out var value))
            {
                ((RamAccess<string>)value).Value = ValidBegin2_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(ValidBegin2_28_Validation, ValidBegin2_28_DB);
                rm.PropertyChanged += ValidBegin2_28ValueChanged;
                Dictionary.Add(nameof(ValidBegin2_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(ValidBegin2_28)];
        }
        set
        {
            ValidBegin2_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ValidBegin2_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        ValidBegin2_28_DB = tmp;
    }

    private static bool ValidBegin2_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region ValidThru2_28

    public string ValidThru2_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Действует по")]
    public RamAccess<string> ValidThru2_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(ValidThru2_28), out var value))
            {
                ((RamAccess<string>)value).Value = ValidThru2_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(ValidThru2_28_Validation, ValidThru2_28_DB);
                rm.PropertyChanged += ValidThru2_28ValueChanged;
                Dictionary.Add(nameof(ValidThru2_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(ValidThru2_28)];
        }
        set
        {
            ValidThru2_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void ValidThru2_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        var tmp = ((RamAccess<string>)value).Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        ValidThru2_28_DB = tmp;
    }

    private static bool ValidThru2_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            return true;
        }
        var tmp = value.Value;
        if (new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$").IsMatch(tmp))
        {
            tmp = tmp.Insert(6, "20");
        }
        if (!new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$").IsMatch(tmp) || !DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region OrganisationReciever_28

    public string OrganisationReciever_28_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Организация, осуществляющая прием сточных вод")]
    public RamAccess<string> OrganisationReciever_28
    {
        get
        {
            if (Dictionary.TryGetValue(nameof(OrganisationReciever_28), out var value))
            {
                ((RamAccess<string>)value).Value = OrganisationReciever_28_DB;
            }
            else
            {
                var rm = new RamAccess<string>(OrganisationReciever_28_Validation, OrganisationReciever_28_DB);
                rm.PropertyChanged += OrganisationReciever_28ValueChanged;
                Dictionary.Add(nameof(OrganisationReciever_28), rm);
            }
            return (RamAccess<string>)Dictionary[nameof(OrganisationReciever_28)];
        }
        set
        {
            OrganisationReciever_28_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void OrganisationReciever_28ValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            OrganisationReciever_28_DB = ((RamAccess<string>)value).Value;
        }
    }

    private bool OrganisationReciever_28_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        return true;
    }

    #endregion

    #endregion

    #endregion

    #endregion

    #region ExportDate

    public string ExportDate_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Дата выгрузки")]
    public RamAccess<string> ExportDate
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ExportDate)))
            {
                ((RamAccess<string>)Dictionary[nameof(ExportDate)]).Value = ExportDate_DB;
                return (RamAccess<string>)Dictionary[nameof(ExportDate)];
            }
            var rm = new RamAccess<string>(ExportDate_Validation, ExportDate_DB);
            rm.PropertyChanged += ExportDateValueChanged;
            Dictionary.Add(nameof(ExportDate), rm);
            return (RamAccess<string>)Dictionary[nameof(ExportDate)];
        }
        set
        {
            ExportDate_DB = value.Value;
            OnPropertyChanged(nameof(ExportDate));
        }
    }

    private void ExportDateValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ExportDate_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool ExportDate_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region IsCorrection

    public bool IsCorrection_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Корректирующий отчет")]
    public RamAccess<bool> IsCorrection
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(IsCorrection)))
            {
                ((RamAccess<bool>)Dictionary[nameof(IsCorrection)]).Value = IsCorrection_DB;
                return (RamAccess<bool>)Dictionary[nameof(IsCorrection)];
            }
            var rm = new RamAccess<bool>(IsCorrection_Validation, IsCorrection_DB);
            rm.PropertyChanged += IsCorrectionValueChanged;
            Dictionary.Add(nameof(IsCorrection), rm);
            return (RamAccess<bool>)Dictionary[nameof(IsCorrection)];
        }
        set
        {
            IsCorrection_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void IsCorrectionValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            IsCorrection_DB = ((RamAccess<bool>)value).Value;
        }
    }

    private static bool IsCorrection_Validation(RamAccess<bool> value)
    {
        return true;
    }

    #endregion

    #region Notes

    [FormProperty(true, "Примечания")]
    ObservableCollectionWithItemPropertyChanged<Note> Notes_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Note> Notes
    {
        get => Notes_DB;
        set
        {
            Notes_DB = value;
            OnPropertyChanged();
        }
    }

    protected void CollectionChangedNotes(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Notes));
    }

    private void NotesValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            Notes_DB = ((RamAccess<ObservableCollectionWithItemPropertyChanged<Note>>)value).Value;
        }
    }

    private bool Notes_Validation(RamAccess<ObservableCollectionWithItemPropertyChanged<Note>> value)
    {
        return true;
    }

    #endregion

    #region NumberInOrder

    public string NumberInOrder_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Номер")]
    public RamAccess<string> NumberInOrder
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(NumberInOrder)))
            {
                ((RamAccess<string>)Dictionary[nameof(NumberInOrder)]).Value = NumberInOrder_DB;
                return (RamAccess<string>)Dictionary[nameof(NumberInOrder)];
            }
            var rm = new RamAccess<string>(NumberInOrder_Validation, NumberInOrder_DB);
            rm.PropertyChanged += NumberInOrderValueChanged;
            Dictionary.Add(nameof(NumberInOrder), rm);
            return (RamAccess<string>)Dictionary[nameof(NumberInOrder)];
        }
        set
        {
            NumberInOrder_DB = value.Value;
            OnPropertyChanged();
        }
    }

    private void NumberInOrderValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            NumberInOrder_DB = ((RamAccess<string>)value).Value;
        }
    }

    private static bool NumberInOrder_Validation(RamAccess<string> value)
    {
        return true;
    }

    #endregion

    #region OkpoRep

    [NotMapped]
    private string _OkpoRep { get; set; } = "";

    [NotMapped]
    public RamAccess<string> OkpoRep
    {
        get
        {
            RamAccess<string> tmp = null;
            switch (FormNum_DB)
            {
                case "1.0" when Rows10[1].Okpo_DB is "" or "-":
                    tmp = Rows10[0].Okpo;
                    tmp.PropertyChanged -= OkpoRepValueChanged;
                    tmp.PropertyChanged += OkpoRepValueChanged;
                    Rows10[1].Okpo.PropertyChanged -= OkpoRepValueChanged;
                    Rows10[1].Okpo.PropertyChanged += OkpoRepValueChanged;
                    break;
                case "1.0":
                    tmp = Rows10[1].Okpo;
                    tmp.PropertyChanged -= OkpoRepValueChanged;
                    tmp.PropertyChanged += OkpoRepValueChanged;
                    Rows10[0].Okpo.PropertyChanged -= OkpoRepValueChanged;
                    Rows10[0].Okpo.PropertyChanged += OkpoRepValueChanged;
                    break;
                case "2.0" when Rows20[1].Okpo_DB is "" or "-":
                    tmp = Rows20[0].Okpo;
                    tmp.PropertyChanged -= OkpoRepValueChanged;
                    tmp.PropertyChanged += OkpoRepValueChanged;
                    Rows20[1].Okpo.PropertyChanged -= OkpoRepValueChanged;
                    Rows20[1].Okpo.PropertyChanged += OkpoRepValueChanged;
                    break;
                case "2.0":
                    tmp = Rows20[1].Okpo;
                    tmp.PropertyChanged -= OkpoRepValueChanged;
                    tmp.PropertyChanged += OkpoRepValueChanged;
                    Rows20[0].Okpo.PropertyChanged -= OkpoRepValueChanged;
                    Rows20[0].Okpo.PropertyChanged += OkpoRepValueChanged;
                    break;
            }
            return tmp;
        }
        set
        {
            _OkpoRep = value.Value;
            OnPropertyChanged();
        }
    }

    private void OkpoRepValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        _OkpoRep = ((RamAccess<string>)value).Value;
        OnPropertyChanged(nameof(OkpoRep));
        OnPropertyChanged(nameof(RegNoRep));
        OnPropertyChanged(nameof(ShortJurLicoRep));
    }

    #endregion

    #region RegNoRep

    [NotMapped]
    private string _RegNoRep { get; set; } = "";

    [NotMapped]
    public RamAccess<string> RegNoRep
    {
        get
        {
            switch (FormNum_DB)
            {
                case "1.0":
                    {
                        RamAccess<string> tmp = null;
                        if ((Rows10[1].RegNo.Value != "" || Rows10[1].Okpo_DB == "-") && Rows10[1].Okpo.Value != "")
                        {
                            tmp = Rows10[1].RegNo;
                        }
                        else
                        {
                            tmp = Rows10[0].RegNo;
                        }
                        tmp.PropertyChanged -= RegNoRepValueChanged;
                        tmp.PropertyChanged += RegNoRepValueChanged;
                        return tmp;
                    }
                case "2.0":
                    {
                        RamAccess<string> tmp;
                        if ((Rows20[1].RegNo.Value != "" || Rows20[1].Okpo_DB == "-") && Rows20[1].Okpo.Value != "")
                        {
                            tmp = Rows20[1].RegNo;
                        }
                        else
                        {
                            tmp = Rows20[0].RegNo;
                        }
                        tmp.PropertyChanged -= RegNoRepValueChanged;
                        tmp.PropertyChanged += RegNoRepValueChanged;
                        return tmp;
                    }
                default:
                    return null;
            }
        }
        set
        {
            _RegNoRep = value.Value;
            OnPropertyChanged();
        }
    }

    private void RegNoRepValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        _RegNoRep = ((RamAccess<string>)value).Value;
        OnPropertyChanged(nameof(RegNoRep));
    }

    #endregion

    #region ShortJurLicoRep

    [NotMapped]
    public string _ShortJurLicoRep { get; set; } = "";

    [NotMapped]
    public RamAccess<string> ShortJurLicoRep
    {
        get
        {
            RamAccess<string> tmp = null;
            switch (FormNum_DB)
            {
                case "1.0" when Rows10[1].Okpo_DB is "" or "-":
                    tmp = Rows10[0].ShortJurLico;
                    tmp.PropertyChanged -= ShortJurLicoRepValueChanged;
                    tmp.PropertyChanged += ShortJurLicoRepValueChanged;
                    Rows10[1].ShortJurLico.PropertyChanged -= ShortJurLicoRepValueChanged;
                    Rows10[1].ShortJurLico.PropertyChanged += ShortJurLicoRepValueChanged;
                    break;
                case "1.0":
                    tmp = Rows10[1].ShortJurLico;
                    tmp.PropertyChanged -= ShortJurLicoRepValueChanged;
                    tmp.PropertyChanged += ShortJurLicoRepValueChanged;
                    Rows10[0].ShortJurLico.PropertyChanged -= ShortJurLicoRepValueChanged;
                    Rows10[0].ShortJurLico.PropertyChanged += ShortJurLicoRepValueChanged;
                    break;
                case "2.0" when Rows20[1].Okpo_DB is "" or "-":
                    tmp = Rows20[0].ShortJurLico;
                    tmp.PropertyChanged -= ShortJurLicoRepValueChanged;
                    tmp.PropertyChanged += ShortJurLicoRepValueChanged;
                    Rows20[1].ShortJurLico.PropertyChanged -= ShortJurLicoRepValueChanged;
                    Rows20[1].ShortJurLico.PropertyChanged += ShortJurLicoRepValueChanged;
                    break;
                case "2.0":
                    tmp = Rows20[1].ShortJurLico;
                    tmp.PropertyChanged -= ShortJurLicoRepValueChanged;
                    tmp.PropertyChanged += ShortJurLicoRepValueChanged;
                    Rows20[0].ShortJurLico.PropertyChanged -= ShortJurLicoRepValueChanged;
                    Rows20[0].ShortJurLico.PropertyChanged += ShortJurLicoRepValueChanged;
                    break;
            }
            return tmp;
        }
        set
        {
            _ShortJurLicoRep = value.Value;
            OnPropertyChanged(nameof(ShortJurLicoRep));
        }
    }

    private void ShortJurLicoRepValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName != "Value") return;
        _ShortJurLicoRep = ((RamAccess<string>)value).Value;
        OnPropertyChanged(nameof(ShortJurLicoRep));
    }

    #endregion

    #region StartPeriod

    public string StartPeriod_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Дата начала периода")]
    public RamAccess<string> StartPeriod
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(StartPeriod)))
            {
                ((RamAccess<string>)Dictionary[nameof(StartPeriod)]).Value = StartPeriod_DB;
                return (RamAccess<string>)Dictionary[nameof(StartPeriod)];
            }
            var rm = new RamAccess<string>(StartPeriod_Validation, StartPeriod_DB);
            rm.PropertyChanged += StartPeriodValueChanged;
            Dictionary.Add(nameof(StartPeriod), rm);
            return (RamAccess<string>)Dictionary[nameof(StartPeriod)];
        }
        set
        {
            StartPeriod_DB = value.Value;
            OnPropertyChanged(nameof(StartPeriod));
        }
    }

    private void StartPeriodValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)value).Value;
            //Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            //if (b.IsMatch(tmp))
            //{
            //    tmp = tmp.Insert(6, "20");
            //}
            StartPeriod_DB = tmp;
        }
    }

    private static bool StartPeriod_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        var tmp = value.Value;
        //Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
        //if (b.IsMatch(tmp))
        //{
        //    tmp = tmp.Insert(6, "20");
        //}
        //Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
        //if (!a.IsMatch(tmp))
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
        if (!DateTime.TryParse(tmp, out _))
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #region EndPeriod

    public string EndPeriod_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Дата конца периода")]
    public RamAccess<string> EndPeriod
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(EndPeriod)))
            {
                ((RamAccess<string>)Dictionary[nameof(EndPeriod)]).Value = EndPeriod_DB;
                return (RamAccess<string>)Dictionary[nameof(EndPeriod)];
            }
            var rm = new RamAccess<string>(EndPeriod_Validation, EndPeriod_DB);
            rm.PropertyChanged += EndPeriodValueChanged;
            Dictionary.Add(nameof(EndPeriod), rm);
            return (RamAccess<string>)Dictionary[nameof(EndPeriod)];
        }
        set
        {
            EndPeriod_DB = value.Value;
            OnPropertyChanged(nameof(EndPeriod));
        }
    }

    private void EndPeriodValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var tmp = ((RamAccess<string>)value).Value;
            //Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            //if (b.IsMatch(tmp))
            //{
            //    tmp = tmp.Insert(6, "20");
            //}
            EndPeriod_DB = tmp;
        }
    }

    private bool EndPeriod_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        //var tmp = value.Value;
        //Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
        //if (b.IsMatch(tmp))
        //{
        //    tmp = tmp.Insert(6, "20");
        //}
        //Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
        //if (!a.IsMatch(tmp))
        //{
        //    value.AddError("Недопустимое значение");
        //    return false;
        //}
        if (!DateTime.TryParse(StartPeriod_DB, out var startDateTime))
        {
            value.AddError("Недопустимое значение начала периода");
            return false;
        }
        if (!DateTime.TryParse(value.Value, out var endDateTime))
        {
            value.AddError("Недопустимое значение конца периода");
            return false;
        }
        if (startDateTime.Date > endDateTime.Date)
        {
            value.AddError("Начало периода должно быть раньше его конца");
            return false;
        }
        return true;
    }

    #endregion

    #region Year

    public string Year_DB { get; set; }

    [NotMapped]
    [FormProperty(true, "Отчетный год")]
    public RamAccess<string> Year
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Year)))
            {
                ((RamAccess<string>)Dictionary[nameof(Year)]).Value = Year_DB;
                return (RamAccess<string>)Dictionary[nameof(Year)];
            }
            var rm = new RamAccess<string>(Year_Validation, Year_DB);
            rm.PropertyChanged += YearValueChanged;
            Dictionary.Add(nameof(Year), rm);
            return (RamAccess<string>)Dictionary[nameof(Year)];
        }
        set
        {
            Year_DB = value.Value;
            OnPropertyChanged(nameof(Year));
        }
    }

    private void YearValueChanged(object value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            var k = ((RamAccess<string>)value).Value;
            Year_DB = k;
        }
    }

    private static bool Year_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (value.Value == null)
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (!short.TryParse(value.Value, out var shortValue) || shortValue is < 2010 or > 2060)
        {
            value.AddError("Недопустимое значение");
            return false;
        }
        return true;
    }

    #endregion

    #endregion

    #region Rows

    #region  Rows10

    ObservableCollectionWithItemPropertyChanged<Form10> Rows10_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form10> Rows10
    {
        get => Rows10_DB;
        set
        {
            Rows10_DB = value;
            OnPropertyChanged(nameof(Rows10));
        }
    }

    private void CollectionChanged10(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(RegNoRep));
        OnPropertyChanged(nameof(OkpoRep));
        OnPropertyChanged(nameof(ShortJurLicoRep));
        OnPropertyChanged(nameof(Rows10));
    }

    #endregion

    #region Rows11

    ObservableCollectionWithItemPropertyChanged<Form11> Rows11_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form11> Rows11
    {
        get => Rows11_DB;
        set
        {
            Rows11_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged11(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows11));
    }

    #endregion

    #region Rows12
    ObservableCollectionWithItemPropertyChanged<Form12> Rows12_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form12> Rows12
    {
        get => Rows12_DB;
        set
        {
            Rows12_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged12(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows12));
    }

    #endregion

    #region Rows13

    ObservableCollectionWithItemPropertyChanged<Form13> Rows13_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form13> Rows13
    {
        get => Rows13_DB;
        set
        {
            Rows13_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged13(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows13));
    }

    #endregion

    #region Rows14

    ObservableCollectionWithItemPropertyChanged<Form14> Rows14_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form14> Rows14
    {
        get => Rows14_DB;
        set
        {
            Rows14_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged14(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows14));
    }

    #endregion

    #region Rows15

    ObservableCollectionWithItemPropertyChanged<Form15> Rows15_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form15> Rows15
    {
        get => Rows15_DB;
        set
        {
            Rows15_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged15(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows15));
    }

    #endregion

    #region Rows16

    ObservableCollectionWithItemPropertyChanged<Form16> Rows16_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form16> Rows16
    {
        get => Rows16_DB;
        set
        {
            Rows16_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged16(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows16));
    }

    #endregion

    #region Rows17

    ObservableCollectionWithItemPropertyChanged<Form17> Rows17_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form17> Rows17
    {
        get => Rows17_DB;
        set
        {
            Rows17_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged17(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows17));
    }

    #endregion

    #region Rows18

    ObservableCollectionWithItemPropertyChanged<Form18> Rows18_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form18> Rows18
    {
        get => Rows18_DB;
        set
        {
            Rows18_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged18(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows18));
    }

    #endregion

    #region Rows19

    ObservableCollectionWithItemPropertyChanged<Form19> Rows19_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form19> Rows19
    {
        get => Rows19_DB;
        set
        {
            Rows19_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged19(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows19));
    }

    #endregion

    #region Rows20

    ObservableCollectionWithItemPropertyChanged<Form20> Rows20_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form20> Rows20
    {
        get => Rows20_DB;
        set
        {
            Rows20_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged20(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(RegNoRep));
        OnPropertyChanged(nameof(OkpoRep));
        OnPropertyChanged(nameof(ShortJurLicoRep));
        OnPropertyChanged(nameof(Rows20));
    }

    #endregion

    #region Rows21

    ObservableCollectionWithItemPropertyChanged<Form21> Rows21_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form21> Rows21
    {
        get => Rows21_DB;
        set
        {
            Rows21_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged21(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows21));
    }

    #endregion

    #region Rows22

    ObservableCollectionWithItemPropertyChanged<Form22> Rows22_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form22> Rows22
    {
        get => Rows22_DB;
        set
        {
            Rows22_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged22(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows22));
    }

    #endregion

    #region Rows23

    ObservableCollectionWithItemPropertyChanged<Form23> Rows23_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form23> Rows23
    {
        get => Rows23_DB;
        set
        {
            Rows23_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged23(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows23));
    }

    #endregion

    #region Rows24

    ObservableCollectionWithItemPropertyChanged<Form24> Rows24_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form24> Rows24
    {
        get => Rows24_DB;
        set
        {
            Rows24_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged24(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows24));
    }

    #endregion

    #region Rows25

    ObservableCollectionWithItemPropertyChanged<Form25> Rows25_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form25> Rows25
    {
        get => Rows25_DB;
        set
        {
            Rows25_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged25(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows25));
    }

    #endregion

    #region Rows26

    ObservableCollectionWithItemPropertyChanged<Form26> Rows26_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form26> Rows26
    {
        get => Rows26_DB;
        set
        {
            Rows26_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged26(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows26));
    }

    #endregion

    #region Rows27

    ObservableCollectionWithItemPropertyChanged<Form27> Rows27_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form27> Rows27
    {
        get => Rows27_DB;
        set
        {
            Rows27_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged27(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows27));
    }

    #endregion

    #region Rows28

    ObservableCollectionWithItemPropertyChanged<Form28> Rows28_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form28> Rows28
    {
        get => Rows28_DB;
        set
        {
            Rows28_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged28(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows28));
    }

    #endregion

    #region Rows29

    ObservableCollectionWithItemPropertyChanged<Form29> Rows29_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form29> Rows29
    {
        get => Rows29_DB;
        set
        {
            Rows29_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged29(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows29));
    }

    #endregion

    #region Rows210

    ObservableCollectionWithItemPropertyChanged<Form210> Rows210_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form210> Rows210
    {
        get => Rows210_DB;
        set
        {
            Rows210_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged210(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows210));
    }

    #endregion

    #region Rows211

    ObservableCollectionWithItemPropertyChanged<Form211> Rows211_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form211> Rows211
    {
        get => Rows211_DB;
        set
        {
            Rows211_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged211(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows211));
    }

    #endregion

    #region Rows212

    ObservableCollectionWithItemPropertyChanged<Form212> Rows212_DB;

    public virtual ObservableCollectionWithItemPropertyChanged<Form212> Rows212
    {
        get => Rows212_DB;
        set
        {
            Rows212_DB = value;
            OnPropertyChanged();
        }
    }

    private void CollectionChanged212(object sender, NotifyCollectionChangedEventArgs args)
    {
        OnPropertyChanged(nameof(Rows212));
    }

    #endregion 

    #endregion

    #region CleanIds

    public void CleanIds()
    {
        Id = 0;
        Rows10.CleanIds();
        Rows11.CleanIds();
        Rows12.CleanIds();
        Rows13.CleanIds();
        Rows14.CleanIds();
        Rows15.CleanIds();
        Rows16.CleanIds();
        Rows17.CleanIds();
        Rows18.CleanIds();
        Rows19.CleanIds();
        Rows20.CleanIds();
        Rows21.CleanIds();
        Rows22.CleanIds();
        Rows23.CleanIds();
        Rows24.CleanIds();
        Rows25.CleanIds();
        Rows26.CleanIds();
        Rows27.CleanIds();
        Rows28.CleanIds();
        Rows29.CleanIds();
        Rows210.CleanIds();
        Rows211.CleanIds();
        Rows212.CleanIds();
        Notes.CleanIds();
    }

    #endregion

    #region GetRows
    
    [NotMapped]
    public IKeyCollection this[string formNum]
    {
        get
        {
            return formNum switch
            {
                "1.0" => Rows10,
                "1.1" => Rows11,
                "1.2" => Rows12,
                "1.3" => Rows13,
                "1.4" => Rows14,
                "1.5" => Rows15,
                "1.6" => Rows16,
                "1.7" => Rows17,
                "1.8" => Rows18,
                "1.9" => Rows19,
                "2.0" => Rows20,
                "2.1" => Rows21,
                "2.2" => Rows22,
                "2.3" => Rows23,
                "2.4" => Rows24,
                "2.5" => Rows25,
                "2.6" => Rows26,
                "2.7" => Rows27,
                "2.8" => Rows28,
                "2.9" => Rows29,
                "2.10" => Rows210,
                "2.11" => Rows211,
                "2.12" => Rows212,
                _ => null
            };
        }
    }

    [NotMapped]
    public IKeyCollection Rows => this[FormNum_DB];

    #endregion

    #region CollectionChanged

    ~Report()
    {
        Rows10.CollectionChanged -= CollectionChanged10;
        Rows11.CollectionChanged -= CollectionChanged11;
        Rows12.CollectionChanged -= CollectionChanged12;
        Rows13.CollectionChanged -= CollectionChanged13;
        Rows14.CollectionChanged -= CollectionChanged14;
        Rows15.CollectionChanged -= CollectionChanged15;
        Rows16.CollectionChanged -= CollectionChanged16;
        Rows17.CollectionChanged -= CollectionChanged17;
        Rows18.CollectionChanged -= CollectionChanged18;
        Rows19.CollectionChanged -= CollectionChanged19;
        Rows20.CollectionChanged -= CollectionChanged20;
        Rows21.CollectionChanged -= CollectionChanged21;
        Rows22.CollectionChanged -= CollectionChanged22;
        Rows23.CollectionChanged -= CollectionChanged23;
        Rows24.CollectionChanged -= CollectionChanged24;
        Rows25.CollectionChanged -= CollectionChanged25;
        Rows26.CollectionChanged -= CollectionChanged26;
        Rows27.CollectionChanged -= CollectionChanged27;
        Rows28.CollectionChanged -= CollectionChanged28;
        Rows29.CollectionChanged -= CollectionChanged29;
        Rows210.CollectionChanged -= CollectionChanged210;
        Rows211.CollectionChanged -= CollectionChanged211;
        Rows212.CollectionChanged -= CollectionChanged212;
        Notes.CollectionChanged -= CollectionChangedNotes;
    }

    #endregion

    #region Sort

    public void Sort()
    {
        Rows11.QuickSort();
        Rows12.QuickSort();
        Rows13.QuickSort();
        Rows14.QuickSort();
        Rows15.QuickSort();
        Rows16.QuickSort();
        Rows17.QuickSort();
        Rows18.QuickSort();
        Rows19.QuickSort();
        Rows21.QuickSort();
        Rows22.QuickSort();
        Rows23.QuickSort();
        Rows24.QuickSort();
        Rows25.QuickSort();
        Rows26.QuickSort();
        Rows27.QuickSort();
        Rows28.QuickSort();
        Rows29.QuickSort();
        Rows210.QuickSort();
        Rows211.QuickSort();
        Rows212.QuickSort();
        Notes.QuickSort();
    }

    #endregion

    #region SortAsync
    
    public async Task SortAsync()
    {
        await Rows11.QuickSortAsync();
        await Rows12.QuickSortAsync();
        await Rows13.QuickSortAsync();
        await Rows14.QuickSortAsync();
        await Rows15.QuickSortAsync();
        await Rows16.QuickSortAsync();
        await Rows17.QuickSortAsync();
        await Rows18.QuickSortAsync();
        await Rows19.QuickSortAsync();
        await Rows21.QuickSortAsync();
        await Rows22.QuickSortAsync();
        await Rows23.QuickSortAsync();
        await Rows24.QuickSortAsync();
        await Rows25.QuickSortAsync();
        await Rows26.QuickSortAsync();
        await Rows27.QuickSortAsync();
        await Rows28.QuickSortAsync();
        await Rows29.QuickSortAsync();
        await Rows210.QuickSortAsync();
        await Rows211.QuickSortAsync();
        await Rows212.QuickSortAsync();
        await Notes.QuickSortAsync();
    }

    #endregion

    #region IExcel

    public void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        throw new NotImplementedException();
    }

    public int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        if (FormNum_DB.Split('.')[0] == "1")
        {
            worksheet.Cells[row, column].Value = CorrectionNumber_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = StartPeriod_DB;
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = EndPeriod_DB;
            return 3;
        }
        if (FormNum_DB.Split('.')[0] == "2")
        {
            worksheet.Cells[row, column].Value = CorrectionNumber_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = Year_DB;
            return 2;
        }
        return 0;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, string formNum, int row, int column, bool transpose = true)
    {
        if (formNum.Split('.')[0] == "1")
        {
            worksheet.Cells[row, column].Value = 
                ((FormPropertyAttribute)Type.GetType("Models.Collections.Report,Models")
                    ?.GetProperty(nameof(CorrectionNumber))
                    ?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())
                ?.Names[0];
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = 
                ((FormPropertyAttribute)Type.GetType("Models.Collections.Report,Models")
                    ?.GetProperty(nameof(StartPeriod))
                    ?.GetCustomAttributes(typeof(FormPropertyAttribute), false)
                    .First())
                ?.Names[0];
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value =
                ((FormPropertyAttribute)Type.GetType("Models.Collections.Report,Models")
                    ?.GetProperty(nameof(EndPeriod))
                    ?.GetCustomAttributes(typeof(FormPropertyAttribute), false)
                    .First())
                ?.Names[0];
            return 3;
        }
        if (formNum.Split('.')[0] == "2")
        {
            worksheet.Cells[row, column].Value = 
                ((FormPropertyAttribute)Type.GetType("Models.Collections.Report,Models")
                    ?.GetProperty(nameof(CorrectionNumber))
                    ?.GetCustomAttributes(typeof(FormPropertyAttribute), false)
                    .First())
                ?.Names[0];
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value =
                ((FormPropertyAttribute)Type.GetType("Models.Collections.Report,Models")
                    ?.GetProperty(nameof(Year))
                    ?.GetCustomAttributes(typeof(FormPropertyAttribute), false)
                    .First())
                ?.Names[0];
            return 2;
        }
        return 0;
    }

    #endregion

    #region Property Changed

    public event PropertyChangedEventHandler PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        if (prop != "All")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        else
        {
            var props = typeof(Report).GetProperties();
            foreach (var proper in props)
            {
                //if (proper.Name == nameof(EndPeriod))
                //{

                //}
                if (proper.Name.Contains("_DB")) continue;
                try
                {
                    var obj = proper.GetValue(this);
                    if (obj is RamAccess rm)
                    {
                        OnPropertyChanged(proper.Name);
                        rm.OnPropertyChanged("Value");
                    }
                }
                catch
                {
                    // ignored
                }
            }
        }
    }

    #endregion

    #region IDataGridColumn

    public DataGridColumns GetColumnStructure(string param = "")
    {
        #region formNumR
        var formNumR =
            ((FormPropertyAttribute)typeof(Report).GetProperty(nameof(FormNum))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD();
        formNumR.SizeCol = 100;
        formNumR.Binding = nameof(FormNum);
        #endregion

        switch (param)
        {
            case "1.0":
            {
                #region startPeriodR

                var startPeriodR =
                    ((FormPropertyAttribute)typeof(Report).GetProperty(nameof(StartPeriod))
                        ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                        .FirstOrDefault())
                    ?.GetDataColumnStructureD();
                if (startPeriodR != null)
                {
                    startPeriodR.SizeCol = 170;
                    startPeriodR.Binding = nameof(StartPeriod);
                    formNumR += startPeriodR;
                }

                #endregion

                #region endPeriodR

                var endPeriodR =
                    ((FormPropertyAttribute)typeof(Report).GetProperty(nameof(EndPeriod))
                        ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                        .FirstOrDefault())
                    ?.GetDataColumnStructureD();
                if (endPeriodR != null)
                {
                    endPeriodR.SizeCol = 170;
                    endPeriodR.Binding = nameof(EndPeriod);
                    formNumR += endPeriodR;
                }

                #endregion

                break;
            }
            case "2.0":
            {
                #region yearR

                var yearR =
                    ((FormPropertyAttribute)typeof(Report).GetProperty(nameof(Year))
                        ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                        .FirstOrDefault())
                    ?.GetDataColumnStructureD();
                if (yearR != null)
                {
                    yearR.SizeCol = 170;
                    yearR.Binding = nameof(Year);
                    formNumR += yearR;
                }

                #endregion

                break;
            }
        }

        #region exportDateR

        var exportDateR =
            ((FormPropertyAttribute)typeof(Report).GetProperty(nameof(ExportDate))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD();
        if (exportDateR != null)
        {
            exportDateR.SizeCol = 170;
            exportDateR.Binding = nameof(ExportDate);
            formNumR += exportDateR;
        } 

        #endregion

        #region correctionNumberR

        var correctionNumberR =
            ((FormPropertyAttribute)typeof(Report).GetProperty(nameof(CorrectionNumber))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD();
        if (correctionNumberR != null)
        {
            correctionNumberR.SizeCol = 170;
            correctionNumberR.Binding = nameof(CorrectionNumber);
            formNumR += correctionNumberR;
        }

        #endregion

        #region commentsR

        var commentsR =
            ((FormPropertyAttribute)typeof(Report).GetProperty(nameof(Comments))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), true)
                .FirstOrDefault())
            ?.GetDataColumnStructureD();
        if (commentsR != null)
        {
            commentsR.SizeCol = 343;
            commentsR.Binding = nameof(Comments);
            formNumR += commentsR;
        }

        #endregion

        return formNumR;
    }

    #endregion
}