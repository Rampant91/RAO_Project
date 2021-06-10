using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.3: Сведения об ОРИ в виде отдельных изделий")]
    public class Form13 : Abstracts.Form1
    {
        public Form13() : base()
        {
            FormNum.Value = "13";
            NumberOfFields.Value = 37;
            Init();
            Validate_all();
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }
        private void Init()
        {
            _dataAccess.Init<string>(nameof(CreationDate), CreationDate_Validation, null);
            CreationDate.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(CreatorOKPO), CreatorOKPO_Validation, null);
            CreatorOKPO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FactoryNumber), FactoryNumber_Validation, null);
            FactoryNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Owner), Owner_Validation, null);
            Owner.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackName), PackName_Validation, null);
            PackName.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackNumber), PackNumber_Validation, null);
            PackNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackType), PackType_Validation, null);
            PackType.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PassportNumber), PassportNumber_Validation, null);
            PassportNumber.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(ProviderOrRecieverOKPO), ProviderOrRecieverOKPO_Validation, null);
            ProviderOrRecieverOKPO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(TransporterOKPO), TransporterOKPO_Validation, null);
            TransporterOKPO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<byte?>(nameof(PropertyCode), PropertyCode_Validation, null);
            PropertyCode.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Type), Type_Validation, null);
            Type.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(TypeRecoded), TypeRecoded_Validation, null);
            TypeRecoded.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Radionuclids), Radionuclids_Validation, null);
            Radionuclids.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Activity), Activity_Validation, null);
            Activity.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<byte?>(nameof(AggregateState), AggregateState_Validation, null);
            AggregateState.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PassportNumberRecoded), PassportNumberRecoded_Validation, null);
            PassportNumberRecoded.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(FactoryNumberRecoded), FactoryNumberRecoded_Validation, null);
            FactoryNumberRecoded.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackNumberRecoded), PackNumberRecoded_Validation, null);
            PackNumberRecoded.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(PackTypeRecoded), PackTypeRecoded_Validation, null);
            PackTypeRecoded.PropertyChanged += InPropertyChanged;
        }

        private void Validate_all()
        {
            TypeRecoded_Validation(TypeRecoded);
            CreationDate_Validation(CreationDate);
            CreatorOKPO_Validation(CreatorOKPO);
            Owner_Validation(Owner);
            PackName_Validation(PackName);
            PackNumber_Validation(PackNumber);
            PackType_Validation(PackType);
            PassportNumber_Validation(PassportNumber);
            PropertyCode_Validation(PropertyCode);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            TransporterOKPO_Validation(TransporterOKPO);
            FactoryNumber_Validation(FactoryNumber);
            AggregateState_Validation(AggregateState);
            Activity_Validation(Activity);
            Radionuclids_Validation(Radionuclids);
            Type_Validation(Type);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackNumberRecoded_Validation(PackNumberRecoded);
            FactoryNumberRecoded_Validation(FactoryNumberRecoded);
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
            if (!spr.Contains((short)value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if ((value.Value == 1) || (value.Value == 13) ||
            (value.Value == 14) || (value.Value == 16) ||
            (value.Value == 26) || (value.Value == 36) ||
            (value.Value == 44) || (value.Value == 45) ||
            (value.Value == 49) || (value.Value == 51) ||
            (value.Value == 52) || (value.Value == 55) ||
            (value.Value == 56) || (value.Value == 57) ||
            (value.Value == 59) || (value.Value == 76))
            {
                value.AddError("Код операции не может быть использован для РВ");
                return false;
            }
            return true;
        }

        //OwnerNote property
        public RamAccess<string> OwnerNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(OwnerNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(OwnerNote), value);
                }
                OnPropertyChanged(nameof(OwnerNote));
            }
        }
        //if change this change validation

        private bool OwnerNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //OwnerNote property

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public RamAccess<string> PassportNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PassportNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PassportNumber), value);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }


        protected bool PassportNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PassportNumberNote.Value == null)||(PassportNumberNote.Value == ""))
                //    value.AddError( "Заполните примечание");//to do note handling
                return true;
            }
            return true;
        }
        //PassportNumber property

        //PassportNumberNote property
        public RamAccess<string> PassportNumberNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PassportNumberNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PassportNumberNote), value);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }


        private bool PassportNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
