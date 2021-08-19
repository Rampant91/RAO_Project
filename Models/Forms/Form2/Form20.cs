using Models.DataAccess; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.0: Титульный лист годового отчета организации")]
    public class Form20 : Abstracts.Form
    {
        public Form20() : base()
        {
            FormNum.Value = "2.0";
            //NumberOfFields.Value = 19;
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

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //RegNo property
        #region  RegNo
        public string RegNo_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Рег. №")]
        public RamAccess<string> RegNo
        {
            get
            {
                    var tmp = new RamAccess<string>(RegNo_Validation, RegNo_DB);
                    tmp.PropertyChanged += RegNoValueChanged;
                    return tmp;
            }
            set
            {
                    RegNo_DB = value.Value;
                OnPropertyChanged(nameof(RegNo));
            }
        }
        private void RegNoValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                RegNo_DB = ((RamAccess<string>)Value).Value;
}
}
private bool RegNo_Validation(RamAccess<string> value)
        {
            return true;
        }
        //RegNo property
        #endregion

        //OrganUprav property
        #region  OrganUprav
        public string OrganUprav_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Орган управления")]
        public RamAccess<string> OrganUprav
        {
            get
            {
                    var tmp = new RamAccess<string>(OrganUprav_Validation, OrganUprav_DB);
                    tmp.PropertyChanged += OrganUpravValueChanged;
                    return tmp;
            }
            set
            {
                    OrganUprav_DB = value.Value;
                OnPropertyChanged(nameof(OrganUprav));
            }
        }
        private void OrganUpravValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                OrganUprav_DB = ((RamAccess<string>)Value).Value;
}
}
private bool OrganUprav_Validation(RamAccess<string> value)
        {
            return true;
        }
        //OrganUprav property
        #endregion

        //SubjectRF property
        #region SubjectRF 
        public string SubjectRF_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Субъект РФ")]
        public RamAccess<string> SubjectRF
        {
            get
            {
                    var tmp = new RamAccess<string>(SubjectRF_Validation, SubjectRF_DB);
                    tmp.PropertyChanged += SubjectRFValueChanged;
                    return tmp;
            }
            set
            {
                    SubjectRF_DB = value.Value;
                OnPropertyChanged(nameof(SubjectRF));
            }
        }
        private void SubjectRFValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                SubjectRF_DB = ((RamAccess<string>)Value).Value;
}
}
private bool SubjectRF_Validation(RamAccess<string> value)
        {
            return true;
        }
        //SubjectRF property
        #endregion

        //JurLico property
        #region  JurLico
        public string JurLico_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Юр. лицо")]
        public RamAccess<string> JurLico
        {
            get
            {
                    var tmp = new RamAccess<string>(JurLico_Validation, JurLico_DB);
                    tmp.PropertyChanged += JurLicoValueChanged;
                    return tmp;
            }
            set
            {
                    JurLico_DB = value.Value;
                OnPropertyChanged(nameof(JurLico));
            }
        }
        private void JurLicoValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                JurLico_DB = ((RamAccess<string>)Value).Value;
}
}
private bool JurLico_Validation(RamAccess<string> value)
        {
            return true;
        }
        //JurLico property
        #endregion

        //ShortJurLico property
        #region  ShortJurLico
        public string ShortJurLico_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Краткое наименование юр. лица")]
        public RamAccess<string> ShortJurLico
        {
            get
            {
                    var tmp = new RamAccess<string>(ShortJurLico_Validation, ShortJurLico_DB);
                    tmp.PropertyChanged += ShortJurLicoValueChanged;
                    return tmp;
            }
            set
            {
                    ShortJurLico_DB = value.Value;
                OnPropertyChanged(nameof(ShortJurLico));
            }
        }
        private void ShortJurLicoValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                ShortJurLico_DB = ((RamAccess<string>)Value).Value;
}
}
private bool ShortJurLico_Validation(RamAccess<string> value)
        {
            return true;
        }
        //ShortJurLico property
        #endregion

        //JurLicoAddress property
        #region  JurLicoAddress
        public string JurLicoAddress_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Адрес юр. лица")]
        public RamAccess<string> JurLicoAddress
        {
            get
            {
                    var tmp = new RamAccess<string>(JurLicoAddress_Validation, JurLicoAddress_DB);
                    tmp.PropertyChanged += JurLicoAddressValueChanged;
                    return tmp;
            }
            set
            {
                    JurLicoAddress_DB = value.Value;
                OnPropertyChanged(nameof(JurLicoAddress));
            }
        }
        private void JurLicoAddressValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                JurLicoAddress_DB = ((RamAccess<string>)Value).Value;
}
}
private bool JurLicoAddress_Validation(RamAccess<string> value)
        {
            return true;
        }
        //JurLicoAddress property
        #endregion

        //JurLicoFactAddress property
        #region  JurLicoFactAddress
        public string JurLicoFactAddress_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Фактический адрес юр. лица")]
        public RamAccess<string> JurLicoFactAddress
        {
            get
            {
                    var tmp = new RamAccess<string>(JurLicoFactAddress_Validation, JurLicoFactAddress_DB);
                    tmp.PropertyChanged += JurLicoFactAddressValueChanged;
                    return tmp;
            }
            set
            {
                    JurLicoFactAddress_DB = value.Value;
                OnPropertyChanged(nameof(JurLicoFactAddress));
            }
        }
        private void JurLicoFactAddressValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                JurLicoFactAddress_DB = ((RamAccess<string>)Value).Value;
}
}
private bool JurLicoFactAddress_Validation(RamAccess<string> value)
        {
            return true;
        }
        //JurLicoFactAddress property
        #endregion

        //GradeFIO property
        #region  GradeFIO
        public string GradeFIO_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ФИО, должность")]
        public RamAccess<string> GradeFIO
        {
            get
            {
                    var tmp = new RamAccess<string>(GradeFIO_Validation, GradeFIO_DB);
                    tmp.PropertyChanged += GradeFIOValueChanged;
                    return tmp;
            }
            set
            {
                    GradeFIO_DB = value.Value;
                OnPropertyChanged(nameof(GradeFIO));
            }
        }
        private void GradeFIOValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                GradeFIO_DB = ((RamAccess<string>)Value).Value;
}
}
private bool GradeFIO_Validation(RamAccess<string> value)
        {
            return true;
        }
        //GradeFIO property
        #endregion

        //Telephone property
        #region  Telephone
        public string Telephone_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Телефон")]
        public RamAccess<string> Telephone
        {
            get
            {
                    var tmp = new RamAccess<string>(Telephone_Validation, Telephone_DB);
                    tmp.PropertyChanged += TelephoneValueChanged;
                    return tmp;
            }
            set
            {
                    Telephone_DB = value.Value;
                OnPropertyChanged(nameof(Telephone));
            }
        }
        private void TelephoneValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Telephone_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Telephone_Validation(RamAccess<string> value)
        {
            return true;
        }
        //Telephone property
        #endregion

        //Fax property
        #region  Fax
        public string Fax_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Факс")]
        public RamAccess<string> Fax
        {
            get
            {
                    var tmp = new RamAccess<string>(Fax_Validation, Fax_DB);
                    tmp.PropertyChanged += FaxValueChanged;
                    return tmp;
            }
            set
            {
                    Fax_DB = value.Value;
                OnPropertyChanged(nameof(Fax));
            }
        }
        private void FaxValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Fax_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Fax_Validation(RamAccess<string> value)
        {
            return true;
        }
        //Fax property
        #endregion

        //Email property
        #region Email 
        public string Email_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("Эл. почта")]
        public RamAccess<string> Email
        {
            get
            {
                    var tmp = new RamAccess<string>(Email_Validation, Email_DB);
                    tmp.PropertyChanged += EmailValueChanged;
                    return tmp;
            }
            set
            {
                    Email_DB = value.Value;
                OnPropertyChanged(nameof(Email));
            }
        }
        private void EmailValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Email_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Email_Validation(RamAccess<string> value)
        {
            return true;
        }
        //Email property
        #endregion

        //Okpo property
        #region  Okpo
        public string Okpo_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ОКПО")]
        public RamAccess<string> Okpo
        {
            get
            {
                    var tmp = new RamAccess<string>(Okpo_Validation, Okpo_DB);
                    tmp.PropertyChanged += OkpoValueChanged;
                    return tmp;
            }
            set
            {
                    Okpo_DB = value.Value;
                OnPropertyChanged(nameof(Okpo));
            }
        }
        private void OkpoValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Okpo_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Okpo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            Regex mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Okpo property
        #endregion

        //Okved property
        #region  Okved
        public string Okved_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ОКВЭД")]
        public RamAccess<string> Okved
        {
            get
            {
                    var tmp = new RamAccess<string>(Okved_Validation, Okved_DB);
                    tmp.PropertyChanged += OkvedValueChanged;
                    return tmp;
            }
            set
            {
                    Okved_DB = value.Value;
                OnPropertyChanged(nameof(Okved));
            }
        }

        private void OkvedValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Okved_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Okved_Validation(RamAccess<string> value)
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
        //Okved property
        #endregion

        //Okogu property
        #region  Okogu
        public string Okogu_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ОКОГУ")]
        public RamAccess<string> Okogu
        {
            get
            {
                    var tmp = new RamAccess<string>(Okogu_Validation, Okogu_DB);
                    tmp.PropertyChanged += OkoguValueChanged;
                    return tmp;
            }
            set
            {
                    Okogu_DB = value.Value;
                OnPropertyChanged(nameof(Okogu));
            }
        }

        private void OkoguValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Okogu_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Okogu_Validation(RamAccess<string> value)
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
        //Okogu property
        #endregion

        //Oktmo property
        #region  Oktmo
        public string Oktmo_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ОКТМО")]
        public RamAccess<string> Oktmo
        {
            get
            {
                    var tmp = new RamAccess<string>(Oktmo_Validation, Oktmo_DB);
                    tmp.PropertyChanged += OktmoValueChanged;
                    return tmp;
            }
            set
            {
                    Oktmo_DB = value.Value;
                OnPropertyChanged(nameof(Oktmo));
            }
        }

        private void OktmoValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Oktmo_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Oktmo_Validation(RamAccess<string> value)
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
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Oktmo property
        #endregion

        //Inn property
        #region Inn 
        public string Inn_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ИНН")]
        public RamAccess<string> Inn
        {
            get
            {
                    var tmp = new RamAccess<string>(Inn_Validation, Inn_DB);
                    tmp.PropertyChanged += InnValueChanged;
                    return tmp;
            }
            set
            {
                    Inn_DB = value.Value;
                OnPropertyChanged(nameof(Inn));
            }
        }

        private void InnValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Inn_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Inn_Validation(RamAccess<string> value)
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
        //Inn property
        #endregion

        //Kpp property
        #region  Kpp
        public string Kpp_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("КПП")]
        public RamAccess<string> Kpp
        {
            get
            {
                    var tmp = new RamAccess<string>(Kpp_Validation, Kpp_DB);
                    tmp.PropertyChanged += KppValueChanged;
                    return tmp;
            }
            set
            {
                    Kpp_DB = value.Value;
                OnPropertyChanged(nameof(Kpp));
            }
        }

        private void KppValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Kpp_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Kpp_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("^[0-9]{9}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Kpp property
        #endregion

        //Okopf property
        #region  Okopf
        public string Okopf_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ОКОПФ")]
        public RamAccess<string> Okopf
        {
            get
            {
                    var tmp = new RamAccess<string>(Okopf_Validation, Okopf_DB);
                    tmp.PropertyChanged += OkopfValueChanged;
                    return tmp;
            }
            set
            {
                    Okopf_DB = value.Value;
                OnPropertyChanged(nameof(Okopf));
            }
        }

        private void OkopfValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Okopf_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Okopf_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("^[0-9]{5}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Okopf property
        #endregion

        //Okfs property
        #region  
public string Okfs_DB { get; set; } = ""; [NotMapped]
        [Attributes.Form_Property("ОКФС")]
        public RamAccess<string> Okfs
        {
            get
            {
                    var tmp = new RamAccess<string>(Okfs_Validation, Okfs_DB);
                    tmp.PropertyChanged += OkfsValueChanged;
                    return tmp;
            }
            set
            {
                    Okfs_DB = value.Value;
                OnPropertyChanged(nameof(Okfs));
            }
        }

        private void OkfsValueChanged(object Value, PropertyChangedEventArgs args)
{
if (args.PropertyName == "Value")
{
                Okfs_DB = ((RamAccess<string>)Value).Value;
}
}
private bool Okfs_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            Regex ex = new Regex("^[0-9]{2}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Okfs property
        #endregion
    }
}
