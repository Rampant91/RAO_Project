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
        [Attributes.Form_Property("Год")]public int? YyearId { get; set; }
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
        [Attributes.Form_Property("Юр. лицо")]public int? JurLicoId { get; set; }
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
        [Attributes.Form_Property("Краткое наименование юр. лица")]public int? ShortJurLicoId { get; set; }
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
        [Attributes.Form_Property("Адрес юр. лица")]public int? JurLicoAddressId { get; set; }
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
        [Attributes.Form_Property("Фактический адрес юр. лица")]public int? JurLicoFactAddressId { get; set; }
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


        //GradeFIO property

        //GradeFIOresponsibleExecutor property
        [Attributes.Form_Property("ФИО, должность ответственного исполнителя")]public int? GradeFIOresponsibleExecutorId { get; set; }
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


        //Email property
    }
}
