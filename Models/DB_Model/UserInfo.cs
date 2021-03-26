using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class UserInfo
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
        private string _surName = "";
        public string SurName
        {
            get
            {
                return _surName;
            }
            set
            {
                _surName = value;
            }
        }
        private string _secondName = "";
        public string SecondName
        {
            get
            {
                return _secondName;
            }
            set
            {
                _secondName = value;
            }
        }
        public UserInfo() { }
        public UserInfo(int id, string name, string surName, string secondName)
        {
            Id = id;
            Name = name;
            SurName = surName;
            SecondName = secondName;
        }
    }
}
