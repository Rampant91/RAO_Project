using Models.Attributes;
using Models.Forms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Models.Passports
{
    [Serializable]
    [Table(name: "package_passport")]
    public class PackagePassport : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        // Метод валидации OKPO
        private void ValidateOkpo(string propertyName, string okpo)
        {
            ClearErrors(propertyName);

            if (string.IsNullOrEmpty(okpo))
            {
                AddError(propertyName, "Поле обязательно для заполнения.");
            }
            else if (okpo.Length != 8 && _ownerOkpo.Length != 14)
            {
                AddError(propertyName, "Длина должна быть 8 или 14 символов.");
            }
            else if (!Form.OkpoRegex().IsMatch(okpo)) // Ваш регулярное выражение
            {
                AddError(propertyName, "Некорректный формат ОКПО.");
            }
        }

        #region Constructor
        public PackagePassport()
        {
            ContentCharacteristics = new ObservableCollection<CharacteristicPrimaryPackage>();
        }
        #endregion

        #region Properties

        #region PassportNum
        [NotMapped]
        private string _passportNum;

        [MaxLength(32)]
        public string PassportNum
        {
            get => _passportNum;
            set
            {
                _passportNum = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PassportDate
        [NotMapped]
        private DateOnly _passportDate;

        public DateOnly PassportDate
        {
            get => _passportDate;
            set
            {
                _passportDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PackageType
        [NotMapped]
        private string _packageType;

        [MaxLength(128)]
        public string PackageType
        {
            get => _packageType;
            set
            {
                _packageType = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region StatusRaoCode

        [NotMapped]
        private string _statusRaoCode;
        [MaxLength(16)]
        public string StatusRaoCode
        {
            get => _statusRaoCode;
            set
            {
                _statusRaoCode = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TechSpecification
        [NotMapped]
        private string _techSpecification;

        [MaxLength(64)]
        public string TechSpecification
        {
            get => _techSpecification;
            set
            {
                _techSpecification = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region NameRao
        [NotMapped]
        private string _nameRao;

        [MaxLength(64)]
        public string NameRao
        {
            get => _nameRao;
            set
            {
                _nameRao = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ClassRao
        [NotMapped]
        private byte _classRao;

        public byte ClassRao
        {
            get => _classRao;
            set
            {
                _classRao = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RaoDisposalNum
        [NotMapped]
        private string _raoDisposalNum;

        [MaxLength(64)]
        public string RaoDisposalNum
        {
            get => _raoDisposalNum;
            set
            {
                _raoDisposalNum = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RaoDisposalDate
        [NotMapped]
        private DateOnly? _raoDisposalDate;

        
        public DateOnly? RaoDisposalDate
        {
            get => _raoDisposalDate;
            set
            {
                _raoDisposalDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PackageIdCode
        [NotMapped]
        private string _packageIdCode;

        [MaxLength(32)]
        public string PackageIdCode
        {
            get => _packageIdCode;
            set
            {
                _packageIdCode = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TypeAndIdPuod
        [NotMapped]
        private string _typeAndIdPuod;

        [MaxLength(64)]
        public string TypeAndIdPuod
        {
            get => _typeAndIdPuod;
            set
            {
                _typeAndIdPuod = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Owner
        [NotMapped]
        private string _owner;

        [MaxLength(256)]
        public string Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region OwnerOkpo
        [NotMapped]
        private string _ownerOkpo;
        //OKPO
        [MaxLength(14)]
        public string OwnerOkpo
        {
            get => _ownerOkpo;
            set
            {
                _ownerOkpo = value;
                ValidateOkpo(nameof(OwnerOkpo), _ownerOkpo); // Валидация при изменении
                OnPropertyChanged();
            }
        }
        #endregion

        #region Manufacturer
        [NotMapped]
        private string _manufacturer;
        
        [MaxLength(256)]
        public string Manufacturer
        {
            get => _manufacturer;
            set
            {
                _manufacturer = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ManufacturerOkpo
        [NotMapped]
        private string _manufacturerOkpo;
        //OKPO
        [MaxLength(14)]
        public string ManufacturerOkpo
        {
            get => _manufacturerOkpo;
            set
            {
                _manufacturerOkpo = value;
                ValidateOkpo(nameof(ManufacturerOkpo), _manufacturerOkpo); // Валидация при изменении
                OnPropertyChanged();
            }
        }
        #endregion

        #region CertificateConformityNum
        [NotMapped]
        private string _certificateConformityNum;

        [MaxLength(32)]
        public string CertificateConformityNum
        {
            get => _certificateConformityNum;
            set
            {
                _certificateConformityNum = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ManufactureDate
        [NotMapped]
        private DateOnly _manufactureDate;

        
        public DateOnly ManufactureDate
        {
            get => _manufactureDate;
            set
            {
                _manufactureDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CertificateConformityStartPeriod
        [NotMapped]
        private DateOnly _certificateConformityStartPeriod;


        public DateOnly CertificateConformityStartPeriod
        {
            get => _certificateConformityStartPeriod;
            set
            {
                _certificateConformityStartPeriod = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region CertificateConformityEndPeriod
        [NotMapped]
        private DateOnly _certificateConformityEndPeriod;


        public DateOnly CertificateConformityEndPeriod
        {
            get => _certificateConformityEndPeriod;
            set
            {
                _certificateConformityEndPeriod = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ServiceLife
        [NotMapped]
        private int _serviceLife;


        public int ServiceLife
        {
            get => _serviceLife;
            set
            {
                _serviceLife = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TransferDate
        [NotMapped]
        private DateOnly? _transferDate;


        public DateOnly? TransferDate
        {
            get => _transferDate;
            set
            {
                _transferDate = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Общая характеристика упаковки РАО

        #region DisposalMethod
        [NotMapped]
        private string _disposalMethod;

        [MaxLength(8)]
        public string DisposalMethod
        {
            get => _disposalMethod;
            set
            {
                _disposalMethod = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PhysicochemicalForm
        [NotMapped]
        private string _physicochemicalForm;

        [MaxLength(1024)]
        public string PhysicochemicalForm
        {
            get => _physicochemicalForm;
            set
            {
                _physicochemicalForm = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region MorphologicalComposition
        [NotMapped]
        private string _morphologicalComposition;

        [MaxLength(1024)]
        public string MorphologicalComposition
        {
            get => _morphologicalComposition;
            set
            {
                _morphologicalComposition = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Flammability
        [NotMapped]
        private string _flammability;

        [MaxLength(256)]
        public string Flammability
        {
            get => _flammability;
            set
            {
                _flammability = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region MatrixMaterialType
        [NotMapped]
        private string _matrixMaterialType;

        [MaxLength(64)]
        public string MatrixMaterialType
        {
            get => _matrixMaterialType;
            set
            {
                _matrixMaterialType = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Diameter
        [NotMapped]
        private int? _diameter;


        public int? Diameter
        {
            get => _diameter;
            set
            {
                _diameter = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Height
        [NotMapped]
        private int? _height;


        public int? Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Length
        [NotMapped]
        private int? _length;


        public int? Length
        {
            get => _length;
            set
            {
                _length = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Width
        [NotMapped]
        private int? _width;


        public int? Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PackageMass
        [NotMapped]
        private double _packageMass;


        public double PackageMass
        {
            get => _packageMass;
            set
            {
                _packageMass = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RaoMass
        [NotMapped]
        private double _raoMass;


        public double RaoMass
        {
            get => _raoMass;
            set
            {
                _raoMass = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PackageVolume
        [NotMapped]
        private double _packageVolume;


        public double PackageVolume
        {
            get => _packageVolume;
            set
            {
                _packageVolume = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RaoVolume
        [NotMapped]
        private double _raoVolume;


        public double RaoVolume
        {
            get => _raoVolume;
            set
            {
                _raoVolume = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RadiationDoseRate10cm
        [NotMapped]
        private double _radiationDoseRate10cm;


        public double RadiationDoseRate10cm
        {
            get => _radiationDoseRate10cm;
            set
            {
                _radiationDoseRate10cm = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RadiationDoseRate1m
        [NotMapped]
        private double _radiationDoseRate1m;


        public double RadiationDoseRate1m
        {
            get => _radiationDoseRate1m;
            set
            {
                _radiationDoseRate1m = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region LevelNonFixedPollutionAlpha
        [NotMapped]
        private double? _levelNonFixedPollutionAlpha;


        public double? LevelNonFixedPollutionAlpha
        {
            get => _levelNonFixedPollutionAlpha;
            set
            {
                _levelNonFixedPollutionAlpha = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region LevelNonFixedPollutionBetaGamma
        [NotMapped]
        private double? _levelNonFixedPollutionBetaGamma;


        public double? LevelNonFixedPollutionBetaGamma
        {
            get => _levelNonFixedPollutionBetaGamma;
            set
            {
                _levelNonFixedPollutionBetaGamma = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region HeatOutput
        [NotMapped]
        private double? _heatOutput;


        public double? HeatOutput
        {
            get => _heatOutput;
            set
            {
                _heatOutput = value;
                OnPropertyChanged();
            }
        }
        #endregion
        #endregion

        #region Характеристика радиоактивного содержимого упаковки РАО

        private ObservableCollection<CharacteristicPrimaryPackage> _contentCharacteristics;
        public ObservableCollection<CharacteristicPrimaryPackage> ContentCharacteristics
        {
            get => _contentCharacteristics;
            set
            {
                _contentCharacteristics = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Notes
        [NotMapped]
        private string _notes;

        [MaxLength(2048)]
        public string Notes
        {
            get => _notes;
            set
            {
                _notes = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ResponsibleTransfer
        [NotMapped]
        private string _responsibleTransfer;

        [MaxLength(256)]
        public string ResponsibleTransfer
        {
            get => _responsibleTransfer;
            set
            {
                _responsibleTransfer = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region GradeAuthorizedPersonTransfer
        [NotMapped]
        private string _gradeAuthorizedPersonTransfer;

        [MaxLength(64)]
        public string GradeAuthorizedPersonTransfer
        {
            get => _gradeAuthorizedPersonTransfer;
            set
            {
                _gradeAuthorizedPersonTransfer = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region FioAuthorizedPersonTransfer
        [NotMapped]
        private string _fioAuthorizedPersonTransfer;

        [MaxLength(256)]
        public string FioAuthorizedPersonTransfer
        {
            get => _fioAuthorizedPersonTransfer;
            set
            {
                _fioAuthorizedPersonTransfer = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region ResponsibleReception
        [NotMapped]
        private string _responsibleReception;

        [MaxLength(256)]
        public string ResponsibleReception
        {
            get => _responsibleReception;
            set
            {
                _responsibleReception = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region GradeAuthorizedPersonReception
        [NotMapped]
        private string _gradeAuthorizedPersonReception;

        [MaxLength(64)]
        public string GradeAuthorizedPersonReception
        {
            get => _gradeAuthorizedPersonReception;
            set
            {
                _gradeAuthorizedPersonReception = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region FioAuthorizedPersonReception
        [NotMapped]
        private string _fioAuthorizedPersonReception;

        [MaxLength(256)]
        public string FioAuthorizedPersonReception
        {
            get => _fioAuthorizedPersonReception;
            set
            {
                _fioAuthorizedPersonReception = value;
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
