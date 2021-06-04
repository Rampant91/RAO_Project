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
            FormNum.Value = "32";
            NumberOfFields.Value = 17;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //UniqueAgreementId property
        [Attributes.Form_Property("Уникльный номер соглашения")]
        public RamAccess<string> UniqueAgreementId
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(UniqueAgreementId));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(UniqueAgreementId), value);
                }
                OnPropertyChanged(nameof(UniqueAgreementId));
            }
        }

                //UniqueAgreementId property

        //SupplyDate property
        [Attributes.Form_Property("Дата поступления")]
        public RamAccess<string> SupplyDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SupplyDate));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SupplyDate), value);
                }
                OnPropertyChanged(nameof(SupplyDate));
            }
        }

                //SupplyDate property

        //RecieverName property
        [Attributes.Form_Property("Наименование получателя")]
        public RamAccess<string> RecieverName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RecieverName));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RecieverName), value);
                }
                OnPropertyChanged(nameof(RecieverName));
            }
        }

                //RecieverName property

        //FieldsOfWorking property
        [Attributes.Form_Property("Вид деятельности")]
        public RamAccess<byte> FieldsOfWorking
        {
            get
            {
                
                {
                    return _dataAccess.Get<byte>(nameof(FieldsOfWorking));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FieldsOfWorking), value);
                }
                OnPropertyChanged(nameof(FieldsOfWorking));
            }
        }

                //FieldsOfWorking property

        //LicenseIdRv property
        [Attributes.Form_Property("Номер лицензии на обращение с РВ")]
        public RamAccess<string> LicenseIdRv
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(LicenseIdRv));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(LicenseIdRv), value);
                }
                OnPropertyChanged(nameof(LicenseIdRv));
            }
        }

                //LicenseIdRv property

        //ValidThruRv property
        [Attributes.Form_Property("Лицензия истекает(РВ)")]
        public RamAccess<string> ValidThruRv
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ValidThruRv));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ValidThruRv), value);
                }
                OnPropertyChanged(nameof(ValidThruRv));
            }
        }

                //ValidThruRv property

        //LicenseIdRao property
        [Attributes.Form_Property("Номер лицензии на обращение с РАО")]
        public RamAccess<string> LicenseIdRao
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(LicenseIdRao));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(LicenseIdRao), value);
                }
                OnPropertyChanged(nameof(LicenseIdRao));
            }
        }

                //LicenseIdRao property

        //ValidThruRao property
        [Attributes.Form_Property("Лицензия истекает(РАО)")]
        public RamAccess<string> ValidThruRao
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ValidThruRao));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ValidThruRao), value);
                }
                OnPropertyChanged(nameof(ValidThruRao));
            }
        }

                //ValidThruRao property

        //SupplyAddress property
        [Attributes.Form_Property("Адрес поставки")]
        public RamAccess<string> SupplyAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SupplyAddress));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SupplyAddress), value);
                }
                OnPropertyChanged(nameof(SupplyAddress));
            }
        }

                //SupplyAddress property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Radionuclids));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Radionuclids_Validation(value);

                
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
                private void Radionuclids_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item2.Equals(value))
                {
                    Radionuclids.Value =item.Item2;
                    return;
                }
            }
        }
        //Radionuclids property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public RamAccess<int> Quantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(Quantity));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Quantity_Validation(value);

                
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
                private void Quantity_Validation(IDataAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //Quantity property

        //SummaryActivity property
        [Attributes.Form_Property("Суммарная активность, Бк")]
        public RamAccess<string> SummaryActivity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SummaryActivity));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SummaryActivity), value);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }

                private void SummaryActivity_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e')))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            string tmp=value.Value;
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
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
