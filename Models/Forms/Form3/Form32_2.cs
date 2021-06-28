using Models.DataAccess;
using System;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Сведения о поставляемых ЗРИ:")]
    public class Form32_2 : Abstracts.Form3
    {
        public Form32_2() : base()
        {
            FormNum.Value = "32_2";
            NumberOfFields.Value = 6;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public RamAccess<string> PackName
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackName));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }
        private bool PackName_Validation(RamAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PackName property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public RamAccess<string> PackType
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackType));//OK

                }

                {

                }
            }
            set
            {



                {
                    DataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation

        private bool PackType_Validation(RamAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError("Поле не заполнено");
                return false;
            }
            return true;
        }
        //PackType property

        //PackTypeRecoded property
        public RamAccess<string> PackTypeRecoded
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(PackTypeRecoded));//OK

                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }
        private bool PackTypeRecoded_Validation(RamAccess<string> value)
        {
            value.ClearErrors(); return true;
        }
        //PackTypeRecoded property

        //Id property
        [Attributes.Form_Property("Идентификационный номер")]
        public RamAccess<string> IdentificationId
        {
            get
            {

                {
                    return DataAccess.Get<string>(nameof(Id));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(Id), value);
                }
                OnPropertyChanged(nameof(Id));
            }
        }
        //Id property

        //CreationYear property
        [Attributes.Form_Property("Год изготовления")]
        public RamAccess<int> CreationYear
        {
            get
            {

                {
                    return DataAccess.Get<int>(nameof(CreationYear));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(CreationYear), value);
                }
                OnPropertyChanged(nameof(CreationYear));
            }
        }
        //CreationYear property

        //DepletedUraniumMass property
        [Attributes.Form_Property("Масса обедненного урана")]
        public RamAccess<double> DepletedUraniumMass
        {
            get
            {

                {
                    return DataAccess.Get<double>(nameof(DepletedUraniumMass));
                }

                {

                }
            }
            set
            {


                {
                    DataAccess.Set(nameof(DepletedUraniumMass), value);
                }
                OnPropertyChanged(nameof(DepletedUraniumMass));
            }
        }
        //DepletedUraniumMass property
    }
}
