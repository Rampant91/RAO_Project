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
            FormNum = "11";
            NumberOfFields = 42;
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
            PackTypeRecoded_Validation();
            PackType_Validation(PackType);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
            PassportNumber_Validation(PassportNumber);
            PropertyCode_Validation(PropertyCode);
            ProviderOrRecieverOKPONote_Validation();
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            Quantity_Validation(Quantity);
            Radionuclids_Validation(Radionuclids);
            SignedServicePeriod_Validation(SignedServicePeriod);
            TransporterOKPONote_Validation();
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
        public IDataAccess<string> PassportNumber
        {
            get
            {
                return _dataAccess.Get<string><string>(nameof(PassportNumber));//OK
            }
            set
            {
                _dataAccess.Set(nameof(PassportNumber), value);
                OnPropertyChanged(nameof(PassportNumber));
            }
        }
        private IDataAccess<string> _PassportNumber_Not_Valid = "";
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
        public IDataAccess<string> PassportNumberNote
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PassportNumberNote));//OK
            }
            set
            {
                

                
                {
                    _dataAccess.Set(nameof(PassportNumberNote), value);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }

        private IDataAccess<string> _PassportNumberNote_Not_Valid = "";
        private void PassportNumberNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public IDataAccess<string> PassportNumberRecoded
        {
            get
            {
                return _dataAccess.Get<string>(nameof(PassportNumberRecoded));//OK 
            }
            set
            {
                _PassportNumberRecoded_Not_Valid = value;
                
                {
                    _dataAccess.Set(nameof(PassportNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation
        private IDataAccess<string> _PassportNumberRecoded_Not_Valid = "";
        private void PassportNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //PassportNumberRecoded property

        //Type property
        [Attributes.Form_Property("Тип")]
        public IDataAccess<string> Type
        {
            get
            {
                return= _dataAccess.Get<string>(nameof(Type));//OK
                else
                {
                    return _Type_Not_Valid;
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

        private IDataAccess<string> _Type_Not_Valid = "";
        private void Type_Validation(IDataAccess<string> value)
        {
            ClearErrors(nameof(Type));
            List<string> spr = new List<string>
            {
                "ГИИД-6",
                "ГИИД-5"
            };    //HERE BINDS SPRAVOCHNIK
            foreach (var item in spr)
            {
                if (item.Equals(value)) return;
            }
            value.AddError( "Недопустимое значение");
        }
        //Type property

        //TypeRecoded property
        public IDataAccess<string> TypeRecoded
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(TypeRecoded));//OK
                    
                }
                else
                {
                    return _TypeRecoded_Not_Valid;
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

        private IDataAccess<string> _TypeRecoded_Not_Valid = "";
        private void TypeRecoded_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //TypeRecoded property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public IDataAccess<string> Radionuclids
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(Radionuclids));//OK
                    
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                
                //_Radionuclids_Not_Valid = value;
                
                
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        private IDataAccess<string> _Radionuclids_Not_Valid = "";
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
                    Radionuclids = item.Item2;
                    return;
                }
            }
        }
        //Radionuclids property

        //FactoryNumber property
        [Attributes.Form_Property("Заводской номер")]
        public IDataAccess<string> FactoryNumber
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(FactoryNumber));//OK
                    
                }
                else
                {
                    return _FactoryNumber_Not_Valid;
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

        private IDataAccess<string> _FactoryNumber_Not_Valid = "";
        private void FactoryNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value== null) || value.ValueEquals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public IDataAccess<string> FactoryNumberRecoded
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(FactoryNumberRecoded));//OK
                    
                }
                else
                {
                    return _FactoryNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _FactoryNumberRecoded_Not_Valid = value;
                
                {
                    _dataAccess.Set(nameof(FactoryNumberRecoded), value);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        //If change this change validation
        private IDataAccess<string> _FactoryNumberRecoded_Not_Valid = "";
        private void FactoryNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //FactoryNumberRecoded property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int? Quantity
        {
            get
            {
                
                {
                    return (int?)_dataAccess.Get<string>(nameof(Quantity));//OK;
                }
                else
                {
                    return _Quantity_Not_Valid;
                }
            }
            set
            {
                

                
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.
        private int? _Quantity_Not_Valid = null;
        private void Quantity_Validation(int? value)//Ready
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
        public IDataAccess<string> Activity
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(Activity));//OK
                    
                }
                else
                {
                    return _Activity_Not_Valid;
                }
            }
            set
            {
                
                //_Activity_Not_Valid = value;

                
                {
                    _dataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }

        private IDataAccess<string> _Activity_Not_Valid = "";
        private void Activity_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value== null) || value.ValueEquals(""))
            {
                value.AddError("Поле не заполнено");
                return;
            }
            if (!((value.ValueContains('e')|| value.ValueContains('E'))))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            if (value.ValueEquals("прим."))
            {
                if ((Activity == null) || (ActivityNote == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //Activity property

        //ActivityNote property
        public IDataAccess<string> ActivityNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(ActivityNote));//OK
                    
                }
                else
                {
                    return _ActivityNote_Not_Valid;
                }
            }
            set
            {
                
                //_ActivityNote_Not_Valid = value;
                
                
                {
                    _dataAccess.Set(nameof(ActivityNote), value);
                }
                OnPropertyChanged(nameof(ActivityNote));
            }
        }
        //If change this change validation
        private IDataAccess<string> _ActivityNote_Not_Valid = "";
        private void ActivityNote_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //ActivityNote property

        //CreationDate property
        [Attributes.Form_Property("Дата изготовления")]
        public IDataAccess<string> CreationDate
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(CreationDate));//OK
                    
                }
                else
                {
                    return _CreationDate_Not_Valid;
                }
            }
            set
            {
                
                //_CreationDate_Not_Valid = value;
                
                {
                    _dataAccess.Set(nameof(CreationDate), value);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation
        private IDataAccess<string> _CreationDate_Not_Valid = "";
        private void CreationDate_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value== null) || value.ValueEquals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.ValueEquals("прим."))
            {
                if ((CreationDateNote == null) || (CreationDateNote == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value); }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
        }
        //CreationDate property

        //CreationDateNote property
        public IDataAccess<string> CreationDateNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(CreationDateNote));//OK
                    
                }
                else
                {
                    return _CreationDateNote_Not_Valid;
                }
            }
            set
            {
                
                //_CreationDateNote_Not_Valid = value;
                
                {
                    _dataAccess.Set(nameof(CreationDateNote), value);
                }
                OnPropertyChanged(nameof(CreationDateNote));
            }
        }
        //If change this change validation
        private IDataAccess<string> _CreationDateNote_Not_Valid = "";

        private void CreationDateNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //CreationDateNote property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
        public IDataAccess<string> CreatorOKPO
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(CreatorOKPO));//OK
                    
                }
                else
                {
                    return _CreatorOKPO_Not_Valid;
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
        private IDataAccess<string> _CreatorOKPO_Not_Valid = "";
        private void CreatorOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value== null) || (value.ValueEquals("")))
            {
                value.AddError("Поле не заполнено");
                return;
            }
            if (value.ValueEquals("прим."))
            {
                if ((PassportNumberNote == null) || (PassportNumberNote == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (OKSM.Contains(value)) return;
            if ((value.ValueLength != 8) && (value.ValueLength != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //CreatorOKPO property

        //CreatorOKPONote property
        public IDataAccess<string> CreatorOKPONote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(CreatorOKPONote));//OK
                    
                }
                else
                {
                    return _CreatorOKPONote_Not_Valid;
                }
            }
            set
            {
                
                //_CreatorOKPONote_Not_Valid = value;
                
                
                {
                    _dataAccess.Set(nameof(CreatorOKPONote), value);
                }
                OnPropertyChanged(nameof(CreatorOKPONote));
            }
        }

        private IDataAccess<string> _CreatorOKPONote_Not_Valid = "";
        private void CreatorOKPONote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //CreatorOKPONote property

        //Category property
        [Attributes.Form_Property("Категория")]
        public short Category
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(Category));//OK
                    
                }
                else
                {
                    return _Category_Not_Valid;
                }
            }
            set
            {
                

                
                {
                    _dataAccess.Set(nameof(Category), value);
                }
                OnPropertyChanged(nameof(Category));
            }
        }

        private short _Category_Not_Valid = -1;
        private void Category_Validation(short value)//TODO
        {
            value.ClearErrors();
            if ((value.Value< 1) || (value.Value> 5))
                value.AddError( "Недопустимое значение");
        }
        //Category property

        //SignedServicePeriod property
        [Attributes.Form_Property("НСС, мес.")]
        public float SignedServicePeriod
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(SignedServicePeriod));//OK
                    
                }
                else
                {
                    return _SignedServicePeriod_Not_Valid;
                }
            }
            set
            {
                

                
                {
                    _dataAccess.Set(nameof(SignedServicePeriod), value);
                }
                OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }

        private float _SignedServicePeriod_Not_Valid = -1;
        private void SignedServicePeriod_Validation(float value)//Ready
        {
            value.ClearErrors();
            if (value.Value<= 0)
                value.AddError( "Недопустимое значение");
        }
        //SignedServicePeriod property

        //PropertyCode property
        [Attributes.Form_Property("Код собственности")]
        public byte PropertyCode
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(PropertyCode));//OK
                    
                }
                else
                {
                    return _PropertyCode_Not_Valid;
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

        private byte _PropertyCode_Not_Valid = 255;
        private void PropertyCode_Validation(byte value)//Ready
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
        public IDataAccess<string> OwnerNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(OwnerNote));//OK
                    
                }
                else
                {
                    return _OwnerNote_Not_Valid;
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
        private IDataAccess<string> _OwnerNote_Not_Valid = "";
        private void OwnerNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //OwnerNote property

        //Owner property
        [Attributes.Form_Property("Владелец")]
        public IDataAccess<string> Owner
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(Owner));//OK
                    
                }
                else
                {
                    return _Owner_Not_Valid;
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
        private IDataAccess<string> _Owner_Not_Valid = "";
        private void Owner_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value==null)||value.ValueEquals(_Owner_Not_Valid))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.ValueEquals("Минобороны")) return;
            if (value.ValueEquals("прим."))
            {
                if ((OwnerNote == null) || OwnerNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (OKSM.Contains(value)) return;
            if ((value.ValueLength != 8) && (value.ValueLength != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //Owner property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public IDataAccess<string> ProviderOrRecieverOKPO
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
                    
                }
                else
                {
                    return _ProviderOrRecieverOKPO_Not_Valid;
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

        private IDataAccess<string> _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value== null) || value.ValueEquals(_ProviderOrRecieverOKPO_Not_Valid))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.ValueEquals("Минобороны")) return;
            if (value.ValueEquals("прим."))
            {
                if ((ProviderOrRecieverOKPONote == null) || ProviderOrRecieverOKPONote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (OKSM.Contains(value)) return;
            if ((value.ValueLength != 8) && (value.ValueLength != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public IDataAccess<string> ProviderOrRecieverOKPONote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
                    
                }
                else
                {
                    return _ProviderOrRecieverOKPONote_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPONote_Not_Valid = value;
                
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }

        private IDataAccess<string> _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation()
        {
            value.ClearErrors();
        }
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        [Attributes.Form_Property("ОКПО перевозчика")]
        public IDataAccess<string> TransporterOKPO
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(TransporterOKPO));//OK
                    
                }
                else
                {
                    return _TransporterOKPO_Not_Valid;
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

        private IDataAccess<string> _TransporterOKPO_Not_Valid = "";
        private void TransporterOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value== null) || value.ValueEquals(_TransporterOKPO_Not_Valid))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.ValueEquals("-")) return;
            if (value.ValueEquals("прим."))
            {
                if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if ((value.ValueLength != 8) && (value.ValueLength != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public IDataAccess<string> TransporterOKPONote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(TransporterOKPONote));//OK
                    
                }
                else
                {
                    return _TransporterOKPONote_Not_Valid;
                }
            }
            set
            {
                _TransporterOKPONote_Not_Valid = value;
                
                {
                    _dataAccess.Set(nameof(TransporterOKPONote), value);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }

        private IDataAccess<string> _TransporterOKPONote_Not_Valid = "";
        private void TransporterOKPONote_Validation()
        {
            value.ClearErrors();
        }
        //TransporterOKPONote property

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public IDataAccess<string> PackName
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackName));//OK
                    
                }
                else
                {
                    return _PackName_Not_Valid;
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

        private IDataAccess<string> _PackName_Not_Valid = "";
        private void PackName_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value== null) || value.ValueEquals(_PackName_Not_Valid))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.ValueEquals("прим."))
            {
                if ((PackNameNote == null) || PackNameNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
        }
        //PackName property

        //PackNameNote property
        public IDataAccess<string> PackNameNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackNameNote));//OK
                    
                }
                else
                {
                    return _PackNameNote_Not_Valid;
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

        private IDataAccess<string> _PackNameNote_Not_Valid = "";
        private void PackNameNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackNameNote property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public IDataAccess<string> PackType
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackType));//OK
                    
                }
                else
                {
                    return _PackType_Not_Valid;
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
        private IDataAccess<string> _PackType_Not_Valid = "";
        private void PackType_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value== null) || value.ValueEquals(_PackType_Not_Valid))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.ValueEquals("прим."))
            {
                if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
        }
        //PackType property

        //PackTypeRecoded property
        public IDataAccess<string> PackTypeRecoded
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackTypeRecoded));//OK
                    
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                _PackTypeRecoded_Not_Valid = value;
                
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        private IDataAccess<string> _PackTypeRecoded_Not_Valid = "";
        private void PackTypeRecoded_Validation()
        {
            value.ClearErrors();
        }
        //PackTypeRecoded property

        //DocumentNumberNote property
        public IDataAccess<string> DocumentNumberNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(DocumentNumberNote));//OK
                    
                }
                else
                {
                    return _DocumentNumberNote_Not_Valid;
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

        private IDataAccess<string> _DocumentNumberNote_Not_Valid = "";
        private void DocumentNumberNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //DocumentNumberNote property

        //PackTypeNote property
        public IDataAccess<string> PackTypeNote
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackTypeNote));//OK
                    
                }
                else
                {
                    return _PackTypeNote_Not_Valid;
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

        private IDataAccess<string> _PackTypeNote_Not_Valid = "";
        private void PackTypeNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackTypeNote property

        //PackNumber property
        [Attributes.Form_Property("Номер упаковки")]
        public IDataAccess<string> PackNumber
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackNumber));//OK
                    
                }
                else
                {
                    return _PackNumber_Not_Valid;
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
        private IDataAccess<string> _PackNumber_Not_Valid = "";
        private void PackNumber_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value== null) || value.ValueEquals(_PackNumber_Not_Valid))//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.ValueEquals("прим."))
            {
                if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
        }
        //PackNumber property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public IDataAccess<string> PackNumberRecoded
        {
            get
            {
                
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackNumberRecoded));
                }
                else
                {
                    return _PackNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _PackNumberRecoded_Not_Valid = value;
                
                {
                    _dataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation
        private IDataAccess<string> _PackNumberRecoded_Not_Valid = "";
        private void PackNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //PackNumberRecoded property

        //PackNumberNote property
        public IDataAccess<string> PackNumberNote
        {
            get
            {
                var tmp = _dataAccess.Get<string>(nameof(PackNumberNote));//OK
            }
            set
            {
                _dataAccess.Set(nameof(PackNumberNote), value);
                OnPropertyChanged(nameof(PackNumberNote));
            }
        }

        private IDataAccess<string> _PackNumberNote_Not_Valid = "";
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
                if ((DocumentNumberNote == null) || DocumentNumberNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if ((value.Value== null) || value.ValueEquals(_DocumentNumber_Not_Valid))//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }

        protected override void OperationCode_Validation(short? value)//OK
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
            foreach(var item in spr)
            {
                if (item == value) flag = true; 
            }
            if (!flag)
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
