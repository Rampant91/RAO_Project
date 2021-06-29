using Models.DataAccess;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.0: Титульный лист оперативного отчета организации")]
    public class Form10 : Abstracts.Form
    {
        public Form10() : base()
        {
            ////NumberOfFields.Value = 19;
            ////FormNum.Value = "10";
            Init_base();
            Validate_base();
        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        private void Init_base()
        {
            DataAccess.Init<string>(nameof(Okpo), Okpo_Validation, null);
            Okpo.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Okved), Okved_Validation, null);
            Okved.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Oktmo), Oktmo_Validation, null);
            Oktmo.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Okogu), Okogu_Validation, null);
            Oktmo.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Okopf), Okopf_Validation, null);
            Okopf.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Inn), Inn_Validation, null);
            Inn.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Kpp), Kpp_Validation, null);
            Kpp.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(RegNo), RegNo_Validation, null);
            RegNo.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(OrganUprav), OrganUprav_Validation, null);
            OrganUprav.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(SubjectRF), SubjectRF_Validation, null);
            SubjectRF.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(JurLico), JurLico_Validation, null);
            OrganUprav.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(ShortJurLico), ShortJurLico_Validation, null);
            ShortJurLico.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(JurLicoAddress), JurLicoAddress_Validation, null);
            JurLicoAddress.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(JurLicoFactAddress), JurLicoFactAddress_Validation, null);
            JurLicoFactAddress.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(GradeFIO), GradeFIO_Validation, null);
            GradeFIO.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Telephone), Telephone_Validation, null);
            Telephone.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Fax), Fax_Validation, null);
            Fax.PropertyChanged += InPropertyChanged;
            DataAccess.Init<string>(nameof(Email), Email_Validation, null);
            Email.PropertyChanged += InPropertyChanged;
        }
        protected void Validate_base()
        {
            Okpo_Validation(Okpo);
            Okved_Validation(Okved);
            Oktmo_Validation(Oktmo);
            Okogu_Validation(Okogu);
            Okopf_Validation(Okopf);
            Inn_Validation(Inn);
            Kpp_Validation(Kpp);
            RegNo_Validation(RegNo);
            OrganUprav_Validation(OrganUprav);
            SubjectRF_Validation(SubjectRF);
            JurLico_Validation(JurLico);
            ShortJurLico_Validation(ShortJurLico);
            JurLicoAddress_Validation(JurLicoAddress);
            JurLicoFactAddress_Validation(JurLicoFactAddress);
            GradeFIO_Validation(GradeFIO);
            Telephone_Validation(Telephone);
            Fax_Validation(Fax);
            Email_Validation(Email);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //RegNo property
        public int? RegNoId { get; set; }
        [Attributes.Form_Property("Рег. №")]
        public virtual RamAccess<string> RegNo
        {
            get => DataAccess.Get<string>(nameof(RegNo));
            set
            {
                DataAccess.Set(nameof(RegNo), value);
                OnPropertyChanged(nameof(RegNo));
            }
        }

        private bool RegNo_Validation(RamAccess<string> value)
        {
            return true;
        }
        //RegNo property

        //OrganUprav property
        public int? OrganUpravId { get; set; }
        [Attributes.Form_Property("Орган управления")]
        public virtual RamAccess<string> OrganUprav
        {
            get => DataAccess.Get<string>(nameof(OrganUprav));
            set
            {
                DataAccess.Set(nameof(OrganUprav), value);
                OnPropertyChanged(nameof(OrganUprav));
            }
        }

        private bool OrganUprav_Validation(RamAccess<string> value)
        {
            return true;
        }

        //OrganUprav property

        //SubjectRF property
        public int? SubjectRFId { get; set; }
        [Attributes.Form_Property("Субъект РФ")]
        public virtual RamAccess<string> SubjectRF
        {
            get => DataAccess.Get<string>(nameof(SubjectRF));
            set
            {
                DataAccess.Set(nameof(SubjectRF), value);
                OnPropertyChanged(nameof(SubjectRF));
            }
        }

        private bool SubjectRF_Validation(RamAccess<string> value)
        {
            return true;
        }

        //SubjectRF property

        //JurLico property
        public int? JurLicoId { get; set; }
        [Attributes.Form_Property("Юр. лицо")]
        public virtual RamAccess<string> JurLico
        {
            get => DataAccess.Get<string>(nameof(JurLico));
            set
            {
                DataAccess.Set(nameof(JurLico), value);
                OnPropertyChanged(nameof(JurLico));
            }
        }

        private bool JurLico_Validation(RamAccess<string> value)
        {
            return true;
        }
        //JurLico property

        //ShortJurLico property
        public int? ShortJurLicoId { get; set; }
        [Attributes.Form_Property("Краткое наименование юр. лица")]
        public virtual RamAccess<string> ShortJurLico
        {
            get => DataAccess.Get<string>(nameof(ShortJurLico));
            set
            {
                DataAccess.Set(nameof(ShortJurLico), value);
                OnPropertyChanged(nameof(ShortJurLico));
            }
        }

        private bool ShortJurLico_Validation(RamAccess<string> value)
        {
            return true;
        }
        //ShortJurLico property

        //JurLicoAddress property
        public int? JurLicoAddressId { get; set; }
        [Attributes.Form_Property("Адрес юр. лица")]
        public virtual RamAccess<string> JurLicoAddress
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(JurLicoAddress));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(JurLicoAddress), value);
                }
                OnPropertyChanged(nameof(JurLicoAddress));
            }
        }
        private bool JurLicoAddress_Validation(RamAccess<string> value)
        {
            return true;
        }
        //JurLicoAddress property

        //JurLicoFactAddress property
        public int? JurLicoFactAddressId { get; set; }
        [Attributes.Form_Property("Фактический адрес юр. лица")]
        public virtual RamAccess<string> JurLicoFactAddress
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(JurLicoFactAddress));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(JurLicoFactAddress), value);
                }
                OnPropertyChanged(nameof(JurLicoFactAddress));
            }
        }
        private bool JurLicoFactAddress_Validation(RamAccess<string> value)
        {
            return true;
        }
        //JurLicoFactAddress property

        //GradeFIO property
        [Attributes.Form_Property("ФИО, должность")]public int? GradeFIOId { get; set; }
        public virtual RamAccess<string> GradeFIO
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(GradeFIO));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(GradeFIO), value);
                }
                OnPropertyChanged(nameof(GradeFIO));
            }
        }
        private bool GradeFIO_Validation(RamAccess<string> value)
        {
            return true;
        }
        //GradeFIO property

        //Telephone property
        [Attributes.Form_Property("Телефон")]public int? TelephoneId { get; set; }
        public virtual RamAccess<string> Telephone
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Telephone));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Telephone), value);
                }
                OnPropertyChanged(nameof(Telephone));
            }
        }

        private bool Telephone_Validation(RamAccess<string> value)
        {
            return true;
        }
        //Telephone property

        //Fax property
        [Attributes.Form_Property("Факс")]public int? FaxId { get; set; }
        public virtual RamAccess<string> Fax
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Fax));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Fax), value);
                }
                OnPropertyChanged(nameof(Fax));
            }
        }

        private bool Fax_Validation(RamAccess<string> value)
        {
            return true;
        }
        //Fax property

        //Email property
        [Attributes.Form_Property("Эл. почта")]public int? EmailId { get; set; }
        public virtual RamAccess<string> Email
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Email));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Email), value);
                }
                OnPropertyChanged(nameof(Email));
            }
        }
        private bool Email_Validation(RamAccess<string> value)
        {
            return true;
        }
        //Email property

        //Okpo property
        public int? OkpoId { get; set; }

        [Attributes.Form_Property("ОКПО")]        
        public virtual RamAccess<string> Okpo
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Okpo));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Okpo), value);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }

        private bool Okpo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
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
        //Okpo property

        //Okved property
        [Attributes.Form_Property("ОКВЭД")]public int? OkvedId { get; set; }
        public virtual RamAccess<string> Okved
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Okved));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Okved), value);
                }
                OnPropertyChanged(nameof(Okved));
            }
        }

        private bool Okved_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            Regex ex = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Okved property

        //Okogu property
        [Attributes.Form_Property("ОКОГУ")]public int? OkoguId { get; set; }
        public virtual RamAccess<string> Okogu
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Okogu));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Okogu), value);
                }
                OnPropertyChanged(nameof(Okogu));
            }
        }

        private bool Okogu_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            Regex ex = new Regex("^[0-9]{5}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Okogu property

        //Oktmo property
        [Attributes.Form_Property("ОКТМО")]public int? OktmoId { get; set; }
        public virtual RamAccess<string> Oktmo
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Oktmo));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Oktmo), value);
                }
                OnPropertyChanged(nameof(Oktmo));
            }
        }

        private bool Oktmo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            Regex ex = new Regex("^[0-9]{11}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Oktmo property

        //Inn property
        [Attributes.Form_Property("ИНН")]public int? InnId { get; set; }
        public virtual RamAccess<string> Inn
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Inn));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Inn), value);
                }
                OnPropertyChanged(nameof(Inn));
            }
        }

        private bool Inn_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            Regex ex = new Regex("[0-9]{10}");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Inn property

        //Kpp property
        [Attributes.Form_Property("КПП")]public int? KppId { get; set; }
        public virtual RamAccess<string> Kpp
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Kpp));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Kpp), value);
                }
                OnPropertyChanged(nameof(Kpp));
            }
        }

        private bool Kpp_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            Regex ex = new Regex("[0-9]{9}");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Kpp property

        //Okopf property
        [Attributes.Form_Property("ОКОПФ")]public int? OkopfId { get; set; }
        public virtual RamAccess<string> Okopf
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Okopf));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Okopf), value);
                }
                OnPropertyChanged(nameof(Okopf));
            }
        }

        private bool Okopf_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            Regex ex = new Regex("^[0-9]{5}^");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Okopf property

        //Okfs property
        [Attributes.Form_Property("ОКФС")]public int? OkfsId { get; set; }
        public virtual RamAccess<string> Okfs
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Okfs));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Okfs), value);
                }
                OnPropertyChanged(nameof(Okfs));
            }
        }

        private bool Okfs_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            Regex ex = new Regex("^[0-9]{2}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Okfs property
    }
}
