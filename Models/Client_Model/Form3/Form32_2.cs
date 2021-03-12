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
        public override void Object_Validation()
        {

        }
        public int NumberOfFields { get; } = 6;

        private string _packName = "";

        [Attributes.FormVisual("Наименование упаковки")]
        public string PackName
        {
            get { return _packName; }
            set
            {
                _packName = value;
                OnPropertyChanged("PackName");
            }
        }

        private string _packType = "";//If change this change validation
        private void PackType_Validation(string value)//Ready
        {
            ClearErrors(nameof(PackType));
        }

        [Attributes.FormVisual("Тип упаковки")]
        public string PackType
        {
            get { return _packType; }
            set
            {
                _packType = value;
                PackType_Validation(value);
                OnPropertyChanged("PackType");
            }
        }

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
