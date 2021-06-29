using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 3.2: Отчет об отправке радиоактивных источников 1 и 2 категории")]
    public class Form32 : Abstracts.Form3
    {
        public Form32() : base()
        {
            //FormNum.Value = "32";
            //NumberOfFields.Value = 17;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //UniqueAgreementid property
        [Attributes.Form_Property("Уникльный номер соглашения")]public int? UniqueAgreementidId { get; set; }
        public virtual RamAccess<string> UniqueAgreementid
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(UniqueAgreementid));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(UniqueAgreementid), value);
                }
                OnPropertyChanged(nameof(UniqueAgreementid));
            }
        }

        //UniqueAgreementid property

        //SupplyDate property
        [Attributes.Form_Property("Дата поступления")]public int? SupplyDateId { get; set; }
        public virtual RamAccess<string> SupplyDate
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(SupplyDate));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(SupplyDate), value);
                }
                OnPropertyChanged(nameof(SupplyDate));
            }
        }

        //SupplyDate property

        //RecieverName property
        [Attributes.Form_Property("Наименование получателя")]public int? RecieverNameId { get; set; }
        public virtual RamAccess<string> RecieverName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(RecieverName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(RecieverName), value);
                }
                OnPropertyChanged(nameof(RecieverName));
            }
        }

        //RecieverName property

        //FieldsOfWorking property
        [Attributes.Form_Property("Вид деятельности")]public int? FieldsOfWorkingId { get; set; }
        public virtual RamAccess<byte> FieldsOfWorking
        {
            get
            {

                {
                    return DataAccess.Get<byte>(nameof(FieldsOfWorking));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FieldsOfWorking), value);
                }
                OnPropertyChanged(nameof(FieldsOfWorking));
            }
        }

        //FieldsOfWorking property

        //LicenseidRv property
        [Attributes.Form_Property("Номер лицензии на обращение с РВ")]public int? LicenseidRvId { get; set; }
        public virtual RamAccess<string> LicenseidRv
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(LicenseidRv));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(LicenseidRv), value);
                }
                OnPropertyChanged(nameof(LicenseidRv));
            }
        }

        //LicenseidRv property

        //ValidThruRv property
        [Attributes.Form_Property("Лицензия истекает(РВ)")]public int? ValidThruRvId { get; set; }
        public virtual RamAccess<string> ValidThruRv
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ValidThruRv));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(ValidThruRv), value);
                }
                OnPropertyChanged(nameof(ValidThruRv));
            }
        }

        //ValidThruRv property

        //LicenseidRao property
        [Attributes.Form_Property("Номер лицензии на обращение с РАО")]public int? LicenseidRaoId { get; set; }
        public virtual RamAccess<string> LicenseidRao
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(LicenseidRao));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(LicenseidRao), value);
                }
                OnPropertyChanged(nameof(LicenseidRao));
            }
        }

        //LicenseidRao property

        //ValidThruRao property
        [Attributes.Form_Property("Лицензия истекает(РАО)")]public int? ValidThruRaoId { get; set; }
        public virtual RamAccess<string> ValidThruRao
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ValidThruRao));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(ValidThruRao), value);
                }
                OnPropertyChanged(nameof(ValidThruRao));
            }
        }

        //ValidThruRao property

        //SupplyAddress property
        [Attributes.Form_Property("Адрес поставки")]public int? SupplyAddressId { get; set; }
        public virtual RamAccess<string> SupplyAddress
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(SupplyAddress));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(SupplyAddress), value);
                }
                OnPropertyChanged(nameof(SupplyAddress));
            }
        }

        //SupplyAddress property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]public int? RadionuclidsId { get; set; }
        public virtual RamAccess<string> Radionuclids
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Radionuclids));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value))
                {
                    Radionuclids.Value = item.Item2;
                    return true;
                }
            }
            return false;
        }
        //Radionuclids property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]public int? QuantityId { get; set; }
        public virtual RamAccess<int> Quantity
        {
            get
            {

                {
                    return DataAccess.Get<int>(nameof(Quantity));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
        private bool Quantity_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Quantity property

        //SummaryActivity property
        [Attributes.Form_Property("Суммарная активность, Бк")]public int? SummaryActivityId { get; set; }
        public virtual RamAccess<string> SummaryActivity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(SummaryActivity));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(SummaryActivity), value);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }

        private bool SummaryActivity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e')))
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
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //SummaryActivity property

        private List<Form32_1> _zriInfo = new List<Form32_1>();
        public List<Form32_1> ZriInfo
        {
            get { return _zriInfo; }
            set
            {
                _zriInfo = value;
                OnPropertyChanged("ZriInfo");
            }
        }

        private List<Form32_2> _packInfo = new List<Form32_2>();
        public List<Form32_2> PackInfo
        {
            get { return _packInfo; }
            set
            {
                _packInfo = value;
                OnPropertyChanged("PackInfo");
            }
        }

        private List<Form32_3> _idsInfo = new List<Form32_3>();
        public List<Form32_3> IdsInfo
        {
            get { return _idsInfo; }
            set
            {
                _idsInfo = value;
                OnPropertyChanged("IdsInfo");
            }
        }
    }
}
