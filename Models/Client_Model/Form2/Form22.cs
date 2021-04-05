using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.2: Наличие РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form22 : Form2
    {
        public Form22() : base()
        {
        }

        [Attributes.FormVisual("Форма")]
        public override string FormNum { get { return "22"; } }
        public override int NumberOfFields { get; } = 25;
        public override void Object_Validation()
        {

        }

        //StoragePlaceName property
        [Attributes.FormVisual("Наименование ПХ")]
        public string StoragePlaceName
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceName)) != null)
                {
                    return (string)_StoragePlaceName.Get();
                }
                else
                {
                    return _StoragePlaceName_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceName_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceName)) != null)
                {
                    _StoragePlaceName.Set(_StoragePlaceName_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        private IDataLoadEngine _StoragePlaceName;//If change this change validation
        private string _StoragePlaceName_Not_Valid = "";
        private void StoragePlaceName_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceName));
        }
        //StoragePlaceName property

        private string _storagePlaceNameNote = "";
        public string StoragePlaceNameNote
        {
            get { return _storagePlaceNameNote; }
            set
            {
                _storagePlaceNameNote = value;
                OnPropertyChanged("StoragePlaceNameNote");
            }
        }

        //StoragePlaceCode property
        [Attributes.FormVisual("Код ПХ")]
        public string StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceCode)) != null)
                {
                    return (string)_StoragePlaceCode.Get();
                }
                else
                {
                    return _StoragePlaceCode_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceCode_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceCode)) != null)
                {
                    _StoragePlaceCode.Set(_StoragePlaceCode_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        private IDataLoadEngine _StoragePlaceCode;//if change this change validation
        private string _StoragePlaceCode_Not_Valid = "";
        private void StoragePlaceCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(StoragePlaceCode));
            if (!(value == "-"))
                if (value.Length != 8)
                    AddError(nameof(StoragePlaceCode), "Недопустимое значение");
                else
                    for (int i = 0; i < 8; i++)
                    {
                        if (!((value[i] >= '0') && (value[i] <= '9')))
                        {
                            AddError(nameof(StoragePlaceCode), "Недопустимое значение");
                            return;
                        }
                    }
        }
        //StoragePlaceCode property

        //PackName property
        [Attributes.FormVisual("Наименование упаковки")]
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) != null)
                {
                    return (string)_PackName.Get();
                }
                else
                {
                    return _PackName_Not_Valid;
                }
            }
            set
            {
                _PackName_Not_Valid = value;
                if (GetErrors(nameof(PackName)) != null)
                {
                    _PackName.Set(_PackName_Not_Valid);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }
        private IDataLoadEngine _PackName;
        private string _PackName_Not_Valid = "";
        private void PackName_Validation()
        {
            ClearErrors(nameof(PackName));
        }
        //PackName property

        private string _packNameNote = "";
        public string PackNameNote
        {
            get { return _packNameNote; }
            set
            {
                _packNameNote = value;
                OnPropertyChanged("PackNameNote");
            }
        }

        //PackType property
        [Attributes.FormVisual("Тип упаковки")]
        public string PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) != null)
                {
                    return (string)_PackType.Get();
                }
                else
                {
                    return _PackType_Not_Valid;
                }
            }
            set
            {
                _PackType_Not_Valid = value;
                if (GetErrors(nameof(PackType)) != null)
                {
                    _PackType.Set(_PackType_Not_Valid);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        private IDataLoadEngine _PackType;//If change this change validation
        private string _PackType_Not_Valid = "";
        private void PackType_Validation()//Ready
        {
            ClearErrors(nameof(PackType));
        }
        //PackType property

        private string _packTypeRecoded = "";
        public string PackTypeRecoded
        {
            get { return _packTypeRecoded; }
            set
            {
                _packTypeRecoded = value;
                OnPropertyChanged("PackTypeRecoded");
            }
        }

        private string _packTypeNote = "";
        public string PackTypeNote
        {
            get { return _packTypeNote; }
            set
            {
                _packTypeNote = value;
                OnPropertyChanged("PackTypeNote");
            }
        }

        //PackQuantity property
        [Attributes.FormVisual("Количество упаковок, шт.")]
        public int PackQuantity
        {
            get
            {
                if (GetErrors(nameof(PackQuantity)) != null)
                {
                    return (int)_PackQuantity.Get();
                }
                else
                {
                    return _PackQuantity_Not_Valid;
                }
            }
            set
            {
                _PackQuantity_Not_Valid = value;
                if (GetErrors(nameof(PackQuantity)) != null)
                {
                    _PackQuantity.Set(_PackQuantity_Not_Valid);
                }
                OnPropertyChanged(nameof(PackQuantity));
            }
        }
        private IDataLoadEngine _PackQuantity;  // positive int.
        private int _PackQuantity_Not_Valid = -1;
        private void PackQuantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(PackQuantity));
            if (value <= 0)
                AddError(nameof(PackQuantity), "Недопустимое значение");
        }
        //PackQuantity property

        //CodeRAO property
        [Attributes.FormVisual("Код РАО")]
        public string CodeRAO
        {
            get
            {
                if (GetErrors(nameof(CodeRAO)) != null)
                {
                    return (string)_CodeRAO.Get();
                }
                else
                {
                    return _CodeRAO_Not_Valid;
                }
            }
            set
            {
                _CodeRAO_Not_Valid = value;
                if (GetErrors(nameof(CodeRAO)) != null)
                {
                    _CodeRAO.Set(_CodeRAO_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }
        private IDataLoadEngine _CodeRAO;
        private string _CodeRAO_Not_Valid = "";
        private void CodeRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAO));
        }
        //CodeRAO property

        //StatusRAO property
        [Attributes.FormVisual("Статус РАО")]
        public string StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAO)) != null)
                {
                    return (string)_StatusRAO.Get();
                }
                else
                {
                    return _StatusRAO_Not_Valid;
                }
            }
            set
            {
                _StatusRAO_Not_Valid = value;
                if (GetErrors(nameof(StatusRAO)) != null)
                {
                    _StatusRAO.Set(_StatusRAO_Not_Valid);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }
        private IDataLoadEngine _StatusRAO;
        private string _StatusRAO_Not_Valid = "";
        private void StatusRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAO));
        }
        //StatusRAO property

        //VolumeInPack property
        [Attributes.FormVisual("Объем с упаковкой, куб. м")]
        public double VolumeInPack
        {
            get
            {
                if (GetErrors(nameof(VolumeInPack)) != null)
                {
                    return (double)_VolumeInPack.Get();
                }
                else
                {
                    return _VolumeInPack_Not_Valid;
                }
            }
            set
            {
                _VolumeInPack_Not_Valid = value;
                if (GetErrors(nameof(VolumeInPack)) != null)
                {
                    _VolumeInPack.Set(_VolumeInPack_Not_Valid);
                }
                OnPropertyChanged(nameof(VolumeInPack));
            }
        }
        private IDataLoadEngine _VolumeInPack;
        private double _VolumeInPack_Not_Valid = -1;
        private void VolumeInPack_Validation(double value)//TODO
        {
            ClearErrors(nameof(VolumeInPack));
        }
        //VolumeInPack property

        //MassInPack Property
        [Attributes.FormVisual("Масса с упаковкой, т")]
        public double MassInPack
        {
            get
            {
                if (GetErrors(nameof(MassInPack)) != null)
                {
                    return (double)_MassInPack.Get();
                }
                else
                {
                    return _MassInPack_Not_Valid;
                }
            }
            set
            {
                _MassInPack_Not_Valid = value;
                if (GetErrors(nameof(MassInPack)) != null)
                {
                    _MassInPack.Set(_MassInPack_Not_Valid);
                }
                OnPropertyChanged(nameof(MassInPack));
            }
        }
        private IDataLoadEngine _MassInPack;
        private double _MassInPack_Not_Valid = -1;
        private void MassInPack_Validation()//TODO
        {
            ClearErrors(nameof(MassInPack));
        }
        //MassInPack Property

        //VolumeOutOfPack property
        [Attributes.FormVisual("Объем без упаковки, куб. м")]
        public double VolumeOutOfPack
        {
            get
            {
                if (GetErrors(nameof(VolumeOutOfPack)) != null)
                {
                    return (double)_VolumeOutOfPack.Get();
                }
                else
                {
                    return _VolumeOutOfPack_Not_Valid;
                }
            }
            set
            {
                _VolumeOutOfPack_Not_Valid = value;
                if (GetErrors(nameof(VolumeOutOfPack)) != null)
                {
                    _VolumeOutOfPack.Set(_VolumeOutOfPack_Not_Valid);
                }
                OnPropertyChanged(nameof(VolumeOutOfPack));
            }
        }
        private IDataLoadEngine _VolumeOutOfPack;
        private double _VolumeOutOfPack_Not_Valid = -1;
        private void VolumeOutOfPack_Validation(double value)//TODO
        {
            ClearErrors(nameof(VolumeOutOfPack));
        }
        //VolumeOutOfPack property

        //MassOutOfPack Property
        [Attributes.FormVisual("Масса без упаковки, т")]
        public double MassOutOfPack
        {
            get
            {
                if (GetErrors(nameof(MassOutOfPack)) != null)
                {
                    return (double)_MassOutOfPack.Get();
                }
                else
                {
                    return _MassOutOfPack_Not_Valid;
                }
            }
            set
            {
                _MassOutOfPack_Not_Valid = value;
                if (GetErrors(nameof(MassOutOfPack)) != null)
                {
                    _MassOutOfPack.Set(_MassOutOfPack_Not_Valid);
                }
                OnPropertyChanged(nameof(MassOutOfPack));
            }
        }
        private IDataLoadEngine _MassOutOfPack;
        private double _MassOutOfPack_Not_Valid = -1;
        private void MasOutOfPack_Validation()//TODO
        {
            ClearErrors(nameof(MassOutOfPack));
        }
        //MassOutOfPack Property

        //QuantityOZIII property
        [Attributes.FormVisual("Количество ОЗИИИ, шт.")]
        public int QuantityOZIII
        {
            get
            {
                if (GetErrors(nameof(QuantityOZIII)) != null)
                {
                    return (int)_QuantityOZIII.Get();
                }
                else
                {
                    return _QuantityOZIII_Not_Valid;
                }
            }
            set
            {
                _QuantityOZIII_Not_Valid = value;
                if (GetErrors(nameof(QuantityOZIII)) != null)
                {
                    _QuantityOZIII.Set(_QuantityOZIII_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        private IDataLoadEngine _QuantityOZIII;  // positive int.
        private int _QuantityOZIII_Not_Valid = -1;
        private void QuantityOZIII_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOZIII));
            if (value <= 0)
                AddError(nameof(QuantityOZIII), "Недопустимое значение");
        }
        //QuantityOZIII property

        //TritiumActivity property
        [Attributes.FormVisual("Активность трития, Бк")]
        public string TritiumActivity
        {
            get
            {
                if (GetErrors(nameof(TritiumActivity)) != null)
                {
                    return (string)_TritiumActivity.Get();
                }
                else
                {
                    return _TritiumActivity_Not_Valid;
                }
            }
            set
            {
                _TritiumActivity_Not_Valid = value;
                if (GetErrors(nameof(TritiumActivity)) != null)
                {
                    _TritiumActivity.Set(_TritiumActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }
        private IDataLoadEngine _TritiumActivity;
        private string _TritiumActivity_Not_Valid = "";
        private void TritiumActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(TritiumActivity));
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(TritiumActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(TritiumActivity), "Недопустимое значение");
            }
        }
        //TritiumActivity property

        //BetaGammaActivity property
        [Attributes.FormVisual("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivity
        {
            get
            {
                if (GetErrors(nameof(BetaGammaActivity)) != null)
                {
                    return (string)_BetaGammaActivity.Get();
                }
                else
                {
                    return _BetaGammaActivity_Not_Valid;
                }
            }
            set
            {
                _BetaGammaActivity_Not_Valid = value;
                if (GetErrors(nameof(BetaGammaActivity)) != null)
                {
                    _BetaGammaActivity.Set(_BetaGammaActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }
        private IDataLoadEngine _BetaGammaActivity;
        private string _BetaGammaActivity_Not_Valid = "";
        private void BetaGammaActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(BetaGammaActivity));
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(BetaGammaActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(BetaGammaActivity), "Недопустимое значение");
            }
        }
        //BetaGammaActivity property

        //AlphaActivity property
        [Attributes.FormVisual("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivity
        {
            get
            {
                if (GetErrors(nameof(AlphaActivity)) != null)
                {
                    return (string)_AlphaActivity.Get();
                }
                else
                {
                    return _AlphaActivity_Not_Valid;
                }
            }
            set
            {
                _AlphaActivity_Not_Valid = value;
                if (GetErrors(nameof(AlphaActivity)) != null)
                {
                    _AlphaActivity.Set(_AlphaActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }
        private IDataLoadEngine _AlphaActivity;
        private string _AlphaActivity_Not_Valid = "";
        private void AlphaActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(AlphaActivity));
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(AlphaActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(AlphaActivity), "Недопустимое значение");
            }
        }
        //AlphaActivity property

        //TransuraniumActivity property
        [Attributes.FormVisual("Активность трансурановых, Бк")]
        public string TransuraniumActivity
        {
            get
            {
                if (GetErrors(nameof(TransuraniumActivity)) != null)
                {
                    return (string)_TransuraniumActivity.Get();
                }
                else
                {
                    return _TransuraniumActivity_Not_Valid;
                }
            }
            set
            {
                _TransuraniumActivity_Not_Valid = value;
                if (GetErrors(nameof(TransuraniumActivity)) != null)
                {
                    _TransuraniumActivity.Set(_TransuraniumActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }
        private IDataLoadEngine _TransuraniumActivity;
        private string _TransuraniumActivity_Not_Valid = "";
        private void TransuraniumActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransuraniumActivity));
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(TransuraniumActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(TransuraniumActivity), "Недопустимое значение");
            }
        }
        //TransuraniumActivity property

        //MainRadionuclids property
        [Attributes.FormVisual("Радионуклиды")]
        public string MainRadionuclids
        {
            get
            {
                if (GetErrors(nameof(MainRadionuclids)) != null)
                {
                    return (string)_MainRadionuclids.Get();
                }
                else
                {
                    return _MainRadionuclids_Not_Valid;
                }
            }
            set
            {
                _MainRadionuclids_Not_Valid = value;
                if (GetErrors(nameof(MainRadionuclids)) != null)
                {
                    _MainRadionuclids.Set(_MainRadionuclids_Not_Valid);
                }
                OnPropertyChanged(nameof(MainRadionuclids));
            }
        }
        private IDataLoadEngine _MainRadionuclids;//If change this change validation
        private string _MainRadionuclids_Not_Valid = "";
        private void MainRadionuclids_Validation()//TODO
        {
            ClearErrors(nameof(MainRadionuclids));
        }
        //MainRadionuclids property

        //Subsidy property
        [Attributes.FormVisual("Субсидия, %")]
        public string Subsidy // 0<number<=100 or empty.
        {
            get
            {
                if (GetErrors(nameof(Subsidy)) != null)
                {
                    return (string)_Subsidy.Get();
                }
                else
                {
                    return _Subsidy_Not_Valid;
                }
            }
            set
            {
                _Subsidy_Not_Valid = value;
                if (GetErrors(nameof(Subsidy)) != null)
                {
                    _Subsidy.Set(_Subsidy_Not_Valid);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }
        private IDataLoadEngine _Subsidy;
        private string _Subsidy_Not_Valid = "";
        private void Subsidy_Validation(string value)//Ready
        {
            ClearErrors(nameof(Subsidy));
            try
            {
                int tmp = Int32.Parse(value);
                if (!((tmp > 0) && (tmp <= 100)))
                    AddError(nameof(Subsidy), "Недопустимое значение");
            }
            catch
            {
                AddError(nameof(Subsidy), "Недопустимое значение");
            }
        }
        //Subsidy property

        //FcpNumber property
        [Attributes.FormVisual("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get
            {
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    return (string)_FcpNumber.Get();
                }
                else
                {
                    return _FcpNumber_Not_Valid;
                }
            }
            set
            {
                _FcpNumber_Not_Valid = value;
                if (GetErrors(nameof(FcpNumber)) != null)
                {
                    _FcpNumber.Set(_FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }
        private IDataLoadEngine _FcpNumber;
        private string _FcpNumber_Not_Valid = "";
        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property
    }
}
