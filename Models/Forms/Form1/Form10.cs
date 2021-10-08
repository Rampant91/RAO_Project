using Models.DataAccess;
using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Models.Abstracts;
using Models.Attributes;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.0: Титульный лист оперативного отчета организации")]
    public class Form10 : Abstracts.Form
    {
        public Form10() : base()
        {
            FormNum.Value = "1.0";
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
                RegNo_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(RegNo));
            }
        }

        private bool RegNo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                return true;
            }

            if (value.Value.Length != 5)
            {
                value.AddError("Недопустимое значение");
                return false;
            }

            Regex mask = new Regex("^[0123456789]{5}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }

            return true;
        }

        #endregion

        #region OrganUprav

        public string OrganUprav_DB { get; set; } = "";

        [NotMapped]
        [Attributes.Form_Property("Орган управления использованием атомной энергии")]
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
                OrganUprav_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(OrganUprav));
            }
        }

        private bool OrganUprav_Validation(RamAccess<string> value) //Ready
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
                SubjectRF_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(SubjectRF));
            }
        }

        private bool SubjectRF_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region JurLico

        public string JurLico_DB { get; set; } = "";

        [NotMapped]
        [Attributes.Form_Property("Наименование юр. лица")]
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
                JurLico_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(JurLico));
            }
        }

        private bool JurLico_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region ShortJurLico

        public string ShortJurLico_DB { get; set; } = "";

        [NotMapped]
        [Attributes.Form_Property("Сокращенное наименование юр. лица")]
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
                ShortJurLico_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(ShortJurLico));
            }
        }

        private bool ShortJurLico_Validation(RamAccess<string> value) //Ready
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
                JurLicoAddress_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(JurLicoAddress));
            }
        }

        private bool JurLicoAddress_Validation(RamAccess<string> value) //Ready
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
                JurLicoFactAddress_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(JurLicoFactAddress));
            }
        }

        private bool JurLicoFactAddress_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region GradeFIO

        public string GradeFIO_DB { get; set; } = "";

        [NotMapped]
        [Attributes.Form_Property("ФИО, должность руководителя")]
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
                GradeFIO_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(GradeFIO));
            }
        }

        private bool GradeFIO_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Telephone

        public string Telephone_DB { get; set; } = "";

        [NotMapped]
        [Attributes.Form_Property("Телефон руководителя")]
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
                Telephone_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Telephone));
            }
        }

        private bool Telephone_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Fax

        public string Fax_DB { get; set; } = "";

        [NotMapped]
        [Attributes.Form_Property("Факс руководителя")]
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
                Fax_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Fax));
            }
        }

        private bool Fax_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            return true;
        }

        #endregion

        #region Email

        public string Email_DB { get; set; } = "";

        [NotMapped]
        [Attributes.Form_Property("Эл. почта руководителя")]
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
                Email_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Email));
            }
        }

        private bool Email_Validation(RamAccess<string> value) //Ready
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
                Okpo_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Okpo));
            }
        }

        private bool Okpo_Validation(RamAccess<string> value) //Ready
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
                Okved_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Okved));
            }
        }

        private bool Okved_Validation(RamAccess<string> value) //Ready
        {
            value.ClearErrors();
            if (string.IsNullOrEmpty(value.Value))
            {
                value.AddError("Поле не заполнено");
                return false;
            }

            Regex ex = new Regex(@"^[0-9]{2}(|\.[0-9]{1,2})(|\.[0-9]{1,2})$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
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
                Okogu_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Okogu));
            }
        }

        private bool Okogu_Validation(RamAccess<string> value) //Ready
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
                value.AddError("Недопустимое значение");
                return false;
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
                Oktmo_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Oktmo));
            }
        }

        private bool Oktmo_Validation(RamAccess<string> value) //Ready
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

        #endregion

        #region Inn

        public string Inn_DB { get; set; } = "";

        [NotMapped]
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
                Inn_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Inn));
            }
        }

        private bool Inn_Validation(RamAccess<string> value) //Ready
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
                value.AddError("Недопустимое значение");
                return false;
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
                Kpp_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Kpp));
            }
        }

        private bool Kpp_Validation(RamAccess<string> value) //Ready
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

        #endregion

        #region Okopf

        public string Okopf_DB { get; set; } = "";

        [NotMapped]
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
                Okopf_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Okopf));
            }
        }

        private bool Okopf_Validation(RamAccess<string> value) //Ready
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

        #endregion

        #region Okfs

        public string Okfs_DB { get; set; } = "";

        [NotMapped]
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
                Okfs_DB = ((RamAccess<string>) Value).Value;
                OnPropertyChanged(nameof(Okfs));
            }
        }

        private bool Okfs_Validation(RamAccess<string> value) //Ready
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
                value.AddError("Недопустимое значение");
                return false;
            }

            return true;
        }

        #endregion

        #region IExcel

        public void ExcelRow(ExcelWorksheet worksheet, int Row)
        {
            base.ExcelRow(worksheet, Row);

            worksheet.Cells[2, Row].Value = SubjectRF_DB;
            worksheet.Cells[3, Row].Value = JurLico_DB;
            worksheet.Cells[4, Row].Value = ShortJurLico_DB;
            worksheet.Cells[5, Row].Value = JurLicoAddress_DB;
            worksheet.Cells[6, Row].Value = JurLicoFactAddress_DB;
            worksheet.Cells[7, Row].Value = GradeFIO_DB;
            worksheet.Cells[8, Row].Value = Telephone_DB;
            worksheet.Cells[9, Row].Value = Fax_DB;
            worksheet.Cells[10, Row].Value = Email_DB;
            worksheet.Cells[11, Row].Value = Okpo_DB;
            worksheet.Cells[12, Row].Value = Okved_DB;
            worksheet.Cells[13, Row].Value = Okogu_DB;
            worksheet.Cells[14, Row].Value = Oktmo_DB;
            worksheet.Cells[15, Row].Value = Inn_DB;
            worksheet.Cells[16, Row].Value = Kpp_DB;
            worksheet.Cells[17, Row].Value = Okopf_DB;
            worksheet.Cells[18, Row].Value = Okfs_DB;
        }

        public static void ExcelHeader(ExcelWorksheet worksheet,int Column)
        {
            Form.ExcelHeader(worksheet);
            if (Column == 1)
            {
                worksheet.Cells[1, Column].Value = "Юридическое лицо";
            }
            else
            {
                worksheet.Cells[1, Column].Value = "Обособленное подраздеение";
            }

            worksheet.Cells[2, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(SubjectRF))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[3, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(JurLico))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[4, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(ShortJurLico))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[5, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(JurLicoAddress))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[6, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(JurLicoFactAddress))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[7, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(GradeFIO))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[8, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Telephone))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[9, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Fax))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[10, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Email))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[11, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Okpo))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[12, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Okved))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[13, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Okogu))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[14, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Oktmo))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[15, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Inn))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[16, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Kpp))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[17, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Okopf))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
            worksheet.Cells[18, Column].Value = ((Form_PropertyAttribute)System.Type.GetType("Models.Form10,Models").GetProperty(nameof(Okfs))
                .GetCustomAttributes(typeof(Form_PropertyAttribute), false).First()).Name;
        }

        #endregion
    }
}
