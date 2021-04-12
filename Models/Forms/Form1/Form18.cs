using System;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.8: Сведения о жидких кондиционированных РАО")]
    public class Form18: Abstracts.Form1
    {
        public Form18(int RowID) : base(RowID)
        {
            FormNum = "18";
            NumberOfFields = 37;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //IndividualNumberZHRO property
        [Attributes.Form_Property("Индивидуальный номер ЖРО")]
        public string IndividualNumberZHRO
        {
            get
            {
                if (GetErrors(nameof(IndividualNumberZHRO)) != null)
                {
                    return (string)_IndividualNumberZHRO.Get();
                }
                else
                {
                    return _IndividualNumberZHRO_Not_Valid;
                }
            }
            set
            {
                _IndividualNumberZHRO_Not_Valid = value;
                if (GetErrors(nameof(IndividualNumberZHRO)) != null)
                {
                    _IndividualNumberZHRO.Set(_IndividualNumberZHRO_Not_Valid);
                }
                OnPropertyChanged(nameof(IndividualNumberZHRO));
            }
        }
        private IDataLoadEngine _IndividualNumberZHRO;
        private string _IndividualNumberZHRO_Not_Valid = "";
        private void IndividualNumberZHRO_Validation(string value)
        {
            ClearErrors(nameof(IndividualNumberZHRO));
        }
        //IndividualNumberZHRO property

        //IndividualNumberZHROrecoded property
        public string IndividualNumberZHROrecoded
        {
            get
            {
                if (GetErrors(nameof(IndividualNumberZHROrecoded)) != null)
                {
                    return (string)_IndividualNumberZHROrecoded.Get();
                }
                else
                {
                    return _IndividualNumberZHROrecoded_Not_Valid;
                }
            }
            set
            {
                _IndividualNumberZHROrecoded_Not_Valid = value;
                if (GetErrors(nameof(IndividualNumberZHROrecoded)) != null)
                {
                    _IndividualNumberZHROrecoded.Set(_IndividualNumberZHROrecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(IndividualNumberZHROrecoded));
            }
        }
        private IDataLoadEngine _IndividualNumberZHROrecoded;
        private string _IndividualNumberZHROrecoded_Not_Valid = "";
        private void IndividualNumberZHROrecoded_Validation(string value)
        {
            ClearErrors(nameof(IndividualNumberZHROrecoded));
        }
        //IndividualNumberZHROrecoded property

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public string PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) != null)
                {
                    return (string)_PassportNumber.Get();
                }
                else
                {
                    return _PassportNumber_Not_Valid;
                }
            }
            set
            {
                _PassportNumber_Not_Valid = value;
                if (GetErrors(nameof(PassportNumber)) != null)
                {
                    _PassportNumber.Set(_PassportNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
        private IDataLoadEngine _PassportNumber;
        private string _PassportNumber_Not_Valid = "";
        private void PassportNumber_Validation()
        {
            ClearErrors(nameof(PassportNumber));
        }
        //PassportNumber property

        //PassportNumberNote property
        public string PassportNumberNote
        {
            get
            {
                if (GetErrors(nameof(PassportNumberNote)) != null)
                {
                    return (string)_PassportNumberNote.Get();
                }
                else
                {
                    return _PassportNumberNote_Not_Valid;
                }
            }
            set
            {
                _PassportNumberNote_Not_Valid = value;
                if (GetErrors(nameof(PassportNumberNote)) != null)
                {
                    _PassportNumberNote.Set(_PassportNumberNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }
        private IDataLoadEngine _PassportNumberNote;
        private string _PassportNumberNote_Not_Valid = "";
        private void PassportNumberNote_Validation()
        {
            ClearErrors(nameof(PassportNumberNote));
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PassportNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PassportNumberRecoded)) != null)
                {
                    return (string)_PassportNumberRecoded.Get();
                }
                else
                {
                    return _PassportNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _PassportNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(PassportNumberRecoded)) != null)
                {
                    _PassportNumberRecoded.Set(_PassportNumberRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        private IDataLoadEngine _PassportNumberRecoded;//If change this change validation
        private string _PassportNumberRecoded_Not_Valid = "";
        private void PassportNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PassportNumberRecoded));
        }
        //PassportNumberRecoded property

        //Volume6 property
        [Attributes.Form_Property("Объем, куб. м")]
        public double Volume6
        {
            get
            {
                if (GetErrors(nameof(Volume6)) != null)
                {
                    return (double)_Volume6.Get();
                }
                else
                {
                    return _Volume6_Not_Valid;
                }
            }
            set
            {
                _Volume6_Not_Valid = value;
                if (GetErrors(nameof(Volume6)) != null)
                {
                    _Volume6.Set(_Volume6_Not_Valid);
                }
                OnPropertyChanged(nameof(Volume6));
            }
        }
        private IDataLoadEngine _Volume6;
        private double _Volume6_Not_Valid = -1;
        private void Volume6_Validation(double value)//TODO
        {
            ClearErrors(nameof(Volume6));
        }
        //Volume6 property

        //Mass7 Property
        [Attributes.Form_Property("Масса, т")]
        public double Mass7
        {
            get
            {
                if (GetErrors(nameof(Mass7)) != null)
                {
                    return (double)_Mass7.Get();
                }
                else
                {
                    return _Mass7_Not_Valid;
                }
            }
            set
            {
                _Mass7_Not_Valid = value;
                if (GetErrors(nameof(Mass7)) != null)
                {
                    _Mass7.Set(_Mass7_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass7));
            }
        }
        private IDataLoadEngine _Mass7;
        private double _Mass7_Not_Valid = -1;
        private void Mass7_Validation()//TODO
        {
            ClearErrors(nameof(Mass7));
        }
        //Mass7 Property

        //SaltConcentration property
        [Attributes.Form_Property("Солесодержание, г/л")]
        public double SaltConcentration
        {
            get
            {
                if (GetErrors(nameof(SaltConcentration)) != null)
                {
                    return (double)_SaltConcentration.Get();
                }
                else
                {
                    return _SaltConcentration_Not_Valid;
                }
            }
            set
            {
                _SaltConcentration_Not_Valid = value;
                if (GetErrors(nameof(SaltConcentration)) != null)
                {
                    _SaltConcentration.Set(_SaltConcentration_Not_Valid);
                }
                OnPropertyChanged(nameof(SaltConcentration));
            }
        }
        private IDataLoadEngine _SaltConcentration;
        private double _SaltConcentration_Not_Valid = -1;
        private void SaltConcentration_Validation(double value)
        {
            ClearErrors(nameof(SaltConcentration));
        }
        //SaltConcentration property

        //Radionuclids property
        [Attributes.Form_Property("Наименования радионуклидов")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    return (string)_Radionuclids.Get();
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                _Radionuclids_Not_Valid = value;
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    _Radionuclids.Set(_Radionuclids_Not_Valid);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        private IDataLoadEngine _Radionuclids;//If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation()//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }
        //Radionuclids property

        //SpecificActivity property
        [Attributes.Form_Property("Удельная активность, Бк/г")]
        public string SpecificActivity
        {
            get
            {
                if (GetErrors(nameof(SpecificActivity)) != null)
                {
                    return (string)_SpecificActivity.Get();
                }
                else
                {
                    return _SpecificActivity_Not_Valid;
                }
            }
            set
            {
                _SpecificActivity_Not_Valid = value;
                if (GetErrors(nameof(SpecificActivity)) != null)
                {
                    _SpecificActivity.Set(_SpecificActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(SpecificActivity));
            }
        }
        private IDataLoadEngine _SpecificActivity;
        private string _SpecificActivity_Not_Valid = "";
        private void SpecificActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(SpecificActivity));
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(SpecificActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(SpecificActivity), "Недопустимое значение");
            }
        }
        //SpecificActivity property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public string ProviderOrRecieverOKPO
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) != null)
                {
                    return (string)_ProviderOrRecieverOKPO.Get();
                }
                else
                {
                    return _ProviderOrRecieverOKPO_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPO_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) != null)
                {
                    _ProviderOrRecieverOKPO.Set(_ProviderOrRecieverOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }
        private IDataLoadEngine _ProviderOrRecieverOKPO;
        private string _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation()//TODO
        {
            ClearErrors(nameof(ProviderOrRecieverOKPO));
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public string ProviderOrRecieverOKPONote
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) != null)
                {
                    return (string)_ProviderOrRecieverOKPONote.Get();
                }
                else
                {
                    return _ProviderOrRecieverOKPONote_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPONote_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) != null)
                {
                    _ProviderOrRecieverOKPONote.Set(_ProviderOrRecieverOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }
        private IDataLoadEngine _ProviderOrRecieverOKPONote;
        private string _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation()
        {
            ClearErrors(nameof(ProviderOrRecieverOKPONote));
        }
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        [Attributes.Form_Property("ОКПО перевозчика")]
        public string TransporterOKPO
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPO)) != null)
                {
                    return (string)_TransporterOKPO.Get();
                }
                else
                {
                    return _TransporterOKPO_Not_Valid;
                }
            }
            set
            {
                _TransporterOKPO_Not_Valid = value;
                if (GetErrors(nameof(TransporterOKPO)) != null)
                {
                    _TransporterOKPO.Set(_TransporterOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }
        private IDataLoadEngine _TransporterOKPO;
        private string _TransporterOKPO_Not_Valid = "";
        private void TransporterOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransporterOKPO));
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public string TransporterOKPONote
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPONote)) != null)
                {
                    return (string)_TransporterOKPONote.Get();
                }
                else
                {
                    return _TransporterOKPONote_Not_Valid;
                }
            }
            set
            {
                _TransporterOKPONote_Not_Valid = value;
                if (GetErrors(nameof(TransporterOKPONote)) != null)
                {
                    _TransporterOKPONote.Set(_TransporterOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }
        private IDataLoadEngine _TransporterOKPONote;
        private string _TransporterOKPONote_Not_Valid = "";
        private void TransporterOKPONote_Validation()
        {
            ClearErrors(nameof(TransporterOKPONote));
        }
        //TransporterOKPONote property

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]
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

        //StoragePlaceNameNote property
        public string StoragePlaceNameNote
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceNameNote)) != null)
                {
                    return (string)_StoragePlaceNameNote.Get();
                }
                else
                {
                    return _StoragePlaceNameNote_Not_Valid;
                }
            }
            set
            {
                _StoragePlaceNameNote_Not_Valid = value;
                if (GetErrors(nameof(StoragePlaceNameNote)) != null)
                {
                    _StoragePlaceNameNote.Set(_StoragePlaceNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        private IDataLoadEngine _StoragePlaceNameNote;//If change this change validation
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

        //CodeRAO property
        [Attributes.Form_Property("Код РАО")]
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
        [Attributes.Form_Property("Статус РАО")]
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

        //Volume20 property
        [Attributes.Form_Property("Объем, куб. м")]
        public double Volume20
        {
            get
            {
                if (GetErrors(nameof(Volume20)) != null)
                {
                    return (double)_Volume20.Get();
                }
                else
                {
                    return _Volume20_Not_Valid;
                }
            }
            set
            {
                _Volume20_Not_Valid = value;
                if (GetErrors(nameof(Volume20)) != null)
                {
                    _Volume20.Set(_Volume20_Not_Valid);
                }
                OnPropertyChanged(nameof(Volume20));
            }
        }
        private IDataLoadEngine _Volume20;
        private double _Volume20_Not_Valid = -1;
        private void Volume20_Validation(double value)//TODO
        {
            ClearErrors(nameof(Volume20));
        }
        //Volume20 property

        //Mass21 Property
        [Attributes.Form_Property("Масса, т")]
        public double Mass21
        {
            get
            {
                if (GetErrors(nameof(Mass21)) != null)
                {
                    return (double)_Mass21.Get();
                }
                else
                {
                    return _Mass21_Not_Valid;
                }
            }
            set
            {
                _Mass21_Not_Valid = value;
                if (GetErrors(nameof(Mass21)) != null)
                {
                    _Mass21.Set(_Mass21_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass21));
            }
        }
        private IDataLoadEngine _Mass21;
        private double _Mass21_Not_Valid = -1;
        private void Mass21_Validation()//TODO
        {
            ClearErrors(nameof(Mass21));
        }
        //Mass21 Property

        //TritiumActivity property
        [Attributes.Form_Property("Активность трития, Бк")]
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
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
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
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
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
        [Attributes.Form_Property("Активность трансурановых, Бк")]
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

        //RefineOrSortRAOCode property
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public string RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                if (GetErrors(nameof(RefineOrSortRAOCode)) != null)
                {
                    return (string)_RefineOrSortRAOCode.Get();
                }
                else
                {
                    return _RefineOrSortRAOCode_Not_Valid;
                }
            }
            set
            {
                _RefineOrSortRAOCode_Not_Valid = value;
                if (GetErrors(nameof(RefineOrSortRAOCode)) != null)
                {
                    _RefineOrSortRAOCode.Set(_RefineOrSortRAOCode_Not_Valid);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        private IDataLoadEngine _RefineOrSortRAOCode;//If change this change validation
        private string _RefineOrSortRAOCode_Not_Valid = "";
        private void RefineOrSortRAOCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(RefineOrSortRAOCode));
            if (value.Length != 2)
                AddError(nameof(RefineOrSortRAOCode), "Недопустимое значение");
            else
                for (int i = 0; i < 2; i++)
                {
                    if (!((value[i] >= '0') && (value[i] <= '9')))
                    {
                        AddError(nameof(RefineOrSortRAOCode), "Недопустимое значение");
                        return;
                    }
                }
        }
        //RefineOrSortRAOCode property

        //Subsidy property
        [Attributes.Form_Property("Субсидия, %")]
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
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
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
