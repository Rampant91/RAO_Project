using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Форма 5.2: Сведения о наличии ЗРИ в подведомственных организациях")]
    public class Form52 : Form5
    {
        public Form52() : base()
        {
        }

        [Attributes.FormVisual("Форма")]
        public override string FormNum { get { return "52"; } }
        public override int NumberOfFields { get; } = 6;
        public override void Object_Validation()
        {

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

        //Kategory property
        [Attributes.FormVisual("Категория")]
        public short Kategory
        {
            get
            {
                if (GetErrors(nameof(Kategory)) != null)
                {
                    return (short)_Kategory.Get();
                }
                else
                {
                    return _Kategory_Not_Valid;
                }
            }
            set
            {
                _Kategory_Not_Valid = value;
                if (GetErrors(nameof(Kategory)) != null)
                {
                    _Kategory.Set(_Kategory_Not_Valid);
                }
                OnPropertyChanged(nameof(Kategory));
            }
        }
        private IDataLoadEngine _Kategory;
        private short _Kategory_Not_Valid = -1;
        private void Kategory_Validation(short value)//TODO
        {
            ClearErrors(nameof(Kategory));
        }
        //Kategory property

        //Activity property
        [Attributes.FormVisual("Активность, Бк")]
        public string Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) != null)
                {
                    return (string)_Activity.Get();
                }
                else
                {
                    return _Activity_Not_Valid;
                }
            }
            set
            {
                _Activity_Not_Valid = value;
                if (GetErrors(nameof(Activity)) != null)
                {
                    _Activity.Set(_Activity_Not_Valid);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }
        private IDataLoadEngine _Activity;
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
    }
}
