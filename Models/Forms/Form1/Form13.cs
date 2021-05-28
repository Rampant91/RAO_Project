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
            FormNum = "13";
            NumberOfFields = 37;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        protected override void OperationCode_Validation(short arg)//OK
        {
            ClearErrors(nameof(OperationCode));
            string value1 = arg.ToString();
            value1 = (value1.Length == 1) ? "0" + value1 : value1;
            if (value1 == null)
            {
                AddError(nameof(OperationCode), "Недопустимое значение");
                return;
            }
            var value = short.Parse(value1);
            if (value == _OperationCode_Not_Valid)
                AddError(nameof(OperationCode), "Поле не заполнено");
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            bool flag = false;
            foreach (var item in spr)
            {
                if (item == value) flag = true;
            }
            if (!flag)
            {
                AddError(nameof(OperationCode), "Недопустимое значение");
                return;
            }
            if ((value==1) || (value==13) ||
            (value==14) || (value==16) ||
            (value==26) || (value==36) ||
            (value==44) || (value==45) ||
            (value==49) || (value==51) ||
            (value==52) || (value==55) ||
            (value==56) || (value==57) ||
            (value==59) || (value==76))
                AddError(nameof(OperationCode), "Код операции не может быть использован для РВ");
            return;
        }

        //OwnerNote property
        public string OwnerNote
        {
            get
            {
                if (GetErrors(nameof(OwnerNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(OwnerNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _OwnerNote_Not_Valid;
                }
            }
            set
            {
                OwnerNote_Validation(value);

                if (GetErrors(nameof(OwnerNote)) == null)
                {
                    _dataAccess.Set(nameof(OwnerNote), value);
                }
                OnPropertyChanged(nameof(OwnerNote));
            }
        }
        //if change this change validation
        private string _OwnerNote_Not_Valid = "";
        private void OwnerNote_Validation(string value)
        {
            ClearErrors(nameof(OwnerNote));
        }
        //OwnerNote property

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public string PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumber_Not_Valid;
                }
            }
            set
            {
                PassportNumber_Validation(value);

                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumber), value);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }

        private string _PassportNumber_Not_Valid = "";
        public void PassportNumber_Validation(string value)
        {
            ClearErrors(nameof(PassportNumber));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(PassportNumber), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((PassportNumberNote==null)||(PassportNumberNote == ""))
                    AddError(nameof(PassportNumberNote), "Заполните примечание");
            }
        }
        //PassportNumber property

        //PassportNumberNote property
        public string PassportNumberNote
        {
            get
            {
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumberNote_Not_Valid;
                }
            }
            set
            {
                PassportNumberNote_Validation(value);
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumberNote), value);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }

        private string _PassportNumberNote_Not_Valid = "";
        private void PassportNumberNote_Validation(string value)
        {
            ClearErrors(nameof(PassportNumberNote));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(PassportNumberNote),"Поле не заполнено");
                return;
            }
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PassportNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PassportNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PassportNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PassportNumberRecoded_Not_Valid;
                }
            }
            set
            {
                PassportNumberRecoded_Validation(value);
                if (GetErrors(nameof(PassportNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation
        private string _PassportNumberRecoded_Not_Valid = "";
        private void PassportNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PassportNumberRecoded));
        }
        //PassportNumberRecoded property

        //Type property
        [Attributes.Form_Property("Тип")]
        public string Type
        {
            get
            {
                if (GetErrors(nameof(Type)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Type));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Type_Not_Valid;
                }
            }
            set
            {
                Type_Validation(value);
                if (GetErrors(nameof(Type)) == null)
                {
                    _dataAccess.Set(nameof(Type), value);
                }
                OnPropertyChanged(nameof(Type));
            }
        }

        private string _Type_Not_Valid = "";
        private void Type_Validation(string value)
        {
            ClearErrors(nameof(Type));
        }
        //Type property

        //TypeRecoded property
        public string TypeRecoded
        {
            get
            {
                if (GetErrors(nameof(TypeRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(TypeRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _TypeRecoded_Not_Valid;
                }
            }
            set
            {
                TypeRecoded_Validation(value);
                if (GetErrors(nameof(TypeRecoded)) == null)
                {
                    _dataAccess.Set(nameof(TypeRecoded), value);
                }
                OnPropertyChanged(nameof(TypeRecoded));
            }
        }

        private string _TypeRecoded_Not_Valid = "";
        private void TypeRecoded_Validation(string value)
        {
            ClearErrors(nameof(TypeRecoded));
        }
        //TypeRecoded property

        //Radionuclids property
        [Attributes.Form_Property("Радионуклиды")]
        public string Radionuclids
        {
            get
            {
                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Radionuclids));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Radionuclids_Not_Valid;
                }
            }
            set
            {
                Radionuclids_Validation(value);

                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation
        private string _Radionuclids_Not_Valid = "";
        private void Radionuclids_Validation(string value)//TODO
        {
            ClearErrors(nameof(Radionuclids));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(Radionuclids), "Поле не заполнено");
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
        public string FactoryNumber
        {
            get
            {
                if (GetErrors(nameof(FactoryNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(FactoryNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _FactoryNumber_Not_Valid;
                }
            }
            set
            {
                FactoryNumber_Validation(value);
                if (GetErrors(nameof(FactoryNumber)) == null)
                {
                    _dataAccess.Set(nameof(FactoryNumber), value);
                }
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }

        private string _FactoryNumber_Not_Valid = "";
        private void FactoryNumber_Validation(string value)
        {
            ClearErrors(nameof(FactoryNumber));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(FactoryNumber), "Поле не заполнено");
                return;
            }
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public string FactoryNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(FactoryNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(FactoryNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _FactoryNumberRecoded_Not_Valid;
                }
            }
            set
            {
                FactoryNumberRecoded_Validation(value);
                if (GetErrors(nameof(FactoryNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(FactoryNumberRecoded), value);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }
        //If change this change validation
        private string _FactoryNumberRecoded_Not_Valid = "";
        private void FactoryNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(FactoryNumberRecoded));
        }
        //FactoryNumberRecoded property

        //Activity property
        [Attributes.Form_Property("Активность, Бк")]
        public string Activity
        {
            get
            {
                if (GetErrors(nameof(Activity)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Activity));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Activity_Not_Valid;
                }
            }
            set
            {
                Activity_Validation(value);
                if (GetErrors(nameof(Activity)) == null)
                {
                    _dataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }

        private string _Activity_Not_Valid = "";
        private void Activity_Validation(string value)//Ready
        {
            ClearErrors(nameof(Activity));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(Activity), "Поле не заполнено");
                return;
            }

            if (value.Equals("прим."))
            {
                if ((ActivityNote == null) || ActivityNote.Equals(""))
                    AddError(nameof(ActivityNote), "Заполните примечание");
                return;
            }
            if (!((value.Contains('e'))|| (value.Contains('E'))))
            {
                AddError(nameof(Activity), "Недопустимое значение");
                return;
            }
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

        //ActivityNote property
        public string ActivityNote
        {
            get
            {
                if (GetErrors(nameof(ActivityNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ActivityNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _ActivityNote_Not_Valid;
                }
            }
            set
            {
                ActivityNote_Validation(value);
                //_ActivityNote_Validation(value);

                if (GetErrors(nameof(ActivityNote)) == null)
                {
                    _dataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(ActivityNote));
            }
        }
        //If change this change validation
        private string _ActivityNote_Not_Valid = "";
        private void ActivityNote_Validation(string value)//Ready
        {
            ClearErrors(nameof(ActivityNote));
        }
        //ActivityNote property

        //CreationDate property
        [Attributes.Form_Property("Дата изготовления")]
        public string CreationDate
        {
            get
            {
                if (GetErrors(nameof(CreationDate)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CreationDate));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _CreationDate_Not_Valid;
                }
            }
            set
            {
                CreationDate_Validation(value);
                //_CreationDate_Validation(value);
                
                if (GetErrors(nameof(CreationDate)) == null)
                {
                    _dataAccess.Set(nameof(CreationDate), value);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation
        private string _CreationDate_Not_Valid = "01/01/1921";
        private void CreationDate_Validation(string value)//Ready
        {
            ClearErrors(nameof(CreationDate));
            if ((value == null) || value.Equals("01/01/1921"))
            {
                AddError(nameof(CreationDate), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((CreationDateNote == null) || CreationDateNote.Equals(""))
                    AddError(nameof(CreationDateNote), "Заполните примечание");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(CreationDate), "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value); }
            catch (Exception)
            {
                AddError(nameof(CreationDate), "Недопустимое значение");
                return;
            }
        }
        //CreationDate property RDY

        //CreationDateNote property
        public string CreationDateNote
        {
            get
            {
                if (GetErrors(nameof(CreationDateNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CreationDateNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _CreationDateNote_Not_Valid;
                }
            }
            set
            {
                CreationDateNote_Validation(value);
                //_CreationDateNote_Validation(value);
                if (GetErrors(nameof(CreationDateNote)) == null)
                {
                    _dataAccess.Set(nameof(CreationDateNote), value);
                }
                OnPropertyChanged(nameof(CreationDateNote));
            }
        }
        //If change this change validation
        private string _CreationDateNote_Not_Valid = "";

        private void CreationDateNote_Validation(string value)
        {
            ClearErrors(nameof(CreationDateNote));
        }
        //CreationDateNote property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
        public string CreatorOKPO
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPO)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CreatorOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _CreatorOKPO_Not_Valid;
                }
            }
            set
            {
                CreatorOKPO_Validation(value);

                if (GetErrors(nameof(CreatorOKPO)) == null)
                {
                    _dataAccess.Set(nameof(CreatorOKPO), value);
                }
                OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
        //If change this change validation
        private string _CreatorOKPO_Not_Valid = "";
        private void CreatorOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CreatorOKPO));
            if ((value == null) || (value.Equals("")))
            {
                AddError(nameof(CreatorOKPO), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((CreatorOKPONote == null) || CreatorOKPONote.Equals(""))
                    AddError(nameof(CreatorOKPONote), "Заполните примечание");
                return;
            }
            foreach (var item in OKSM)
            {
                if (value.Equals(item)) return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(CreatorOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(CreatorOKPO), "Недопустимое значение");
            }
        }
        //CreatorOKPO property

        //CreatorOKPONote property
        public string CreatorOKPONote
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(CreatorOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _CreatorOKPONote_Not_Valid;
                }
            }
            set
            {
                CreatorOKPONote_Validation(value);
                //_CreatorOKPONote_Validation(value);

                if (GetErrors(nameof(CreatorOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(CreatorOKPONote), value);
                }
                OnPropertyChanged(nameof(CreatorOKPONote));
            }
        }

        private string _CreatorOKPONote_Not_Valid = "";
        private void CreatorOKPONote_Validation(string value)
        {
            ClearErrors(nameof(CreatorOKPONote));
        }
        //CreatorOKPONote property

        //AggregateState property
        [Attributes.Form_Property("Агрегатное состояние")]
        public byte AggregateState//1 2 3
        {
            get
            {
                if (GetErrors(nameof(AggregateState)) == null)
                {
                    return (byte)_dataAccess.Get(nameof(AggregateState));
                }
                else
                {
                    return _AggregateState_Not_Valid;
                }
            }
            set
            {
                AggregateState_Validation(value);
                if (GetErrors(nameof(AggregateState)) == null)
                {
                    _dataAccess.Set(nameof(AggregateState), value);
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

        //PropertyCode property
        [Attributes.Form_Property("Код собственности")]
        public byte PropertyCode
        {
            get
            {
                if (GetErrors(nameof(PropertyCode)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PropertyCode));//OK
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
                {
                    return _PropertyCode_Not_Valid;
                }
            }
            set
            {
                PropertyCode_Validation(value);

                if (GetErrors(nameof(PropertyCode)) == null)
                {
                    _dataAccess.Set(nameof(PropertyCode), value);
                }
                OnPropertyChanged(nameof(PropertyCode));
            }
        }

        private byte _PropertyCode_Not_Valid = 255;
        private void PropertyCode_Validation(byte value)//Ready
        {
            ClearErrors(nameof(PropertyCode));
            //if (value == 255)//ok
            //{
            //    AddError(nameof(PropertyCode), "Поле не заполнено");
            //    return;
            //}
            if (!((value >= 1) && (value <= 9)))
                AddError(nameof(PropertyCode), "Недопустимое значение");
        }
        //PropertyCode property

        //Owner property
        [Attributes.Form_Property("Владелец")]
        public string Owner
        {
            get
            {
                if (GetErrors(nameof(Owner)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(Owner));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _Owner_Not_Valid;
                }
            }
            set
            {
                Owner_Validation(value);

                if (GetErrors(nameof(Owner)) == null)
                {
                    _dataAccess.Set(nameof(Owner), value);
                }
                OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation
        private string _Owner_Not_Valid = "";
        private void Owner_Validation(string value)//Ready
        {
            ClearErrors(nameof(Owner));
            if ((value == null) || value.Equals(_Owner_Not_Valid))
            {
                AddError(nameof(Owner), "Поле не заполнено");
                return;
            }
            if (value.Equals("Минобороны")) return;
            if (value.Equals("прим."))
            {
                if ((OwnerNote == null) || OwnerNote.Equals(""))
                    AddError(nameof(OwnerNote), "Заполните примечание");
                return;
            }
            foreach (var item in OKSM)
            {
                if (value.Equals(item)) return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(Owner), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(Owner), "Недопустимое значение");
            }
        }
        //Owner property

        //ProviderOrRecieverOKPO property
        [Attributes.Form_Property("ОКПО поставщика/получателя")]
        public string ProviderOrRecieverOKPO
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ProviderOrRecieverOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _ProviderOrRecieverOKPO_Not_Valid;
                }
            }
            set
            {
                ProviderOrRecieverOKPO_Validation(value);

                if (GetErrors(nameof(ProviderOrRecieverOKPO)) == null)
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }

        private string _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(ProviderOrRecieverOKPO));
            if ((value == null) || value.Equals(_ProviderOrRecieverOKPO_Not_Valid))
            {
                AddError(nameof(ProviderOrRecieverOKPO), "Поле не заполнено");
                return;
            }
            if (value.Equals("Минобороны")) return;
            if (value.Equals("прим."))
            {
                if ((ProviderOrRecieverOKPONote == null) || ProviderOrRecieverOKPONote.Equals(""))
                    AddError(nameof(ProviderOrRecieverOKPONote), "Заполните примечание");
                return;
            }
            bool a = (OperationCode >= 10) && (OperationCode <= 12);
            bool b = (OperationCode >= 41) && (OperationCode <= 43);
            bool c = (OperationCode >= 71) && (OperationCode <= 73);
            bool d = (OperationCode == 15) || (OperationCode == 17) || (OperationCode == 18) || (OperationCode == 46) ||
                (OperationCode == 47) || (OperationCode == 48) || (OperationCode == 53) || (OperationCode == 54) ||
                (OperationCode == 58) || (OperationCode == 61) || (OperationCode == 62) || (OperationCode == 65) ||
                (OperationCode == 67) || (OperationCode == 68) || (OperationCode == 75) || (OperationCode == 76);
            if (a || b || c || d)
            {
                ProviderOrRecieverOKPO = "ОКПО ОТЧИТЫВАЮЩЕЙСЯ ОРГ";
                return;
            }
            foreach (var item in OKSM)
            {
                if (item.Equals(value)) return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(ProviderOrRecieverOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(ProviderOrRecieverOKPO), "Недопустимое значение");
            }
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public string ProviderOrRecieverOKPONote
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(ProviderOrRecieverOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _ProviderOrRecieverOKPONote_Not_Valid;
                }
            }
            set
            {
                ProviderOrRecieverOKPONote_Validation(value);
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }

        private string _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation(string value)
        {
            ClearErrors(nameof(ProviderOrRecieverOKPONote));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(ProviderOrRecieverOKPONote), "Поле не заполнено");
                return;
            }
        }
        //ProviderOrRecieverOKPONote property

        //TransporterOKPO property
        [Attributes.Form_Property("ОКПО перевозчика")]
        public string TransporterOKPO
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPO)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(TransporterOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _TransporterOKPO_Not_Valid;
                }
            }
            set
            {
                TransporterOKPO_Validation(value);

                if (GetErrors(nameof(TransporterOKPO)) == null)
                {
                    _dataAccess.Set(nameof(TransporterOKPO), value);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }

        private string _TransporterOKPO_Not_Valid = "";
        private void TransporterOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransporterOKPO));
            if ((value == null) || value.Equals(_TransporterOKPO_Not_Valid))
            {
                AddError(nameof(TransporterOKPO), "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (value.Equals("прим."))
            {
                if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                    AddError(nameof(TransporterOKPONote), "Заполните примечание");
                return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                AddError(nameof(TransporterOKPO), "Недопустимое значение");
            else
            {
                var mask = new Regex("[0123456789_]*");
                if (!mask.IsMatch(value))
                    AddError(nameof(TransporterOKPO), "Недопустимое значение");
            }
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public string TransporterOKPONote
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(TransporterOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _TransporterOKPONote_Not_Valid;
                }
            }
            set
            {
                TransporterOKPONote_Validation(value);
                if (GetErrors(nameof(TransporterOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(TransporterOKPONote), value);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }

        private string _TransporterOKPONote_Not_Valid = "";
        private void TransporterOKPONote_Validation(string value)
        {
            ClearErrors(nameof(TransporterOKPONote));
        }
        //TransporterOKPONote property

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackName));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackName_Not_Valid;
                }
            }
            set
            {
                PackName_Validation(value);

                if (GetErrors(nameof(PackName)) == null)
                {
                    _dataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }

        private string _PackName_Not_Valid = "";
        private void PackName_Validation(string value)
        {
            ClearErrors(nameof(PackName));
            if ((value == null) || value.Equals(_PackName_Not_Valid))
            {
                AddError(nameof(PackName), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((PackNameNote == null) || PackNameNote.Equals(""))
                    AddError(nameof(PackNameNote), "Заполните примечание");
                return;
            }
        }
        //PackName property

        //PackNameNote property
        public string PackNameNote
        {
            get
            {
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNameNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNameNote_Not_Valid;
                }
            }
            set
            {
                PackNameNote_Validation(value);
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    _dataAccess.Set(nameof(PackNameNote), value);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }

        private string _PackNameNote_Not_Valid = "";
        private void PackNameNote_Validation(string value)
        {
            ClearErrors(nameof(PackNameNote));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(PackNameNote), "Поле не заполнено");
                return;
            }
        }
        //PackNameNote property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public string PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackType));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackType_Not_Valid;
                }
            }
            set
            {
                PackType_Validation(value);

                if (GetErrors(nameof(PackType)) == null)
                {
                    _dataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
        private string _PackType_Not_Valid = "";
        private void PackType_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackType));
            if ((value == null) || value.Equals(_PackType_Not_Valid))
            {
                AddError(nameof(PackType), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((PackTypeNote == null) || PackTypeNote.Equals(""))
                    AddError(nameof(PackTypeNote), "Заполните примечание");
                return;
            }
        }
        //PackType property

        //PackTypeRecoded property
        public string PackTypeRecoded
        {
            get
            {
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackTypeRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                PackTypeRecoded_Validation(value);
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        private string _PackTypeRecoded_Not_Valid = "";
        private void PackTypeRecoded_Validation(string value)
        {
            ClearErrors(nameof(PackTypeRecoded));
        }
        //PackTypeRecoded property

        //PackTypeNote property
        public string PackTypeNote
        {
            get
            {
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackTypeNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackTypeNote_Not_Valid;
                }
            }
            set
            {
                PackTypeNote_Validation(value);
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeNote), value);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }

        private string _PackTypeNote_Not_Valid = "";
        private void PackTypeNote_Validation(string value)
        {
            ClearErrors(nameof(PackTypeNote));
        }
        //PackTypeNote property

        //PackNumber property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumber
        {
            get
            {
                if (GetErrors(nameof(PackNumber)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNumber_Not_Valid;
                }
            }
            set
            {
                PackNumber_Validation(value);

                if (GetErrors(nameof(PackNumber)) == null)
                {
                    _dataAccess.Set(nameof(PackNumber), value);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation
        private string _PackNumber_Not_Valid = "";
        private void PackNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumber));
            if ((value == null) || value.Equals(_PackNumber_Not_Valid))//ok
            {
                AddError(nameof(PackNumber), "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                    AddError(nameof(PackNumberNote), "Заполните примечание");
                return;
            }
        }
        //PackNumber property

        //PackNumberNote property
        public string PackNumberNote
        {
            get
            {
                if (GetErrors(nameof(PackNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNumberNote_Not_Valid;
                }
            }
            set
            {
                PackNumberNote_Validation(value);

                if (GetErrors(nameof(PackNumberNote)) == null)
                {
                    _dataAccess.Set(nameof(PackNumberNote), value);
                }
                OnPropertyChanged(nameof(PackNumberNote));
            }
        }

        private string _PackNumberNote_Not_Valid = "";
        private void PackNumberNote_Validation(string value)
        {
            ClearErrors(nameof(PackNumberNote));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(PackNumberNote), "Поле не заполнено");
                return;
            }
        }
        //PackNumberNote property

        //DocumentNumberNote property
        public string DocumentNumberNote
        {
            get
            {
                if (GetErrors(nameof(DocumentNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(DocumentNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _DocumentNumberNote_Not_Valid;
                }
            }
            set
            {
                DocumentNumberNote_Validation(value);

                if (GetErrors(nameof(DocumentNumberNote)) == null)
                {
                    _dataAccess.Set(nameof(DocumentNumberNote), value);
                }
                OnPropertyChanged(nameof(DocumentNumberNote));
            }
        }

        private string _DocumentNumberNote_Not_Valid = "";
        private void DocumentNumberNote_Validation(string value)
        {
            ClearErrors(nameof(DocumentNumberNote));
            if ((value == null) || value.Equals(""))
            {
                AddError(nameof(DocumentNumberNote), "Поле не заполнено");
                return;
            }
        }
        //DocumentNumberNote property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackNumberRecoded_Not_Valid;
                }
            }
            set
            {
                PackNumberRecoded_Validation(value);
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation
        private string _PackNumberRecoded_Not_Valid = "";
        private void PackNumberRecoded_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumberRecoded));
        }
        //PackNumberRecoded property

        private List<string> OKSM = new List<string>
            {
                "АФГАНИСТАН",
                "АЛБАНИЯ",
                "АНТАРКТИДА",
                "АЛЖИР",
                "АМЕРИКАНСКОЕ САМОА",
                "АНДОРРА",
                "АНГОЛА",
                "АНТИГУА И БАРБУДА",
                "АЗЕРБАЙДЖАН",
                "АРГЕНТИНА",
                "АВСТРАЛИЯ",
                "АВСТРИЯ",
                "БАГАМЫ",
                "БАХРЕЙН",
                "БАНГЛАДЕШ",
                "АРМЕНИЯ",
                "БАРБАДОС",
                "БЕЛЬГИЯ",
                "БЕРМУДЫ",
                "БУТАН",
                "БОЛИВИЯ, МНОГОНАЦИОНАЛЬНОЕ ГОСУДАРСТВО",
                "БОСНИЯ И ГЕРЦЕГОВИНА",
                "БОТСВАНА",
                "ОСТРОВ БУВЕ",
                "БРАЗИЛИЯ",
                "БЕЛИЗ",
                "БРИТАНСКАЯ ТЕРРИТОРИЯ В ИНДИЙСКОМ ОКЕАНЕ",
                "СОЛОМОНОВЫ ОСТРОВА",
                "ВИРГИНСКИЕ ОСТРОВА (БРИТАНСКИЕ)",
                "БРУНЕЙ-ДАРУССАЛАМ",
                "БОЛГАРИЯ",
                "МЬЯНМА",
                "БУРУНДИ",
                "БЕЛАРУСЬ",
                "КАМБОДЖА",
                "КАМЕРУН",
                "КАНАДА",
                "КАБО-ВЕРДЕ",
                "ОСТРОВА КАЙМАН",
                "ЦЕНТРАЛЬНО-АФРИКАНСКАЯ РЕСПУБЛИКА",
                "ШРИ-ЛАНКА",
                "ЧАД",
                "ЧИЛИ",
                "КИТАЙ",
                "ТАЙВАНЬ (КИТАЙ)",
                "ОСТРОВ РОЖДЕСТВА",
                "КОКОСОВЫЕ (КИЛИНГ) ОСТРОВА",
                "КОЛУМБИЯ",
                "КОМОРЫ",
                "МАЙОТТА",
                "КОНГО",
                "КОНГО, ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА",
                "ОСТРОВА КУКА",
                "КОСТА-РИКА",
                "ХОРВАТИЯ",
                "КУБА",
                "КИПР",
                "ЧЕХИЯ",
                "БЕНИН",
                "ДАНИЯ",
                "ДОМИНИКА",
                "ДОМИНИКАНСКАЯ РЕСПУБЛИКА",
                "ЭКВАДОР",
                "ЭЛЬ-САЛЬВАДОР",
                "ЭКВАТОРИАЛЬНАЯ ГВИНЕЯ",
                "ЭФИОПИЯ",
                "ЭРИТРЕЯ",
                "ЭСТОНИЯ",
                "ФАРЕРСКИЕ ОСТРОВА",
                "ФОЛКЛЕНДСКИЕ ОСТРОВА (МАЛЬВИНСКИЕ)",
                "ЮЖНАЯ ДЖОРДЖИЯ И ЮЖНЫЕ САНДВИЧЕВЫ ОСТРОВА",
                "ФИДЖИ",
                "ФИНЛЯНДИЯ",
                "ЭЛАНДСКИЕ ОСТРОВА",
                "ФРАНЦИЯ",
                "ФРАНЦУЗСКАЯ ГВИАНА",
                "ФРАНЦУЗСКАЯ ПОЛИНЕЗИЯ",
                "ФРАНЦУЗСКИЕ ЮЖНЫЕ ТЕРРИТОРИИ",
                "ДЖИБУТИ",
                "ГАБОН",
                "ГРУЗИЯ",
                "ГАМБИЯ",
                "ПАЛЕСТИНА, ГОСУДАРСТВО",
                "ГЕРМАНИЯ",
                "ГАНА",
                "ГИБРАЛТАР",
                "КИРИБАТИ",
                "ГРЕЦИЯ",
                "ГРЕНЛАНДИЯ",
                "ГРЕНАДА",
                "ГВАДЕЛУПА",
                "ГУАМ",
                "ГВАТЕМАЛА",
                "ГВИНЕЯ",
                "ГАЙАНА",
                "ГАИТИ",
                "ОСТРОВ ХЕРД И ОСТРОВА МАКДОНАЛЬД",
                "ПАПСКИЙ ПРЕСТОЛ (ГОСУДАРСТВО - ГОРОД ВАТИКАН)",
                "ГОНДУРАС",
                "ГОНКОНГ",
                "ВЕНГРИЯ",
                "ИСЛАНДИЯ",
                "ИНДИЯ",
                "ИНДОНЕЗИЯ",
                "ИРАН (ИСЛАМСКАЯ РЕСПУБЛИКА)",
                "ИРАК","ИРЛАНДИЯ",
                "ИЗРАИЛЬ","ИТАЛИЯ","КОТ Д'ИВУАР","ЯМАЙКА","ЯПОНИЯ",
                "КАЗАХСТАН","ИОРДАНИЯ","КЕНИЯ","КОРЕЯ, НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","КОРЕЯ, РЕСПУБЛИКА","КУВЕЙТ","КИРГИЗИЯ",
                "ЛАОССКАЯ НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ЛИВАН","ЛЕСОТО","ЛАТВИЯ","ЛИБЕРИЯ","ЛИВИЯ","ЛИХТЕНШТЕЙН",
                "ЛИТВА",
                "ЛЮКСЕМБУРГ",
                "МАКАО",
                "МАДАГАСКАР","МАЛАВИ",
                "МАЛАЙЗИЯ",
                "МАЛЬДИВЫ",
                "МАЛИ",
                "МАЛЬТА",
                "МАРТИНИКА",
                "МАВРИТАНИЯ",
                "МАВРИКИЙ",
                "МЕКСИКА",
                "МОНАКО",
                "МОНГОЛИЯ",
                "МОЛДОВА, РЕСПУБЛИКА",
                "ЧЕРНОГОРИЯ",
                "МОНТСЕРРАТ",
                "МАРОККО",
                "МОЗАМБИК",
                "ОМАН",
                "НАМИБИЯ",
                "НАУРУ",
                "НЕПАЛ",
                "НИДЕРЛАНДЫ",
                "КЮРАСАО",
                "АРУБА",
                "СЕН-МАРТЕН (нидерландская часть)",
                "БОНЭЙР, СИНТ-ЭСТАТИУС И САБА","НОВАЯ КАЛЕДОНИЯ","ВАНУАТУ","НОВАЯ ЗЕЛАНДИЯ","НИКАРАГУА","НИГЕР",
                "НИГЕРИЯ","НИУЭ","ОСТРОВ НОРФОЛК","НОРВЕГИЯ","СЕВЕРНЫЕ МАРИАНСКИЕ ОСТРОВА",
                "МАЛЫЕ ТИХООКЕАНСКИЕ ОТДАЛЕННЫЕ ОСТРОВА СОЕДИНЕННЫХ ШТАТОВ","МИКРОНЕЗИЯ, ФЕДЕРАТИВНЫЕ ШТАТЫ","МАРШАЛЛОВЫ ОСТРОВА",
                "ПАЛАУ","ПАКИСТАН","ПАНАМА","ПАПУА-НОВАЯ ГВИНЕЯ","ПАРАГВАЙ","ПЕРУ",
                "ФИЛИППИНЫ","ПИТКЕРН","ПОЛЬША","ПОРТУГАЛИЯ","ГВИНЕЯ-БИСАУ","ТИМОР-ЛЕСТЕ",
                "ПУЭРТО-РИКО","КАТАР","РЕЮНЬОН","РУМЫНИЯ","РОССИЯ","РУАНДА","СЕН-БАРТЕЛЕМИ",
                "СВЯТАЯ ЕЛЕНА, ОСТРОВ ВОЗНЕСЕНИЯ, ТРИСТАН-ДА-КУНЬЯ","СЕНТ-КИТС И НЕВИС","АНГИЛЬЯ","СЕНТ-ЛЮСИЯ",
                "СЕН-МАРТЕН (французская часть)","СЕН-ПЬЕР И МИКЕЛОН","СЕНТ-ВИНСЕНТ И ГРЕНАДИНЫ",
                "САН-МАРИНО","САН-ТОМЕ И ПРИНСИПИ","САУДОВСКАЯ АРАВИЯ","СЕНЕГАЛ","СЕРБИЯ","СЕЙШЕЛЫ",
                "СЬЕРРА-ЛЕОНЕ","СИНГАПУР","СЛОВАКИЯ","ВЬЕТНАМ","СЛОВЕНИЯ","СОМАЛИ","ЮЖНАЯ АФРИКА","ЗИМБАБВЕ","ИСПАНИЯ",
                "ЗАПАДНАЯ САХАРА","СУДАН","СУРИНАМ","ШПИЦБЕРГЕН И ЯН МАЙЕН","ЭСВАТИНИ","ШВЕЦИЯ","ШВЕЙЦАРИЯ",
                "СИРИЙСКАЯ АРАБСКАЯ РЕСПУБЛИКА","ТАДЖИКИСТАН","ТАИЛАНД","ТОГО","ТОКЕЛАУ","ТОНГА","ТРИНИДАД И ТОБАГО",
                "ОБЪЕДИНЕННЫЕ АРАБСКИЕ ЭМИРАТЫ","ТУНИС","ТУРЦИЯ","ТУРКМЕНИСТАН","ОСТРОВА ТЕРКС И КАЙКОС",
                "ТУВАЛУ","УГАНДА","УКРАИНА","СЕВЕРНАЯ МАКЕДОНИЯ","ЕГИПЕТ",
                "СОЕДИНЕННОЕ КОРОЛЕВСТВО","ГЕРНСИ","ДЖЕРСИ","ОСТРОВ МЭН","ТАНЗАНИЯ, ОБЪЕДИНЕННАЯ РЕСПУБЛИКА","СОЕДИНЕННЫЕ ШТАТЫ",
                "ВИРГИНСКИЕ ОСТРОВА (США)","БУРКИНА-ФАСО","УРУГВАЙ","УЗБЕКИСТАН",
                "ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)","УОЛЛИС И ФУТУНА","САМОА","ЙЕМЕН",
                "ЗАМБИЯ","АБХАЗИЯ","ЮЖНАЯ ОСЕТИЯ","ЮЖНЫЙ СУДАН"};
        private void DocumentDate_Validation(DateTimeOffset value)
        {
            ClearErrors(nameof(DocumentDate));
            short tmp = OperationCode;
            bool a = (tmp >= 11) && (tmp <= 18);
            bool b = (tmp >= 41) && (tmp <= 49);
            bool c = (tmp >= 51) && (tmp <= 59);
            bool d = (tmp == 65) || (tmp == 68);
            if (a || b || c || d)
                if (!value.Date.Equals(DateTimeOffset.Parse(OperationDate).Date))
                    AddError(nameof(DocumentDate), "Заполните примечание");
        }

        protected override void DocumentDate_Validation(string value)
        {
            ClearErrors(nameof(DocumentDate));
            if ((value == null) || value.Equals(_DocumentDate_Not_Valid))
            {
                AddError(nameof(DocumentDate), "Поле не заполнено");
                return;
            }
            var a = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{4}$");
            if (!a.IsMatch(value))
            {
                AddError(nameof(DocumentDate), "Недопустимое значение");
                return;
            }
            try { DateTimeOffset.Parse(value); }
            catch (Exception)
            {
                AddError(nameof(DocumentDate), "Недопустимое значение");
                return;
            }
            short tmp = OperationCode;
            bool af = (tmp >= 11) && (tmp <= 18);
            bool b = (tmp >= 41) && (tmp <= 49);
            bool c = (tmp >= 51) && (tmp <= 59);
            bool d = (tmp == 65) || (tmp == 68);
            if (af || b || c || d)
                if (!value.Equals(OperationDate))
                    AddError(nameof(DocumentDate), "Заполните примечание");
        }

        protected override void DocumentNumber_Validation(string value)
        {
            ClearErrors(nameof(DocumentNumber));
            if (value == "прим.")
            {
                if ((DocumentNumberNote == null) || DocumentNumberNote.Equals(""))
                    AddError(nameof(DocumentNumberNote), "Заполните примечание");
                return;
            }
            if ((value == null) || value.Equals(_DocumentNumber_Not_Valid))//ok
            {
                AddError(nameof(DocumentNumber), "Поле не заполнено");
                return;
            }
        }

        protected override void OperationDate_Validation(string value)
        {
            ClearErrors(nameof(OperationDate));
            if ((value == null) || value.Equals(_OperationDate_Not_Valid))
            {
                AddError(nameof(OperationDate), "Поле не заполнено");
                return;
            }
        }
    }
}
