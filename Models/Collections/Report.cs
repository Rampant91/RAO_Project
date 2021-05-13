using Models.DataAccess;
using Models.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Collections
{
    public class Report : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        IDataAccess _dataAccess { get; set; }

        public Report(IDataAccess Access)
        {
            _dataAccess = Access;
        }

        public Report()
        {
            _dataAccess = new Models.DataAccess.RamAccess();
        }

        [Key]
        public int ReportId { get; set; }

        void Update(object sender, EventArgs args)
        {
            OnPropertyChanged("GetFilteredRows10");
            OnPropertyChanged("Rows10");
        }

        public ObservableCollection<Models.Form10> Rows10
        {
            get
            {
                if (GetErrors(nameof(Rows10)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows10));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows10), new ObservableCollection<Models.Form10>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows10));
                    return (ObservableCollection<Models.Form10>)tmp;
                }
                else
                {
                    return _Rows10_Not_Valid;
                }
            }
            set
            {
                _Rows10_Not_Valid = value;
                if (GetErrors(nameof(Rows10)) == null)
                {
                    _dataAccess.Set(nameof(Rows10), _Rows10_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows10));
            }
        }
        private ObservableCollection<Models.Form10> _Rows10_Not_Valid = new ObservableCollection<Models.Form10>();
        private bool Rows10_Validation()
        {
            return true;
        }


        public ObservableCollection<Models.Form11> Rows11
        {
            get
            {
                if (GetErrors(nameof(Rows11)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows11));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows11), new ObservableCollection<Models.Form11>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows11));
                    return (ObservableCollection<Models.Form11>)tmp;
                }
                else
                {
                    return _Rows11_Not_Valid;
                }
            }
            set
            {
                _Rows11_Not_Valid = value;
                if (GetErrors(nameof(Rows11)) == null)
                {
                    _dataAccess.Set(nameof(Rows11), _Rows11_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows11));
            }
        }
        private ObservableCollection<Models.Form11> _Rows11_Not_Valid = new ObservableCollection<Models.Form11>();
        private bool Rows11_Validation()
        {
            return true;
        }


        public ObservableCollection<Models.Form12> Rows12
        {
            get
            {
                if (GetErrors(nameof(Rows12)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Rows12));
                    if (tmp == null)
                    {
                        _dataAccess.Set(nameof(Rows12), new ObservableCollection<Models.Form12>());
                    }
                    tmp = _dataAccess.Get(nameof(Rows12));
                    return (ObservableCollection<Models.Form12>)tmp;
                }
                else
                {
                    return _Rows12_Not_Valid;
                }
            }
            set
            {
                _Rows12_Not_Valid = value;
                if (GetErrors(nameof(Rows12)) == null)
                {
                    _dataAccess.Set(nameof(Rows12), _Rows12_Not_Valid);
                }
                OnPropertyChanged(nameof(Rows12));
            }
        }
        private ObservableCollection<Models.Form12> _Rows12_Not_Valid = new ObservableCollection<Models.Form12>();
        private bool Rows12_Validation()
        {
            return true;
        }

        [Form_Property("Форма")]
        public string FormNum { get; set; }

        //IsCorrection 
        [Form_Property("Корректирующий отчет")]
        public bool IsCorrection
        {
            get
            {
                if (GetErrors(nameof(IsCorrection)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(IsCorrection));
                    return tmp != null ? (bool)tmp : false;
                }
                else
                {
                    return _IsCorrection_Not_Valid;
                }
            }
            set
            {
                _IsCorrection_Not_Valid = value;
                if (GetErrors(nameof(IsCorrection)) == null)
                {
                    _dataAccess.Set(nameof(IsCorrection), _IsCorrection_Not_Valid);
                }
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        private bool _IsCorrection_Not_Valid = false;
        private bool IsCorrection_Validation()
        {
            return true;
        }
        //IsCorrection

        //CorrectionNumber property
        [Form_Property("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CorrectionNumber));
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) == null)
                {
                    _dataAccess.Set(nameof(CorrectionNumber), _CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

        //NumberInOrder property
        [Form_Property("Номер")]
        public string NumberInOrder
        {
            get
            {
                if (GetErrors(nameof(NumberInOrder)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(NumberInOrder));
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _NumberInOrder_Not_Valid;
                }
            }
            set
            {
                _NumberInOrder_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) == null)
                {
                    _dataAccess.Set(nameof(NumberInOrder), _NumberInOrder_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private string _NumberInOrder_Not_Valid = "";
        private bool NumberInOrder_Validation()
        {
            return true;
        }
        //NumberInOrder property

        //Comments property
        [Form_Property("Комментарий")]
        public string Comments
        {
            get
            {
                if (GetErrors(nameof(Comments)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Comments));
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Comments_Not_Valid;
                }
            }
            set
            {
                _Comments_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) == null)
                {
                    _dataAccess.Set(nameof(Comments), _Comments_Not_Valid);
                }
                OnPropertyChanged(nameof(Comments));
            }
        }
        private string _Comments_Not_Valid = "";
        private bool Comments_Validation()
        {
            return true;
        }
        //Comments property

        //Notes property
        [Form_Property("Примечания")]
        ObservableCollection<Models.Note> _notes = new ObservableCollection<Models.Note>();
        public ObservableCollection<Models.Note> Notes
        {
            get
            {
                if (GetErrors(nameof(Notes)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Notes));
                    return tmp != null ? (ObservableCollection<Models.Note>)tmp : null;
                }
                else
                {
                    return _Notes_Not_Valid;
                }
            }
            set
            {
                _Notes_Not_Valid = value;
                if (GetErrors(nameof(Notes)) == null)
                {
                    _dataAccess.Set(nameof(Notes), _Notes_Not_Valid);
                }
                OnPropertyChanged(nameof(Notes));
            }
        }
        private ObservableCollection<Models.Note> _Notes_Not_Valid = new ObservableCollection<Models.Note>();
        private bool Notes_Validation()
        {
            return true;
        }
        //Notes property

        //StartPeriod
        [Form_Property("Начало")]
        public DateTimeOffset StartPeriod
        {
            get
            {
                if (GetErrors(nameof(StartPeriod)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(StartPeriod));
                    return tmp != null ? (DateTimeOffset)tmp : DateTimeOffset.MinValue;
                }
                else
                {
                    return _StartPeriod_Not_Valid;
                }
            }
            set
            {
                _StartPeriod_Not_Valid = value;
                if (GetErrors(nameof(StartPeriod)) == null)
                {
                    _dataAccess.Set(nameof(StartPeriod), _StartPeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private DateTimeOffset _StartPeriod_Not_Valid = DateTimeOffset.Now;
        private void StartPeriod_Validation()
        {
            ClearErrors(nameof(StartPeriod));
            if (_StartPeriod_Not_Valid.Equals(DateTimeOffset.MinValue))
                AddError(nameof(StartPeriod), "Не заполнено начало периода");
            else
                if (!EndPeriod.Equals(DateTimeOffset.MinValue))
                if (_StartPeriod_Not_Valid.CompareTo(_EndPeriod_Not_Valid) > 0)
                    AddError(nameof(StartPeriod), "Начало периода не может быть позже его конца");
        }
        //StartPeriod

        //EndPeriod
        [Form_Property("Конец")]
        public DateTimeOffset EndPeriod
        {
            get
            {
                if (GetErrors(nameof(EndPeriod)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(EndPeriod));
                    return tmp != null ? (DateTimeOffset)tmp : DateTimeOffset.MinValue;
                }
                else
                {
                    return _EndPeriod_Not_Valid;
                }
            }
            set
            {
                _EndPeriod_Not_Valid = value;
                if (GetErrors(nameof(EndPeriod)) == null)
                {
                    _dataAccess.Set(nameof(EndPeriod), _EndPeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        private DateTimeOffset _EndPeriod_Not_Valid = DateTimeOffset.Now;
        private void EndPeriod_Validation()
        {
            ClearErrors(nameof(EndPeriod));
            if (_EndPeriod_Not_Valid.Equals(DateTimeOffset.MinValue))
                AddError(nameof(EndPeriod), "Не заполнено начало периода");
            else
                if (!EndPeriod.Equals(DateTimeOffset.MinValue))
                if (_StartPeriod_Not_Valid.CompareTo(_EndPeriod_Not_Valid) > 0)
                    AddError(nameof(EndPeriod), "Начало периода не может быть позже его конца");
        }
        //EndPeriod

        //ExportDate
        [Form_Property("Дата выгрузки")]
        public DateTimeOffset ExportDate
        {
            get
            {
                if (GetErrors(nameof(ExportDate)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ExportDate));
                    return tmp != null ? (DateTimeOffset)tmp : DateTimeOffset.MinValue;
                }
                else
                {
                    return _ExportDate_Not_Valid;
                }
            }
            set
            {
                _ExportDate_Not_Valid = value;
                if (GetErrors(nameof(ExportDate)) == null)
                {
                    _dataAccess.Set(nameof(ExportDate), _ExportDate_Not_Valid);
                }
                OnPropertyChanged(nameof(ExportDate));
            }
        }
        private DateTimeOffset _ExportDate_Not_Valid = DateTimeOffset.MinValue;
        private bool ExportDate_Validation()
        {
            return true;
        }
        //ExportDate

        //Property Changed
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        //Property Changed

        //Data Validation
        protected readonly Dictionary<string, List<string>> _errorsByPropertyName = new Dictionary<string, List<string>>();
        public bool HasErrors => _errorsByPropertyName.Any();
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public IEnumerable GetErrors(string propertyName)
        {
            var tmp = _errorsByPropertyName.ContainsKey(propertyName) ?
                _errorsByPropertyName[propertyName] : null;
            if (tmp != null)
            {
                List<Exception> lst = new List<Exception>();
                foreach (var item in tmp)
                {
                    lst.Add(new Exception(item));
                }
                return lst;
            }
            else
            {
                return null;
            }
        }
        protected void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }
        protected void ClearErrors(string propertyName)
        {
            if (_errorsByPropertyName.ContainsKey(propertyName))
            {
                _errorsByPropertyName.Remove(propertyName);
                OnErrorsChanged(propertyName);
            }
        }
        protected void AddError(string propertyName, string error)
        {
            if (!_errorsByPropertyName.ContainsKey(propertyName))
                _errorsByPropertyName[propertyName] = new List<string>();

            if (!_errorsByPropertyName[propertyName].Contains(error))
            {
                _errorsByPropertyName[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }
        //Data Validation
    }
}
