using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Форма 5.0: Титульный лист годового отчета СГУК РВ и РАО")]
    public class Form50 : Abstracts.Form
    {
        public Form50() : base()
        {
            //FormNum.Value = "50";
            //NumberOfFields.Value = 11;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }
        public enum Authority
        {
            FederalAuthority,
            CorporationRosatom,
            DepartmentOfDefense,
            None
        }

        //Authority1 property
        [Attributes.Form_Property("ВИАЦ")]public int? Authority1Id { get; set; }
        public virtual RamAccess<Authority> Authority1
        {
            get
            {
                
                {
                    return _dataAccess.Get<Authority>(nameof(Authority1));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Authority1), value);
                }
                OnPropertyChanged(nameof(Authority1));
            }
        }


        //Authority1 property

        //Yyear property
        [Attributes.Form_Property("Год")]public int? YyearId { get; set; }
        public virtual RamAccess<int> Yyear
        {
            get
            {
                
                {
                    return _dataAccess.Get<int>(nameof(Yyear));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(Yyear), value);
                }
                OnPropertyChanged(nameof(Yyear));
            }
        }


        //Yyear property

        //JurLico property
        [Attributes.Form_Property("Юр. лицо")]public int? JurLicoId { get; set; }
        public virtual RamAccess<string> JurLico
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


        //JurLico property

        //ShortJurLico property
        [Attributes.Form_Property("Краткое наименование юр. лица")]public int? ShortJurLicoId { get; set; }
        public virtual RamAccess<string> ShortJurLico
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


        //ShortJurLico property

        //JurLicoAddress property
        [Attributes.Form_Property("Адрес юр. лица")]public int? JurLicoAddressId { get; set; }
        public virtual RamAccess<string> JurLicoAddress
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


        //JurLicoAddress property

        //JurLicoFactAddress property
        [Attributes.Form_Property("Фактический адрес юр. лица")]public int? JurLicoFactAddressId { get; set; }
        public virtual RamAccess<string> JurLicoFactAddress
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


        //JurLicoFactAddress property

        //GradeFIO property
        [Attributes.Form_Property("ФИО, должность")]public int? GradeFIOId { get; set; }
        public virtual RamAccess<string> GradeFIO
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


        //GradeFIO property

        //GradeFIOresponsibleExecutor property
        [Attributes.Form_Property("ФИО, должность ответственного исполнителя")]public int? GradeFIOresponsibleExecutorId { get; set; }
        public virtual RamAccess<string> GradeFIOresponsibleExecutor
        {
            get
            {
                
                {
                    return _dataAccess.Get<string>(nameof(GradeFIOresponsibleExecutor));
                }
                
                {
                    
                }
            }
            set
            {

                
                {
                    _dataAccess.Set(nameof(GradeFIOresponsibleExecutor), value);
                }
                OnPropertyChanged(nameof(GradeFIOresponsibleExecutor));
            }
        }


        //GradeFIOresponsibleExecutor property

        //Telephone property
        [Attributes.Form_Property("Телефон")]public int? TelephoneId { get; set; }
        public virtual RamAccess<string> Telephone
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


        //Telephone property

        //Fax property
        [Attributes.Form_Property("Факс")]public int? FaxId { get; set; }
        public virtual RamAccess<string> Fax
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


        //Fax property

        //Email property
        [Attributes.Form_Property("Эл. почта")]public int? EmailId { get; set; }
        public virtual RamAccess<string> Email
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


        //Email property
    }
}
