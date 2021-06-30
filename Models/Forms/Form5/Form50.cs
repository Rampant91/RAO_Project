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
        public int? Authority1Id { get; set; }
        [Attributes.Form_Property("ВИАЦ")]
        public virtual RamAccess<Authority> Authority1
        {
            get
            {

                {
                    return DataAccess.Get<Authority>(nameof(Authority1));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Authority1), value);
                }
                OnPropertyChanged(nameof(Authority1));
            }
        }


        //Authority1 property

        //Yyear property
        public int? YyearId { get; set; }
        [Attributes.Form_Property("Год")]
        public virtual RamAccess<int> Yyear
        {
            get
            {

                {
                    return DataAccess.Get<int>(nameof(Yyear));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Yyear), value);
                }
                OnPropertyChanged(nameof(Yyear));
            }
        }


        //Yyear property

        //JurLico property
        public int? JurLicoId { get; set; }
        [Attributes.Form_Property("Юр. лицо")]
        public virtual RamAccess<string> JurLico
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(JurLico));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(JurLico), value);
                }
                OnPropertyChanged(nameof(JurLico));
            }
        }


        //JurLico property

        //ShortJurLico property
        public int? ShortJurLicoId { get; set; }
        [Attributes.Form_Property("Краткое наименование юр. лица")]
        public virtual RamAccess<string> ShortJurLico
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(ShortJurLico));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(ShortJurLico), value);
                }
                OnPropertyChanged(nameof(ShortJurLico));
            }
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


        //JurLicoFactAddress property

        //GradeFIO property
        public int? GradeFIOId { get; set; }
        [Attributes.Form_Property("ФИО, должность")]
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


        //GradeFIO property

        //GradeFIOresponsibleExecutor property
        public int? GradeFIOresponsibleExecutorId { get; set; }
        [Attributes.Form_Property("ФИО, должность ответственного исполнителя")]
        public virtual RamAccess<string> GradeFIOresponsibleExecutor
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(GradeFIOresponsibleExecutor));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(GradeFIOresponsibleExecutor), value);
                }
                OnPropertyChanged(nameof(GradeFIOresponsibleExecutor));
            }
        }


        //GradeFIOresponsibleExecutor property

        //Telephone property
        public int? TelephoneId { get; set; }
        [Attributes.Form_Property("Телефон")]
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


        //Telephone property

        //Fax property
        public int? FaxId { get; set; }
        [Attributes.Form_Property("Факс")]
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


        //Fax property

        //Email property
        public int? EmailId { get; set; }
        [Attributes.Form_Property("Эл. почта")]
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


        //Email property
    }
}
