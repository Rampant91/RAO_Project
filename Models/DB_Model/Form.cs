using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class Form
    {
        private string _name = "";
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private FormStructure _formStructure = new FormStructure();
        public FormStructure FormStructure
        {
            get
            {
                return _formStructure;
            }
            set
            {
                _formStructure = value;
            }
        }
        public Form() { }
        public Form(string name, int id, FormStructure formStructure)
        {
            Name = name;
            Id = id;
            FormStructure = formStructure;
        }
    }
}