return false;
            }
            return true;
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PassportNumberRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PassportNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PassportNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation

        private bool PassportNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //PassportNumberRecoded property

        //Type property
        [Attributes.Form_Property("Тип")]
        public RamAccess<string> Type
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Type));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Type), value);
                }
                OnPropertyChanged(nameof(Type));
            }
        }


        private bool Type_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //Type property

        //TypeRecoded property
        public RamAccess<string> TypeRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TypeRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(TypeRecoded), value);
                }
                OnPropertyChanged(nameof(TypeRecoded));
            }
        }


        private bool TypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //TypeRecoded property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Radionuclids));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
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
                value.AddError( "Поле не заполнено");
return false;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
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
        [Attributes.Form_Property("Заводской номер")]
        public RamAccess<string> FactoryNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactoryNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FactoryNumber), value);
                }
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }


        private bool FactoryNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            return true;
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public RamAccess<string> FactoryNumberRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FactoryNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(FactoryNumberRecoded), value);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        //If change this change validation

        private bool FactoryNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //FactoryNumberRecoded property

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public RamAccess<string> Activity
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Activity));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }


        private bool Activity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }

            if (value.Value.Equals("прим."))
            {
                //if ((ActivityNote == null) || ActivityNote.Equals(""))
                //    value.AddError( "Заполните примечание");
return true;
            }
            if (!((value.Value.Contains('e'))|| (value.Value.Contains('E'))))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0)){value.AddError("Число должно быть больше нуля");return false;}
            }
            catch
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            return true;
        }
        //Activity property

        //ActivityNote property
        public RamAccess<string> ActivityNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ActivityNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {



                
                {
                    _dataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(ActivityNote));
            }
        }
        //If change this change validation

        private bool ActivityNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //ActivityNote property

        //CreationDate property
        [Attributes.Form_Property("Дата изготовления")]
        public RamAccess<string> CreationDate
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CreationDate));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                
                {
                    _dataAccess.Set(nameof(CreationDate), value);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation

        private bool CreationDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((CreationDateNote == null) || CreationDateNote.Equals(""))
                //    value.AddError( "Заполните примечание");
return true;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
return false;
            }
            return true;
        }
        //CreationDate property RDY

        //CreationDateNote property
        public RamAccess<string> CreationDateNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CreationDateNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(CreationDateNote), value);
                }
                OnPropertyChanged(nameof(CreationDateNote));
            }
        }
        //If change this change validation


        private bool CreationDateNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //CreationDateNote property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
        public RamAccess<string> CreatorOKPO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CreatorOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(CreatorOKPO), value);
                }
                OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
        //If change this change validation

        private bool CreatorOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Value.Equals("")))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((CreatorOKPONote.Value == null) || CreatorOKPONote.Value.Equals(""))
                //    value.AddError( "Заполните примечание");
return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;

            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //CreatorOKPO property

        //CreatorOKPONote property
        public RamAccess<string> CreatorOKPONote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(CreatorOKPONote));//OK
                }
                
                {
                    
                }
            }
            set
            {



                
                {
                    _dataAccess.Set(nameof(CreatorOKPONote), value);
                }
                OnPropertyChanged(nameof(CreatorOKPONote));
            }
        }


        private bool CreatorOKPONote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //CreatorOKPONote property

        //AggregateState property
        [Attributes.Form_Property("Агрегатное состояние")]
        public RamAccess<byte?> AggregateState//1 2 3
        {
            get
            {
                
                {
                    return _dataAccess.Get<byte?>(nameof(AggregateState));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(AggregateState), value);
                }
                OnPropertyChanged(nameof(AggregateState));
            }
        }


        private bool AggregateState_Validation(RamAccess<byte?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            if ((value.Value != 1) && (value.Value != 2) && (value.Value != 3))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
                return true;
        }
        //AggregateState property

        //PropertyCode property
        [Attributes.Form_Property("Код собственности")]
        public RamAccess<byte?> PropertyCode
        {
            get
            {
                
                {
                    return _dataAccess.Get<byte?>(nameof(PropertyCode));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PropertyCode), value);
                }
                OnPropertyChanged(nameof(PropertyCode));
            }
        }


        private bool PropertyCode_Validation(RamAccess<byte?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value == null)//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (!((value.Value >= 1) && (value.Value <= 9)))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //PropertyCode property

        //Owner property
        [Attributes.Form_Property("Владелец")]
        public RamAccess<string> Owner
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Owner));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(Owner), value);
                }
                OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation

        private bool Owner_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((OwnerNote.Value == null) || OwnerNote.Value.Equals(""))
                //    value.AddError( "Заполните примечание");
