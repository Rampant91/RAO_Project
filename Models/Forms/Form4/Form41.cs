using Models.DataAccess;
using System;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 4.1: Перечень организаций, зарегистрированных в СГУК РВ и РАО на региональном уровне")]
    public class Form41 : Abstracts.Form
    {
        public Form41() : base()
        {
            FormNum.Value = "41";
            NumberOfFields.Value = 10;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public RamAccess<int> NumberInOrder
        {
            get
            {

                {
                    return DataAccess.Get<int>(nameof(NumberInOrder));

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(NumberInOrder), value);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }


        private bool NumberInOrder_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //NumberInOrder property

        //RegNo property
        [Attributes.Form_Property("Регистрационный номер")]
        public RamAccess<string> RegNo
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(RegNo));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(RegNo), value);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }


        //RegNo property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public RamAccess<string> Okpo
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Okpo));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Okpo), value);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }

        private bool Okpo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Okpo property

        //OrgName property
        [Attributes.Form_Property("Наименование организации")]
        public RamAccess<string> OrgName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(OrgName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(OrgName), value);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }


        //OrgName property

        //LicenseInfo property
        [Attributes.Form_Property("Сведения о лицензии")]
        public RamAccess<string> LicenseInfo
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(LicenseInfo));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(LicenseInfo), value);
                }
                OnPropertyChanged(nameof(LicenseInfo));
            }
        }


        //LicenseInfo property

        //QuantityOfFormsInv property
        [Attributes.Form_Property("Количество отчетных форм по инвентаризации, шт.")]
        public RamAccess<int> QuantityOfFormsInv
        {
            get
            {

                {
                    return DataAccess.Get<int>(nameof(QuantityOfFormsInv));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityOfFormsInv), value);
                }
                OnPropertyChanged(nameof(QuantityOfFormsInv));
            }
        }
        // positive int.

        private bool QuantityOfFormsInv_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //QuantityOfFormsInv property

        //QuantityOfFormsOper property
        [Attributes.Form_Property("Количество форм оперативных отчетов, шт.")]
        public RamAccess<int> QuantityOfFormsOper
        {
            get
            {

                {
                    return DataAccess.Get<int>(nameof(QuantityOfFormsOper));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityOfFormsOper), value);
                }
                OnPropertyChanged(nameof(QuantityOfFormsOper));
            }
        }
        // positive int.

        private bool QuantityOfFormsOper_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //QuantityOfFormsOper property

        //QuantityOfFormsYear property
        [Attributes.Form_Property("Количество форм годовых отчетов, шт.")]
        public RamAccess<int> QuantityOfFormsYear
        {
            get
            {

                {
                    return DataAccess.Get<int>(nameof(QuantityOfFormsYear));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(QuantityOfFormsYear), value);
                }
                OnPropertyChanged(nameof(QuantityOfFormsYear));
            }
        }
        // positive int.

        private bool QuantityOfFormsYear_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //QuantityOfFormsYear property

        //Notes property
        [Attributes.Form_Property("Примечания")]
        public RamAccess<string> Notes
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Notes));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Notes), value);
                }
                OnPropertyChanged(nameof(Notes));
            }
        }


        //Notes property
    }
}
