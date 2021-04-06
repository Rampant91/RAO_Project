using System;
using System.Globalization;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Характеристики экспортируемых ЗРИ/ОЗИИИ:")]
    public class Form31_1 : Form3
    {
        public Form31_1() : base()
        {
            FormNum = "31_1";
            NumberOfFields = 3;
        }

        [Attributes.FormVisual("Форма")]

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

        //SummaryActivity property
        [Attributes.FormVisual("Суммарная активность, Бк")]
        public string SummaryActivity
        {
            get
            {
                if (GetErrors(nameof(SummaryActivity)) != null)
                {
                    return (string)_SummaryActivity.Get();
                }
                else
                {
                    return _SummaryActivity_Not_Valid;
                }
            }
            set
            {
                _SummaryActivity_Not_Valid = value;
                if (GetErrors(nameof(SummaryActivity)) != null)
                {
                    _SummaryActivity.Set(_SummaryActivity_Not_Valid);
                }
                OnPropertyChanged(nameof(SummaryActivity));
            }
        }
        private IDataLoadEngine _SummaryActivity;
        private string _SummaryActivity_Not_Valid = "";
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
        //SummaryActivity property
    }
}
