using Models.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using ClassLibrary1;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("")]
    public class Form32_1 : Abstracts.Form3
    {
        public Form32_1() : base()
        {
            //FormNum.Value = "32_1";
            //NumberOfFields.Value = 15;
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
                //{
                //    value.AddError("Поле не может быть пустым"); return false;
                //}
                return true;
            }
            return true;
        }
        //PassportNumber property

        ////PassportNumberNote property
        //public virtual RamAccess<string> PassportNumberNote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(PassportNumberNote));//OK

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {


        //        {
        //            DataAccess.Set(nameof(PassportNumberNote), value);
        //        }
        //        OnPropertyChanged(nameof(PassportNumberNote));
        //    }
        //}

        //private bool PassportNumberNote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////PassportNumberNote property

        //CreatorOKPO property
        public int? CreatorOKPOId { get; set; }
        [Attributes.Form_Property("ОКПО изготовителя")]
        public virtual RamAccess<string> CreatorOKPO
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(CreatorOKPO));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(CreatorOKPO), value);
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
                value.AddError("Поле не заполнено");
                return false;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
//CreatorOKPO property

protected List<string> OKSM = new List<string>
            {
                "АФГАНИСТАН","АЛБАНИЯ","АНТАРКТИДА","АЛЖИР","АМЕРИКАНСКОЕ САМОА","АНДОРРА","АНГОЛА","АНТИГУА И БАРБУДА","АЗЕРБАЙДЖАН","АРГЕНТИНА","АВСТРАЛИЯ","АВСТРИЯ","БАГАМЫ","БАХРЕЙН",
                "БАНГЛАДЕШ","АРМЕНИЯ","БАРБАДОС","БЕЛЬГИЯ","БЕРМУДЫ","БУТАН","БОЛИВИЯ, МНОГОНАЦИОНАЛЬНОЕ ГОСУДАРСТВО","БОСНИЯ И ГЕРЦЕГОВИНА","БОТСВАНА","ОСТРОВ БУВЕ","БРАЗИЛИЯ","БЕЛИЗ",
                "БРИТАНСКАЯ ТЕРРИТОРИЯ В ИНДИЙСКОМ ОКЕАНЕ","СОЛОМОНОВЫ ОСТРОВА","ВИРГИНСКИЕ ОСТРОВА (БРИТАНСКИЕ)","БРУНЕЙ-ДАРУССАЛАМ","БОЛГАРИЯ","МЬЯНМА","БУРУНДИ","БЕЛАРУСЬ","КАМБОДЖА",
                "КАМЕРУН","КАНАДА","КАБО-ВЕРДЕ","ОСТРОВА КАЙМАН","ЦЕНТРАЛЬНО-АФРИКАНСКАЯ РЕСПУБЛИКА","ШРИ-ЛАНКА","ЧАД","ЧИЛИ","КИТАЙ","ТАЙВАНЬ (КИТАЙ)","ОСТРОВ РОЖДЕСТВА","КОКОСОВЫЕ (КИЛИНГ) ОСТРОВА",
                "КОЛУМБИЯ","КОМОРЫ","МАЙОТТА","КОНГО","КОНГО, ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ОСТРОВА КУКА","КОСТА-РИКА","ХОРВАТИЯ","КУБА","КИПР","ЧЕХИЯ","БЕНИН","ДАНИЯ","ДОМИНИКА","ДОМИНИКАНСКАЯ РЕСПУБЛИКА",
                "ЭКВАДОР","ЭЛЬ-САЛЬВАДОР","ЭКВАТОРИАЛЬНАЯ ГВИНЕЯ","ЭФИОПИЯ","ЭРИТРЕЯ","ЭСТОНИЯ","ФАРЕРСКИЕ ОСТРОВА","ФОЛКЛЕНДСКИЕ ОСТРОВА (МАЛЬВИНСКИЕ)","ЮЖНАЯ ДЖОРДЖИЯ И ЮЖНЫЕ САНДВИЧЕВЫ ОСТРОВА",
                "ФИНЛЯНДИЯ","ЭЛАНДСКИЕ ОСТРОВА","ФРАНЦИЯ","ФРАНЦУЗСКАЯ ГВИАНА","БОНЭЙР, СИНТ-ЭСТАТИУС И САБА","НОВАЯ КАЛЕДОНИЯ","ВАНУАТУ","НОВАЯ ЗЕЛАНДИЯ","НИКАРАГУА","НИГЕР","ФИДЖИ",
                "ФРАНЦУЗСКАЯ ПОЛИНЕЗИЯ","ФРАНЦУЗСКИЕ ЮЖНЫЕ ТЕРРИТОРИИ","ДЖИБУТИ","ГАБОН","ГРУЗИЯ","ГАМБИЯ","ПАЛЕСТИНА, ГОСУДАРСТВО","ГЕРМАНИЯ","ГАНА","ГИБРАЛТАР","КИРИБАТИ","МАЛИ","МАЛЬТА",
                "ГРЕЦИЯ","ГРЕНЛАНДИЯ","ГРЕНАДА","ГВАДЕЛУПА","ГУАМ","ГВАТЕМАЛА","ГВИНЕЯ","ГАЙАНА","ГАИТИ","ОСТРОВ ХЕРД И ОСТРОВА МАКДОНАЛЬД","ПАПСКИЙ ПРЕСТОЛ (ГОСУДАРСТВО - ГОРОД ВАТИКАН)",
                "ГОНДУРАС","ГОНКОНГ","ВЕНГРИЯ","ИСЛАНДИЯ","ИНДИЯ","ИНДОНЕЗИЯ","ИРАН (ИСЛАМСКАЯ РЕСПУБЛИКА)","ИРАК","ИРЛАНДИЯ","ИЗРАИЛЬ","ИТАЛИЯ","КОТ Д'ИВУАР","ЯМАЙКА","ЯПОНИЯ","МАЛЬДИВЫ",
                "КАЗАХСТАН","ИОРДАНИЯ","КЕНИЯ","КОРЕЯ, НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","КОРЕЯ, РЕСПУБЛИКА","КУВЕЙТ","КИРГИЗИЯ","НИГЕРИЯ","НИУЭ","ОСТРОВ НОРФОЛК","НОРВЕГИЯ","СЕВЕРНЫЕ МАРИАНСКИЕ ОСТРОВА",
                "ЛАОССКАЯ НАРОДНО-ДЕМОКРАТИЧЕСКАЯ РЕСПУБЛИКА","ЛИВАН","ЛЕСОТО","ЛАТВИЯ","ЛИБЕРИЯ","ЛИВИЯ","ЛИХТЕНШТЕЙН","ЛИТВА","ЛЮКСЕМБУРГ","МАКАО","МАДАГАСКАР","МАЛАВИ","МАЛАЙЗИЯ",
                "МАРТИНИКА","МАВРИТАНИЯ","МАВРИКИЙ","МЕКСИКА","МОНАКО","МОНГОЛИЯ","МОЛДОВА, РЕСПУБЛИКА","ЧЕРНОГОРИЯ","МОНТСЕРРАТ","МАРОККО","МОЗАМБИК","ОМАН","НАМИБИЯ","НАУРУ","НЕПАЛ",
                "АРУБА","СЕН-МАРТЕН (нидерландская часть)","МАЛЫЕ ТИХООКЕАНСКИЕ ОТДАЛЕННЫЕ ОСТРОВА СОЕДИНЕННЫХ ШТАТОВ","МИКРОНЕЗИЯ, ФЕДЕРАТИВНЫЕ ШТАТЫ","МАРШАЛЛОВЫ ОСТРОВА","КЮРАСАО",
                "ПАЛАУ","ПАКИСТАН","ПАНАМА","ПАПУА-НОВАЯ ГВИНЕЯ","ПАРАГВАЙ","ПЕРУ","ФИЛИППИНЫ","ПИТКЕРН","ПОЛЬША","ПОРТУГАЛИЯ","ГВИНЕЯ-БИСАУ","ТИМОР-ЛЕСТЕ","ШВЕЦИЯ","ШВЕЙЦАРИЯ","НИДЕРЛАНДЫ",
                "ПУЭРТО-РИКО","КАТАР","РЕЮНЬОН","РУМЫНИЯ","РОССИЯ","РУАНДА","СЕН-БАРТЕЛЕМИ","СВЯТАЯ ЕЛЕНА, ОСТРОВ ВОЗНЕСЕНИЯ, ТРИСТАН-ДА-КУНЬЯ","СЕНТ-КИТС И НЕВИС","АНГИЛЬЯ","СЕНТ-ЛЮСИЯ",
                "СЕН-МАРТЕН (французская часть)","СЕН-ПЬЕР И МИКЕЛОН","СЕНТ-ВИНСЕНТ И ГРЕНАДИНЫ","САН-МАРИНО","САН-ТОМЕ И ПРИНСИПИ","САУДОВСКАЯ АРАВИЯ","СЕНЕГАЛ","СЕРБИЯ","СЕЙШЕЛЫ","ЮЖНЫЙ СУДАН",
                "СЬЕРРА-ЛЕОНЕ","СИНГАПУР","СЛОВАКИЯ","ВЬЕТНАМ","СЛОВЕНИЯ","СОМАЛИ","ЮЖНАЯ АФРИКА","ЗИМБАБВЕ","ИСПАНИЯ","ЗАПАДНАЯ САХАРА","СУДАН","СУРИНАМ","ШПИЦБЕРГЕН И ЯН МАЙЕН","ЭСВАТИНИ",
                "СИРИЙСКАЯ АРАБСКАЯ РЕСПУБЛИКА","ТАДЖИКИСТАН","ТАИЛАНД","ТОГО","ТОКЕЛАУ","ТОНГА","ТРИНИДАД И ТОБАГО","ОБЪЕДИНЕННЫЕ АРАБСКИЕ ЭМИРАТЫ","ТУНИС","ТУРЦИЯ","ТУРКМЕНИСТАН","ОСТРОВА ТЕРКС И КАЙКОС",
                "ТУВАЛУ","УГАНДА","УКРАИНА","СЕВЕРНАЯ МАКЕДОНИЯ","ЕГИПЕТ","СОЕДИНЕННОЕ КОРОЛЕВСТВО","ГЕРНСИ","ДЖЕРСИ","ОСТРОВ МЭН","ТАНЗАНИЯ, ОБЪЕДИНЕННАЯ РЕСПУБЛИКА","СОЕДИНЕННЫЕ ШТАТЫ",
                "ВИРГИНСКИЕ ОСТРОВА (США)","БУРКИНА-ФАСО","УРУГВАЙ","УЗБЕКИСТАН","ВЕНЕСУЭЛА (БОЛИВАРИАНСКАЯ РЕСПУБЛИКА)","УОЛЛИС И ФУТУНА","САМОА","ЙЕМЕН","ЗАМБИЯ","АБХАЗИЯ","ЮЖНАЯ ОСЕТИЯ"
            };

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
            foreach (var item in Spravochniki.SprRadionuclids)
            {
                if (item.Item1.Equals(value.Value))
                {
                    return true;
                }
            }
            value.AddError("Недопустимое значение");
            return false;
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
        //If change this change validation
        private bool FactoryNumberRecoded_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //FactoryNumberRecoded property

        //ActivityOnCreation property
        public int? ActivityOnCreationId { get; set; }
        [Attributes.Form_Property("Активность на дату создания, Бк")]
        public virtual RamAccess<string> ActivityOnCreation
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ActivityOnCreation));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(ActivityOnCreation), value);
                }
                OnPropertyChanged(nameof(ActivityOnCreation));
            }
        }

        private bool ActivityOnCreation_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            var styles = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
               NumberStyles.AllowExponent;
            try
            {
                if (!(double.Parse(value.Value, styles, CultureInfo.CreateSpecificCulture("en-GB")) > 0))
                {
                    value.AddError("Число должно быть больше нуля"); return false;
                }
            }
            catch
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //ActivityOnCreation property

        //CreationDate property
        public int? CreationDateId { get; set; }
        [Attributes.Form_Property("Дата изготовления")]
        public virtual RamAccess<string> CreationDate
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(CreationDate));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CreationDate), value);
                }
                OnPropertyChanged(nameof(CreationDate));
            }
        }
        //If change this change validation
        private bool CreationDate_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors(); return true;
        }
        //CreationDate property

        ////CreatorOKPONote property
        //public virtual RamAccess<string> CreatorOKPONote
        //{
        //    get
        //    {

        //        {
        //            return DataAccess.Get<string>(nameof(CreatorOKPONote));//OK

        //        }

        //        {

        //        }
        //    }
        //    set
        //    {




        //        {
        //            DataAccess.Set(nameof(CreatorOKPONote), value);
        //        }
        //        OnPropertyChanged(nameof(CreatorOKPONote));
        //    }
        //}

        //private bool CreatorOKPONote_Validation(RamAccess<string> value)
        //{
        //    value.ClearErrors(); return true;
        //}
        ////CreatorOKPONote property

        //Kategory property
        public int? KategoryId { get; set; }
        [Attributes.Form_Property("Категория")]
        public virtual RamAccess<short> Kategory
        {
            get
            {
                return DataAccess.Get<short>(nameof(Kategory));
            }
            set
            {
                DataAccess.Set(nameof(Kategory), value);
                OnPropertyChanged(nameof(Kategory));
            }
        }

        private bool Kategory_Validation(RamAccess<short> value)//TODO
        {
            value.ClearErrors(); return true;
        }
        //Kategory property

        //NuclearMaterialPresence property
        public int? NuclearMaterialPresenceId { get; set; }
        [Attributes.Form_Property("Содержание ядерных материалов")]
        public virtual RamAccess<double> NuclearMaterialPresence
        {
            get
            {

                {
                    return DataAccess.Get<double>(nameof(NuclearMaterialPresence));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(NuclearMaterialPresence), value);
                }
                OnPropertyChanged(nameof(NuclearMaterialPresence));
            }
        }

        //NuclearMaterialPresence property

        //Certificateid property
        public int? CertificateidId { get; set; }
        [Attributes.Form_Property("Номер сертификата")]
        public virtual RamAccess<string> Certificateid
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Certificateid));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Certificateid), value);
                }
                OnPropertyChanged(nameof(Certificateid));
            }
        }

        //Certificateid property

        //ValidThru property
        public int? ValidThruId { get; set; }
        [Attributes.Form_Property("Действует по")]
        public virtual RamAccess<string> ValidThru
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ValidThru));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(ValidThru), value);
                }
                OnPropertyChanged(nameof(ValidThru));
            }
        }

        //ValidThru property
    }
}
