using System;
using System.Globalization;
using DBRealization;
using Collections.Rows_Collection;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.4: Сведения о наличии в подведомственных организациях ОРИ")]
    public class Form54 : Abstracts.Form5
    {
        public Form54(IDataAccess Access) : base(Access)
        {
            FormNum = "54";
            NumberOfFields = 10;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //TypeOfAccountedParts property
        [Attributes.Form_Property("Тип учетных единиц")]
        public int TypeOfAccountedParts
        {
            get
            {
                if (GetErrors(nameof(TypeOfAccountedParts)) == null)
                {
                    return (int)_dataAccess.Get(nameof(TypeOfAccountedParts))[0][0];
                }
                else
                {
                    return _TypeOfAccountedParts_Not_Valid;
                }
            }
            set
            {
                _TypeOfAccountedParts_Not_Valid = value;
                if (GetErrors(nameof(TypeOfAccountedParts)) == null)
                {
                    _dataAccess.Set(nameof(TypeOfAccountedParts), _TypeOfAccountedParts_Not_Valid);
                }
                OnPropertyChanged(nameof(TypeOfAccountedParts));
            }
        }
         //1 or 2
        private int _TypeOfAccountedParts_Not_Valid = -1; //1 or 2
        private void TypeOfAccountedParts_Validation(int value)//Ready
        {
            ClearErrors(nameof(TypeOfAccountedParts));
            if ((value != 1) && (value != 2))
                AddError(nameof(TypeOfAccountedParts), "Недопустимое значение");
        }
        //TypeOfAccountedParts property

        //KindOri property
        [Attributes.Form_Property("Вид ОРИ")]
        public int KindOri
        {
            get
            {
                if (GetErrors(nameof(KindOri)) == null)
                {
                    return (int)_dataAccess.Get(nameof(KindOri))[0][0];
                }
                else
                {
                    return _KindOri_Not_Valid;
                }
            }
            set
            {
                _KindOri_Not_Valid = value;
                if (GetErrors(nameof(KindOri)) == null)
                {
                    _dataAccess.Set(nameof(KindOri), _KindOri_Not_Valid);
                }
                OnPropertyChanged(nameof(KindOri));
            }
        }
        
        private int _KindOri_Not_Valid = -1;
        private void KindOri_Validation(int value)//TODO
        {
        }
        //KindOri property

        //AggregateState property
        [Attributes.Form_Property("Агрегатное состояние")]
        public byte AggregateState//1 2 3
        {
            get
            {
                if (GetErrors(nameof(AggregateState)) == null)
                {
                    return (byte)_dataAccess.Get(nameof(AggregateState))[0][0];
                }
                else
                {
                    return _AggregateState_Not_Valid;
                }
            }
            set
            {
                _AggregateState_Not_Valid = value;
                if (GetErrors(nameof(AggregateState)) == null)
                {
                    _dataAccess.Set(nameof(AggregateState), _AggregateState_Not_Valid);
                }
                OnPropertyChanged(nameof(AggregateState));
            }
        }
        
        private byte _AggregateState_Not_Valid = 0;
        private void AggregateState_Validation(byte value)//Ready
        {
            ClearErrors(nameof(AggregateState));
            if ((value != 1) && (value != 2) && (value != 3))
                AddError(nameof(AggregateState), "Недопустимое значение");
        }
        //AggregateState property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Radionuclids))[0][0];                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                _Radionuclids_Not_Valid = value;
                if (GetErrors(nameof(Radionuclids)) == null)
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

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public string Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Activity))[0][0];
                }
                else
                {
                    return _Activity_Not_Valid;
                }
            }
            set
            {
                _Activity_Not_Valid = value;
                if (GetErrors(nameof(Activity)) == null)
                {
                    _dataAccess.Set(nameof(Activity), _Activity_Not_Valid);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }
        
        private string _Activity_Not_Valid = "";
        private void Activity_Validation(string value)//Ready
        {
            ClearErrors(nameof(Activity));
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    AddError(nameof(Activity), "Число должно быть больше нуля");
            }
            catch
            {
                AddError(nameof(Activity), "Недопустимое значение");
            }
        }
        //Activity property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) == null)
                {
                    return (int)_dataAccess.Get(nameof(Quantity))[0][0];
                }
                else
                {
                    return _Quantity_Not_Valid;
                }
            }
            set
            {
                _Quantity_Not_Valid = value;
                if (GetErrors(nameof(Quantity)) == null)
                {
                    _dataAccess.Set(nameof(Quantity), _Quantity_Not_Valid);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
          // positive int.
        private int _Quantity_Not_Valid = -1;
        private void Quantity_Validation(int value)//Ready
        {
            ClearErrors(nameof(Quantity));
            if (value <= 0)
                AddError(nameof(Quantity), "Недопустимое значение");
        }
        //Quantity property

        //Volume property
        [Attributes.Form_Property("Объем, куб. м")]
        public double Volume
        {
            get
            {
                if (GetErrors(nameof(Volume)) == null)
                {
                    return (double)_dataAccess.Get(nameof(Volume))[0][0];
                }
                else
                {
                    return _Volume_Not_Valid;
                }
            }
            set
            {
                _Volume_Not_Valid = value;
                if (GetErrors(nameof(Volume)) == null)
                {
                    _dataAccess.Set(nameof(Volume), _Volume_Not_Valid);
                }
                OnPropertyChanged(nameof(Volume));
            }
        }
        
        private double _Volume_Not_Valid = -1;
        private void Volume_Validation(double value)//TODO
        {
            ClearErrors(nameof(Volume));
        }
        //Volume property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public double Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) == null)
                {
                    return (double)_dataAccess.Get(nameof(Mass))[0][0];
                }
                else
                {
                    return _Mass_Not_Valid;
                }
            }
            set
            {
                _Mass_Not_Valid = value;
                if (GetErrors(nameof(Mass)) == null)
                {
                    _dataAccess.Set(nameof(Mass), _Mass_Not_Valid);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }
        
        private double _Mass_Not_Valid = -1;
        private void Mass_Validation()//TODO
        {
            ClearErrors(nameof(Mass));
        }
        //Mass Property
    }
}
