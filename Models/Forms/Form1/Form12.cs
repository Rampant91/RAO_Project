using Collections.Rows_Collection;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.2: Сведения об изделиях из обедненного урана")]
    public class Form12 : Abstracts.Form1
    {
        public Form12(IDataAccess Access) : base(Access)
        {
            FormNum = "12";
            NumberOfFields = 32;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PassportNumber property
        [Attributes.Form_Property("Номер паспорта")]
        public string PassportNumber
        {
            get
            {
                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PassportNumber))[0][0];
                }
                else
                {
                    return _PassportNumber_Not_Valid;
                }
            }
            set
            {
                _PassportNumber_Not_Valid = value;
                if (GetErrors(nameof(PassportNumber)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumber), _PassportNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }

        private string _PassportNumber_Not_Valid = "";
        private void PassportNumber_Validation()
        {
            ClearErrors(nameof(PassportNumber));
        }
        //PassportNumber property

        private void OperationCode_Validation(string value)
        {
            ClearErrors(nameof(OperationCode));
            var a = new Regex("[0-9]{2}");
            List<string> spr = new List<string>();    //HERE BINDS SPRAVOCHNIK
            if (!a.IsMatch(value) || !spr.Contains(value))
            {
                AddError(nameof(OperationCode), "Недопустимое значение");
                return;
            }
            bool a0 = value.Equals("01");
            bool a1 = value.Equals("13");
            bool a2 = value.Equals("14");
            bool a3 = value.Equals("16");
            bool a4 = value.Equals("26");
            bool a5 = value.Equals("36");
            bool a6 = value.Equals("44");
            bool a7 = value.Equals("45");
            bool a8 = value.Equals("49");
            bool a9 = value.Equals("51");
            bool a10 = value.Equals("52");
            bool a11 = value.Equals("55");
            bool a12 = value.Equals("56");
            bool a13 = value.Equals("57");
            bool a14 = value.Equals("59");
            bool a15 = value.Equals("76");
            if (a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11 || a12 || a13 || a14 || a15)
                AddError(nameof(OperationCode), "Код операции не может быть использован для РВ");
            return;
        }

        //PassportNumberNote property
        public string PassportNumberNote
        {
            get
            {
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PassportNumberNote))[0][0];
                }
                else
                {
                    return _PassportNumberNote_Not_Valid;
                }
            }
            set
            {
                _PassportNumberNote_Not_Valid = value;
                if (GetErrors(nameof(PassportNumberNote)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumberNote), _PassportNumberNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }

        private string _PassportNumberNote_Not_Valid = "";
        private void PassportNumberNote_Validation()
        {
            ClearErrors(nameof(PassportNumberNote));
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
                    return (string)_dataAccess.Get(nameof(PassportNumberRecoded))[0][0];
                }
                else
                {
                    return _PassportNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _PassportNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(PassportNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PassportNumberRecoded), _PassportNumberRecoded_Not_Valid);
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

        //NameIOU property
        [Attributes.Form_Property("Наименование ИОУ")]
        public string NameIOU
        {
            get
            {
                if (GetErrors(nameof(NameIOU)) == null)
                {
                    return (string)_dataAccess.Get(nameof(NameIOU))[0][0];
                }
                else
                {
                    return _NameIOU_Not_Valid;
                }
            }
            set
            {
                _NameIOU_Not_Valid = value;
                if (GetErrors(nameof(NameIOU)) == null)
                {
                    _dataAccess.Set(nameof(NameIOU), _NameIOU_Not_Valid);
                }
                OnPropertyChanged(nameof(NameIOU));
            }
        }

        private string _NameIOU_Not_Valid = "";
        private void NameIOU_Validation(string value)//TODO
        {
            ClearErrors(nameof(NameIOU));
        }
        //NameIOU property

        //FactoryNumber property
        [Attributes.Form_Property("Заводской номер")]
        public string FactoryNumber
        {
            get
            {
                if (GetErrors(nameof(FactoryNumber)) == null)
                {
                    return (string)_dataAccess.Get(nameof(FactoryNumber))[0][0];
                }
                else
                {
                    return _FactoryNumber_Not_Valid;
                }
            }
            set
            {
                _FactoryNumber_Not_Valid = value;
                if (GetErrors(nameof(FactoryNumber)) == null)
                {
                    _dataAccess.Set(nameof(FactoryNumber), _FactoryNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }

        private string _FactoryNumber_Not_Valid = "";
        private void FactoryNumber_Validation()
        {
            ClearErrors(nameof(FactoryNumber));
        }
        //FactoryNumber property

        //FactoryNumberRecoded property
        public string FactoryNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(FactoryNumberRecoded)) == null)
                {
                    return (string)_dataAccess.Get(nameof(FactoryNumberRecoded))[0][0];
                }
                else
                {
                    return _FactoryNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _FactoryNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(FactoryNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(FactoryNumberRecoded), _FactoryNumberRecoded_Not_Valid);
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

        //Mass Property
        [Attributes.Form_Property("Масса, кг")]
        public string Mass
        {
            get
            {
                if (GetErrors(nameof(Mass)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Mass))[0][0];
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

        private string _Mass_Not_Valid = "";
        private void Mass_Validation(string value)//TODO
        {
            ClearErrors(nameof(Mass));
        }
        //Mass Property

        //CreatorOKPO property
        [Attributes.Form_Property("ОКПО изготовителя")]
        public string CreatorOKPO
        {
            get
            {
                if (GetErrors(nameof(CreatorOKPO)) == null)
                {
                    return (string)_dataAccess.Get(nameof(CreatorOKPO))[0][0];
                }
                else
                {
                    return _CreatorOKPO_Not_Valid;
                }
            }
            set
            {
                _CreatorOKPO_Not_Valid = value;
                if (GetErrors(nameof(CreatorOKPO)) == null)
                {
                    _dataAccess.Set(nameof(CreatorOKPO), _CreatorOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(CreatorOKPO));
            }
        }
        //If change this change validation
        private string _CreatorOKPO_Not_Valid = "";
        private void CreatorOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(CreatorOKPO));
            if (value.Equals("прим.")) return;
            var mask1 = new Regex("[А-Яа-я]*");
            if (mask1.IsMatch(value)) return;
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
                    return (string)_dataAccess.Get(nameof(CreatorOKPONote))[0][0];
                }
                else
                {
                    return _CreatorOKPONote_Not_Valid;
                }
            }
            set
            {
                _CreatorOKPONote_Not_Valid = value;
                if (GetErrors(nameof(CreatorOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(CreatorOKPONote), _CreatorOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(CreatorOKPONote));
            }
        }

        private string _CreatorOKPONote_Not_Valid = "";
        private void CreatorOKPONote_Validation()
        {
            ClearErrors(nameof(CreatorOKPONote));
        }
        //CreatorOKPONote property

        //CreationDate property
        [Attributes.Form_Property("Дата изготовления")]
        public DateTimeOffset CreationDate
        {
            get
            {
                if (GetErrors(nameof(CreationDate)) == null)
                {
                    return (DateTimeOffset)_dataAccess.Get(nameof(CreationDate))[0][0];
                }
                else
                {
                    return _CreationDate_Not_Valid;
                }
            }
            set
            {
                _CreationDate_Not_Valid = value;
                if (GetErrors(nameof(CreationDate)) == null)
                {
                    _dataAccess.Set(nameof(CreationDate), _CreationDate_Not_Valid);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation
        private DateTimeOffset _CreationDate_Not_Valid = DateTimeOffset.MinValue;
        private void CreationDate_Validation(DateTimeOffset value)//Ready
        {
            ClearErrors(nameof(CreationDate));
        }
        //CreationDate property

        //SignedServicePeriod property
        [Attributes.Form_Property("НСС, мес.")]
        public float SignedServicePeriod
        {
            get
            {
                if (GetErrors(nameof(SignedServicePeriod)) == null)
                {
                    return (float)_dataAccess.Get(nameof(SignedServicePeriod))[0][0];
                }
                else
                {
                    return _SignedServicePeriod_Not_Valid;
                }
            }
            set
            {
                _SignedServicePeriod_Not_Valid = value;
                if (GetErrors(nameof(SignedServicePeriod)) == null)
                {
                    _dataAccess.Set(nameof(SignedServicePeriod), _SignedServicePeriod_Not_Valid);
                }
                OnPropertyChanged(nameof(SignedServicePeriod));
            }
        }

        private float _SignedServicePeriod_Not_Valid = -1;
        private void SignedServicePeriod_Validation(int value)//Ready
        {
            ClearErrors(nameof(SignedServicePeriod));
            if (value <= 0)
                AddError(nameof(SignedServicePeriod), "Недопустимое значение");
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
                    return (byte)_dataAccess.Get(nameof(PropertyCode))[0][0];
                }
                else
                {
                    return _PropertyCode_Not_Valid;
                }
            }
            set
            {
                _PropertyCode_Not_Valid = value;
                if (GetErrors(nameof(PropertyCode)) == null)
                {
                    _dataAccess.Set(nameof(PropertyCode), _PropertyCode_Not_Valid);
                }
                OnPropertyChanged(nameof(PropertyCode));
            }
        }

        private byte _PropertyCode_Not_Valid = 255;
        private void PropertyCode_Validation(byte value)//Ready
        {
            ClearErrors(nameof(PropertyCode));
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
                    return (string)_dataAccess.Get(nameof(Owner))[0][0];
                }
                else
                {
                    return _Owner_Not_Valid;
                }
            }
            set
            {
                _Owner_Not_Valid = value;
                if (GetErrors(nameof(Owner)) == null)
                {
                    _dataAccess.Set(nameof(Owner), _Owner_Not_Valid);
                }
                OnPropertyChanged(nameof(Owner));
            }
        }
        //if change this change validation
        private string _Owner_Not_Valid = "";
        private void Owner_Validation(string value)//Ready
        {
            ClearErrors(nameof(Owner));
            if (value.Equals("прим.")) return;
            var mask1 = new Regex("[А-Яа-я]*");
            if (mask1.IsMatch(value)) return;
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
                    return (string)_dataAccess.Get(nameof(ProviderOrRecieverOKPO))[0][0];
                }
                else
                {
                    return _ProviderOrRecieverOKPO_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPO_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPO)) == null)
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), _ProviderOrRecieverOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }

        private string _ProviderOrRecieverOKPO_Not_Valid = "";
        private void ProviderOrRecieverOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(ProviderOrRecieverOKPO));
            if (value.Equals("Минобороны") || value.Equals("прим.")) return;
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
                    return (string)_dataAccess.Get(nameof(ProviderOrRecieverOKPONote))[0][0];
                }
                else
                {
                    return _ProviderOrRecieverOKPONote_Not_Valid;
                }
            }
            set
            {
                _ProviderOrRecieverOKPONote_Not_Valid = value;
                if (GetErrors(nameof(ProviderOrRecieverOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), _ProviderOrRecieverOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }

        private string _ProviderOrRecieverOKPONote_Not_Valid = "";
        private void ProviderOrRecieverOKPONote_Validation()
        {
            ClearErrors(nameof(ProviderOrRecieverOKPONote));
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
                    return (string)_dataAccess.Get(nameof(TransporterOKPO))[0][0];
                }
                else
                {
                    return _TransporterOKPO_Not_Valid;
                }
            }
            set
            {
                _TransporterOKPO_Not_Valid = value;
                if (GetErrors(nameof(TransporterOKPO)) == null)
                {
                    _dataAccess.Set(nameof(TransporterOKPO), _TransporterOKPO_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }

        private string _TransporterOKPO_Not_Valid = "";
        private void TransporterOKPO_Validation(string value)//TODO
        {
            ClearErrors(nameof(TransporterOKPO));
            if (value.Equals("прим.") || value.Equals("-")) return;
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
                    return (string)_dataAccess.Get(nameof(TransporterOKPONote))[0][0];
                }
                else
                {
                    return _TransporterOKPONote_Not_Valid;
                }
            }
            set
            {
                _TransporterOKPONote_Not_Valid = value;
                if (GetErrors(nameof(TransporterOKPONote)) == null)
                {
                    _dataAccess.Set(nameof(TransporterOKPONote), _TransporterOKPONote_Not_Valid);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }

        private string _TransporterOKPONote_Not_Valid = "";
        private void TransporterOKPONote_Validation()
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
                    return (string)_dataAccess.Get(nameof(PackName))[0][0];
                }
                else
                {
                    return _PackName_Not_Valid;
                }
            }
            set
            {
                _PackName_Not_Valid = value;
                if (GetErrors(nameof(PackName)) == null)
                {
                    _dataAccess.Set(nameof(PackName), _PackName_Not_Valid);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }

        private string _PackName_Not_Valid = "";
        private void PackName_Validation()
        {
            ClearErrors(nameof(PackName));
        }
        //PackName property

        //PackNameNote property
        public string PackNameNote
        {
            get
            {
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackNameNote))[0][0];
                }
                else
                {
                    return _PackNameNote_Not_Valid;
                }
            }
            set
            {
                _PackNameNote_Not_Valid = value;
                if (GetErrors(nameof(PackNameNote)) == null)
                {
                    _dataAccess.Set(nameof(PackNameNote), _PackNameNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }

        private string _PackNameNote_Not_Valid = "";
        private void PackNameNote_Validation()
        {
            ClearErrors(nameof(PackNameNote));
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
                    return (string)_dataAccess.Get(nameof(PackType))[0][0];
                }
                else
                {
                    return _PackType_Not_Valid;
                }
            }
            set
            {
                _PackType_Not_Valid = value;
                if (GetErrors(nameof(PackType)) == null)
                {
                    _dataAccess.Set(nameof(PackType), _PackType_Not_Valid);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
        private string _PackType_Not_Valid = "";
        private void PackType_Validation()//Ready
        {
            ClearErrors(nameof(PackType));
        }
        //PackType property

        //PackTypeRecoded property
        public string PackTypeRecoded
        {
            get
            {
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackTypeRecoded))[0][0];
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                _PackTypeRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), _PackTypeRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        private string _PackTypeRecoded_Not_Valid = "";
        private void PackTypeRecoded_Validation()
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
                    return (string)_dataAccess.Get(nameof(PackTypeNote))[0][0];
                }
                else
                {
                    return _PackTypeNote_Not_Valid;
                }
            }
            set
            {
                _PackTypeNote_Not_Valid = value;
                if (GetErrors(nameof(PackTypeNote)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeNote), _PackTypeNote_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }

        private string _PackTypeNote_Not_Valid = "";
        private void PackTypeNote_Validation()
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
                    return (string)_dataAccess.Get(nameof(PackNumber))[0][0];
                }
                else
                {
                    return _PackNumber_Not_Valid;
                }
            }
            set
            {
                _PackNumber_Not_Valid = value;
                if (GetErrors(nameof(PackNumber)) == null)
                {
                    _dataAccess.Set(nameof(PackNumber), _PackNumber_Not_Valid);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation
        private string _PackNumber_Not_Valid = "";
        private void PackNumber_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackNumber));
        }
        //PackNumber property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public string PackNumberRecoded
        {
            get
            {
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    return (string)_dataAccess.Get(nameof(PackNumberRecoded))[0][0];
                }
                else
                {
                    return _PackNumberRecoded_Not_Valid;
                }
            }
            set
            {
                _PackNumberRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackNumberRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackNumberRecoded), _PackNumberRecoded_Not_Valid);
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
    }
}
