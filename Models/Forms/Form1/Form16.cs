using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Spravochniki;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;

namespace Models
{
    [Attributes.Form_Class("Форма 1.6: Сведения о некондиционированных РАО")]
    public class Form16 : Abstracts.Form1
    {
        public Form16() : base()
        {
            FormNum.Value = "1.6";
            Validate_all();
        }

        private void Validate_all()
        {
            CodeRAO_Validation(CodeRAO);
            PackName_Validation(PackName);
            PackNumber_Validation(PackNumber);
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
            return !(CodeRAO.HasErrors||
            PackName.HasErrors||
            PackNumber.HasErrors||
            PackType.HasErrors||
            Volume.HasErrors||
            Mass.HasErrors||
            ActivityMeasurementDate.HasErrors||
            ProviderOrRecieverOKPO.HasErrors||
            TransporterOKPO.HasErrors||
            TritiumActivity.HasErrors||
            BetaGammaActivity.HasErrors||
            AlphaActivity.HasErrors||
            TransuraniumActivity.HasErrors||
            MainRadionuclids.HasErrors||
            QuantityOZIII.HasErrors||
            RefineOrSortRAOCode.HasErrors||
            Subsidy.HasErrors||
            FcpNumber.HasErrors||
            StatusRAO.HasErrors||
            StoragePlaceName.HasErrors||
            StoragePlaceCode.HasErrors);
        }

