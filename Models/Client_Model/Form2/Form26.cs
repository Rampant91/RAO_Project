using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.6: Контроль загрязнения подземных вод РВ")]
    public class Form26 : Form2
    {
        public Form26() : base()
        {
            FormNum = "26";
            NumberOfFields = 11;
            //if (isSQL)
            //{
            //    _CodeOYATnote = new SQLite("CodeOYATnote", FormNum, 0);
            //    _MassCreated = new SQLite("MassCreated", FormNum, 0);
            //    _QuantityCreated = new SQLite("QuantityCreated", FormNum, 0);
            //    _QuantityCreatedNote = new SQLite("QuantityCreatedNote", FormNum, 0);
            //    _MassFromAnothers = new SQLite("MassFromAnothers", FormNum, 0);
            //    _QuantityFromAnothers = new SQLite("QuantityFromAnothers", FormNum, 0);
            //    _QuantityFromAnothersNote = new SQLite("QuantityFromAnothersNote", FormNum, 0);
            //    _MassFromAnothersImported = new SQLite("MassFromAnothersImported", FormNum, 0);
            //    _QuantityFromAnothersImported = new SQLite("QuantityFromAnothersImported", FormNum, 0);
            //    _QuantityFromAnothersImportedNote = new SQLite("QuantityFromAnothersImportedNote", FormNum, 0);
            //    _MassAnotherReasons = new SQLite("MassAnotherReasons", FormNum, 0);
            //    _QuantityAnotherReasons = new SQLite("QuantityAnotherReasons", FormNum, 0);
            //    _QuantityAnotherReasonsNote = new SQLite("QuantityAnotherReasonsNote", FormNum, 0);
            //    _MassRefined = new SQLite("MassRefined", FormNum, 0);
            //    _MassTransferredToAnother = new SQLite("MassTransferredToAnother", FormNum, 0);
            //    _QuantityTransferredToAnother = new SQLite("QuantityTransferredToAnother", FormNum, 0);
            //    _QuantityTransferredToAnotherNote = new SQLite("CodeOYAT", FormNum, 0);
            //    _QuantityRefined = new SQLite("QuantityRefined", FormNum, 0);
            //    _QuantityRefinedNote = new SQLite("QuantityRefinedNote", FormNum, 0);
            //    _MassRemovedFromAccount = new SQLite("MassRemovedFromAccount", FormNum, 0);
            //    _QuantityRemovedFromAccount = new SQLite("QuantityRemovedFromAccount", FormNum, 0);
            //    _QuantityRemovedFromAccountNote = new SQLite("QuantityRemovedFromAccountNote", FormNum, 0);
            //    _CodeOYAT = new SQLite("CodeOYAT", FormNum, 0);
            //    _DocumentDate = new SQLite("DocumentDate", FormNum, 0);
            //    _DocumentName = new SQLite("DocumentName", FormNum, 0);
            //    _DocumentNumber = new SQLite("DocumentNumber", FormNum, 0);
            //    _DocumentNumberRecoded = new SQLite("DocumentNumberRecoded", FormNum, 0);
            //    _ExpirationDate = new SQLite("ExpirationDate", FormNum, 0);
            //    _ProjectVolume = new SQLite("ProjectVolume", FormNum, 0);
            //    _ProjectVolumeNote = new SQLite("ProjectVolumeNote", FormNum, 0);
            //    _SummaryActivity = new SQLite("SummaryActivity", FormNum, 0);
            //    _QuantityOZIII = new SQLite("QuantityOZIII", FormNum, 0);
            //    _PackQuantity = new SQLite("PackQuantity", FormNum, 0);
            //    _MassInPack = new SQLite("MassInPack", FormNum, 0);
            //    _VolumeInPack = new SQLite("VolumeInPack", FormNum, 0);
            //    _RefineMachineName = new SQLite("RefineMachineName", FormNum, 0);
            //    _MachineCode = new SQLite("MachineCode", FormNum, 0);
            //    _MachinePower = new SQLite("MachinePower", FormNum, 0);
            //    _NumberOfHoursPerYear = new SQLite("NumberOfHoursPerYear", FormNum, 0);
            //    _CodeRAOIn = new SQLite("CodeRAOIn", FormNum, 0);
            //    _StatusRAOIn = new SQLite("StatusRAOIn", FormNum, 0);
            //    _VolumeIn = new SQLite("VolumeIn", FormNum, 0);
            //    _MassIn = new SQLite("MassIn", FormNum, 0);
            //    _QuantityIn = new SQLite("QuantityIn", FormNum, 0);
            //    _CodeRAOout = new SQLite("CodeRAOout", FormNum, 0);
            //    _StatusRAOout = new SQLite("StatusRAOout", FormNum, 0);
            //    _VolumeOut = new SQLite("VolumeOut", FormNum, 0);
            //    _MassOut = new SQLite("MassOut", FormNum, 0);
            //    _TritiumActivityIn = new SQLite("TritiumActivityIn", FormNum, 0);
            //    _TritiumActivityOut = new SQLite("TritiumActivityOut", FormNum, 0);
            //    _QuantityOZIIIout = new SQLite("QuantityOZIIIout", FormNum, 0);
            //    _TransuraniumActivityIn = new SQLite("TransuraniumActivityIn", FormNum, 0);
            //    _TransuraniumActivityOut = new SQLite("TransuraniumActivityOut", FormNum, 0);
            //    _BetaGammaActivityIn = new SQLite("BetaGammaActivityIn", FormNum, 0);
            //    _AlphaActivityIn = new SQLite("AlphaActivityIn", FormNum, 0);
            //    _BetaGammaActivityOut = new SQLite("BetaGammaActivityOut", FormNum, 0);
            //    _AlphaActivityOut = new SQLite("AlphaActivityOut", FormNum, 0);
            //    _RegNo = new SQLite("RegNo", FormNum, 0);
            //    _OrganUprav = new SQLite("OrganUprav", FormNum, 0);
            //    _SubjectRF = new SQLite("SubjectRF", FormNum, 0);
            //    _JurLico = new SQLite("JurLico", FormNum, 0);
            //    _ShortJurLico = new SQLite("ShortJurLico", FormNum, 0);
            //    _JurLicoAddress = new SQLite("JurLicoAddress", FormNum, 0);
            //    _JurLicoFactAddress = new SQLite("JurLicoFactAddress", FormNum, 0);
            //    _GradeFIO = new SQLite("GradeFIO", FormNum, 0);
            //    _Telephone = new SQLite("Telephone", FormNum, 0);
            //    _Fax = new SQLite("Fax", FormNum, 0);
            //    _Email = new SQLite("Email", FormNum, 0);
            //    _Okpo = new SQLite("Okpo", FormNum, 0);
            //    _Okved = new SQLite("Okved", FormNum, 0);
            //    _Okogu = new SQLite("Okogu", FormNum, 0);
            //    _Oktmo = new SQLite("Oktmo", FormNum, 0);
            //    _Inn = new SQLite("Inn", FormNum, 0);
            //    _Kpp = new SQLite("Kpp", FormNum, 0);
            //    _Okopf = new SQLite("Okopf", FormNum, 0);
            //    _Okfs = new SQLite("Okfs", FormNum, 0);
            //    _Volume20 = new SQLite("Volume20", FormNum, 0);
            //    _Volume6 = new SQLite("Volume6", FormNum, 0);
            //    _SaltConcentration = new SQLite("SaltConcentration", FormNum, 0);
            //    _SpecificActivity = new SQLite("SpecificActivity", FormNum, 0);
            //    _Mass21 = new SQLite("Mass21", FormNum, 0);
            //    _Mass7 = new SQLite("Mass7", FormNum, 0);
            //    _IndividualNumberZHRO = new SQLite("IndividualNumberZHRO", FormNum, 0);
            //    _IndividualNumberZHROrecoded = new SQLite("IndividualNumberZHRORecoded", FormNum, 0);
            //    _VolumeOutOfPack = new SQLite("VolumeOutOfPack", FormNum, 0);
            //    _PackFactoryNumber = new SQLite("PackFactoryNumber", FormNum, 0);
            //    _MassOutOfPack = new SQLite("MassOutOfPack", FormNum, 0);
            //    _FormingDate = new SQLite("FormingDate", FormNum, 0);
            //    _MainRadionuclids = new SQLite("MainRadionuclids", FormNum, 0);
            //    _CodeRAO = new SQLite("CodeRAO", FormNum, 0);
            //    _AlphaActivity = new SQLite("AlphaActivity", FormNum, 0);
            //    _BetaGammaActivity = new SQLite("BetaGammaActivity", FormNum, 0);
            //    _TritiumActivity = new SQLite("TritiumActivity", FormNum, 0);
            //    _TransuraniumActivity = new SQLite("TransuraniumActivity", FormNum, 0);
            //    _StoragePlaceCode = new SQLite("StoragePlaceCode", FormNum, 0);
            //    _StoragePlaceName = new SQLite("StoragePlaceName", FormNum, 0);
            //    _Subsidy = new SQLite("Subsidy", FormNum, 0);
            //    _StoragePlaceNameNote = new SQLite("StoragePlaceNameNote", FormNum, 0);
            //    _StatusRAO = new SQLite("StatusRAO", FormNum, 0);
            //    _RefineOrSortRAOCode = new SQLite("RefineOrSortRAOCode", FormNum, 0);
            //    _FcpNumber = new SQLite("FcpNumber", FormNum, 0);
            //    _Radionuclids = new SQLite("Radionuclids", FormNum, 0);
            //    _Volume = new SQLite("Volume", FormNum, 0);
            //    _Mass = new SQLite("Mass", FormNum, 0);
            //    _ActivityMeasurementDate = new SQLite("ActivityMeasurementDate", FormNum, 0);
            //    _Sort = new SQLite("Sort", FormNum, 0);
            //    _Name = new SQLite("Name", FormNum, 0);
            //    _AggregateState = new SQLite("AggregateState", FormNum, 0);
            //    _PassportNumber = new SQLite("PassportNumber", FormNum, 0);
            //    _PassportNumberRecoded = new SQLite("PassportNumberRecoded", FormNum, 0);
            //    _PassportNumberNote = new SQLite("PassportNumberNote", FormNum, 0);
            //    _Type = new SQLite("Type", FormNum, 0);
            //    _TypeRecoded = new SQLite("TypeRecoded", FormNum, 0);
            //    _Radionuclids = new SQLite("Radionuclids", FormNum, 0);
            //    _FactoryNumber = new SQLite("FactoryNumber", FormNum, 0);
            //    _FactoryNumberRecoded = new SQLite("FactoryNumberRecoded", FormNum, 0);
            //    _Quantity = new SQLite("Quantity", FormNum, 0);
            //    _Activity = new SQLite("Activity", FormNum, 0);
            //    _ActivityNote = new SQLite("ActivityNote", FormNum, 0);
            //    _CreationDate = new SQLite("CreationDate", FormNum, 0);
            //    _CreatorOKPO = new SQLite("CreatorOKPO", FormNum, 0);
            //    _CreatorOKPONote = new SQLite("CreatorOKPONote", FormNum, 0);
            //    _Category = new SQLite("Category", FormNum, 0);
            //    _SignedServicePeriod = new SQLite("SignedServicePeriod", FormNum, 0);
            //    _PropertyCode = new SQLite("PropertyCode", FormNum, 0);
            //    _Owner = new SQLite("Owner", FormNum, 0);
            //    _ProviderOrRecieverOKPO = new SQLite("ProviderOrRecieverOKPO", FormNum, 0);
            //    _ProviderOrRecieverOKPONote = new SQLite("ProviderOrRecieverOKPONote", FormNum, 0);
            //    _TransporterOKPO = new SQLite("TransporterOKPO", FormNum, 0);
            //    _TransporterOKPONote = new SQLite("TransporterOKPONote", FormNum, 0);
            //    _PackName = new SQLite("PackName", FormNum, 0);
            //    _PackNameNote = new SQLite("PackNameNote", FormNum, 0);
            //    _PackType = new SQLite("PackType", FormNum, 0);
            //    _PackTypeRecoded = new SQLite("PackTypeRecoded", FormNum, 0);
            //    _PackTypeNote = new SQLite("PackTypeNote", FormNum, 0);
            //    _PackNumber = new SQLite("PackNumber", FormNum, 0);
            //    _PackNumberRecoded = new SQLite("PackNumberRecoded", FormNum, 0);
            //}
            //else
            //{
            //    _CodeOYATnote = new File();
            //    _MassCreated = new File();
            //    _QuantityCreated = new File();
            //    _QuantityCreatedNote = new File();
            //    _MassFromAnothers = new File();
            //    _QuantityFromAnothers = new File();
            //    _QuantityFromAnothersNote = new File();
            //    _MassFromAnothersImported = new File();
            //    _QuantityFromAnothersImported = new File();
            //    _QuantityFromAnothersImportedNote = new File();
            //    _MassAnotherReasons = new File();
            //    _QuantityAnotherReasons = new File();
            //    _QuantityAnotherReasonsNote = new File();
            //    _MassRefined = new File();
            //    _MassTransferredToAnother = new File();
            //    _QuantityTransferredToAnother = new File();
            //    _QuantityTransferredToAnotherNote = new File();
            //    _QuantityRefined = new File();
            //    _QuantityRefinedNote = new File();
            //    _MassRemovedFromAccount = new File();
            //    _QuantityRemovedFromAccount = new File();
            //    _QuantityRemovedFromAccountNote = new File();
            //    _CodeOYAT = new File();
            //    _DocumentDate = new File();
            //    _DocumentName = new File();
            //    _DocumentNumber = new File();
            //    _DocumentNumberRecoded = new File();
            //    _ExpirationDate = new File();
            //    _ProjectVolume = new File();
            //    _ProjectVolumeNote = new File();
            //    _SummaryActivity = new File();
            //    _QuantityOZIII = new File();
            //    _PackQuantity = new File();
            //    _MassInPack = new File();
            //    _VolumeInPack = new File();
            //    _RefineMachineName = new File();
            //    _MachineCode = new File();
            //    _MachinePower = new File();
            //    _NumberOfHoursPerYear = new File();
            //    _CodeRAOIn = new File();
            //    _StatusRAOIn = new File();
            //    _VolumeIn = new File();
            //    _MassIn = new File();
            //    _QuantityIn = new File();
            //    _CodeRAOout = new File();
            //    _StatusRAOout = new File();
            //    _VolumeOut = new File();
            //    _MassOut = new File();
            //    _TritiumActivityIn = new File();
            //    _TritiumActivityOut = new File();
            //    _QuantityOZIIIout = new File();
            //    _TransuraniumActivityIn = new File();
            //    _TransuraniumActivityOut = new File();
            //    _BetaGammaActivityIn = new File();
            //    _BetaGammaActivityOut = new File();
            //    _AlphaActivityIn = new File();
            //    _AlphaActivityOut = new File();
            //    _RegNo = new File();
            //    _OrganUprav = new File();
            //    _SubjectRF = new File();
            //    _JurLico = new File();
            //    _ShortJurLico = new File();
            //    _JurLicoAddress = new File();
            //    _JurLicoFactAddress = new File();
            //    _GradeFIO = new File();
            //    _Telephone = new File();
            //    _Fax = new File();
            //    _Email = new File();
            //    _Okpo = new File();
            //    _Okved = new File();
            //    _Okogu = new File();
            //    _Oktmo = new File();
            //    _Inn = new File();
            //    _Kpp = new File();
            //    _Okopf = new File();
            //    _Okfs = new File();
            //    _Volume20 = new File();
            //    _Volume6 = new File();
            //    _SaltConcentration = new File();
            //    _SpecificActivity = new File();
            //    _Mass21 = new File();
            //    _Mass7 = new File();
            //    _IndividualNumberZHRO = new File();
            //    _IndividualNumberZHROrecoded = new File();
            //    _VolumeOutOfPack = new File();
            //    _PackFactoryNumber = new File();
            //    _MassOutOfPack = new File();
            //    _FormingDate = new File();
            //    _MainRadionuclids = new File();
            //    _CodeRAO = new File();
            //    _AlphaActivity = new File();
            //    _BetaGammaActivity = new File();
            //    _TritiumActivity = new File();
            //    _TransuraniumActivity = new File();
            //    _StoragePlaceCode = new File();
            //    _StoragePlaceName = new File();
            //    _Subsidy = new File();
            //    _StoragePlaceNameNote = new File();
            //    _StatusRAO = new File();
            //    _RefineOrSortRAOCode = new File();
            //    _FcpNumber = new File();
            //    _Radionuclids = new File();
            //    _Volume = new File();
            //    _Mass = new File();
            //    _ActivityMeasurementDate = new File();
            //    _Sort = new File();
            //    _Name = new File();
            //    _AggregateState = new File();
            //    _PassportNumber = new File();
            //    _PassportNumberRecoded = new File();
            //    _PassportNumberNote = new File();
            //    _Type = new File();
            //    _TypeRecoded = new File();
            //    _Radionuclids = new File();
            //    _FactoryNumber = new File();
            //    _FactoryNumberRecoded = new File();
            //    _Quantity = new File();
            //    _Activity = new File();
            //    _ActivityNote = new File();
            //    _CreationDate = new File();
            //    _CreatorOKPO = new File();
            //    _CreatorOKPONote = new File();
            //    _Category = new File();
            //    _SignedServicePeriod = new File();
            //    _PropertyCode = new File();
            //    _Owner = new File();
            //    _ProviderOrRecieverOKPO = new File();
            //    _ProviderOrRecieverOKPONote = new File();
            //    _TransporterOKPO = new File();
            //    _TransporterOKPONote = new File();
            //    _PackName = new File();
            //    _PackNameNote = new File();
            //    _PackType = new File();
            //    _PackTypeRecoded = new File();
            //    _PackTypeNote = new File();
            //    _PackNumber = new File();
            //    _PackNumberRecoded = new File();
            //}
        }

        [Attributes.FormVisual("Форма")]
        public override void Object_Validation()
        {

        }

        //SourcesQuantity property
        [Attributes.FormVisual("Количество источников, шт.")]
        public int SourcesQuantity
        {
            get
            {
                if (GetErrors(nameof(SourcesQuantity)) != null)
                {
                    return (int)_SourcesQuantity.Get();
                }
                else
                {
                    return _SourcesQuantity_Not_Valid;
                }
            }
            set
            {
                _SourcesQuantity_Not_Valid = value;
                if (GetErrors(nameof(SourcesQuantity)) != null)
                {
                    _SourcesQuantity.Set(_SourcesQuantity_Not_Valid);
                }
                OnPropertyChanged(nameof(SourcesQuantity));
            }
        }
        private IDataLoadEngine _SourcesQuantity;  // positive int.
        private int _SourcesQuantity_Not_Valid = -1;
        private void SourcesQuantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(SourcesQuantity));
            if (value <= 0)
                AddError(nameof(SourcesQuantity), "Недопустимое значение");
        }
        //SourcesQuantity property

        //ObservedSourceNumber property
        [Attributes.FormVisual("Номер наблюдательной скважины")]
        public string ObservedSourceNumber
        {
            get
            {
                if (GetErrors(nameof(ObservedSourceNumber)) != null)
                {
                    return (string)_ObservedSourceNumber.Get();
                }
                else
                {
                    return _ObservedSourceNumber_Not_Valid;
                }
            }
            set
            {
                _ObservedSourceNumber_Not_Valid = value;
                if (GetErrors(nameof(ObservedSourceNumber)) != null)
                {
                    _ObservedSourceNumber.Set(_ObservedSourceNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(ObservedSourceNumber));
            }
        }
        private IDataLoadEngine _ObservedSourceNumber;//If change this change validation
        private string _ObservedSourceNumber_Not_Valid = "";
        private void ObservedSourceNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(ObservedSourceNumber));
        }
        //ObservedSourceNumber property

        //ControlledAreaName property
        [Attributes.FormVisual("Наименование зоны контроля")]
        public string ControlledAreaName
        {
            get
            {
                if (GetErrors(nameof(ControlledAreaName)) != null)
                {
                    return (string)_ControlledAreaName.Get();
                }
                else
                {
                    return _ControlledAreaName_Not_Valid;
                }
            }
            set
            {
                _ControlledAreaName_Not_Valid = value;
                if (GetErrors(nameof(ControlledAreaName)) != null)
                {
                    _ControlledAreaName.Set(_ControlledAreaName_Not_Valid);
                }
                OnPropertyChanged(nameof(ControlledAreaName));
            }
        }
        private IDataLoadEngine _ControlledAreaName;//If change this change validation
        private string _ControlledAreaName_Not_Valid = "";
        private void ControlledAreaName_Validation(string value)//Ready
        {
            ClearErrors(nameof(ControlledAreaName));
        }
        //ControlledAreaName property
        
        //SupposedWasteSource property
        [Attributes.FormVisual("Предполагаемый источник поступления радиоактивных веществ")]
        public string SupposedWasteSource
        {
            get
            {
                if (GetErrors(nameof(SupposedWasteSource)) != null)
                {
                    return (string)_SupposedWasteSource.Get();
                }
                else
                {
                    return _SupposedWasteSource_Not_Valid;
                }
            }
            set
            {
                _SupposedWasteSource_Not_Valid = value;
                if (GetErrors(nameof(SupposedWasteSource)) != null)
                {
                    _SupposedWasteSource.Set(_SupposedWasteSource_Not_Valid);
                }
                OnPropertyChanged(nameof(SupposedWasteSource));
            }
        }
        private IDataLoadEngine _SupposedWasteSource;
        private string _SupposedWasteSource_Not_Valid = "";
        private void SupposedWasteSource_Validation(string value)//Ready
        {
            ClearErrors(nameof(SupposedWasteSource));
        }
        //SupposedWasteSource property

        //DistanceToWasteSource property
        [Attributes.FormVisual("Расстояние от источника поступления радиоактивных веществ до наблюдательной скважины, м")]
        public int DistanceToWasteSource
        {
            get
            {
                if (GetErrors(nameof(DistanceToWasteSource)) != null)
                {
                    return (int)_DistanceToWasteSource.Get();
                }
                else
                {
                    return _DistanceToWasteSource_Not_Valid;
                }
            }
            set
            {
                _DistanceToWasteSource_Not_Valid = value;
                if (GetErrors(nameof(DistanceToWasteSource)) != null)
                {
                    _DistanceToWasteSource.Set(_DistanceToWasteSource_Not_Valid);
                }
                OnPropertyChanged(nameof(DistanceToWasteSource));
            }
        }
        private IDataLoadEngine _DistanceToWasteSource;
        private int _DistanceToWasteSource_Not_Valid = -1;
        private void DistanceToWasteSource_Validation(int value)//Ready
        {
            ClearErrors(nameof(DistanceToWasteSource));
        }
        //DistanceToWasteSource property

        //TestDepth property
        [Attributes.FormVisual("Глубина отбора проб, м")]
        public int TestDepth
        {
            get
            {
                if (GetErrors(nameof(TestDepth)) != null)
                {
                    return (int)_TestDepth.Get();
                }
                else
                {
                    return _TestDepth_Not_Valid;
                }
            }
            set
            {
                _TestDepth_Not_Valid = value;
                if (GetErrors(nameof(TestDepth)) != null)
                {
                    _TestDepth.Set(_TestDepth_Not_Valid);
                }
                OnPropertyChanged(nameof(TestDepth));
            }
        }
        private IDataLoadEngine _TestDepth;
        private int _TestDepth_Not_Valid = -1;
        private void TestDepth_Validation(int value)//Ready
        {
            ClearErrors(nameof(TestDepth));
        }
        //TestDepth property

        private int _testDepthNote = -1;
        public int TestDepthNote
        {
            get { return _testDepthNote; }
            set
            {
                _testDepthNote = value;
                OnPropertyChanged("TestDepthNote");
            }
        }

        //RadionuclidName property
        [Attributes.FormVisual("Радионуклид")]
        public string RadionuclidName
        {
            get
            {
                if (GetErrors(nameof(RadionuclidName)) != null)
                {
                    return (string)_RadionuclidName.Get();
                }
                else
                {
                    return _RadionuclidName_Not_Valid;
                }
            }
            set
            {
                _RadionuclidName_Not_Valid = value;
                if (GetErrors(nameof(RadionuclidName)) != null)
                {
                    _RadionuclidName.Set(_RadionuclidName_Not_Valid);
                }
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }
        private IDataLoadEngine _RadionuclidName;//If change this change validation
        private string _RadionuclidName_Not_Valid = "";
        private void RadionuclidName_Validation()//TODO
        {
            ClearErrors(nameof(RadionuclidName));
        }
        //RadionuclidName property

        //AverageYearConcentration property
        [Attributes.FormVisual("Среднегодовое содержание радионуклида, Бк/кг")]
        public double AverageYearConcentration
        {
            get
            {
                if (GetErrors(nameof(AverageYearConcentration)) != null)
                {
                    return (double)_AverageYearConcentration.Get();
                }
                else
                {
                    return _AverageYearConcentration_Not_Valid;
                }
            }
            set
            {
                _AverageYearConcentration_Not_Valid = value;
                if (GetErrors(nameof(AverageYearConcentration)) != null)
                {
                    _AverageYearConcentration.Set(_AverageYearConcentration_Not_Valid);
                }
                OnPropertyChanged(nameof(AverageYearConcentration));
            }
        }
        private IDataLoadEngine _AverageYearConcentration;
        private double _AverageYearConcentration_Not_Valid = -1;
        private void AverageYearConcentration_Validation()//TODO
        {
            ClearErrors(nameof(AverageYearConcentration));
        }
        //AverageYearConcentration property
    }
}
