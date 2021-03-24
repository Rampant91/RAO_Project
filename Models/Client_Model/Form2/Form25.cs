using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 2.5: Наличие РВ, содержащихся в отработавшем ядерном топливе, в пунктах хранения")]
    public class Form25: Form2
    {
        public override string FormNum { get { return "2.5"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 12;

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

        //CodeOYAT property
        [Attributes.FormVisual("Код ОЯТ")]
        public string CodeOYAT
        {
            get
            {
                if (GetErrors(nameof(CodeOYAT)) != null)
                {
                    return (string)_CodeOYAT.Get();
                }
                else
                {
                    return _CodeOYAT_Not_Valid;
                }
            }
            set
            {
                _CodeOYAT_Not_Valid = value;
                if (GetErrors(nameof(CodeOYAT)) != null)
                {
                    _CodeOYAT.Set(_CodeOYAT_Not_Valid);
                }
                OnPropertyChanged(nameof(CodeOYAT));
            }
        }
        private IDataLoadEngine _CodeOYAT;
        private string _CodeOYAT_Not_Valid = "";
        private void CodeOYAT_Validation()
        {
            ClearErrors(nameof(CodeOYAT));
        }
        //CodeOYAT property

        private string _codeOYATnote = "";
        public string CodeOYATnote
        {
            get { return _codeOYATnote; }
            set
            {
                _codeOYATnote = value;
                OnPropertyChanged("CodeOYATnote");
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

        //FuelMass property
        [Attributes.FormVisual("Масса топлива, т")]
        public double FuelMass
        {
            get
            {
                if (GetErrors(nameof(FuelMass)) != null)
                {
                    return (double)_FuelMass.Get();
                }
                else
                {
                    return _FuelMass_Not_Valid;
                }
            }
            set
            {
                _FuelMass_Not_Valid = value;
                if (GetErrors(nameof(FuelMass)) != null)
                {
                    _FuelMass.Set(_FuelMass_Not_Valid);
                }
                OnPropertyChanged(nameof(FuelMass));
            }
        }
        private IDataLoadEngine _FuelMass;
        private double _FuelMass_Not_Valid = -1;
        private void FuelMass_Validation(double value)//TODO
        {
            ClearErrors(nameof(FuelMass));
        }
        //FuelMass property

        //CellMass property
        [Attributes.FormVisual("Масса ОТВС(ТВЭЛ, выемной части реактора), т")]
        public double CellMass
        {
            get
            {
                if (GetErrors(nameof(CellMass)) != null)
                {
                    return (double)_CellMass.Get();
                }
                else
                {
                    return _CellMass_Not_Valid;
                }
            }
            set
            {
                _CellMass_Not_Valid = value;
                if (GetErrors(nameof(CellMass)) != null)
                {
                    _CellMass.Set(_CellMass_Not_Valid);
                }
                OnPropertyChanged(nameof(CellMass));
            }
        }
        private IDataLoadEngine _CellMass;
        private double _CellMass_Not_Valid = -1;
        private void CellMass_Validation(double value)//TODO
        {
            ClearErrors(nameof(CellMass));
        }
        //CellMass property

        //Quantity property
        [Attributes.FormVisual("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) != null)
                {
                    return (int)_Quantity.Get();
                }
                else
                {
                    return _Quantity_Not_Valid;
                }
            }
            set
            {
                _Quantity_Not_Valid = value;
                if (GetErrors(nameof(Quantity)) != null)
                {
                    _Quantity.Set(_Quantity_Not_Valid);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private IDataLoadEngine _Quantity;  // positive int.
        private int _Quantity_Not_Valid = -1;
        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }
        //Quantity property

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
    }
}
