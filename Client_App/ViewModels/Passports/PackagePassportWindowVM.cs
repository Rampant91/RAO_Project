using Models.Passports;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Client_App.ViewModels.Passports
{
    public class PackagePassportWindowVM : BaseVM, INotifyPropertyChanged
    {
        #region Properties

        private PackagePassport _passport;
        public PackagePassport Passport
        {
            get
            {
                return _passport;
            }
            set
            {
                _passport = value;
                OnPropertyChanged();
            }
        }



        #endregion

        #region Constructor
        public PackagePassportWindowVM()
        {
            _passport = new PackagePassport();

            Passport.PassportNum = "КРАД-1,36/XXXX";
            Passport.PassportDate = new DateOnly(2024, 8, 20);
            Passport.PackageType = "КРАД-1,36";
            Passport.StatusRaoCode = "1";
            Passport.TechSpecificationNum = "ТУ 4100-027-07630224";
            Passport.NameRao = "твердые, очень низкоактивные";
            Passport.ClassRao = 4;

            Passport.AgreementNumRaoDisposal = "№319/ХХХ-Д от 14.04.2025";
            Passport.UniquePackageNum = "34371127/0000";
            Passport.TypePuod = "ПТН-О №№8722001, 8722002";
            Passport.Owner = "Филиал \"Звезда России\" АО \"Медведь\"";
            Passport.OwnerOkpo = "00000000";
            Passport.Manufacturer = "Акционерное общество \"ЭКОЛОГИЯ\" (АО \"ЭКОЛОГИЯ\") ";
            Passport.ManufacturerOkpo = "11111111";

            Passport.CertificateConformityNum = "ОИАЭ.RU.095(OC).01123";
            Passport.ManufactureDate = new DateOnly(2021, 9, 29);
            Passport.StartPeriodCertificateConformity = new DateOnly(2024, 9, 28);
            Passport.EndPeriodCertificateConformity = new DateOnly(2024, 8, 20);
            Passport.ServiceLife = 50;

            Passport.ResponsibleForTransferRao = "АО \"ЭКОЛОГИЯ\"";
            Passport.GradeAuthorizedPersonTransfer = "Генеральный директор";
            Passport.FioAuthorizedPersonTransfer = "Цветков А.Э.";
            Passport.ResponsibleForReceptionRao = "ФГУП \"НО РАО\" филиал \"Уральский\"";
            Passport.GradeAuthorizedPersonReception = "Главный специалист";
            Passport.FioAuthorizedPersonReception = "Вася Пупкин";

            Passport.DisposalMethod = "навал";
            Passport.PhysicochemicalForm = "Твердые\n" +
                "Заполнение упаковки - 98%\n" +
                "Свободная жидкость - отсутствует.\n" +
                "Материалы, реагирующие с водой с выделением самовоспламеняющихся или воспламеняющихся газов - отсутствуют.\n" +
                "Материалы, реагирующие с водой с выделением тепла - отсутствуют.\n" +
                "Материалы, способные выделять газы, пары, возгоны при взаимодействии с водой, воздухом или другими веществами - отсутствуют.";
            Passport.MorphologicalCompositionRao = "Спец. одежда, СИЗ, ветошь, обтир.\n" +
                "Коррозионно-активные вещества - отсутствуют.\n" +
                "Комплексообразующие вещества - отсутствуют.\n" +
                "Химические токсичные вещества отсутствуют.\n" +
                "Инфицирующие (патогенные) вещества - отсутствуют.\n";
            Passport.Flammability = "Самовозгорающиеся и легковоспламеняющиеся вещества - отсутствуют.";
            Passport.MatrixMaterialType = "песчано-цементная смесь";
            Passport.Diameter = 0;
            Passport.Height = 1375;
            Passport.Length = 1650;
            Passport.Width = 1651;
            Passport.PackageMass = 6920;
            Passport.PackageVolume = 3.74;
            Passport.RaoMass = 2300;
            Passport.RaoVolume = 1.47;
            Passport.RadiationDoseRate10cm = 0.15;
            Passport.RadiationDoseRate1m = 0.14;
            Passport.LevelNonFixedPollutionAlpha = 0.1;
            Passport.LevelNonFixedPollutionBetaGamma = 10;
            Passport.PackageHeatOutput = 1.1;


            Passport.ContentCharacteristics.Add(new CharacteristicOfContentRaoPackage()
            {
                PrimaryPackageType = Passport.PackageType,
                PrimaryPackageNum = Passport.UniquePackageNum,
                ClassRao = Passport.ClassRao,
            });

            OnPropertyChanged(nameof(Passport));
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
