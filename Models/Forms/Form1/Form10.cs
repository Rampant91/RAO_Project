using Models.DataAccess;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.0: Титульный лист оперативного отчета организации")]
    public class Form10 : Abstracts.Form
    {
        public Form10() : base()
        {
            FormNum = "10";
            Validate_base();
        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }
        protected void Validate_base()
        {
            Okfs_Validation(Okfs);
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
        public override bool Object_Validation()
        {
            return false;
        }

        #region RegNo
        public string RegNo_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Рег. №")]
        public RamAccess<string> RegNo
        {
            get => new RamAccess<string>(RegNo_Validation, RegNo_DB);
            set
            {
                RegNo_DB = value.Value;
                OnPropertyChanged(nameof(RegNo));
            }
        }
        private bool RegNo_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region OrganUprav
        public string OrganUprav_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Орган управления")]
        public RamAccess<string> OrganUprav
        {
            get => new RamAccess<string>(OrganUprav_Validation, OrganUprav_DB);
            set
            {
                OrganUprav_DB = value.Value;
                OnPropertyChanged(nameof(OrganUprav));
            }
        }
        private bool OrganUprav_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region SubjectRF
        public string SubjectRF_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Субъект РФ")]
        public RamAccess<string> SubjectRF
        {
            get => new RamAccess<string>(SubjectRF_Validation, SubjectRF_DB);
            set
            {
                SubjectRF_DB = value.Value;
                OnPropertyChanged(nameof(SubjectRF));
            }
        }
        private bool SubjectRF_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region JurLico
        public string JurLico_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Юр. лицо")]
        public RamAccess<string> JurLico
        {
            get => new RamAccess<string>(JurLico_Validation, JurLico_DB);
            set
            {
                JurLico_DB = value.Value;
                OnPropertyChanged(nameof(JurLico));
            }
        }
        private bool JurLico_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region ShortJurLico
        public string ShortJurLico_DB { get; set; } = 0;
        [NotMapped]
        [Attributes.Form_Property("Краткое наименование юр. лица")]
        public RamAccess<string> ShortJurLico
        {
            get => new RamAccess<string>(ShortJurLico_Validation, ShortJurLico_DB);
            set
            {
                ShortJurLico_DB = value.Value;
                OnPropertyChanged(nameof(ShortJurLico));
            }
        }
        private bool ShortJurLico_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region JurLicoAddress
        public string JurLicoAddress_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Адрес юр. лица")]
        public RamAccess<string> JurLicoAddress
        {
            get => new RamAccess<string>(JurLicoAddress_Validation, JurLicoAddress_DB);
            set
            {
                JurLicoAddress_DB = value.Value;
                OnPropertyChanged(nameof(JurLicoAddress));
            }
        }
        private bool JurLicoAddress_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region JurLicoFactAddress
        public string JurLicoFactAddress_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Фактический адрес юр. лица")]
        public RamAccess<string> JurLicoFactAddress
        {
            get => new RamAccess<string>(JurLicoFactAddress_Validation, JurLicoFactAddress_DB);
            set
            {
                JurLicoFactAddress_DB = value.Value;
                OnPropertyChanged(nameof(JurLicoFactAddress));
            }
        }
        private bool JurLicoFactAddress_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region GradeFIO
        public string GradeFIO_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ФИО, должность")]
        public RamAccess<string> GradeFIO
        {
            get => new RamAccess<string>(GradeFIO_Validation, GradeFIO_DB);
            set
            {
                GradeFIO_DB = value.Value;
                OnPropertyChanged(nameof(GradeFIO));
            }
        }
        private bool GradeFIO_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region Telephone
        public string Telephone_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Телефон")]
        public RamAccess<string> Telephone
        {
            get => new RamAccess<string>(Telephone_Validation, Telephone_DB);
            set
            {
                Telephone_DB = value.Value;
                OnPropertyChanged(nameof(Telephone));
            }
        }
        private bool Telephone_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region Fax
        public string Fax_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Факс")]
        public RamAccess<string> Fax
        {
            get => new RamAccess<string>(Fax_Validation, Fax_DB);
            set
            {
                Fax_DB = value.Value;
                OnPropertyChanged(nameof(Fax));
            }
        }
        private bool Fax_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region Email
        public string Email_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("Эл. почта")]
        public RamAccess<string> Email
        {
            get => new RamAccess<string>(Email_Validation, Email_DB);
            set
            {
                Email_DB = value.Value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private bool Email_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            return true;
        }
        #endregion

        #region Okpo
        public string Okpo_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ОКПО")]
        public RamAccess<string> Okpo
        {
            get => new RamAccess<string>(Okpo_Validation, Okpo_DB);
            set
            {
                Okpo_DB = value.Value;
                OnPropertyChanged(nameof(Okpo));
            }
        }
        private bool Okpo_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
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
        #endregion

        #region Okved
        public string Okved_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ОКВЭД")]
        public RamAccess<string> Okved
        {
            get => new RamAccess<string>(Okved_Validation, Okved_DB);
            set
            {
                Okved_DB = value.Value;
                OnPropertyChanged(nameof(Okved));
            }
        }
        private bool Okved_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region Okogu
        public string Okogu_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ОКОГУ")]
        public RamAccess<string> Okogu
        {
            get => new RamAccess<string>(Okogu_Validation, Okogu_DB);
            set
            {
                Okogu_DB = value.Value;
                OnPropertyChanged(nameof(Okogu));
            }
        }
        private bool Okogu_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("^[0-9]{7}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region Oktmo
        public string Oktmo_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ОКТМО")]
        public RamAccess<string> Oktmo
        {
            get => new RamAccess<string>(Oktmo_Validation, Oktmo_DB);
            set
            {
                Oktmo_DB = value.Value;
                OnPropertyChanged(nameof(Oktmo));
            }
        }
        private bool Oktmo_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("^[0-9]{11}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region Inn
        public string Inn_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ИНН")]
        public RamAccess<string> Inn
        {
            get => new RamAccess<string>(Inn_Validation, Inn_DB);
            set
            {
                Inn_DB = value.Value;
                OnPropertyChanged(nameof(Inn));
            }
        }
        private bool Inn_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("^[0-9]{10}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region Kpp
        public string Kpp_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("КПП")]
        public RamAccess<string> Kpp
        {
            get => new RamAccess<string>(Kpp_Validation, Kpp_DB);
            set
            {
                Kpp_DB = value.Value;
                OnPropertyChanged(nameof(Kpp));
            }
        }
        private bool Kpp_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("[0-9]{9}");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region Okopf
        public string Okopf_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ОКОПФ")]
        public RamAccess<string> Okopf
        {
            get => new RamAccess<string>(Okopf_Validation, Okopf_DB);
            set
            {
                Okopf_DB = value.Value;
                OnPropertyChanged(nameof(Okopf));
            }
        }
        private bool Okopf_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("[0-9]{5}");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion

        #region Okfs
        public string Okfs_DB { get; set; } = "";
        [NotMapped]
        [Attributes.Form_Property("ОКФС")]
        public RamAccess<string> Okfs
        {
            get => new RamAccess<string>(Okfs_Validation, Okfs_DB);
            set
            {
                Okfs_DB = value.Value;
                OnPropertyChanged(nameof(Okfs));
            }
        }
        private bool Okfs_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("[0-9]{5}");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        #endregion
    }
}
