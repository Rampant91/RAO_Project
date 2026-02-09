using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Models.Passports
{
    public class PackagePassport
    {
        string PassportNum;
        DateOnly PassportDate;

        string PackageType;

        byte StatusRaoCode;

        string TechSpecificationNum;

        string NameRao;
        byte ClassRao;

        string AgreementNumRaoDisposal;
        string UniquePackageNum;

        string TypePuod;
        string IdPuod;

        string OwnerRaoPackage;
        string ManufacturerRaoPackage;
        string CertificateConformityNum;
        DateOnly StartPeriodCertificateConformity;
        DateOnly EndPeriodCertificateConformity;
        int ServiceLife;

        DateOnly ManufactureDate;
        DateOnly TransferDate;

        #region Общая характеристика упаковки РАО

        string DisposalMethod;

        string PrimaryPackageType;
        string PrimaryPackageNum;
        int PrimaryPackageQuantity;
        double PrimaryPackageVolume;
        double PrimaryPackageMass;

        string? MatrixMaterialType;
        DateOnly FillingWasteDate;
        

        double Diameter;
        double Height;
        double Lenght;
        double Width;

        double PackageMass;
        double RaoMass;

        double RadiationDoseRate1m;
        double RadiationDoseRate10cm;

        double LevelNonFixedPollutionAlpha;
        double LevelNonFixedPollutionBetaGamma;

        double? PackageHeatOutput;
        #endregion

        #region Характеристика радиоактивного содержимого упаковки РАО

        //string string UniquePackageNum;

        //byte ClassRao;

        string CodeRao;

        string PhysicochemicalForm;

        string MorphologicalCompositionRao;

        string Flammability;

        string RadionuclidComposition;

        double Activity;

        double LongLivingActivity;
        double TransuraniumActivity;
        double AlphaActivity;
        double BetaGammaActivity;
        double TritiumActivity;

        double TotalActivity;

        string? NuclearHazardousFissileNuclides;

        #endregion

        string ResponsibleForTransferRao;
        string GradeFioAuthotizedPersonTransfer;

        string ResponsibleForReceptionRao;
        string GradeFioAuthotizedPersonReception;
    }
}
