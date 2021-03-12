using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 4.1: Перечень организаций, зарегистрированных в СГУК РВ и РАО на региональном уровне")]
    public class Form41 : Form4
    {
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 9;

        private int _numberInOrder = -1;
        [Attributes.FormVisual("№ п/п")]
        public int NumberInOrder
        {
            get { return _numberInOrder; }
            set
            {
                _numberInOrder = value;
                OnPropertyChanged("NumberInOrder");
            }
        }

        private string _regNo = "";
        [Attributes.FormVisual("Регистрационный номер")]
        public string RegNo
        {
            get { return _regNo; }
            set
            {
                _regNo = value;
                OnPropertyChanged("RegNo");
            }
        }

        private string _okpo = "";
        [Attributes.FormVisual("ОКПО")]
        public string Okpo
        {
            get { return _okpo; }
            set
            {
                _okpo = value;
                OnPropertyChanged("Okpo");
            }
        }

        private string _orgName = "";
        [Attributes.FormVisual("Наименование организации")]
        public string OrgName
        {
            get { return _orgName; }
            set
            {
                _orgName = value;
                OnPropertyChanged("OrgName");
            }
        }

        private string _licenseInfo = "";
        [Attributes.FormVisual("Сведения о лицензии")]
        public string LicenseInfo
        {
            get { return _licenseInfo; }
            set
            {
                _licenseInfo = value;
                OnPropertyChanged("LicenseInfo");
            }
        }

        private int _quantityOfFormsInv = -1;
        [Attributes.FormVisual("Количество отчетных форм по инвентаризации")]
        public int QuantityOfFormsInv
        {
            get { return _quantityOfFormsInv; }
            set
            {
                _quantityOfFormsInv = value;
                OnPropertyChanged("QuantityOfFormsInv");
            }
        }

        private int _quantityOfFormsOper = -1;
        [Attributes.FormVisual("Количество форм оперативных отчетов")]
        public int QuantityOfFormsOper
        {
            get { return _quantityOfFormsOper; }
            set
            {
                _quantityOfFormsOper = value;
                OnPropertyChanged("QuantityOfFormsOper");
            }
        }

        private int _quantityOfFormsYear = -1;
        [Attributes.FormVisual("Количество форм годового отчета")]
        public int QuantityOfFormsYear
        {
            get { return _quantityOfFormsYear; }
            set
            {
                _quantityOfFormsYear = value;
                OnPropertyChanged("QuantityOfFormsYear");
            }
        }

        private string _notes = "";
        [Attributes.FormVisual("Примечания")]
        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                OnPropertyChanged("Notes");
            }
        }
    }
}
