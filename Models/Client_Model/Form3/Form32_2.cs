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
        public override string FormNum { get { return "3.2_2"; } }
        public override void Object_Validation()
        {

        }
        public override int NumberOfFields { get; } = 6;

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

        private string _id = "";
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private int _creationYear = -1;
        public int CreationYear
        {
            get { return _creationYear; }
            set
            {
                _creationYear = value;
                OnPropertyChanged("CreationYear");
            }
        }

        private double _depletedUraniumMass = -1;
        public double DepletedUraniumMass
        {
            get { return _depletedUraniumMass; }
            set
            {
                _depletedUraniumMass = value;
                OnPropertyChanged("DepletedUraniumMass");
            }
        }
    }
}
