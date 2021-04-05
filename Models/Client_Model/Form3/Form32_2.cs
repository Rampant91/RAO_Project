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
        public Form32_2() : base()
        {
        }

        [Attributes.FormVisual("Форма")]
        public override string FormNum { get { return "32_2"; } }
        public override int NumberOfFields { get; } = 6;
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

        private string _packTypeRecoded = "";
        public string PackTypeRecoded
        {
            get { return _packTypeRecoded; }
            set
            {
                _packTypeRecoded = value;
                OnPropertyChanged("PackTypeRecoded");
            }
        }

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
