using Spravochniki;
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models.Passports
{
    [Serializable]
    [Table(name: "characteristic_package")]
    public class CharacteristicPrimaryPackage : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        private void ValidatePostiveDouble(string propertyName, double? value)
        {

            ClearErrors(propertyName);
            if (value == null) return;

            if (value < 0)
                AddError(propertyName, "Число должно быть положительным");
        }

        
        public CharacteristicPrimaryPackage()
        {
            _passport = new PackagePassport();
            _radionuclidsList = new ObservableCollection<Radionuclid>();

            _radionuclidsList.CollectionChanged += RadionuclidsList_CollectionChanged;

        }

        public CharacteristicPrimaryPackage(PackagePassport passport)
        {
            _passport = passport;
            _radionuclidsList = new ObservableCollection<Radionuclid>();

            _radionuclidsList.CollectionChanged += RadionuclidsList_CollectionChanged;
        }
        ~CharacteristicPrimaryPackage()
        {
            _radionuclidsList.CollectionChanged -= RadionuclidsList_CollectionChanged;
        }
        private void RadionuclidsList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            OnPropertyChanged(nameof(LongLivingActivity));
            OnPropertyChanged(nameof(AlphaActivity));
            OnPropertyChanged(nameof(BetaGammaActivity));
            OnPropertyChanged(nameof(TransuraniumActivity));
            OnPropertyChanged(nameof(TritiumActivity));
            OnPropertyChanged(nameof(TotalActivity));
        }
        public void UpdateRadionuclidsActivityType()
        {
            for (int i =0; i< RadionuclidsList.Count; i++)
                RadionuclidsList[i].GetGroupCode();
        }

        #region Properties

        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Passport))]
        public int? PassportId { get; set; }

        #region Passport
        [NotMapped]
        private PackagePassport _passport;
        public PackagePassport Passport
        {
            get
            {
                return _passport;
            }
        }
        #endregion


        #region PackageType
        [NotMapped]
        private string _packageType;

        [MaxLength(16)]
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

        #region PackageNum
        [NotMapped]
        private string _packageNum;

        [MaxLength(16)]
        public string PackageNum
        {
            get => _packageNum;
            set
            {
                _packageNum = value;
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

        #region CodeRao
        [NotMapped]
        private string _codeRao;

        //В коде РАО могут записываться несколько кодов РАО, длиной 11 символов
        //Разделяются ';'
        [MaxLength(128)] 
        public string CodeRao
        {
            get => _codeRao;
            set
            {
                _codeRao = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PrimaryPackageQuantity
        [NotMapped]
        private uint _primaryPackageQuantity;


        public uint PrimaryPackageQuantity
        {
            get => _primaryPackageQuantity;
            set
            {
                _primaryPackageQuantity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region PrimaryPackageVolume
        [NotMapped]
        private double _primaryPackageVolume;


        public double PrimaryPackageVolume
        {
            get => _primaryPackageVolume;
            set
            {
                _primaryPackageVolume = value; 
                ValidatePostiveDouble(nameof(PrimaryPackageVolume), _primaryPackageVolume);
                OnPropertyChanged();
            }
        }
        #endregion

        #region PrimaryPackageMass
        [NotMapped]
        private double _primaryPackageMass;


        public double PrimaryPackageMass
        {
            get => _primaryPackageMass;
            set
            {
                _primaryPackageMass = value;
                ValidatePostiveDouble(nameof(PrimaryPackageMass), _primaryPackageMass);
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalActivity));
            }
        }
        #endregion

        #region RadionuclidsList
        [NotMapped]
        private ObservableCollection<Radionuclid> _radionuclidsList;

        public ObservableCollection<Radionuclid> RadionuclidsList
        {
            get
            {
                return _radionuclidsList;
            }
            set
            {
                _radionuclidsList = value;


                OnPropertyChanged(nameof(RadionuclidsList));

            }
        }
        #endregion

        #region RadionuclidsComposition
        private string _radionuclidsComposition;

        // Предназначен для альтернативной записи списка наименований радионуклидов, входящих в состав
        [MaxLength(256)]
        public string RadionuclidsComposition
        {
            get => _radionuclidsComposition;
            set
            {
                _radionuclidsComposition = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region RadionuclidsActivity
        private string _radionuclidsActivity;

        //Экспоненциальный вид
        // Предназначен для альтернативной записи списка удельных активностей радионуклидов, входящих в состав
        [MaxLength(256)]
        public string RadionuclidsActivity
        {
            get => _radionuclidsActivity;
            set
            {
                _radionuclidsActivity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region LongLivingActivity
        //Экспоненциальный вид
        public double LongLivingActivity 
        {
            get
            {
                double result = 0;
                foreach (var radionuclid in RadionuclidsList.Where(rad => rad.IsLongLivingActivity))
                {
                    result += radionuclid.Activity;
                }
                return result;
            }
        }
        public void UpdateLongLivingActivity()
        {
            OnPropertyChanged(nameof(LongLivingActivity));
        }
        #endregion

        #region TransuraniumActivity
        public double TransuraniumActivity
        {

            get
            {
                double result = 0;
                foreach (var radionuclid in RadionuclidsList.Where(rad => rad.ActivityType == GroupCode.Transuranic))
                {
                    result += radionuclid.Activity;
                }
                return result;
            }
        }

        public void UpdateTransuraniumActivity()
        {
            OnPropertyChanged(nameof(TransuraniumActivity));
        }
        #endregion

        #region AlphaActivity
        public double AlphaActivity
        {
            get 
            {
                double result = 0;
                foreach (var radionuclid in RadionuclidsList.Where(rad => rad.ActivityType == GroupCode.Alpha))
                {
                    result += radionuclid.Activity;
                }
                return result;
            }
        }

        public void UpdateAlphaActivity()
        {
            OnPropertyChanged(nameof(AlphaActivity));
        }
        #endregion

        #region BetaGammaActivity
        public double BetaGammaActivity
        {
            get
            {
                double result = 0;
                foreach (var radionuclid in RadionuclidsList.Where(rad => rad.ActivityType == GroupCode.BetaGamma))
                {
                    result += radionuclid.Activity;
                }
                return result;
            }
        }

        public void UpdateBetaGammaActivity()
        {
            OnPropertyChanged(nameof(BetaGammaActivity));
        }
        #endregion

        #region TritiumActivity
        public double TritiumActivity
        {
            get
            {
                double result = 0;
                foreach (var radionuclid in RadionuclidsList.Where(rad => rad.ActivityType == GroupCode.Tritium))
                {
                    result += radionuclid.Activity;
                }
                return result;
            }
        }

        public void UpdateTritiumActivity()
        {
            OnPropertyChanged(nameof(TritiumActivity));
        }
        #endregion

        #region TotalActivity
        //Экспоненциальный вид
        public double TotalActivity
        {
            get
            {
                double result = 0;
                result += TransuraniumActivity;
                result += AlphaActivity;
                result += BetaGammaActivity;
                result += TritiumActivity;

                result *= Passport.RaoMass * 1000;
                return result;
            }
        }
        public void UpdateTotalActivity()
        {
            OnPropertyChanged(nameof(TotalActivity));
        }

        #endregion

        #region NuclearHazardousFissileNuclides
        private string _nuclearHazardousFissileNuclides;
        
        [MaxLength(64)]
        public string NuclearHazardousFissileNuclides
        {
            get => _nuclearHazardousFissileNuclides;
            set
            {
                _nuclearHazardousFissileNuclides = value;
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
