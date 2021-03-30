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

        //NumberInOrder property
        [Attributes.FormVisual("№ п/п")]
        public int NumberInOrder
        {
            get
            {
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    return (int)_NumberInOrder.Get();
                }
                else
                {
                    return _NumberInOrder_Not_Valid;
                }
            }
            set
            {
                _NumberInOrder_Not_Valid = value;
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    _NumberInOrder.Set(_NumberInOrder_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        private IDataLoadEngine _NumberInOrder;
        private int _NumberInOrder_Not_Valid = -1;
        private void NumberInOrder_Validation()
        {
            ClearErrors(nameof(NumberInOrder));
        }
        //NumberInOrder property

        //RegNo property
        [Attributes.FormVisual("Регистрационный номер")]
        public string RegNo
        {
            get
            {
                if (GetErrors(nameof(RegNo)) != null)
                {
                    return (string)_RegNo.Get();
                }
                else
                {
                    return _RegNo_Not_Valid;
                }
            }
            set
            {
                _RegNo_Not_Valid = value;
                if (GetErrors(nameof(RegNo)) != null)
                {
                    _RegNo.Set(_RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }
        private IDataLoadEngine _RegNo;
        private string _RegNo_Not_Valid = "";
        //RegNo property

        //Okpo property
        [Attributes.FormVisual("ОКПО")]
        public string Okpo
        {
            get
            {
                if (GetErrors(nameof(Okpo)) != null)
                {
                    return (string)_Okpo.Get();
                }
                else
                {
                    return _Okpo_Not_Valid;
                }
            }
            set
            {
                _Okpo_Not_Valid = value;
                if (GetErrors(nameof(Okpo)) != null)
                {
                    _Okpo.Set(_Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        private IDataLoadEngine _Okpo;
        private string _Okpo_Not_Valid = "";
        //Okpo property

        //OrgName property
        [Attributes.FormVisual("Наименование организации")]
        public string OrgName
        {
            get
            {
                if (GetErrors(nameof(OrgName)) != null)
                {
                    return (string)_OrgName.Get();
                }
                else
                {
                    return _OrgName_Not_Valid;
                }
            }
            set
            {
                _OrgName_Not_Valid = value;
                if (GetErrors(nameof(OrgName)) != null)
                {
                    _OrgName.Set(_OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }
        private IDataLoadEngine _OrgName;
        private string _OrgName_Not_Valid = "";
        //OrgName property

        //LicenseInfo property
        [Attributes.FormVisual("Сведения о лицензии")]
        public string LicenseInfo
        {
            get
            {
                if (GetErrors(nameof(LicenseInfo)) != null)
                {
                    return (string)_LicenseInfo.Get();
                }
                else
                {
                    return _LicenseInfo_Not_Valid;
                }
            }
            set
            {
                _LicenseInfo_Not_Valid = value;
                if (GetErrors(nameof(LicenseInfo)) != null)
                {
                    _LicenseInfo.Set(_LicenseInfo_Not_Valid);
                }
                OnPropertyChanged(nameof(LicenseInfo));
            }
        }
        private IDataLoadEngine _LicenseInfo;
        private string _LicenseInfo_Not_Valid = "";
        //LicenseInfo property

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

        //Notes property
        [Attributes.FormVisual("Примечания")]
        public string Notes
        {
            get
            {
                if (GetErrors(nameof(Notes)) != null)
                {
                    return (string)_Notes.Get();
                }
                else
                {
                    return _Notes_Not_Valid;
                }
            }
            set
            {
                _Notes_Not_Valid = value;
                if (GetErrors(nameof(Notes)) != null)
                {
                    _Notes.Set(_Notes_Not_Valid);
                }
                OnPropertyChanged(nameof(Notes));
            }
        }
        private IDataLoadEngine _Notes;
        private string _Notes_Not_Valid = "";
        //Notes property
    }
}
