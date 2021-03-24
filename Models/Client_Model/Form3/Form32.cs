using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 3.2: Отчет об отправке радиоактивных источников 1 и 2 категории")]
    public class Form32 : Form3
    {
        public override string FormNum { get { return "3.2"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 17;

        private string _uniqueAgreementId = "";
        public string UniqueAgreementId
        {
            get { return _uniqueAgreementId; }
            set
            {
                _uniqueAgreementId = value;
                OnPropertyChanged("UniqueAgreementId");
            }
        }

        private DateTime _supplyDate = DateTime.MinValue;
        public DateTime SupplyDate
        {
            get { return _supplyDate; }
            set
            {
                _supplyDate = value;
                OnPropertyChanged("SupplyDate");
            }
        }

        private string _recieverName = "";
        public string RecieverName
        {
            get { return _recieverName; }
            set
            {
                _recieverName = value;
                OnPropertyChanged("RecieverName");
            }
        }

        private byte _fieldsOfWorking = 0;
        public byte FieldsOfWorking
        {
            get { return _fieldsOfWorking; }
            set
            {
                _fieldsOfWorking = value;
                OnPropertyChanged("FieldsOfWorking");
            }
        }

        private string _licenseIdRv = "";
        public string LicenseIdRv
        {
            get { return _licenseIdRv; }
            set
            {
                _licenseIdRv = value;
                OnPropertyChanged("LicenseIdRv");
            }
        }

        private DateTime _validThruRv = DateTime.MinValue;
        public DateTime ValidThruRv
        {
            get { return _validThruRv; }
            set
            {
                _validThruRv = value;
                OnPropertyChanged("ValidThruRv");
            }
        }

        private string _licenseIdRao = "";
        public string LicenseIdRao
        {
            get { return _licenseIdRao; }
            set
            {
                _licenseIdRao = value;
                OnPropertyChanged("LicenseIdRao");
            }
        }

        private DateTime _validThruRao = DateTime.MinValue;
        public DateTime ValidThruRao
        {
            get { return _validThruRao; }
            set
            {
                _validThruRao = value;
                OnPropertyChanged("ValidThruRao");
            }
        }

        private string _supplyAddress = "";
        public string SupplyAddress
        {
            get { return _supplyAddress; }
            set
            {
                _supplyAddress = value;
                OnPropertyChanged("SupplyAddress");
            }
        }

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