return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;

            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Owner property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
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
            if (value.Value.Equals("прим."))
            {
                //if ((ProviderOrRecieverOKPONote.Value == null) || ProviderOrRecieverOKPONote.Value.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            bool a = (OperationCode.Value >= 10) && (OperationCode.Value <= 12);
            bool b = (OperationCode.Value >= 41) && (OperationCode.Value <= 43);
            bool c = (OperationCode.Value >= 71) && (OperationCode.Value <= 73);
            bool d = (OperationCode.Value == 15) || (OperationCode.Value == 17) || (OperationCode.Value == 18) || (OperationCode.Value == 46) ||
                (OperationCode.Value == 47) || (OperationCode.Value == 48) || (OperationCode.Value == 53) || (OperationCode.Value == 54) ||
                (OperationCode.Value == 58) || (OperationCode.Value == 61) || (OperationCode.Value == 62) || (OperationCode.Value == 65) ||
                (OperationCode.Value == 67) || (OperationCode.Value == 68) || (OperationCode.Value == 75) || (OperationCode.Value == 76);
            if (a || b || c || d)
            {
                ProviderOrRecieverOKPO.Value = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                return false;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;

            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public RamAccess<string> ProviderOrRecieverOKPONote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }


        private bool ProviderOrRecieverOKPONote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            return true;
        }
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        [Attributes.Form_Property("ОКПО перевозчика")]
        public RamAccess<string> TransporterOKPO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TransporterOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(TransporterOKPO), value);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }


        private bool TransporterOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((TransporterOKPONote.Value == null) || TransporterOKPONote.Value.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;

            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public RamAccess<string> TransporterOKPONote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(TransporterOKPONote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(TransporterOKPONote), value);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }


        private bool TransporterOKPONote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //TransporterOKPONote property

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public RamAccess<string> PackName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackName));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }


        private bool PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNameNote == null) || PackNameNote.Equals(""))
                //    value.AddError( "Заполните примечание");//to do note handling
return true;
            }
            return true;
        }
        //PackName property

        //PackNameNote property
        public RamAccess<string> PackNameNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNameNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PackNameNote), value);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }


        private bool PackNameNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            return true;
        }
        //PackNameNote property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public RamAccess<string> PackType
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackType));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PackType), value);
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
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                //    value.AddError( "Заполните примечание");//to do note handling
return true;
            }
            return true;
        }
        //PackType property

        //PackTypeRecoded property
        public RamAccess<string> PackTypeRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackTypeRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }


        private bool PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PackTypeRecoded property

        //PackTypeNote property
        public RamAccess<string> PackTypeNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackTypeNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PackTypeNote), value);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }


        private bool PackTypeNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;}
        //PackTypeNote property

        //PackNumber property
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PackNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PackNumber), value);
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
                value.AddError( "Поле не заполнено");
return false;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                //    value.AddError( "Заполните примечание");//to do note handling
return true;
            }
            return true;
        }
        //PackNumber property

        //PackNumberNote property
        public RamAccess<string> PackNumberNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNumberNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(PackNumberNote), value);
                }
                OnPropertyChanged(nameof(PackNumberNote));
            }
        }


        private bool PackNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            return true;
        }
        //PackNumberNote property

        //DocumentNumberNote property
        public RamAccess<string> DocumentNumberNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(DocumentNumberNote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {


                
                {
                    _dataAccess.Set(nameof(DocumentNumberNote), value);
                }
                OnPropertyChanged(nameof(DocumentNumberNote));
            }
        }


        private bool DocumentNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
return false;
            }
            return true;
        }
        //DocumentNumberNote property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PackNumberRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation

        private bool PackNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;}
        //PackNumberRecoded property

        protected override bool DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value == "прим.")
            {
                //if ((DocumentNumberNote == null) || DocumentNumberNote.Equals(""))
                //    value.AddError( "Заполните примечание");
                return true;
            }
            if ((value.Value == null))//ok
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
    }
}
