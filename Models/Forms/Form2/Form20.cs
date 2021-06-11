using Models.DataAccess;
using System;
using System.Text.RegularExpressions;
using System.ComponentModel;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 2.0: Титульный лист годового отчета организации")]
    public class Form20 : Abstracts.Form
    {
        public Form20() : base()
        {
            FormNum.Value = "20";
            NumberOfFields.Value = 19;
            Init_base();
            Validate_base();
        }
        protected void InPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            OnPropertyChanged(args.PropertyName);
        }

        private void Init_base()
        {
            _dataAccess.Init<string>(nameof(Okpo), Okpo_Validation, null);
            Okpo.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Okved), Okved_Validation, null);
            Okved.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Oktmo), Oktmo_Validation, null);
            Oktmo.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Okogu), Okogu_Validation, null);
            Oktmo.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Okopf), Okopf_Validation, null);
            Okopf.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Inn), Inn_Validation, null);
            Inn.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Kpp), Kpp_Validation, null);
            Kpp.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(RegNo), RegNo_Validation, null);
            RegNo.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(OrganUprav), OrganUprav_Validation, null);
            OrganUprav.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(SubjectRF), SubjectRF_Validation, null);
            SubjectRF.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(JurLico), JurLico_Validation, null);
            OrganUprav.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(ShortJurLico), ShortJurLico_Validation, null);
            ShortJurLico.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(JurLicoAddress), JurLicoAddress_Validation, null);
            JurLicoAddress.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(JurLicoFactAddress), JurLicoFactAddress_Validation, null);
            JurLicoFactAddress.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(GradeFIO), GradeFIO_Validation, null);
            GradeFIO.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Telephone), Telephone_Validation, null);
            Telephone.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Fax), Fax_Validation, null);
            Fax.PropertyChanged += InPropertyChanged;
            _dataAccess.Init<string>(nameof(Email), Email_Validation, null);
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
            Kpp_Validation(RegNo);
            Kpp_Validation(OrganUprav);
            Kpp_Validation(SubjectRF);
            Kpp_Validation(JurLico);
            Kpp_Validation(ShortJurLico);
            Kpp_Validation(JurLicoAddress);
            Kpp_Validation(JurLicoFactAddress);
            Kpp_Validation(GradeFIO);
            Kpp_Validation(Telephone);
            Kpp_Validation(Fax);
            Kpp_Validation(Email);
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //RegNo property
        [Attributes.Form_Property("Рег. №")]
        public RamAccess<string> RegNo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RegNo));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RegNo), value);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }
        private bool RegNo_Validation(RamAccess<string> value)
        {
            return true;
        }
        //RegNo property

        //OrganUprav property
        [Attributes.Form_Property("Орган управления")]
        public RamAccess<string> OrganUprav
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(OrganUprav));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(OrganUprav), value);
                }
                OnPropertyChanged(nameof(OrganUprav));
            }
        }
        private bool OrganUprav_Validation(RamAccess<string> value)
        {
            return true;
        }
        //OrganUprav property

        //SubjectRF property
        [Attributes.Form_Property("Субъект РФ")]
        public RamAccess<string> SubjectRF
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SubjectRF));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SubjectRF), value);
                }
                OnPropertyChanged(nameof(SubjectRF));
            }
        }
        private bool SubjectRF_Validation(RamAccess<string> value)
        {
            return true;
        }
        //SubjectRF property

        //JurLico property
        [Attributes.Form_Property("Юр. лицо")]
        public RamAccess<string> JurLico
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(JurLico));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(JurLico), value);
                }
                OnPropertyChanged(nameof(JurLico));
            }
        }
        private bool JurLico_Validation(RamAccess<string> value)
        {
            return true;
        }
        //JurLico property

        //ShortJurLico property
        [Attributes.Form_Property("Краткое наименование юр. лица")]
        public RamAccess<string> ShortJurLico
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ShortJurLico));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ShortJurLico), value);
                }
                OnPropertyChanged(nameof(ShortJurLico));
            }
        }
        private bool ShortJurLico_Validation(RamAccess<string> value)
        {
            return true;
        }
        //ShortJurLico property

        //JurLicoAddress property
        [Attributes.Form_Property("Адрес юр. лица")]
        public RamAccess<string> JurLicoAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(JurLicoAddress));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(JurLicoAddress), value);
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
        [Attributes.Form_Property("Фактический адрес юр. лица")]
        public RamAccess<string> JurLicoFactAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(JurLicoFactAddress));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(JurLicoFactAddress), value);
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
        [Attributes.Form_Property("ФИО, должность")]
        public RamAccess<string> GradeFIO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(GradeFIO));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(GradeFIO), value);
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
        [Attributes.Form_Property("Телефон")]
        public RamAccess<string> Telephone
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Telephone));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Telephone), value);
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
        [Attributes.Form_Property("Факс")]
        public RamAccess<string> Fax
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Fax));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Fax), value);
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
        [Attributes.Form_Property("Эл. почта")]
        public RamAccess<string> Email
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Email));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Email), value);
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
        [Attributes.Form_Property("ОКПО")]
        public RamAccess<string> Okpo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okpo));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okpo), value);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        private bool Okpo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value.Length != 8) && (value.Value.Length != 14))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
            if (!mask.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Okpo property

        //Okved property
        [Attributes.Form_Property("ОКВЭД")]
        public RamAccess<string> Okved
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okved));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okved), value);
                }
                OnPropertyChanged(nameof(Okved));
            }
        }

                private bool Okved_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("^[0-9]{2}\\.[0-9]{2}\\.[0-9]{2}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение"); return false;
            }
            return true;
        }
        //Okved property

        //Okogu property
        [Attributes.Form_Property("ОКОГУ")]
        public RamAccess<string> Okogu
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okogu));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okogu), value);
                }
                OnPropertyChanged(nameof(Okogu));
            }
        }

                private bool Okogu_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("^[0-9]{5}$");
            if (!ex.IsMatch(value.Value)){
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //Okogu property

        //Oktmo property
        [Attributes.Form_Property("ОКТМО")]
        public RamAccess<string> Oktmo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Oktmo));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Oktmo), value);
                }
                OnPropertyChanged(nameof(Oktmo));
            }
        }

        private bool Oktmo_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("^[0-9]{11}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            return true;
        }
        //Oktmo property

        //Inn property
        [Attributes.Form_Property("ИНН")]
        public RamAccess<string> Inn
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Inn));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Inn), value);
                }
                OnPropertyChanged(nameof(Inn));
            }
        }

                private bool Inn_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0-9]{10}");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //Inn property

        //Kpp property
        [Attributes.Form_Property("КПП")]
        public RamAccess<string> Kpp
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Kpp));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Kpp), value);
                }
                OnPropertyChanged(nameof(Kpp));
            }
        }

                private bool Kpp_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0-9]{9}");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение");
                return false;
            }
            return true;
        }
        //Kpp property

        //Okopf property
        [Attributes.Form_Property("ОКОПФ")]
        public RamAccess<string> Okopf
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okopf));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okopf), value);
                }
                OnPropertyChanged(nameof(Okopf));
            }
        }

        private bool Okopf_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0-9]{5}");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError("Недопустимое значение");
                return false;
            }
            return true;
        }
        //Okopf property

        //Okfs property
        [Attributes.Form_Property("ОКФС")]
        public RamAccess<string> Okfs
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okfs));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okfs), value);
                }
                OnPropertyChanged(nameof(Okfs));
            }
        }

                private bool Okfs_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("^[0-9]{2}$");
            if (!ex.IsMatch(value.Value))
            {
                value.AddError( "Недопустимое значение"); return false;
            }
            return true;
        }
        //Okfs property
    }
}
