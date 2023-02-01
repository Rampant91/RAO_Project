using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.RegularExpressions;
using Models.Attributes;
using Models.Collections;
using Models.Forms.DataAccess;
using OfficeOpenXml;

namespace Models.Forms.Form2;

[Serializable]
[Form_Class("Форма 2.0: Титульный лист годового отчета организации")]
public class Form20 : Form
{
    public Form20()
    {
        FormNum.Value = "2.0";
    }
    protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
    {
        OnPropertyChanged(args.PropertyName);
    }
    public override bool Object_Validation()
    {
        return !(Okfs.HasErrors ||
                 Okpo.HasErrors ||
                 Okved.HasErrors ||
                 Oktmo.HasErrors ||
                 Okogu.HasErrors ||
                 Okopf.HasErrors ||
                 Inn.HasErrors ||
                 Kpp.HasErrors ||
                 RegNo.HasErrors ||
                 OrganUprav.HasErrors ||
                 SubjectRF.HasErrors ||
                 JurLico.HasErrors ||
                 ShortJurLico.HasErrors ||
                 JurLicoAddress.HasErrors ||
                 JurLicoFactAddress.HasErrors ||
                 GradeFIO.HasErrors ||
                 Telephone.HasErrors ||
                 Fax.HasErrors ||
                 Email.HasErrors);
    }

    #region RegNo

