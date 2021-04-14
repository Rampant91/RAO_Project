using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 4.1: Перечень организаций, зарегистрированных в СГУК РВ и РАО на региональном уровне")]
    public class Form41 : Abstracts.Form
    {
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            string doubleNotNullDeclaration = " float(53) not null, ";
            return
            nameof(Notes) + strNotNullDeclaration +
            nameof(OrgName) + strNotNullDeclaration +
            nameof(NumberInOrder) + intNotNullDeclaration +
            nameof(LicenseInfo) + strNotNullDeclaration +
            nameof(QuantityOfFormsInv) + intNotNullDeclaration +
            nameof(QuantityOfFormsOper) + intNotNullDeclaration +
            nameof(QuantityOfFormsYear) + intNotNullDeclaration +
            nameof(Notes) + strNotNullDeclaration +
            nameof(RegNo) + strNotNullDeclaration +
            nameof(Okpo) + " varchar(255) not null, ";
        }
        public Form41(int RowID) : base(RowID)
        {
            FormNum = "41";
            NumberOfFields = 10;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public int NumberInOrder
        {
            get
            {
                if (GetErrors(nameof(NumberInOrder)) != null)
                {
                    return (int)_dataAccess.Get(nameof(NumberInOrder));
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
                    _dataAccess.Set(nameof(NumberInOrder), _NumberInOrder_Not_Valid);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }
        
        private int _NumberInOrder_Not_Valid = -1;
        private void NumberInOrder_Validation()
        {
            ClearErrors(nameof(NumberInOrder));
        }
        //NumberInOrder property

        //RegNo property
        [Attributes.Form_Property("Регистрационный номер")]
        public string RegNo
        {
            get
            {
                if (GetErrors(nameof(RegNo)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RegNo));
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
                    _dataAccess.Set(nameof(RegNo), _RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }
        
        private string _RegNo_Not_Valid = "";
        //RegNo property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public string Okpo
        {
            get
            {
                if (GetErrors(nameof(Okpo)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Okpo));
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
                    _dataAccess.Set(nameof(Okpo), _Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        
        private string _Okpo_Not_Valid = "";
        //Okpo property

        //OrgName property
        [Attributes.Form_Property("Наименование организации")]
        public string OrgName
        {
            get
            {
                if (GetErrors(nameof(OrgName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(OrgName));
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
                    _dataAccess.Set(nameof(OrgName), _OrgName_Not_Valid);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }
        
        private string _OrgName_Not_Valid = "";
        //OrgName property

        //LicenseInfo property
        [Attributes.Form_Property("Сведения о лицензии")]
        public string LicenseInfo
        {
            get
            {
                if (GetErrors(nameof(LicenseInfo)) != null)
                {
                    return (string)_dataAccess.Get(nameof(LicenseInfo));
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
                    _dataAccess.Set(nameof(LicenseInfo), _LicenseInfo_Not_Valid);
                }
                OnPropertyChanged(nameof(LicenseInfo));
            }
        }
        
        private string _LicenseInfo_Not_Valid = "";
        //LicenseInfo property

        //QuantityOfFormsInv property
        [Attributes.Form_Property("Количество отчетных форм по инвентаризации, шт.")]
        public int QuantityOfFormsInv
        {
            get
            {
                if (GetErrors(nameof(QuantityOfFormsInv)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityOfFormsInv));
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
                    _dataAccess.Set(nameof(QuantityOfFormsInv), _QuantityOfFormsInv_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOfFormsInv));
            }
        }
          // positive int.
        private int _QuantityOfFormsInv_Not_Valid = -1;
        private void QuantityOfFormsInv_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOfFormsInv));
            if (value <= 0)
                AddError(nameof(QuantityOfFormsInv), "Недопустимое значение");
        }
        //QuantityOfFormsInv property

        //QuantityOfFormsOper property
        [Attributes.Form_Property("Количество форм оперативных отчетов, шт.")]
        public int QuantityOfFormsOper
        {
            get
            {
                if (GetErrors(nameof(QuantityOfFormsOper)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityOfFormsOper));
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
                    _dataAccess.Set(nameof(QuantityOfFormsOper), _QuantityOfFormsOper_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOfFormsOper));
            }
        }
          // positive int.
        private int _QuantityOfFormsOper_Not_Valid = -1;
        private void QuantityOfFormsOper_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOfFormsOper));
            if (value <= 0)
                AddError(nameof(QuantityOfFormsOper), "Недопустимое значение");
        }
        //QuantityOfFormsOper property

        //QuantityOfFormsYear property
        [Attributes.Form_Property("Количество форм годовых отчетов, шт.")]
        public int QuantityOfFormsYear
        {
            get
            {
                if (GetErrors(nameof(QuantityOfFormsYear)) != null)
                {
                    return (int)_dataAccess.Get(nameof(QuantityOfFormsYear));
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
                    _dataAccess.Set(nameof(QuantityOfFormsYear), _QuantityOfFormsYear_Not_Valid);
                }
                OnPropertyChanged(nameof(QuantityOfFormsYear));
            }
        }
          // positive int.
        private int _QuantityOfFormsYear_Not_Valid = -1;
        private void QuantityOfFormsYear_Validation(int value)//Ready
        {
            ClearErrors(nameof(QuantityOfFormsYear));
            if (value <= 0)
                AddError(nameof(QuantityOfFormsYear), "Недопустимое значение");
        }
        //QuantityOfFormsYear property

        //Notes property
        [Attributes.Form_Property("Примечания")]
        public string Notes
        {
            get
            {
                if (GetErrors(nameof(Notes)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Notes));
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
                    _dataAccess.Set(nameof(Notes), _Notes_Not_Valid);
                }
                OnPropertyChanged(nameof(Notes));
            }
        }
        
        private string _Notes_Not_Valid = "";
        //Notes property
    }
}
