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
            FormNum.Value = "15";
            NumberOfFields.Value = 39;
        }

        [Attributes.Form_Property("Форма")]
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
                
                {
                    return _dataAccess.Get<string>(nameof(PassportNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                PassportNumber_Validation(value);

                
                {
                    _dataAccess.Set(nameof(PassportNumber), value);
                }
                OnPropertyChanged(nameof(PassportNumber));
            }
        }


        private void PassportNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим."))
            {
                if ((PassportNumberNote.Value == null) || (PassportNumberNote.Value == ""))
                    value.AddError( "Заполните примечание");
            }
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
                PassportNumberNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PassportNumberNote), value);
                }
                OnPropertyChanged(nameof(PassportNumberNote));
            }
        }


        private void PassportNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
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
                PassportNumberRecoded_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PassportNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PassportNumberRecoded));
            }
        }
        //If change this change validation

        private void PassportNumberRecoded_Validation(RamAccess<string> value)//Ready
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
                
                {
                    return _dataAccess.Get<string>(nameof(Type));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Type_Validation(value);
                
                {
                    _dataAccess.Set(nameof(Type), value);
                }
                OnPropertyChanged(nameof(Type));
            }
        }


        private void Type_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
        }
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
                TypeRecoded_Validation(value);
                
                {
                    _dataAccess.Set(nameof(TypeRecoded), value);
                }
                OnPropertyChanged(nameof(TypeRecoded));
            }
        }


        private void TypeRecoded_Validation(RamAccess<string> value)
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
                
                {
                    return _dataAccess.Get<string>(nameof(Radionuclids));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Radionuclids_Validation(value);

                
                {
                    _dataAccess.Set(nameof(Radionuclids), value);
                }
                OnPropertyChanged(nameof(Radionuclids));
            }
        }
        //If change this change validation

        private void Radionuclids_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
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
                
                {
                    return _dataAccess.Get<string>(nameof(FactoryNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                FactoryNumber_Validation(value);
                
                {
                    _dataAccess.Set(nameof(FactoryNumber), value);
                }
                OnPropertyChanged(nameof(FactoryNumber));
            }
        }


        private void FactoryNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
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
                
                {
                    return _dataAccess.Get<string>(nameof(FactoryNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                FactoryNumberRecoded_Validation(value);
                
                {
                    _dataAccess.Set(nameof(FactoryNumberRecoded), value);
                }
                OnPropertyChanged(nameof(FactoryNumberRecoded));
            }
        }

        private void FactoryNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //FactoryNumberRecoded property

        //Quantity property
        [Attributes.Form_Property("Количество, шт.")]
        public RamAccess<int> Quantity
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(Quantity));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Quantity_Validation(value);
                //_Quantity_Validation(value);

                
                {
                    _dataAccess.Set(nameof(Quantity), value);
                }
                OnPropertyChanged(nameof(Quantity));
            }
        }
        // positive int.

        private void Quantity_Validation(RamAccess<int> value)//Ready
        {
            value.ClearErrors();
            if (value.Value <= 0)
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
                
                {
                    return _dataAccess.Get<string>(nameof(Activity));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                Activity_Validation(value);
                
                {
                    _dataAccess.Set(nameof(Activity), value);
                }
                OnPropertyChanged(nameof(Activity));
            }
        }


        private void Activity_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (!(value.Value.Contains('e')||value.Value.Contains('E')))
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            string tmp=value.Value;
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
                CreationDate_Validation(value);
                //_CreationDate_Validation(value);
                
                {
                    _dataAccess.Set(nameof(CreationDate), value);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation

        private void CreationDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
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
        public RamAccess<string> StatusRAO  //1 cyfer or OKPO.
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StatusRAO));
                }
                
                {
                    
                }
            }
            set
            {
                StatusRAO_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StatusRAO), value);
                }
                OnPropertyChanged(nameof(StatusRAO));
            }
        }


        private void StatusRAO_Validation(RamAccess<string> value)//rdy
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
                        value.AddError( "Недопустимое значение");
                    }
                }
                catch (Exception)
                {
                    value.AddError( "Недопустимое значение");
                }
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
        //StatusRAO property

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
                ProviderOrRecieverOKPO_Validation(value);

                
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPO), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPO));
            }
        }


        private void ProviderOrRecieverOKPO_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            if (value.Value.Equals("прим.")) { }
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
                return;
            }
            if (value.Value.Equals("Минобороны")) return;
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
                
                {
                    return _dataAccess.Get<string>(nameof(ProviderOrRecieverOKPONote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                ProviderOrRecieverOKPONote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(ProviderOrRecieverOKPONote), value);
                }
                OnPropertyChanged(nameof(ProviderOrRecieverOKPONote));
            }
        }


        private void ProviderOrRecieverOKPONote_Validation(RamAccess<string> value)
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
                
                {
                    return _dataAccess.Get<string>(nameof(TransporterOKPO));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                TransporterOKPO_Validation(value);

                
                {
                    _dataAccess.Set(nameof(TransporterOKPO), value);
                }
                OnPropertyChanged(nameof(TransporterOKPO));
            }
        }


        private void TransporterOKPO_Validation(RamAccess<string> value)//Done
        {
            value.ClearErrors();
            if ((value.Value == null))
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
                
                {
                    return _dataAccess.Get<string>(nameof(TransporterOKPONote));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                TransporterOKPONote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(TransporterOKPONote), value);
                }
                OnPropertyChanged(nameof(TransporterOKPONote));
            }
        }


        private void TransporterOKPONote_Validation(RamAccess<string> value)
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
                
                {
                    return _dataAccess.Get<string>(nameof(PackName));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                PackName_Validation(value);

                
                {
                    _dataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }


        private void PackName_Validation(RamAccess<string> value)
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
                PackNameNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PackNameNote), value);
                }
                OnPropertyChanged(nameof(PackNameNote));
            }
        }


        private void PackNameNote_Validation(RamAccess<string> value)
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
                
                {
                    return _dataAccess.Get<string>(nameof(PackType));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                PackType_Validation(value);

                
                {
                    _dataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation

        private void PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
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
                
                {
                    return _dataAccess.Get<string>(nameof(PackTypeRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                PackTypeRecoded_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }


        private void PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
        }
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
                PackTypeNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PackTypeNote), value);
                }
                OnPropertyChanged(nameof(PackTypeNote));
            }
        }


        private void PackTypeNote_Validation(RamAccess<string> value)
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
                
                {
                    return _dataAccess.Get<string>(nameof(PackNumber));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                PackNumber_Validation(value);

                
                {
                    _dataAccess.Set(nameof(PackNumber), value);
                }
                OnPropertyChanged(nameof(PackNumber));
            }
        }
        //If change this change validation

        private void PackNumber_Validation(RamAccess<string> value)//Ready
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
                PackNumberNote_Validation(value);

                
                {
                    _dataAccess.Set(nameof(PackNumberNote), value);
                }
                OnPropertyChanged(nameof(PackNumberNote));
            }
        }


        private void PackNumberNote_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals(""))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }
        //PackNumberNote property

        //PackNumberRecoded property
        [Attributes.Form_Property("Номер упаковки")]
        public RamAccess<string> PackNumberRecoded
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(PackNumberRecoded));//OK
                    
                }
                
                {
                    
                }
            }
            set
            {
                PackNumberRecoded_Validation(value);
                
                {
                    _dataAccess.Set(nameof(PackNumberRecoded), value);
                }
                OnPropertyChanged(nameof(PackNumberRecoded));
            }
        }
        //If change this change validation

        private void PackNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //PackNumberRecoded property

        //StoragePlaceName property
        [Attributes.Form_Property("Наименование ПХ")]
        public RamAccess<string> StoragePlaceName
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceName));
                }
                
                {
                    
                }
            }
            set
            {
                StoragePlaceName_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StoragePlaceName), value);
                }
                OnPropertyChanged(nameof(StoragePlaceName));
            }
        }
        //If change this change validation

        private void StoragePlaceName_Validation(RamAccess<string> value)//Ready
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
        public RamAccess<string> StoragePlaceNameNote
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceNameNote));
                }
                
                {
                    
                }
            }
            set
            {
                StoragePlaceNameNote_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StoragePlaceNameNote), value);
                }
                OnPropertyChanged(nameof(StoragePlaceNameNote));
            }
        }
        //If change this change validation

        private void StoragePlaceNameNote_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
        }
        //StoragePlaceNameNote property

        //StoragePlaceCode property
        [Attributes.Form_Property("Код ПХ")]
        public RamAccess<string> StoragePlaceCode //8 cyfer code or - .
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(StoragePlaceCode));
                }
                
                {
                    
                }
            }
            set
            {
                StoragePlaceCode_Validation(value);
                
                {
                    _dataAccess.Set(nameof(StoragePlaceCode), value);
                }
                OnPropertyChanged(nameof(StoragePlaceCode));
            }
        }
        //if change this change validation

        private void StoragePlaceCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            var lst = new List<string>();//HERE binds spr
            foreach(var item in lst)
            {
                if (item.Equals(value)) return;
            }
            value.AddError( "Такого значения нет в справочнике");
            //if (!(value.Value == "-"))
            //    if (value.Value.Length != 8)
            //        value.AddError( "Недопустимое значение");
            //    
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
        public RamAccess<string> RefineOrSortRAOCode //2 cyfer code or empty.
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RefineOrSortRAOCode));
                }
                
                {
                    
                }
            }
            set
            {
                RefineOrSortRAOCode_Validation(value);
                
                {
                    _dataAccess.Set(nameof(RefineOrSortRAOCode), value);
                }
                OnPropertyChanged(nameof(RefineOrSortRAOCode));
            }
        }
        //If change this change validation

        private void RefineOrSortRAOCode_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
            if((value.Value == null) || value.Value.Equals(""))
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
        public RamAccess<string> Subsidy // 0<number<=100 or empty.
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Subsidy));
                }
                
                {
                    
                }
            }
            set
            {
                Subsidy_Validation(value);
                
                {
                    _dataAccess.Set(nameof(Subsidy), value);
                }
                OnPropertyChanged(nameof(Subsidy));
            }
        }


        private void Subsidy_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null) || value.Value.Equals("")) return;
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
        public RamAccess<string> FcpNumber
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(FcpNumber));
                }
                
                {
                    
                }
            }
            set
            {
                FcpNumber_Validation(value);
                
                {
                    _dataAccess.Set(nameof(FcpNumber), value);
                }
                OnPropertyChanged(nameof(FcpNumber));
            }
        }


        private void FcpNumber_Validation(RamAccess<string> value)//TODO
        {
            value.ClearErrors();
        }
        //FcpNumber property

        protected override void DocumentNumber_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))//ok
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }

        protected override void OperationCode_Validation(RamAccess<short?> value)//OK
        {
            value.ClearErrors();
            if (value.Value == null)
            {
                value.AddError( "Поле не заполнено");
                return;
            }
            List<short> spr = new List<short>();    //HERE BINDS SPRAVOCHNIK
            bool flag = false;
            foreach (var item in spr)
            {
                if (item == value.Value) flag = true;
            }
            if (!flag)
            {
                value.AddError( "Недопустимое значение");
                return;
            }
            bool a0 = value.Value==15;
            bool a1 = value.Value==17;
            bool a2 = value.Value==46;
            bool a3 = value.Value==47;
            bool a4 = value.Value==53;
            bool a5 = value.Value==54;
            bool a6 = value.Value==58;
            bool a7 = value.Value==61;
            bool a8 = value.Value==62;
            bool a9 = value.Value==65;
            bool a10 = value.Value==66;
            bool a11 = value.Value==67;
            bool a12 = value.Value==81;
            bool a13 = value.Value==82;
            bool a14 = value.Value==83;
            bool a15 = value.Value==85;
            bool a16 = value.Value==86;
            bool a17 = value.Value==87;
            if (a0 || a1 || a2 || a3 || a4 || a5 || a6 || a7 || a8 || a9 || a10 || a11 || a12 || a13 || a14 || a15 || a16 || a17)
                value.AddError( "Код операции не может быть использован для РАО");
            return;
        }
    }
}
