using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.1: Сведения о ЗРИ")]
    public class Form11 : Abstracts.Form1
    {
        public Form11() : base()
        {
            FormNum.Value = "11";
            NumberOfFields.Value = 42;
            Validate_all();
        }

        private void Validate_all()
        {
            PackNumberNote_Validation(PackNumberNote);
            OwnerNote_Validation(OwnerNote);
            PackTypeNote_Validation(PackTypeNote);
            ActivityNote_Validation(ActivityNote);
            Activity_Validation(Activity);
            Category_Validation(Category);
            CreationDate_Validation(CreationDate);
            CreatorOKPONote_Validation(CreatorOKPONote);
            CreatorOKPO_Validation(CreatorOKPO);
            FactoryNumberRecoded_Validation(FactoryNumberRecoded);
            FactoryNumber_Validation(FactoryNumber);
            Owner_Validation(Owner);
            PackNameNote_Validation(PackNameNote);
            PackName_Validation(PackName);
            PackNumberRecoded_Validation(PackNumberRecoded);
            PackNumber_Validation(PackNumber);
            PackTypeNote_Validation(PackTypeNote);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackType_Validation(PackType);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
            PassportNumber_Validation(PassportNumber);
            PropertyCode_Validation(PropertyCode);
            ProviderOrRecieverOKPONote_Validation(ProviderOrRecieverOKPONote);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            Quantity_Validation(Quantity);
            Radionuclids_Validation(Radionuclids);
            SignedServicePeriod_Validation(SignedServicePeriod);
            TransporterOKPONote_Validation(TransporterOKPONote);
            TransporterOKPO_Validation(TransporterOKPO);
            TypeRecoded_Validation(TypeRecoded);
            Type_Validation(Type);
            CreationDateNote_Validation(CreationDateNote);
            PackNumberNote_Validation(PackNumberNote);
        }

        public override bool Object_Validation()
        {
            return false;
        }

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public RamAccess<string> PassportNumber
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PassportNumber));//OK
            }
            set
            {
                _dataAccess.Set(nameof(PassportNumber), value);
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
        public void PassportNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим."))
            {
                //if ((PassportNumberNote.Value==null)||(PassportNumberNote.Value == ""))
                //    value.AddError("Заполните примечание");
            }
        }

        //PassportNumber property

        //PassportNumberNote property
        public RamAccess<string> PassportNumberNote
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PassportNumberNote));//OK
            }
            set
            {
                _dataAccess.Set(nameof(PassportNumberNote), value);
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }

                private void PassportNumberNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PassportNumberRecoded
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PassportNumberRecoded));//OK 
            }
            set
            {
                    _dataAccess.Set(nameof(PassportNumberRecoded), value);
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation
                private void PassportNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //PassportNumberRecoded property

        //Type property
        [Attributes.Form_Property("Тип")]
        public RamAccess<string> Type
        {
            get
            {
                return _dataAccess.Get<string>(nameof(Type));//OK
            }
            set
            {
                

                
                {
                    _dataAccess.Set(nameof(Type), value);
                }
                OnPropertyChanged(nameof(Type));
            }
        }

                private void Type_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            List<string> spr = new List<string>
            {
                "ГИИД-6",
                "ГИИД-5"
            };    //HERE BINDS SPRAVOCHNIK
            if(!spr.Contains(value.Value))
                value.AddError( "Недопустимое значение");
        }
        //Type property

        //TypeRecoded property
        public RamAccess<string> TypeRecoded
        {
            get
            {
                return _dataAccess.Get<string>(nameof(TypeRecoded));//OK
            }
            set
            {
                _dataAccess.Set(nameof(TypeRecoded), value);
                OnPropertyChanged(nameof(TypeRecoded));
            }
        }

                private void TypeRecoded_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //TypeRecoded property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public RamAccess<string> Radionuclids
        {
            get
            {
                return _dataAccess.Get<string>(nameof(Radionuclids));//OK
            }
            set
            {
                _dataAccess.Set(nameof(Radionuclids), value);
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
                private void Radionuclids_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value== null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            List<Tuple<string, string>> spr = new List<Tuple<string, string>>();//Here binds spravochnik
            foreach (var item in spr)
            {
                if (item.Item1.Equals(Type))
                {
                    Radionuclids.Value = item.Item2;
                    return;
                }
            }
        }
        //Radionuclids property

        //FactoryNumber property
        [Attributes.Form_Property("Заводской номер")]
        public RamAccess<string> FactoryNumber
        {
            get
            {
                return _dataAccess.Get<string>(nameof(FactoryNumber));//OK
            }
            set
            {
                _dataAccess.Set(nameof(FactoryNumber), value);
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }

                private void FactoryNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value== null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public RamAccess<string> FactoryNumberRecoded
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(FactoryNumberRecoded));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(FactoryNumberRecoded), value);
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        //If change this change validation
                private void FactoryNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //FactoryNumberRecoded property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public RamAccess<int?> Quantity
        {
            get
            {
                    return _dataAccess.Get<int?>(nameof(Quantity));//OK;
            }
            set
            {
                    _dataAccess.Set(nameof(Quantity), value);
                OnPropertyChanged(nameof(Quantity));
            }
        }
        private void Quantity_Validation(IDataAccess<int?> value)//Ready
        {
            value.ClearErrors();
            if (value.Value== null)
            {
                value.AddError( "Поле не заполнено");
            }
            if (value.Value<= 0)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //Quantity property

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public RamAccess<string> Activity
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(Activity));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(Activity), value);
                OnPropertyChanged(nameof(Activity));
            }
        }

        private void Activity_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value== null) || value.Value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return;
            }
            if (!((value.Value.Contains('e')|| value.Value.Contains('E'))))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            if (value.Value.Equals("прим."))
            {
                if ((Activity.Value == null) || (ActivityNote.Value == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //Activity property

        //ActivityNote property
        public RamAccess<string> ActivityNote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(ActivityNote));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(ActivityNote), value);
                OnPropertyChanged(nameof(ActivityNote));
            }
        }
        //If change this change validation
        private void ActivityNote_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //ActivityNote property

        //CreationDate property
        [Attributes.Form_Property("Дата изготовления")]
        public RamAccess<string> CreationDate
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(CreationDate));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(CreationDate), value);
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation
        private void CreationDate_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value== null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим."))
            {
                if ((CreationDateNote.Value == null) || (CreationDateNote.Value == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value.Value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //CreationDate property

        //CreationDateNote property
        public RamAccess<string> CreationDateNote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(CreationDateNote));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(CreationDateNote), value);
                OnPropertyChanged(nameof(CreationDateNote));
            }
        }
        //If change this change validation

        private void CreationDateNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //CreationDateNote property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
        public RamAccess<string> CreatorOKPO
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(CreatorOKPO));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(CreatorOKPO), value);
                OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
        //If change this change validation
        private void CreatorOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value== null) || (value.Value.Equals("")))
            {
                value.AddError("Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим."))
            {
                if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (OKSM.Contains(value.Value)) return;
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
                value.AddError( "Недопустимое значение");
            
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //CreatorOKPO property

        //CreatorOKPONote property
        public RamAccess<string> CreatorOKPONote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(CreatorOKPONote));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(CreatorOKPONote), value);
                OnPropertyChanged(nameof(CreatorOKPONote));
            }
        }

        private void CreatorOKPONote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //CreatorOKPONote property

        //Category property
        [Attributes.Form_Property("Категория")]
        public RamAccess<short> Category
        {
            get
            {
                    return _dataAccess.Get<short>(nameof(Category));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(Category), value);
                OnPropertyChanged(nameof(Category));
            }
        }

        private void Category_Validation(IDataAccess<short> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value< 1) || (value.Value> 5))
                value.AddError( "Недопустимое значение");
        }
        //Category property

        //SignedServicePeriod property
        [Attributes.Form_Property("НСС, мес.")]
        public RamAccess<float> SignedServicePeriod
        {
            get
            {
                    return _dataAccess.Get<float>(nameof(SignedServicePeriod));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(SignedServicePeriod), value);
                OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }

        private void SignedServicePeriod_Validation(IDataAccess<float> value)//Ready
        {
            value.ClearErrors();
            if (value.Value<= 0)
                value.AddError( "Недопустимое значение");
        }
        //SignedServicePeriod property

        //PropertyCode property
        [Attributes.Form_Property("Код собственности")]
        public RamAccess<byte> PropertyCode
        {
            get
            {
                    return _dataAccess.Get<byte>(nameof(PropertyCode));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(PropertyCode), value);
                OnPropertyChanged(nameof(PropertyCode));
            }
        }

        private void PropertyCode_Validation(IDataAccess<byte> value)//Ready
        {
            value.ClearErrors();
            //if (value.Value== 255)//ok
            //{
            //    value.AddError( "Поле не заполнено");
            //    return;
            //}
            if (!((value.Value>= 1) && (value.Value<= 9)))
                value.AddError( "Недопустимое значение");
        }
        //PropertyCode property

        //OwnerNote property
        public RamAccess<string> OwnerNote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(OwnerNote));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(OwnerNote), value);
                OnPropertyChanged(nameof(OwnerNote));
            }
        }
        //if change this change validation
        private void OwnerNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //OwnerNote property

        //Owner property
        [Attributes.Form_Property("Владелец")]
        public RamAccess<string> Owner
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(Owner));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(Owner), value);
                OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation
                private void Owner_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value==null)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("Минобороны")) return;
            if (value.Value.Equals("прим."))
            {
                if ((OwnerNote == null) || OwnerNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (OKSM.Contains(value.Value)) return;
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
                value.AddError( "Недопустимое значение");
            
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //Owner property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public RamAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }

        private void ProviderOrRecieverOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value== null)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("Минобороны")) return;
            if (value.Value.Equals("прим."))
            {
                if ((ProviderOrRecieverOKPONote == null) || ProviderOrRecieverOKPONote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (OKSM.Contains(value.Value)) return;
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
                value.AddError( "Недопустимое значение");
            
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public RamAccess<string> ProviderOrRecieverOKPONote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }

                private void ProviderOrRecieverOKPONote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        [Attributes.Form_Property("ОКПО перевозчика")]
        public RamAccess<string> TransporterOKPO
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(TransporterOKPO));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(TransporterOKPO), value);
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }

                private void TransporterOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if (value.Value== null)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("-")) return;
            if (value.Value.Equals("прим."))
            {
                if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
                value.AddError( "Недопустимое значение");
            
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public RamAccess<string> TransporterOKPONote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(TransporterOKPONote));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(TransporterOKPONote), value);
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }

                private void TransporterOKPONote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //TransporterOKPONote property

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public RamAccess<string> PackName
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(PackName));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(PackName), value);
                OnPropertyChanged(nameof(PackName));
            }
        }

                private void PackName_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value== null)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим."))
            {
                if ((PackNameNote == null) || PackNameNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
        }
        //PackName property

        //PackNameNote property
        public RamAccess<string> PackNameNote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(PackNameNote));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(PackNameNote), value);
                OnPropertyChanged(nameof(PackNameNote));
            }
        }

                private void PackNameNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackNameNote property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public RamAccess<string> PackType
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(PackType));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(PackType), value);
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
                private void PackType_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value== null)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим."))
            {
                if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
        }
        //PackType property

        //PackTypeRecoded property
        public RamAccess<string> PackTypeRecoded
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(PackTypeRecoded));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

                private void PackTypeRecoded_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackTypeRecoded property

        //DocumentNumberNote property
        public RamAccess<string> DocumentNumberNote
        {
            get
            {
                    return _dataAccess.Get<string>(nameof(DocumentNumberNote));//OK
            }
            set
            {
                    _dataAccess.Set(nameof(DocumentNumberNote), value);
                OnPropertyChanged(nameof(DocumentNumberNote));
            }
        }

                private void DocumentNumberNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //DocumentNumberNote property

        //PackTypeNote property
        public RamAccess<string> PackTypeNote
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PackTypeNote));//OK
            }
            set
            {
                _dataAccess.Set(nameof(PackTypeNote), value);
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }

                private void PackTypeNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackTypeNote property

        //PackNumber property
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PackNumber
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PackNumber));//OK
            }
            set
            {
                _dataAccess.Set(nameof(PackNumber), value);
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation
                private void PackNumber_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (value.Value== null)//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим."))
            {
                if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
        }
        //PackNumber property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PackNumberRecoded
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PackNumberRecoded));
            }
            set
            {
                    _dataAccess.Set(nameof(PackNumberRecoded), value);
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation
                private void PackNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //PackNumberRecoded property

        //PackNumberNote property
        public RamAccess<string> PackNumberNote
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PackNumberNote));//OK
            }
            set
            {
                _dataAccess.Set(nameof(PackNumberNote), value);
                OnPropertyChanged(nameof(PackNumberNote));
            }
        }

                private void PackNumberNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackNumberNote property

        protected override void DocumentNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value== "прим.")
            {
                if ((DocumentNumberNote.Value == null) || DocumentNumberNote.Value.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (value.Value== null)//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }

        protected override void OperationCode_Validation(IDataAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value== null)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            List<short> spr = new List<short>()
            {
                1,10,11,12,13,14,15,16,17,18,21,22,25,26,27,28,29,31,32,35,36,37,38,39,41,42,43,44,45,
                46,47,48,49,51,52,53,54,55,56,57,58,59,61,62,63,64,65,66,67,68,71,72,73,74,75,76,81,82,
                83,84,85,86,87,88,97,98,99
            };    //HERE BINDS SPRAVOCHNIK
            bool flag = false;
            if (!spr.Contains((short)value.Value))
            {
                value.AddError("Недопустимое значение");
                return;
            }
            if ((value.Value== 1) || (value.Value== 13) ||
                (value.Value== 14) || (value.Value== 16) ||
                (value.Value== 26) || (value.Value== 36) ||
                (value.Value== 44) || (value.Value== 45) ||
                (value.Value== 49) || (value.Value== 51) ||
                (value.Value== 52) || (value.Value== 55) ||
                (value.Value== 56) || (value.Value== 57) ||
                (value.Value== 59) || (value.Value== 76))
                value.AddError( "Код операции не может быть использован для РВ");
            return;
        }
    }
}
