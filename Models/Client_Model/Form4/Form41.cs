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
        public override string FormNum { get { return "4.1"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 9;

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

        //QuantityOfFormsInv property
        [Attributes.FormVisual("Количество отчетных форм по инвентаризации, шт.")]
        public int QuantityOfFormsInv
        {
            get
            {
                if (GetErrors(nameof(QuantityOfFormsInv)) != null)
                {
                    return (int)_QuantityOfFormsInv.Get();
                }
                else
                {
                    return _QuantityOfFormsInv_Not_Valid;
                }
            }
            set
            {
                _QuantityOfFormsInv_Not_Valid = value;
                if (GetErrors(nameof(QuantityOfFormsInv)) != null)
                {
                    _QuantityOfFormsInv.Set(_QuantityOfFormsInv_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOfFormsInv));
            }
        }
        private IDataLoadEngine _QuantityOfFormsInv;  // positive int.
        private int _QuantityOfFormsInv_Not_Valid = -1;
        private void QuantityOfFormsInv_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOfFormsInv));
            if (value <= 0)
                AddError(nameof(QuantityOfFormsInv), "Недопустимое значение");
        }
        //QuantityOfFormsInv property

        //QuantityOfFormsOper property
        [Attributes.FormVisual("Количество форм оперативных отчетов, шт.")]
        public int QuantityOfFormsOper
        {
            get
            {
                if (GetErrors(nameof(QuantityOfFormsOper)) != null)
                {
                    return (int)_QuantityOfFormsOper.Get();
                }
                else
                {
                    return _QuantityOfFormsOper_Not_Valid;
                }
            }
            set
            {
                _QuantityOfFormsOper_Not_Valid = value;
                if (GetErrors(nameof(QuantityOfFormsOper)) != null)
                {
                    _QuantityOfFormsOper.Set(_QuantityOfFormsOper_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOfFormsOper));
            }
        }
        private IDataLoadEngine _QuantityOfFormsOper;  // positive int.
        private int _QuantityOfFormsOper_Not_Valid = -1;
        private void QuantityOfFormsOper_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOfFormsOper));
            if (value <= 0)
                AddError(nameof(QuantityOfFormsOper), "Недопустимое значение");
        }
        //QuantityOfFormsOper property

        //QuantityOfFormsYear property
        [Attributes.FormVisual("Количество форм годовых отчетов, шт.")]
        public int QuantityOfFormsYear
        {
            get
            {
                if (GetErrors(nameof(QuantityOfFormsYear)) != null)
                {
                    return (int)_QuantityOfFormsYear.Get();
                }
                else
                {
                    return _QuantityOfFormsYear_Not_Valid;
                }
            }
            set
            {
                _QuantityOfFormsYear_Not_Valid = value;
                if (GetErrors(nameof(QuantityOfFormsYear)) != null)
                {
                    _QuantityOfFormsYear.Set(_QuantityOfFormsYear_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOfFormsYear));
            }
        }
        private IDataLoadEngine _QuantityOfFormsYear;  // positive int.
        private int _QuantityOfFormsYear_Not_Valid = -1;
        private void QuantityOfFormsYear_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOfFormsYear));
            if (value <= 0)
                AddError(nameof(QuantityOfFormsYear), "Недопустимое значение");
        }
        //QuantityOfFormsYear property

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
