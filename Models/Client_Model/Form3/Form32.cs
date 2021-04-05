using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 3.2: Отчет об отправке радиоактивных источников 1 и 2 категории")]
    public class Form32 : Form3
    {
        public Form32() : base()
        {
        }

        [Attributes.FormVisual("Форма")]
        public override string FormNum { get { return "32"; } }
        public override int NumberOfFields { get; } = 17;
        public override void Object_Validation()
        {

        }

        //UniqueAgreementId property
        [Attributes.FormVisual("Уникльный номер соглашения")]
        public string UniqueAgreementId
        {
            get
            {
                if (GetErrors(nameof(UniqueAgreementId)) != null)
                {
                    return (string)_UniqueAgreementId.Get();
                }
                else
                {
                    return _UniqueAgreementId_Not_Valid;
                }
            }
            set
            {
                _UniqueAgreementId_Not_Valid = value;
                if (GetErrors(nameof(UniqueAgreementId)) != null)
                {
                    _UniqueAgreementId.Set(_UniqueAgreementId_Not_Valid);
                }
                OnPropertyChanged(nameof(UniqueAgreementId));
            }
        }
        private IDataLoadEngine _UniqueAgreementId;
        private string _UniqueAgreementId_Not_Valid = "";
        //UniqueAgreementId property

        //SupplyDate property
        [Attributes.FormVisual("Дата поступления")]
        public DateTime SupplyDate
        {
            get
            {
                if (GetErrors(nameof(SupplyDate)) != null)
                {
                    return (DateTime)_SupplyDate.Get();
                }
                else
                {
                    return _SupplyDate_Not_Valid;
                }
            }
            set
            {
                _SupplyDate_Not_Valid = value;
                if (GetErrors(nameof(SupplyDate)) != null)
                {
                    _SupplyDate.Set(_SupplyDate_Not_Valid);
                }
                OnPropertyChanged(nameof(SupplyDate));
            }
        }
        private IDataLoadEngine _SupplyDate;
        private DateTime _SupplyDate_Not_Valid = DateTime.MinValue;
        //SupplyDate property

        //RecieverName property
        [Attributes.FormVisual("Наименование получателя")]
        public string RecieverName
        {
            get
            {
                if (GetErrors(nameof(RecieverName)) != null)
                {
                    return (string)_RecieverName.Get();
                }
                else
                {
                    return _RecieverName_Not_Valid;
                }
            }
            set
            {
                _RecieverName_Not_Valid = value;
                if (GetErrors(nameof(RecieverName)) != null)
                {
                    _RecieverName.Set(_RecieverName_Not_Valid);
                }
                OnPropertyChanged(nameof(RecieverName));
            }
        }
        private IDataLoadEngine _RecieverName;
        private string _RecieverName_Not_Valid = "";
        //RecieverName property

        //FieldsOfWorking property
        [Attributes.FormVisual("Вид деятельности")]
        public byte FieldsOfWorking
        {
            get
            {
                if (GetErrors(nameof(FieldsOfWorking)) != null)
                {
                    return (byte)_FieldsOfWorking.Get();
                }
                else
                {
                    return _FieldsOfWorking_Not_Valid;
                }
            }
            set
            {
                _FieldsOfWorking_Not_Valid = value;
                if (GetErrors(nameof(FieldsOfWorking)) != null)
                {
                    _FieldsOfWorking.Set(_FieldsOfWorking_Not_Valid);
                }
                OnPropertyChanged(nameof(FieldsOfWorking));
            }
        }
        private IDataLoadEngine _FieldsOfWorking;
        private byte _FieldsOfWorking_Not_Valid = 0;
        //FieldsOfWorking property

        //LicenseIdRv property
        [Attributes.FormVisual("Номер лицензии на обращение с РВ")]
        public string LicenseIdRv
        {
            get
            {
                if (GetErrors(nameof(LicenseIdRv)) != null)
                {
                    return (string)_LicenseIdRv.Get();
                }
                else
                {
                    return _LicenseIdRv_Not_Valid;
                }
            }
            set
            {
                _LicenseIdRv_Not_Valid = value;
                if (GetErrors(nameof(LicenseIdRv)) != null)
                {
                    _LicenseIdRv.Set(_LicenseIdRv_Not_Valid);
                }
                OnPropertyChanged(nameof(LicenseIdRv));
            }
        }
        private IDataLoadEngine _LicenseIdRv;
        private string _LicenseIdRv_Not_Valid = "";
        //LicenseIdRv property

        //ValidThruRv property
        [Attributes.FormVisual("Лицензия истекает(РВ)")]
        public DateTime ValidThruRv
        {
            get
            {
                if (GetErrors(nameof(ValidThruRv)) != null)
                {
                    return (DateTime)_ValidThruRv.Get();
                }
                else
                {
                    return _ValidThruRv_Not_Valid;
                }
            }
            set
            {
                _ValidThruRv_Not_Valid = value;
                if (GetErrors(nameof(ValidThruRv)) != null)
                {
                    _ValidThruRv.Set(_ValidThruRv_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThruRv));
            }
        }
        private IDataLoadEngine _ValidThruRv;
        private DateTime _ValidThruRv_Not_Valid = DateTime.MinValue;
        //ValidThruRv property

        //LicenseIdRao property
        [Attributes.FormVisual("Номер лицензии на обращение с РАО")]
        public string LicenseIdRao
        {
            get
            {
                if (GetErrors(nameof(LicenseIdRao)) != null)
                {
                    return (string)_LicenseIdRao.Get();
                }
                else
                {
                    return _LicenseIdRao_Not_Valid;
                }
            }
            set
            {
                _LicenseIdRao_Not_Valid = value;
                if (GetErrors(nameof(LicenseIdRao)) != null)
                {
                    _LicenseIdRao.Set(_LicenseIdRao_Not_Valid);
                }
                OnPropertyChanged(nameof(LicenseIdRao));
            }
        }
        private IDataLoadEngine _LicenseIdRao;
        private string _LicenseIdRao_Not_Valid = "";
        //LicenseIdRao property

        //ValidThruRao property
        [Attributes.FormVisual("Лицензия истекает(РАО)")]
        public DateTime ValidThruRao
        {
            get
            {
                if (GetErrors(nameof(ValidThruRao)) != null)
                {
                    return (DateTime)_ValidThruRao.Get();
                }
                else
                {
                    return _ValidThruRao_Not_Valid;
                }
            }
            set
            {
                _ValidThruRao_Not_Valid = value;
                if (GetErrors(nameof(ValidThruRao)) != null)
                {
                    _ValidThruRao.Set(_ValidThruRao_Not_Valid);
                }
                OnPropertyChanged(nameof(ValidThruRao));
            }
        }
        private IDataLoadEngine _ValidThruRao;
        private DateTime _ValidThruRao_Not_Valid = DateTime.MinValue;
        //ValidThruRao property

        //SupplyAddress property
        [Attributes.FormVisual("Адрес поставки")]
        public string SupplyAddress
        {
            get
            {
                if (GetErrors(nameof(SupplyAddress)) != null)
                {
                    return (string)_SupplyAddress.Get();
                }
                else
                {
                    return _SupplyAddress_Not_Valid;
                }
            }
            set
            {
                _SupplyAddress_Not_Valid = value;
                if (GetErrors(nameof(SupplyAddress)) != null)
                {
                    _SupplyAddress.Set(_SupplyAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(SupplyAddress));
            }
        }
        private IDataLoadEngine _SupplyAddress;
        private string _SupplyAddress_Not_Valid = "";
        //SupplyAddress property

        //Radionuclids property
        [Attributes.FormVisual("Радионуклиды")]
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

        //SummaryActivity property
        [Attributes.FormVisual("Суммарная активность, Бк")]
        public string SummaryActivity
        {
            get
            {
                if (GetErrors(nameof(SummaryActivity)) != null)
                {
                    return (string)_SummaryActivity.Get();
                }
                else
                {
                    return _SummaryActivity_Not_Valid;
                }
            }
            set
            {
                _SummaryActivity_Not_Valid = value;
                if (GetErrors(nameof(SummaryActivity)) != null)
                {
                    _SummaryActivity.Set(_SummaryActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }
        private IDataLoadEngine _SummaryActivity;
        private string _SummaryActivity_Not_Valid = "";
        private void SummaryActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(SummaryActivity));
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
