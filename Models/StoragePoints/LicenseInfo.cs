using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models.StoragePoints
{
    [Serializable]
    [Table(name: "license_info")]
    public class LicenseInfo : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Constructor
        public LicenseInfo() 
        {
            Storage = new StoragePoint();
        }
        public LicenseInfo(StoragePoint storage)
        {
            Storage = storage;
        }
        #endregion 


        #region Properties
        [Key]
        public int Id { get; set; }

        #region StoragePoint

        [ForeignKey(nameof(Storage))]
        public int? StorageId { get; set; }

        private StoragePoint _storage;

        public StoragePoint Storage
        {
            get => _storage;
            set
            {
                _storage = value;
                OnPropertyChanged();
            }
        }



        #endregion

        #region LicenseName
        private string _licenseName;

        [MaxLength(64)]
        public string LicenseName
        {
            get => _licenseName;
            set
            {
                _licenseName = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region StartPeriod
        private DateOnly _startPeriod;

        public DateOnly StartPeriod
        {
            get => _startPeriod;
            set
            {
                _startPeriod = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region EndPeriod
        private DateOnly _endPeriod;

        public DateOnly EndPeriod
        {
            get => _endPeriod;
            set
            {
                _endPeriod = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CodeRAO (5)
        private string _codeRAO;

        [MaxLength(64)]
        public string CodeRAO
        {
            get => _codeRAO;
            set
            {
                _codeRAO = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Volume (6)
        private double _volume;

        public double Volume
        {
            get => _volume;
            set
            {
                _volume = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Mass (7)
        private double _mass;

        public double Mass
        {
            get => _mass;
            set
            {
                _mass = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region QuantityOZIII (8)
        private int _quantityOZIII;

        public int QuantityOZIII
        {
            get => _quantityOZIII;
            set
            {
                _quantityOZIII = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region SummaryActivity (9)
        private double _summaryActivity;

        public double SummaryActivity
        {
            get => _summaryActivity;
            set
            {
                _summaryActivity = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DocumentNumber (10)
        private string _documentNumber;

        [MaxLength(64)]
        public string DocumentNumber
        {
            get => _documentNumber;
            set
            {
                _documentNumber = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DocumentDate (11)
        private DateOnly _documentDate;

        public DateOnly DocumentDate
        {
            get => _documentDate;
            set
            {
                _documentDate = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ExpirationDate (12)
        private DateOnly _expirationDate;

        public DateOnly ExpirationDate
        {
            get => _expirationDate;
            set
            {
                _expirationDate = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region DocumentName (13)
        private string _documentName;

        [MaxLength(64)]
        public string DocumentName
        {
            get => _documentName;
            set
            {
                _documentName = value;
                LastUpdate = DateTime.Now;
                OnPropertyChanged();
            }
        }
        #endregion

        #region LastUpdate
        private DateTime _lastUpdate;

        public DateTime LastUpdate
        {
            get => _lastUpdate;
            private set
            {
                _lastUpdate = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion


        #region NotifyDataError


        private readonly Dictionary<string, List<string>> _errors = new();

        // Добавление ошибки
        private void AddError(string propertyName, string error)
        {
            if (!_errors.ContainsKey(propertyName))
                _errors[propertyName] = new List<string>();

            if (!_errors[propertyName].Contains(error))
            {
                _errors[propertyName].Add(error);
                OnErrorsChanged(propertyName);
            }
        }

        // Очистка ошибок свойства
        private void ClearErrors(string propertyName)
        {
            if (_errors.Remove(propertyName))
                OnErrorsChanged(propertyName);
        }

        // INotifyDataErrorInfo
        public bool HasErrors => _errors.Any();

        public event EventHandler<DataErrorsChangedEventArgs>? ErrorsChanged;

        protected virtual void OnErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        public IEnumerable GetErrors(string? propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return _errors.SelectMany(x => x.Value);

            return _errors.TryGetValue(propertyName, out var errors) ? errors : Enumerable.Empty<string>();
        }
        #endregion

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
