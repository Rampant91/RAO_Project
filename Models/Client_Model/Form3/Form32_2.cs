using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Models.Client_Model
{
    [Serializable]
    [Attributes.FormVisual_Class("Сведения о поставляемых ЗРИ:")]
    public class Form32_2: Form3
    {
        public Form32_2(bool isSQL) : base()
        {
            FormNum = "32_2";
            NumberOfFields = 6;
            if (isSQL)
            {
                _DepletedUraniumMass = new SQLite("DepletedUraniumMass", FormNum, 0);
                _CreationYear = new SQLite("CreationYear", FormNum, 0);
                _Id = new SQLite("Id", FormNum, 0);
                _PackName = new SQLite("PackName", FormNum, 0);
                _PackType = new SQLite("PackType", FormNum, 0);
                _PackTypeRecoded = new SQLite("PackTypeRecoded", FormNum, 0);
            }
            else
            {
                _DepletedUraniumMass = new File();
                _CreationYear = new File();
                _Id = new File();
                _PackName = new File();
                _PackType = new File();
                _PackTypeRecoded = new File();
            }
        }

        [Attributes.FormVisual("Форма")]
        public override void Object_Validation()
        {

        }

        //PackName property
        [Attributes.FormVisual("Наименование упаковки")]
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
        [Attributes.FormVisual("Тип упаковки")]
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
        [Attributes.FormVisual("Идентификационный номер")]
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
        [Attributes.FormVisual("Год изготовления")]
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
        [Attributes.FormVisual("Масса обедненного урана")]
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
