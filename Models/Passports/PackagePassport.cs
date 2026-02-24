using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Models.Passports
{
    public class PackagePassport : INotifyPropertyChanged
    {
        #region Constructor
        public PackagePassport()
        {
            ContentCharacteristics = new ObservableCollection<CharacteristicOfContentRaoPackage>();
            ContentCharacteristics.CollectionChanged += (s, e) => { OnPropertyChanged(nameof(ContentCharacteristics)); };
        }
        #endregion

        #region Properties
        [MaxLength(30)]
        public string PassportNum { get; set; }
        public DateOnly PassportDate { get; set; }
        
        [MaxLength(100)]
        public string PackageType { get; set; }

        //OKPO
        public string StatusRaoCode { get; set; }

        [MaxLength(40)]
        public string TechSpecificationNum { get; set; }

        [MaxLength(50)]
        public string NameRao { get; set; }

        //3 или 4
        public byte ClassRao { get; set; }


        [MaxLength(50)]
        public string AgreementNumRaoDisposal { get; set; }
        public DateOnly? DateAgreementRaoDisposal { get; set; }

        [MaxLength(30)]
        public string UniquePackageNum { get; set; }

        [MaxLength(50)]
        public string TypePuod { get; set; }

        [MaxLength(256)]
        public string Owner { get; set; }

        //OKPO
        public string OwnerOkpo { get; set; }

        [MaxLength(256)]
        public string Manufacturer { get; set; }
        //OKPO
        public string ManufacturerOkpo { get; set; }
        [MaxLength(30)]
        public string CertificateConformityNum { get; set; }
        public DateOnly ManufactureDate { get; set; }
        public DateOnly StartPeriodCertificateConformity { get; set; }
        public DateOnly EndPeriodCertificateConformity { get; set; }
        public int ServiceLife { get; set; }
        public DateOnly? TransferDate { get; set; }

        #region Общая характеристика упаковки РАО

        [MaxLength(5)]
        public string DisposalMethod { get; set; }

        [MaxLength(1000)]
        public string PhysicochemicalForm { get; set; }
        [MaxLength(1000)]
        public string MorphologicalCompositionRao { get; set; }

        [MaxLength(256)]
        public string Flammability { get; set; }

        [MaxLength(50)]
        public string MatrixMaterialType { get; set; }

        public int? Diameter { get; set; }
        public int? Height { get; set; }
        public int? Length { get; set; }
        public int? Width { get; set; }
        public double PackageMass { get; set; }
        public double RaoMass { get; set; }
        public double PackageVolume { get; set; }
        public double RaoVolume { get; set; }
        public double RadiationDoseRate1m { get; set; }
        public double RadiationDoseRate10cm { get; set; }
        public double? LevelNonFixedPollutionAlpha { get; set; }
        public double? LevelNonFixedPollutionBetaGamma { get; set; }
        public double? PackageHeatOutput { get; set; }
        #endregion

        #region Характеристика радиоактивного содержимого упаковки РАО

        private ObservableCollection<CharacteristicOfContentRaoPackage> _contentCharacteristics;
        public ObservableCollection<CharacteristicOfContentRaoPackage> ContentCharacteristics
        {
            get => _contentCharacteristics;
            set
            {
                _contentCharacteristics = value;
                OnPropertyChanged();
            }
        }

        #endregion

        [MaxLength(2000)]
        public string Notes { get; set; }

        public string ResponsibleForTransferRao { get; set; }
        public string GradeAuthorizedPersonTransfer { get; set; }
        public string FioAuthorizedPersonTransfer { get; set; }
        public string ResponsibleForReceptionRao { get; set; }
        public string GradeAuthorizedPersonReception { get; set; }
        public string FioAuthorizedPersonReception { get; set; }
        #endregion

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class CharacteristicOfContentRaoPackage : INotifyPropertyChanged
    {
        public CharacteristicOfContentRaoPackage ()
        {
            _radionuclidsList = new ObservableCollection<RadionuclidDTO>();
        }
        [MaxLength(25)]
        public string PrimaryPackageType { get; set; }
        [MaxLength(15)]
        public string PrimaryPackageNum { get; set; }

        public byte ClassRao { get; set; }

        [MaxLength(11)] //Length
        public string CodeRao { get; set; }


        public DateOnly FillingWasteDate { get; set; }


        public int PrimaryPackageQuantity { get; set; }
        public double PrimaryPackageVolume { get; set; }
        public double PrimaryPackageMass { get; set; }
        
        //Как в справочнике


        public string RadionuclidComposition 
        {
            get
            {
                string result="";

                if (_radionuclidsList.Count>0)
                    result = _radionuclidsList[0].Name;

                for (int i = 1; i < _radionuclidsList.Count; i++)
                {
                    result += "; ";
                    result += _radionuclidsList[i].Name;
                }
                return result;
            }
        } 

        //Экспоненциальный вид
        public string Activity
        {
            get
            {
                string result="";

                if (_radionuclidsList.Count > 0)
                    result = _radionuclidsList[0].Activity.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));

                for (int i = 1; i < _radionuclidsList.Count; i++)
                {
                    result += "; ";
                    result += _radionuclidsList[i].Activity.ToString("e5", CultureInfo.CreateSpecificCulture("ru-RU"));
                }
                return result;
            }
        }

        [NotMapped]
        private ObservableCollection<RadionuclidDTO> _radionuclidsList;

        [NotMapped]
        public ObservableCollection<RadionuclidDTO> RadionuclidsList 
        { 
            get
            {
                return _radionuclidsList;
            }
            set
            {
                _radionuclidsList = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RadionuclidComposition));
                OnPropertyChanged(nameof(Activity));
            }
        }

       
        //Экспоненциальный вид
        public double LongLivingActivity { get; set; }
        //Экспоненциальный вид
        public double TransuraniumActivity { get; set; }
        //Экспоненциальный вид
        public double AlphaActivity { get; set; }
        //Экспоненциальный вид
        public double BetaGammaActivity { get; set; }
        //Экспоненциальный вид
        public double TritiumActivity { get; set; }
        //Экспоненциальный вид
        public double TotalActivity { get; set; }
        [MaxLength(50)]        
        public string NuclearHazardousFissileNuclides { get; set; }

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class RadionuclidDTO : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public double Activity { get; set; }

        #region OnPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
