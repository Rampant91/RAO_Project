using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models
{
    [Serializable]
    [Attributes.Form_Class("Сведения о поставляемых ЗРИ:")]
    public class Form32_2: Abstracts.Form3
    {
        public Form32_2(int RowID) : base(RowID)
        {
            FormNum = "32_2";
            NumberOfFields = 6;
        }

        [Attributes.Form_Property("Форма")]
        public override void Object_Validation()
        {

        }

        //PackName property
        [Attributes.Form_Property("Наименование упаковки")]
        public string PackName
        {
            get
            {
                if (GetErrors(nameof(PackName)) != null)
                {
                    return (string)_PackName.Get();
                }
                else
                {
                    return _PackName_Not_Valid;
                }
            }
            set
            {
                _PackName_Not_Valid = value;
                if (GetErrors(nameof(PackName)) != null)
                {
                    _PackName.Set(_PackName_Not_Valid);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }
        private IDataLoadEngine _PackName;
        private string _PackName_Not_Valid = "";
        private void PackName_Validation()
        {
            ClearErrors(nameof(PackName));
        }
        //PackName property

        //PackType property
        [Attributes.Form_Property("Тип упаковки")]
        public string PackType
        {
            get
            {
                if (GetErrors(nameof(PackType)) != null)
                {
                    return (string)_PackType.Get();
                }
                else
                {
                    return _PackType_Not_Valid;
                }
            }
            set
            {
                _PackType_Not_Valid = value;
                if (GetErrors(nameof(PackType)) != null)
                {
                    _PackType.Set(_PackType_Not_Valid);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        private IDataLoadEngine _PackType;//If change this change validation
        private string _PackType_Not_Valid = "";
        private void PackType_Validation()//Ready
        {
            ClearErrors(nameof(PackType));
        }
        //PackType property

        //PackTypeRecoded property
        public string PackTypeRecoded
        {
            get
            {
                if (GetErrors(nameof(PackTypeRecoded)) != null)
                {
                    return (string)_PackTypeRecoded.Get();
                }
                else
                {
                    return _PackTypeRecoded_Not_Valid;
                }
            }
            set
            {
                _PackTypeRecoded_Not_Valid = value;
                if (GetErrors(nameof(PackTypeRecoded)) != null)
                {
                    _PackTypeRecoded.Set(_PackTypeRecoded_Not_Valid);
                }
                OnPropertyChanged(nameof(PackTypeRecoded));
            }
        }
        private IDataLoadEngine _PackTypeRecoded;
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
                if (GetErrors(nameof(Id)) != null)
                {
                    return (string)_Id.Get();
                }
                else
                {
                    return _Id_Not_Valid;
                }
            }
            set
            {
                _Id_Not_Valid = value;
                if (GetErrors(nameof(Id)) != null)
                {
                    _Id.Set(_Id_Not_Valid);
                }
                OnPropertyChanged(nameof(Id));
            }
        }
        private IDataLoadEngine _Id;
        private string _Id_Not_Valid = "";
        //Id property

        //CreationYear property
        [Attributes.Form_Property("Год изготовления")]
        public int CreationYear
        {
            get
            {
                if (GetErrors(nameof(CreationYear)) != null)
                {
                    return (int)_CreationYear.Get();
                }
                else
                {
                    return _CreationYear_Not_Valid;
                }
            }
            set
            {
                _CreationYear_Not_Valid = value;
                if (GetErrors(nameof(CreationYear)) != null)
                {
                    _CreationYear.Set(_CreationYear_Not_Valid);
                }
                OnPropertyChanged(nameof(CreationYear));
            }
        }
        private IDataLoadEngine _CreationYear;
        private int _CreationYear_Not_Valid = -1;
        //CreationYear property

        //DepletedUraniumMass property
        [Attributes.Form_Property("Масса обедненного урана")]
        public double DepletedUraniumMass
        {
            get
            {
                if (GetErrors(nameof(DepletedUraniumMass)) != null)
                {
                    return (double)_DepletedUraniumMass.Get();
                }
                else
                {
                    return _DepletedUraniumMass_Not_Valid;
                }
            }
            set
            {
                _DepletedUraniumMass_Not_Valid = value;
                if (GetErrors(nameof(DepletedUraniumMass)) != null)
                {
                    _DepletedUraniumMass.Set(_DepletedUraniumMass_Not_Valid);
                }
                OnPropertyChanged(nameof(DepletedUraniumMass));
            }
        }
        private IDataLoadEngine _DepletedUraniumMass;
        private double _DepletedUraniumMass_Not_Valid = -1;
        //DepletedUraniumMass property
    }
}