        #region CodeRAO
        public string CodeRAO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Код РАО")]
        public RamAccess<string> CodeRAO
        {
            get
            {
                var tmp = new RamAccess<string>(CodeRAO_Validation, CodeRAO_DB);
                tmp.PropertyChanged += CodeRAOValueChanged;
                return tmp;
            }
            set
            {
                CodeRAO_DB = value.Value;
                OnPropertyChanged(nameof(CodeRAO));
            }
        }
        private void CodeRAOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value.ToLower();
                tmp = tmp.Replace("х", "x");
                CodeRAO_DB = tmp;
            }
        }
        private bool CodeRAO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = value.Value.ToLower();
            tmp = tmp.Replace("х", "x");
            Regex a = new Regex("^[0-9x+]{11}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region StatusRAO
        public string StatusRAO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Статус РАО")]
        public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                var tmp = new RamAccess<string>(StatusRAO_Validation, StatusRAO_DB);
                tmp.PropertyChanged += StatusRAOValueChanged;
                return tmp;
            }
            set
            {
                StatusRAO_DB = value.Value;
                OnPropertyChanged(nameof(StatusRAO));
            }
        }
        private void StatusRAOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                StatusRAO_DB = ((RamAccess<string>)Value).Value;
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
        #endregion

        #region Volume
        public string Volume_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("объем без упаковки, куб. м")]
        public RamAccess<string> Volume
        {
            get
            {
                var tmp = new RamAccess<string>(Volume_Validation, Volume_DB);
                tmp.PropertyChanged += VolumeValueChanged;
                return tmp;
            }
            set
            {
                Volume_DB = value.Value;
                OnPropertyChanged(nameof(Volume));
            }
        }
        private void VolumeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    Volume_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                Volume_DB = value1;
            }
        }
        private bool Volume_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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
        #endregion

        #region Mass
        public string Mass_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("масса без упаковки (нетто), т")]
        public RamAccess<string> Mass
        {
            get
            {
                var tmp = new RamAccess<string>(Mass_Validation, Mass_DB);
                tmp.PropertyChanged += MassValueChanged;
                return tmp;
            }
            set
            {
                Mass_DB = value.Value;
                OnPropertyChanged(nameof(Mass));
            }
        }
        private void MassValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    Mass_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                Mass_DB = value1;
            }
        }
        private bool Mass_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
              NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region MainRadionuclids
        public string MainRadionuclids_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Основные радионуклиды")]
        public RamAccess<string> MainRadionuclids
        {
            get
            {
                var tmp = new RamAccess<string>(MainRadionuclids_Validation, MainRadionuclids_DB);
                tmp.PropertyChanged += MainRadionuclidsValueChanged;
                return tmp;
            }
            set
            {
                MainRadionuclids_DB = value.Value;
                OnPropertyChanged(nameof(MainRadionuclids));
            }
        }//If change this change validation

        private void MainRadionuclidsValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                MainRadionuclids_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool MainRadionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            string[] nuclids = value.Value.Split(";");
            for (int k = 0; k < nuclids.Length; k++)
            {
                nuclids[k] = nuclids[k].ToLower().Replace(" ", "");
            }
            bool flag = true;
            foreach (var nucl in nuclids)
            {
                var tmp = from item in Spravochniks.SprRadionuclids where nucl == item.Item1 select item.Item1;
                if (tmp.Count() == 0)
                    flag = false;
            }
            if (!flag)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region TritiumActivity
        public string TritiumActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("тритий")]
        public RamAccess<string> TritiumActivity
        {
            get
            {
                var tmp = new RamAccess<string>(TritiumActivity_Validation, TritiumActivity_DB);
                tmp.PropertyChanged += TritiumActivityValueChanged;
                return tmp;
            }
            set
            {
                TritiumActivity_DB = value.Value;
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }
        private void TritiumActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TritiumActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                TritiumActivity_DB = value1;
            }
        }
        private bool TritiumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        #region BetaGammaActivity
        public string BetaGammaActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("бета-, гамма-излучающие радионуклиды (исключая тритий)")]
        public RamAccess<string> BetaGammaActivity
        {
            get
            {
                var tmp = new RamAccess<string>(BetaGammaActivity_Validation, BetaGammaActivity_DB);
                tmp.PropertyChanged += BetaGammaActivityValueChanged;
                return tmp;
            }
            set
            {
                BetaGammaActivity_DB = value.Value;
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }
        private void BetaGammaActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    BetaGammaActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                BetaGammaActivity_DB = value1;
            }
        }
        private bool BetaGammaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        #region AlphaActivity
        public string AlphaActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("альфа-излучающие радионуклиды (исключая трансурановые)")]
        public RamAccess<string> AlphaActivity
        {
            get
            {
                var tmp = new RamAccess<string>(AlphaActivity_Validation, AlphaActivity_DB);
                tmp.PropertyChanged += AlphaActivityValueChanged;
                return tmp;
            }
            set
            {
                AlphaActivity_DB = value.Value;
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }
        private void AlphaActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    AlphaActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                AlphaActivity_DB = value1;
            }
        }
        private bool AlphaActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        #region TransuraniumActivity
        public string TransuraniumActivity_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("трансурановые радионуклиды")]
        public RamAccess<string> TransuraniumActivity
        {
            get
            {
                var tmp = new RamAccess<string>(TransuraniumActivity_Validation, TransuraniumActivity_DB);
                tmp.PropertyChanged += TransuraniumActivityValueChanged;
                return tmp;
            }
            set
            {
                TransuraniumActivity_DB = value.Value;
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }
        private void TransuraniumActivityValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
                if (value1.Equals("-"))
                {
                    TransuraniumActivity_DB = value1;
                    return;
                }
                if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
                {
                    value1 = value1.Replace("+", "e+").Replace("-", "e-");
                }
                TransuraniumActivity_DB = value1;
            }
        }
        private bool TransuraniumActivity_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if(string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value == "-")
            {
                return true;
            }
            var value1 = value.Value.Replace('е', 'e').Replace('Е', 'e').Replace('E', 'e');
            if ((!value1.Contains('e')) && (value1.Contains('+') ^ value1.Contains('-')))
            {
                value1 = value1.Replace("+", "e+").Replace("-", "e-");
            }
            string tmp = value1;
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
        #endregion

        #region ActivityMeasurementDate
        public string ActivityMeasurementDate_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Дата измерения активности")]

        public virtual RamAccess<string> ActivityMeasurementDate
        {
            get
            {
                var tmp = new RamAccess<string>(ActivityMeasurementDate_Validation, ActivityMeasurementDate_DB);
                tmp.PropertyChanged += ActivityMeasurementDateValueChanged;
                return tmp;
            }
            set
            {
                ActivityMeasurementDate_DB = value.Value;
                OnPropertyChanged(nameof(ActivityMeasurementDate));
            }
        }//if change this change validation

        private void ActivityMeasurementDateValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var tmp = ((RamAccess<string>)Value).Value;
                Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
                if (b.IsMatch(tmp))
                {
                    tmp = tmp.Insert(6, "20");
                }
                ActivityMeasurementDate_DB = tmp;
            }
        }
        private bool ActivityMeasurementDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            var tmp = value.Value;
            Regex b = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (b.IsMatch(tmp))
            {
                tmp = tmp.Insert(6, "20");
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(tmp))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(tmp); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        #region QuantityOZIII
        public string QuantityOZIII_DB { get; set; } = null;
        [NotMapped]
        [Attributes.Form_Property("Количество ОЗИИИ, шт.")]
        public RamAccess<string> QuantityOZIII
        {
            get
            {
                var tmp = new RamAccess<string>(QuantityOZIII_Validation, QuantityOZIII_DB);
                tmp.PropertyChanged += QuantityOZIIIValueChanged;
                return tmp;
            }
            set
            {
                QuantityOZIII_DB = value.Value;
                OnPropertyChanged(nameof(QuantityOZIII));
            }
        }// positive int.

        private void QuantityOZIIIValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                QuantityOZIII_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool QuantityOZIII_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            try
            {
                if (int.Parse(value.Value) <= 0)
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
        #endregion

        #region ProviderOrRecieverOKPO
        public string ProviderOrRecieverOKPO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("поставщика или получателя")]
        public RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                var tmp = new RamAccess<string>(ProviderOrRecieverOKPO_Validation, ProviderOrRecieverOKPO_DB);
                tmp.PropertyChanged += ProviderOrRecieverOKPOValueChanged;
                return tmp;
            }
            set
            {
                ProviderOrRecieverOKPO_DB = value.Value;
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }
        [NotMapped]
        public string OKPOofFormFiller { get; set; } = null;
        private void ProviderOrRecieverOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                var value1 = ((RamAccess<string>)Value).Value;
                value1 = value1.ToUpper();
                ProviderOrRecieverOKPO_DB = value1;
                bool a = (OperationCode.Value >= 10) && (OperationCode.Value <= 14);
                bool b = (OperationCode.Value >= 41) && (OperationCode.Value <= 45);
                bool c = (OperationCode.Value >= 71) && (OperationCode.Value <= 73);
                bool e = (OperationCode.Value >= 55) && (OperationCode.Value <= 57);
                bool d = (OperationCode.Value == 1) || (OperationCode.Value == 16) || (OperationCode.Value == 18) || (OperationCode.Value == 48) ||
                    (OperationCode.Value == 49) || (OperationCode.Value == 51) || (OperationCode.Value == 52) || (OperationCode.Value == 59) ||
                    (OperationCode.Value == 68) || (OperationCode.Value == 75) || (OperationCode.Value == 76);
                if (a || b || c || d || e)
                {
                    //ProviderOrRecieverOKPO_DB = OKPOofFormFiller;
                }
            }
        }
        private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (OKSM.Contains(value.Value.ToUpper()))
            {
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
        #endregion
        protected List<string> OKSM = new List<string>
            {
                "АФГАНИСТАН","АЛБАНИЯ","АНТАРКТИДА","АЛЖИР","АМЕРИКАНСКОЕ САМОА","АНДОРРА","АНГОЛА","АНТИГУА И БАРБУДА","АЗЕРБАЙДЖАН","АРГЕНТИНА","АВСТРАЛИЯ","АВСТРИЯ","БАГАМЫ","БАХРЕЙН",
                "БАНГЛАДЕШ","АРМЕНИЯ","БАРБАДОС","БЕЛЬГИЯ","БЕРМУДЫ","БУТАН","БОЛИВИЯ, МНОГОНАЦИОНАЛЬНОЕ ГОСУДАРСТВО","БОСНИЯ И ГЕРЦЕГОВИНА","БОТСВАНА","ОСТРОВ БУВЕ","БРАЗИЛИЯ","БЕЛИЗ",
                "БРИТАНСКАЯ ТЕРРИТОРИЯ В ИНДИЙСКОМ ОКЕАНЕ","СОЛОМОНОВЫ ОСТРОВА","ВИРГИНСКИЕ ОСТРОВА (БРИТАНСКИЕ)","БРУНЕЙ-ДАРУССАЛАМ","БОЛГАРИЯ","МЬЯНМА","БУРУНДИ","БЕЛАРУСЬ","КАМБОДЖА",
                "КАМЕРУН","КАНАДА","КАБО-ВЕРДЕ","ОСТРОВА КАЙМАН","ЦЕНТРАЛЬНО-АФРИКАНСКАЯ РЕСПУБЛИКА","ШРИ-ЛАНКА","ЧАД","ЧИЛИ","КИТАЙ","ТАЙВАНЬ (КИТАЙ)","ОСТРОВ РОЖДЕСТВА","КОКОСОВЫЕ (КИЛИНГ) ОСТРОВА",
                "КОЛУМБИЯ","КОМОРЫ","МАЙОТТА","КОНГО","КОНГО, ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ОСТРОВА КУКА","КОСТА-РИКА","ХОРВАТИЯ","КУБА","КИПР","ЧЕХИЯ","БЕНИН","ДАНИЯ","ДОМИНИКА","ДОМИНИКАНСКАЯ РЕСПУБЛИКА",
                "ЭКВАДОР","ЭЛЬ-САЛЬВАДОР","ЭКВАТОРИАЛЬНАЯ ГВИНЕЯ","ЭФИОПИЯ","ЭРИТРЕЯ","ЭСТОНИЯ","ФАРЕРСКИЕ ОСТРОВА","ФОЛКЛЕНДСКИЕ ОСТРОВА (МАЛЬВИНСКИЕ)","ЮЖНАЯ ДЖОРДЖИЯ И ЮЖНЫЕ САНДВИЧЕВЫ ОСТРОВА",
                "ФИНЛЯНДИЯ","ЭЛАНДСКИЕ ОСТРОВА","ФРАНЦИЯ","ФРАНЦУЗСКАЯ ГВИАНА","БОНЭЙР, СИНТ-ЭСТАТИУС И САБА","НОВАЯ КАЛЕДОНИЯ","ВАНУАТУ","НОВАЯ ЗЕЛАНДИЯ","НИКАРАГУА","НИГЕР","ФИДЖИ",
                "ФРАНЦУЗСКАЯ ПОЛИНЕЗИЯ","ФРАНЦУЗСКИЕ ЮЖНЫЕ ТЕРРИТОРИИ","ДЖИБУТИ","ГАБОН","ГРУЗИЯ","ГАМБИЯ","ПАЛЕСТИНА, ГОСУДАРСТВО","ГЕРМАНИЯ","ГАНА","ГИБРАЛТАР","КИРИБАТИ","МАЛИ","МАЛЬТА",
                "ГРЕЦИЯ","ГРЕНЛАНДИЯ","ГРЕНАДА","ГВАДЕЛУПА","ГУАМ","ГВАТЕМАЛА","ГВИНЕЯ","ГАЙАНА","ГАИТИ","ОСТРОВ ХЕРД И ОСТРОВА МАКДОНАЛЬД","ПАПСКИЙ ПРЕСТОЛ (ГОСУДАРСТВО - ГОРОД ВАТИКАН)",
                "ГОНДУРАС","ГОНКОНГ","ВЕНГРИЯ","ИСЛАНДИЯ","ИНДИЯ","ИНДОНЕЗИЯ","ИРАН (ИСЛАМСКАЯ РЕСПУБЛИКА)","ИРАК","ИРЛАНДИЯ","ИЗРАИЛЬ","ИТАЛИЯ","КОТ Д'ИВУАР","ЯМАЙКА","ЯПОНИЯ","МАЛЬДИВЫ",
                "КАЗАХСТАН","ИОРДАНИЯ","КЕНИЯ","КОРЕЯ, НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","КОРЕЯ, РЕСПУБЛИКА","КУВЕЙТ","КИРГИЗИЯ","НИГЕРИЯ","НИУЭ","ОСТРОВ НОРФОЛК","НОРВЕГИЯ","СЕВЕРНЫЕ МАРИАНСКИЕ ОСТРОВА",
                "ЛАОССКАЯ НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ЛИВАН","ЛЕСОТО","ЛАТВИЯ","ЛИБЕРИЯ","ЛИВИЯ","ЛИХТЕНШТЕЙН","ЛИТВА","ЛЮКСЕМБУРГ","МАКАО","МАДАГАСКАР","МАЛАВИ","МАЛАЙЗИЯ",
                "МАРТИНИКА","МАВРИТАНИЯ","МАВРИКИЙ","МЕКСИКА","МОНАКО","МОНГОЛИЯ","МОЛДОВА, РЕСПУБЛИКА","ЧЕРНОГОРИЯ","МОНТСЕРРАТ","МАРОККО","МОЗАМБИК","ОМАН","НАМИБИЯ","НАУРУ","НЕПАЛ",
                "АРУБА","СЕН-МАРТЕН (нидерландская часть)","МАЛЫЕ ТИХООКЕАНСКИЕ ОТДАЛЕННЫЕ ОСТРОВА СОЕДИНЕННЫХ ШТАТОВ","МИКРОНЕЗИЯ, ФЕДЕРАТИВНЫЕ ШТАТЫ","МАРШАЛЛОВЫ ОСТРОВА","КЮРАСАО",
                "ПАЛАУ","ПАКИСТАН","ПАНАМА","ПАПУА-НОВАЯ ГВИНЕЯ","ПАРАГВАЙ","ПЕРУ","ФИЛИППИНЫ","ПИТКЕРН","ПОЛЬША","ПОРТУГАЛИЯ","ГВИНЕЯ-БИСАУ","ТИМОР-ЛЕСТЕ","ШВЕЦИЯ","ШВЕЙЦАРИЯ","НИДЕРЛАНДЫ",
                "ПУЭРТО-РИКО","КАТАР","РЕЮНЬОН","РУМЫНИЯ","РОССИЯ","РУАНДА","СЕН-БАРТЕЛЕМИ","СВЯТАЯ ЕЛЕНА, ОСТРОВ ВОЗНЕСЕНИЯ, ТРИСТАН-ДА-КУНЬЯ","СЕНТ-КИТС И НЕВИС","АНГИЛЬЯ","СЕНТ-ЛЮСИЯ",
                "СЕН-МАРТЕН (французская часть)","СЕН-ПЬЕР И МИКЕЛОН","СЕНТ-ВИНСЕНТ И ГРЕНАДИНЫ","САН-МАРИНО","САН-ТОМЕ И ПРИНСИПИ","САУДОВСКАЯ АРАВИЯ","СЕНЕГАЛ","СЕРБИЯ","СЕЙШЕЛЫ","ЮЖНЫЙ СУДАН",
                "СЬЕРРА-ЛЕОНЕ","СИНГАПУР","СЛОВАКИЯ","ВЬЕТНАМ","СЛОВЕНИЯ","СОМАЛИ","ЮЖНАЯ АФРИКА","ЗИМБАБВЕ","ИСПАНИЯ","ЗАПАДНАЯ САХАРА","СУДАН","СУРИНАМ","ШПИЦБЕРГЕН И ЯН МАЙЕН","ЭСВАТИНИ",
                "СИРИЙСКАЯ АРАБСКАЯ РЕСПУБЛИКА","ТАДЖИКИСТАН","ТАИЛАНД","ТОГО","ТОКЕЛАУ","ТОНГА","ТРИНИДАД И ТОБАГО","ОБЪЕДИНЕННЫЕ АРАБСКИЕ ЭМИРАТЫ","ТУНИС","ТУРЦИЯ","ТУРКМЕНИСТАН","ОСТРОВА ТЕРКС И КАЙКОС",
                "ТУВАЛУ","УГАНДА","УКРАИНА","СЕВЕРНАЯ МАКЕДОНИЯ","ЕГИПЕТ","СОЕДИНЕННОЕ КОРОЛЕВСТВО","ГЕРНСИ","ДЖЕРСИ","ОСТРОВ МЭН","ТАНЗАНИЯ, ОБЪЕДИНЕННАЯ РЕСПУБЛИКА","СОЕДИНЕННЫЕ ШТАТЫ",
                "ВИРГИНСКИЕ ОСТРОВА (США)","БУРКИНА-ФАСО","УРУГВАЙ","УЗБЕКИСТАН","ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)","УОЛЛИС И ФУТУНА","САМОА","ЙЕМЕН","ЗАМБИЯ","АБХАЗИЯ","ЮЖНАЯ ОСЕТИЯ"
            };
        #region TransporterOKPO
        public string TransporterOKPO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("перевозчика")]
        public RamAccess<string> TransporterOKPO
        {
            get
            {
                var tmp = new RamAccess<string>(TransporterOKPO_Validation, TransporterOKPO_DB);
                tmp.PropertyChanged += TransporterOKPOValueChanged;
                return tmp;
            }
            set
            {
                TransporterOKPO_DB = value.Value;
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }
        private void TransporterOKPOValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                TransporterOKPO_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool TransporterOKPO_Validation(RamAccess<string> value)//Done
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("-"))
            {
                return true;
            }
            if (value.Value.Equals("Минобороны"))
            {
                return true;
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
        #endregion

        #region PackName
        public string PackName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("наименование")]
        public RamAccess<string> PackName
        {
            get
            {
                var tmp = new RamAccess<string>(PackName_Validation, PackName_DB);
                tmp.PropertyChanged += PackNameValueChanged;
                return tmp;
            }
            set
            {
                PackName_DB = value.Value;
                OnPropertyChanged(nameof(PackName));
            }
        }
        private void PackNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PackName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        #endregion

        #region PackType
        public string PackType_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("тип")]
        public RamAccess<string> PackType
        {
            get
            {
                var tmp = new RamAccess<string>(PackType_Validation, PackType_DB);
                tmp.PropertyChanged += PackTypeValueChanged;
                return tmp;
            }
            set
            {
                PackType_DB = value.Value;
                OnPropertyChanged(nameof(PackType));
            }
        }//If change this change validation

        private void PackTypeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PackType_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
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
        #endregion

        #region PackNumber
        public string PackNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("номер упаковки")]
        public RamAccess<string> PackNumber
        {
            get
            {
                var tmp = new RamAccess<string>(PackNumber_Validation, PackNumber_DB);
                tmp.PropertyChanged += PackNumberValueChanged;
                return tmp;
            }
            set
            {
                PackNumber_DB = value.Value;
                OnPropertyChanged(nameof(PackNumber));
            }
        }//If change this change validation

        private void PackNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                PackNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool PackNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))//ok
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
        #endregion

        #region StoragePlaceName
        public string StoragePlaceName_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("наименование")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                var tmp = new RamAccess<string>(StoragePlaceName_Validation, StoragePlaceName_DB);
                tmp.PropertyChanged += StoragePlaceNameValueChanged;
                return tmp;
            }
            set
            {
                StoragePlaceName_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        private void StoragePlaceNameValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                StoragePlaceName_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            //var a = new List<string>();//here binds spr
            //if (a.Contains(value.Value))
            //    return true;
            //value.AddError("Недопустимое значение");
            //return false;
            return true;
        }
        #endregion

        #region StoragePlaceCode
        public string StoragePlaceCode_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("код")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                var tmp = new RamAccess<string>(StoragePlaceCode_Validation, StoragePlaceCode_DB);
                tmp.PropertyChanged += StoragePlaceCodeValueChanged;
                return tmp;
            }
            set
            {
                StoragePlaceCode_DB = value.Value;
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }//if change this change validation

        private void StoragePlaceCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                StoragePlaceCode_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            //var lst = new List<string>();//HERE binds spr
            //if (!lst.Contains(value.Value))
            //{
            //    value.AddError("Недопустимое значение");
            //    return false;
            //}
            //return true;
            if (value.Value == "-") return true;
            Regex a = new Regex("^[0-9]{8}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region Subsidy
        public string Subsidy_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Субсидия, %")]
        public RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {
                var tmp = new RamAccess<string>(Subsidy_Validation, Subsidy_DB);
                tmp.PropertyChanged += SubsidyValueChanged;
                return tmp;
            }
            set
            {
                Subsidy_DB = value.Value;
                OnPropertyChanged(nameof(Subsidy));
            }
        }
        private void SubsidyValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                Subsidy_DB = ((RamAccess<string>)Value).Value;
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
        #endregion

        #region FcpNumber
        public string FcpNumber_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public RamAccess<string> FcpNumber
        {
            get
            {
                var tmp = new RamAccess<string>(FcpNumber_Validation, FcpNumber_DB);
                tmp.PropertyChanged += FcpNumberValueChanged;
                return tmp;
            }
            set
            {
                FcpNumber_DB = value.Value;
                OnPropertyChanged(nameof(FcpNumber));
            }
        }
        private void FcpNumberValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                FcpNumber_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        #endregion

        #region RefineOrSortRAOCode
        public string RefineOrSortRAOCode_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                var tmp = new RamAccess<string>(RefineOrSortRAOCode_Validation, RefineOrSortRAOCode_DB);
                tmp.PropertyChanged += RefineOrSortRAOCodeValueChanged;
                return tmp;
            }
            set
            {
                RefineOrSortRAOCode_DB = value.Value;
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }//If change this change validation

        private void RefineOrSortRAOCodeValueChanged(object Value, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == "Value")
            {
                RefineOrSortRAOCode_DB = ((RamAccess<string>)Value).Value;
            }
        }
        private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!Spravochniks.SprRifineOrSortCodes.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        #endregion

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))//ok
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
            if (!Spravochniks.SprOpCodes.Contains((short)value.Value))
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
        #region IExcel
        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);
            worksheet.Cells[Row, 4].Value = CodeRAO_DB;
            worksheet.Cells[Row, 5].Value = StatusRAO_DB;
            worksheet.Cells[Row, 6].Value = Volume_DB;
            worksheet.Cells[Row, 7].Value = Mass_DB;
            worksheet.Cells[Row, 8].Value = QuantityOZIII_DB;
            worksheet.Cells[Row, 9].Value = MainRadionuclids_DB;
            worksheet.Cells[Row, 10].Value = TritiumActivity_DB;
            worksheet.Cells[Row, 11].Value = BetaGammaActivity_DB;
            worksheet.Cells[Row, 12].Value = AlphaActivity_DB;
            worksheet.Cells[Row, 13].Value = TransuraniumActivity_DB;
            worksheet.Cells[Row, 14].Value = ActivityMeasurementDate_DB;
            worksheet.Cells[Row, 15].Value = DocumentVid_DB;
            worksheet.Cells[Row, 16].Value = DocumentNumber_DB;
            worksheet.Cells[Row, 17].Value = DocumentDate_DB;
            worksheet.Cells[Row, 18].Value = ProviderOrRecieverOKPO_DB;
            worksheet.Cells[Row, 19].Value = TransporterOKPO_DB;
            worksheet.Cells[Row, 20].Value = StoragePlaceName_DB;
            worksheet.Cells[Row, 21].Value = StoragePlaceCode_DB;
            worksheet.Cells[Row, 22].Value = RefineOrSortRAOCode_DB;
            worksheet.Cells[Row, 23].Value = PackName_DB;
            worksheet.Cells[Row, 24].Value = PackType_DB;
            worksheet.Cells[Row, 25].Value = PackNumber_DB;
            worksheet.Cells[Row, 26].Value = Subsidy_DB;
            worksheet.Cells[Row, 27].Value = FcpNumber_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet)
        {
            Form1.ExcelHeader(worksheet);
            worksheet.Cells[1, 4].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(CodeRAO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 5].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(StatusRAO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 6].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(Volume))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 7].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(Mass))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 8].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(QuantityOZIII))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 9].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(MainRadionuclids))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 10].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(TritiumActivity))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 11].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(BetaGammaActivity))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 12].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(AlphaActivity))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 13].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(TransuraniumActivity))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 14].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(ActivityMeasurementDate))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 15].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(DocumentVid))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 16].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(DocumentNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 17].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(DocumentDate))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 18].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(ProviderOrRecieverOKPO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 19].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(TransporterOKPO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 20].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(StoragePlaceName))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 21].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(StoragePlaceCode))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 22].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(RefineOrSortRAOCode))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 23].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(PackName))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 24].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(PackType))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 25].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(PackNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 26].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(Subsidy))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[1, 27].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form16,Models").GetProperty(nameof(FcpNumber))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }
        #endregion
    }
}
