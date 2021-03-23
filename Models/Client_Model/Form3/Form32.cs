using System;
using System.Collections.Generic;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 3.2: Отчет об отправке радиоактивных источников 1 и 2 категории")]
    public class Form32 : Form3
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 17;

        [Attributes.FormVisual("Номер корректировки")]
        public byte CorrectionNumber
        {
            get
            {
                if (GetErrors(nameof(CorrectionNumber)) != null)
                {
                    return _correctionNumber;
                }
                else
                {
                    return _correctionNumber_Not_Valid;
                }
            }
            set
            {
                _correctionNumber_Not_Valid = value;
                if (CorrectionNumber_Validation())
                {
                    _correctionNumber = _correctionNumber_Not_Valid;
                }
                OnPropertyChanged(nameof(CorrectionNumber));
            }
        }
        private byte _correctionNumber = 255;
        private byte _correctionNumber_Not_Valid = 255;
        private bool CorrectionNumber_Validation()
        {
            return true;
            //ClearErrors(nameof(CorrectionNumber));
            ////Пример
            //if (value < 10)
            //    AddError(nameof(CorrectionNumber), "Значение должно быть больше 10.");
        }

        private DateTime _notificationDate = DateTime.MinValue;
        public DateTime NotificationDate
        {
            get { return _notificationDate; }
            set
            {
                _notificationDate = value;
                OnPropertyChanged("NotificationDate");
            }
        }

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

        private string _radionuclids = "";//If change this change validation

        private void Radionuclids_Validation(string value)//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }

        [Attributes.FormVisual("Радионуклиды")]
        public string Radionuclids
        {
            get { return _radionuclids; }
            set
            {
                _radionuclids = value;
                Radionuclids_Validation(value);
                OnPropertyChanged("Radionuclids");
            }
        }

        private int _quantity = -1;  // positive int.

        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }

        [Attributes.FormVisual("Количество, шт.")]
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                Quantity_Validation(value);
                OnPropertyChanged("Quantity");
            }
        }

        private string _summaryActivity = "";
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

        [Attributes.FormVisual("Суммарная активность, Бк")]
        public string SummaryActivity
        {
            get { return _summaryActivity; }
            set
            {
                _summaryActivity = value;
                SummaryActivity_Validation(value);
                OnPropertyChanged("SummaryActivity");
            }
        }

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
