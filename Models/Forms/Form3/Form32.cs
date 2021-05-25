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
            FormNum = "32";
            NumberOfFields = 17;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //UniqueAgreementId property
        [Attributes.Form_Property("Уникльный номер соглашения")]
        public string UniqueAgreementId
        {
            get
            {
                if (GetErrors(nameof(UniqueAgreementId)) == null)
                {
                    return (string)_dataAccess.Get(nameof(UniqueAgreementId));
                }
                else
                {
                    return _UniqueAgreementId_Not_Valid;
                }
            }
            set
            {
                _UniqueAgreementId_Not_Valid = value;
                if (GetErrors(nameof(UniqueAgreementId)) == null)
                {
                    _dataAccess.Set(nameof(UniqueAgreementId), _UniqueAgreementId_Not_Valid);
                }
                OnPropertyChanged(nameof(UniqueAgreementId));
            }
        }

        private string _UniqueAgreementId_Not_Valid = "";
        //UniqueAgreementId property

        //SupplyDate property
        [Attributes.Form_Property("Дата поступления")]
        public DateTimeOffset SupplyDate
        {
            get
            {
                if (GetErrors(nameof(SupplyDate)) == null)
                {
                    return (DateTime)_dataAccess.Get(nameof(SupplyDate));
                }
                else
                {
                    return _SupplyDate_Not_Valid;
                }
            }
            set
            {
                _SupplyDate_Not_Valid = value;
                if (GetErrors(nameof(SupplyDate)) == null)
                {
                    _dataAccess.Set(nameof(SupplyDate), _SupplyDate_Not_Valid);
                }
                OnPropertyChanged(nameof(SupplyDate));
            }
        }

        private DateTimeOffset _SupplyDate_Not_Valid = DateTimeOffset.Parse("01/01/1921");
        //SupplyDate property

        //RecieverName property
        [Attributes.Form_Property("Наименование получателя")]
        public string RecieverName
        {
            get
            {
                if (GetErrors(nameof(RecieverName)) == null)
                {
                    return (string)_dataAccess.Get(nameof(RecieverName));
                }
                else
                {
                    return _RecieverName_Not_Valid;
                }
            }
            set
            {
                _RecieverName_Not_Valid = value;
                if (GetErrors(nameof(RecieverName)) == null)
                {
                    _dataAccess.Set(nameof(RecieverName), _RecieverName_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverName));
            }
        }

        private string _RecieverName_Not_Valid = "";
        //RecieverName property

        //FieldsOfWorking property
        [Attributes.Form_Property("Вид деятельности")]
        public byte FieldsOfWorking
        {
            get
            {
                if (GetErrors(nameof(FieldsOfWorking)) == null)
                {
                    return (byte)_dataAccess.Get(nameof(FieldsOfWorking));
                }
                else
                {
                    return _FieldsOfWorking_Not_Valid;
                }
            }
            set
            {
                _FieldsOfWorking_Not_Valid = value;
                if (GetErrors(nameof(FieldsOfWorking)) == null)
                {
                    _dataAccess.Set(nameof(FieldsOfWorking), _FieldsOfWorking_Not_Valid);
                }
                OnPropertyChanged(nameof(FieldsOfWorking));
            }
        }

        private byte _FieldsOfWorking_Not_Valid = 0;
        //FieldsOfWorking property

        //LicenseIdRv property
        [Attributes.Form_Property("Номер лицензии на обращение с РВ")]
        public string LicenseIdRv
        {
            get
            {
                if (GetErrors(nameof(LicenseIdRv)) == null)
                {
                    return (string)_dataAccess.Get(nameof(LicenseIdRv));
                }
                else
                {
                    return _LicenseIdRv_Not_Valid;
                }
            }
            set
            {
                _LicenseIdRv_Not_Valid = value;
                if (GetErrors(nameof(LicenseIdRv)) == null)
                {
                    _dataAccess.Set(nameof(LicenseIdRv), _LicenseIdRv_Not_Valid);
                }
                OnPropertyChanged(nameof(LicenseIdRv));
            }
        }

        private string _LicenseIdRv_Not_Valid = "";
        //LicenseIdRv property

        //ValidThruRv property
        [Attributes.Form_Property("Лицензия истекает(РВ)")]
        public DateTimeOffset ValidThruRv
        {
            get
            {
                if (GetErrors(nameof(ValidThruRv)) == null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidThruRv));
                }
                else
                {
                    return _ValidThruRv_Not_Valid;
                }
            }
            set
            {
                _ValidThruRv_Not_Valid = value;
                if (GetErrors(nameof(ValidThruRv)) == null)
                {
                    _dataAccess.Set(nameof(ValidThruRv), _ValidThruRv_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThruRv));
            }
        }

        private DateTimeOffset _ValidThruRv_Not_Valid = DateTimeOffset.Parse("01/01/1921");
        //ValidThruRv property

        //LicenseIdRao property
        [Attributes.Form_Property("Номер лицензии на обращение с РАО")]
        public string LicenseIdRao
        {
            get
            {
                if (GetErrors(nameof(LicenseIdRao)) == null)
                {
                    return (string)_dataAccess.Get(nameof(LicenseIdRao));
                }
                else
                {
                    return _LicenseIdRao_Not_Valid;
                }
            }
            set
            {
                _LicenseIdRao_Not_Valid = value;
                if (GetErrors(nameof(LicenseIdRao)) == null)
                {
                    _dataAccess.Set(nameof(LicenseIdRao), _LicenseIdRao_Not_Valid);
                }
                OnPropertyChanged(nameof(LicenseIdRao));
            }
        }

        private string _LicenseIdRao_Not_Valid = "";
        //LicenseIdRao property

        //ValidThruRao property
        [Attributes.Form_Property("Лицензия истекает(РАО)")]
        public DateTimeOffset ValidThruRao
        {
            get
            {
                if (GetErrors(nameof(ValidThruRao)) == null)
                {
                    return (DateTime)_dataAccess.Get(nameof(ValidThruRao));
                }
                else
                {
                    return _ValidThruRao_Not_Valid;
                }
            }
            set
            {
                _ValidThruRao_Not_Valid = value;
                if (GetErrors(nameof(ValidThruRao)) == null)
                {
                    _dataAccess.Set(nameof(ValidThruRao), _ValidThruRao_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThruRao));
            }
        }

        private DateTimeOffset _ValidThruRao_Not_Valid = DateTimeOffset.Parse("01/01/1921");
        //ValidThruRao property

        //SupplyAddress property
        [Attributes.Form_Property("Адрес поставки")]
        public string SupplyAddress
        {
            get
            {
                if (GetErrors(nameof(SupplyAddress)) == null)
                {
                    return (string)_dataAccess.Get(nameof(SupplyAddress));
                }
                else
                {
                    return _SupplyAddress_Not_Valid;
                }
            }
            set
            {
                _SupplyAddress_Not_Valid = value;
                if (GetErrors(nameof(SupplyAddress)) == null)
                {
                    _dataAccess.Set(nameof(SupplyAddress), _SupplyAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(SupplyAddress));
            }
        }

        private string _SupplyAddress_Not_Valid = "";
        //SupplyAddress property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
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
                    _dataAccess.Set(nameof(Radionuclids), _Radionuclids_Not_Valid);
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

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Quantity));//OK
                    return tmp != null ? (int)tmp : -1;
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
                    _dataAccess.Set(nameof(Quantity), _Quantity_Not_Valid);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
        private int _Quantity_Not_Valid = -1;
        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }
        //Quantity property

        //SummaryActivity property
        [Attributes.Form_Property("Суммарная активность, Бк")]
        public string SummaryActivity
        {
            get
            {
                if (GetErrors(nameof(SummaryActivity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(SummaryActivity));
                }
                else
                {
                    return _SummaryActivity_Not_Valid;
                }
            }
            set
            {
                _SummaryActivity_Not_Valid = value;
                if (GetErrors(nameof(SummaryActivity)) == null)
                {
                    _dataAccess.Set(nameof(SummaryActivity), _SummaryActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }

        private string _SummaryActivity_Not_Valid = "";
        private void SummaryActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(SummaryActivity));
            if (value == null || value.Equals(""))
            {
                AddError(nameof(SummaryActivity), "Поле не заполнено");
                return;
            }
            if (!(value.Contains('e')))
            {
                AddError(nameof(SummaryActivity), "Недопустимое значение");
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
                    AddError(nameof(SummaryActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(SummaryActivity), "Недопустимое значение");
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
