using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DB_Model
{
    class Token
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
        private DateTime _dateTime = DateTime.MinValue;
        public DateTime DateTime
        {
            get
            {
                return _dateTime;
            }
            set
            {
                _dateTime = value;
            }
        }
        private int _hash;
        public int Hash
        {
            get
            {
                return _hash;
            }
            set
            {
                _hash = value;
            }
        }
        public Token() { }
        public Token(int id, DateTime dateTime, int hash)
        {
            Id = id;
            DateTime = dateTime;
            Hash = hash;
        }
    }
}