    public string RegNo_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"Рег. №")]
    public RamAccess<string> RegNo
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(RegNo)))
            {
                ((RamAccess<string>)Dictionary[nameof(RegNo)]).ValueWithOutHandlerAndPropChanged = RegNo_DB;
                return (RamAccess<string>)Dictionary[nameof(RegNo)];
            }
            else
            {
                var rm = new RamAccess<string>(RegNo_Validation, RegNo_DB);
                rm.PropertyChanged += RegNoValueChanged;
                Dictionary.Add(nameof(RegNo), rm);
                return (RamAccess<string>)Dictionary[nameof(RegNo)];
            }
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
            RegNo_DB = ((RamAccess<string>) Value).Value;
        }
    }

    private bool RegNo_Validation(RamAccess<string> value)
    {
        value.ClearErrors();
        if (string.IsNullOrEmpty(value.Value))
        {
            value.AddError("Поле не заполнено");
            return false;
        }
        if (value.Value == "-")
        {
            return true;
        }
        if (value.Value.Length != 5)
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        Regex mask = new("^[0123456789]{5}$");
        if (!mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //RegNo property

    #endregion

    #region OrganUprav

    public string OrganUprav_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"Орган управления использованием атомной энергии")]
    public RamAccess<string> OrganUprav
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(OrganUprav)))
            {
                ((RamAccess<string>)Dictionary[nameof(OrganUprav)]).Value = OrganUprav_DB;
                return (RamAccess<string>)Dictionary[nameof(OrganUprav)];
            }
            else
            {
                var rm = new RamAccess<string>(OrganUprav_Validation, OrganUprav_DB);
                rm.PropertyChanged += OrganUpravValueChanged;
                Dictionary.Add(nameof(OrganUprav), rm);
                return (RamAccess<string>)Dictionary[nameof(OrganUprav)];
            }
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
            OrganUprav_DB = ((RamAccess<string>) Value).Value;
        }
    }

    private bool OrganUprav_Validation(RamAccess<string> value)
    {
        return true;
    }

    //OrganUprav property

    #endregion

    #region SubjectRF

    public string SubjectRF_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Субъект Российской Федерации", "Субъект Российской Федерации")]
    public RamAccess<string> SubjectRF
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(SubjectRF)))
            {
                ((RamAccess<string>)Dictionary[nameof(SubjectRF)]).Value = SubjectRF_DB;
                return (RamAccess<string>)Dictionary[nameof(SubjectRF)];
            }
            else
            {
                var rm = new RamAccess<string>(SubjectRF_Validation, SubjectRF_DB);
                rm.PropertyChanged += SubjectRFValueChanged;
                Dictionary.Add(nameof(SubjectRF), rm);
                return (RamAccess<string>)Dictionary[nameof(SubjectRF)];
            }
        }
        set
        {
            SubjectRF_DB = ParseInnerText(value.Value);
            OnPropertyChanged(nameof(SubjectRF));
        }
    }

    private void SubjectRFValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            SubjectRF_DB = ParseInnerText(((RamAccess<string>) Value).Value);
        }
    }

    private bool SubjectRF_Validation(RamAccess<string> value)
    {
        return true;
    }

    //SubjectRF property

    #endregion

    #region JurLico

    public string JurLico_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Наименование юридического лица", "Наименование обособленного подразделения")]
    public RamAccess<string> JurLico
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(JurLico)))
            {
                ((RamAccess<string>)Dictionary[nameof(JurLico)]).Value = JurLico_DB;
                return (RamAccess<string>)Dictionary[nameof(JurLico)];
            }
            else
            {
                var rm = new RamAccess<string>(JurLico_Validation, JurLico_DB);
                rm.PropertyChanged += JurLicoValueChanged;
                Dictionary.Add(nameof(JurLico), rm);
                return (RamAccess<string>)Dictionary[nameof(JurLico)];
            }
        }
        set
        {
            JurLico_DB = ParseInnerText(value.Value);
            OnPropertyChanged(nameof(JurLico));
        }
    }

    private void JurLicoValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            JurLico_DB = ParseInnerText(((RamAccess<string>) Value).Value);
        }
    }

    private bool JurLico_Validation(RamAccess<string> value)
    {
        return true;
    }

    //JurLico property

    #endregion

    #region ShortJurLico

    public string ShortJurLico_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Сокращенное наименование", "Сокращенное наименование")]
    public RamAccess<string> ShortJurLico
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(ShortJurLico)))
            {
                ((RamAccess<string>)Dictionary[nameof(ShortJurLico)]).ValueWithOutHandlerAndPropChanged = ShortJurLico_DB;
                return (RamAccess<string>)Dictionary[nameof(ShortJurLico)];
            }
            else
            {
                var rm = new RamAccess<string>(ShortJurLico_Validation, ShortJurLico_DB);
                rm.PropertyChanged += ShortJurLicoValueChanged;
                Dictionary.Add(nameof(ShortJurLico), rm);
                return (RamAccess<string>)Dictionary[nameof(ShortJurLico)];
            }
        }
        set
        {
            ShortJurLico_DB = ParseInnerText(value.Value);
            OnPropertyChanged(nameof(ShortJurLico));
        }
    }

    private void ShortJurLicoValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            ShortJurLico_DB = ParseInnerText(((RamAccess<string>) Value).Value);
        }
    }

    private bool ShortJurLico_Validation(RamAccess<string> value)
    {
        return true;
    }

    //ShortJurLico property

    #endregion

    #region JurLicoAddress

    public string JurLicoAddress_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Адрес места нахождения юридического лица", "Адрес места нахождения обособленного подразделения")]
    public RamAccess<string> JurLicoAddress
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(JurLicoAddress)))
            {
                ((RamAccess<string>)Dictionary[nameof(JurLicoAddress)]).Value = JurLicoAddress_DB;
                return (RamAccess<string>)Dictionary[nameof(JurLicoAddress)];
            }
            else
            {
                var rm = new RamAccess<string>(JurLicoAddress_Validation, JurLicoAddress_DB);
                rm.PropertyChanged += JurLicoAddressValueChanged;
                Dictionary.Add(nameof(JurLicoAddress), rm);
                return (RamAccess<string>)Dictionary[nameof(JurLicoAddress)];
            }
        }
        set
        {
            JurLicoAddress_DB = ParseInnerText(value.Value);
            OnPropertyChanged(nameof(JurLicoAddress));
        }
    }

    private void JurLicoAddressValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            JurLicoAddress_DB = ParseInnerText(((RamAccess<string>) Value).Value);
        }
    }

    private bool JurLicoAddress_Validation(RamAccess<string> value)
    {
        return true;
    }

    //JurLicoAddress property

    #endregion

    #region JurLicoFactAddress

    public string JurLicoFactAddress_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true, "Фактический адрес юр. лица", "Фактический адрес обособленного подразделения")]
    public RamAccess<string> JurLicoFactAddress
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(JurLicoFactAddress)))
            {
                ((RamAccess<string>)Dictionary[nameof(JurLicoFactAddress)]).Value = JurLicoFactAddress_DB;
                return (RamAccess<string>)Dictionary[nameof(JurLicoFactAddress)];
            }
            else
            {
                var rm = new RamAccess<string>(JurLicoFactAddress_Validation, JurLicoFactAddress_DB);
                rm.PropertyChanged += JurLicoFactAddressValueChanged;
                Dictionary.Add(nameof(JurLicoFactAddress), rm);
                return (RamAccess<string>)Dictionary[nameof(JurLicoFactAddress)];
            }
        }
        set
        {
            JurLicoFactAddress_DB = ParseInnerText(value.Value);
            OnPropertyChanged(nameof(JurLicoFactAddress));
        }
    }

    private void JurLicoFactAddressValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            JurLicoFactAddress_DB = ParseInnerText(((RamAccess<string>) Value).Value);
        }
    }

    private bool JurLicoFactAddress_Validation(RamAccess<string> value)
    {
        return true;
    }

    //JurLicoFactAddress property

    #endregion

    #region GradeFIO

    public string GradeFIO_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"ФИО, должность руководителя", "ФИО, должность руководителя")]
    public RamAccess<string> GradeFIO
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(GradeFIO)))
            {
                ((RamAccess<string>)Dictionary[nameof(GradeFIO)]).Value = GradeFIO_DB;
                return (RamAccess<string>)Dictionary[nameof(GradeFIO)];
            }
            else
            {
                var rm = new RamAccess<string>(GradeFIO_Validation, GradeFIO_DB);
                rm.PropertyChanged += GradeFIOValueChanged;
                Dictionary.Add(nameof(GradeFIO), rm);
                return (RamAccess<string>)Dictionary[nameof(GradeFIO)];
            }
        }
        set
        {
            GradeFIO_DB = ParseInnerText(value.Value);
            OnPropertyChanged(nameof(GradeFIO));
        }
    }

    private void GradeFIOValueChanged(object Value, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            GradeFIO_DB = ParseInnerText(((RamAccess<string>) Value).Value);
        }
    }

    private bool GradeFIO_Validation(RamAccess<string> value)
    {
        return true;
    }

    //GradeFIO property

    #endregion

    #region Telephone

    public string Telephone_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"Телефон организации", "Телефон организации")]
    public RamAccess<string> Telephone
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Telephone)))
            {
                ((RamAccess<string>)Dictionary[nameof(Telephone)]).Value = Telephone_DB;
                return (RamAccess<string>)Dictionary[nameof(Telephone)];
            }
            else
            {
                var rm = new RamAccess<string>(Telephone_Validation, Telephone_DB);
                rm.PropertyChanged += TelephoneValueChanged;
                Dictionary.Add(nameof(Telephone), rm);
                return (RamAccess<string>)Dictionary[nameof(Telephone)];
            }
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
            Telephone_DB = ((RamAccess<string>) Value).Value;
        }
    }

    private bool Telephone_Validation(RamAccess<string> value)
    {
        return true;
    }

    //Telephone property

    #endregion

    #region Fax

    public string Fax_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"Факс организации", "Факс организации")]
    public RamAccess<string> Fax
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Fax)))
            {
                ((RamAccess<string>)Dictionary[nameof(Fax)]).Value = Fax_DB;
                return (RamAccess<string>)Dictionary[nameof(Fax)];
            }
            else
            {
                var rm = new RamAccess<string>(Fax_Validation, Fax_DB);
                rm.PropertyChanged += FaxValueChanged;
                Dictionary.Add(nameof(Fax), rm);
                return (RamAccess<string>)Dictionary[nameof(Fax)];
            }
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
            Fax_DB = ((RamAccess<string>) Value).Value;
        }
    }

    private bool Fax_Validation(RamAccess<string> value)
    {
        return true;
    }

    //Fax property

    #endregion

    #region Email

    public string Email_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"Эл. почта организации", "Эл. почта организации")]
    public RamAccess<string> Email
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Email)))
            {
                ((RamAccess<string>)Dictionary[nameof(Email)]).Value = Email_DB;
                return (RamAccess<string>)Dictionary[nameof(Email)];
            }
            else
            {
                var rm = new RamAccess<string>(Email_Validation, Email_DB);
                rm.PropertyChanged += EmailValueChanged;
                Dictionary.Add(nameof(Email), rm);
                return (RamAccess<string>)Dictionary[nameof(Email)];
            }
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
            Email_DB = ((RamAccess<string>) Value).Value;
        }
    }

    private bool Email_Validation(RamAccess<string> value)
    {
        return true;
    }

    //Email property

    #endregion

    #region Okpo

    public string Okpo_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"ОКПО", "ОКПО")]
    public RamAccess<string> Okpo
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Okpo)))
            {
                ((RamAccess<string>)Dictionary[nameof(Okpo)]).ValueWithOutHandlerAndPropChanged = Okpo_DB;
                return (RamAccess<string>)Dictionary[nameof(Okpo)];
            }
            else
            {
                var rm = new RamAccess<string>(Okpo_Validation, Okpo_DB);
                rm.PropertyChanged += OkpoValueChanged;
                Dictionary.Add(nameof(Okpo), rm);
                return (RamAccess<string>)Dictionary[nameof(Okpo)];
            }
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
            Okpo_DB = ((RamAccess<string>) Value).Value;
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

        if (value.Value.Length != 8 && value.Value.Length != 14)
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        Regex mask = new("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
        if (!mask.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //Okpo property

    #endregion

    #region Okved

    public string Okved_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"ОКВЭД", "ОКВЭД")]
    public RamAccess<string> Okved
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Okved)))
            {
                ((RamAccess<string>)Dictionary[nameof(Okved)]).Value = Okved_DB;
                return (RamAccess<string>)Dictionary[nameof(Okved)];
            }
            else
            {
                var rm = new RamAccess<string>(Okved_Validation, Okved_DB);
                rm.PropertyChanged += OkvedValueChanged;
                Dictionary.Add(nameof(Okved), rm);
                return (RamAccess<string>)Dictionary[nameof(Okved)];
            }
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
            Okved_DB = ((RamAccess<string>) Value).Value;
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

        Regex ex = new(@"^[0-9]{2}(|\.[0-9]{1,2})(|\.[0-9]{1,2})$");
        if (!ex.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //Okved property

    #endregion

    #region Okogu

    public string Okogu_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"ОКОГУ", "ОКОГУ")]
    public RamAccess<string> Okogu
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Okogu)))
            {
                ((RamAccess<string>)Dictionary[nameof(Okogu)]).Value = Okogu_DB;
                return (RamAccess<string>)Dictionary[nameof(Okogu)];
            }
            else
            {
                var rm = new RamAccess<string>(Okogu_Validation, Okogu_DB);
                rm.PropertyChanged += OkoguValueChanged;
                Dictionary.Add(nameof(Okogu), rm);
                return (RamAccess<string>)Dictionary[nameof(Okogu)];
            }
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
            Okogu_DB = ((RamAccess<string>) Value).Value;
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

        Regex ex = new("^[0-9]{7}$");
        if (!ex.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //Okogu property

    #endregion

    #region Oktmo

    public string Oktmo_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"ОКТМО", "ОКТМО")]
    public RamAccess<string> Oktmo
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Oktmo)))
            {
                ((RamAccess<string>)Dictionary[nameof(Oktmo)]).Value = Oktmo_DB;
                return (RamAccess<string>)Dictionary[nameof(Oktmo)];
            }
            else
            {
                var rm = new RamAccess<string>(Oktmo_Validation, Oktmo_DB);
                rm.PropertyChanged += OktmoValueChanged;
                Dictionary.Add(nameof(Oktmo), rm);
                return (RamAccess<string>)Dictionary[nameof(Oktmo)];
            }
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
            Oktmo_DB = ((RamAccess<string>) Value).Value;
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

        Regex ex = new("^[0-9]{11}$");
        if (!ex.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //Oktmo property

    #endregion

    #region Inn

    public string Inn_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"ИНН", "ИНН")]
    public RamAccess<string> Inn
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Inn)))
            {
                ((RamAccess<string>)Dictionary[nameof(Inn)]).Value = Inn_DB;
                return (RamAccess<string>)Dictionary[nameof(Inn)];
            }
            else
            {
                var rm = new RamAccess<string>(Inn_Validation, Inn_DB);
                rm.PropertyChanged += InnValueChanged;
                Dictionary.Add(nameof(Inn), rm);
                return (RamAccess<string>)Dictionary[nameof(Inn)];
            }
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
            Inn_DB = ((RamAccess<string>) Value).Value;
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

        Regex ex = new("^[0-9]{10}$");
        if (!ex.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //Inn property

    #endregion

    #region Kpp

    public string Kpp_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"КПП", "КПП")]
    public RamAccess<string> Kpp
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Kpp)))
            {
                ((RamAccess<string>)Dictionary[nameof(Kpp)]).Value = Kpp_DB;
                return (RamAccess<string>)Dictionary[nameof(Kpp)];
            }
            else
            {
                var rm = new RamAccess<string>(Kpp_Validation, Kpp_DB);
                rm.PropertyChanged += KppValueChanged;
                Dictionary.Add(nameof(Kpp), rm);
                return (RamAccess<string>)Dictionary[nameof(Kpp)];
            }
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
            Kpp_DB = ((RamAccess<string>) Value).Value;
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

        Regex ex = new("^[0-9]{9}$|-");
        if (!ex.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //Kpp property

    #endregion

    #region Okopf

    public string Okopf_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"ОКОПФ", "ОКОПФ")]
    public RamAccess<string> Okopf
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Okopf)))
            {
                ((RamAccess<string>)Dictionary[nameof(Okopf)]).Value = Okopf_DB;
                return (RamAccess<string>)Dictionary[nameof(Okopf)];
            }
            else
            {
                var rm = new RamAccess<string>(Okopf_Validation, Okopf_DB);
                rm.PropertyChanged += OkopfValueChanged;
                Dictionary.Add(nameof(Okopf), rm);
                return (RamAccess<string>)Dictionary[nameof(Okopf)];
            }
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
            Okopf_DB = ((RamAccess<string>) Value).Value;
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

        Regex ex = new("^[0-9]{5}$");
        if (!ex.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //Okopf property

    #endregion

    #region Okfs

    public string Okfs_DB { get; set; } = "";

    [NotMapped]
    [FormProperty(true,"ОКФС", "ОКФС")]
    public RamAccess<string> Okfs
    {
        get
        {
            if (Dictionary.ContainsKey(nameof(Okfs)))
            {
                ((RamAccess<string>)Dictionary[nameof(Okfs)]).Value = Okfs_DB;
                return (RamAccess<string>)Dictionary[nameof(Okfs)];
            }
            else
            {
                var rm = new RamAccess<string>(Okfs_Validation, Okfs_DB);
                rm.PropertyChanged += OkfsValueChanged;
                Dictionary.Add(nameof(Okfs), rm);
                return (RamAccess<string>)Dictionary[nameof(Okfs)];
            }
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
            Okfs_DB = ((RamAccess<string>) Value).Value;
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

        Regex ex = new("^[0-9]{2}$");
        if (!ex.IsMatch(value.Value))
        {
            value.AddError("Недопустимое значение");
            return false;
        }

        return true;
    }

    //Okfs property

    #endregion

    #region ParseInnerText
    private string ParseInnerText(string Text)
    {
        Text = Text.Replace("\r", " ").Replace("\n", " ").Replace("\t", " ");
        return Text;
    }
    #endregion

    #region IExcel

    public override void ExcelGetRow(ExcelWorksheet worksheet, int row)
    {
        throw new NotImplementedException();
    }

    public override int ExcelRow(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string sumNumber = "")
    {
        if (sumNumber == "")
        {
            worksheet.Cells[row, column].Value = Okpo_DB;
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = ShortJurLico_DB;
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = RegNo_DB;

            return 3;
        }

        worksheet.Cells[row, column].Value = sumNumber;
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value = Okpo_DB;
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value = ShortJurLico_DB;
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value = RegNo_DB;

        return 4;
    }

    public static int ExcelHeader(ExcelWorksheet worksheet, int row, int column, bool transpose = true, string id = "")
    {
        var cnt = Form.ExcelHeader(worksheet, row, column, transpose);
        column += transpose ? cnt : 0;
        row += !transpose ? cnt : 0;

        if (id.Equals(""))
        {
            worksheet.Cells[row, column].Value =
                ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form20,Models")?.GetProperty(nameof(Okpo))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[0];                        
            worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value =
                ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form20,Models")?.GetProperty(nameof(ShortJurLico))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[0];                        
            worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value =
                ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form20,Models")?.GetProperty(nameof(RegNo))
                ?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[0];                        
            
            return 3;                                                                                                 
        }

        worksheet.Cells[row, column].Value = id;                       
        worksheet.Cells[row + (!transpose ? 1 : 0), column + (transpose ? 1 : 0)].Value =
            ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form20,Models")?.GetProperty(nameof(Okpo))
            ?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[0];                        
        worksheet.Cells[row + (!transpose ? 2 : 0), column + (transpose ? 2 : 0)].Value =
            ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form20,Models")?.GetProperty(nameof(ShortJurLico))
            ?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[0];                        
        worksheet.Cells[row + (!transpose ? 3 : 0), column + (transpose ? 3 : 0)].Value =
            ((FormPropertyAttribute) Type.GetType("Models.Forms.Form2.Form20,Models")?.GetProperty(nameof(RegNo))
            ?.GetCustomAttributes(typeof(FormPropertyAttribute), false).First())?.Names[0];
        
        return 4;
    }

    #endregion

    #region IDataGridColumn

    public override DataGridColumns GetColumnStructure(string param)
    {
        return null;
    }

    #endregion
}