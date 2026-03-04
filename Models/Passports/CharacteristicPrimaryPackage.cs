using System;
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
    [Table(name: "characteristic_primary_package")]
    public class CharacteristicPrimaryPackage : INotifyPropertyChanged
    {
        public CharacteristicPrimaryPackage()
        {
            _passport = new PackagePassport();
            _radionuclidsList = new ObservableCollection<Radionuclid>();

        }
        public CharacteristicPrimaryPackage(PackagePassport passport)
        {
            _passport = passport;
            _radionuclidsList = new ObservableCollection<Radionuclid>();

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


        #region PrimaryPackageNum
        [NotMapped]
        private string _primaryPackageNum;

        [MaxLength(64)]
        public string PackageIdNum
        {
            get => _primaryPackageNum;
            set
            {
                _primaryPackageNum = value;
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

        [Length(11, 11)] //Length
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
        private int _primaryPackageQuantity;


        public int PrimaryPackageQuantity
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
                OnPropertyChanged();
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

        #region LongLivingActivity
        private double _longLivingActivity;
        //Экспоненциальный вид
        public double LongLivingActivity 
        {
            get => _longLivingActivity;
            set
            {
                _longLivingActivity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TransuraniumActivity
        private double _transuraniumActivity;
        //Экспоненциальный вид
        public double TransuraniumActivity
        {
            get => _transuraniumActivity;
            set
            {
                _transuraniumActivity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region AlphaActivity
        private double _alphaActivity;
        //Экспоненциальный вид
        public double AlphaActivity
        {
            get => _alphaActivity;
            set
            {
                _alphaActivity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region BetaGammaActivity
        private double _betaGammaActivity;
        //Экспоненциальный вид
        public double BetaGammaActivity
        {
            get => _betaGammaActivity;
            set
            {
                _betaGammaActivity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TritiumActivity
        private double _tritiumActivity;
        //Экспоненциальный вид
        public double TritiumActivity
        {
            get => _tritiumActivity;
            set
            {
                _tritiumActivity = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region TotalActivity
        private double _totalActivity;
        //Экспоненциальный вид
        public double TotalActivity
        {
            get => _totalActivity;
            set
            {
                _totalActivity = value;
                OnPropertyChanged();
            }
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

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
