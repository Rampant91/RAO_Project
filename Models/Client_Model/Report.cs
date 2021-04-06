using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections;

namespace Models.Client_Model
{
    public class Report : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        int ID { get; set; }
        public Report(int arg, bool isSQL)
        {
            if (isSQL)
            {

            }
            else
            {

            }

            _Filters = new Storage.Filter.Filter<Form>();
            _Filters.Filter_List.CollectionChanged += RowsChanged;
            _Filters.PropertyChanged += RowsChanged;
            _Rows.CollectionChanged += RowsChanged;
        }

        Row_Observable<Form> _Rows=new Row_Observable<Form>();
        public Row_Observable<Form> Rows 
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

        void RowsChanged(object sender, EventArgs args)
        {
            OnPropertyChanged("GetFilteredRows");
        }

        /// <summary>
        /// Дает итератор с фильтрованными и сортированными формами
        /// </summary>
        public IEnumerable<Client_Model.Form> GetFilteredRows
        {
            get
            {
                foreach (var item in Filters.CheckAndSort(_Rows.ToArray()))
                {
                    yield return item;
                }
            }
        }

        public string Name
        {
            get
            {
                if (Rows.Count > 0)
                {
                    return Rows[0].GetType().Name.Replace("Form", "");
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        Storage.Filter.Filter<Client_Model.Form> _Filters;
        public Storage.Filter.Filter<Client_Model.Form> Filters
        {
            get
            {
                return _Filters;
            }
            set
            {
                if (value.GetType() == _Filters.GetType())
                {
                    if (value != _Filters)
                    {
                        _Filters = value;
                        OnPropertyChanged("Filters");
                    }
                }
            }
        }

        [Attributes.FormVisual("Форма")]
        public string FormNum { get; set; }

        //IsCorrection  (пример идеальной реализации свойства)
        [Attributes.FormVisual("Корректирующий отчет")]
        public bool IsCorrection
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return (bool)_IsCorrection.Get();
                }
                else
                {
                    return _IsCorrection_Not_Valid;
                }
            }
            set
            {
                _IsCorrection_Not_Valid = value;
                if (GetErrors(nameof(IsCorrection)) != null)
                {
                    _IsCorrection.Set(_IsCorrection_Not_Valid);
                }
                OnPropertyChanged(nameof(IsCorrection));
            }
        }
        private IDataLoadEngine _IsCorrection;
        private bool _IsCorrection_Not_Valid = false;
        private bool IsCorrection_Validation()
        {
            return true;
        }
        //IsCorrection

