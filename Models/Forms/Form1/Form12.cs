using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.2: Сведения об изделиях из обедненного урана")]
    public class Form12 : Abstracts.Form1
    {
        public Form12() : base()
        {
            FormNum = "12";
            NumberOfFields = 35;
            Validate_all();
        }

        private void Validate_all()
        {
            CreationDateNote_Validation(CreationDate);
            CreationDate_Validation(CreationDate);
            CreatorOKPONote_Validation(CreatorOKPONote);
            CreatorOKPO_Validation(CreatorOKPO);
            FactoryNumberRecoded_Validation(FactoryNumberRecoded);
            Owner_Validation(Owner);
            OwnerNote_Validation(OwnerNote);
            PackName_Validation(PackName);
            PackNumberRecoded_Validation(PackNumberRecoded);
            PackNumber_Validation(PackNumber);
            PackNumberNote_Validation(PackNumberNote);
            PackTypeRecoded_Validation(PackTypeRecoded);
            PackType_Validation(PackType);
            PassportNumberRecoded_Validation(PassportNumberRecoded);
            PassportNumber_Validation(PassportNumber);
            PropertyCode_Validation(PropertyCode);
            ProviderOrRecieverOKPONote_Validation(ProviderOrRecieverOKPONote);
            ProviderOrRecieverOKPO_Validation(ProviderOrRecieverOKPO);
            SignedServicePeriod_Validation(SignedServicePeriod);
            TransporterOKPONote_Validation(TransporterOKPONote);
            TransporterOKPO_Validation(TransporterOKPO);
            FactoryNumber_Validation(FactoryNumber);
            Mass_Validation(Mass);
            NameIOU_Validation(NameIOU);
            PackNameNote_Validation(PackNameNote);
            PackTypeNote_Validation(PackTypeNote);
            DocumentNumberNote_Validation(DocumentNumberNote);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //DocumentNumberNote property
        public IDataAccess<string> DocumentNumberNote
        {
            get
            {
                if (GetErrors(nameof(DocumentNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(DocumentNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void DocumentNumberNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //DocumentNumberNote property

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public IDataAccess<string> PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PassportNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void PassportNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((PassportNumberNote == null) || (PassportNumberNote == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
        }
        //PassportNumber property

        //OwnerNote property
        public IDataAccess<string> OwnerNote
        {
            get
            {
                if (GetErrors(nameof(OwnerNote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(OwnerNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void OwnerNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //OwnerNote property

        protected override void OperationCode_Validation(IDataAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == _OperationCode_Not_Valid)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            bool flag = false;
            foreach (var item in spr)
            {
                if (item == value) flag = true;
            }
            if (!flag)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            if ((value==01) || (value==13) ||
            (value==14) || (value==16) ||
            (value==26) || (value==36) ||
            (value==44) || (value==45) ||
            (value==49) || (value==51) ||
            (value==52) || (value==55) ||
            (value==56) || (value==57) ||
            (value==59) || (value==76))
                value.AddError( "Код операции не может быть использован для РВ");
            return;
        }

        //PassportNumberNote property
        public IDataAccess<string> PassportNumberNote
        {
            get
            {
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PassportNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void PassportNumberNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PassportNumberNote property

        //PassportNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public IDataAccess<string> PassportNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PassportNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PassportNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void PassportNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //PassportNumberRecoded property

        //NameIOU property
        [Attributes.Form_Property("Наименование ИОУ")]
        public IDataAccess<string> NameIOU
        {
            get
            {
                if (GetErrors(nameof(NameIOU)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(NameIOU));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {
                NameIOU_Validation(value);

                if (GetErrors(nameof(NameIOU)) == null)
                {
                    _dataAccess.Set(nameof(NameIOU), value);
                }
                OnPropertyChanged(nameof(NameIOU));
            }
        }


        private void NameIOU_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
            }
        }
        //NameIOU property

        //FactoryNumber property
        [Attributes.Form_Property("Заводской номер")]
        public IDataAccess<string> FactoryNumber
        {
            get
            {
                if (GetErrors(nameof(FactoryNumber)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(FactoryNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void FactoryNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
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
                if (GetErrors(nameof(FactoryNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(FactoryNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void FactoryNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //FactoryNumberRecoded property

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public IDataAccess<string> Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(Mass));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {
                Mass_Validation(value);
                if (GetErrors(nameof(Mass)) == null)
                {
                    _dataAccess.Set(nameof(Mass), value);
                }
                OnPropertyChanged(nameof(Mass));
            }
        }


        private void Mass_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            string tmp = value;
            int len = tmp.Length;
            if ((tmp[0] == '(') && (tmp[len - 1] == ')'))
            {
                tmp = tmp.Remove(len - 1, 1);
                tmp = tmp.Remove(0, 1);
            }
            try
            {
                if (!(double.Parse(tmp) > 0))
                    value.AddError( "Число должно быть больше нуля");
            }
            catch (Exception)
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //Mass Property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
        public IDataAccess<string> CreatorOKPO
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPO)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(CreatorOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void CreatorOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || (value.Equals("")))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((CreatorOKPONote == null) || (CreatorOKPONote == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (OKSM.Contains(value)) return;
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //CreatorOKPO property

        //CreatorOKPONote property
        public IDataAccess<string> CreatorOKPONote
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(CreatorOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void CreatorOKPONote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //CreatorOKPONote property

        //CreationDate property
        [Attributes.Form_Property("Дата изготовления")]
        public IDataAccess<string> CreationDate
        {
            get
            {
                if (GetErrors(nameof(CreationDate)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(CreationDate));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {
                CreationDate_Validation(value);
                
                if (GetErrors(nameof(CreationDate)) == null)
                {
                    _dataAccess.Set(nameof(CreationDate), value);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation

        private void CreationDate_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((CreationDateNote == null) || (CreationDateNote == ""))
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
        public IDataAccess<string> CreationDateNote
        {
            get
            {
                if (GetErrors(nameof(CreationDateNote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(CreationDateNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {
                CreationDateNote_Validation(value);

                if (GetErrors(nameof(CreationDateNote)) == null)
                {
                    _dataAccess.Set(nameof(CreationDateNote), value);
                }
                OnPropertyChanged(nameof(CreationDateNote));
            }
        }
        //If change this change validation


        private void CreationDateNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //CreationDateNote property

        //SignedServicePeriod property
        [Attributes.Form_Property("НСС, мес.")]
        public float SignedServicePeriod
        {
            get
            {
                if (GetErrors(nameof(SignedServicePeriod)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(SignedServicePeriod));//OK
                    return tmp != null ? (float)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                SignedServicePeriod_Validation(value);

                if (GetErrors(nameof(SignedServicePeriod)) == null)
                {
                    _dataAccess.Set(nameof(SignedServicePeriod), value);
                }
                OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }


        private void SignedServicePeriod_Validation(float value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
                value.AddError( "Недопустимое значение");
        }
        //SignedServicePeriod property

        //PropertyCode property
        [Attributes.Form_Property("Код собственности")]
        public byte PropertyCode
        {
            get
            {
                if (GetErrors(nameof(PropertyCode)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PropertyCode));//OK
                    return tmp != null ? (byte)tmp : (byte)0;
                }
                else
                {
                    
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


        private void PropertyCode_Validation(byte value)//Ready
        {
            value.ClearErrors();
            //if (value.Value == 255)//ok
            //{
            //    value.AddError( "Поле не заполнено");
            //    return;
            //}
            if (!((value >= 1) && (value <= 9)))
                value.AddError( "Недопустимое значение");
        }
        //PropertyCode property

        //Owner property
        [Attributes.Form_Property("Владелец")]
        public IDataAccess<string> Owner
        {
            get
            {
                if (GetErrors(nameof(Owner)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(Owner));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void Owner_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("Минобороны")) return;
            if (value.Equals("прим."))
            {
                if ((OwnerNote == null) || (OwnerNote == ""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if (OKSM.Contains(value)) return;
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
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
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void ProviderOrRecieverOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((ProviderOrRecieverOKPONote == null) || ProviderOrRecieverOKPONote.Equals(""))
                    value.AddError( "Заполните примечания");
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
            if (value.Equals("Минобороны")) return;
            if (OKSM.Contains(value)) return;
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //ProviderOrRecieverOKPO property

        //ProviderOrRecieverOKPONote property
        public IDataAccess<string> ProviderOrRecieverOKPONote
        {
            get
            {
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void ProviderOrRecieverOKPONote_Validation(IDataAccess<string> value)
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
                if (GetErrors(nameof(TransporterOKPO)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(TransporterOKPO));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void TransporterOKPO_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("-")) return;
            if (value.Equals("прим."))
            {
                if ((TransporterOKPONote == null) || TransporterOKPONote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //TransporterOKPO property

        //TransporterOKPONote property
        public IDataAccess<string> TransporterOKPONote
        {
            get
            {
                if (GetErrors(nameof(TransporterOKPONote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(TransporterOKPONote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void TransporterOKPONote_Validation(IDataAccess<string> value)
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
                if (GetErrors(nameof(PackName)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackName));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void PackName_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
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
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackNameNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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
                if (GetErrors(nameof(PackType)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackType));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void PackType_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
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
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackTypeRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void PackTypeRecoded_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //PackTypeRecoded property

        //PackTypeNote property
        public IDataAccess<string> PackTypeNote
        {
            get
            {
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackTypeNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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
                if (GetErrors(nameof(PackNumber)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackNumber));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void PackNumber_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Equals("прим."))
            {
                if ((PackNumberNote == null) || PackNumberNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
        }
        //PackNumber property

        //PackNumberNote property
        public IDataAccess<string> PackNumberNote
        {
            get
            {
                if (GetErrors(nameof(PackNumberNote)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackNumberNote));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void PackNumberNote_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError("Поле не заполнено");
                return;
            }
        }
        //PackNumberNote property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public IDataAccess<string> PackNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackNumberRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void PackNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //PackNumberRecoded property

        protected override void DocumentNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if (value.Value == "прим.")
            {
                if ((DocumentNumberNote == null) || DocumentNumberNote.Equals(""))
                    value.AddError( "Заполните примечание");
                return;
            }
            if ((value.Value == null))//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }
    }
}
