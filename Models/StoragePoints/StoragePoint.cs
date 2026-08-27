using Models.Attributes;
using Models.Forms;
using Models.Forms.DataAccess;
using Models.Passports;
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

namespace Models.StoragePoints
{
    [Serializable]
    [Table(name: "storage_point")]
    public class StoragePoint : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Constructor

        public StoragePoint()
        {
            _licenseInfoList = new ObservableCollection<LicenseInfo>();
        }

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
                if (value != _sgukName)
                {
                    _sgukName = value;
                    SgukNameLastUpdate = DateTime.Now;
                    OnPropertyChanged();
                }
            }
        }

        #region SgukNameLastUpdate
        private DateTime _sgukNameLastUpdateDate;

        public DateTime SgukNameLastUpdate
        {
            get => _sgukNameLastUpdateDate;
            private set
            {
                _sgukNameLastUpdateDate = value;
                OnPropertyChanged();
            }
        }
        #endregion
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

        #region EgrnName
        [NotMapped]
        private string _egrnName;

        [MaxLength(64)]
        public string EgrnName
        {
            get => _egrnName;
            set
            {
                if (value != _egrnName)
               { 
                    _egrnName = value;
                    EgrnNameLastUpdate = DateTime.Now;
                    OnPropertyChanged();
                }
            }
        }
        #region EgrnNameLastUpdate
        private DateTime _egrnNameLastUpdateDate;

        public DateTime EgrnNameLastUpdate
        {
            get => _egrnNameLastUpdateDate;
            private set
            {
                _egrnNameLastUpdateDate = value;
                OnPropertyChanged();
            }
        }
        #endregion
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
                if (value != _cadastreNum)
                {
                    _cadastreNum = value;
                    CadastreNumLastUpdate = DateTime.Now;
                    OnPropertyChanged();
                }

            }
        }

        #region CadastreNumLastUpdate
        private DateTime _cadastreNumLastUpdate;

        public DateTime CadastreNumLastUpdate
        {
            get => _cadastreNumLastUpdate;
            private set
            {
                _cadastreNumLastUpdate = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion

        #region ProjectVolume (4)
        private double _projectVolume;

        public double ProjectVolume
        {
            get => _projectVolume;
            set
            {
                _projectVolume = value;
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

        #region LicenseInfoList
        private ObservableCollection<LicenseInfo> _licenseInfoList;
        public ObservableCollection<LicenseInfo> LicenseInfoList
        {
            get
            {
                return _licenseInfoList;
            }
            set
            {
                _licenseInfoList = value;

                OnPropertyChanged(nameof(LicenseInfoList));

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
