using System;
using System.Globalization;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.9: Активность радионуклидов, отведенных со сточными водами")]
    public class Form29 : Abstracts.Form2
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
                Abstracts.Form2.SQLCommandParamsBase() +
            nameof(AllowedActivity) + strNotNullDeclaration +
            nameof(AllowedActivityNote) + strNotNullDeclaration +
            nameof(FactedActivity) + strNotNullDeclaration +
            nameof(FactedActivityNote) + strNotNullDeclaration +
            nameof(RadionuclidName) + strNotNullDeclaration +
            nameof(WasteSourceName) + " varchar(255) not null";
        }
        public Form29(IDataAccess Access) : base(Access)
        {
            FormNum = "29";
            NumberOfFields = 8;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //WasteSourceName property
        [Attributes.Form_Property("Наименование, номер выпуска сточных вод")]
        public string WasteSourceName
        {
            get
            {
                if (GetErrors(nameof(WasteSourceName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(WasteSourceName));
                }
                else
                {
                    return _WasteSourceName_Not_Valid;
                }
            }
            set
            {
                _WasteSourceName_Not_Valid = value;
                if (GetErrors(nameof(WasteSourceName)) != null)
                {
                    _dataAccess.Set(nameof(WasteSourceName), _WasteSourceName_Not_Valid);
                }
                OnPropertyChanged(nameof(WasteSourceName));
            }
        }
        
        private string _WasteSourceName_Not_Valid = "";
        private void WasteSourceName_Validation()
        {
            ClearErrors(nameof(WasteSourceName));
        }
        //WasteSourceName property

        //RadionuclidName property
        [Attributes.Form_Property("Радионуклид")]
        public string RadionuclidName
        {
            get
            {
                if (GetErrors(nameof(RadionuclidName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RadionuclidName));
                }
                else
                {
                    return _RadionuclidName_Not_Valid;
                }
            }
            set
            {
                _RadionuclidName_Not_Valid = value;
                if (GetErrors(nameof(RadionuclidName)) != null)
                {
                    _dataAccess.Set(nameof(RadionuclidName), _RadionuclidName_Not_Valid);
                }
                OnPropertyChanged(nameof(RadionuclidName));
            }
        }
        //If change this change validation
        private string _RadionuclidName_Not_Valid = "";
        private void RadionuclidName_Validation()//TODO
        {
            ClearErrors(nameof(RadionuclidName));
        }
        //RadionuclidName property

        //AllowedActivity property
        [Attributes.Form_Property("Допустимая активность радионуклида, Бк")]
        public string AllowedActivity
        {
            get
            {
                if (GetErrors(nameof(AllowedActivity)) != null)
                {
                    return (string)_dataAccess.Get(nameof(AllowedActivity));
                }
                else
                {
                    return _AllowedActivity_Not_Valid;
                }
            }
            set
            {
                _AllowedActivity_Not_Valid = value;
                if (GetErrors(nameof(AllowedActivity)) != null)
                {
                    _dataAccess.Set(nameof(AllowedActivity), _AllowedActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedActivity));
            }
        }
        
        private string _AllowedActivity_Not_Valid = "";
        private void AllowedActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(AllowedActivity));
            if (value != "прим.")
            {
                var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
                   NumberStyles.AllowExponent;
                try
                {
                    if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                        AddError(nameof(AllowedActivity), "Число должно быть больше нуля");
                }
                catch
                {
                    AddError(nameof(AllowedActivity), "Недопустимое значение");
                }
            }
        }
        //AllowedActivity property

        //AllowedActivityNote property
        public string AllowedActivityNote
        {
            get
            {
                if (GetErrors(nameof(AllowedActivityNote)) != null)
                {
                    return (string)_dataAccess.Get(nameof(AllowedActivityNote));
                }
                else
                {
                    return _AllowedActivityNote_Not_Valid;
                }
            }
            set
            {
                _AllowedActivityNote_Not_Valid = value;
                if (GetErrors(nameof(AllowedActivityNote)) != null)
                {
                    _dataAccess.Set(nameof(AllowedActivityNote), _AllowedActivityNote_Not_Valid);
                }
                OnPropertyChanged(nameof(AllowedActivityNote));
            }
        }
        
        private string _AllowedActivityNote_Not_Valid = "";
        private void AllowedActivityNote_Validation(string value)//Ready
        {

        }
        //AllowedActivityNote property

        //FactedActivity property
        [Attributes.Form_Property("Фактическая активность радионуклида, Бк")]
        public string FactedActivity
        {
            get
            {
                if (GetErrors(nameof(FactedActivity)) != null)
                {
                    return (string)_dataAccess.Get(nameof(FactedActivity));
                }
                else
                {
                    return _FactedActivity_Not_Valid;
                }
            }
            set
            {
                _FactedActivity_Not_Valid = value;
                if (GetErrors(nameof(FactedActivity)) != null)
                {
                    _dataAccess.Set(nameof(FactedActivity), _FactedActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(FactedActivity));
            }
        }
        
        private string _FactedActivity_Not_Valid = "";
        private void FactedActivity_Validation(string value)//Ready
        {
            ClearErrors(nameof(FactedActivity));
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(FactedActivity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(FactedActivity), "Недопустимое значение");
            }
        }
        //FactedActivity property

        //FactedActivityNote property
        public string FactedActivityNote
        {
            get
            {
                if (GetErrors(nameof(FactedActivityNote)) != null)
                {
                    return (string)_dataAccess.Get(nameof(FactedActivityNote));
                }
                else
                {
                    return _FactedActivityNote_Not_Valid;
                }
            }
            set
            {
                _FactedActivityNote_Not_Valid = value;
                if (GetErrors(nameof(FactedActivityNote)) != null)
                {
                    _dataAccess.Set(nameof(FactedActivityNote), _FactedActivityNote_Not_Valid);
                }
                OnPropertyChanged(nameof(FactedActivityNote));
            }
        }
        
        private string _FactedActivityNote_Not_Valid = "";
        private void FactedActivityNote_Validation(string value)//Ready
        {

        }
        //FactedActivityNote property
    }
}
