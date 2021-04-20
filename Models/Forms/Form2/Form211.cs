using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DBRealization;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.11: Радионуклидный состав загрязненных участков территорий")]
    public class Form211 : Abstracts.Form2
    {
        //public static string SQLCommandParams()
        //{
        //    return
        //        Abstracts.Form2.SQLCommandParamsBase() +
        //    nameof(Radionuclids) + SQLconsts.strNotNullDeclaration +
        //    nameof(PlotName) + SQLconsts.strNotNullDeclaration +
        //    nameof(PlotKadastrNumber) + SQLconsts.strNotNullDeclaration +
        //    nameof(PlotCode) + SQLconsts.strNotNullDeclaration +
        //    nameof(InfectedArea) + SQLconsts.intNotNullDeclaration +
        //    nameof(RadionuclidNameNote) + SQLconsts.strNotNullDeclaration +
        //    nameof(SpecificActivityOfPlot) + SQLconsts.strNotNullDeclaration +
        //    nameof(SpecificActivityOfLiquidPart) + SQLconsts.strNotNullDeclaration +
        //    nameof(SpecificActivityOfDensePart) + " varchar(255) not null";
        //}
        public Form211(IDataAccess Access) : base(Access)
        {
            FormNum = "211";
            NumberOfFields = 11;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PlotName property
        [Attributes.Form_Property("Наименование участка")]
        public string PlotName
        {
            get
            {
                if (GetErrors(nameof(PlotName)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PlotName));
                }
                else
                {
                    return _PlotName_Not_Valid;
                }
            }
            set
            {
                _PlotName_Not_Valid = value;
                if (GetErrors(nameof(PlotName)) != null)
                {
                    _dataAccess.Set(nameof(PlotName), _PlotName_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotName));
            }
        }
        
        private string _PlotName_Not_Valid = "";
        private void PlotName_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotName));
        }
        //PlotName property

        //PlotKadastrNumber property
        [Attributes.Form_Property("Кадастровый номер участка")]
        public string PlotKadastrNumber
        {
            get
            {
                if (GetErrors(nameof(PlotKadastrNumber)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PlotKadastrNumber));
                }
                else
                {
                    return _PlotKadastrNumber_Not_Valid;
                }
            }
            set
            {
                _PlotKadastrNumber_Not_Valid = value;
                if (GetErrors(nameof(PlotKadastrNumber)) != null)
                {
                    _dataAccess.Set(nameof(PlotKadastrNumber), _PlotKadastrNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PlotKadastrNumber));
            }
        }
        
        private string _PlotKadastrNumber_Not_Valid = "";
        private void PlotKadastrNumber_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotKadastrNumber));
        }
        //PlotKadastrNumber property

        //PlotCode property
        [Attributes.Form_Property("Код участка")]
        public string PlotCode
        {
            get
            {
                if (GetErrors(nameof(PlotCode)) != null)
                {
                    return (string)_dataAccess.Get(nameof(PlotCode));                }
                else
                {
                    return _PlotCode_Not_Valid;
                }
            }
            set
            {
                _PlotCode_Not_Valid = value;
                if (GetErrors(nameof(PlotCode)) != null)
                {
                    _dataAccess.Set(nameof(PlotCode), _PlotCode_Not_Valid);                }
                OnPropertyChanged(nameof(PlotCode));
            }
        }
         //6 symbols code
        private string _PlotCode_Not_Valid = ""; //6 symbols code
        private void PlotCode_Validation(string value)//TODO
        {
            ClearErrors(nameof(PlotCode));
        }
        //PlotCode property

        //InfectedArea property
        [Attributes.Form_Property("Площадь загрязненной территории, кв. м")]
        public int InfectedArea
        {
            get
            {
                if (GetErrors(nameof(InfectedArea)) != null)
                {
                    return (int)_dataAccess.Get(nameof(InfectedArea));
                }
                else
                {
                    return _InfectedArea_Not_Valid;
                }
            }
            set
            {
                _InfectedArea_Not_Valid = value;
                if (GetErrors(nameof(InfectedArea)) != null)
                {
                    _dataAccess.Set(nameof(InfectedArea), _InfectedArea_Not_Valid);
                }
                OnPropertyChanged(nameof(InfectedArea));
            }
        }
        
        private int _InfectedArea_Not_Valid = -1;
        private void InfectedArea_Validation(int value)//TODO
        {
            ClearErrors(nameof(InfectedArea));
        }
        //InfectedArea property

        //Radionuclids property
        [Attributes.Form_Property("Наименования радионуклидов")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) != null)
                {
                    return (string)_dataAccess.Get(nameof(Radionuclids));                }
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
                    _dataAccess.Set(nameof(Radionuclids), _Radionuclids_Not_Valid);                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation()//TODO
        {
            ClearErrors(nameof(Radionuclids));
        }
        //Radionuclids property

        //RadionuclidNameNote property
        public string RadionuclidNameNote
        {
            get
            {
                if (GetErrors(nameof(RadionuclidNameNote)) != null)
                {
                    return (string)_dataAccess.Get(nameof(RadionuclidNameNote));
                }
                else
                {
                    return _RadionuclidNameNote_Not_Valid;
                }
            }
            set
            {
                _RadionuclidNameNote_Not_Valid = value;
                if (GetErrors(nameof(RadionuclidNameNote)) != null)
                {
                    _dataAccess.Set(nameof(RadionuclidNameNote), _RadionuclidNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(RadionuclidNameNote));
            }
        }
        
        private string _RadionuclidNameNote_Not_Valid = "";
        private void RadionuclidNameNote_Validation()
        {
            ClearErrors(nameof(RadionuclidNameNote));
        }
        //RadionuclidNameNote property

        //SpecificActivityOfPlot property
        [Attributes.Form_Property("Удельная активность, Бк/г")]
        public string SpecificActivityOfPlot
        {
            get
            {
                if (GetErrors(nameof(SpecificActivityOfPlot)) != null)
                {
                    return (string)_dataAccess.Get(nameof(SpecificActivityOfPlot));
                }
                else
                {
                    return _SpecificActivityOfPlot_Not_Valid;
                }
            }
            set
            {
                _SpecificActivityOfPlot_Not_Valid = value;
                if (GetErrors(nameof(SpecificActivityOfPlot)) != null)
                {
                    _dataAccess.Set(nameof(SpecificActivityOfPlot), _SpecificActivityOfPlot_Not_Valid);
                }
                OnPropertyChanged(nameof(SpecificActivityOfPlot));
            }
        }
        
        private string _SpecificActivityOfPlot_Not_Valid = "";
        private void SpecificActivityOfPlot_Validation(string value)//TODO
        {
            //ClearErrors(nameof(SpecificActivityOfPlot));
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        AddError(nameof(SpecificActivityOfPlot), "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    AddError(nameof(SpecificActivityOfPlot), "Недопустимое значение");
            //}
        }
        //SpecificActivityOfPlot property

        //SpecificActivityOfLiquidPart property
        [Attributes.Form_Property("Удельная активность жидкой части, Бк/г")]
        public string SpecificActivityOfLiquidPart
        {
            get
            {
                if (GetErrors(nameof(SpecificActivityOfLiquidPart)) != null)
                {
                    return (string)_dataAccess.Get(nameof(SpecificActivityOfLiquidPart));
                }
                else
                {
                    return _SpecificActivityOfLiquidPart_Not_Valid;
                }
            }
            set
            {
                _SpecificActivityOfLiquidPart_Not_Valid = value;
                if (GetErrors(nameof(SpecificActivityOfLiquidPart)) != null)
                {
                    _dataAccess.Set(nameof(SpecificActivityOfLiquidPart), _SpecificActivityOfLiquidPart_Not_Valid);
                }
                OnPropertyChanged(nameof(SpecificActivityOfLiquidPart));
            }
        }
        
        private string _SpecificActivityOfLiquidPart_Not_Valid = "";
        private void SpecificActivityOfLiquidPart_Validation(string value)//TODO
        {
            //ClearErrors(nameof(SpecificActivityOfLiquidPart));
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        AddError(nameof(SpecificActivityOfLiquidPart), "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    AddError(nameof(SpecificActivityOfLiquidPart), "Недопустимое значение");
            //}
        }
        //SpecificActivityOfLiquidPart property

        //SpecificActivityOfDensePart property
        [Attributes.Form_Property("Удельная активность твердой части, Бк/г")]
        public string SpecificActivityOfDensePart
        {
            get
            {
                if (GetErrors(nameof(SpecificActivityOfDensePart)) != null)
                {
                    return (string)_dataAccess.Get(nameof(SpecificActivityOfDensePart));
                }
                else
                {
                    return _SpecificActivityOfDensePart_Not_Valid;
                }
            }
            set
            {
                _SpecificActivityOfDensePart_Not_Valid = value;
                if (GetErrors(nameof(SpecificActivityOfDensePart)) != null)
                {
                    _dataAccess.Set(nameof(SpecificActivityOfDensePart), _SpecificActivityOfDensePart_Not_Valid);
                }
                OnPropertyChanged(nameof(SpecificActivityOfDensePart));
            }
        }
        
        private string _SpecificActivityOfDensePart_Not_Valid = "";
        private void SpecificActivityOfDensePart_Validation(string value)//TODO
        {
            //ClearErrors(nameof(SpecificActivityOfDensePart));
            //var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //   NumberStyles.AllowExponent;
            //try
            //{
            //    if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
            //        AddError(nameof(SpecificActivityOfDensePart), "Число должно быть больше нуля");
            //}
            //catch
            //{
            //    AddError(nameof(SpecificActivityOfDensePart), "Недопустимое значение");
            //}
        }
        //SpecificActivityOfDensePart property
    }
}