        //CorrectionNumber property
        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return (byte)_CorrectionNumber.Get();
                }
                else
                {
                    return _CorrectionNumber_Not_Valid;
                }
            }
            set
            {
                _CorrectionNumber_Not_Valid = value;
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    _CorrectionNumber.Set(_CorrectionNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private IDataLoadEngine _CorrectionNumber;
        private byte _CorrectionNumber_Not_Valid = 255;
        private void CorrectionNumber_Validation()
        {
            ClearErrors(nameof(CorrectionNumber));
        }
        //CorrectionNumber property

        //NumberInOrder property
        [Attributes.FormVisual("Номер")]
        public string NumberInOrder
        {
            get
            {
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    return (string)_NumberInOrder.Get();
                }
                else
                {
                    return _NumberInOrder_Not_Valid;
                }
            }
            set
            {
                _NumberInOrder_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    _NumberInOrder.Set(_NumberInOrder_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private IDataLoadEngine _NumberInOrder;
        private string _NumberInOrder_Not_Valid = "";
        private bool NumberInOrder_Validation()
        {
            return true;
        }
        //NumberInOrder property

        //Comments property
        [Attributes.FormVisual("Комментарий")]
        public string Comments
        {
            get
            {
                if (GetErrors(nameof(Comments)) != null)
                {
                    return (string)_Comments.Get();
                }
                else
                {
                    return _Comments_Not_Valid;
                }
            }
            set
            {
                _Comments_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    _Comments.Set(_Comments_Not_Valid);
                }
                OnPropertyChanged(nameof(Comments));
            }
        }
        private IDataLoadEngine _Comments;
        private string _Comments_Not_Valid = "";
        private bool Comments_Validation()
        {
            return true;
        }
        //Comments property

        //Notes property
        [Attributes.FormVisual("Примечания")]
        public Client_Model.Note_Observable Notes
        {
            get
            {
                if (GetErrors(nameof(Notes)) != null)
                {
                    return (Client_Model.Note_Observable)_Notes.Get();
                }
                else
                {
                    return _Notes_Not_Valid;
                }
            }
            set
            {
                _Notes_Not_Valid = value;
                if (GetErrors(nameof(Notes)) != null)
                {
                    _Notes.Set(_Notes_Not_Valid);
                }
                OnPropertyChanged(nameof(Notes));
            }
        }
        private IDataLoadEngine _Notes;
        private Client_Model.Note_Observable _Notes_Not_Valid=new Note_Observable();
        private bool Notes_Validation()
        {
            return true;
        }
        //Notes property

        //StartPeriod
        [Attributes.FormVisual("Начало")]
        public DateTimeOffset StartPeriod
        {
            get
            {
                if (GetErrors(nameof(StartPeriod)) != null)
                {
                    return (DateTimeOffset)_StartPeriod.Get();
                }
                else
                {
                    return _StartPeriod_Not_Valid;
                }
            }
            set
            {
                _StartPeriod_Not_Valid = value;
                if (GetErrors(nameof(StartPeriod)) != null)
                {
                    _StartPeriod.Set(_StartPeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(StartPeriod));
            }
        }
        private IDataLoadEngine _StartPeriod;
        private DateTimeOffset _StartPeriod_Not_Valid = DateTimeOffset.Now;
        private bool StartPeriod_Validation()
        {
            ClearErrors(nameof(StartPeriod));
            if (_StartPeriod_Not_Valid.Equals(DateTimeOffset.MinValue))
                AddError(nameof(StartPeriod), "Не заполнено начало периода");
            else
                if (!EndPeriod.Equals(DateTimeOffset.MinValue))
                if (_StartPeriod_Not_Valid.CompareTo(_EndPeriod_Not_Valid) > 0)
                    AddError(nameof(StartPeriod), "Начало периода не может быть позже его конца");

            if (GetErrors(nameof(StartPeriod)) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //StartPeriod

        //EndPeriod
        [Attributes.FormVisual("Конец")]
        public DateTimeOffset EndPeriod
        {
            get
            {
                if (GetErrors(nameof(EndPeriod)) != null)
                {
                    return (DateTimeOffset)_EndPeriod.Get();
                }
                else
                {
                    return _EndPeriod_Not_Valid;
                }
            }
            set
            {
                _EndPeriod_Not_Valid = value;
                if (GetErrors(nameof(EndPeriod)) != null)
                {
                    _EndPeriod.Set(_EndPeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(EndPeriod));
            }
        }
        private IDataLoadEngine _EndPeriod;
        private DateTimeOffset _EndPeriod_Not_Valid = DateTimeOffset.Now;
        private bool EndPeriod_Validation()
        {
            ClearErrors(nameof(EndPeriod));
            if (_EndPeriod_Not_Valid.Equals(DateTimeOffset.MinValue))
                AddError(nameof(EndPeriod), "Не заполнено начало периода");
            else
                if (!EndPeriod.Equals(DateTimeOffset.MinValue))
                if (_StartPeriod_Not_Valid.CompareTo(_EndPeriod_Not_Valid) > 0)
                    AddError(nameof(EndPeriod), "Начало периода не может быть позже его конца");

            if (GetErrors(nameof(EndPeriod)) != null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        //EndPeriod

        //ExportDate
        [Attributes.FormVisual("Дата выгрузки")]
        public DateTimeOffset ExportDate
        {
            get
            {
                if (GetErrors(nameof(ExportDate)) != null)
                {
                    return (DateTimeOffset)_ExportDate.Get();
                }
                else
                {
                    return _ExportDate_Not_Valid;
                }
            }
            set
            {
                _ExportDate_Not_Valid = value;
                if (GetErrors(nameof(ExportDate)) != null)
                {
                    _ExportDate.Set(_ExportDate_Not_Valid);
                }
                OnPropertyChanged(nameof(ExportDate));
            }
        }
        private IDataLoadEngine _ExportDate;
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
