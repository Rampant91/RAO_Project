using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.8: Сведения о жидких кондиционированных РАО")]
    public class Form18 : Abstracts.Form1
    {
        public Form18() : base()
        {
            FormNum = "18";
            NumberOfFields = 37;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //IndividualNumberZHRO property
        [Attributes.Form_Property("Индивидуальный номер ЖРО")]
        public string IndividualNumberZHRO
        {
            get
            {
                if (GetErrors(nameof(IndividualNumberZHRO)) == null)
                {
                    return (string)_dataAccess.Get(nameof(IndividualNumberZHRO));
                }
                else
                {
                    return _IndividualNumberZHRO_Not_Valid;
                }
            }
            set
            {
                IndividualNumberZHRO_Validation(value);
                if (GetErrors(nameof(IndividualNumberZHRO)) == null)
                {
                    _dataAccess.Set(nameof(IndividualNumberZHRO), value);
                }
                OnPropertyChanged(nameof(IndividualNumberZHRO));
            }
        }

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
                if (GetErrors(nameof(IndividualNumberZHROrecoded)) == null)
                {
                    return (string)_dataAccess.Get(nameof(IndividualNumberZHROrecoded));
                }
                else
                {
                    return _IndividualNumberZHROrecoded_Not_Valid;
                }
            }
            set
            {
                IndividualNumberZHROrecoded_Validation(value);
                if (GetErrors(nameof(IndividualNumberZHROrecoded)) == null)
                {
                    _dataAccess.Set(nameof(IndividualNumberZHROrecoded), value);
                }
                OnPropertyChanged(nameof(IndividualNumberZHROrecoded));
            }
        }

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
                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumber_Not_Valid;
                }
            }
            set
            {
                PassportNumber_Validation(value);

                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumber), value);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }

        private string _PassportNumber_Not_Valid = "";
        private void PassportNumber_Validation(string value)
        {
            ClearErrors(nameof(PassportNumber));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(PassportNumber), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((PassportNumberNote == null) || (PassportNumberNote == ""))
                    AddError(nameof(PassportNumberNote), "Поле не может быть пустым");
            }
        }
        //PassportNumber property

        //PassportNumberNote property
        public string PassportNumberNote
        {
            get
            {
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumberNote_Not_Valid;
                }
            }
            set
            {
                PassportNumberNote_Validation(value);
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumberNote), value);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }

        private string _PassportNumberNote_Not_Valid = "";
        private void PassportNumberNote_Validation(string value)
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
                if (GetErrors(nameof(PassportNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumberRecoded_Not_Valid;
                }
            }
            set
            {
                PassportNumberRecoded_Validation(value);
                if (GetErrors(nameof(PassportNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation
        private string _PassportNumberRecoded_Not_Valid = "";
        private void PassportNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PassportNumberRecoded));
        }
        //PassportNumberRecoded property

        //Volume6 property
        [Attributes.Form_Property("Объем, куб. м")]
        public string Volume6
        {
            get
            {
                if (GetErrors(nameof(Volume6)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Volume6));
                }
                else
                {
                    return _Volume6_Not_Valid;
                }
            }
            set
            {
                Volume6_Validation(value);
                if (GetErrors(nameof(Volume6)) == null)
                {
                    _dataAccess.Set(nameof(Volume6), value);
                }
                OnPropertyChanged(nameof(Volume6));
            }
        }

        private string _Volume6_Not_Valid = "";
        private void Volume6_Validation(string value)//TODO
        {
            ClearErrors(nameof(Volume6));
            if (string.IsNullOrEmpty(value)) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(Volume6), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Volume6), "Число должно быть больше нуля");
            }
            catch (Exception)
            {
                AddError(nameof(Volume6), "Недопустимое значение");
                return;
            }
        }
        //Volume6 property

        //Mass7 Property
        [Attributes.Form_Property("Масса, т")]
        public string Mass7
        {
            get
            {
                if (GetErrors(nameof(Mass7)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Mass7));
                }
                else
                {
                    return _Mass7_Not_Valid;
                }
            }
            set
            {
                Mass7_Validation(value);
                if (GetErrors(nameof(Mass7)) == null)
                {
                    _dataAccess.Set(nameof(Mass7), value);
                }
                OnPropertyChanged(nameof(Mass7));
            }
        }

        private string _Mass7_Not_Valid = "";
        private void Mass7_Validation(string value)//TODO
        {
            ClearErrors(nameof(Mass7));
            if (string.IsNullOrEmpty(value)) return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(Mass7), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Mass7), "Число должно быть больше нуля");
            }
            catch (Exception)
            {
                AddError(nameof(Mass7), "Недопустимое значение");
                return;
            }
        }
        //Mass7 Property

        //SaltConcentration property
        [Attributes.Form_Property("Солесодержание, г/л")]
        public double? SaltConcentration
        {
            get
            {
                if (GetErrors(nameof(SaltConcentration)) == null)
                {
                    return (double?)_dataAccess.Get(nameof(SaltConcentration));
                }
                else
                {
                    return _SaltConcentration_Not_Valid;
                }
            }
            set
            {
                SaltConcentration_Validation(value);
                if (GetErrors(nameof(SaltConcentration)) == null)
                {
                    _dataAccess.Set(nameof(SaltConcentration), value);
                }
                OnPropertyChanged(nameof(SaltConcentration));
            }
        }

        private double? _SaltConcentration_Not_Valid = null;
        private void SaltConcentration_Validation(double? value)
        {
            ClearErrors(nameof(SaltConcentration));
            if (value==null) return;
            if (value <= 0)
            {
                AddError(nameof(SaltConcentration), "Недопустимое значение");
                return;
            }
        }
        //SaltConcentration property

        //Radionuclids property
        [Attributes.Form_Property("Наименования радионуклидов")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Radionuclids));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                Radionuclids_Validation(value);

                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation(string value)//TODO
        {
            ClearErrors(nameof(Radionuclids));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(Radionuclids), "Поле не заполнено");
                return;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value))
                {
                    Radionuclids = item.Item2;
                    return;
                }
            }
        }
        //Radionuclids property

        //SpecificActivity property
        [Attributes.Form_Property("Удельная активность, Бк/г")]
        public string SpecificActivity
        {
            get
            {
                if (GetErrors(nameof(SpecificActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(SpecificActivity));
                }
                else
                {
                    return _SpecificActivity_Not_Valid;
                }
            }
            set
            {
                SpecificActivity_Validation(value);
                if (GetErrors(nameof(SpecificActivity)) == null)
                {
                    _dataAccess.Set(nameof(SpecificActivity), value);
                }
                OnPropertyChanged(nameof(SpecificActivity));
            }
        }

        private string _SpecificActivity_Not_Valid = "";
        private void SpecificActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(SpecificActivity));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(SpecificActivity), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')||value.Contains('E')))
            {
                AddError(nameof(SpecificActivity), "Недопустимое значение");
                return;
            }
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
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ProviderOrRecieverOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _ProviderOrRecieverOKPO_Not_Valid;
                }
            }
            set
            {
                ProviderOrRecieverOKPO_Validation(value);

                if (GetErrors(nameof(ProviderOrRecieverOKPO)) == null)
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }

        private string _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(ProviderOrRecieverOKPO));
            if ((value == null) || value.Equals(_ProviderOrRecieverOKPO_Not_Valid))
            {
                return;
            }
            if (value.Equals("Минобороны") || value.Equals("прим.")) return;
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(ProviderOrRecieverOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    AddError(nameof(ProviderOrRecieverOKPO), "Недопустимое значение");
            }
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public string ProviderOrRecieverOKPONote
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ProviderOrRecieverOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _ProviderOrRecieverOKPONote_Not_Valid;
                }
            }
            set
            {
                ProviderOrRecieverOKPONote_Validation(value);
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }

        private string _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation(string value)
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
                if (GetErrors(nameof(TransporterOKPO)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(TransporterOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _TransporterOKPO_Not_Valid;
                }
            }
            set
            {
                TransporterOKPO_Validation(value);

                if (GetErrors(nameof(TransporterOKPO)) == null)
                {
                    _dataAccess.Set(nameof(TransporterOKPO), value);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }

        private string _TransporterOKPO_Not_Valid = "";
        private void TransporterOKPO_Validation(string value)//Done
        {
            ClearErrors(nameof(TransporterOKPO));
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (value.Equals("-")) return;
            if (value.Equals("Минобороны")) return;
            if (value.Equals("прим."))
            {
                if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                    AddError(nameof(TransporterOKPONote), "Заполните примечание");
                return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(TransporterOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    AddError(nameof(TransporterOKPO), "Недопустимое значение");
            }
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public string TransporterOKPONote
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(TransporterOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _TransporterOKPONote_Not_Valid;
                }
            }
            set
            {
                TransporterOKPONote_Validation(value);
                if (GetErrors(nameof(TransporterOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(TransporterOKPONote), value);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }

        private string _TransporterOKPONote_Not_Valid = "";
        private void TransporterOKPONote_Validation(string value)
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
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceName));
                }
                else
                {
                    return _StoragePlaceName_Not_Valid;
                }
            }
            set
            {
                StoragePlaceName_Validation(value);
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceName), value);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation
        private string _StoragePlaceName_Not_Valid = "";
        private void StoragePlaceName_Validation(string value)//Ready
        {
            ClearErrors(nameof(StoragePlaceName));
            ClearErrors(nameof(StoragePlaceName));
            if (string.IsNullOrEmpty(value)) return;
            var spr = new List<string>();
            if (!spr.Contains(value))
            {
                AddError(nameof(StoragePlaceName), "Недопустимое значение");
                return;
            }
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public string StoragePlaceNameNote
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(StoragePlaceNameNote));
                }
                else
                {
                    return _StoragePlaceNameNote_Not_Valid;
                }
            }
            set
            {
                StoragePlaceNameNote_Validation(value);
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), value);
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
                    return (string)_dataAccess.Get(nameof(StoragePlaceCode));
                }
                else
                {
                    return _StoragePlaceCode_Not_Valid;
                }
            }
            set
            {
                StoragePlaceCode_Validation(value);
                if (GetErrors(nameof(StoragePlaceCode)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), value);
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
            //ClearErrors(nameof(StoragePlaceCode));
            //if (!(value == "-"))
            //    if (value.Length != 8)
            //        AddError(nameof(StoragePlaceCode), "Недопустимое значение");
            //    else
            //        for (int i = 0; i < 8; i++)
            //        {
            //            if (!((value[i] >= '0') && (value[i] <= '9')))
            //            {
            //                AddError(nameof(StoragePlaceCode), "Недопустимое значение");
            //                return;
            //            }
            //        }
        }
        //StoragePlaceCode property

        //CodeRAO property
        [Attributes.Form_Property("Код РАО")]
        public string CodeRAO
        {
            get
            {
                if (GetErrors(nameof(CodeRAO)) == null)
                {
                    return (string)_dataAccess.Get(nameof(CodeRAO));
                }
                else
                {
                    return _CodeRAO_Not_Valid;
                }
            }
            set
            {
                CodeRAO_Validation(value);
                if (GetErrors(nameof(CodeRAO)) == null)
                {
                    _dataAccess.Set(nameof(CodeRAO), value);
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
                    return (string)_dataAccess.Get(nameof(StatusRAO));
                }
                else
                {
                    return _StatusRAO_Not_Valid;
                }
            }
            set
            {
                StatusRAO_Validation(value);
                if (GetErrors(nameof(StatusRAO)) == null)
                {
                    _dataAccess.Set(nameof(StatusRAO), value);
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

        //Volume20 property
        [Attributes.Form_Property("Объем, куб. м")]
        public double Volume20
        {
            get
            {
                if (GetErrors(nameof(Volume20)) == null)
                {
                    return (double)_dataAccess.Get(nameof(Volume20));
                }
                else
                {
                    return _Volume20_Not_Valid;
                }
            }
            set
            {
                Volume20_Validation(value);
                if (GetErrors(nameof(Volume20)) == null)
                {
                    _dataAccess.Set(nameof(Volume20), value);
                }
                OnPropertyChanged(nameof(Volume20));
            }
        }

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
                if (GetErrors(nameof(Mass21)) == null)
                {
                    return (double)_dataAccess.Get(nameof(Mass21));
                }
                else
                {
                    return _Mass21_Not_Valid;
                }
            }
            set
            {
                Mass21_Validation(value);
                if (GetErrors(nameof(Mass21)) == null)
                {
                    _dataAccess.Set(nameof(Mass21), value);
                }
                OnPropertyChanged(nameof(Mass21));
            }
        }

        private double _Mass21_Not_Valid = -1;
        private void Mass21_Validation(double value)//TODO
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
                if (GetErrors(nameof(TritiumActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(TritiumActivity));
                }
                else
                {
                    return _TritiumActivity_Not_Valid;
                }
            }
            set
            {
                TritiumActivity_Validation(value);
                if (GetErrors(nameof(TritiumActivity)) == null)
                {
                    _dataAccess.Set(nameof(TritiumActivity), value);
                }
                OnPropertyChanged(nameof(TritiumActivity));
            }
        }

        private string _TritiumActivity_Not_Valid = "";
        private void TritiumActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(TritiumActivity));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(TritiumActivity), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')))
            {
                AddError(nameof(TritiumActivity), "Недопустимое значение");
                return;
            }
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
                    return (string)_dataAccess.Get(nameof(BetaGammaActivity));
                }
                else
                {
                    return _BetaGammaActivity_Not_Valid;
                }
            }
            set
            {
                BetaGammaActivity_Validation(value);
                if (GetErrors(nameof(BetaGammaActivity)) == null)
                {
                    _dataAccess.Set(nameof(BetaGammaActivity), value);
                }
                OnPropertyChanged(nameof(BetaGammaActivity));
            }
        }

        private string _BetaGammaActivity_Not_Valid = "";
        private void BetaGammaActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(BetaGammaActivity));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(BetaGammaActivity), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')))
            {
                AddError(nameof(BetaGammaActivity), "Недопустимое значение");
                return;
            }
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
                    return (string)_dataAccess.Get(nameof(AlphaActivity));
                }
                else
                {
                    return _AlphaActivity_Not_Valid;
                }
            }
            set
            {
                AlphaActivity_Validation(value);
                if (GetErrors(nameof(AlphaActivity)) == null)
                {
                    _dataAccess.Set(nameof(AlphaActivity), value);
                }
                OnPropertyChanged(nameof(AlphaActivity));
            }
        }

        private string _AlphaActivity_Not_Valid = "";
        private void AlphaActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(AlphaActivity));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(AlphaActivity), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')))
            {
                AddError(nameof(AlphaActivity), "Недопустимое значение");
                return;
            }
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
                    return (string)_dataAccess.Get(nameof(TransuraniumActivity));
                }
                else
                {
                    return _TransuraniumActivity_Not_Valid;
                }
            }
            set
            {
                TransuraniumActivity_Validation(value);
                if (GetErrors(nameof(TransuraniumActivity)) == null)
                {
                    _dataAccess.Set(nameof(TransuraniumActivity), value);
                }
                OnPropertyChanged(nameof(TransuraniumActivity));
            }
        }

        private string _TransuraniumActivity_Not_Valid = "";
        private void TransuraniumActivity_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransuraniumActivity));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(TransuraniumActivity), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')))
            {
                AddError(nameof(TransuraniumActivity), "Недопустимое значение");
                return;
            }
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
                if (GetErrors(nameof(RefineOrSortRAOCode)) == null)
                {
                    return (string)_dataAccess.Get(nameof(RefineOrSortRAOCode));
                }
                else
                {
                    return _RefineOrSortRAOCode_Not_Valid;
                }
            }
            set
            {
                RefineOrSortRAOCode_Validation(value);
                if (GetErrors(nameof(RefineOrSortRAOCode)) == null)
                {
                    _dataAccess.Set(nameof(RefineOrSortRAOCode), value);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        //If change this change validation
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
                if (GetErrors(nameof(Subsidy)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Subsidy));
                }
                else
                {
                    return _Subsidy_Not_Valid;
                }
            }
            set
            {
                Subsidy_Validation(value);
                if (GetErrors(nameof(Subsidy)) == null)
                {
                    _dataAccess.Set(nameof(Subsidy), value);
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
                    return (string)_dataAccess.Get(nameof(FcpNumber));
                }
                else
                {
                    return _FcpNumber_Not_Valid;
                }
            }
            set
            {
                FcpNumber_Validation(value);
                if (GetErrors(nameof(FcpNumber)) == null)
                {
                    _dataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }

        private string _FcpNumber_Not_Valid = "";
        private void FcpNumber_Validation(string value)//TODO
        {
            ClearErrors(nameof(FcpNumber));
        }
        //FcpNumber property

        protected override void OperationCode_Validation(short? value)//OK
        {
            ClearErrors(nameof(OperationCode));
            if (value == null) return;
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            if (!spr.Contains((short)value))
            {
                AddError(nameof(OperationCode), "Недопустимое значение");
                return;
            }
            bool a0 = value == 1;
            bool a2 = value == 18;
            bool a3 = value == 55;
            bool a4 = value == 63;
            bool a5 = value == 64;
            bool a6 = value == 68;
            bool a7 = value == 97;
            bool a8 = value == 98;
            bool a9 = value == 99;
            bool a10 = (value >= 21) && (value <= 29);
            bool a11 = (value >= 31) && (value <= 39);
            if (!(a0 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11))
                AddError(nameof(OperationCode), "Код операции не может быть использован в форме 1.7");
            return;
        }

        protected override void OperationDate_Validation(string value)
        {
            ClearErrors(nameof(OperationDate));
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(OperationDate), "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value); }
            catch (Exception)
            {
                AddError(nameof(OperationDate), "Недопустимое значение");
                return;
            }
        }

        protected override void DocumentNumber_Validation(string value)
        {
            ClearErrors(nameof(DocumentNumber));
        }

        protected override void DocumentVid_Validation(byte? value)
        {
            ClearErrors(nameof(DocumentVid));
            List<Tuple<byte?, string>> spr = new List<Tuple<byte?, string>>
            {
                new Tuple<byte?, string>(0,""),
                new Tuple<byte?, string>(1,""),
                new Tuple<byte?, string>(2,""),
                new Tuple<byte?, string>(3,""),
                new Tuple<byte?, string>(4,""),
                new Tuple<byte?, string>(5,""),
                new Tuple<byte?, string>(6,""),
                new Tuple<byte?, string>(7,""),
                new Tuple<byte?, string>(8,""),
                new Tuple<byte?, string>(9,""),
                new Tuple<byte?, string>(10,""),
                new Tuple<byte?, string>(11,""),
                new Tuple<byte?, string>(12,""),
                new Tuple<byte?, string>(13,""),
                new Tuple<byte?, string>(14,""),
                new Tuple<byte?, string>(15,""),
                new Tuple<byte?, string>(19,""),
                new Tuple<byte?, string>(null,"")
            };   //HERE BINDS SPRAVOCHNICK
            foreach (var item in spr)
            {
                if (item.Item1 == value) return;
            }
            AddError(nameof(DocumentVid), "Недопустимое значение");
        }

        protected override void DocumentDate_Validation(string value)
        {
            ClearErrors(nameof(DocumentDate));
            if ((value == null) || value.Equals(""))
            {
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(DocumentDate), "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value); }
            catch (Exception)
            {
                AddError(nameof(DocumentDate), "Недопустимое значение");
                return;
            }
            bool b = (OperationCode == 68);
            bool c = (OperationCode == 52) || (OperationCode == 55);
            bool d = (OperationCode == 18) || (OperationCode == 51);
            if (b || c || d)
                if (!value.Equals(OperationDate))
                    AddError(nameof(DocumentDate), "Заполните примечание");
        }
    }
}
