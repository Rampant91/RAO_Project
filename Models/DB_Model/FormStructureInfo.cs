using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class FormStructureInfo                       //TO DO.
    {
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
        private FormDate _formDate = new FormDate();
        public FormDate FormDate
        {
            get
            {
                return _formDate;
            }
            set
            {
                _formDate = value;
            }
        }
        public FormStructureInfo() { }
        public FormStructureInfo(int id, FormDate formDate)
        {
            Id = id;
            FormDate = formDate;
        }
    }
}
