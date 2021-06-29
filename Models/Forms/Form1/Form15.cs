using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.5: Сведения о РАО в виде отработавших ЗРИ")]
    public class Form15 : Abstracts.Form1
    {
        public Form15() : base()
        {
            //FormNum.Value = "15";
            //NumberOfFields.Value = 39;
            Init();
            Validate_all();
        }

        private void Init()
        {
            DataAccess.Init<string>(nameof(Type), Type_Validation, null);
            Type.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TypeRecoded), TypeRecoded_Validation, null);
            TypeRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackName), PackName_Validation, null);
            PackName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackNumberRecoded), PackNumberRecoded_Validation, null);
            PackNumberRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackNumber), PackNumber_Validation, null);
            PackNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackTypeRecoded), PackTypeRecoded_Validation, null);
            PackTypeRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PackType), PackType_Validation, null);
            PackType.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PassportNumberRecoded), PassportNumberRecoded_Validation, null);
            PassportNumberRecoded.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(PassportNumber), PassportNumber_Validation, null);
            PassportNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ProviderOrRecieverOKPO), ProviderOrRecieverOKPO_Validation, null);
            ProviderOrRecieverOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(TransporterOKPO), TransporterOKPO_Validation, null);
            TransporterOKPO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Activity), Activity_Validation, null);
            Activity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Radionuclids), Radionuclids_Validation, null);
            Radionuclids.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FactoryNumber), FactoryNumber_Validation, null);
            FactoryNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<int?>(nameof(Quantity), Quantity_Validation, null);
            Quantity.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(CreationDate), CreationDate_Validation, null);
            CreationDate.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StatusRAO), StatusRAO_Validation, null);
            StatusRAO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Subsidy), Subsidy_Validation, null);
            Subsidy.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FcpNumber), FcpNumber_Validation, null);
            FcpNumber.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceName), StoragePlaceName_Validation, null);
            StoragePlaceName.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(StoragePlaceCode), StoragePlaceCode_Validation, null);
            StoragePlaceCode.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(FactoryNumberRecoded), FactoryNumberRecoded_Validation, null);
            FactoryNumberRecoded.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            Type_Validation(Type);
            TypeRecoded_Validation(TypeRecoded);
            PackName_Validation(PackName);
            PackNumberRecoded_Validation(PackNumberRecoded);
            PackNumber_Validation(PackNumber);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackType_Validation(PackType);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
            PassportNumber_Validation(PassportNumber);
            FactoryNumber_Validation(FactoryNumber);
            FactoryNumberRecoded_Validation(FactoryNumberRecoded);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            TransporterOKPO_Validation(TransporterOKPO);
            Activity_Validation(Activity);
            Radionuclids_Validation(Radionuclids);
            Quantity_Validation(Quantity);
            CreationDate_Validation(CreationDate);
            Subsidy_Validation(Subsidy);
            FcpNumber_Validation(FcpNumber);
            StatusRAO_Validation(StatusRAO);
            StoragePlaceName_Validation(StoragePlaceName);
            StoragePlaceCode_Validation(StoragePlaceCode);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PassportNumber property
        public int? PassportNumberId { get; set; }
        [Attributes.Form_Property("Номер паспорта")]
        public virtual RamAccess<string> PassportNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PassportNumber));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PassportNumber), value);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }


        private bool PassportNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            return true;
        }
        //PassportNumber property

        //PassportNumberNote property
        public virtual RamAccess<string> PassportNumberNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PassportNumberNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PassportNumberNote), value);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }


        private bool PassportNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        public int? PassportNumberRecodedId { get; set; }
        public virtual RamAccess<string> PassportNumberRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PassportNumberRecoded));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PassportNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation

        private bool PassportNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //PassportNumberRecoded property

        //Type property
        public int? TypeId { get; set; }
        [Attributes.Form_Property("Тип")]
        public virtual RamAccess<string> Type
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Type));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Type), value);
                }
                OnPropertyChanged(nameof(Type));
            }
        }


        private bool Type_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //Type property

        //TypeRecoded property
        public virtual RamAccess<string> TypeRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(TypeRecoded));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(TypeRecoded), value);
                }
                OnPropertyChanged(nameof(TypeRecoded));
            }
        }


        private bool TypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //TypeRecoded property

        //Radionuclids property
        public int? RadionuclidsId { get; set; }
        [Attributes.Form_Property("Радионуклиды")]
        public virtual RamAccess<string> Radionuclids
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Radionuclids));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation

        private bool Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (Tuple<string, string> item in spr)
            {
                if (item.Item1.Equals(Type))
                {
                    Radionuclids.Value = item.Item2;
                    return true;
                }
            }
            return true;
        }
        //Radionuclids property

        //FactoryNumber property
        public int? FactoryNumberId { get; set; }
        [Attributes.Form_Property("Заводской номер")]
        public virtual RamAccess<string> FactoryNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(FactoryNumber));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FactoryNumber), value);
                }
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }


        private bool FactoryNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public virtual RamAccess<string> FactoryNumberRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(FactoryNumberRecoded));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FactoryNumberRecoded), value);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }

        private bool FactoryNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //FactoryNumberRecoded property

        //Quantity property
        public int? QuantityId { get; set; }
        [Attributes.Form_Property("Количество, шт.")]
        public virtual RamAccess<int?> Quantity
        {
            get
            {

                {
                    return DataAccess.Get<int?>(nameof(Quantity));//OK

                }

                {

                }
            }
            set
            {




                {
                    DataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.

        private bool Quantity_Validation(RamAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value <= 0)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Quantity property

        //Activity property
        public int? ActivityId { get; set; }
        [Attributes.Form_Property("Активность, Бк")]
        public virtual RamAccess<string> Activity
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Activity));//OK

                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(Activity), value);
                OnPropertyChanged(nameof(Activity));
            }
        }


        private bool Activity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!(value.Value.Contains('e') || value.Value.Contains('E')))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            string tmp = value.Value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            NumberStyles styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(tmp, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)) { value.AddError("Число должно быть больше нуля"); return false; }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Activity property

        //CreationDate property
        public int? CreationDateId { get; set; }
        [Attributes.Form_Property("Дата изготовления")]
        public virtual RamAccess<string> CreationDate
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(CreationDate));//OK

                }

                {

                }
            }
            set
            {
                DataAccess.Set(nameof(CreationDate), value);
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation

        private bool CreationDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //CreationDate property

        //StatusRAO property
        public int? StatusRAOId { get; set; }
        [Attributes.Form_Property("Статус РАО")]
        public virtual RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StatusRAO));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StatusRAO), value);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }


        private bool StatusRAO_Validation(RamAccess<string> value)//rdy
        {
            value.ClearErrors();
            if (value.Value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError("Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    value.AddError("Недопустимое значение");
                }
                return false;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //StatusRAO property

        //ProviderOrRecieverOKPO property
        public int? ProviderOrRecieverOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public virtual RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }


        private bool ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим.")) { return true; }
            short tmp = (short)OperationCode.Value;
            bool a = (tmp >= 10) && (tmp <= 14);
            bool b = (tmp >= 41) && (tmp <= 45);
            bool c = (tmp >= 71) && (tmp <= 73);
            bool e = (tmp >= 55) && (tmp <= 57);
            bool d = (tmp == 1) || (tmp == 16) || (tmp == 18) || (tmp == 48) ||
                (tmp == 49) || (tmp == 51) || (tmp == 52) || (tmp == 59) ||
                (tmp == 68) || (tmp == 75) || (tmp == 76);
            if (a || b || c || d || e)
            {
                ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public virtual RamAccess<string> ProviderOrRecieverOKPONote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }


        private bool ProviderOrRecieverOKPONote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        public int? TransporterOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО перевозчика")]
        public virtual RamAccess<string> TransporterOKPO
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(TransporterOKPO));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(TransporterOKPO), value);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }


        private bool TransporterOKPO_Validation(RamAccess<string> value)//Done
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public virtual RamAccess<string> TransporterOKPONote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(TransporterOKPONote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(TransporterOKPONote), value);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }


        private bool TransporterOKPONote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //TransporterOKPONote property

        //PackName property
        public int? PackNameId { get; set; }
        [Attributes.Form_Property("Наименование упаковки")]
        public virtual RamAccess<string> PackName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackName));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }


        private bool PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PackName property

        //PackNameNote property
        public virtual RamAccess<string> PackNameNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackNameNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PackNameNote), value);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }


        private bool PackNameNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PackNameNote property

        //PackType property
        public int? PackTypeId { get; set; }
        [Attributes.Form_Property("Тип упаковки")]
        public virtual RamAccess<string> PackType
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackType));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation

        private bool PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                //    value.AddError( "Заполните примечание");// to do note handling
                return true;
            }
            return true;
        }
        //PackType property

        //PackTypeRecoded property
        public virtual RamAccess<string> PackTypeRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackTypeRecoded));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }


        private bool PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PackTypeRecoded property

        //PackTypeNote property
        public virtual RamAccess<string> PackTypeNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackTypeNote));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PackTypeNote), value);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }


        private bool PackTypeNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PackTypeNote property

        //PackNumber property
        public int? PackNumberId { get; set; }
        [Attributes.Form_Property("Номер упаковки")]
        public virtual RamAccess<string> PackNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackNumber));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackNumber), value);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation

        private bool PackNumber_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PackNumber property

        //PackNumberNote property
        public virtual RamAccess<string> PackNumberNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackNumberNote));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackNumberNote), value);
                }
                OnPropertyChanged(nameof(PackNumberNote));
            }
        }


        private bool PackNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PackNumberNote property

        //PackNumberRecoded property
        public int? PackNumberRecodedId { get; set; }
        public virtual RamAccess<string> PackNumberRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackNumberRecoded));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation

        private bool PackNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //PackNumberRecoded property

        //StoragePlaceName property
        public int? StoragePlaceNameId { get; set; }
        [Attributes.Form_Property("Наименование ПХ")]
        public virtual RamAccess<string> StoragePlaceName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceName));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceName), value);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation

        private bool StoragePlaceName_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<string> a = new List<string>();//here binds spr
            if (a.Contains(value.Value))
            {
                return true;
            }

            value.AddError("Недопустимое значение");
            return false;
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public virtual RamAccess<string> StoragePlaceNameNote
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceNameNote));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceNameNote), value);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation

        private bool StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //StoragePlaceNameNote property

        //StoragePlaceCode property
        public int? StoragePlaceCodeId { get; set; }
        [Attributes.Form_Property("Код ПХ")]
        public virtual RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(StoragePlaceCode));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(StoragePlaceCode), value);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation

        private bool StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<string> lst = new List<string>();//HERE binds spr
            if (!lst.Contains(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //StoragePlaceCode property

        //RefineOrSortRAOCode property
        public int? RefineOrSortRAOCodeId { get; set; }
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public virtual RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(RefineOrSortRAOCode));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(RefineOrSortRAOCode), value);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        //If change this change validation

        private bool RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                return false;
            }
            Regex a = new Regex("^[0-9][0-9]$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //RefineOrSortRAOCode property

        //Subsidy property
        public int? SubsidyId { get; set; }
        [Attributes.Form_Property("Субсидия, %")]
        public virtual RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Subsidy));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Subsidy), value);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }


        private bool Subsidy_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            try
            {
                int tmp = int.Parse(value.Value);
                if (!((tmp > 0) && (tmp <= 100)))
                {
                    value.AddError("Недопустимое значение"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Subsidy property

        //FcpNumber property
        public int? FcpNumberId { get; set; }
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public virtual RamAccess<string> FcpNumber
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(FcpNumber));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }


        private bool FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        //FcpNumber property

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }

        protected override bool OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            bool flag = false;
            foreach (short item in spr)
            {
                if (item == value.Value)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            bool a0 = value.Value == 15;
            bool a1 = value.Value == 17;
            bool a2 = value.Value == 46;
            bool a3 = value.Value == 47;
            bool a4 = value.Value == 53;
            bool a5 = value.Value == 54;
            bool a6 = value.Value == 58;
            bool a7 = value.Value == 61;
            bool a8 = value.Value == 62;
            bool a9 = value.Value == 65;
            bool a10 = value.Value == 66;
            bool a11 = value.Value == 67;
            bool a12 = value.Value == 81;
            bool a13 = value.Value == 82;
            bool a14 = value.Value == 83;
            bool a15 = value.Value == 85;
            bool a16 = value.Value == 86;
            bool a17 = value.Value == 87;
            if (a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11 || a12 || a13 || a14 || a15 || a16 || a17)
            {
                value.AddError("Код операции не может быть использован для РАО");
            }

            return false;
        }
    }
}
