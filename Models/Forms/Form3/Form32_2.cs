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
            FormNum = "32_2";
            NumberOfFields = 6;
        }

        [Attributes.Form_Property("Форма")]
        public override bool Object_Validation()
        {
            return false;
        }

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public IDataAccess<string> PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackName));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {
                PackName_Validation(value);

                if (GetErrors(nameof(PackName)) == null)
                {
                    _dataAccess.Set(nameof(PackName), value);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }


        private void PackName_Validation(IDataAccess<string> value)
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }
        //PackName property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public IDataAccess<string> PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackType));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {
                PackType_Validation(value);

                if (GetErrors(nameof(PackType)) == null)
                {
                    _dataAccess.Set(nameof(PackType), value);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation

        private void PackType_Validation(IDataAccess<string> value)//Ready
        {
            value.ClearErrors();
            if ((value.Value == null))
            {
                value.AddError( "Поле не заполнено");
                return;
            }
        }
        //PackType property

        //PackTypeRecoded property
        public IDataAccess<string> PackTypeRecoded
        {
            get
            {
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    var tmp = _dataAccess.Get<string>(nameof(PackTypeRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }


        private void PackTypeRecoded_Validation()
        {
            value.ClearErrors();
        }
        //PackTypeRecoded property

        //Id property
        [Attributes.Form_Property("Идентификационный номер")]
        public IDataAccess<string> Id
        {
            get
            {
                if (GetErrors(nameof(Id)) == null)
                {
                    return _dataAccess.Get<string>(nameof(Id));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(Id)) == null)
                {
                    _dataAccess.Set(nameof(Id), value);
                }
                OnPropertyChanged(nameof(Id));
            }
        }


        //Id property

        //CreationYear property
        [Attributes.Form_Property("Год изготовления")]
        public int CreationYear
        {
            get
            {
                if (GetErrors(nameof(CreationYear)) == null)
                {
                    return _dataAccess.Get<string>(nameof(CreationYear));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(CreationYear)) == null)
                {
                    _dataAccess.Set(nameof(CreationYear), value);
                }
                OnPropertyChanged(nameof(CreationYear));
            }
        }


        //CreationYear property

        //DepletedUraniumMass property
        [Attributes.Form_Property("Масса обедненного урана")]
        public double DepletedUraniumMass
        {
            get
            {
                if (GetErrors(nameof(DepletedUraniumMass)) == null)
                {
                    return _dataAccess.Get<string>(nameof(DepletedUraniumMass));
                }
                else
                {
                    
                }
            }
            set
            {

                if (GetErrors(nameof(DepletedUraniumMass)) == null)
                {
                    _dataAccess.Set(nameof(DepletedUraniumMass), value);
                }
                OnPropertyChanged(nameof(DepletedUraniumMass));
            }
        }


        //DepletedUraniumMass property
    }
}
