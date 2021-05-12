using Collections.Rows_Collection;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.2: Наличие РАО в пунктах хранения, местах сбора и/или временного хранения")]
    public class Form22 : Abstracts.Form2
    {
        public Form22(IDataAccess Access) : base(Access)
        {
            FormNum = "22";
            NumberOfFields = 25;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]
        public string StoragePlaceName
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceName))[0][0];
                }
                else
                {
                    return _StoragePlaceName_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceName_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceName), _StoragePlaceName_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
        private string _StoragePlaceName_Not_Valid = "";
        private void StoragePlaceName_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceName));
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public string StoragePlaceNameNote
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceNameNote))[0][0];
                }
                else
                {
                    return _StoragePlaceNameNote_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceNameNote_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), _StoragePlaceNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation
        private string _StoragePlaceNameNote_Not_Valid = "";
        private void StoragePlaceNameNote_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceNameNote));
        }
        //StoragePlaceNameNote property

        //StoragePlaceCode property
        [Attributes.Form_Property("Код ПХ")]
        public string StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceCode)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceCode))[0][0];
                }
                else
                {
                    return _StoragePlaceCode_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceCode_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceCode)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), _StoragePlaceCode_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation
        private string _StoragePlaceCode_Not_Valid = "";
        private void StoragePlaceCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(StoragePlaceCode));
            var lst = new List<string>();//HERE binds spr
            foreach (var item in lst)
            {
                if (item.Equals(value)) return;
            }
            AddError(nameof(StoragePlaceCode), "Такого значения нет в справочнике");
        }
        //StoragePlaceCode property

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackName))[0][0];
                }
                else
                {
                    return _PackName_Not_Valid;
                }
            }
            set
            {
                _PackName_Not_Valid = value;
                if (GetErrors(nameof(PackName)) == null)
                {
                    _dataAccess.Set(nameof(PackName), _PackName_Not_Valid);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }

        private string _PackName_Not_Valid = "";
        private void PackName_Validation()
        {
            ClearErrors(nameof(PackName));
        }
        //PackName property

        //PackNameNote property
        public string PackNameNote
        {
            get
            {
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackNameNote))[0][0];
                }
                else
                {
                    return _PackNameNote_Not_Valid;
                }
            }
            set
            {
                _PackNameNote_Not_Valid = value;
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    _dataAccess.Set(nameof(PackNameNote), _PackNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }

        private string _PackNameNote_Not_Valid = "";
        private void PackNameNote_Validation()
        {
            ClearErrors(nameof(PackNameNote));
        }
        //PackNameNote property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public string PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackType))[0][0];
                }
                else
                {
                    return _PackType_Not_Valid;
                }
            }
            set
            {
                _PackType_Not_Valid = value;
                if (GetErrors(nameof(PackType)) == null)
                {
                    _dataAccess.Set(nameof(PackType), _PackType_Not_Valid);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
        private string _PackType_Not_Valid = "";
        private void PackType_Validation()//Ready
        {
            ClearErrors(nameof(PackType));
        }
        //PackType property

        //PackTypeRecoded property
        public string PackTypeRecoded
        {
            get
            {
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackTypeRecoded))[0][0];
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                _PackTypeRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), _PackTypeRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        private string _PackTypeRecoded_Not_Valid = "";
        private void PackTypeRecoded_Validation()
        {
            ClearErrors(nameof(PackTypeRecoded));
        }
        //PackTypeRecoded property

        //PackTypeNote property
        public string PackTypeNote
        {
            get
            {
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackTypeNote))[0][0];
                }
                else
                {
                    return _PackTypeNote_Not_Valid;
                }
            }
            set
            {
                _PackTypeNote_Not_Valid = value;
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeNote), _PackTypeNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }

        private string _PackTypeNote_Not_Valid = "";
        private void PackTypeNote_Validation()
        {
            ClearErrors(nameof(PackTypeNote));
        }
        //PackTypeNote property

        //PackQuantity property
        [Attributes.Form_Property("Количество упаковок, шт.")]
        public int PackQuantity
        {
            get
            {
                if (GetErrors(nameof(PackQuantity)) == null)
                {
                    return (int)_dataAccess.Get(nameof(PackQuantity))[0][0];
                }
                else
                {
                    return _PackQuantity_Not_Valid;
                }
            }
            set
            {
                _PackQuantity_Not_Valid = value;
                if (GetErrors(nameof(PackQuantity)) == null)
                {
                    _dataAccess.Set(nameof(PackQuantity), _PackQuantity_Not_Valid);
                }
                OnPropertyChanged(nameof(PackQuantity));
            }
        }
        // positive int.
        private int _PackQuantity_Not_Valid = -1;
        private void PackQuantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(PackQuantity));
            if (value <= 0)
                AddError(nameof(PackQuantity), "Недопустимое значение");
        }
        //PackQuantity property

        //CodeRAO property
        [Attributes.Form_Property("Код РАО")]
        public string CodeRAO
        {
            get
            {
                if (GetErrors(nameof(CodeRAO)) == null)
                {
                    return (string)_dataAccess.Get(nameof(CodeRAO))[0][0];
                }
                else
                {
                    return _CodeRAO_Not_Valid;
                }
            }
            set
            {
                _CodeRAO_Not_Valid = value;
                if (GetErrors(nameof(CodeRAO)) == null)
                {
                    _dataAccess.Set(nameof(CodeRAO), _CodeRAO_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }

        private string _CodeRAO_Not_Valid = "";
        private void CodeRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CodeRAO));
        }
        //CodeRAO property

        //StatusRAO property
        [Attributes.Form_Property("Статус РАО")]
        public string StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAO)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StatusRAO))[0][0];
                }
                else
                {
                    return _StatusRAO_Not_Valid;
                }
            }
            set
            {
                _StatusRAO_Not_Valid = value;
                if (GetErrors(nameof(StatusRAO)) == null)
                {
                    _dataAccess.Set(nameof(StatusRAO), _StatusRAO_Not_Valid);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }

        private string _StatusRAO_Not_Valid = "";
        private void StatusRAO_Validation(string value)//TODO
        {
            ClearErrors(nameof(StatusRAO));
        }
        //StatusRAO property

        //VolumeInPack property
        [Attributes.Form_Property("Объем с упаковкой, куб. м")]
        public double VolumeInPack
        {
            get
            {
                if (GetErrors(nameof(VolumeInPack)) == null)
                {
                    return (double)_dataAccess.Get(nameof(VolumeInPack))[0][0];
                }
                else
                {
                    return _VolumeInPack_Not_Valid;
                }
            }
            set
            {
                _VolumeInPack_Not_Valid = value;
                if (GetErrors(nameof(VolumeInPack)) == null)
                {
                    _dataAccess.Set(nameof(VolumeInPack), _VolumeInPack_Not_Valid);
                }
                OnPropertyChanged(nameof(VolumeInPack));
            }
        }

        private double _VolumeInPack_Not_Valid = -1;
        private void VolumeInPack_Validation(double value)//TODO
        {
            ClearErrors(nameof(VolumeInPack));
        }
        //VolumeInPack property

        //MassInPack Property
        [Attributes.Form_Property("Масса с упаковкой, т")]
        public double MassInPack
        {
            get
            {
                if (GetErrors(nameof(MassInPack)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassInPack))[0][0];
                }
                else
                {
                    return _MassInPack_Not_Valid;
                }
            }
            set
            {
                _MassInPack_Not_Valid = value;
                if (GetErrors(nameof(MassInPack)) == null)
                {
                    _dataAccess.Set(nameof(MassInPack), _MassInPack_Not_Valid);
                }
                OnPropertyChanged(nameof(MassInPack));
            }
        }

        private double _MassInPack_Not_Valid = -1;
        private void MassInPack_Validation()//TODO
        {
            ClearErrors(nameof(MassInPack));
        }
        //MassInPack Property

        //VolumeOutOfPack property
        [Attributes.Form_Property("Объем без упаковки, куб. м")]
        public double VolumeOutOfPack
        {
            get
            {
                if (GetErrors(nameof(VolumeOutOfPack)) == null)
                {
                    return (double)_dataAccess.Get(nameof(VolumeOutOfPack))[0][0];
                }
                else
                {
                    return _VolumeOutOfPack_Not_Valid;
                }
            }
            set
            {
                _VolumeOutOfPack_Not_Valid = value;
                if (GetErrors(nameof(VolumeOutOfPack)) == null)
                {
                    _dataAccess.Set(nameof(VolumeOutOfPack), _VolumeOutOfPack_Not_Valid);
                }
                OnPropertyChanged(nameof(VolumeOutOfPack));
            }
        }

        private double _VolumeOutOfPack_Not_Valid = -1;
        private void VolumeOutOfPack_Validation(double value)//TODO
        {
            ClearErrors(nameof(VolumeOutOfPack));
        }
        //VolumeOutOfPack property

        //MassOutOfPack Property
        [Attributes.Form_Property("Масса без упаковки, т")]
        public double MassOutOfPack
        {
            get
            {
                if (GetErrors(nameof(MassOutOfPack)) == null)
                {
                    return (double)_dataAccess.Get(nameof(MassOutOfPack))[0][0];
                }
                else
                {
                    return _MassOutOfPack_Not_Valid;
                }
            }
            set
            {
                _MassOutOfPack_Not_Valid = value;
                if (GetErrors(nameof(MassOutOfPack)) == null)
                {
                    _dataAccess.Set(nameof(MassOutOfPack), _MassOutOfPack_Not_Valid);
                }
                OnPropertyChanged(nameof(MassOutOfPack));
            }
        }

        private double _MassOutOfPack_Not_Valid = -1;
        private void MasOutOfPack_Validation()//TODO
        {
            ClearErrors(nameof(MassOutOfPack));
        }
        //MassOutOfPack Property

        //QuantityOZIII property
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public int QuantityOZIII
        {
            get
            {
                if (GetErrors(nameof(QuantityOZIII)) == null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityOZIII))[0][0];
                }
                else
                {
                    return _QuantityOZIII_Not_Valid;
                }
            }
            set
            {
                _QuantityOZIII_Not_Valid = value;
                if (GetErrors(nameof(QuantityOZIII)) == null)
                {
                    _dataAccess.Set(nameof(QuantityOZIII), _QuantityOZIII_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.
        private int _QuantityOZIII_Not_Valid = -1;
        private void QuantityOZIII_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOZIII));
            if (value <= 0)
                AddError(nameof(QuantityOZIII), "Недопустимое значение");
        }
        //QuantityOZIII property

        //TritiumActivity property
        [Attributes.Form_Property("Активность трития, Бк")]
        public string TritiumActivity
        {
            get
            {
                if (GetErrors(nameof(TritiumActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TritiumActivity))[0][0];
                }
                else
                {
                    return _TritiumActivity_Not_Valid;
                }
            }
            set
            {
                _TritiumActivity_Not_Valid = value;
                if (GetErrors(nameof(TritiumActivity)) == null)
                {
                    _dataAccess.Set(nameof(TritiumActivity), _TritiumActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }

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
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public string BetaGammaActivity
        {
            get
            {
                if (GetErrors(nameof(BetaGammaActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(BetaGammaActivity))[0][0];
                }
                else
                {
                    return _BetaGammaActivity_Not_Valid;
                }
            }
            set
            {
                _BetaGammaActivity_Not_Valid = value;
                if (GetErrors(nameof(BetaGammaActivity)) == null)
                {
                    _dataAccess.Set(nameof(BetaGammaActivity), _BetaGammaActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }

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
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public string AlphaActivity
        {
            get
            {
                if (GetErrors(nameof(AlphaActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(AlphaActivity))[0][0];
                }
                else
                {
                    return _AlphaActivity_Not_Valid;
                }
            }
            set
            {
                _AlphaActivity_Not_Valid = value;
                if (GetErrors(nameof(AlphaActivity)) == null)
                {
                    _dataAccess.Set(nameof(AlphaActivity), _AlphaActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }

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
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public string TransuraniumActivity
        {
            get
            {
                if (GetErrors(nameof(TransuraniumActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TransuraniumActivity))[0][0];
                }
                else
                {
                    return _TransuraniumActivity_Not_Valid;
                }
            }
            set
            {
                _TransuraniumActivity_Not_Valid = value;
                if (GetErrors(nameof(TransuraniumActivity)) == null)
                {
                    _dataAccess.Set(nameof(TransuraniumActivity), _TransuraniumActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }

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
        [Attributes.Form_Property("Радионуклиды")]
        public string MainRadionuclids
        {
            get
            {
                if (GetErrors(nameof(MainRadionuclids)) == null)
                {
                    return (string)_dataAccess.Get(nameof(MainRadionuclids))[0][0];
                }
                else
                {
                    return _MainRadionuclids_Not_Valid;
                }
            }
            set
            {
                _MainRadionuclids_Not_Valid = value;
                if (GetErrors(nameof(MainRadionuclids)) == null)
                {
                    _dataAccess.Set(nameof(MainRadionuclids), _MainRadionuclids_Not_Valid);
                }
                OnPropertyChanged(nameof(MainRadionuclids));
            }
        }
        //If change this change validation
        private string _MainRadionuclids_Not_Valid = "";
        private void MainRadionuclids_Validation()//TODO
        {
            ClearErrors(nameof(MainRadionuclids));
        }
        //MainRadionuclids property

        //Subsidy property
        [Attributes.Form_Property("Субсидия, %")]
        public string Subsidy // 0<number<=100 or empty.
        {
            get
            {
                if (GetErrors(nameof(Subsidy)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Subsidy))[0][0];
                }
                else
                {
                    return _Subsidy_Not_Valid;
                }
            }
            set
            {
                _Subsidy_Not_Valid = value;
                if (GetErrors(nameof(Subsidy)) == null)
                {
                    _dataAccess.Set(nameof(Subsidy), _Subsidy_Not_Valid);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }

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
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public string FcpNumber
        {
            get
            {
                if (GetErrors(nameof(FcpNumber)) == null)
                {
                    return (string)_dataAccess.Get(nameof(FcpNumber))[0][0];
                }
                else
                {
                    return _FcpNumber_Not_Valid;
                }
            }
            set
            {
                _FcpNumber_Not_Valid = value;
                if (GetErrors(nameof(FcpNumber)) == null)
                {
                    _dataAccess.Set(nameof(FcpNumber), _FcpNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        private string _FcpNumber_Not_Valid = "";
        private void FcpNuber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property
    }
}
