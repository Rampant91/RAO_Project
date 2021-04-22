using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using Models;
using Models.Attributes;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using DBRealization;

namespace Collections
{
    public class Report : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        IDataAccess _dataAccess { get; set; }

        public int ID
        {
            get
            {
                return _dataAccess.ReportID;
            }
        }

        public Report(IDataAccess Access)
        {
            _dataAccess = Access;

            _Rows = new Basics.Rows();
            //_Rows.CollectionChanged += Update;       
        }

        void Update(object sender, EventArgs args)
        {
            OnPropertyChanged("GetFilteredRows");
            OnPropertyChanged("Rows");
        }

        Basics.Rows _Rows;
        public Basics.Rows Rows 
        {
            get
            {
                return _Rows;
            }
            set
            {
                if (value != _Rows)
                {
                    _Rows = value;
                    OnPropertyChanged("Rows");
                }
            }
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
                    return (bool)_dataAccess.Get(nameof(IsCorrection));
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
                    return (byte)_dataAccess.Get(nameof(CorrectionNumber));
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
                    return (string)_dataAccess.Get(nameof(NumberInOrder));
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
                    return (string)_dataAccess.Get(nameof(Comments));
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

        ////Notes property
        //[Form_Property("Примечания")]
        //public ObservableConcurrentDictionary<string, Note> Notes
        //{
        //    get
        //    {
        //        if (GetErrors(nameof(Notes)) == null)
        //        {
        //            return (ObservableConcurrentDictionary<string, Note>)_dataAccess.GetOne\(.*\,"");
        //        }
        //        else
        //        {
        //            return _Notes_Not_Valid;
        //        }
        //    }
        //    set
        //    {
        //        _Notes_Not_Valid = value;
        //        if (GetErrors(nameof(Notes)) == null)
        //        {
        //            _dataAccess.Set(nameof(Notes), _Notes_Not_Valid);
        //        }
        //        OnPropertyChanged(nameof(Notes));
        //    }
        //}
        //private ObservableConcurrentDictionary<string, Note> _Notes_Not_Valid = new ObservableConcurrentDictionary<string, Note>();
        //private bool Notes_Validation()
        //{
        //    return true;
        //}
        ////Notes property

        //StartPeriod
        [Form_Property("Начало")]
        public DateTimeOffset StartPeriod
        {
            get
            {
                if (GetErrors(nameof(StartPeriod)) == null)
                {
                    return (DateTimeOffset)_dataAccess.Get(nameof(StartPeriod));
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
                    return (DateTimeOffset)_dataAccess.Get(nameof(EndPeriod));
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
                    return (DateTimeOffset)_dataAccess.Get(nameof(ExportDate));
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
