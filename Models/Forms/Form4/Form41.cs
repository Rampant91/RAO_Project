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
            FormNum = "41";
            NumberOfFields = 10;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //NumberInOrder property
        [Attributes.Form_Property("№ п/п")]
        public int NumberInOrder
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(NumberInOrder));
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(NumberInOrder), value);
                }
                OnPropertyChanged(nameof(NumberInOrder));
            }
        }


        private void NumberInOrder_Validation()
        {
            value.ClearErrors();
        }
        //NumberInOrder property

        //RegNo property
        [Attributes.Form_Property("Регистрационный номер")]
        public IDataAccess<string> RegNo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RegNo));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RegNo), value);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }


        //RegNo property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public IDataAccess<string> Okpo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okpo));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okpo), value);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }

        private void Okpo_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //Okpo property

        //OrgName property
        [Attributes.Form_Property("Наименование организации")]
        public IDataAccess<string> OrgName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(OrgName));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(OrgName), value);
                }
                OnPropertyChanged(nameof(OrgName));
            }
        }


        //OrgName property

        //LicenseInfo property
        [Attributes.Form_Property("Сведения о лицензии")]
        public IDataAccess<string> LicenseInfo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(LicenseInfo));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(LicenseInfo), value);
                }
                OnPropertyChanged(nameof(LicenseInfo));
            }
        }


        //LicenseInfo property

        //QuantityOfFormsInv property
        [Attributes.Form_Property("Количество отчетных форм по инвентаризации, шт.")]
        public int QuantityOfFormsInv
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityOfFormsInv));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityOfFormsInv_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityOfFormsInv), value);
                }
                OnPropertyChanged(nameof(QuantityOfFormsInv));
            }
        }
        // positive int.

        private void QuantityOfFormsInv_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //QuantityOfFormsInv property

        //QuantityOfFormsOper property
        [Attributes.Form_Property("Количество форм оперативных отчетов, шт.")]
        public int QuantityOfFormsOper
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityOfFormsOper));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityOfFormsOper_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityOfFormsOper), value);
                }
                OnPropertyChanged(nameof(QuantityOfFormsOper));
            }
        }
        // positive int.

        private void QuantityOfFormsOper_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityOfFormsOper property

        //QuantityOfFormsYear property
        [Attributes.Form_Property("Количество форм годовых отчетов, шт.")]
        public int QuantityOfFormsYear
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(QuantityOfFormsYear));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                QuantityOfFormsYear_Validation(value);

                
                {
                    _dataAccess.Set(nameof(QuantityOfFormsYear), value);
                }
                OnPropertyChanged(nameof(QuantityOfFormsYear));
            }
        }
        // positive int.

        private void QuantityOfFormsYear_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //QuantityOfFormsYear property

        //Notes property
        [Attributes.Form_Property("Примечания")]
        public IDataAccess<string> Notes
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Notes));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Notes), value);
                }
                OnPropertyChanged(nameof(Notes));
            }
        }


        //Notes property
    }
}
