using Models.Forms.DataAccess;
using Models.Interfaces;
using Models.Passports;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Models.Forms.Form3
{
    [Serializable]
    [Table(name: "form_32_table_1")]
    public class Form32ExportedZriInfo : INotifyPropertyChanged, INotifyDataErrorInfo, ICopiable
    {
        #region Constructor
        public Form32ExportedZriInfo()
        {
        }
        public Form32ExportedZriInfo(Form32 form32)
        {
            _form32 = form32;
        }
        #endregion
        [Key]
        public int Id { get; set; }

        #region ForeignKey Form32Id
        public int Form32Id { get; set; }

        private Form32 _form32;

        [ForeignKey(nameof(Form32Id))]
        public Form32 Form32
        {
            get
            {
                return _form32;
            }
            private set
            {
                _form32 = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PassportNum (8.1)
        private string _PassportNum;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string PassportNum
        {
            get => _PassportNum;
            set
            {
                _PassportNum = value ?? string.Empty;
                ValidateString(nameof(PassportNum), _PassportNum);
                OnPropertyChanged();
            }
        }
        #endregion

        #region Type (8.2)
        private string _Type;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string Type
        {
            get => _Type;
            set
            {
                _Type = value ?? string.Empty;
                ValidateString(nameof(Type), _Type);
                OnPropertyChanged();
            }
        }
        #endregion

        #region FactoryNum (8.3)
        private string _FactoryNum;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string FactoryNum
        {
            get => _FactoryNum;
            set
            {
                _FactoryNum = value ?? string.Empty;
                ValidateString(nameof(FactoryNum), _FactoryNum);
                OnPropertyChanged();
            }
        }
        #endregion

        #region RadionuclidComposition (8.4)
        private string _radionuclidComposition;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string RadionuclidComposition
        {
            get => _radionuclidComposition;
            set
            {
                _radionuclidComposition = value ?? string.Empty;
                ValidateString(nameof(RadionuclidComposition), _radionuclidComposition);
                OnPropertyChanged();
            }
        }
        #endregion

        #region ReleaseDate (8.5)
        private DateOnly? _ReleaseDate;
        public DateOnly? ReleaseDate
        {
            get => _ReleaseDate;
            set
            {
                _ReleaseDate = value;
                ValidateDateOnly(nameof(ReleaseDate), _ReleaseDate);
                OnPropertyChanged();
            }
        }
        #endregion

        #region ActivityOnRealeseDate (8.6)
        private string _activityOnRealeseDate;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string ActivityOnRealeseDate
        {
            get => _activityOnRealeseDate;
            set
            {
                _activityOnRealeseDate = value ?? string.Empty;
                ValidateString(nameof(ActivityOnRealeseDate), _activityOnRealeseDate);
                OnPropertyChanged();
            }
        }
        #endregion

        #region NuclearMaterials (8.7)
        private string _nuclearMaterials;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string NuclearMaterials
        {
            get => _nuclearMaterials;
            set
            {
                _nuclearMaterials = value ?? string.Empty;
                ValidateString(nameof(NuclearMaterials), _nuclearMaterials);
                OnPropertyChanged();
            }
        }
        #endregion

        #region Category (8.8)
        private string _category;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string Category
        {
            get => _category;
            set
            {
                _category = value ?? string.Empty;
                ValidateString(nameof(Category), _category);
                OnPropertyChanged();
            }
        }
        #endregion

        #region ManufacturerOksm (8.9)
        private string _manufacturerOksm;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string ManufacturerOksm
        {
            get => _manufacturerOksm;
            set
            {
                _manufacturerOksm = value ?? string.Empty;
                ValidateString(nameof(ManufacturerOksm), _manufacturerOksm);
                OnPropertyChanged();
            }
        }
        #endregion

        #region CertificateNum (8.10)
        private string _certificateNum;

        [MaxLength(64)]
        [Column(TypeName = "varchar(64)")]
        public string CertificateNum
        {
            get => _certificateNum;
            set
            {
                _certificateNum = value ?? string.Empty;
                ValidateString(nameof(CertificateNum), _certificateNum);
                OnPropertyChanged();
            }
        }
        #endregion

        #region CertificateExpirationDate (8.11)
        private DateOnly? _certificateExpirationDate;
        public DateOnly? CertificateExpirationDate
        {
            get => _certificateExpirationDate;
            set
            {
                _certificateExpirationDate = value;
                ValidateDateOnly(nameof(CertificateExpirationDate), _certificateExpirationDate);
                OnPropertyChanged();
            }
        }
        #endregion


        #region ICopiable
        #region ConvertToTSVstring

        /// <summary>
        /// </summary>
        /// <returns>Возвращает строку с записанными данными в формате TSV(Tab-Separated Values) </returns>
        public string ConvertToTSVstring()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region PasteParsedTSVstring
        public void PasteParsedTSVstring(string[] parsedTSVstring)
        {
            throw new NotImplementedException();
        }
        #endregion
        #endregion

        #region Validation
        private void ValidateString(string propertyName, string str)
        {
            ClearErrors(propertyName);
            if (string.IsNullOrEmpty(str))
                AddError(propertyName, "Поле не заполнено");
        }

        private void ValidateDateOnly(string propertyName, DateOnly? dateOnly)
        {
            ClearErrors(propertyName);
            if (dateOnly is null || dateOnly == DateOnly.MinValue)
                AddError(propertyName, "Поле не заполнено");
        }
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
