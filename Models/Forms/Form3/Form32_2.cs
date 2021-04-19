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
        public static string SQLCommandParams()
        {
            string strNotNullDeclaration = " varchar(255) not null, ";
            string intNotNullDeclaration = " int not null, ";
            string shortNotNullDeclaration = " smallint not null, ";
            string byteNotNullDeclaration = " tinyint not null, ";
            string dateNotNullDeclaration = " ????, ";
            string doubleNotNullDeclaration = " float(53) not null, ";
            return
                Abstracts.Form3.SQLCommandParamsBase() +
            nameof(DepletedUraniumMass) + doubleNotNullDeclaration +
            nameof(CreationYear) + strNotNullDeclaration +
            nameof(Id) + strNotNullDeclaration +
            nameof(PackName) + strNotNullDeclaration +
            nameof(PackType) + strNotNullDeclaration +
            nameof(PackTypeRecoded) + " varchar(255) not null";
        }
        public Form32_2(IDataAccess Access) : base(Access)
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
                    return (string)_dataAccess.Get(nameof(PackName));
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
                    _dataAccess.Set(nameof(PackName), _PackName_Not_Valid);
                }
                OnPropertyChanged(nameof(PackName));
            }
        }
        
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
                    return (string)_dataAccess.Get(nameof(PackType));
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
                    _dataAccess.Set(nameof(PackType), _PackType_Not_Valid);
                }
                OnPropertyChanged(nameof(PackType));
            }
        }
        //If change this change validation
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
                    return (string)_dataAccess.Get(nameof(PackTypeRecoded));
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
                    _dataAccess.Set(nameof(PackTypeRecoded), _PackTypeRecoded_Not_Valid);
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
                if (GetErrors(nameof(Id)) != null)
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
                if (GetErrors(nameof(Id)) != null)
                {
                    _dataAccess.Set(nameof(Id), _Id_Not_Valid);
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
                if (GetErrors(nameof(CreationYear)) != null)
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
                if (GetErrors(nameof(CreationYear)) != null)
                {
                    _dataAccess.Set(nameof(CreationYear), _CreationYear_Not_Valid);
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
                if (GetErrors(nameof(DepletedUraniumMass)) != null)
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
                if (GetErrors(nameof(DepletedUraniumMass)) != null)
                {
                    _dataAccess.Set(nameof(DepletedUraniumMass), _DepletedUraniumMass_Not_Valid);
                }
                OnPropertyChanged(nameof(DepletedUraniumMass));
            }
        }
        
        private double _DepletedUraniumMass_Not_Valid = -1;
        //DepletedUraniumMass property
    }
}
