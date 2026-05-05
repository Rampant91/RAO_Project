using Models.Attributes;
using Models.Forms;
using Models.Forms.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models.StoragePoint
{
    [Serializable]
    [Table(name: "storage_point")]
    public class StoragePoint : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        #region Constructor
        #endregion

        #region Properties

        [Key]
        public int Id { get; set; }

        #region SgukName
        private string _sgukName;

        [MaxLength(64)]
        public string SgukName
        {
            get => _sgukName;
            set
            {
                _sgukName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Code
        [NotMapped]
        private string _code;

        [MaxLength(64)]
        public string Code
        {
            get => _code;
            set
            {
                _code = value;
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
                OnPropertyChanged();
            }
        }
        #endregion

        #region EgrnName
        [NotMapped]
        private string _egrnName;

        [MaxLength(64)]
        public string EgrnName
        {
            get => _egrnName;
            set
            {
                _egrnName = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CadastreNum
        [NotMapped]
        private string _cadastreNum;

        [MaxLength(64)]
        public string CadastreNum
        {
            get => _cadastreNum;
            set
            {
                _cadastreNum = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ProjectVolume (4)
        private double _projectVolume;

        public double ProjectVolume
        {
            get => _projectVolume;
            set
            {
                //Exponentional
                _projectVolume = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CodeRAO (5)
        private string _codeRAO;

        public string CodeRAO
        {
            get => _codeRAO;
            set
            {
                _codeRAO = value;
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
                OnPropertyChanged();
            }
        }
        #endregion

        #region DocumentNumber (10)
        private string _documentNumber;

        public string DocumentNumber
        {
            get => _documentNumber;
            set
            {
                _documentNumber = value;
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
                OnPropertyChanged();
            }
        }
        #endregion

        #region IsRaoPlaced
        private bool _isRaoPlaced;

        public bool IsRaoPlaced
        {
            get => _isRaoPlaced;
            set
            {
                _isRaoPlaced = value;
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
