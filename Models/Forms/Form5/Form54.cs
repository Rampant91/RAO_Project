namespace Models
{
//    [Serializable]
//    [Attributes.Form_Class("Форма 5.4: Сведения о наличии в подведомственных организациях ОРИ")]
//    public class Form54 : Abstracts.Form5
//    {
//        public Form54() : base()
//        {
//            //FormNum.Value = "54";
//            //NumberOfFields.Value = 10;
//        }

//        [Attributes.Form_Property("Форма")]
//        public override bool Object_Validation()
//        {
//            return false;
//        }

//        //TypeOfAccountedParts property
//        public int? TypeOfAccountedPartsId { get; set; }
//        [Attributes.Form_Property("Тип учетных единиц")]
//        public virtual RamAccess<int> TypeOfAccountedParts
//        {
//            get
//            {

//                {
//                    return DataAccess.Get<int>(nameof(TypeOfAccountedParts));
//                }

//                {

//                }
//            }
//            set
//            {


//                {
//                    DataAccess.Set(nameof(TypeOfAccountedParts), value);
//                }
//                OnPropertyChanged(nameof(TypeOfAccountedParts));
//            }
//        }
//        //1 or 2
//        //1 or 2
//        private bool TypeOfAccountedParts_Validation(RamAccess<int> value)//Ready
//        {
//            value.ClearErrors();
//            if ((value.Value != 1) && (value.Value != 2))
//            {
//                value.AddError("Недопустимое значение");
//            }
//            return true;
//        }
//        //TypeOfAccountedParts property

//        //KindOri property
//        public int? KindOriId { get; set; }
//        [Attributes.Form_Property("Вид ОРИ")]
//        public virtual RamAccess<int> KindOri
//        {
//            get
//            {

//                {
//                    return DataAccess.Get<int>(nameof(KindOri));
//                }

//                {

//                }
//            }
//            set
//            {


//                {
//                    DataAccess.Set(nameof(KindOri), value);
//                }
//                OnPropertyChanged(nameof(KindOri));
//            }
//        }


//        private bool KindOri_Validation(RamAccess<int> value)//TODO
//        {
//            return true;
//        }
//        //KindOri property

//        //AggregateState property
//public int? AggregateStateId { get; set; }
//        [Attributes.Form_Property("Агрегатное состояние")]
//        public virtual RamAccess<byte> AggregateState//1 2 3
//        {
//            get
//            {

//                {
//                    return DataAccess.Get<byte>(nameof(AggregateState));
//                }

//                {

//                }
//            }
//            set
//            {


//                {
//                    DataAccess.Set(nameof(AggregateState), value);
//                }
//                OnPropertyChanged(nameof(AggregateState));
//            }
//        }


//        private bool AggregateState_Validation(RamAccess<byte> value)//Ready
//        {
//            value.ClearErrors();
//            if ((value.Value != 1) && (value.Value != 2) && (value.Value != 3))
//            {
//                value.AddError("Недопустимое значение");
//                return false;
//            }
//            return true;
//        }
//        //AggregateState property

//        //Radionuclids property
//        public int? RadionuclidsId { get; set; }
//        [Attributes.Form_Property("радионуклиды")]
//        public virtual RamAccess<string> Radionuclids
//        {
//            get
//            {

//                {
//                    return DataAccess.Get<string>(nameof(Radionuclids));//OK

//                }

//                {

//                }
//            }
//            set
//            {



//                {
//                    DataAccess.Set(nameof(Radionuclids), value);
//                }
//                OnPropertyChanged(nameof(Radionuclids));
//            }
//        }
//        //If change this change validation

//        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
//        {
//            value.ClearErrors();
//            if (string.IsNullOrEmpty(value.Value))
//            {
//                value.AddError("Поле не заполнено");
//                return false;
//            }
//            string[] nuclids = value.Value.Split("; ");
//            bool flag = true;
//            foreach (var nucl in nuclids)
//            {
//                var tmp = from item in Spravochniks.SprRadionuclids where nucl == item.Item1 select item.Item1;
//                if (tmp.Count() == 0)
//                    flag = false;
//            }
//            if (!flag)
//            {
//                value.AddError("Недопустимое значение");
//                return false;
//            }
//            return true;
//        }
//        //Radionuclids property

//        //Activity property
//        public int? ActivityId { get; set; }
//        [Attributes.Form_Property("активность, Бк")]
//        public virtual RamAccess<string> Activity
//        {
//            get
//            {

//                {
//                    return DataAccess.Get<string>(nameof(Activity));//OK

//                }

//                {

//                }
//            }
//            set
//            {


//                {
//                    DataAccess.Set(nameof(Activity), value);
//                }
//                OnPropertyChanged(nameof(Activity));
//            }
//        }


//        private bool Activity_Validation(RamAccess<string> value)//Ready
//        {
//            value.ClearErrors();
//            if (string.IsNullOrEmpty(value.Value))
//            {
//                value.AddError("Поле не заполнено");
//                return false;
//            }
//            if (!(value.Value.Contains('e')))
//            {
//                value.AddError("Недопустимое значение");
//                return false;
//            }
//            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
//               NumberStyles.AllowExponent;
//            try
//            {
//                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
//                {
//                    value.AddError("Число должно быть больше нуля"); return false;
//                }
//            }
//            catch
//            {
//                value.AddError("Недопустимое значение"); return false;
//            }
//            return true;
//        }
//        //Activity property

//        //Quantity property
//        public int? QuantityId { get; set; }
//        [Attributes.Form_Property("количество, шт.")]
//        public virtual RamAccess<int> Quantity
//        {
//            get
//            {

//                {
//                    return DataAccess.Get<int>(nameof(Quantity));//OK

//                }

//                {

//                }
//            }
//            set
//            {




//                {
//                    DataAccess.Set(nameof(Quantity), value);
//                }
//                OnPropertyChanged(nameof(Quantity));
//            }
//        }
//        // positive int.

//        private bool Quantity_Validation(RamAccess<int> value)//Ready
//        {
//            value.ClearErrors();
//            if (value.Value <= 0)
//            {
//                value.AddError("Недопустимое значение");
//                return false;
//            }
//            return true;
//        }
//        //Quantity property

//        //Volume property
//        public int? VolumeId { get; set; }
//        [Attributes.Form_Property("Объем, куб. м")]
//        public virtual RamAccess<double> Volume
//        {
//            get
//            {

//                {
//                    return DataAccess.Get<double>(nameof(Volume));
//                }

//                {

//                }
//            }
//            set
//            {


//                {
//                    DataAccess.Set(nameof(Volume), value);
//                }
//                OnPropertyChanged(nameof(Volume));
//            }
//        }


//        private bool Volume_Validation(RamAccess<double> value)//TODO
//        {
//            value.ClearErrors();
//            if (value.Value <= 0)
//            {
//                value.AddError("Недопустимое значение");
//                return false;
//            }
//            return true;
//        }
//        //Volume property

//        //Mass Property
//        public int? MassId { get; set; }
//        [Attributes.Form_Property("Масса, кг")]
//        public virtual RamAccess<double> Mass
//        {
//            get
//            {

//                {
//                    return DataAccess.Get<double>(nameof(Mass));
//                }

//                {

//                }
//            }
//            set
//            {


//                {
//                    DataAccess.Set(nameof(Mass), value);
//                }
//                OnPropertyChanged(nameof(Mass));
//            }
//        }


//        private bool Mass_Validation(RamAccess<double> value)//TODO
//        {
//            value.ClearErrors();
//            if (value.Value <= 0)
//            {
//                value.AddError("Недопустимое значение");
//                return false;
//            }
//            return true;
//        }
//        //Mass Property
//    }
}
