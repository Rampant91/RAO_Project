using Models.DataAccess;
using System;
using System.Text.RegularExpressions;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 1.0: Титульный лист оперативного отчета организации")]
    public class Form10 : Abstracts.Form
    {
        public Form10() : base()
        {
            NumberOfFields = 19;
            FormNum = "10";
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //RegNo property
        [Attributes.Form_Property("Рег. №")]
        public IDataAccess<string> RegNo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(RegNo));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(RegNo), _RegNo_Not_Valid);
                }
                OnPropertyChanged(nameof(RegNo));
            }
        }

                //RegNo property

        //OrganUprav property
        [Attributes.Form_Property("Орган управления")]
        public IDataAccess<string> OrganUprav
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(OrganUprav));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(OrganUprav), _OrganUprav_Not_Valid);
                }
                OnPropertyChanged(nameof(OrganUprav));
            }
        }

                //OrganUprav property

        //SubjectRF property
        [Attributes.Form_Property("Субъект РФ")]
        public IDataAccess<string> SubjectRF
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(SubjectRF));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(SubjectRF), _SubjectRF_Not_Valid);
                }
                OnPropertyChanged(nameof(SubjectRF));
            }
        }

                //SubjectRF property

        //JurLico property
        [Attributes.Form_Property("Юр. лицо")]
        public IDataAccess<string> JurLico
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(JurLico));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(JurLico), _JurLico_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLico));
            }
        }

                //JurLico property

        //ShortJurLico property
        [Attributes.Form_Property("Краткое наименование юр. лица")]
        public IDataAccess<string> ShortJurLico
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(ShortJurLico));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(ShortJurLico), _ShortJurLico_Not_Valid);
                }
                OnPropertyChanged(nameof(ShortJurLico));
            }
        }

                //ShortJurLico property

        //JurLicoAddress property
        [Attributes.Form_Property("Адрес юр. лица")]
        public IDataAccess<string> JurLicoAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(JurLicoAddress));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(JurLicoAddress), _JurLicoAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLicoAddress));
            }
        }

                //JurLicoAddress property

        //JurLicoFactAddress property
        [Attributes.Form_Property("Фактический адрес юр. лица")]
        public IDataAccess<string> JurLicoFactAddress
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(JurLicoFactAddress));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(JurLicoFactAddress), _JurLicoFactAddress_Not_Valid);
                }
                OnPropertyChanged(nameof(JurLicoFactAddress));
            }
        }

                //JurLicoFactAddress property

        //GradeFIO property
        [Attributes.Form_Property("ФИО, должность")]
        public IDataAccess<string> GradeFIO
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(GradeFIO));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(GradeFIO), _GradeFIO_Not_Valid);
                }
                OnPropertyChanged(nameof(GradeFIO));
            }
        }

                //GradeFIO property

        //Telephone property
        [Attributes.Form_Property("Телефон")]
        public IDataAccess<string> Telephone
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Telephone));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Telephone), _Telephone_Not_Valid);
                }
                OnPropertyChanged(nameof(Telephone));
            }
        }

                //Telephone property

        //Fax property
        [Attributes.Form_Property("Факс")]
        public IDataAccess<string> Fax
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Fax));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Fax), _Fax_Not_Valid);
                }
                OnPropertyChanged(nameof(Fax));
            }
        }

                //Fax property

        //Email property
        [Attributes.Form_Property("Эл. почта")]
        public IDataAccess<string> Email
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Email));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Email), _Email_Not_Valid);
                }
                OnPropertyChanged(nameof(Email));
            }
        }

                //Email property

        //Okpo property
        [Attributes.Form_Property("ОКПО")]
        public IDataAccess<string> Okpo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okpo));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okpo), _Okpo_Not_Valid);
                }
                OnPropertyChanged(nameof(Okpo));
            }
        }
        
        private void Okpo_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Length != 8) && (value.Length != 14))
                value.AddError( "Недопустимое значение");
            else
            {
                var mask = new Regex("^[0123456789]{8}([0123456789_][0123456789]{5}){0,1}$");
                if (!mask.IsMatch(value.Value))
                    value.AddError( "Недопустимое значение");
            }
        }
        //Okpo property

        //Okved property
        [Attributes.Form_Property("ОКВЭД")]
        public IDataAccess<string> Okved
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okved));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okved), _Okved_Not_Valid);
                }
                OnPropertyChanged(nameof(Okved));
            }
        }

                private void Okved_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0123456789]{2}\\.[0123456789]{2}\\.[0123456789]{2}");
            if (!ex.IsMatch(value.Value))
                value.AddError( "Недопустимое значение");
        }
        //Okved property

        //Okogu property
        [Attributes.Form_Property("ОКОГУ")]
        public IDataAccess<string> Okogu
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okogu));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okogu), _Okogu_Not_Valid);
                }
                OnPropertyChanged(nameof(Okogu));
            }
        }

                private void Okogu_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0123456789]{5}");
            if (!ex.IsMatch(value.Value))
                value.AddError( "Недопустимое значение");
        }
        //Okogu property

        //Oktmo property
        [Attributes.Form_Property("ОКТМО")]
        public IDataAccess<string> Oktmo
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Oktmo));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Oktmo), _Oktmo_Not_Valid);
                }
                OnPropertyChanged(nameof(Oktmo));
            }
        }

                private void Oktmo_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0123456789]{11}");
            if (!ex.IsMatch(value.Value))
                value.AddError( "Недопустимое значение");
        }
        //Oktmo property

        //Inn property
        [Attributes.Form_Property("ИНН")]
        public IDataAccess<string> Inn
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Inn));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Inn), _Inn_Not_Valid);
                }
                OnPropertyChanged(nameof(Inn));
            }
        }

                private void Inn_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0123456789]{10}");
            if (!ex.IsMatch(value.Value))
                value.AddError( "Недопустимое значение");
        }
        //Inn property

        //Kpp property
        [Attributes.Form_Property("КПП")]
        public IDataAccess<string> Kpp
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Kpp));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Kpp), _Kpp_Not_Valid);
                }
                OnPropertyChanged(nameof(Kpp));
            }
        }

                private void Kpp_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0123456789]{10}");
            if (!ex.IsMatch(value.Value))
                value.AddError( "Недопустимое значение");
        }
        //Kpp property

        //Okopf property
        [Attributes.Form_Property("ОКОПФ")]
        public IDataAccess<string> Okopf
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okopf));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okopf), _Okopf_Not_Valid);
                }
                OnPropertyChanged(nameof(Okopf));
            }
        }

                private void Okopf_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0123456789]{5}");
            if (!ex.IsMatch(value.Value))
                value.AddError( "Недопустимое значение");
        }
        //Okopf property

        //Okfs property
        [Attributes.Form_Property("ОКФС")]
        public IDataAccess<string> Okfs
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(Okfs));
                }
                else
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Okfs), _Okfs_Not_Valid);
                }
                OnPropertyChanged(nameof(Okfs));
            }
        }

                private void Okfs_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            var ex = new Regex("[0123456789]{2}");
            if (!ex.IsMatch(value.Value))
                value.AddError( "Недопустимое значение");
        }
        //Okfs property
    }
}
