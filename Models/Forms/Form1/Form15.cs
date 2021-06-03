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
            FormNum = "15";
            NumberOfFields = 39;
        }

        [Attributes.Form_Property("Форма")]
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
            }
        }
        //PassportNumber property

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

        //Type property
        [Attributes.Form_Property("Тип")]
        public IDataAccess<string> Type
        {
            get
            {
                if (GetErrors(nameof(Type)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(Type));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void Type_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
        }
        //Type property

        //TypeRecoded property
        public IDataAccess<string> TypeRecoded
        {
            get
            {
                if (GetErrors(nameof(TypeRecoded)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(TypeRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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
                if (GetErrors(nameof(Radionuclids)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(Radionuclids));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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

        private void Radionuclids_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
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

        private void FactoryNumberRecoded_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //FactoryNumberRecoded property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public int Quantity
        {
            get
            {
                if (GetErrors(nameof(Quantity)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(Quantity));//OK
                    return tmp != null ? (int)tmp : -1;
                }
                else
                {
                    
                }
            }
            set
            {
                Quantity_Validation(value);
                //_Quantity_Validation(value);

                if (GetErrors(nameof(Quantity)) == null)
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.

        private void Quantity_Validation(int value)//Ready
        {
            value.ClearErrors();
            if (value <= 0)
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
                if (GetErrors(nameof(Activity)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(Activity));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
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


        private void Activity_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e')||value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
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
                    value.AddError( "Число должно быть больше нуля");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //Activity property

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
                //_CreationDate_Validation(value);
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

        //StatusRAO property
        [Attributes.Form_Property("Статус РАО")]
        public IDataAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                if (GetErrors(nameof(StatusRAO)) == null)
                {
                    return _dataAccess.Get<string>(nameof(StatusRAO));
                }
                else
                {
                    
                }
            }
            set
            {
                StatusRAO_Validation(value);
                if (GetErrors(nameof(StatusRAO)) == null)
                {
                    _dataAccess.Set(nameof(StatusRAO), value);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }


        private void StatusRAO_Validation(IDataAccess<string> value)//rdy
        {
            value.ClearErrors();
            if (value.Length == 1)
            {
                int tmp;
                try
                {
                    tmp = int.Parse(value.Value);
                    if ((tmp < 1) || ((tmp > 4) && (tmp != 6) && (tmp != 9)))
                    {
                        value.AddError( "Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    value.AddError( "Недопустимое значение");
                }
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
        //StatusRAO property

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
            if (value.Equals("прим.")) { }
            short tmp = (short)OperationCode;
            bool a = (tmp >= 10) && (tmp <= 14);
            bool b = (tmp >= 41) && (tmp <= 45);
            bool c = (tmp >= 71) && (tmp <= 73);
            bool e = (tmp >= 55) && (tmp <= 57);
            bool d = (tmp == 1) || (tmp == 16) || (tmp == 18) || (tmp == 48) ||
                (tmp == 49) || (tmp == 51) || (tmp == 52) || (tmp == 59) ||
                (tmp == 68) || (tmp == 75) || (tmp == 76);
            if (a || b || c || d || e)
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


        private void TransporterOKPO_Validation(IDataAccess<string> value)//Done
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
                value.AddError( "Поле не заполнено");
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

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]
        public IDataAccess<string> StoragePlaceName
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceName));
                }
                else
                {
                    
                }
            }
            set
            {
                StoragePlaceName_Validation(value);
                if (GetErrors(nameof(StoragePlaceName)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceName), value);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation

        private void StoragePlaceName_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            var a = new List<string>();//here binds spr
            foreach(var item in a)
            {
                if (a.Equals(value)) return;
            }
            value.AddError( "Такого значения нет в справочнике");
        }
        //StoragePlaceName property

        //StoragePlaceNameNote property
        public IDataAccess<string> StoragePlaceNameNote
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceNameNote));
                }
                else
                {
                    
                }
            }
            set
            {
                StoragePlaceNameNote_Validation(value);
                if (GetErrors(nameof(StoragePlaceNameNote)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), value);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation

        private void StoragePlaceNameNote_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //StoragePlaceNameNote property

        //StoragePlaceCode property
        [Attributes.Form_Property("Код ПХ")]
        public IDataAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                if (GetErrors(nameof(StoragePlaceCode)) == null)
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceCode));
                }
                else
                {
                    
                }
            }
            set
            {
                StoragePlaceCode_Validation(value);
                if (GetErrors(nameof(StoragePlaceCode)) == null)
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), value);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation

        private void StoragePlaceCode_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            var lst = new List<string>();//HERE binds spr
            foreach(var item in lst)
            {
                if (item.Equals(value)) return;
            }
            value.AddError( "Такого значения нет в справочнике");
            //if (!(value.Value == "-"))
            //    if (value.Length != 8)
            //        value.AddError( "Недопустимое значение");
            //    else
            //        for (int i = 0; i < 8; i++)
            //        {
            //            if (!((value[i] >= '0') && (value[i] <= '9')))
            //            {
            //                value.AddError( "Недопустимое значение");
            //                return;
            //            }
            //        }
        }
        //StoragePlaceCode property

        //RefineOrSortRAOCode property
        [Attributes.Form_Property("Код переработки/сортировки РАО")]
        public IDataAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                if (GetErrors(nameof(RefineOrSortRAOCode)) == null)
                {
                    return _dataAccess.Get<string>(nameof(RefineOrSortRAOCode));
                }
                else
                {
                    
                }
            }
            set
            {
                RefineOrSortRAOCode_Validation(value);
                if (GetErrors(nameof(RefineOrSortRAOCode)) == null)
                {
                    _dataAccess.Set(nameof(RefineOrSortRAOCode), value);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        //If change this change validation

        private void RefineOrSortRAOCode_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
            if((value.Value == null) || value.Equals(""))
            {
                return;  
            }
            var a = new Regex("^[0-9][0-9]$");
            if (!a.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //RefineOrSortRAOCode property

        //Subsidy property
        [Attributes.Form_Property("Субсидия, %")]
        public IDataAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {
                if (GetErrors(nameof(Subsidy)) == null)
                {
                    return _dataAccess.Get<string>(nameof(Subsidy));
                }
                else
                {
                    
                }
            }
            set
            {
                Subsidy_Validation(value);
                if (GetErrors(nameof(Subsidy)) == null)
                {
                    _dataAccess.Set(nameof(Subsidy), value);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }


        private void Subsidy_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Equals("")) return;
            try
            {
                int tmp = Int32.Parse(value.Value);
                if (!((tmp > 0) && (tmp <= 100)))
                    value.AddError( "Недопустимое значение");
            }
            catch
            {
                value.AddError( "Недопустимое значение");
            }
        }
        //Subsidy property

        //FcpNumber property
        [Attributes.Form_Property("Номер мероприятия ФЦП")]
        public IDataAccess<string> FcpNumber
        {
            get
            {
                if (GetErrors(nameof(FcpNumber)) == null)
                {
                    return _dataAccess.Get<string>(nameof(FcpNumber));
                }
                else
                {
                    
                }
            }
            set
            {
                FcpNumber_Validation(value);
                if (GetErrors(nameof(FcpNumber)) == null)
                {
                    _dataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }


        private void FcpNumber_Validation(IDataAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //FcpNumber property

        protected override void DocumentNumber_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }

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
            bool a0 = value==15;
            bool a1 = value==17;
            bool a2 = value==46;
            bool a3 = value==47;
            bool a4 = value==53;
            bool a5 = value==54;
            bool a6 = value==58;
            bool a7 = value==61;
            bool a8 = value==62;
            bool a9 = value==65;
            bool a10 = value==66;
            bool a11 = value==67;
            bool a12 = value==81;
            bool a13 = value==82;
            bool a14 = value==83;
            bool a15 = value==85;
            bool a16 = value==86;
            bool a17 = value==87;
            if (a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11 || a12 || a13 || a14 || a15 || a16 || a17)
                value.AddError( "Код операции не может быть использован для РАО");
            return;
        }
    }
}
