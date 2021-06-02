using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.7: Сведения о твердых кондиционированных РАО")]
    public class Form17 : Abstracts.Form1
    {
        public Form17() : base()
        {
            FormNum = "17";
            NumberOfFields = 43;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackName));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackName_Not_Valid;
                }
            }
            set
            {
                PackName_Validation(value);

                if (GetErrors(nameof(PackName)) == null)
                {
                    _dataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }

        private string _PackName_Not_Valid = "";
        private void PackName_Validation(string value)
        {
            ClearErrors(nameof(PackName));
            if ((value == null) || value.Equals("")) return;
            if (value.Equals("Неупакованные РАО")) return;
            var spr = new List<string>();
            if (!spr.Contains(value))
            {
                AddError(nameof(PackName), "Недопустимое значение");
                return;
            }
        }
        //PackName property

        //PackNameNote property
        public string PackNameNote
        {
            get
            {
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNameNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNameNote_Not_Valid;
                }
            }
            set
            {
                PackNameNote_Validation(value);
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    _dataAccess.Set(nameof(PackNameNote), value);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }

        private string _PackNameNote_Not_Valid = "";
        private void PackNameNote_Validation(string value)
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
                    var tmp = _dataAccess.Get(nameof(PackType));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackType_Not_Valid;
                }
            }
            set
            {
                PackType_Validation(value);

                if (GetErrors(nameof(PackType)) == null)
                {
                    _dataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
        private string _PackType_Not_Valid = "";
        private void PackType_Validation(string value)//Ready
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
                    var tmp = _dataAccess.Get(nameof(PackTypeRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                PackTypeRecoded_Validation(value);
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        private string _PackTypeRecoded_Not_Valid = "";
        private void PackTypeRecoded_Validation(string value)
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
                    var tmp = _dataAccess.Get(nameof(PackTypeNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackTypeNote_Not_Valid;
                }
            }
            set
            {
                PackTypeNote_Validation(value);
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeNote), value);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }

        private string _PackTypeNote_Not_Valid = "";
        private void PackTypeNote_Validation(string value)
        {
            ClearErrors(nameof(PackTypeNote));
        }
        //PackTypeNote property

        //PackNumber property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumber
        {
            get
            {
                if (GetErrors(nameof(PackNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNumber_Not_Valid;
                }
            }
            set
            {
                PackNumber_Validation(value);

                if (GetErrors(nameof(PackNumber)) == null)
                {
                    _dataAccess.Set(nameof(PackNumber), value);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation
        private string _PackNumber_Not_Valid = "";
        private void PackNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumber));
        }
        //PackNumber property

        //PackNumberNote property
        public string PackNumberNote
        {
            get
            {
                if (GetErrors(nameof(PackNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNumberNote_Not_Valid;
                }
            }
            set
            {
                PackNumberNote_Validation(value);

                if (GetErrors(nameof(PackNumberNote)) == null)
                {
                    _dataAccess.Set(nameof(PackNumberNote), value);
                }
                OnPropertyChanged(nameof(PackNumberNote));
            }
        }

        private string _PackNumberNote_Not_Valid = "";
        private void PackNumberNote_Validation(string value)
        {
            ClearErrors(nameof(PackNumberNote));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(PackNumberNote), "Поле не заполнено");
                return;
            }
        }
        //PackNumberNote property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNumberRecoded_Not_Valid;
                }
            }
            set
            {
                PackNumberRecoded_Validation(value);
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation
        private string _PackNumberRecoded_Not_Valid = "";
        private void PackNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumberRecoded));
        }
        //PackNumberRecoded property

        //PackFactoryNumber property
        [Attributes.Form_Property("Заводской номер упаковки")]
        public string PackFactoryNumber
        {
            get
            {
                if (GetErrors(nameof(PackFactoryNumber)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackFactoryNumber));
                }
                else
                {
                    return _PackFactoryNumber_Not_Valid;
                }
            }
            set
            {
                PackFactoryNumber_Validation(value);
                if (GetErrors(nameof(PackFactoryNumber)) == null)
                {
                    _dataAccess.Set(nameof(PackFactoryNumber), value);
                }
                OnPropertyChanged(nameof(PackFactoryNumber));
            }
        }

        private string _PackFactoryNumber_Not_Valid = "";
        private void PackFactoryNumber_Validation(string value)//TODO
        {
            ClearErrors(nameof(PackFactoryNumber));
        }
        //PackFactoryNumber property

        //FormingDate property
        [Attributes.Form_Property("Дата формирования")]
        public string FormingDate
        {
            get
            {
                if (GetErrors(nameof(FormingDate)) == null)
                {
                    return (string)_dataAccess.Get(nameof(FormingDate));
                }
                else
                {
                    return _FormingDate_Not_Valid;
                }
            }
            set
            {
                FormingDate_Validation(value);
                if (GetErrors(nameof(FormingDate)) == null)
                {
                    _dataAccess.Set(nameof(FormingDate), value);
                }
                OnPropertyChanged(nameof(FormingDate));
            }
        }

        private string _FormingDate_Not_Valid = "";
        private void FormingDate_Validation(string value)//TODO
        {
            ClearErrors(nameof(FormingDate));
            if (string.IsNullOrEmpty(value) || value.Equals("-")) return;
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
        //FormingDate property

        //Volume property
        [Attributes.Form_Property("Объем, куб. м")]
        public string Volume
        {
            get
            {
                if (GetErrors(nameof(Volume)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Volume));
                }
                else
                {
                    return _Volume_Not_Valid;
                }
            }
            set
            {
                Volume_Validation(value);
                if (GetErrors(nameof(Volume)) == null)
                {
                    _dataAccess.Set(nameof(Volume), value);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }

        private string _Volume_Not_Valid = "-1";
        private void Volume_Validation(string value)//TODO
        {
            ClearErrors(nameof(Volume));
            if (string.IsNullOrEmpty(value)) return;
            if (value == "-") return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(Volume), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Volume), "Число должно быть больше нуля");
            }
            catch (Exception)
            {
                AddError(nameof(Volume), "Недопустимое значение");
                return;
            }
        }
        //Volume property

        //Mass Property
        [Attributes.Form_Property("Масса брутто, т")]
        public string Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Mass));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Mass_Not_Valid;
                }
            }
            set
            {
                Mass_Validation(value);
                if (GetErrors(nameof(Mass)) == null)
                {
                    _dataAccess.Set(nameof(Mass), value);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }

        private string _Mass_Not_Valid = "";
        private void Mass_Validation(string value)//TODO
        {
            ClearErrors(nameof(Mass));
            if (string.IsNullOrEmpty(value)) return;
            if (value == "-") return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(Mass), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Mass), "Число должно быть больше нуля");
            }
            catch (Exception)
            {
                AddError(nameof(Mass), "Недопустимое значение");
            }
        }
        //Mass Property

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
        }
        //PassportNumber property

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
            if (string.IsNullOrEmpty(value))
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
            AddError(nameof(Radionuclids), "Недопустимое значение");
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
            if (string.IsNullOrEmpty(value))
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
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            if (value.Equals("прим."))
            {

            }
            if (value.Equals("Минобороны")) return;
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
            if (string.IsNullOrEmpty(value)|| value.Equals("-")) return;
            var lst = new List<string>();//HERE binds spr
            if(!lst.Contains(value))
            AddError(nameof(StoragePlaceCode), "Такого значения нет в справочнике");
        }
        //StoragePlaceCode property

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
            if ((value == null) || value.Equals("")) return;
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
            ClearErrors(nameof(CodeRAO));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(CodeRAO), "Поле не заполнено");
                return;
            }
            var a = new Regex("^[0-9]{11}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(CodeRAO), "Недопустимое значение");
                return;
            }
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
            if (value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        AddError(nameof(StatusRAO), "Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    AddError(nameof(StatusRAO), "Недопустимое значение");
                }
                return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(StatusRAO), "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    AddError(nameof(StatusRAO), "Недопустимое значение");
            }
        }
        //StatusRAO property

        //VolumeOutOfPack property
        [Attributes.Form_Property("Объем без упаковки, куб. м")]
        public string VolumeOutOfPack
        {
            get
            {
                if (GetErrors(nameof(VolumeOutOfPack)) == null)
                {
                    return (string)_dataAccess.Get(nameof(VolumeOutOfPack));
                }
                else
                {
                    return _VolumeOutOfPack_Not_Valid;
                }
            }
            set
            {
                VolumeOutOfPack_Validation(value);
                if (GetErrors(nameof(VolumeOutOfPack)) == null)
                {
                    _dataAccess.Set(nameof(VolumeOutOfPack), value);
                }
                OnPropertyChanged(nameof(VolumeOutOfPack));
            }
        }

        private string _VolumeOutOfPack_Not_Valid = "";
        private void VolumeOutOfPack_Validation(string value)//TODO
        {
            ClearErrors(nameof(VolumeOutOfPack));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(VolumeOutOfPack), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(VolumeOutOfPack), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(VolumeOutOfPack), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(VolumeOutOfPack), "Недопустимое значение");
            }
        }
        //VolumeOutOfPack property

        //MassOutOfPack Property
        [Attributes.Form_Property("Масса без упаковки, т")]
        public string MassOutOfPack
        {
            get
            {
                if (GetErrors(nameof(MassOutOfPack)) == null)
                {
                    return (string)_dataAccess.Get(nameof(MassOutOfPack));
                }
                else
                {
                    return _MassOutOfPack_Not_Valid;
                }
            }
            set
            {
                MassOutOfPack_Validation(value);
                if (GetErrors(nameof(MassOutOfPack)) == null)
                {
                    _dataAccess.Set(nameof(MassOutOfPack), value);
                }
                OnPropertyChanged(nameof(MassOutOfPack));
            }
        }

        private string _MassOutOfPack_Not_Valid = "";
        private void MassOutOfPack_Validation(string value)//TODO
        {
            ClearErrors(nameof(MassOutOfPack));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(MassOutOfPack), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')||value.Contains('E')))
            {
                AddError(nameof(MassOutOfPack), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(MassOutOfPack), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(MassOutOfPack), "Недопустимое значение");
            }
        }
        //MassOutOfPack Property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int? Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) == null)
                {
                    return (int?)_dataAccess.Get(nameof(Quantity));//OK;
                }
                else
                {
                    return _Quantity_Not_Valid;
                }
            }
            set
            {
                Quantity_Validation(value);

                if (GetErrors(nameof(Quantity)) == null)
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
        private int? _Quantity_Not_Valid = null;
        private void Quantity_Validation(int? value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value == null) return;
            if (value <= 0)
            {
                AddError(nameof(Quantity), "Недопустимое значение");
                return;
            }
        }
        //Quantity property

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
            if (value == "-") return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(TritiumActivity), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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
            if (value == "-") return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(BetaGammaActivity), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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
            if (value == "-") return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(AlphaActivity), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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
            if (value == "-") return;
            if (!(value.Contains('e') || value.Contains('E')))
            {
                AddError(nameof(TransuraniumActivity), "Недопустимое значение");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
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
            if (string.IsNullOrEmpty(value))
            {
                return;
            }
            var a = new Regex("^[0-9][0-9]$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(RefineOrSortRAOCode), "Недопустимое значение");
                return;
            }
        }
        //RefineOrSortRAOCode property

        protected override void OperationCode_Validation(short? value)//OK
        {
            ClearErrors(nameof(OperationCode));
            if (value == _OperationCode_Not_Valid)
            {
                AddError(nameof(OperationCode), "Поле не заполнено");
                return;
            }
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            if (!spr.Contains((short)value))
            {
                AddError(nameof(OperationCode), "Недопустимое значение");
                return;
            }
            bool a0 = value == 1;
            bool a1 = value == 10;
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
            if (!(a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11))
                AddError(nameof(OperationCode), "Код операции не может быть использован в форме 1.7");
            return;
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
            bool ab = (OperationCode == 51) || (OperationCode == 52);
            bool c = (OperationCode == 68);
            bool d = (OperationCode == 18) || (OperationCode == 55);
            if (ab || c || d)
                if (!value.Equals(OperationDate))
                    AddError(nameof(DocumentDate), "Заполните примечание");
        }
    }
}
