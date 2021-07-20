using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using ClassLibrary1;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.6: Сведения о некондиционированных РАО")]
    public class Form16 : Abstracts.Form1
    {
        public Form16() : base()
        {
            //FormNum.Value = "16";
            //NumberOfFields.Value = 37;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(CodeRAO), CodeRAO_Validation, null);
            CodeRAO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackName), PackName_Validation, null);
            PackName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackNumberRecoded), PackNumberRecoded_Validation, null);
            PackNumberRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackNumber), PackNumber_Validation, null);
            PackNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackTypeRecoded), PackTypeRecoded_Validation, null);
            PackTypeRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackType), PackType_Validation, null);
            PackType.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Volume), Volume_Validation, null);
            Volume.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Mass), Mass_Validation, null);
            Mass.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ProviderOrRecieverOKPO), ProviderOrRecieverOKPO_Validation, null);
            ProviderOrRecieverOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransporterOKPO), TransporterOKPO_Validation, null);
            TransporterOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TritiumActivity), TritiumActivity_Validation, null);
            TritiumActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(BetaGammaActivity), BetaGammaActivity_Validation, null);
            BetaGammaActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(AlphaActivity), AlphaActivity_Validation, null);
            AlphaActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransuraniumActivity), TransuraniumActivity_Validation, null);
            TransuraniumActivity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(MainRadionuclids), MainRadionuclids_Validation, null);
            MainRadionuclids.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ActivityMeasurementDate), ActivityMeasurementDate_Validation, null);
            ActivityMeasurementDate.PropertyChanged += InPropertyChanged;
            DataAccess.Init<int?>(nameof(QuantityOZIII), QuantityOZIII_Validation, null);
            QuantityOZIII.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(RefineOrSortRAOCode), RefineOrSortRAOCode_Validation, null);
            RefineOrSortRAOCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StatusRAO), StatusRAO_Validation, null);
            StatusRAO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Subsidy), Subsidy_Validation, null);
            Subsidy.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            CodeRAO_Validation(CodeRAO);
            PackName_Validation(PackName);
            PackNumberRecoded_Validation(PackNumberRecoded);
            PackNumber_Validation(PackNumber);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackType_Validation(PackType);
            Volume_Validation(Volume);
            Mass_Validation(Mass);
            ActivityMeasurementDate_Validation(ActivityMeasurementDate);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            TransporterOKPO_Validation(TransporterOKPO);
            TritiumActivity_Validation(TritiumActivity);
            BetaGammaActivity_Validation(BetaGammaActivity);
            AlphaActivity_Validation(AlphaActivity);
            TransuraniumActivity_Validation(TransuraniumActivity);
            MainRadionuclids_Validation(MainRadionuclids);
            QuantityOZIII_Validation(QuantityOZIII);
            RefineOrSortRAOCode_Validation(RefineOrSortRAOCode);
            Subsidy_Validation(Subsidy);
            FcpNumber_Validation(FcpNumber);
            StatusRAO_Validation(StatusRAO);
            StoragePlaceName_Validation(StoragePlaceName);
            StoragePlaceCode_Validation(StoragePlaceCode);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //CodeRAO property
        public int? CodeRAOId { get; set; }
        [Attributes.Form_Property("Код РАО")]
        public virtual RamAccess<string> CodeRAO
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(CodeRAO));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CodeRAO), value);
                }
                OnPropertyChanged(nameof(CodeRAO));
            }
        }


        private bool CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CodeRAO property

        //StatusRAO property
        public int? StatusRAOId { get; set; }
        [Attributes.Form_Property("Статус РАО")]
        public virtual RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StatusRAO));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StatusRAO), value);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }


        private bool StatusRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError("Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    value.AddError("Недопустимое значение");
                }
                return false;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //StatusRAO property

        //Volume property
        public int? VolumeId { get; set; }
        [Attributes.Form_Property("Объем, куб. м")]
        public virtual RamAccess<string> Volume
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Volume));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Volume), value);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }


        private bool Volume_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            try
            {
                if (!(double.Parse(tmp) > 0))
                {
                    value.AddError("Число должно быть больше нуля");
                    return false;
                }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Volume property

        //Mass Property
        public int? MassId { get; set; }
        [Attributes.Form_Property("Масса, кг")]
        public virtual RamAccess<string> Mass
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Mass));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Mass), value);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }


        private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            try
            {
                if (!(double.Parse(tmp) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Mass Property

        //MainRadionuclids property
        public int? MainRadionuclidsId { get; set; }
        [Attributes.Form_Property("Радионуклиды")]
        public virtual RamAccess<string> MainRadionuclids
        {
            get
            {
                {
                    return DataAccess.Get<string>(nameof(MainRadionuclids));
                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(MainRadionuclids), value);
                OnPropertyChanged(nameof(MainRadionuclids));
            }
        }
        //If change this change validation

        private bool MainRadionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            foreach (var item in Spravochniki.SprRadionuclids)
            {
                if (item.Item1.Equals(value.Value))
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
        }
        //MainRadionuclids property

        //TritiumActivity property
        public int? TritiumActivityId { get; set; }
        [Attributes.Form_Property("Активность трития, Бк")]
        public virtual RamAccess<string> TritiumActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(TritiumActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(TritiumActivity), value);
                }
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }


        private bool TritiumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //TritiumActivity property

        //BetaGammaActivity property
        public int? BetaGammaActivityId { get; set; }
        [Attributes.Form_Property("Активность бета-, гамма-излучающих, кроме трития, Бк")]
        public virtual RamAccess<string> BetaGammaActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(BetaGammaActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(BetaGammaActivity), value);
                }
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }


        private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //BetaGammaActivity property

        //AlphaActivity property
        public int? AlphaActivityId { get; set; }
        [Attributes.Form_Property("Активность альфа-излучающих, кроме трансурановых, Бк")]
        public virtual RamAccess<string> AlphaActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(AlphaActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(AlphaActivity), value);
                }
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }


        private bool AlphaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //AlphaActivity property

        //TransuraniumActivity property
        public int? TransuraniumActivityId { get; set; }
        [Attributes.Form_Property("Активность трансурановых, Бк")]
        public virtual RamAccess<string> TransuraniumActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(TransuraniumActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(TransuraniumActivity), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }


        private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //TransuraniumActivity property

        //ActivityMeasurementDate property
        public int? ActivityMeasurementDateId { get; set; }
        [Attributes.Form_Property("Дата измерения активности")]
        
        public virtual RamAccess<string> ActivityMeasurementDate
        {
            get
            {
                return DataAccess.Get<string>(nameof(ActivityMeasurementDate));
            }
            set
            {
                DataAccess.Set(nameof(ActivityMeasurementDate), value);
                OnPropertyChanged(nameof(ActivityMeasurementDate));
            }
        }
        //if change this change validation

        private bool ActivityMeasurementDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ActivityMeasurementDate property

        //QuantityOZIII property
        public int? QuantityOZIIIId { get; set; }
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public virtual RamAccess<int?> QuantityOZIII
        {
            get
            {

                {
                    return DataAccess.Get<int?>(nameof(QuantityOZIII));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityOZIII), value);
                }
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }
        // positive int.

        private bool QuantityOZIII_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityOZIIIout property

        //ProviderOrRecieverOKPO property
        public int? ProviderOrRecieverOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public virtual RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }


        private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            bool a = (OperationCode.Value >= 10) && (OperationCode.Value <= 14);
            bool b = (OperationCode.Value >= 41) && (OperationCode.Value <= 45);
            bool c = (OperationCode.Value >= 71) && (OperationCode.Value <= 73);
            bool e = (OperationCode.Value >= 55) && (OperationCode.Value <= 57);
            bool d = (OperationCode.Value == 1) || (OperationCode.Value == 16) || (OperationCode.Value == 18) || (OperationCode.Value == 48) ||
                (OperationCode.Value == 49) || (OperationCode.Value == 51) || (OperationCode.Value == 52) || (OperationCode.Value == 59) ||
                (OperationCode.Value == 68) || (OperationCode.Value == 75) || (OperationCode.Value == 76);
            if (a || b || c || d || e)
            {
                ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                return false;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;

            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //ProviderOrRecieverOKPO property

        ////ProviderOrRecieverOKPONote property
        //public virtual RamAccess<string> ProviderOrRecieverOKPONote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
        //        }
        //        OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
        //    }
        //}


        //private bool ProviderOrRecieverOKPONote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        public int? TransporterOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО перевозчика")]
        public virtual RamAccess<string> TransporterOKPO
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(TransporterOKPO));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(TransporterOKPO), value);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }


        private bool TransporterOKPO_Validation(RamAccess<string> value)//Done
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TransporterOKPO property

        ////TransporterOKPONote property
        //public virtual RamAccess<string> TransporterOKPONote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(TransporterOKPONote));//OK

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(TransporterOKPONote), value);
        //        }
        //        OnPropertyChanged(nameof(TransporterOKPONote));
        //    }
        //}


        //private bool TransporterOKPONote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////TransporterOKPONote property

        //PackName property
        public int? PackNameId { get; set; }
        [Attributes.Form_Property("Наименование упаковки")]
        public virtual RamAccess<string> PackName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackName));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }


        private bool PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNameNote == null) || PackNameNote.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            return true;
        }
        //PackName property

        ////PackNameNote property
        //public virtual RamAccess<string> PackNameNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(PackNameNote));//OK

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(PackNameNote), value);
        //        }
        //        OnPropertyChanged(nameof(PackNameNote));
        //    }
        //}


        //private bool PackNameNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////PackNameNote property

        //PackType property
        public int? PackTypeId { get; set; }
        [Attributes.Form_Property("Тип упаковки")]
        public virtual RamAccess<string> PackType
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackType));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation

        private bool PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                //    value.AddError( "Заполните примечание");//to do note handling
                return true;
            }
            return true;
        }
        //PackType property

        //PackTypeRecoded property
        public virtual RamAccess<string> PackTypeRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackTypeRecoded));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }


        private bool PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PackTypeRecoded property

        ////PackTypeNote property
        //public virtual RamAccess<string> PackTypeNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(PackTypeNote));//OK

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(PackTypeNote), value);
        //        }
        //        OnPropertyChanged(nameof(PackTypeNote));
        //    }
        //}


        //private bool PackTypeNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////PackTypeNote property

        //PackNumber property
        public int? PackNumberId { get; set; }
        [Attributes.Form_Property("Номер упаковки")]
        public virtual RamAccess<string> PackNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackNumber));
                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackNumber), value);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation

        private bool PackNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            return true;
        }
        //PackNumber property

        ////PackNumberNote property
        //public virtual RamAccess<string> PackNumberNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(PackNumberNote));//OK

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {



        //        {
        //            DataAccess.Set(nameof(PackNumberNote), value);
        //        }
        //        OnPropertyChanged(nameof(PackNumberNote));
        //    }
        //}


        //private bool PackNumberNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors();
        //    if ((value.Value == null) || value.Value.Equals(""))
        //    {
        //        value.AddError("Поле не заполнено");
        //        return false;
        //    }
        //    return true;
        //}
        ////PackNumberNote property

        //StoragePlaceName property
        public int? StoragePlaceNameId { get; set; }
        [Attributes.Form_Property("Наименование ПХ")]
        public virtual RamAccess<string> StoragePlaceName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceName), value);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }

        private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var a = new List<string>();//here binds spr
            if (a.Contains(value.Value))
                return true;
            value.AddError("Недопустимое значение");
            return false;
        }
        //StoragePlaceName property

        //StoragePlaceCode property
        public int? StoragePlaceCodeId { get; set; }
        [Attributes.Form_Property("Код ПХ")]
        public virtual RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceCode));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceCode), value);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation

        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var lst = new List<string>();//HERE binds spr
            if (!lst.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceCode property

        //PackNumberRecoded property
        //public int? PackNumberRecodedId { get; set; }
        public virtual RamAccess<string> PackNumberRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackNumberRecoded));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation

        private bool PackNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //PackNumberRecoded property

        //Subsidy property
        public int? SubsidyId { get; set; }
        [Attributes.Form_Property("Субсидия, %")]
        public virtual RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Subsidy));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Subsidy), value);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }


        private bool Subsidy_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            try
            {
                int tmp = Int32.Parse(value.Value);
                if (!((tmp > 0) && (tmp <= 100)))
                {
                    value.AddError("Недопустимое значение"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Subsidy property

        //FcpNumber property
        public int? FcpNumberId { get; set; }
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public virtual RamAccess<string> FcpNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(FcpNumber));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }


        private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        //FcpNumber property

        //RefineOrSortRAOCode property
        public int? RefineOrSortRAOCodeId { get; set; }
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public virtual RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(RefineOrSortRAOCode));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(RefineOrSortRAOCode), value);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        //If change this change validation

        private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var a = new Regex("^[0-9][0-9]$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //RefineOrSortRAOCode property

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            bool flag = false;
            foreach (var item in spr)
            {
                if (item == value.Value) flag = true;
            }
            if (!flag)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            bool a0 = value.Value == 15;
            bool a1 = value.Value == 17;
            bool a2 = value.Value == 46;
            bool a3 = value.Value == 47;
            bool a4 = value.Value == 53;
            bool a5 = value.Value == 54;
            bool a6 = value.Value == 58;
            bool a7 = value.Value == 61;
            bool a8 = value.Value == 62;
            bool a9 = value.Value == 65;
            bool a10 = value.Value == 66;
            bool a11 = value.Value == 67;
            bool a12 = value.Value == 81;
            bool a13 = value.Value == 82;
            bool a14 = value.Value == 83;
            bool a15 = value.Value == 85;
            bool a16 = value.Value == 86;
            bool a17 = value.Value == 87;
            if (a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11 || a12 || a13 || a14 || a15 || a16 || a17)
                value.AddError("Код операции не может быть использован для РАО");
            return false;
        }
    }
}
