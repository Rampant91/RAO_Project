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
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackName));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackName_Not_Valid;
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

        private string _PackName_Not_Valid = "";
        private void PackName_Validation(string value)
        {
            ClearErrors(nameof(PackName));
            if ((value == null) || value.Equals(_PackName_Not_Valid))
            {
                AddError(nameof(PackName), "Поле не заполнено");
                return;
            }
        }
        //PackName property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public string PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackType));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackType_Not_Valid;
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
        private string _PackType_Not_Valid = "";
        private void PackType_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackType));
            if ((value == null) || value.Equals(_PackType_Not_Valid))
            {
                AddError(nameof(PackType), "Поле не заполнено");
                return;
            }
        }
        //PackType property

        //PackTypeRecoded property
        public string PackTypeRecoded
        {
            get
            {
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    var tmp = _dataAccess.Get(nameof(PackTypeRecoded));//OK
                    return tmp != null ? (string)tmp : null;
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                _PackTypeRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackTypeRecoded)) == null)
                {
                    _dataAccess.Set(nameof(PackTypeRecoded), value);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }

        private string _PackTypeRecoded_Not_Valid = "";
        private void PackTypeRecoded_Validation()
        {
            ClearErrors(nameof(PackTypeRecoded));
        }
        //PackTypeRecoded property

        //Id property
        [Attributes.Form_Property("Идентификационный номер")]
        public string Id
        {
            get
            {
                if (GetErrors(nameof(Id)) == null)
                {
                    return (string)_dataAccess.Get(nameof(Id));
                }
                else
                {
                    return _Id_Not_Valid;
                }
            }
            set
            {
                _Id_Not_Valid = value;
                if (GetErrors(nameof(Id)) == null)
                {
                    _dataAccess.Set(nameof(Id), value);
                }
                OnPropertyChanged(nameof(Id));
            }
        }

        private string _Id_Not_Valid = "";
        //Id property

        //CreationYear property
        [Attributes.Form_Property("Год изготовления")]
        public int CreationYear
        {
            get
            {
                if (GetErrors(nameof(CreationYear)) == null)
                {
                    return (int)_dataAccess.Get(nameof(CreationYear));
                }
                else
                {
                    return _CreationYear_Not_Valid;
                }
            }
            set
            {
                _CreationYear_Not_Valid = value;
                if (GetErrors(nameof(CreationYear)) == null)
                {
                    _dataAccess.Set(nameof(CreationYear), value);
                }
                OnPropertyChanged(nameof(CreationYear));
            }
        }

        private int _CreationYear_Not_Valid = -1;
        //CreationYear property

        //DepletedUraniumMass property
        [Attributes.Form_Property("Масса обедненного урана")]
        public double DepletedUraniumMass
        {
            get
            {
                if (GetErrors(nameof(DepletedUraniumMass)) == null)
                {
                    return (double)_dataAccess.Get(nameof(DepletedUraniumMass));
                }
                else
                {
                    return _DepletedUraniumMass_Not_Valid;
                }
            }
            set
            {
                _DepletedUraniumMass_Not_Valid = value;
                if (GetErrors(nameof(DepletedUraniumMass)) == null)
                {
                    _dataAccess.Set(nameof(DepletedUraniumMass), value);
                }
                OnPropertyChanged(nameof(DepletedUraniumMass));
            }
        }

        private double _DepletedUraniumMass_Not_Valid = -1;
        //DepletedUraniumMass property
    }
}
